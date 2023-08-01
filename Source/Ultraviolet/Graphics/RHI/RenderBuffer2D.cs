using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="RenderBuffer2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="format">The render buffer's format.</param>
    /// <param name="width">The render buffer's width in pixels.</param>
    /// <param name="height">The render buffer's height in pixels.</param>
    /// <param name="options">The render buffer's configuration options.</param>
    /// <returns>The instance of <see cref="RenderBuffer2D"/> that was created.</returns>
    public delegate RenderBuffer2D RenderBuffer2DFactory(UltravioletContext uv, RenderBufferFormat format, Int32 width, Int32 height, RenderBufferOptions options);

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
        /// Gets the render buffer's format.
        /// </summary>
        public abstract RenderBufferFormat Format
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the render buffer has been attached to a render target.
        /// </summary>
        public abstract Boolean Attached
        {
            get;
        }
    }
}
