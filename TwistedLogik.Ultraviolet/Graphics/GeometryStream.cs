using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the GeometryStream class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of GeometryStream that was created.</returns>
    public delegate GeometryStream GeometryStreamFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a buffer containing references to vertex and index data and which
    /// can stream geometry to the rendering engine.
    /// </summary>
    public abstract class GeometryStream : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the GeometryStream class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public GeometryStream(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the GeometryStream class.
        /// </summary>
        /// <returns>The instance of GeometryStream that was created.</returns>
        public static GeometryStream Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<GeometryStreamFactory>()(uv);
        }

        /// <summary>
        /// Attaches a vertex buffer to the geometry buffer.
        /// </summary>
        /// <param name="vbuffer">The vertex buffer to attach to the geometry buffer.</param>
        public abstract void Attach(VertexBuffer vbuffer);

        /// <summary>
        /// Attaches an index buffer to the geometry buffer.
        /// </summary>
        /// <param name="ibuffer">The index buffer to attach to the geometry buffer.</param>
        public abstract void Attach(IndexBuffer ibuffer);

        /// <summary>
        /// Gets a value indicating whether the geometry buffer is in a valid state for rendering.
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
        /// Gets a value indicating whether the geometry buffer has any vertex buffers attached to it.
        /// </summary>
        public abstract Boolean HasVertices
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the geometry buffer has any index buffers attached to it.
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
