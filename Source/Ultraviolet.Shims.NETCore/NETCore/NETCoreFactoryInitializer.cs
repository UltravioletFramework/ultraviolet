using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Shims.NETCore.Graphics;
using Ultraviolet.Shims.NETCore.Input;
using Ultraviolet.Shims.NETCore.Platform;

namespace Ultraviolet.Shims.NETCore
{
    /// <summary>
    /// Initializes factory methods for the .NET Core platform compatibility shim.
    /// </summary>
    internal sealed class NETCoreFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new NETCoreSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new NETCoreSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new NETCoreIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new NETCoreScreenRotationService(display));

            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETCoreScreenDensityService_Windows(owner, display));
                    break;

                default:
                    factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETCoreScreenDensityService(owner, display));
                    break;
            }

            var softwareKeyboardService = new NETCoreSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
