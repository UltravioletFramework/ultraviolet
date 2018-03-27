using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the VertexBuffer class.
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
            Contract.Require(vdecl, nameof(vdecl));

            this.vdecl = vdecl;
            this.usage = usage;
            this.size = new IntPtr(vdecl.VertexStride * vcount);

            var buffer = 0u;

            uv.QueueWorkItem(state =>
            {
                using (OpenGLState.ScopedCreateArrayBuffer(out buffer))
                {
                    gl.NamedBufferData(buffer, gl.GL_ARRAY_BUFFER, size, null, usage);
                    gl.ThrowIfError();
                }
            }).Wait();

            this.buffer = buffer;
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data)
        {
            Contract.Require(data, nameof(data));
            Contract.Ensure(data.Length <= VertexCount, OpenGLStrings.DataTooLargeForBuffer);

            SetDataInternal(data, 0, data.Length * vdecl.VertexStride, SetDataOptions.None);
        }
        
        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureRange(offset >= 0 && offset + count <= data.Length, nameof(offset));
            Contract.Ensure(count <= VertexCount, OpenGLStrings.DataTooLargeForBuffer);

            SetDataInternal(data, offset * vdecl.VertexStride, count * vdecl.VertexStride, options);
        }

        /// <inheritdoc/>
        public override void SetDataAligned<T>(T[] data, Int32 dataOffset, Int32 dataCount, Int32 bufferOffset, out Int32 bufferSize, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(dataCount > 0, nameof(dataCount));
            Contract.EnsureRange(dataOffset >= 0 && dataOffset + dataCount <= data.Length, nameof(dataOffset));
            Contract.EnsureRange(bufferOffset >= 0, nameof(bufferOffset));
            Contract.Ensure(dataCount <= VertexCount, OpenGLStrings.DataTooLargeForBuffer);

            bufferSize = vdecl.VertexStride * dataCount;

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var caps = (OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities;
                if (caps.SupportsMapBufferRange && caps.MinMapBufferAlignment > 0)
                    bufferSize = Math.Min(Math.Max(caps.MinMapBufferAlignment, MathUtil.FindNextPowerOfTwo(bufferSize)), SizeInBytes - bufferOffset);

                using (OpenGLState.ScopedBindArrayBuffer(buffer))
                {
                    var isPartialUpdate = (bufferOffset > 0 || bufferSize < SizeInBytes);
                    var isDiscarding = (options == SetDataOptions.Discard);
                    if (isDiscarding || !isPartialUpdate)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ARRAY_BUFFER, this.size, isPartialUpdate ? null : handle.AddrOfPinnedObject().ToPointer(), usage);
                        gl.ThrowIfError();

                        /* FIX: 
                         * I have no idea why the following code is necessary, but
                         * it seems to fix flickering sprites on Intel HD 4000 devices. */
                        if (OpenGLState.SupportsVertexArrayObjects)
                        {
                            var vao = (uint)OpenGLState.GL_VERTEX_ARRAY_BINDING;
                            gl.BindVertexArray(vao);
                            gl.ThrowIfError();
                        }
                    }

                    if (isPartialUpdate)
                    {
                        if (caps.SupportsMapBufferRange)
                        {
                            var bufferRangeAccess = gl.GL_MAP_WRITE_BIT | (options == SetDataOptions.NoOverwrite ? gl.GL_MAP_UNSYNCHRONIZED_BIT : 0);
                            var bufferRangePtr = (Byte*)gl.MapNamedBufferRange(buffer, gl.GL_ARRAY_BUFFER,
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, bufferRangeAccess);
                            gl.ThrowIfError();

                            var sourceRangePtr = (Byte*)handle.AddrOfPinnedObject() + (dataOffset * vdecl.VertexStride);
                            var sourceSizeInBytes = dataCount * vdecl.VertexStride;

                            for (int i = 0; i < sourceSizeInBytes; i++)
                                *bufferRangePtr++ = *sourceRangePtr++;

                            gl.UnmapNamedBuffer(buffer, gl.GL_ARRAY_BUFFER);
                            gl.ThrowIfError();
                        }
                        else
                        {
                            gl.NamedBufferSubData(buffer, gl.GL_ARRAY_BUFFER,
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, (Byte*)handle.AddrOfPinnedObject().ToPointer() + (dataOffset * vdecl.VertexStride));
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
            return Math.Max(1, ((OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities).MinMapBufferAlignment);
        }

        /// <inheritdoc/>
        public override Int32 GetAlignedSize(Int32 count)
        {
            Contract.EnsureRange(count >= 0, nameof(count));
            
            var caps = (OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities;
            if (caps.MinMapBufferAlignment == 0 || count == VertexCount)
            {
                return count * vdecl.VertexStride;
            }
            return Math.Max(caps.MinMapBufferAlignment, MathUtil.FindNextPowerOfTwo(count * vdecl.VertexStride));
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

        /// <inheritdoc/>
        public override Boolean IsContentLost => false;

        /// <inheritdoc/>
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

        /// <summary>
        /// Sets the data contained by the vertex buffer.
        /// </summary>
        private void SetDataInternal<T>(T[] data, Int32 offsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (OpenGLState.ScopedBindArrayBuffer(buffer))
                {
                    var isPartialUpdate = (offsetInBytes > 0 || countInBytes < SizeInBytes);
                    var isDiscarding = (options == SetDataOptions.Discard);
                    if (isDiscarding || !isPartialUpdate)
                    {
                        gl.NamedBufferData(buffer, gl.GL_ARRAY_BUFFER, this.size, isPartialUpdate ? null : handle.AddrOfPinnedObject().ToPointer(), usage);
                        gl.ThrowIfError();

                        /* FIX: 
                         * I have no idea why the following code is necessary, but
                         * it seems to fix flickering sprites on Intel HD 4000 devices. */
                        if (OpenGLState.SupportsVertexArrayObjects)
                        {
                            var vao = (uint)OpenGLState.GL_VERTEX_ARRAY_BINDING;
                            gl.BindVertexArray(vao);
                            gl.ThrowIfError();
                        }
                    }

                    if (isPartialUpdate)
                    {
                        gl.NamedBufferSubData(buffer, gl.GL_ARRAY_BUFFER, (IntPtr)offsetInBytes, (IntPtr)countInBytes, handle.AddrOfPinnedObject().ToPointer());
                        gl.ThrowIfError();
                    }
                }
            }
            finally
            {
                handle.Free();
            }
        }
        
        // Property values.
        private UInt32 buffer;

        // State values.
        private UInt32 usage;
        private IntPtr size;
        private readonly VertexDeclaration vdecl;        
    }
}
