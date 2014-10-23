using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="VertexBuffer"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="vdecl">The vertex declaration for the buffer.</param>
    /// <param name="vcount">The number of vertices in the buffer.</param>
    /// <returns>The instance of <see cref="VertexBuffer"/> that was created.</returns>
    public delegate VertexBuffer VertexBufferFactory(UltravioletContext uv, VertexDeclaration vdecl, Int32 vcount);

    /// <summary>
    /// Represents a buffer containing vertex data.
    /// </summary>
    public abstract class VertexBuffer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="vdecl">The vertex declaration for the buffer.</param>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        protected VertexBuffer(UltravioletContext uv, VertexDeclaration vdecl, Int32 vcount)
            : base(uv)
        {
            Contract.Require(vdecl, "vdecl");
            Contract.EnsureRange(vcount > 0, "vcount");

            this.vertexDeclaration = vdecl;
            this.vertexCount = vcount;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="vdecl">The vertex declaration for the buffer.</param>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        /// <returns>The instance of <see cref="VertexBuffer"/> that was created.</returns>
        public static VertexBuffer Create(VertexDeclaration vdecl, Int32 vcount)
        {
            Contract.Require(vdecl, "vdecl");
            Contract.EnsureRange(vcount > 0, "vcount");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<VertexBufferFactory>()(uv, vdecl, vcount);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <typeparam name="T">The vertex type that defines the layout of the buffer's vertices.</typeparam>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        /// <returns>The instance of <see cref="VertexBuffer"/> that was created.</returns>
        public static VertexBuffer Create<T>(Int32 vcount) where T : struct, IVertexType
        {
            Contract.EnsureRange(vcount > 0, "vcount");

            var vdecl = new T().VertexDeclaration;

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<VertexBufferFactory>()(uv, vdecl, vcount);
        }

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the vertex buffer.</param>
        public abstract void SetData<T>(T[] data) where T : struct;

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the vertex buffer.</param>
        /// <param name="offset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="count">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public abstract void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options) where T : struct;

        /// <summary>
        /// Gets the buffer's vertex declaration.
        /// </summary>
        public VertexDeclaration VertexDeclaration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
 
                return vertexDeclaration; 
            }
        }

        /// <summary>
        /// Gets the buffer's vertex count.
        /// </summary>
        public Int32 VertexCount
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
 
                return vertexCount; 
            }
        }

        // Property values.
        private readonly VertexDeclaration vertexDeclaration;
        private readonly Int32 vertexCount;
    }
}
