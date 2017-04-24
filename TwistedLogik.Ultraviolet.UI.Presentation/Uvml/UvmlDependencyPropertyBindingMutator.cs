using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which binds a dependency property value.
    /// </summary>
    internal sealed class UvmlDependencyPropertyBindingMutator : UvmlMutator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlDependencyPropertyBindingMutator"/> class.
        /// </summary>
        /// <param name="dpropID">The property which is being mutated.</param>
        /// <param name="dpropValue">The value to set on the property.</param>
        public UvmlDependencyPropertyBindingMutator(DependencyProperty dpropID, UvmlNode dpropValue)
        {
            Contract.Require(dpropID, nameof(dpropID));
            Contract.Require(dpropValue, nameof(dpropValue));

            this.dpropID = dpropID;
            this.dpropValue = dpropValue;
        }

        /// <inheritdoc/>
        public override Object InstantiateValue(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            return dpropValue.Instantiate(uv, context);
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context)
        {
            var value = InstantiateValue(uv, instance, context);
            Mutate(uv, instance, value, context);
        }

        /// <inheritdoc/>
        public override void Mutate(UltravioletContext uv, Object instance, Object value, UvmlInstantiationContext context)
        {
            var dobj = instance as DependencyObject;
            if (dobj == null)
                return;

            var expression = ProcessPrecomputedValue<String>(value, context);
            if (expression == null)
                throw new UvmlException(PresentationStrings.InvalidBindingExpression.Format("(null)"));

            var compiled = context.GetCompiledBindingExpression(dpropID.PropertyType, expression);
            if (compiled == null)
                throw new UvmlException(PresentationStrings.CompiledExpressionNotFound.Format(expression));

            var fmtString = BindingExpressions.GetBindingFormatStringPart(expression);
            if (fmtString != null)
            {
                dobj.BindValue(dpropID, context.DataSourceType, "{{" + compiled.Name + "}}[" + fmtString + "]");
            }
            else
            {
                dobj.BindValue(dpropID, context.DataSourceType, "{{" + compiled.Name + "}}");
            }
        }

        // State values.
        private readonly DependencyProperty dpropID;
        private readonly UvmlNode dpropValue;
    }
}
