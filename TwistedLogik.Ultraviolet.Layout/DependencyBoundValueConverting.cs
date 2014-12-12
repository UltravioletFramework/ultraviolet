using System;
using System.ComponentModel;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a value which is bound to a dependency property and which requires
    /// special conversion logic.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency property.</typeparam>
    /// <typeparam name="TBound">The type of the bound property.</typeparam>
    internal sealed class DependencyBoundValueConverting<TDependency, TBound> : DependencyBoundValue<TBound>, IDependencyBoundValue<TDependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyBoundValueConverting{TDependency, TBound}"/> class.
        /// </summary>
        /// <param name="value">The dependency property value which created this object.</param>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="viewModelType">The type of the view model.</param>
        /// <param name="expression">The binding expression.</param>
        public DependencyBoundValueConverting(IDependencyPropertyValue value, Type expressionType, Type viewModelType, String expression)
            : base(value, expressionType, viewModelType, expression)
        {
            this.formatString = BindingExpressions.GetBindingFormatStringPart(expression);
        }

        /// <inheritdoc/>
        public TDependency Get()
        {
            return hasCachedInputValue ? cachedInputValue : cachedConvertedValue;
        }

        /// <inheritdoc/>
        public TDependency GetFresh()
        {
            var value = GetUnderlyingValue();
            return ConvertBoundValue(value);
        }

        /// <inheritdoc/>
        public void Set(TDependency value)
        {
            var converted = (TBound)ConvertValue(value, typeof(TDependency), typeof(TBound));
            SetUnderlyingValue(converted);
            Digest();

            cachedInputValue = value;
            hasCachedInputValue = true;
        }

        /// <inheritdoc/>
        protected override void OnCachedValueChanged(TBound value)
        {
            hasCachedInputValue  = false;
            cachedConvertedValue = ConvertBoundValue(value);
        }

        /// <summary>
        /// Converts a value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <returns>The converted value.</returns>
        private Object ConvertValue(Object value, Type originalType, Type conversionType)
        {
            if (conversionType == typeof(String))
            {
                return ConvertUsingToString(value, originalType);
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
        private Object ConvertUsingTypeConverter(Object value, Type originalType, Type conversionType)
        {
            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter != null && converter.CanConvertFrom(originalType))
            {
                if (converter.IsValid(value))
                {
                    return converter.ConvertFrom(value);
                }
            }

            return conversionType.IsClass ? null : Activator.CreateInstance(conversionType);
        }

        /// <summary>
        /// Converts a value to a string using the <see cref="Object.ToString()"/> method.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <returns>The converted value.</returns>
        private Object ConvertUsingToString(Object value, Type originalType)
        {
            if (!String.IsNullOrEmpty(formatString))
            {
                var dotNetFormatString = String.Format("{{0:{0}}}", formatString);
                return String.Format(dotNetFormatString, value);
            }

            return (value == null) ? null : value.ToString();
        }

        /// <summary>
        /// Converts a bound value to the type of the dependency property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        private TDependency ConvertBoundValue(TBound value)
        {
            return (TDependency)ConvertValue(value, typeof(TBound), typeof(TDependency));
        }

        // State values.
        private Boolean hasCachedInputValue;
        private TDependency cachedInputValue;
        private TDependency cachedConvertedValue;
        private String formatString;
    }
}
