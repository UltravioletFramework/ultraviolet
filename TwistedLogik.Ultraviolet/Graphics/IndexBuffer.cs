using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
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
        public IndexBufferElementType IndexElementType
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return itype; 
            }
        }

        /// <summary>
        /// Gets the buffer's index count.
        /// </summary>
        public Int32 IndexCount
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return icount; 
            }
        }
        
        /// <summary>
        /// Gets the buffer's size in bytes.
        /// </summary>
        public Int32 SizeInBytes
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return icount * ((IndexElementType == IndexBufferElementType.Int16) ? sizeof(short) : sizeof(int));
            }
        }

        // Property values.
        private readonly IndexBufferElementType itype;
        private readonly Int32 icount;
    }
}
