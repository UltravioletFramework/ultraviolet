using System;

namespace Ultraviolet.Graphics
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
        /// Attaches a vertex buffer to the geometry stream with the specified instance frequency.
        /// </summary>
        /// <param name="vbuffer">The <see cref="VertexBuffer"/> to attach to the geometry stream.</param>
        /// <param name="instanceFrequency">The number of instances which are drawn with each set of
        /// vertex data in the buffer, or 0 to disable instancing.</param>
        public abstract void Attach(VertexBuffer vbuffer, Int32 instanceFrequency);

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
        /// Gets a value indicating whether this geometry stream has any vertex position data.
        /// </summary>
        public abstract Boolean HasVertexPosition { get; }

        /// <summary>
        /// Gets a value indicating whether this geometry stream has any vertex color data.
        /// </summary>
        public abstract Boolean HasVertexColor { get; }

        /// <summary>
        /// Gets a value indicating whether this geometry stream has any vertex texture data.
        /// </summary>
        public abstract Boolean HasVertexTexture { get; }

        /// <summary>
        /// Gets a value indicating whether this geometry stream has any vertex normal data.
        /// </summary>
        public abstract Boolean HasVertexNormal { get; }

        /// <summary>
        /// Gets a value indicating whether this geometry stream has any vertex tangent data.
        /// </summary>
        public abstract Boolean HasVertexTangent { get; }

        /// <summary>
        /// Gets a value indicating whether this geometry stream has any vertex blending data.
        /// </summary>
        public abstract Boolean HasVertexBlend { get; }

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
