using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a singleton resource.  Only one instance of the resource will be created
    /// during the lifespan of a particular Ultraviolet context, but the resource will be destroyed
    /// and recreated if a new context is introduced.
    /// </summary>
    public sealed class UltravioletSingleton<T> where T : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the UltravioletContextBoundResource class.
        /// </summary>
        /// <param name="initializer">A function which initializes a new instance of the bound resource.</param>
        public UltravioletSingleton(Func<UltravioletContext, T> initializer)
        {
            Contract.Require(initializer, "initializer");

            this.initializer = initializer;
        }

        /// <summary>
        /// Implicitly converts a bound resource to its underlying resource object.
        /// </summary>
        /// <param name="resource">The bound resource to convert.</param>
        /// <returns>The converted resource.</returns>
        public static implicit operator T(UltravioletSingleton<T> resource)
        {
            return resource == null ? null : resource.Value;
        }

        /// <summary>
        /// Gets the bound resource.
        /// </summary>
        public T Value
        {
            get
            {
                var uv = UltravioletContext.DemandCurrent();
                if (resource == null || resource.Ultraviolet != uv)
                {
                    UltravioletContext.ContextInvalidated += uv_contextInvalidated;
                    resource = initializer(uv);
                }
                return resource;
            }
        }

        /// <summary>
        /// Handles the contextInvalidated event.
        /// </summary>
        private void uv_contextInvalidated(object sender, EventArgs e)
        {
            UltravioletContext.ContextInvalidated -= uv_contextInvalidated;
            SafeDispose.DisposeRef(ref resource);
        }

        // Property values.
        private T resource;

        // State values.
        private readonly Func<UltravioletContext, T> initializer;
    }
}
