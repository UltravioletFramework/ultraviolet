using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="Texture3D"/> class.
    /// </summary>
    public unsafe sealed class OpenGLTexture3D : Texture3D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="data">A list of pointers to the raw pixel data for each of the texture's layers.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
        public OpenGLTexture3D(UltravioletContext uv, IList<IntPtr> data, Int32 width, Int32 height, Int32 bytesPerPixel)
            : base(uv)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(bytesPerPixel == 3 || bytesPerPixel == 4, nameof(bytesPerPixel));

            var format = GetOpenGLTextureFormatFromBytesPerPixel(bytesPerPixel);
            var internalformat = GetOpenGLTextureInternalFormatFromBytesPerPixel(bytesPerPixel);

            if (type == gl.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            var pixels = IntPtr.Zero;
            try
            {
                pixels = CreateConcatenatedPixelBuffer(data, width * height * bytesPerPixel);
                CreateNativeTexture(uv, internalformat, width, height, data.Count, type, 
                    gl.GL_UNSIGNED_BYTE, (void*)pixels, true);
            }
            finally
            {
                if (pixels != IntPtr.Zero)
                    Marshal.FreeHGlobal(pixels);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="depth">The texture's depth in layers.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        public OpenGLTexture3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, Boolean immutable)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));

            CreateNativeTexture(uv, gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8, width, height, depth, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, null, immutable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texture's texel format.</param>
        /// <param name="type">The texture's texel type.</param>
        /// <param name="data">A list of pointers to the raw pixel data for each of the texture's layers.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        public OpenGLTexture3D(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, IList<IntPtr> data, Boolean immutable)
            : this(uv, internalformat, width, height, format, type, data, immutable, false)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLTexture3D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texture's texel format.</param>
        /// <param name="type">The texture's texel type.</param>
        /// <param name="data">The texture's data.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        /// <param name="rbuffer">A value indicating whether this texture is being used as a render buffer.</param>
        /// <returns>The instance of Texture2D that was created.</returns>
        internal OpenGLTexture3D(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, IList<IntPtr> data, Boolean immutable, Boolean rbuffer)
            : base(uv)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            this.rbuffer = rbuffer;

            var bytesPerPixel = (format == gl.GL_RGB || format == gl.GL_BGR) ? 3 : 4;
            var pixels = IntPtr.Zero;
            try
            {
                pixels = CreateConcatenatedPixelBuffer(data, width * height * bytesPerPixel);
                CreateNativeTexture(uv, internalformat, width, height, data.Count, format,
                    type, (void*)pixels, immutable);
            }
            finally
            {
                if (pixels != IntPtr.Zero)
                    Marshal.FreeHGlobal(pixels);
            }
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture other)
        {
            var id1 = texture;
            var id2 = (other == null) ? 0 : ((IOpenGLResource)other).OpenGLName;
            return id1.CompareTo(id2);
        }

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height, Int32 depth)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(width >= 1, nameof(width));
            Contract.EnsureRange(height >= 1, nameof(height));

            if (BoundForReading || BoundForWriting)
                throw new InvalidOperationException(OpenGLStrings.InvalidOperationWhileBound);

            this.width = width;
            this.height = height;
            this.depth = depth;

            if (immutable)
                throw new InvalidOperationException(rbuffer ? OpenGLStrings.RenderBufferIsImmutable : OpenGLStrings.TextureIsImmutable);

            if (Ultraviolet.IsExecutingOnCurrentThread)
            {
                ProcessResize();
            }
            else
            {
                Ultraviolet.QueueWorkItem((state) => { ((OpenGLTexture3D)state).ProcessResize(); }, this);
            }
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));

            SetDataInternal(0, 0, 0, Width, Height, 0, Depth, data, 0, data.Length);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            SetDataInternal(0, 0, 0, Width, Height, 0, Depth, data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            SetDataInternal(level, left, top, right, bottom, front, back, data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public void BindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite == 0, OpenGLStrings.ResourceCannotBeReadWhileWriting);

            boundRead++;
        }

        /// <inheritdoc/>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead == 0, OpenGLStrings.ResourceCannotBeWrittenWhileReading);

            boundWrite++;
        }

        /// <inheritdoc/>
        public void UnbindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead > 0, OpenGLStrings.ResourceNotBound);

            boundRead--;
        }

        /// <inheritdoc/>
        public void UnbindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite > 0, OpenGLStrings.ResourceNotBound);

            boundWrite--;
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return height;
            }
        }
        
        /// <inheritdoc/>
        public override Int32 Depth
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return depth;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForReading
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundRead > 0;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForWriting
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundWrite > 0;
            }
        }

        /// <inheritdoc/>
        public override Boolean ImmutableStorage
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return immutable;
            }
        }

        /// <inheritdoc/>
        public override Boolean WillNotBeSampled
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return false;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    ((OpenGLUltravioletGraphics)Ultraviolet.GetGraphics()).UnbindTexture(this);
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteTexture(((OpenGLTexture3D)state).texture);
                        gl.ThrowIfError();
                    }, this);
                }
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return texture;
            }
        }

        /// <summary>
        /// Creates a new buffer in unmanaged memory which contains the contents of the specified buffers.
        /// </summary>
        private static IntPtr CreateConcatenatedPixelBuffer(IList<IntPtr> data, Int32 lengthInBytes)
        {
            var pixels = Marshal.AllocHGlobal(data.Count * lengthInBytes);
            var pixelsOffset = 0;
            if (Environment.Is64BitProcess && lengthInBytes % sizeof(Int64) == 0)
            {
                var lengthInInt64 = lengthInBytes / sizeof(Int64);
                foreach (var layer in data)
                {
                    CopyBuffer64(layer, pixels, pixelsOffset, lengthInInt64);
                    pixelsOffset += lengthInInt64;
                }
            }
            else
            {
                if (lengthInBytes % sizeof(Int32) == 0)
                {
                    var lengthInInt32 = lengthInBytes / sizeof(Int32);
                    foreach (var layer in data)
                    {
                        CopyBuffer32(layer, pixels, pixelsOffset, lengthInInt32);
                        pixelsOffset += lengthInInt32;
                    }
                }
                else
                {
                    foreach (var layer in data)
                    {
                        CopyBuffer8(layer, pixels, pixelsOffset, lengthInBytes);
                        pixelsOffset += lengthInBytes;
                    }
                }
            }
            return pixels;
        }

        /// <summary>
        /// Copies an unmanaged buffer into another unmanaged buffer at the specified offset.
        /// </summary>
        private static void CopyBuffer64(IntPtr src, IntPtr dst, Int32 dstOffset, Int32 length)
        {
            var pSrc = ((Int64*)src);
            var pDst = ((Int64*)dst + dstOffset);
            
            for (int i = 0; i < length; i++)
                *pDst++ = *pSrc++;
        }

        /// <summary>
        /// Copies an unmanaged buffer into another unmanaged buffer at the specified offset.
        /// </summary>
        private static void CopyBuffer32(IntPtr src, IntPtr dst, Int32 dstOffset, Int32 length)
        {
            var pSrc = ((Int32*)src);
            var pDst = ((Int32*)dst + dstOffset);

            for (int i = 0; i < length; i++)
                *pDst++ = *pSrc++;
        }

        /// <summary>
        /// Copies an unmanaged buffer into another unmanaged buffer at the specified offset.
        /// </summary>
        private static void CopyBuffer8(IntPtr src, IntPtr dst, Int32 dstOffset, Int32 length)
        {
            var pSrc = ((Byte*)src);
            var pDst = ((Byte*)dst + dstOffset);

            for (int i = 0; i < length; i++)
                *pDst++ = *pSrc++;
        }

        /// <summary>
        /// Gets the OpenGL internal texture format that corresponds to the specified number of bytes per pixel.
        /// </summary>
        private static UInt32 GetOpenGLTextureInternalFormatFromBytesPerPixel(Int32 bytesPerPixel)
        {
            if (bytesPerPixel == 4)
                return gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8;

            if (bytesPerPixel == 3)
                return gl.GL_RGB;

            return gl.GL_NONE;
        }

        /// <summary>
        /// Gets the OpenGL texture format that corresponds to the specified number of bytes per pixel.
        /// </summary>
        private static UInt32 GetOpenGLTextureFormatFromBytesPerPixel(Int32 bytesPerPixel)
        {
            if (bytesPerPixel == 4)
                return gl.GL_RGBA;

            if (bytesPerPixel == 3)
                return gl.GL_RGB;

            return gl.GL_NONE;
        }

        /// <summary>
        /// Gets the OpenGL texture format flag that corresponds to the specified texture data format.
        /// </summary>
        /// <param name="format">The texture data format to convert.</param>
        /// <returns>The converted texture data format.</returns>
        private static UInt32 GetOpenGLTextureFormatFromTextureDataFormat(TextureDataFormat format)
        {
            switch (format)
            {
                case TextureDataFormat.RGBA:
                    return gl.GL_RGBA;
                case TextureDataFormat.BGRA:
                    return gl.GL_BGRA;
            }
            throw new NotSupportedException("format");
        }

        /// <summary>
        /// Creates the underlying native OpenGL texture with the specified format and data.
        /// </summary>
        private void CreateNativeTexture(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, Int32 depth, 
            UInt32 format, UInt32 type, void* pixels, Boolean immutable)
        {
            if (uv.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);

            this.width = width;
            this.height = height;
            this.depth = depth;
            this.internalformat = internalformat;
            this.format = format;
            this.type = type;
            this.immutable = immutable;

            this.texture = uv.QueueWorkItemAndWait(() =>
            {
                uint glname;

                using (OpenGLState.ScopedCreateTexture3D(out glname))
                {
                    if (!gl.IsGLES2)
                    {
                        gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MAX_LEVEL, 0);
                        gl.ThrowIfError();
                    }

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();
                    
                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_WRAP_R, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_WRAP_S, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_3D, gl.GL_TEXTURE_WRAP_T, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();
                    
                    if (immutable)
                    {
                        if (gl.IsTextureStorageAvailable)
                        {
                            gl.TextureStorage3D(glname, gl.GL_TEXTURE_3D, 1, internalformat, width, height, depth);
                            gl.ThrowIfError();

                            if (pixels != null)
                            {
                                gl.TextureSubImage3D(glname, gl.GL_TEXTURE_3D, 0, 0, 0, 0, width, height, depth, format, type, pixels);
                                gl.ThrowIfError();
                            }
                        }
                        else
                        {
                            gl.TextureImage3D(glname, gl.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                            gl.ThrowIfError();
                        }
                    }
                }

                if (!immutable)
                {
                    using (OpenGLState.ScopedBindTexture3D(glname, true))
                    {
                        gl.TexImage3D(gl.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                        gl.ThrowIfError();
                    }
                }

                return glname;
            });
        }

        /// <summary>
        /// Processes a resize operation for this texture.
        /// </summary>
        private void ProcessResize()
        {
            using (OpenGLState.ScopedBindTexture3D(texture, true))
            {
                gl.TexImage3D(gl.GL_TEXTURE_3D, 0, (int)internalformat,
                    width, height, depth, 0, format, type, null);
            }
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        private void SetDataInternal<T>(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, T[] data, Int32 startIndex, Int32 elementCount) where T : struct
        {
            var elementSizeInBytes = Marshal.SizeOf(typeof(T));
            var width = right - left;
            var height = bottom - top;
            var depth = back - front;

            var pixelSizeInBytes = (format == gl.GL_RGB || format == gl.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height * depth != elementSizeInBytes * elementCount)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Ultraviolet.QueueWorkItemAndWait(() =>
            {
                var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    var pData = dataHandle.AddrOfPinnedObject() + (startIndex * Marshal.SizeOf(typeof(T)));
                    gl.TextureSubImage3D(OpenGLName, gl.GL_TEXTURE_3D, level, left, top, front, 
                        right - left, bottom - top, back - front, format, type, (void*)pData);
                    gl.ThrowIfError();
                }
                finally { dataHandle.Free(); }
            });
        }

        // Property values.
        private UInt32 texture;
        private Int32 width;
        private Int32 height;
        private Int32 depth;
        private UInt32 internalformat;
        private UInt32 format;
        private UInt32 type;
        private Boolean immutable;
        private Boolean rbuffer;
        private Int32 boundRead;
        private Int32 boundWrite;
    }
}
