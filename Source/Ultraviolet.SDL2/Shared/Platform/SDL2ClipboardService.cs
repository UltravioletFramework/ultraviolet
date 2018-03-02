using System;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="ClipboardService"/> class.
    /// </summary>
    public sealed class SDL2ClipboardService : ClipboardService
    {
        /// <inheritdoc/>
        public override String Text
        {
            get
            {
                if (SDLNative.SDL_HasClipboardText())
                {
                    var text = SDLNative.SDL_GetClipboardText();
                    if (text == null)
                        throw new SDL2Exception();

                    return text;
                }
                return null;
            }
            set { SDLNative.SDL_SetClipboardText(value); }
        }
    }
}
