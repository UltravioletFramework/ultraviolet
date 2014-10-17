using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents the render handler for the CEF client.
    /// </summary>
    internal sealed unsafe class XiliumCefRenderHandler : CefRenderHandler, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the XiliumCefRenderHandler class.
        /// </summary>
        /// <param name="x">The x-coordinate of the browser's initial position.</param>
        /// <param name="y">The y-coordinate of the browser's initial position.</param>
        /// <param name="width">The browser's initial width in pixels.</param>
        /// <param name="height">The browser's initial height in pixels.</param>
        public XiliumCefRenderHandler(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            this.nullTexture = Texture2D.Create(1, 1);
            this.nullTexture.SetData(new Color[] { Color.White });

            CreateRenderTarget();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Draws the client's web view.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the client's web view.</param>
        /// <param name="color">The color with which to tint the layout.</param>
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.EnsureNotDisposed(this, Disposed);

            var texture = readyToRender ? viewTexture : nullTexture;
            spriteBatch.Draw(texture, new RectangleF(x, y, width, height), null, color, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
        }

        /// <summary>
        /// Gets or sets the position of the renderer's view area relative to the screen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return new Vector2(x, y);
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                x = (int)value.X;
                y = (int)value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the renderer's view area relative to the screen.
        /// </summary>
        public Int32 X
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return x;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                this.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the renderer's view area relative to the screen.
        /// </summary>
        public Int32 Y
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return y;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                this.y = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the renderer's view area in pixels.
        /// </summary>
        public Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return new Size2(width, height);
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value.Width > 0 && value.Height > 0, "value");

                if (this.width != value.Width || this.height != value.Width)
                {
                    this.width = value.Width;
                    this.height = value.Height;
                    CreateRenderTarget();
                }
            }
        }

        /// <summary>
        /// Gets the width of the renderer's view area in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return width;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value > 0, "value");

                if (this.width != value)
                {
                    this.width = value;
                    CreateRenderTarget();
                }
            }
        }

        /// <summary>
        /// Gets the height of the renderer's view area in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return height;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value > 0, "value");

                if (this.height != value)
                {
                    this.height = value;
                    CreateRenderTarget();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Called to allow the client to fill in the CefScreenInfo object with appropriate values.
        /// </summary>
        protected override Boolean GetScreenInfo(CefScreenInfo screenInfo)
        {
            return false;
        }

        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen
        /// coordinates. Return true if the screen coordinates were provided.
        /// </summary>
        protected override Boolean GetScreenPoint(int viewX, int viewY, ref int screenX, ref int screenY)
        {
            screenX = x + viewX;
            screenY = y + viewY;
            return true;
        }

        /// <summary>
        /// Called to retrieve the view rectangle which is relative to screen coordinates.
        /// Return true if the rectangle was provided.
        /// </summary>
        protected override Boolean GetViewRect(ref CefRectangle rect)
        {
            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;
            return true;
        }

        /// <summary>
        /// Called when the browser's cursor has changed.
        /// </summary>
        protected override void OnCursorChange(IntPtr cursorHandle)
        {

        }

        /// <summary>
        /// Called when the browser view is being painted.
        /// </summary>
        protected override void OnPaint(CefPaintElementType type, CefRectangle dirtyRect, IntPtr buffer, Int32 width, Int32 height)
        {
            if (viewTexture != null && viewTexture.Width == width && viewTexture.Height == height)
            {
                var data = (uint*)buffer + (dirtyRect.Y * width + dirtyRect.X);
                var dataRect = new Rectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
                viewTexture.SetData(0, dataRect, (IntPtr)data, 0, dirtyRect.Width * dirtyRect.Height * 4, width, TextureDataFormat.BGRA);
            }
            readyToRender = true;
        }

        /// <summary>
        /// Called when the browser wants to move or resize the popup widget.
        /// </summary>
        protected override void OnPopupSize(CefRectangle rect)
        {

        }

        /// <summary>
        /// Called when the scroll offset has changed.
        /// </summary>
        protected override void OnScrollOffsetChanged()
        {

        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(viewTexture);
            }
            disposed = true;
            base.Dispose(disposing);
        }

        /// <summary>
        /// Recreates the browser's render target.
        /// </summary>
        private void CreateRenderTarget()
        {
            SafeDispose.Dispose(viewTexture);
            this.viewTexture = RenderBuffer2D.Create(width, height);
            this.readyToRender = false;
        }

        // The texture to which the browser view is rendered.
        private Texture2D viewTexture;
        private Texture2D nullTexture;

        // Property values.
        private Int32 x;
        private Int32 y;
        private Int32 width;
        private Int32 height;
        private Boolean readyToRender;
        private Boolean disposed;
    }
}
