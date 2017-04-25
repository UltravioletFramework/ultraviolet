using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Shims.macOS.Graphics;
using Ultraviolet.Shims.macOS.Input;
using Ultraviolet.Shims.macOS.Platform;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.macOS
{
    [Preserve(AllMembers = true)]
    internal sealed class macOSFactoryInitializer : IUltravioletFactoryInitializer
	{   
		/// <inheritdoc/>
		public void Initialize(UltravioletContext owner, UltravioletFactory factory)
		{
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new macOSSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new macOSSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new macOSIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new macOSScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new macOSScreenDensityService(display));

            var softwareKeyboardService = new macOSSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
		}
	}
}