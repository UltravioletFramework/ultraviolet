﻿using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Desktop.Graphics;
using TwistedLogik.Ultraviolet.Desktop.Input;
using TwistedLogik.Ultraviolet.Desktop.Platform;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop
{
    /// <summary>
    /// Initializes factory methods for the Desktop platform compatibility shim.
    /// </summary>
    internal sealed class DesktopFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopFactoryInitializer"/> class.
        /// </summary>
        [Preserve]
        public DesktopFactoryInitializer() { }

        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        [Preserve]
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new DesktopSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new DesktopSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new DesktopIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new DesktopScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new DesktopScreenDensityService(owner, display));

            var softwareKeyboardService = new DesktopSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
