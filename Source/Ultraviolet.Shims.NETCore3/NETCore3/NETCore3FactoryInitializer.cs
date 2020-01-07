using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Shims.NETCore3.Graphics;
using Ultraviolet.Shims.NETCore3.Input;
using Ultraviolet.Shims.NETCore3.Platform;

namespace Ultraviolet.Shims.NETCore3
{
    /// <summary>
    /// Initializes factory methods for the .NET Core 3.0 platform compatibility shim.
    /// </summary>
    internal sealed class NETCore3FactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new NETCore3SurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new NETCore3SurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new NETCore3IconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new NETCore3ScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETCore3ScreenDensityService(owner, display));

            var softwareKeyboardService = new NETCore2SoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
