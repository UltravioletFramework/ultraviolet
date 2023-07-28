using System;
using Ultraviolet.Core;

namespace Ultraviolet
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
            : this(UltravioletSingletonFlags.None, initializer)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletSingleton{T}"/> class.
        /// </summary>
        /// <param name="flags">A set of flags which modify the singleton's behavior.</param>
        /// <param name="initializer">A function which initializes a new instance of the bound resource.</param>
        public UltravioletSingleton(UltravioletSingletonFlags flags, Func<UltravioletContext, T> initializer)
        {
            Contract.Require(initializer, nameof(initializer));

            this.Flags = flags;
            this.initializer = initializer;

            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && uv.IsInitialized && (flags & UltravioletSingletonFlags.Lazy) != UltravioletSingletonFlags.Lazy)
                InitializeResource();

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
        /// Initializes the singleton resource, assuming the Ultraviolet context is currently in a valid state.
        /// </summary>
        /// <returns><see langword="true"/> if the instance was successfully initialized; otherwise, <see langword="false"/>.</returns>
        public Boolean InitializeResource()
        {
            if (initialized)
                return true;

            var uv = UltravioletContext.RequestCurrent();
            if (uv == null || uv.Disposing || uv.Disposed)
                return false;

            if (resource == null || resource.Ultraviolet != uv)
            {
                if (ShouldInitializeResource(uv))
                    resource = initializer(uv);
            }

            initialized = true;
            return true;
        }

        /// <summary>
        /// Gets the singleton's flags.
        /// </summary>
        public UltravioletSingletonFlags Flags { get; }

        /// <summary>
        /// Gets the bound resource.
        /// </summary>
        public T Value
        {
            get 
            {
                if (!initialized && !InitializeResource())
                    throw new InvalidOperationException(UltravioletStrings.FailedToInitializeSingleton);

                return resource; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the singleton should be initialized for the
        /// specified Ultraviolet context.
        /// </summary>
        private Boolean ShouldInitializeResource(UltravioletContext uv)
        {
            var disabledInServiceMode = (Flags & UltravioletSingletonFlags.DisabledInServiceMode) == UltravioletSingletonFlags.DisabledInServiceMode;
            if (disabledInServiceMode && uv.IsRunningInServiceMode)
                return false;

            return true;
        }
        
        /// <summary>
        /// Handles the <see cref="UltravioletContext.ContextInitialized"/> event.
        /// </summary>
        private void UltravioletContext_ContextInitialized(object sender, EventArgs e)
        {
            if ((Flags & UltravioletSingletonFlags.Lazy) != UltravioletSingletonFlags.Lazy)
                InitializeResource();
        }

        /// <summary>
        /// Handles the <see cref="UltravioletContext.ContextInvalidated"/> event.
        /// </summary>
        private void UltravioletContext_ContextInvalidated(object sender, EventArgs e)
        {
            SafeDispose.DisposeRef(ref resource);
            initialized = false;
        }

        // State values.
        private readonly Func<UltravioletContext, T> initializer;
        private T resource;
        private Boolean initialized;
    }
}
