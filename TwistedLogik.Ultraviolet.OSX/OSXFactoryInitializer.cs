using System;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.OSX
{
	internal sealed class OSXFactoryInitializer : IUltravioletFactoryInitializer
	{   
		/// <inheritdoc/>
		public void Initialize(UltravioletContext owner, UltravioletFactory factory)
		{
			MonoMac.AppKit.NSApplication.Init();

			factory.RemoveFactoryMethod<SurfaceSourceFactory>();
			factory.RemoveFactoryMethod<SurfaceSaverFactory>();
			factory.RemoveFactoryMethod<IconLoaderFactory>();
			factory.RemoveFactoryMethod<ScreenDensityServiceFactory>();

			factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new OSXSurfaceSource(stream));
			factory.SetFactoryMethod<SurfaceSaverFactory>(() => new OSXSurfaceSaver());
			factory.SetFactoryMethod<IconLoaderFactory>(() => new OSXIconLoader());
			factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new OSXScreenDensityService(display));
		}
	}
}