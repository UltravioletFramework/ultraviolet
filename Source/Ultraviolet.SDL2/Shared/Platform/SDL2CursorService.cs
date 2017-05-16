using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="CursorService"/> class.
    /// </summary>
    public sealed class SDL2CursorService : CursorService
    {
        /// <inheritdoc/>
        public override Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;

                unsafe
                {
                    var sdlCursor = (value == null) ? SDL.GetDefaultCursor() : ((SDL2Cursor)value).Native;
                    SDL.SetCursor(sdlCursor);
                }
            }
        }

        // Property values.
        private Cursor cursor;
    }
}
