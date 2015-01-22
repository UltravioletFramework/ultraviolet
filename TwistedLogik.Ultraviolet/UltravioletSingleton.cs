using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a singleton resource.  Only one instance of the resource will be created
    /// during the lifespan of a particular Ultraviolet context, but the resource will be destroyed
    /// and recreated if a new context is introduced.
    /// </summary>
    /// <typeparam name="T">The type of object which is owned by the singleton.</typeparam>
    public sealed class UltravioletSingleton<T> where T : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletSingleton{T}"/> class.
        /// </summary>
        /// <param name="initializer">A function which initializes a new instance of the bound resource.</param>
        public UltravioletSingleton(Func<UltravioletContext, T> initializer)
        {
            Contract.Require(initializer, "initializer");

            this.initializer = initializer;

            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && uv.IsInitialized)
            {
                initializer(uv);
            }

            UltravioletContext.ContextInitialized += UltravioletContext_ContextInitialized;
            UltravioletContext.ContextInvalidated += UltravioletContext_ContextInvalidated;
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
                InitializeResource();
                return resource; 
            }
        }

        /// <summary>
        /// Initializes the singleton resource.
        /// </summary>
        private void InitializeResource()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv == null)
                return;

            if (resource == null || resource.Ultraviolet != uv)
            {
                resource = initializer(uv);
            }
        }

        /// <summary>
        /// Handles the <see cref="UltravioletContext.ContextInitialized"/> event.
        /// </summary>
        private void UltravioletContext_ContextInitialized(object sender, EventArgs e)
        {
            InitializeResource();
        }

        /// <summary>
        /// Handles the <see cref="UltravioletContext.ContextInvalidated"/> event.
        /// </summary>
        private void UltravioletContext_ContextInvalidated(object sender, EventArgs e)
        {
            SafeDispose.DisposeRef(ref resource);
        }

        // Property values.
        private T resource;

        // State values.
        private readonly Func<UltravioletContext, T> initializer;
    }
}
