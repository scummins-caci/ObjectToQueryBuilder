﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ObjectToQueryBuilder
{
    public class QueryTranslator : ExpressionVisitor
    {
        private StringBuilder sb;
        private string _orderBy = string.Empty;
        private int? _skip = null;
        private int? _take = null;
        private string _whereClause = string.Empty;

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            _whereClause = this.sb.ToString();
            return _whereClause;
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.Visit(m.Arguments[0]);
                var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            
            switch (m.Method.Name)
            {
                case "StartsWith":
                    if (ParseStartsWithExpression(m))
                    {
                        return null;
                    }
                    break;

                case "EndsWith":
                    if (ParseEndsWithExpression(m))
                    {
                        return null;
                    }
                    break;

                case "Contains":
                    if (ParseContainsExpression(m))
                    {
                        return null;
                    }
                    break;

                case "Take":
                    if (this.ParseTakeExpression(m))
                    {
                        var nextExpression = m.Arguments[0];
                        return this.Visit(nextExpression);
                    }
                    break;
                case "Skip":
                    if (this.ParseSkipExpression(m))
                    {
                        var nextExpression = m.Arguments[0];
                        return this.Visit(nextExpression);
                    }
                    break;
                case "OrderBy":
                    if (this.ParseOrderByExpression(m, "ASC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return this.Visit(nextExpression);
                    }
                    break;
                case "OrderByDescending":
                    if (this.ParseOrderByExpression(m, "DESC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return this.Visit(nextExpression);
                    }
                    break;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            var q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            var unary = (UnaryExpression)expression.Arguments[1];
            var lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;
            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    _orderBy = string.Format("{0} {1}", body.Member.Name, order);
                }
                else
                {
                    _orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name, order);
                }

                return true;
            }

            return false;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }

        private bool ParseContainsExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("{0} like '%{1}%'", memberExpression.Member.Name, constantExpression.Value);
            return true;
        }

        private bool ParseEndsWithExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("{0} like '%{1}'", memberExpression.Member.Name, constantExpression.Value);
            return true;
        }

        private bool ParseStartsWithExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("{0} like '{1}%'", memberExpression.Member.Name, constantExpression.Value);
            return true;
        }

    }
}
