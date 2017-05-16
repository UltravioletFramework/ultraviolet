using Ultraviolet.Core;
using Ultraviolet.SDL2.Platform;
using Ultraviolet.Platform;

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
