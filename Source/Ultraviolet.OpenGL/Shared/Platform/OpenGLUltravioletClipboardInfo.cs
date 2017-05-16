using System;
using Ultraviolet.Platform;
using Ultraviolet.SDL2;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL implementation of the IUltravioletClipboardInfo interface.
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
