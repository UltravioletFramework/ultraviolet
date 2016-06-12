using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.iOS.Graphics;
using TwistedLogik.Ultraviolet.iOS.Input;
using TwistedLogik.Ultraviolet.iOS.Platform;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.iOS
{
    /// <summary>
    /// Initializes factory methods for the iOS platform compatibility shim.
    /// </summary>
    internal sealed class iOSFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new iOSSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new iOSSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new iOSIconLoader());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new iOSScreenRotationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new iOSScreenDensityService(display));

            var softwareKeyboardService = new iOSSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
