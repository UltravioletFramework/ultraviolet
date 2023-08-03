using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the Texture2D class.
    /// </summary>
    public unsafe sealed class OpenGLTexture2D : Texture2D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="pixels">A pointer to the raw pixel data with which to populate the texture.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The format of each pixel in the raw data.</param>
        /// <param name="options">The texture's configuration options.</param>
        public OpenGLTexture2D(UltravioletContext uv, IntPtr pixels, Int32 width, Int32 height, TextureFormat format, TextureOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(format == TextureFormat.RGB || format == TextureFormat.BGR || format == TextureFormat.RGBA || format == TextureFormat.BGRA, nameof(format));

            var isSrgb = (options & TextureOptions.SrgbColor) == TextureOptions.SrgbColor;
            var isLinear = (options & TextureOptions.LinearColor) == TextureOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.TextureCannotHaveMultipleEncodings);

            var caps = uv.GetGraphics().Capabilities;
            var srgbEncoded = (isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForTexture2D)) && caps.SrgbEncodingEnabled;

            var glFormat = OpenGLTextureUtil.GetGLFormatFromTextureFormat(format);
            var internalformat = OpenGLTextureUtil.GetInternalGLFormatFromTextureFormat(format, srgbEncoded);

            if (glFormat == GL.GL_NONE || internalformat == GL.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            CreateNativeTexture(uv, internalformat, width, height, glFormat,
                GL.GL_UNSIGNED_BYTE, (void*)pixels, (options & TextureOptions.ImmutableStorage) == TextureOptions.ImmutableStorage);
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="options">The texture's configuration options.</param>
        /// <returns>The instance of Texture2D that was created.</returns>
        public OpenGLTexture2D(UltravioletContext uv, Int32 width, Int32 height, TextureOptions options)
            : this(uv, IntPtr.Zero, width, height, TextureFormat.RGBA, options)
        { }

        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texture's texel format.</param>
        /// <param name="type">The texture's texel type.</param>
        /// <param name="data">The texture's data.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        /// <returns>The instance of Texture2D that was created.</returns>
        public OpenGLTexture2D(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, IntPtr data, Boolean immutable)
            : base(uv)
        {
            CreateNativeTexture(uv, internalformat, width, height, format, type, (void*)data, immutable);
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
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
        internal OpenGLTexture2D(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, IntPtr data, Boolean immutable, Boolean rbuffer)
            : base(uv)
        {
            this.rbuffer = rbuffer;
            CreateNativeTexture(uv, internalformat, width, height, format, type, (void*)data, immutable);
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture other)
        {
            var id1 = texture;
            var id2 = (other is IOpenGLResource glother) ? glother.OpenGLName : 0;
            return id1.CompareTo(id2);
        }

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(width >= 1, nameof(width));
            Contract.EnsureRange(height >= 1, nameof(height));

            if (BoundForReading || BoundForWriting)
                throw new InvalidOperationException(OpenGLStrings.InvalidOperationWhileBound);

            this.width = width;
            this.height = height;

            if (immutable)
                throw new InvalidOperationException(rbuffer ? OpenGLStrings.RenderBufferIsImmutable : OpenGLStrings.TextureIsImmutable);

            if (Ultraviolet.IsExecutingOnCurrentThread)
            {
                ProcessResize();
            }
            else
            {
                Ultraviolet.QueueWorkItem((state) => { ((OpenGLTexture2D)state).ProcessResize(); }, this);
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
                SetDataInternal_OnMainThread(0, null, data, 0, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(0, null, data, 0, sizeInBytes);
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
                SetDataInternal_OnMainThread(0, null, data, offsetInBytes, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(0, null, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(level >= 0, nameof(level));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = elementCount * sizeInBytesPerElement;
            var offsetInBytes = startIndex * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetDataInternal_OnMainThread(level, rect, data, offsetInBytes, sizeInBytes);
            else
                SetDataInternal_OnBackgroundThread(level, rect, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData<T>(IntPtr data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = elementCount * sizeInBytesPerElement;
            var offsetInBytes = startIndex * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(0, null, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(0, null, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, IntPtr data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(level >= 0, nameof(level));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));

            var sizeInBytesPerElement = Marshal.SizeOf(typeof(T));
            var sizeInBytes = elementCount * sizeInBytesPerElement;
            var offsetInBytes = startIndex * sizeInBytesPerElement;

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(level, rect, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(level, rect, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(offsetInBytes >= 0, nameof(offsetInBytes));

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(0, null, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(0, null, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetRawData(Int32 level, Rectangle? rect, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(level >= 0, nameof(level));
            Contract.EnsureRange(offsetInBytes >= 0, nameof(offsetInBytes));

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(level, rect, data, offsetInBytes, sizeInBytes);
            else
                SetRawDataInternal_OnBackgroundThread(level, rect, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData(Surface2D surface)
        {
            Contract.Require(surface, nameof(surface));
            Contract.EnsureNotDisposed(this, Disposed);

            if (surface.Width != Width || surface.Height != Height)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            if (Ultraviolet.IsExecutingOnCurrentThread)
                SetRawDataInternal_OnMainThread(0, null, surface.Pixels, 0, Width * Height * surface.BytesPerPixel);
            else
                SetRawDataInternal_OnBackgroundThread(0, null, surface.Pixels, 0, Width * Height * surface.BytesPerPixel);
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
        /// Creates the underlying native OpenGL texture with the specified format and data.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texel format.</param>
        /// <param name="type">The texel data type.</param>
        /// <param name="pixels">A pointer to the beginning of the texture's pixel data.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        private void CreateNativeTexture(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, void* pixels, Boolean immutable)
        {
            if (uv.IsRunningInServiceMode)
                throw new NotSupportedException(UltravioletStrings.NotSupportedInServiceMode);

            this.width = width;
            this.height = height;
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

                using (OpenGLState.ScopedCreateTexture2D(out glname))
                {
                    if (GL.IsTextureMaxLevelSupported)
                    {
                        GL.TextureParameteri(glname, GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAX_LEVEL, 0);
                        GL.ThrowIfError();
                    }

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_CLAMP_TO_EDGE);
                    GL.ThrowIfError();

                    GL.TextureParameteri(glname, GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_CLAMP_TO_EDGE);
                    GL.ThrowIfError();

                    if (immutable)
                    {
                        if (GL.IsTextureStorageAvailable)
                        {
                            GL.TextureStorage2D(glname, GL.GL_TEXTURE_2D, 1, internalformat, width, height);
                            GL.ThrowIfError();

                            if (pixels != null)
                            {
                                GL.TextureSubImage2D(glname, GL.GL_TEXTURE_2D, 0, 0, 0, width, height, format, type, pixels);
                                GL.ThrowIfError();
                            }
                        }
                        else
                        {
                            GL.TextureImage2D(glname, GL.GL_TEXTURE_2D, 0, (int)internalformat, width, height, 0, format, type, pixels);
                            GL.ThrowIfError();
                        }
                    }
                }

                if (!immutable)
                {
                    using (OpenGLState.ScopedBindTexture2D(glname, true))
                    {
                        GL.TexImage2D(GL.GL_TEXTURE_2D, 0, (int)internalformat, width, height, 0, format, type, pixels);
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
            using (OpenGLState.ScopedBindTexture2D(texture, true))
            {
                GL.TexImage2D(GL.GL_TEXTURE_2D, 0, (int)internalformat,
                    width, height, 0, format, type, null);
            }
        }

        /// <summary>
        /// Sets the texture's data from managed memory.
        /// </summary>
        private unsafe void SetDataInternal_OnMainThread(Int32 level, Rectangle? rect, Object data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                SetRawDataInternal_OnMainThread(level, rect, pData.AddrOfPinnedObject(), offsetInBytes, sizeInBytes);
            }
            finally { pData.Free(); }
        }

        /// <summary>
        /// Sets the texture's data from managed memory.
        /// </summary>
        private unsafe void SetDataInternal_OnBackgroundThread(Int32 level, Rectangle? rect, Object data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            Ultraviolet.QueueWorkItem(state =>
            {
                var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    SetRawDataInternal_OnMainThread(level, rect, pData.AddrOfPinnedObject(), 0, sizeInBytes);
                }
                finally { pData.Free(); }
            }, null, WorkItemOptions.ReturnNullOnSynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Sets the texture's data from native memory.
        /// </summary>
        private unsafe void SetRawDataInternal_OnMainThread(Int32 level, Rectangle? rect, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var region = rect ?? new Rectangle(0, 0, width, height);

            var pixelSizeInBytes = (format == GL.GL_RGB || format == GL.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height != sizeInBytes)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Upload(level, region, data, offsetInBytes);
        }

        /// <summary>
        /// Sets the texture's data from native memory.
        /// </summary>
        private unsafe void SetRawDataInternal_OnBackgroundThread(Int32 level, Rectangle? rect, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            var region = rect ?? new Rectangle(0, 0, width, height);
            var regionWidth = region.Width;
            var regionHeight = region.Height;

            var pixelSizeInBytes = (format == GL.GL_RGB || format == GL.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height != sizeInBytes)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Ultraviolet.QueueWorkItem(state =>
            {
                Upload(level, region, data, offsetInBytes);
            }, null, WorkItemOptions.ReturnNullOnSynchronousExecution)?.Wait();
        }

        /// <summary>
        /// Uploads texture data to the graphics device.
        /// </summary>
        private void Upload(Int32 level, Rectangle region, IntPtr ldata, Int32 offsetInBytes)
        {
            using (OpenGLState.ScopedBindTexture2D(OpenGLName))
            {
                var pData = ldata + offsetInBytes;
                GL.TextureSubImage2D(OpenGLName, GL.GL_TEXTURE_2D, level, region.X, region.Y,
                    region.Width, region.Height, format, type, (void*)pData);
                GL.ThrowIfError();
            }
        }

        // Property values.
        private UInt32 texture;
        private Int32 width;
        private Int32 height;
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
