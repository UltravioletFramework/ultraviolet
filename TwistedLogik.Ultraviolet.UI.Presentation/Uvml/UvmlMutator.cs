using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvml
{
    /// <summary>
    /// Represents a mutation to an object produced by a UVML template.
    /// </summary>
    internal abstract class UvmlMutator
    {
        /// <summary>
        /// Applies the mutation to the specified object instance.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="instance">The object instance to mutate.</param>
        /// <param name="context">The current instantiation context.</param>
        public abstract void Mutate(UltravioletContext uv, Object instance, UvmlInstantiationContext context);
    }
}
