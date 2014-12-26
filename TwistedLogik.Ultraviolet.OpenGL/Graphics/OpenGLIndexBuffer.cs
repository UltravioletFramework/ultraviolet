using System;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
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
            Contract.EnsureRange(icount >= 0, "icount");

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
            Contract.Require(data, "data");
            Contract.Ensure(data.Length <= IndexCount, OpenGLStrings.DataTooLargeForBuffer);

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindElementArrayBuffer(buffer))
                {
                    var size = new IntPtr(GetElementSize() * data.Length);
                    gl.NamedBufferSubData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, IntPtr.Zero, size, handle.AddrOfPinnedObject().ToPointer());
                    gl.ThrowIfError();
                }
            }
            finally
            {
                handle.Free();
            }
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
            Contract.Require(data, "data");
            Contract.EnsureRange(count > 0, "count");
            Contract.EnsureRange(offset >= 0 && offset + count <= data.Length, "offset");
            Contract.Ensure(count <= IndexCount, OpenGLStrings.DataTooLargeForBuffer);

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindElementArrayBuffer(buffer))
                {
                    if (options == SetDataOptions.Discard)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, this.size, null, usage);
                        gl.ThrowIfError();
                    }

                    var elementsize = GetElementSize();
                    var start = new IntPtr(offset * elementsize);
                    var size = new IntPtr(elementsize * count);

                    gl.NamedBufferSubData(buffer, gl.GL_ELEMENT_ARRAY_BUFFER, start, size, handle.AddrOfPinnedObject().ToPointer());
                    gl.ThrowIfError();
                }
            }
            finally
            {
                handle.Free();
            }
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
