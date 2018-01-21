using System;
using System.Reflection;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains utility methods relating to expression compilation.
    /// </summary>
    internal static class ExpressionUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified attribute name represents an attached property.
        /// </summary>
        /// <param name="name">The attribute name to evaluate.</param>
        /// <param name="container">The attached property's container type.</param>
        /// <param name="property">The attached property's property name.</param>
        /// <returns><see langword="true"/> if the specified name represents an attached property; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsAttachedProperty(String name, out String container, out String property)
        {
            container = null;
            property = null;

            var delimiterIx = name.IndexOf('.');
            if (delimiterIx >= 0)
            {
                container = name.Substring(0, delimiterIx);
                property = name.Substring(delimiterIx + 1);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified method.
        /// </summary>
        public static Boolean NeedsWrapper(MethodInfo method)
        {
            if (method.IsSpecialName)
                return false;

            if (method.Name.StartsWith("get_") || method.Name.StartsWith("end_"))
                return false;

            if (method.DeclaringType == typeof(Object))
                return false;

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified property.
        /// </summary>
        public static Boolean NeedsWrapper(PropertyInfo property)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether a wrapper should be generated for the specified field.
        /// </summary>
        public static Boolean NeedsWrapper(FieldInfo field)
        {
            return true;
        }
    }
}
