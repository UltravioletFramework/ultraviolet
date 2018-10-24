using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="IndexBuffer"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="itype">The index element type.</param>
    /// <param name="icount">The index element count.</param>
    /// <returns>The instance of <see cref="IndexBuffer"/> that was created.</returns>
    public delegate IndexBuffer IndexBufferFactory(UltravioletContext uv, IndexBufferElementType itype, Int32 icount);

    /// <summary>
    /// Represents an index buffer.
    /// </summary>
    public abstract class IndexBuffer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="itype">The index element type.</param>
        /// <param name="icount">The index element count.</param>
        protected IndexBuffer(UltravioletContext uv, IndexBufferElementType itype, Int32 icount)
            : base(uv)
        {
            Contract.EnsureRange(icount > 0, nameof(icount));

            this.itype = itype;
            this.icount = icount;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="itype">The index element type.</param>
        /// <param name="icount">The index element count.</param>
        /// <returns>The instance of <see cref="IndexBuffer"/> that was created.</returns>
        public static IndexBuffer Create(IndexBufferElementType itype, Int32 icount)
        {
            Contract.EnsureRange(icount > 0, nameof(icount));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<IndexBufferFactory>()(uv, itype, icount);
        }

        /// <summary>
        /// Sets the data contained by the index buffer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the index buffer.</param>
        public abstract void SetData<T>(T[] data);

        /// <summary>
        /// Sets the data contained by the index buffer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the index buffer.</param>
        /// <param name="offset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="count">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public abstract void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options);

        /// <summary>
        /// Sets the index buffer's data from the data at the specified pointer.
        /// </summary>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="srcOffsetInBytes">The offset from the beginning of the source data, in bytes, at which to begin copying.</param>
        /// <param name="dstOffsetInBytes">The offset from the beginning of the index buffer, in bytes, at which to begin copying.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public abstract void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes, SetDataOptions options);

        /// <summary>
        /// Sets the data contained by the index buffer, allowing the driver to align the data in such a way as to
        /// optimize the speed of the operation, perhaps at the cost of video memory.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array to set as the buffer's data.</typeparam>
        /// <param name="data">An array containing the data to set in the index buffer.</param>
        /// <param name="dataOffset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="dataCount">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="bufferOffset">The offset into the index buffer at which to begin uploading index data.</param>
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
        /// <param name="count">The number of indices which will be written into the index buffer.</param>
        /// <returns>The size, in bytes, of the aligned buffer region which will be allocated for the specified number of indices.</returns>
        public abstract Int32 GetAlignedSize(Int32 count);

        /// <summary>
        /// Gets the buffer's element type.
        /// </summary>
        public IndexBufferElementType IndexElementType => itype;

        /// <summary>
        /// Gets the buffer's index count.
        /// </summary>
        public Int32 IndexCount => icount;
        
        /// <summary>
        /// Gets the buffer's size in bytes.
        /// </summary>
        public Int32 SizeInBytes => icount * ((IndexElementType == IndexBufferElementType.Int16) ? sizeof(UInt16) : sizeof(UInt32));

        // Property values.
        private readonly IndexBufferElementType itype;
        private readonly Int32 icount;
    }
}
