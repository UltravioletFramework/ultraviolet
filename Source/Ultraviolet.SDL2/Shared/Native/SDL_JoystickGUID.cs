using System;

namespace Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Contains methods relating to the SDL_JoystickGUID native structure.
    /// </summary>
    internal struct SDL_JoystickGUID
    {
        /// <summary>
        /// Marshals between <see cref="System.Guid"/> and SDL_JoystickGUID.
        /// </summary>
        internal static Guid Marshal(Guid guid)
        {
            unsafe
            {
                var pGuid = (Byte*)&guid;
                return new Guid(
                    (Int32)(pGuid[0] << 24 | pGuid[1] << 16 | pGuid[2] << 8 | pGuid[3]),
                    (Int16)(pGuid[4] << 8 | pGuid[5]),
                    (Int16)(pGuid[6] << 8 | pGuid[7]),
                    pGuid[8], pGuid[9], pGuid[10], pGuid[11], pGuid[12], pGuid[13], pGuid[14], pGuid[15]
                );
            }
        }
    }
}
