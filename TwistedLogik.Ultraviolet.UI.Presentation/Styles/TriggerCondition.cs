using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents one of the conditions of a property trigger.
    /// </summary>
    public class TriggerCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerCondition"/> class.
        /// </summary>
        /// <param name="op">A <see cref="TriggerComparisonOp"/> value that specifies the type of comparison performed by this condition.</param>
        /// <param name="dpropName">The name of the dependency property to evaluate.</param>
        /// <param name="refval">The reference value to compare to the value of the dependency property.</param>
        internal TriggerCondition(TriggerComparisonOp op, String dpropName, String refval)
        {
            this.op        = op;
            this.dpropName = dpropName;
            this.refval    = refval;
        }

        /// <summary>
        /// Evaluates whether the condition is true for the specified object.
        /// </summary>
        /// <param name="dobj">The object against which to evaluate the trigger condition.</param>
        /// <returns><c>true</c> if the condition is true for the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Evaluate(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var dprop = DependencyProperty.FindByStylingName(dpropName, dobj.GetType());
            if (dprop == null)
                return false;

            if (refvalCache == null || refvalCache.GetType() != dprop.PropertyType)
                refvalCache = ObjectResolver.FromString(refval, dprop.PropertyType);

            var comparison = TriggerComparisonCache.Get(dprop.PropertyType, op);
            return comparison(dobj, dprop, refvalCache);
        }

        /// <summary>
        /// Gets the name of the dependency property which is evaluated by this condition.
        /// </summary>
        public String DependencyPropertyName
        {
            get { return dpropName; }
        }

        /// <summary>
        /// Gets a string which represents the reference value for this condition.
        /// </summary>
        public String ReferenceValue
        {
            get { return refval; }
        }

        // State values.
        private readonly TriggerComparisonOp op;
        private readonly String dpropName;
        private readonly String refval;
        private Object refvalCache;
    }
}
