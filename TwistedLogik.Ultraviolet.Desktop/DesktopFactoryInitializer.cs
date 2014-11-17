using TwistedLogik.Ultraviolet.Desktop.Graphics;
using TwistedLogik.Ultraviolet.Desktop.Platform;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop
{
    /// <summary>
    /// Initializes factory methods for the Desktop platform compatibility shim.
    /// </summary>
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
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new DesktopScreenDensityService(display));
        }
    }
}
