using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="RenderBuffer2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="format">The render buffer's format.</param>
    /// <param name="width">The render buffer's width in pixels.</param>
    /// <param name="height">The render buffer's height in pixels.</param>
    /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
    public delegate RenderBuffer2D RenderBuffer2DFactory(UltravioletContext uv, RenderBufferFormat format, Int32 width, Int32 height);

    /// <summary>
    /// Represents a two-dimensional render buffer containing color, depth, or stencil data.
    /// </summary>
    public abstract class RenderBuffer2D : Texture2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBuffer2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public RenderBuffer2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="RenderBuffer2D"/> that represents a color buffer.
        /// </summary>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
        public static new RenderBuffer2D Create(Int32 width, Int32 height)
        {
            return Create(RenderBufferFormat.Color, width, height);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RenderBuffer2D"/> class.
        /// </summary>
        /// <param name="format">A <see cref="RenderBufferFormat"/> value specifying the render buffer's data format.</param>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
        public static RenderBuffer2D Create(RenderBufferFormat format, Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<RenderBuffer2DFactory>()(uv, format, width, height);
        }

        /// <summary>
        /// Gets the render buffer's format.
        /// </summary>
        public abstract RenderBufferFormat Format
        {
            get;
        }
    }
}
