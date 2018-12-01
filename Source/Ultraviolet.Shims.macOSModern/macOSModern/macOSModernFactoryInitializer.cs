using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Shims.macOSModern.Graphics;
using Ultraviolet.Shims.macOSModern.Input;
using Ultraviolet.Shims.macOSModern.Platform;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOSModern
{
    internal sealed class macOSFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new macOSModernSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new macOSModernSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new macOSModernIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new macOSModernScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new macOSModernScreenDensityService(display));

            var softwareKeyboardService = new macOSModernSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}