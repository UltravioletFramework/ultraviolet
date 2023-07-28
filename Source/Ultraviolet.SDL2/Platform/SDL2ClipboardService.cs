using System;
using System.Runtime.InteropServices;
using System.Text;
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
                    var ptr = SDL_GetClipboardText();
                    if (ptr == IntPtr.Zero)
                        throw new SDL2Exception();

                    try
                    {
                        var length = 0;
                        while (Marshal.ReadByte(ptr, length) != 0)
                            length++;

                        var utf8 = new Byte[length];
                        Marshal.Copy(ptr, utf8, 0, utf8.Length);

                        var utf16 = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8);
                        var result = Encoding.Unicode.GetString(utf16);

                        return result;
                    }
                    finally { SDL_free(ptr); }
                }
                return null;
            }
            set
            {
                var utf8ByteCount = Encoding.UTF8.GetByteCount(value);
                var utf8BufManaged = new Byte[utf8ByteCount];
                var utf8BufNative = Marshal.AllocHGlobal(utf8BufManaged.Length);
                try
                {
                    Encoding.UTF8.GetBytes(value, 0, value.Length, utf8BufManaged, 0);
                    Marshal.Copy(utf8BufManaged, 0, utf8BufNative, utf8BufManaged.Length);
                    SDL_SetClipboardText(utf8BufNative);
                }
                finally
                {
                    if (utf8BufNative != IntPtr.Zero)
                        Marshal.FreeHGlobal(utf8BufNative);
                }
            }
        }
    }
}
