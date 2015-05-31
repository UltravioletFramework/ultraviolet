using System;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletClipboardInfo interface.
    /// </summary>
    public sealed class OpenGLUltravioletClipboardInfo : IUltravioletClipboardInfo
    {
        /// <inheritdoc/>
        public String Text
        {
            get 
            {
                if (SDL.HasClipboardText())
                {
                    var text = SDL.GetClipboardText();
                    if (text == null)
                        throw new SDL2Exception();

                    return text;
                }
                return null;
            }
            set { SDL.SetClipboardText(value); }
        }
    }
}
