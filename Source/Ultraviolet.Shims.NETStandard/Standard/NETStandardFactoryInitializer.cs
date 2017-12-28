using Ultraviolet.Core;
using Ultraviolet.Shims.NETStandard.Graphics;
using Ultraviolet.Shims.NETStandard.Input;
using Ultraviolet.Shims.NETStandard.Platform;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETStandard
{
    /// <summary>
    /// Initializes factory methods for the .NET Standard 2.0 platform compatibility shim.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class NETStandardFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new NETStandardSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new NETStandardSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new NETStandardIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new NETStandardScreenOrientationService(display));
            factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new NETStandardScreenDensityService(owner, display));

            var softwareKeyboardService = new NETStandardSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
