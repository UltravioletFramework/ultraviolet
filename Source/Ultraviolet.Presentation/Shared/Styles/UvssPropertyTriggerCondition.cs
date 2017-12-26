using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents one of the conditions of a property trigger.
    /// </summary>
    public class UvssPropertyTriggerCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssPropertyTriggerCondition"/> class.
        /// </summary>
        /// <param name="op">A <see cref="TriggerComparisonOp"/> value that specifies the type of comparison performed by this condition.</param>
        /// <param name="propertyName">The name of the property to evaluate.</param>
        /// <param name="propertyValue">The value to compare to the value of the evaluated property.</param>
        internal UvssPropertyTriggerCondition(TriggerComparisonOp op, DependencyName propertyName, DependencyValue propertyValue)
        {
            this.op = op;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

        /// <summary>
        /// Evaluates whether the condition is true for the specified object.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The object against which to evaluate the trigger condition.</param>
        /// <returns><see langword="true"/> if the condition is true for the specified object; otherwise, <see langword="false"/>.</returns>
        internal Boolean Evaluate(UltravioletContext uv, DependencyObject dobj)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));

            var dprop = DependencyProperty.FindByStylingName(uv, dobj, propertyName.Owner, propertyName.Name);
            if (dprop == null)
                return false;

            var refvalCacheType = (propertyValueCache == null) ? null : propertyValueCache.GetType();
            if (refvalCacheType == null || (refvalCacheType != dprop.PropertyType &&  refvalCacheType != dprop.UnderlyingType))
            {
                propertyValueCache = ObjectResolver.FromString(
                    propertyValue.Value, dprop.PropertyType, propertyValue.Culture);
            }

            var comparison = TriggerComparisonCache.Get(dprop.PropertyType, op);
            if (comparison == null)
                throw new InvalidOperationException(PresentationStrings.InvalidTriggerComparison.Format(propertyName, op, dprop.PropertyType));

            return comparison(dobj, dprop, propertyValueCache);
        }

        /// <summary>
        /// Gets the comparison operation performed by this condition.
        /// </summary>
        /// <value>A <see cref="TriggerComparisonOp"/> value that specifies how the value of the condition's
        /// dependency property will be compared against its reference value.</value>
        public TriggerComparisonOp ComparisonOperation
        {
            get { return op; }
        }

        /// <summary>
        /// Gets the name of the dependency property which is evaluated by this condition.
        /// </summary>
        /// <value>A <see cref="DependencyName"/> value which describes the name of the dependency property
        /// that will be compared against this condition's reference value.</value>
        public DependencyName PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets a string which represents the reference value for this condition.
        /// </summary>
        /// <value>A <see cref="DependencyValue"/> value which contains the reference value which will be
        /// compared to the value of the condition's dependency property.</value>
        public DependencyValue PropertyValue
        {
            get { return propertyValue; }
        }

        // State values.
        private readonly TriggerComparisonOp op;
        private readonly DependencyName propertyName;
        private readonly DependencyValue propertyValue;
        private Object propertyValueCache;
    }
}
