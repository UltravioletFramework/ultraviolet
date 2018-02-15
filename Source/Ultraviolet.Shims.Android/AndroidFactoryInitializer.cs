using Ultraviolet.Shims.Android.Graphics;
using Ultraviolet.Shims.Android.Input;
using Ultraviolet.Shims.Android.Platform;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android
{
    /// <summary>
    /// Initializes factory methods for the Android platform compatibility shim.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class AndroidFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            var androidActivityService = new AndroidActivityServiceImpl();
            factory.SetFactoryMethod<AndroidActivityServiceFactory>(() => androidActivityService);

            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new AndroidSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new AndroidSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new AndroidIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new AndroidScreenRotationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new AndroidScreenDensityService(display));

            var softwareKeyboardService = new AndroidSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
