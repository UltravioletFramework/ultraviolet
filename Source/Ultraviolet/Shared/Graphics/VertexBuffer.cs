using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
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
            Contract.Require(vdecl, nameof(vdecl));
            Contract.EnsureRange(vcount > 0, nameof(vcount));

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
            Contract.Require(vdecl, nameof(vdecl));
            Contract.EnsureRange(vcount > 0, nameof(vcount));

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
            Contract.EnsureRange(vcount > 0, nameof(vcount));

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
        /// Sets the vertex buffer's data from the data at the specified pointer.
        /// </summary>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="srcOffsetInBytes">The offset from the beginning of the source data, in bytes, at which to begin copying.</param>
        /// <param name="dstOffsetInBytes">The offset from the beginning of the vertex buffer, in bytes, at which to begin copying.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public abstract void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes, SetDataOptions options);

        /// <summary>
        /// Sets the data contained by the vertex buffer, allowing the driver to align the data in such a way as to
        /// optimize the speed of the operation, perhaps at the cost of video memory.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the vertex buffer.</param>
        /// <param name="dataOffset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="dataCount">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="bufferOffset">The offset into the vertex buffer at which to begin uploading vertex data.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer region to which the data was uploaded. This may be larger than 
        /// is strictly required by the uploaded data due to driver-specific alignment concerns.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public abstract void SetDataAligned<T>(T[] data, Int32 dataOffset, Int32 dataCount, Int32 bufferOffset, out Int32 bufferSize, SetDataOptions options) where T : struct;

        /// <summary>
        /// Gets the size of the smallest buffer region which can be allocated by a call to the <see cref="SetDataAligned{T}"/> method.
        /// </summary>
        /// <returns>The size, in bytes, of the smallest possible aligned buffer region.</returns>
        public abstract Int32 GetAlignmentUnit();

        /// <summary>
        /// Gets the size of the buffer region which will be allocated by a call to the <see cref="SetDataAligned{T}"/> method.
        /// </summary>
        /// <param name="count">The number of vertices which will be written into the vertex buffer.</param>
        /// <returns>The size, in bytes, of the aligned buffer region which will be allocated for the specified number of vertices.</returns>
        public abstract Int32 GetAlignedSize(Int32 count);

        /// <summary>
        /// Gets the buffer's vertex declaration.
        /// </summary>
        public VertexDeclaration VertexDeclaration => vertexDeclaration;

        /// <summary>
        /// Gets the buffer's vertex count.
        /// </summary>
        public Int32 VertexCount => vertexCount;

        /// <summary>
        /// Gets the buffer's size in bytes.
        /// </summary>
        public Int32 SizeInBytes => vertexCount * vertexDeclaration.VertexStride;

        // Property values.
        private readonly VertexDeclaration vertexDeclaration;
        private readonly Int32 vertexCount;
    }
}
