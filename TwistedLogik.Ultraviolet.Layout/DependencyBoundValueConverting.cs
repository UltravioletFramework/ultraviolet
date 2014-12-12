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
            return hasStringRepresentation ? cachedStringRepresentation : cachedConvertedValue;
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
            var blargh = false;
            var converted = (TBound)ConvertValue(value, typeof(TDependency), typeof(TBound), out blargh);
            SetUnderlyingValue(converted);
            if (blargh)
            {
                Digest();
                hasStringRepresentation = true;
                cachedStringRepresentation = (TDependency)(Object)value;
            }
        }

        /// <inheritdoc/>
        protected override void OnCachedValueChanged(TBound value)
        {
            cachedConvertedValue = ConvertBoundValue(value);
        }

        /// <summary>
        /// Converts a value to the specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <param name="failed">A value indicating whether the conversion failed.</param>
        /// <returns>The converted value.</returns>
        private Object ConvertValue(Object value, Type originalType, Type conversionType, out Boolean failed)
        {
            hasStringRepresentation = false;

            failed = false;
            if (conversionType == typeof(String))
            {
                return ConvertUsingToString(value, originalType, out failed);
            }
            return ConvertUsingTypeConverter(value, originalType, conversionType, out failed);
        }

        /// <summary>
        /// Converts a value to the specified type using a <see cref="TypeConverter"/>, if one is available.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="originalType">The value's original type.</param>
        /// <param name="conversionType">The type to which to convert the value.</param>
        /// <param name="failed">A value indicating whether the conversion failed.</param>
        /// <returns>The converted value.</returns>
        private Object ConvertUsingTypeConverter(Object value, Type originalType, Type conversionType, out Boolean failed)
        {
            failed = true;

            var converter = TypeDescriptor.GetConverter(conversionType);
            if (converter != null && converter.CanConvertFrom(originalType))
            {
                if (converter.IsValid(value))
                {
                    failed = false;
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
        /// <param name="failed">A value indicating whether the conversion failed.</param>
        /// <returns>The converted value.</returns>
        private Object ConvertUsingToString(Object value, Type originalType, out Boolean failed)
        {
            failed = false;

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
            var failed = false;
            var result = (TDependency)ConvertValue(value, typeof(TBound), typeof(TDependency), out failed);
            if (failed)
            {
                hasStringRepresentation = true;
                cachedStringRepresentation = (TDependency)(Object)value;
            }
            return result;
        }

        // State values.
        private Boolean hasStringRepresentation;
        private TDependency cachedStringRepresentation;
        private TDependency cachedConvertedValue;
        private String formatString;
    }
}
