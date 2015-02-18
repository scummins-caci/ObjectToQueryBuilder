using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectToQueryBuilder
{
    public static class PropertyHelper
    {
        public static string GetPropertyName<TObject>(this TObject type,
                                        Expression<Func<TObject, object>> PropertyRefExp)
        {
            return GetPropertyNameCore(PropertyRefExp.Body);
        }

        public static string GetName<TObject>(Expression<Func<TObject, object>> propertyRefExpr)
        {
            return GetPropertyNameCore(propertyRefExpr.Body);
        }

        private static string GetPropertyNameCore(Expression propertyRefExpr)
        {
            if (propertyRefExpr == null)
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");

            var memberExpr = propertyRefExpr as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = propertyRefExpr as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                return memberExpr.Member.Name;

            throw new ArgumentException("No property reference expression was found.",
                             "propertyRefExpr");
        }
    }
}
