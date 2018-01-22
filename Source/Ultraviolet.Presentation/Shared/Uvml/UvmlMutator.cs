using System;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a mutation to an object produced by a UVML template.
    /// </summary>
    internal abstract class UvmlMutator
    {
        /// <summary>
        /// Instantiates the value associated with this mutation.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance to mutate.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The value which was instantiated.</returns>
        public abstract Object InstantiateValue(UltravioletContext uv, Object instance, UvmlInstantiationContext context);

        /// <summary>
        /// Applies the mutation to the specified object instance.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance to mutate.</param>
        /// <param name="context">The current instantiation context.</param>
        public abstract void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context);

        /// <summary>
        /// Applies the mutation to the specified object instance using the specified precomputed value.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance to mutate.</param>
        /// <param name="value">The precomputed value to apply with the mutation.</param>
        /// <param name="context">The current instantiation context.</param>
        public abstract void Mutate(UltravioletContext uv, Object instance, Object value, UvmlInstantiationContext context);

        /// <summary>
        /// Processes the specified value for use by a mutator.
        /// </summary>
        /// <typeparam name="T">The type of value which is expected.</typeparam>
        /// <param name="value">The value to process.</param>
        /// <param name="context">The current instantiation context.</param>
        /// <returns>The value after it has been converted to the expected type.</returns>
        protected static T ProcessPrecomputedValue<T>(Object value, UvmlInstantiationContext context)
        {
            var templateInstance = value as UvmlTemplateInstance;
            if (templateInstance != null)
                return (T)templateInstance.FinalizeInstance();

            var elementReference = value as UvmlElementReference;
            if (elementReference != null)
                return (T)elementReference.GetReferencedElement(context.Namescope);

            return (T)value;
        }
    }
}
