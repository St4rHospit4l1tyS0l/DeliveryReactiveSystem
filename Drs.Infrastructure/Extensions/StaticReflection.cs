using System;
using System.Linq.Expressions;

namespace Drs.Infrastructure.Extensions
{
    public static class StaticReflection
    {
        public static string PropertyName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var body = expression.Body as MemberExpression;
            return body == null ? string.Empty : body.Member.Name;
        }
        
        public static string GetMemberName<T>(
            this T instance,
            Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(
            this T instance,
            Expression<Action<T>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            var expression1 = expression as MemberExpression;
            if (expression1 != null)
            {
                // Reference type property or field
                var memberExpression =
                    expression1;
                return memberExpression.Member.Name;
            }

            var callExpression = expression as MethodCallExpression;
            if (callExpression != null)
            {
                // Reference type method
                var methodCallExpression =
                    callExpression;
                return methodCallExpression.Method.Name;
            }

            var unaryExpression1 = expression as UnaryExpression;
            if (unaryExpression1 != null)
            {
                // Property, field of method returning value type
                var unaryExpression = unaryExpression1;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(
            UnaryExpression unaryExpression)
        {
            var operand = unaryExpression.Operand as MethodCallExpression;
            if (operand == null)
                return ((MemberExpression) unaryExpression.Operand).Member.Name;
            var methodExpression =
                operand;
            return methodExpression.Method.Name;
        }

        public static object GetValue(object model, string columnName)
        {
            var property = (model).GetType().GetProperty(columnName);

            if (property == null)
                return null;

            return property.GetValue(model);
        }
    }
}
