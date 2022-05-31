using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a delegate which produces instances of the <see cref="DependencyBoundValueConverting{TDependency, TBound}"/> class.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency property.</typeparam>
    /// <param name="value">The dependency property value which created this object.</param>
    /// <param name="expressionType">The type of the bound expression.</param>
    /// <param name="dataSourceType">The type of the data source.</param>
    /// <param name="expression">The binding expression.</param>
    /// <param name="coerceToString">A value indicating whether to coerce Object values to String values if no valid type conversion exists.</param>
    /// <returns>The bound value instance which was created.</returns>
    internal delegate IDependencyBoundValue<TDependency> DependencyBoundValueConvertingCtor<TDependency>(
        IDependencyPropertyValue value, Type expressionType, Type dataSourceType, String expression, Boolean coerceToString);

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
        /// <param name="dataSourceType">The type of the data source.</param>
        /// <param name="expression">The binding expression.</param>
        /// <param name="coerceToString">A value indicating whether to coerce Object values to String values if no valid type conversion exists.</param>
        public DependencyBoundValueConverting(IDependencyPropertyValue value, Type expressionType, Type dataSourceType, String expression, Boolean coerceToString)
            : base(value, expressionType, dataSourceType, expression)
        {
            var expressionFormatString = BindingExpressions.GetBindingFormatStringPart(expression);
            SetFormatString(expressionFormatString);

            this.coerceToString = coerceToString;
            this.cachedConvertedValue = GetFresh();
        }

        /// <inheritdoc/>
        public override void SetFormatString(String formatString)
        {
            this.formatString = (formatString == null) ? null : String.Format("{{0:{0}}}", formatString); ;

            if (hasCachedInputValue)
            {
                Set(cachedInputValue);
            }
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
        public override void InvalidateDisplayCache()
        {
            cachedInputValue = cachedConvertedValue;
        }

        /// <inheritdoc/>
        public void Set(TDependency value)
        {
            var converted = (TBound)BindingConversions.ConvertValue(value, 
                typeof(TDependency), typeof(TBound), formatString, coerceToString);
            SetUnderlyingValue(converted);

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
        /// Converts a bound value to the type of the dependency property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        private TDependency ConvertBoundValue(TBound value)
        {
            return (TDependency)BindingConversions.ConvertValue(value, 
                typeof(TBound), typeof(TDependency), formatString, coerceToString);
        }

        // State values.
        private Boolean hasCachedInputValue;
        private TDependency cachedInputValue;
        private TDependency cachedConvertedValue;
        private String formatString;
        private Boolean coerceToString;
    }    
}
