using System;
using Ultraviolet.Platform;
using static Ultraviolet.SDL2.Native.SDLNative;

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
                if (SDL_HasClipboardText())
                {
                    var text = SDL_GetClipboardText();
                    if (text == null)
                        throw new SDL2Exception();

                    return text;
                }
                return null;
            }
            set { SDL_SetClipboardText(value); }
        }
    }
}
