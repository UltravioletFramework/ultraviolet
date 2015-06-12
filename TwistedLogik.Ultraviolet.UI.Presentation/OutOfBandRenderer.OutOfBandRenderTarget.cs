using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class OutOfBandRenderer
    {
        /// <summary>
        /// Represents one of the render targets used to render an out-of-band element.
        /// </summary>
        private class OutOfBandRenderTarget : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="OutOfBandRenderTarget"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public OutOfBandRenderTarget(UltravioletContext uv)
                : base(uv)
            {
                this.colorBuffer = RenderBuffer2D.Create(1, 1, false);
 
                this.renderTarget = RenderTarget2D.Create(1, 1);
                this.renderTarget.Attach(this.colorBuffer);
            }

            /// <summary>
            /// Resizes the render target.
            /// </summary>
            /// <param name="width">The render target's new width in pixels.</param>
            /// <param name="height">The render target's new height in pixels.</param>
            public void Resize(Int32 width, Int32 height)
            {
                Contract.EnsureRange(width >= 1, "width");
                Contract.EnsureRange(height >= 1, "height");
                Contract.EnsureNotDisposed(this, Disposed);

                this.renderTarget.Resize(width, height);
            }

            /// <summary>
            /// Gets or sets a value indicating whether the render target is ready to be rendered.
            /// </summary>
            public Boolean IsReady
            {
                get
                {
                    Contract.EnsureNotDisposed(this, Disposed);

                    return isReady;
                }
                set
                {
                    Contract.EnsureNotDisposed(this, Disposed);

                    isReady = value;
                }
            }

            /// <summary>
            /// Gets the width of the render target in pixels.
            /// </summary>
            public Int32 Width
            {
                get
                {
                    Contract.EnsureNotDisposed(this, Disposed);
                    
                    return renderTarget.Width; 
                }
            }

            /// <summary>
            /// Gets the height of the render target in pixels.
            /// </summary>
            public Int32 Height
            {
                get 
                {
                    Contract.EnsureNotDisposed(this, Disposed);

                    return renderTarget.Height; 
                }
            }

            /// <summary>
            /// Gets the <see cref="RenderTarget2D"/> used to render this out-of-band element.
            /// </summary>
            public RenderTarget2D RenderTarget
            {
                get 
                {
                    Contract.EnsureNotDisposed(this, Disposed);

                    return renderTarget; 
                }
            }

            /// <summary>
            /// Gets the <see cref="RenderBuffer2D"/> that represents this element's color buffer.
            /// </summary>
            public RenderBuffer2D ColorBuffer
            {
                get
                {
                    Contract.EnsureNotDisposed(this, Disposed);

                    return colorBuffer;
                }
            }

            /// <inheritdoc/>
            protected override void Dispose(Boolean disposing)
            {
                if (disposing)
                {
                    SafeDispose.Dispose(renderTarget);
                    SafeDispose.Dispose(colorBuffer);
                }
                base.Dispose(disposing);
            }

            // Property values.
            private Boolean isReady;
            private readonly RenderTarget2D renderTarget;
            private readonly RenderBuffer2D colorBuffer;
        }
    }
}
