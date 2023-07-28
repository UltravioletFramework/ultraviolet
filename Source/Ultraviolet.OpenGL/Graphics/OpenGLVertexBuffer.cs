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
                    GL.NamedBufferData(buffer, GL.GL_ARRAY_BUFFER, size, null, usage);
                    GL.ThrowIfError();
                }
            }).Wait();

            this.buffer = buffer;
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data)
        {
            Contract.Require(data, nameof(data));

            var inputElemSize = Marshal.SizeOf(typeof(T));
            var inputSizeInBytes = inputElemSize * data.Length;
            if (inputSizeInBytes > size.ToInt32())
                throw new InvalidOperationException(OpenGLStrings.DataTooLargeForBuffer);

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(data, 0, 0, inputSizeInBytes, SetDataOptions.None);
            else
                SetDataInternal_OnBackgroundThread(data, 0, 0, inputSizeInBytes, SetDataOptions.None);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureRange(offset >= 0 && offset + count <= data.Length, nameof(offset));

            var inputElemSize = Marshal.SizeOf(typeof(T));
            var inputSizeInBytes = inputElemSize * count;
            if (inputSizeInBytes > size.ToInt32())
                throw new InvalidOperationException(OpenGLStrings.DataTooLargeForBuffer);

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(data, (offset * inputElemSize), 0, inputSizeInBytes, options);
            else
                SetDataInternal_OnBackgroundThread(data, (offset * inputElemSize), 0, inputSizeInBytes, options);
        }

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes, SetDataOptions options)
        {
            Contract.EnsureRange(srcOffsetInBytes >= 0, nameof(srcOffsetInBytes));
            Contract.EnsureRange(dstOffsetInBytes >= 0, nameof(dstOffsetInBytes));
            Contract.EnsureRange(sizeInBytes >= 0, nameof(sizeInBytes));

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(data + srcOffsetInBytes, dstOffsetInBytes, sizeInBytes, options);
            else
                SetRawDataInternal_OnBackgroundThread(data + srcOffsetInBytes, dstOffsetInBytes, sizeInBytes, options);
        }

        /// <inheritdoc/>
        public override void SetDataAligned<T>(T[] data, Int32 dataOffset, Int32 dataCount, Int32 bufferOffset, out Int32 bufferSize, SetDataOptions options)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(dataCount > 0, nameof(dataCount));
            Contract.EnsureRange(dataOffset >= 0 && dataOffset + dataCount <= data.Length, nameof(dataOffset));
            Contract.EnsureRange(bufferOffset >= 0, nameof(bufferOffset));

            var inputElemSize = Marshal.SizeOf(typeof(T));
            var inputSizeInBytes = inputElemSize * dataCount;
            if (inputSizeInBytes > size.ToInt32())
                throw new InvalidOperationException(OpenGLStrings.DataTooLargeForBuffer);

            bufferSize = inputSizeInBytes;

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var caps = (OpenGLGraphicsCapabilities)Ultraviolet.GetGraphics().Capabilities;
                if (caps.MinMapBufferAlignment > 0)
                    bufferSize = Math.Min(Math.Max(caps.MinMapBufferAlignment, MathUtil.FindNextPowerOfTwo(bufferSize)), SizeInBytes - bufferOffset);

                using (OpenGLState.ScopedBindArrayBuffer(buffer))
                {
                    var isPartialUpdate = (bufferOffset > 0 || bufferSize < SizeInBytes);
                    var isDiscarding = (options == SetDataOptions.Discard);
                    if (isDiscarding || !isPartialUpdate)
                    {
                        GL.NamedBufferData(buffer, GL.GL_ARRAY_BUFFER, this.size, isPartialUpdate ? null : handle.AddrOfPinnedObject().ToPointer(), usage);
                        GL.ThrowIfError();

                        /* FIX: 
                         * I have no idea why the following code is necessary, but
                         * it seems to fix flickering sprites on Intel HD 4000 devices. */
                        if (GL.IsVertexArrayObjectAvailable)
                        {
                            var vao = (uint)OpenGLState.GL_VERTEX_ARRAY_BINDING;
                            GL.BindVertexArray(vao);
                            GL.ThrowIfError();
                        }
                    }

                    if (isPartialUpdate)
                    {
                        if (caps.MinMapBufferAlignment >= 0)
                        {
                            var bufferRangeAccess = GL.GL_MAP_WRITE_BIT | (options == SetDataOptions.NoOverwrite ? GL.GL_MAP_UNSYNCHRONIZED_BIT : 0);
                            var bufferRangePtr = (Byte*)GL.MapNamedBufferRange(buffer, GL.GL_ARRAY_BUFFER,
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, bufferRangeAccess);
                            GL.ThrowIfError();

                            var sourceRangePtr = (Byte*)handle.AddrOfPinnedObject() + (dataOffset * inputElemSize);
                            var sourceSizeInBytes = dataCount * inputElemSize;

                            for (int i = 0; i < sourceSizeInBytes; i++)
                                *bufferRangePtr++ = *sourceRangePtr++;

                            GL.UnmapNamedBuffer(buffer, GL.GL_ARRAY_BUFFER);
                            GL.ThrowIfError();
                        }
                        else
                        {
                            GL.NamedBufferSubData(buffer, GL.GL_ARRAY_BUFFER,
                                (IntPtr)bufferOffset, (IntPtr)bufferSize, (Byte*)handle.AddrOfPinnedObject().ToPointer() + (dataOffset * inputElemSize));
                            GL.ThrowIfError();
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
        public UInt32 OpenGLName => buffer;

        /// <inheritdoc/>
        public override Boolean IsContentLost => false;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (OpenGLState.GL_ARRAY_BUFFER_BINDING == buffer)
                    OpenGLState.GL_ARRAY_BUFFER_BINDING.Update(0);

                var glname = buffer;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        GL.DeleteBuffer(glname);
                        GL.ThrowIfError();
                    }, this, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                buffer = 0;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets the buffer's data from native memory.
        /// </summary>
        private void SetDataInternal_OnMainThread(Object data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                Upload(pData.AddrOfPinnedObject() + srcOffsetInBytes, dstOffsetInBytes, countInBytes, options);
            }
            finally { pData.Free(); }
        }

        /// <summary>
        /// Sets the buffer's data from native memory.
        /// </summary>
        private void SetDataInternal_OnBackgroundThread(Object data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            Ultraviolet.QueueWorkItem(state =>
            {
                var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    Upload(pData.AddrOfPinnedObject() + srcOffsetInBytes, dstOffsetInBytes, countInBytes, options);
                }
                finally { pData.Free(); }
            }, null, WorkItemOptions.ForceAsynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Sets the buffer's data from native memory.
        /// </summary>
        private void SetRawDataInternal_OnMainThread(IntPtr data, Int32 offsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            Upload(data, offsetInBytes, countInBytes, options);
        }

        /// <summary>
        /// Sets the buffer's data from native memory.
        /// </summary>
        private void SetRawDataInternal_OnBackgroundThread(IntPtr data, Int32 offsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            Ultraviolet.QueueWorkItem(state =>
            {
                Upload(data, offsetInBytes, countInBytes, options);
            }, null, WorkItemOptions.ForceAsynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Uploads buffer data to the graphics device.
        /// </summary>
        private void Upload(IntPtr data, Int32 offsetInBytes, Int32 countInBytes, SetDataOptions options)
        {
            using (OpenGLState.ScopedBindArrayBuffer(buffer))
            {
                var isPartialUpdate = (offsetInBytes > 0 || countInBytes < SizeInBytes);
                var isDiscarding = (options == SetDataOptions.Discard);
                if (isDiscarding || !isPartialUpdate)
                {
                    GL.NamedBufferData(buffer, GL.GL_ARRAY_BUFFER, this.size, isPartialUpdate ? null : (void*)data, usage);
                    GL.ThrowIfError();

                    /* FIX: 
                     * I have no idea why the following code is necessary, but
                     * it seems to fix flickering sprites on Intel HD 4000 devices. */
                    if (GL.IsVertexArrayObjectAvailable)
                    {
                        var vao = (uint)OpenGLState.GL_VERTEX_ARRAY_BINDING;
                        GL.BindVertexArray(vao);
                        GL.ThrowIfError();
                    }
                }

                if (isPartialUpdate)
                {
                    GL.NamedBufferSubData(buffer, GL.GL_ARRAY_BUFFER, (IntPtr)offsetInBytes, (IntPtr)countInBytes, (void*)data);
                    GL.ThrowIfError();
                }
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
