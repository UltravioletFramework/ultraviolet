using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Graphics;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL implementation of the Cursor class.
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
        [Preserve]
        public OpenGLCursor(UltravioletContext uv, Surface2D surface, Int32 hx, Int32 hy)
            : base(uv)
        {
            Contract.Require(surface, nameof(surface));

            uv.ValidateResource(surface);

            if (AreCursorsSupported(uv))
            {
                this.cursor = SDL.CreateColorCursor(((OpenGLSurface2D)surface).Native, hx, hy);
                this.Width = surface.Width;
                this.Height = surface.Height;
                this.HotspotX = hx;
                this.HotspotY = hy;

                if (this.cursor == null)
                {
                    this.Width = 0;
                    this.Height = 0;
                    this.HotspotX = 0;
                    this.HotspotY = 0;
                }
            }
            else
            {
                this.cursor = null;
                this.Width = 0;
                this.Height = 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether cursors are supported on the current platform.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns><see langword="true"/> if cursors are supported; otherwise, <see langword="false"/>.</returns>
        public static Boolean AreCursorsSupported(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            return 
                uv.Platform != UltravioletPlatform.Android && 
                uv.Platform != UltravioletPlatform.iOS;
        }

        /// <inhertidoc/>
        public override Int32 Width { get; }

        /// <inhertidoc/>
        public override Int32 Height { get; }

        /// <inhertidoc/>
        public override Int32 HotspotX { get; }

        /// <inhertidoc/>
        public override Int32 HotspotY { get; }

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
    }
}
