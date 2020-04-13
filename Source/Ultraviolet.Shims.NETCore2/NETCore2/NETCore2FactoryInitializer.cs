using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Shims.NETCore2.Graphics;
using Ultraviolet.Shims.NETCore2.Input;
using Ultraviolet.Shims.NETCore2.Platform;

namespace Ultraviolet.Shims.NETCore2
{
    /// <summary>
    /// Initializes factory methods for the .NET Core 2.0 platform compatibility shim.
    /// </summary>
    internal sealed class NETCore2FactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new NETCore2SurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new NETCore2SurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new NETCore2IconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new NETCore2ScreenOrientationService(display));

            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETCore2ScreenDensityService_Windows(owner, display));
                    break;

                default:
                    factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETCore2ScreenDensityService(owner, display));
                    break;
            }

            var softwareKeyboardService = new NETCore2SoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
