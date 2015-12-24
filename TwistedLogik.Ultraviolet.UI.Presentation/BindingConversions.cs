using System;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for performing conversions between values used by binding expressions.
    /// </summary>
    internal static class BindingConversions
    {
        /// <summary>
        /// Converts a value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <returns>The converted value.</returns>
        public static Object ConvertValue(Object value, Type originalType, Type conversionType)
        {
            return ConvertValue(value, originalType, conversionType, null, false);
        }

        /// <summary>
        /// Converts a value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <param name="formatString">The format string used to convert objects to strings.</param>
        /// <param name="coerceToString">A value indicating whether to coerce Object values to String values if no valid type conversion exists.</param>
        /// <returns>The converted value.</returns>
        public static Object ConvertValue(Object value, Type originalType, Type conversionType, String formatString, Boolean coerceToString)
        {
            if (conversionType == typeof(String) || (conversionType == typeof(Object) && coerceToString))
            {
                return ConvertUsingToString(value, originalType, formatString);
            }
            return ConvertUsingTypeConverter(value, originalType, conversionType);
        }

        /// <summary>
        /// Converts a value to the specified type using a <see cref="TypeConverter"/>, if one is available.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <returns>The converted value.</returns>
        private static Object ConvertUsingTypeConverter(Object value, Type originalType, Type conversionType)
        {
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter != null && converter.CanConvertFrom(originalType))
            {
                /* HACK: converter.IsValid() will throw an exception for null/empty strings
                 * in some circumstances. It's handled in System.dll but ultimately a pointless
                 * inefficiency, so we prevent that here. */
                var assumeInvalid = false;
                if (originalType == typeof(String) && conversionType.IsNumericType())
                {
                    if (String.IsNullOrEmpty((String)value))
                        assumeInvalid = true;
                }

                if (!assumeInvalid && converter.IsValid(value))
                {
                    return converter.ConvertFrom(value);
                }
            }

            if (conversionType.IsAssignableFrom(originalType))
                return value;

            return conversionType.IsClass ? null : Activator.CreateInstance(conversionType);
        }

        /// <summary>
        /// Converts a value to a string using the <see cref="Object.ToString()"/> method.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="formatString">The format string used to convert the value.</param>
        /// <returns>The converted value.</returns>
        private static Object ConvertUsingToString(Object value, Type originalType, String formatString)
        {
            if (!String.IsNullOrEmpty(formatString))
            {
                return String.Format(formatString, value);
            }
            return (value == null) ? null : value.ToString();
        }
    }
}
