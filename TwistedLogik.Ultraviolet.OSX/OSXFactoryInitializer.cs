using Ultraviolet.Core;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.OSX.Graphics;
using TwistedLogik.Ultraviolet.OSX.Input;
using TwistedLogik.Ultraviolet.OSX.Platform;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.OSX
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