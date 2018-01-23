using Ultraviolet.Core;
using Ultraviolet.Shims.NETCore.Graphics;
using Ultraviolet.Shims.NETCore.Input;
using Ultraviolet.Shims.NETCore.Platform;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.NETCore
{
    /// <summary>
    /// Initializes factory methods for the .NET Standard 2.0 platform compatibility shim.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class NETCoreFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <summary>
        /// Initializes the specified factory.
        /// </summary>
        /// <param name="owner">The Ultraviolet context that owns the initializer.</param>
        /// <param name="factory">The <see cref="UltravioletFactory"/> to initialize.</param>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            // .NET Core doesn't have a reliable way to do certain things yet without pulling in a bunch 
            // of extra dependencies on some platforms, so fall back to the UV implementation if it's available.
            factory.SetFactoryMethod(factory.TryGetFactoryMethod<ScreenDensityServiceFactory>("ImplFallback") ??
                ((display) => new NETCoreScreenDensityService(owner, display)));

            factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new NETCoreSurfaceSource(stream));
            factory.SetFactoryMethod<SurfaceSaverFactory>(() => new NETCoreSurfaceSaver());
            factory.SetFactoryMethod<IconLoaderFactory>(() => new NETCoreIconLoader());
            factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
            factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new NETCoreScreenOrientationService(display));

            var softwareKeyboardService = new NETCoreSoftwareKeyboardService();
            factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
        }
    }
}
