using System;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents an exception thrown as a result of an SDL2 API error.
    /// </summary>
    [Serializable]
    public sealed class SDL2Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SDL2Exception class.
        /// </summary>
        public SDL2Exception()
            : base(SDL_GetError())
        {

        }
    }
}
