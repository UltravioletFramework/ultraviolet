using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.OSX.Graphics;
using Ultraviolet.OSX.Input;
using Ultraviolet.OSX.Platform;
using Ultraviolet.Platform;

namespace Ultraviolet.OSX
{
    [Preserve(AllMembers = true)]
    internal sealed class OSXFactoryInitializer : IUltravioletFactoryInitializer
	{   
		/// <inheritdoc/>
		public void Initialize(UltravioletContext owner, UltravioletFactory factory)
		{
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new OSXSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new OSXSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new OSXIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new OSXScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new OSXScreenDensityService(display));

            var softwareKeyboardService = new OSXSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
		}
	}
}