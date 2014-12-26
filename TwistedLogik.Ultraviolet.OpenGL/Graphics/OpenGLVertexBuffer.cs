using System;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the VertexBuffer class.
    /// </summary>
    public unsafe sealed class OpenGLVertexBuffer : DynamicVertexBuffer
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLVertexBuffer.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="vdecl">The vertex declaration for this buffer.</param>
        /// <param name="vcount">The number of vertices in the buffer.</param>
        /// <param name="usage">The buffer's usage type.</param>
        public OpenGLVertexBuffer(UltravioletContext uv, VertexDeclaration vdecl, Int32 vcount, UInt32 usage)
            : base(uv, vdecl, vcount)
        {
            Contract.Require(vdecl, "vdecl");

            this.vdecl = vdecl;
            this.usage = usage;
            this.size = new IntPtr(vdecl.VertexStride * vcount);

            var buffer = 0u;

            uv.QueueWorkItemAndWait(() =>
            {
                using (OpenGLState.ScopedCreateArrayBuffer(out buffer))
                {
                    gl.NamedBufferData(buffer, gl.GL_ARRAY_BUFFER, size, null, usage);
                    gl.ThrowIfError();
                }
            });

            this.buffer = buffer;
        }

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        /// <param name="data">An array containing the data to set in the vertex buffer.</param>
        public override void SetData<T>(T[] data)
        {
            Contract.Require(data, "data");
            Contract.Ensure(data.Length <= VertexCount, OpenGLStrings.DataTooLargeForBuffer);
            
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindArrayBuffer(buffer))
                {
                    var size = new IntPtr(vdecl.VertexStride * data.Length);
                    gl.NamedBufferSubData(buffer, gl.GL_ARRAY_BUFFER, IntPtr.Zero, size, handle.AddrOfPinnedObject().ToPointer());
                    gl.ThrowIfError();
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        /// <param name="data">An array containing the data to set in the vertex buffer.</param>
        /// <param name="offset">The offset into <paramref name="data"/> at which to begin setting elements into the buffer.</param>
        /// <param name="count">The number of elements from <paramref name="data"/> to set into the buffer.</param>
        /// <param name="options">A hint to the underlying driver indicating whether data will be overwritten by this operation.</param>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options)
        {
            Contract.Require(data, "data");
            Contract.EnsureRange(count > 0, "count");
            Contract.EnsureRange(offset >= 0 && offset + count <= data.Length, "offset");
            Contract.Ensure(count <= VertexCount, OpenGLStrings.DataTooLargeForBuffer);

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindArrayBuffer(buffer))
                {
                    if (options == SetDataOptions.Discard)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ARRAY_BUFFER, this.size, null, usage);
                        gl.ThrowIfError();

                        /* FIX: 
                         * I have no idea why the following code is necessary, but
                         * it seems to fix flickering sprites on Intel HD 4000 devices. */
                        var vao = (uint)OpenGLState.GL_VERTEX_ARRAY_BINDING;
                        gl.BindVertexArray(vao);
                        gl.ThrowIfError();
                    }

                    var start = new IntPtr(offset * vdecl.VertexStride);
                    var size = new IntPtr(vdecl.VertexStride * count);

                    gl.NamedBufferSubData(buffer, gl.GL_ARRAY_BUFFER, start, size, handle.AddrOfPinnedObject().ToPointer());
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
                        gl.DeleteBuffer(((OpenGLVertexBuffer)state).buffer);
                        gl.ThrowIfError();
                    }, this);
                }
            }

            base.Dispose(disposing);
        }

        // Property values.
        private UInt32 buffer;

        // State values.
        private UInt32 usage;
        private IntPtr size;
        private readonly VertexDeclaration vdecl;
    }
}
