using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
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
        /// Provides a hint to the graphics driver that the contents of one or more of this render target's attached
        /// render buffers will not be required for further rendering and may be discarded. For example, you can call 
        /// this method to indicate that a depth buffer which is only required for initially populating the render 
        /// target is not required by any subsequent calls. This can potentially improve memory bandwidth.
        /// </summary>
        /// <param name="color">A value indicating whether one or more of the attached color buffers should be invalidated.</param>
        /// <param name="depth">A value indicating whether the attached depth buffer should be invalidated.</param>
        /// <param name="stencil">A value indicating whether the attached stencil buffer should be invalidated.</param>
        /// <param name="depthStencil">A value indicating whether the attached depth/stencil buffer should be invalidated.</param>
        /// <param name="colorOffset">The offset of the first color buffer to invalidate, if <paramref name="color"/> is <see langword="true"/>.</param>
        /// <param name="colorCount">The number of color buffers to invalidate, if <paramref name="color"/> is <see langword="true"/>.</param>
        /// <remarks>For best effect, call this method after rendering to the target, but prior to unbinding 
        /// the target from the graphics device using <see cref="IUltravioletGraphics.SetRenderTarget(RenderTarget2D)"/>.</remarks>
        public abstract void Invalidate(Boolean color, Boolean depth, Boolean stencil, Boolean depthStencil, Int32 colorOffset, Int32 colorCount);

        /// <summary>
        /// Provides a hint to the graphics driver that the contents of one or more of this render target's attached
        /// render buffers will not be required for further rendering and may be discarded. See the documentation
        /// for the <see cref="Invalidate(bool, bool, bool, bool, int, int)"/> overload for more information.
        /// </summary>
        /// <param name="offset">The offset of the first color buffer to invalidate.</param>
        /// <param name="count">The number of color buffers to invalidate.</param>
        public void InvalidateColor(Int32 offset, Int32 count) =>
            Invalidate(true, false, false, false, offset, count);

        /// <summary>
        /// Provides a hint to the graphics driver that the contents of this render target's attached
        /// depth buffer will not be required for further rendering and may be discarded. See the documentation
        /// for the <see cref="Invalidate(bool, bool, bool, bool, int, int)"/> overload for more information.
        /// </summary>
        public void InvalidateDepth() =>
            Invalidate(false, true, false, false, 0, 0);

        /// <summary>
        /// Provides a hint to the graphics driver that the contents of this render target's attached
        /// stencil buffer will not be required for further rendering and may be discarded. See the documentation
        /// for the <see cref="Invalidate(bool, bool, bool, bool, int, int)"/> overload for more information.
        /// </summary>
        public void InvalidateStencil() =>
            Invalidate(false, false, true, false, 0, 0);

        /// <summary>
        /// Provides a hint to the graphics driver that the contents of this render target's attached
        /// depth/stencil buffer will not be required for further rendering and may be discarded. See the documentation
        /// for the <see cref="Invalidate(bool, bool, bool, bool, int, int)"/> overload for more information.
        /// </summary>
        public void InvalidateDepthStencil() =>
            Invalidate(false, false, false, false, 0, 0);

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
        /// Gets a value indicating whether the render target has an sRGB encoded color buffer.
        /// </summary>
        public abstract Boolean HasSrgbEncodedColorBuffer { get; }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached depth buffer.
        /// </summary>
        public abstract Boolean HasDepthBuffer { get; }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached stencil buffer.
        /// </summary>
        public abstract Boolean HasStencilBuffer { get; }

        /// <summary>
        /// Gets a value indicating whether the render target has an attached depth/stencil buffer.
        /// </summary>
        public abstract Boolean HasDepthStencilBuffer { get; }

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
