using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Graphics;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the Cursor class.
    /// </summary>
    public unsafe sealed class OpenGLCursor : Cursor
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLCursor class.
        /// </summary>
        /// <param name="uv">The UltravioletContext class.</param>
        /// <param name="surface">The surface that contains the cursor image.</param>
        /// <param name="hx">The x-coordinate of the cursor's hotspot.</param>
        /// <param name="hy">The y-coordinate of the cursor's hotspot.</param>
        public OpenGLCursor(UltravioletContext uv, Surface2D surface, Int32 hx, Int32 hy)
            : base(uv)
        {
            Contract.Require(surface, "surface");

            uv.ValidateResource(surface);

            if (AreCursorsSupported(uv))
            {
                this.cursor = SDL.CreateColorCursor(((OpenGLSurface2D)surface).Native, hx, hy);
                this.width  = surface.Width;
                this.height = surface.Height;

                if (this.cursor == null)
                {
                    throw new SDL2Exception();
                }
            }
            else
            {
                this.cursor = null;
                this.width  = 0;
                this.height = 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether cursors are supported on the current platform.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns><c>true</c> if cursors are supported; otherwise, <c>false</c>.</returns>
        public static Boolean AreCursorsSupported(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            return uv.Platform != UltravioletPlatform.Android;
        }

        /// <inhertidoc/>
        public override Int32 Width
        {
            get { return width; }
        }

        /// <inhertidoc/>
        public override Int32 Height
        {
            get { return height; }
        }

        /// <summary>
        /// Gets a pointer to the native SDL2 cursor.
        /// </summary>
        public SDL_Cursor* Native
        {
            get { return cursor; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (!Ultraviolet.Disposed && Ultraviolet.GetPlatform().Cursor == this)
            {
                Ultraviolet.GetPlatform().Cursor = null;
            }

            SDL.FreeCursor(cursor);

            base.Dispose(disposing);
        }

        // The native SDL2 cursor.
        private readonly SDL_Cursor* cursor;

        // Property values.
        private readonly Int32 width;
        private readonly Int32 height;
    }
}
