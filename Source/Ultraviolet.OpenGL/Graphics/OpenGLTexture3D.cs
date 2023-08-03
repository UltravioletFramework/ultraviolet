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
        /// <param name="format">The format of each pixel in the raw data.</param>
        /// <param name="options">The texture's configuration options.</param>
        public OpenGLTexture3D(UltravioletContext uv, IList<IntPtr> data, Int32 width, Int32 height, TextureFormat format, TextureOptions options)
            : base(uv)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(format == TextureFormat.RGB || format == TextureFormat.BGR || format == TextureFormat.RGBA || format == TextureFormat.BGRA, nameof(format));

            var isLinear = (options & TextureOptions.LinearColor) == TextureOptions.LinearColor;
            var isSrgb = (options & TextureOptions.SrgbColor) == TextureOptions.SrgbColor;
            if (isLinear && isSrgb)
                throw new ArgumentException(UltravioletStrings.TextureCannotHaveMultipleEncodings);

            var caps = uv.GetGraphics().Capabilities;
            var srgbEncoded = isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForSurface3D) && caps.SrgbEncodingEnabled;

            var glFormat = OpenGLTextureUtil.GetGLFormatFromTextureFormat(format);
            var internalformat = OpenGLTextureUtil.GetInternalGLFormatFromTextureFormat(format, srgbEncoded);

            if (glFormat == GL.GL_NONE || internalformat == GL.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            var pixels = IntPtr.Zero;
            try
            {
                var componentsPerPixel = OpenGLTextureUtil.GetComponentCountFromTextureFormat(format);
                pixels = CreateConcatenatedPixelBuffer(data, width * height * componentsPerPixel);
                CreateNativeTexture(uv, internalformat, width, height, data.Count, glFormat, 
                    GL.GL_UNSIGNED_BYTE, (void*)pixels, true);
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
        /// <param name="options">The texture's configuration options.</param>
        public OpenGLTexture3D(UltravioletContext uv, Int32 width, Int32 height, Int32 depth, TextureOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(depth > 0, nameof(depth));

            var isLinear = (options & TextureOptions.LinearColor) == TextureOptions.LinearColor;
            var isSrgb = (options & TextureOptions.SrgbColor) == TextureOptions.SrgbColor;
            if (isLinear && isSrgb)
                throw new ArgumentException(UltravioletStrings.TextureCannotHaveMultipleEncodings);

            var caps = uv.GetGraphics().Capabilities;
            var srgbEncoded = isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForSurface3D) && caps.SrgbEncodingEnabled;

            TextureFormat format = TextureFormat.RGBA;
            var glFormat = OpenGLTextureUtil.GetGLFormatFromTextureFormat(format);
            var internalformat = OpenGLTextureUtil.GetInternalGLFormatFromTextureFormat(format, srgbEncoded);

            CreateNativeTexture(uv, internalformat, width, height, depth, glFormat, GL.GL_UNSIGNED_BYTE, null, immutable);
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

            var bytesPerPixel = (format == GL.GL_RGB || format == GL.GL_BGR) ? 3 : 4;
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
            var id2 = (other is IOpenGLResource glother) ? glother.OpenGLName : 0;
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

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = data.Length * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(0, 0, 0, Width, Height, 0, Depth, data, 0, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(0, 0, 0, Width, Height, 0, Depth, data, 0, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = elementCount * sizeInBytesPerElement;
            var offsetInBytes = startIndex * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(0, 0, 0, Width, Height, 0, Depth, data, offsetInBytes, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(0, 0, 0, Width, Height, 0, Depth, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = elementCount * sizeInBytesPerElement;
            var offsetInBytes = startIndex * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(offsetInBytes >= 0, nameof(offsetInBytes));

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(0, 0, 0, Width, Height, 0, Depth, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(0, 0, 0, Width, Height, 0, Depth, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetRawData(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(level >= 0, nameof(level));
            Contract.EnsureRange(offsetInBytes >= 0, nameof(offsetInBytes));

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
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
        public UInt32 OpenGLName => texture;

        /// <inheritdoc/>
        public override Boolean SrgbEncoded => srgbEncoded;

        /// <inheritdoc/>
        public override Int32 Width => width;

        /// <inheritdoc/>
        public override Int32 Height => height;

        /// <inheritdoc/>
        public override Int32 Depth => depth;

        /// <inheritdoc/>
        public override Boolean BoundForReading => boundRead > 0;

        /// <inheritdoc/>
        public override Boolean BoundForWriting => boundWrite > 0;

        /// <inheritdoc/>
        public override Boolean ImmutableStorage => immutable;

        /// <inheritdoc/>
        public override Boolean WillNotBeSampled => false;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                var glname = texture;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    ((OpenGLUltravioletGraphics)Ultraviolet.GetGraphics()).UnbindTexture(this);
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        GL.DeleteTexture(glname);
                        GL.ThrowIfError();
                    }, this, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                texture = 0;
            }

            base.Dispose(disposing);
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
            this.srgbEncoded =
                internalformat == GL.GL_SRGB ||
                internalformat == GL.GL_SRGB_ALPHA ||
                internalformat == GL.GL_SRGB8 ||
                internalformat == GL.GL_SRGB8_ALPHA8;

            this.texture = uv.QueueWorkItem(state =>
            {
                uint glname;

                using (OpenGLState.ScopedCreateTexture3D(out glname))
                {
                    if (GL.IsTextureMaxLevelSupported)
                    {
                        GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_MAX_LEVEL, 0);
                        GL.ThrowIfError();
                    }

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_WRAP_R, (int)GL.GL_CLAMP_TO_EDGE);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_CLAMP_TO_EDGE);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_3D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_CLAMP_TO_EDGE);
                    GL.ThrowIfError();

                    if (immutable)
                    {
                        if (GL.IsTextureStorageAvailable)
                        {
                            GL.TextureStorage3D(glname, GL.GL_TEXTURE_3D, 1, internalformat, width, height, depth);
                            GL.ThrowIfError();

                            if (pixels != null)
                            {
                                GL.TextureSubImage3D(glname, GL.GL_TEXTURE_3D, 0, 0, 0, 0, width, height, depth, format, type, pixels);
                                GL.ThrowIfError();
                            }
                        }
                        else
                        {
                            GL.TextureImage3D(glname, GL.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                            GL.ThrowIfError();
                        }
                    }
                }

                if (!immutable)
                {
                    using (OpenGLState.ScopedBindTexture3D(glname, true))
                    {
                        GL.TexImage3D(GL.GL_TEXTURE_3D, 0, (int)internalformat, width, height, depth, 0, format, type, pixels);
                        GL.ThrowIfError();
                    }
                }

                return glname;
            }).Result;
        }

        /// <summary>
        /// Processes a resize operation for this texture.
        /// </summary>
        private void ProcessResize()
        {
            using (OpenGLState.ScopedBindTexture3D(texture, true))
            {
                GL.TexImage3D(GL.GL_TEXTURE_3D, 0, (int)internalformat,
                    width, height, depth, 0, format, type, null);
            }
        }

        /// <summary>
        /// Sets the texture's data from managed memory.
        /// </summary>
        private void SetDataInternal_OnMainThread(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, Object data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                SetRawDataInternal_OnMainThread(level, left, top, right, bottom, front, back, pData.AddrOfPinnedObject(), offsetInBytes, sizeInBytes);
            }
            finally { pData.Free(); }
        }

        /// <summary>
        /// Sets the texture's data from managed memory.
        /// </summary>
        private void SetDataInternal_OnBackgroundThread(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, Object data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Ultraviolet.QueueWorkItem(state =>
            {
                var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    SetRawDataInternal_OnMainThread(level, left, top, right, bottom, front, back, pData.AddrOfPinnedObject(), 0, sizeInBytes);
                }
                finally { pData.Free(); }
            }, null, WorkItemOptions.ReturnNullOnSynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Sets the texture's data from native memory.
        /// </summary>
        private void SetRawDataInternal_OnMainThread(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var width = right - left;
            var height = bottom - top;
            var depth = back - front;

            var pixelSizeInBytes = (format == GL.GL_RGB || format == GL.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height * depth != sizeInBytes)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Upload(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
        }

        /// <summary>
        /// Sets the texture's data from native memory.
        /// </summary>
        private void SetRawDataInternal_OnBackgroundThread(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var width = right - left;
            var height = bottom - top;
            var depth = back - front;

            var pixelSizeInBytes = (format == GL.GL_RGB || format == GL.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height * depth != sizeInBytes)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Ultraviolet.QueueWorkItem(state =>
            {
                Upload(level, left, top, right, bottom, front, back, data, offsetInBytes, sizeInBytes);
            }, null, WorkItemOptions.ReturnNullOnSynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Uploads texture data to the graphics device.
        /// </summary>
        private void Upload(Int32 level, Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 front, Int32 back, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            using (OpenGLState.ScopedBindTexture3D(OpenGLName))
            {
                var pData = data + offsetInBytes;
                GL.TextureSubImage3D(OpenGLName, GL.GL_TEXTURE_3D, level, left, top, front,
                    right - left, bottom - top, back - front, format, type, (void*)pData);
                GL.ThrowIfError();
            }
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
        private Boolean srgbEncoded;
        private Int32 boundRead;
        private Int32 boundWrite;
    }
}
