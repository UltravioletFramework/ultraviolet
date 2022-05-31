using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Graphics;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="Cursor"/> class.
    /// </summary>
    public unsafe sealed class SDL2Cursor : Cursor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Cursor"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="surface">The surface that contains the cursor image.</param>
        /// <param name="hx">The x-coordinate of the cursor's hotspot.</param>
        /// <param name="hy">The y-coordinate of the cursor's hotspot.</param>
        public SDL2Cursor(UltravioletContext uv, Surface2D surface, Int32 hx, Int32 hy)
            : base(uv)
        {
            Contract.Require(surface, nameof(surface));

            uv.ValidateResource(surface);

            if (uv.Platform != UltravioletPlatform.Android && uv.Platform != UltravioletPlatform.iOS)
            {
                this.cursor = SDL_CreateColorCursor(((SDL2Surface2D)surface).NativePtr, hx, hy);
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
        public SDL_Cursor* Native =>
            Disposed ? throw new ObjectDisposedException(GetType().Name) : cursor;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (!Ultraviolet.Disposed && Ultraviolet.GetPlatform().Cursor == this)
                Ultraviolet.GetPlatform().Cursor = null;

            SDL_FreeCursor(cursor);
            cursor = null;

            base.Dispose(disposing);
        }

        // The native SDL2 cursor.
        private SDL_Cursor* cursor;
    }
}
