namespace Wpf3DAnimations.Common.Mvvm
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class PropertyHelper
    {
        public static string GetName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(@"The expression is not a member access expression.", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(@"The member access expression does not access a property.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        public static T CreateIfNeeded<T>(ref T field, Func<T> newInstanceCreateMethod) where T : class
        {
            if (newInstanceCreateMethod == null)
            {
                return default(T);
            }
            return field ?? (field = newInstanceCreateMethod.Invoke());
        }
    }
}
