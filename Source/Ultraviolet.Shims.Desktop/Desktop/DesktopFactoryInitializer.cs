using Ultraviolet.Core;
using Ultraviolet.Shims.Desktop.Graphics;
using Ultraviolet.Shims.Desktop.Input;
using Ultraviolet.Shims.Desktop.Platform;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Desktop
{
    /// <summary>
    /// Initializes factory methods for the Desktop platform compatibility shim.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class DesktopFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new DesktopSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new DesktopSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new DesktopIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new DesktopScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new DesktopScreenDensityService(owner, display));

            var softwareKeyboardService = new DesktopSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
