using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="RenderTarget2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The render target's width in pixels.</param>
    /// <param name="height">The render target's height in pixels.</param>
    /// <param name="usage">A <see cref="RenderTargetUsage"/> value specifying whether the 
    /// render target's data is discarded or preserved when it is bound to the graphics device.</param>
    /// <returns>The instance of <see cref="RenderTarget2D"/> that was created.</returns>
    public delegate RenderTarget2D RenderTarget2DFactory(UltravioletContext uv, Int32 width, Int32 height, RenderTargetUsage usage);

    /// <summary>
    /// Represents a two-dimensional render target.
    /// </summary>
    public abstract class RenderTarget2D : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTarget2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public RenderTarget2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="RenderTarget2D"/> class.
        /// </summary>
        /// <param name="width">The render target's width in pixels.</param>
        /// <param name="height">The render target's height in pixels.</param>
        /// <param name="usage">A <see cref="RenderTargetUsage"/> value specifying whether the 
        /// render target's data is discarded or preserved when it is bound to the graphics device.</param>
        /// <returns>The instance of <see cref="RenderTarget2D"/> that was created.</returns>
        public static RenderTarget2D Create(Int32 width, Int32 height, RenderTargetUsage usage = RenderTargetUsage.DiscardContents)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<RenderTarget2DFactory>()(uv, width, height, usage);
        }

        /// <summary>
        /// Attaches a render buffer to this render target.
        /// </summary>
        /// <param name="buffer">The <see cref="RenderBuffer2D"/> to attach to this render target.</param>
        public abstract void Attach(RenderBuffer2D buffer);

        /// <summary>
        /// Resizes the render target.
        /// </summary>
        /// <param name="width">The render target's new width in pixels.</param>
        /// <param name="height">The render target's new height in pixels.</param>
        public abstract void Resize(Int32 width, Int32 height);

        /// <summary>
        /// Gets the render target's data.
        /// </summary>
        /// <param name="data">An array to populate with the render target's data.</param>
        public abstract void GetData(Color[] data);

        /// <summary>
        /// Gets the render target's data.
        /// </summary>
        /// <param name="data">An array to populate with the render target's data.</param>
        /// <param name="region">The region of the render target from which to retrieve data.</param>
        public abstract void GetData(Color[] data, Rectangle region);
        
        /// <summary>
        /// Gets the render target's size in pixels.
        /// </summary>
        public abstract Size2 Size
        {
            get;
        }

        /// <summary>
        /// Gets the render target's width in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the render target's height in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target is bound to the device for reading.
        /// </summary>
        public abstract Boolean BoundForReading
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target is bound to the device for writing.
        /// </summary>
        public abstract Boolean BoundForWriting
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached color buffer.
        /// </summary>
        public abstract Boolean HasColorBuffer
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached depth buffer.
        /// </summary>
        public abstract Boolean HasDepthBuffer
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached stencil buffer.
        /// </summary>
        public abstract Boolean HasStencilBuffer
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render target's data is preserved when
        /// it is bound to the graphics device.
        /// </summary>
        public abstract RenderTargetUsage RenderTargetUsage
        {
            get;
        }
    }
}
