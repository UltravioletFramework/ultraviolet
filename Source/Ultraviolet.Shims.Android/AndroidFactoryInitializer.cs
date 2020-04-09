using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Shims.Android.Graphics;
using Ultraviolet.Shims.Android.Input;
using Ultraviolet.Shims.Android.Platform;

namespace Ultraviolet.Shims.Android
{
    /// <summary>
    /// Initializes factory methods for the Android platform compatibility shim.
    /// </summary>
    internal sealed class AndroidFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new AndroidSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new AndroidSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new AndroidIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new AndroidScreenRotationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new AndroidScreenDensityService(display));
            factory.SetFactoryMethod<AssemblyLoaderServiceFactory>(() => new AndroidAssemblyLoaderService());

            var softwareKeyboardService = new AndroidSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}