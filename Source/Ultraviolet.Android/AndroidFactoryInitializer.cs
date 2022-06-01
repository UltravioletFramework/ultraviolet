using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Android.Graphics;
using Ultraviolet.Android.Input;
using Ultraviolet.Android.Platform;

namespace Ultraviolet.Android
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