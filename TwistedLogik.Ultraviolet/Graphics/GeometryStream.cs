using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="GeometryStream"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="GeometryStream"/> that was created.</returns>
    public delegate GeometryStream GeometryStreamFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a buffer containing references to vertex and index data and which
    /// can stream geometry to the graphics device.
    /// </summary>
    public abstract class GeometryStream : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryStream"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public GeometryStream(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="GeometryStream"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="GeometryStream"/> that was created.</returns>
        public static GeometryStream Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<GeometryStreamFactory>()(uv);
        }

        /// <summary>
        /// Attaches a vertex buffer to the geometry stream.
        /// </summary>
        /// <param name="vbuffer">The <see cref="VertexBuffer"/> to attach to the geometry stream.</param>
        public abstract void Attach(VertexBuffer vbuffer);

        /// <summary>
        /// Attaches an index buffer to the geometry stream.
        /// </summary>
        /// <param name="ibuffer">The <see cref="IndexBuffer"/> to attach to the geometry buffer.</param>
        public abstract void Attach(IndexBuffer ibuffer);

        /// <summary>
        /// Gets a value indicating whether the geometry stream is in a valid state for rendering.
        /// </summary>
        public virtual Boolean IsValid
        {
            get { return HasVertices; }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry stream reads data from multiple vertex buffers.
        /// </summary>
        public abstract Boolean HasMultipleSources
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the geometry stream has any vertex buffers attached to it.
        /// </summary>
        public abstract Boolean HasVertices
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the geometry stream has any index buffers attached to it.
        /// </summary>
        public abstract Boolean HasIndices
        {
            get;
        }

        /// <summary>
        /// Gets the type of the elements in the stream's index buffer, if it has an index buffer.
        /// </summary>
        public abstract IndexBufferElementType IndexBufferElementType
        {
            get;
        }
    }
}
