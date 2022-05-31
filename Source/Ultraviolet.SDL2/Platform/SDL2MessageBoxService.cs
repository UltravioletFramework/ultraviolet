using System;
using Ultraviolet.Platform;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="MessageBoxService"/> class.
    /// </summary>
    public sealed class SDL2MessageBoxService : MessageBoxService
    {
        /// <inhertidoc/>
        public override void ShowMessageBox(MessageBoxType type, String title, String message, IntPtr window)
        {
            var flags = GetSDLMessageBoxFlag(type);

            if (SDL_ShowSimpleMessageBox(flags, title, message, window) < 0)
                throw new SDL2Exception();
        }
        
        /// <summary>
        /// Converts a <see cref="MessageBoxType"/> value to the equivalent SDL2 flag.
        /// </summary>
        private static UInt32 GetSDLMessageBoxFlag(MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.Information:
                    const UInt32 SDL_MESSAGEBOX_INFORMATION = 0x00000040;
                    return SDL_MESSAGEBOX_INFORMATION;

                case MessageBoxType.Warning:
                    const UInt32 SDL_MESSAGEBOX_WARNING = 0x00000020;
                    return SDL_MESSAGEBOX_WARNING;

                case MessageBoxType.Error:
                    const UInt32 SDL_MESSAGEBOX_ERROR = 0x00000010;
                    return SDL_MESSAGEBOX_ERROR;

                default:
                    return 0;
            }
        }
    }
}
