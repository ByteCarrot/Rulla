using System;
using System.Linq.Expressions;

namespace ByteCarrot.Rulla.Infrastructure
{
    public static class ExpressionExtensions
    {
        public static string GetName<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var body = expression.Body;
            return GetMemberName(body);
        }

        public static string GetName<TResult>(this Expression<Func<TResult>> expression)
        {
            var body = expression.Body;
            return GetMemberName(body);
        }

        private static string GetMemberName(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    return GetMemberName(memberExpression.Expression)
                           + "."
                           + memberExpression.Member.Name;
                }

                return memberExpression.Member.Name;
            }

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                if (unaryExpression.NodeType != ExpressionType.Convert)
                {
                    throw new Exception("Cannot interpret member from {0}" + expression);
                }

                return GetMemberName(unaryExpression.Operand);
            }

            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
            {
                if (methodCallExpression.Object.NodeType == ExpressionType.MemberAccess)
                {
                    return GetMemberName(methodCallExpression.Object)
                           + "."
                           + methodCallExpression.Method.Name;
                }

                return methodCallExpression.Method.Name;
            }

            throw new NotSupportedException("Could not determine member from {0}" + expression);
        }
    }
}