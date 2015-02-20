using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectToQueryBuilder
{
    public class QueryTranslator : ExpressionVisitor
    {
        private StringBuilder sb;
        private string _orderBy = string.Empty;
        private string _whereClause = string.Empty;
        private readonly IDictionary<string, string> _propertyMapping;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyMapping">mapping between class properties and sql column names</param>
        public QueryTranslator(IDictionary<string, string> propertyMapping = null)
        {
            Take = null;
            Skip = null;
            _propertyMapping = propertyMapping;
        }

        public int? Skip { get; private set; }

        public int? Take { get; private set; }

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
            sb = new StringBuilder();
            Visit(expression);
            _whereClause = sb.ToString();

            // remove leading and trailing parenthesis
            if (_whereClause.Length > 0)
            {
                _whereClause = _whereClause.Substring(1, _whereClause.Length - 2);    
            }
            
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
                Visit(m.Arguments[0]);
                var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                Visit(lambda.Body);
                return m;
            }
            
            switch (m.Method.Name)
            {
                case "StartsWith":
                    if (ParseStartsWithExpression(m))
                    {
                        return m;
                    }
                    break;

                case "EndsWith":
                    if (ParseEndsWithExpression(m))
                    {
                        return m;
                    }
                    break;

                case "Contains":
                    if (ParseContainsExpression(m))
                    {
                        return m;
                    }
                    break;

                case "Take":
                    if (ParseTakeExpression(m))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
                    }
                    break;
                case "Skip":
                    if (ParseSkipExpression(m))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
                    }
                    break;
                case "OrderBy":
                    if (ParseOrderByExpression(m, "ASC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
                    }
                    break;
                case "ThenBy":
                    if (ParseOrderByExpression(m, "ASC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
                    }
                    break;
                case "OrderByDescending":
                    if (ParseOrderByExpression(m, "DESC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
                    }
                    break;
                case "ThenByDescending":
                    if (ParseOrderByExpression(m, "DESC"))
                    {
                        var nextExpression = m.Arguments[0];
                        return Visit(nextExpression);
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
                    Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    Visit(u.Operand);
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
            Visit(b.Left);

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
                    sb.Append(IsNullConstant(b.Right) ? " IS " : " = ");
                    break;

                case ExpressionType.NotEqual:
                    sb.Append(IsNullConstant(b.Right) ? " IS NOT " : " <> ");
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

            Visit(b.Right);
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
                        sb.AppendFormat("TO_DATE('{0}', 'MM/DD/YYYY HH:MI:SS AM')", c.Value);
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
            if (m.Expression == null || m.Expression.NodeType != ExpressionType.Parameter)
            {
                throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
            }

            sb.Append(MapMemberNameToColumnName(m.Member));
            return m;
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

            var body = lambdaExpression.Body as MemberExpression;
            if (body == null) return false;

            _orderBy = string.IsNullOrEmpty(_orderBy) 
                ? string.Format("{0} {1}", MapMemberNameToColumnName(body.Member), order)
                : string.Format("{0}, {1} {2}", _orderBy, MapMemberNameToColumnName(body.Member), order);

            return true;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            var sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (!int.TryParse(sizeExpression.Value.ToString(), out size)) return false;

            Take = size;
            return true;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            var sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (!int.TryParse(sizeExpression.Value.ToString(), out size)) return false;

            Skip = size;
            return true;
        }

        private bool ParseContainsExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("({0} like '%{1}%')", MapMemberNameToColumnName(memberExpression.Member), constantExpression.Value);
            return true;
        }

        private bool ParseEndsWithExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("({0} like '%{1}')", MapMemberNameToColumnName(memberExpression.Member), constantExpression.Value);
            return true;
        }

        private bool ParseStartsWithExpression(MethodCallExpression expression)
        {
            var memberExpression = expression.Object as MemberExpression;
            var constantExpression = expression.Arguments[0] as ConstantExpression;

            if (memberExpression == null || constantExpression == null) return false;

            sb.AppendFormat("({0} like '{1}%')", MapMemberNameToColumnName(memberExpression.Member), constantExpression.Value);
            return true;
        }

        /// <summary>
        /// Takes the member name and maps it to colun name
        /// </summary>
        /// <param name="member">member to check</param>
        /// <returns>column name</returns>
        private string MapMemberNameToColumnName(MemberInfo member)
        {
            if (_propertyMapping == null)
            {
                return member.Name;
            }

            if (!_propertyMapping.ContainsKey(member.Name))
            {
                return member.Name;
            }

            return _propertyMapping.FirstOrDefault(x => x.Key == member.Name).Value;
        }
    }
}
