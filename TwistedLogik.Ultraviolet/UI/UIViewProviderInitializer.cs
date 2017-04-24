using System;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="UIViewProviderInitializer"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="UIViewProviderInitializer"/> that was created.</returns>
    public delegate UIViewProviderInitializer UIViewProviderInitializerFactory();

    /// <summary>
    /// Represents a class which is responsible for initializing the application's view provider.
    /// </summary>
    public abstract class UIViewProviderInitializer
    {
        /// <summary>
        /// Initializes the view provider.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The view provider configuration object.</param>
        public abstract void Initialize(UltravioletContext uv, Object configuration);
    }
}
