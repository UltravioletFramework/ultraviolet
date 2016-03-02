using System;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets a standard property value.
    /// </summary>
    internal sealed class UvmlStandardPropertyValueMutator : UvmlMutator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlStandardPropertyValueMutator"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property which is being mutated.</param>
        /// <param name="propertyValue">The value to set on the property.</param>
        public UvmlStandardPropertyValueMutator(PropertyInfo propertyInfo, UvmlNode propertyValue)
        {
            Contract.Require(propertyInfo, nameof(propertyInfo));
            Contract.Require(propertyValue, nameof(propertyValue));

            this.propertyInfo = propertyInfo;
            this.propertyValue = propertyValue;
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            propertyInfo.SetValue(instance, propertyValue.Instantiate(uv, context), null);
        }

        // State values.
        private readonly PropertyInfo propertyInfo;
        private readonly UvmlNode propertyValue;
    }
}
