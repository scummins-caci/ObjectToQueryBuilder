using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http.OData.Query;

namespace ODataToQuery
{
    public static class ODataQueryOptionsExtensions
    {
        public static Expression<Func<TElement, bool>> ToExpression<TElement>(this FilterQueryOption filter)
        {
            IQueryable queryable = Enumerable.Empty<TElement>().AsQueryable();
            queryable = filter.ApplyTo(queryable, new ODataQuerySettings
            {
                EnableConstantParameterization = false, 
                HandleNullPropagation = HandleNullPropagationOption.False
            });

            var methodCallExp = queryable.Expression as MethodCallExpression;
            if (methodCallExp == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof (TElement)));
            }


            var quote = methodCallExp.Arguments[1] as UnaryExpression;
            if (quote == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(TElement)));                
            }

            return quote.Operand as Expression<Func<TElement, bool>>;
        }

        public static Expression ToExpression<TElement>(this OrderByQueryOption orderBy)
        {
            IQueryable queryable = Enumerable.Empty<TElement>().AsQueryable();
            queryable = orderBy.ApplyTo(queryable, new ODataQuerySettings
            {
                EnableConstantParameterization = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            });

            var methodCallExp = queryable.Expression as MethodCallExpression;
            if (methodCallExp == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, string>>(Expression.Constant(true),
                    Expression.Parameter(typeof(TElement)));
            }

            return methodCallExp;
        }
    
    }

    
}