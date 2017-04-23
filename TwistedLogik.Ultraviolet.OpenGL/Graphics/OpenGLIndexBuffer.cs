using System;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using Ultraviolet.Core;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IndexBuffer class.
    /// </summary>
    public unsafe sealed class OpenGLIndexBuffer : DynamicIndexBuffer, IOpenGLResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLIndexBuffer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="itype">The index element type.</param>
        /// <param name="icount">The index element count.</param>
        /// <param name="usage">The buffer's usage type.</param>
        public OpenGLIndexBuffer(UltravioletContext uv, IndexBufferElementType itype, Int32 icount, UInt32 usage)
            : base(uv, itype, icount)
        {
            Contract.EnsureRange(icount >= 0, nameof(icount));

            this.usage = usage;
            this.size = new IntPtr(GetElementSize() * icount);

            var buffer = 0u;

            uv.QueueWorkItemAndWait(() =>
            {
                using (OpenGLState.ScopedCreateElementArrayBuffer(out buffer))
                {
                    gl.NamedBufferData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, size, null, usage);
                    gl.ThrowIfError();
                }
            });

            this.buffer = buffer;
        }

        /// <summary>
        /// Sets the data contained by the index buffer.
        /// </summary>
        /// <param name="data">An array containing the data to set in the index buffer.</param>
        public override void SetData<T>(T[] data)
        {
            Contract.Require(data, nameof(data));
            Contract.Ensure(data.Length <= IndexCount, OpenGLStrings.DataTooLargeForBuffer);

            var indexStride = GetElementSize();
            SetDataInternal(data, 0, data.Length * indexStride, SetDataOptions.None);
        }

        /// <summary>
        /// Sets the data contained by the index buffer.
        /// </summary>
        /// <param name="data">An array containing the data to set in the index buffer.</param>
        /// <param name="offset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="count">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureRange(offset >= 0 && offset + count <= data.Length, nameof(offset));
            Contract.Ensure(count <= IndexCount, OpenGLStrings.DataTooLargeForBuffer);

            var indexStride = GetElementSize();
            SetDataInternal(data, offset * indexStride, count * indexStride, options);
        }

        /// <inheritdoc/>
        public override void SetDataAligned<T>(T[] data, Int32 dataOffset, Int32 dataCount, Int32 bufferOffset, out Int32 bufferSize, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(dataCount > 0, nameof(dataCount));
            Contract.EnsureRange(dataOffset >= 0 && dataOffset + dataCount <= data.Length, nameof(dataOffset));
            Contract.EnsureRange(bufferOffset >= 0, nameof(bufferOffset));
            Contract.Ensure(dataCount <= IndexCount, OpenGLStrings.DataTooLargeForBuffer);

            var indexStride = GetElementSize();
            bufferSize = indexStride * dataCount;

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var caps = (OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities;
                if (caps.SupportsMapBufferRange && caps.MinMapBufferAlignment > 0)
                    bufferSize = Math.Min(Math.Max(caps.MinMapBufferAlignment, MathUtil.FindNextPowerOfTwo(bufferSize)), SizeInBytes - bufferOffset);

                using (OpenGLState.ScopedBindElementArrayBuffer(buffer))
                {
                    var isPartialUpdate = (bufferOffset > 0 || bufferSize < SizeInBytes);
                    var isDiscarding = (options == SetDataOptions.Discard);
                    if (isDiscarding || !isPartialUpdate)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, this.size, isPartialUpdate ? null : handle.AddrOfPinnedObject().ToPointer(), usage);
                        gl.ThrowIfError();
                    }

                    if (isPartialUpdate)
                    {
                        if (caps.SupportsMapBufferRange)
                        {
                            var bufferRangeAccess = gl.GL_MAP_WRITE_BIT | (options == SetDataOptions.NoOverwrite ? gl.GL_MAP_UNSYNCHRONIZED_BIT : 0);
                            var bufferRangePtr = (Byte*)gl.MapNamedBufferRange(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, 
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, bufferRangeAccess);
                            gl.ThrowIfError();

                            var sourceRangePtr = (Byte*)handle.AddrOfPinnedObject() + (dataOffset * indexStride);
                            var sourceSizeInBytes = dataCount * indexStride;

                            for (int i = 0; i < sourceSizeInBytes; i++)
                                *bufferRangePtr++ = *sourceRangePtr++;

                            gl.UnmapNamedBuffer(buffer, gl.GL_ELEMENT_ARRAY_BUFFER);
                            gl.ThrowIfError();
                        }
                        else
                        {
                            gl.NamedBufferSubData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER,
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, (Byte*)handle.AddrOfPinnedObject().ToPointer() + (dataOffset * indexStride));
                            gl.ThrowIfError();
                        }
                    }
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <inheritdoc/>
        public override Int32 GetAlignmentUnit()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return Math.Max(1, ((OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities).MinMapBufferAlignment);
        }

        /// <inheritdoc/>
        public override Int32 GetAlignedSize(Int32 count)
        {
            Contract.EnsureRange(count >= 0, nameof(count));
            Contract.EnsureNotDisposed(this, Disposed);

            var indexStride = GetElementSize();

            var caps = (OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities;
            if (caps.MinMapBufferAlignment == 0 || count == IndexCount)
                return count * indexStride;

            return Math.Max(caps.MinMapBufferAlignment, MathUtil.FindNextPowerOfTwo(count * indexStride));
        }

        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return buffer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the buffer's content has been lost.
        /// </summary>
        public override Boolean IsContentLost
        {
            get { return false; }
        }

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteBuffer(((OpenGLIndexBuffer)state).buffer);
                        gl.ThrowIfError();                        
                    }, this);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        private void SetDataInternal<T>(T[] data, Int32 offsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindElementArrayBuffer(buffer))
                {
                    var isPartialUpdate = (offsetInBytes > 0 || countInBytes < SizeInBytes);
                    var isDiscarding = (options == SetDataOptions.Discard);
                    if (isDiscarding || !isPartialUpdate)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, this.size, isPartialUpdate ? null : handle.AddrOfPinnedObject().ToPointer(), usage);
                        gl.ThrowIfError();
                    }

                    if (isPartialUpdate)
                    {
                        gl.NamedBufferSubData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, (IntPtr)offsetInBytes, (IntPtr)countInBytes, handle.AddrOfPinnedObject().ToPointer());
                        gl.ThrowIfError();
                    }
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Gets the size of one of the buffer's elements, in bytes.
        /// </summary>
        /// <returns>The size of one of the buffer's elements.</returns>
        private Int32 GetElementSize()
        {
            switch (IndexElementType)
            {
                case IndexBufferElementType.Int16:
                    return sizeof(short);
                case IndexBufferElementType.Int32:
                    return sizeof(int);
                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedElementType);
            }
        }

        // Property values.
        private readonly UInt32 buffer;

        // State values.
        private readonly UInt32 usage;
        private readonly IntPtr size;
    }
}
