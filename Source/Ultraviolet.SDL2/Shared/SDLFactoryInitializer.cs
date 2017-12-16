using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Graphics;
using Ultraviolet.SDL2.Platform;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.SDL2
{

    /// <summary>
    /// Initializes factory methods for the SDL implementation of the graphics subsystem.
    /// </summary>
    [Preserve(AllMembers = true)]
    public sealed class SDLFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            // Core classes.
            factory.SetFactoryMethod<PlatformNativeSurfaceFactory>((source) => new SDL2PlatformNativeSurface(source));
            factory.SetFactoryMethod<Surface2DFactory>((uv, width, height) => new SDL2Surface2D(uv, width, height));
            factory.SetFactoryMethod<Surface2DFromSourceFactory>((uv, source) => new SDL2Surface2D(uv, source));
            factory.SetFactoryMethod<Surface2DFromNativeSurfaceFactory>((uv, surface) => new SDL2Surface2D(uv, surface));
            factory.SetFactoryMethod<Surface3DFactory>((uv, width, height, depth, bytesPerPixel) => new SDL2Surface3D(uv, width, height, depth, bytesPerPixel));
            factory.SetFactoryMethod<CursorFactory>((uv, surface, hx, hv) => new SDL2Cursor(uv, surface, hx, hv));

            // Platform services
            var msgboxService = new SDL2MessageBoxService();
            factory.SetFactoryMethod<MessageBoxServiceFactory>(() => msgboxService);
            
            var clipboardService = new SDL2ClipboardService();
            factory.SetFactoryMethod<ClipboardServiceFactory>(() => clipboardService);

            var powerManagementService = new SDL2PowerManagementService();
            factory.SetFactoryMethod<PowerManagementServiceFactory>(() => powerManagementService);
        }
    }
}
