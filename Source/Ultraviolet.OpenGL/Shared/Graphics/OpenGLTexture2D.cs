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
        /// <param name="bytesPerPixel">The number of bytes which represent each pixel in the raw data.</param>
        public OpenGLTexture2D(UltravioletContext uv, IntPtr pixels, Int32 width, Int32 height, Int32 bytesPerPixel)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));
            Contract.EnsureRange(bytesPerPixel >= 3 || bytesPerPixel <= 4, nameof(bytesPerPixel));

            var format = GetOpenGLTextureFormatFromBytesPerPixel(bytesPerPixel);
            var internalformat = GetOpenGLTextureInternalFormatFromBytesPerPixel(bytesPerPixel);

            if (format == gl.GL_NONE || internalformat == gl.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            CreateNativeTexture(uv, internalformat, width, height, format, 
                gl.GL_UNSIGNED_BYTE, (void*)pixels, true);
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="immutable">A value indicating whether to use immutable texture storage.</param>
        /// <returns>The instance of Texture2D that was created.</returns>
        public OpenGLTexture2D(UltravioletContext uv, Int32 width, Int32 height, Boolean immutable)
            : base(uv)
        {
            CreateNativeTexture(uv, gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8, width, height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, null, immutable);
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
            var id2 = (other == null) ? 0 : ((IOpenGLResource)other).OpenGLName;
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

            SetDataInternal(0, null, data, 0, data.Length);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            SetDataInternal(0, null, data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(level >= 0, nameof(level));
            Contract.EnsureRange(startIndex >= 0, nameof(startIndex));
            Contract.EnsureRange(elementCount >= 0 && startIndex + elementCount <= data.Length, nameof(elementCount));

            SetDataInternal(level, rect, data, startIndex, elementCount);
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
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return texture;
            }
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
                        gl.DeleteTexture(((OpenGLTexture2D)state).texture);
                        gl.ThrowIfError();
                    }, this);
                }
            }

            base.Dispose(disposing);
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

            this.texture = uv.QueueWorkItem(state =>
            {
                uint glname;

                using (OpenGLState.ScopedCreateTexture2D(out glname))
                {
                    if (!gl.IsGLES2)
                    {
                        gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_LEVEL, 0);
                        gl.ThrowIfError();
                    }

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_S, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_T, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    if (immutable)
                    {
                        if (gl.IsTextureStorageAvailable)
                        {
                            gl.TextureStorage2D(glname, gl.GL_TEXTURE_2D, 1, internalformat, width, height);
                            gl.ThrowIfError();

                            if (pixels != null)
                            {
                                gl.TextureSubImage2D(glname, gl.GL_TEXTURE_2D, 0, 0, 0, width, height, format, type, pixels);
                                gl.ThrowIfError();
                            }
                        }
                        else
                        {
                            gl.TextureImage2D(glname, gl.GL_TEXTURE_2D, 0, (int)internalformat, width, height, 0, format, type, pixels);
                            gl.ThrowIfError();
                        }
                    }
                }

                if (!immutable)
                {
                    using (OpenGLState.ScopedBindTexture2D(glname, true))
                    {
                        gl.TexImage2D(gl.GL_TEXTURE_2D, 0, (int)internalformat, width, height, 0, format, type, pixels);
                        gl.ThrowIfError();
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
                gl.TexImage2D(gl.GL_TEXTURE_2D, 0, (int)internalformat,
                    width, height, 0, format, type, null);
            }
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        private unsafe void SetDataInternal<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount) where T : struct
        {
            var elementSizeInBytes = Marshal.SizeOf(typeof(T));

            var region = rect ?? new Rectangle(0, 0, width, height);
            var regionWidth = region.Width;
            var regionHeight = region.Height;

            var pixelSizeInBytes = (format == gl.GL_RGB || format == gl.GL_BGR) ? 3 : 4;
            if (pixelSizeInBytes * width * height != elementSizeInBytes * elementCount)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            Ultraviolet.QueueWorkItem(state =>
            {
                var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    using (OpenGLState.ScopedBindTexture2D(OpenGLName))
                    {
                        var pData = dataHandle.AddrOfPinnedObject() + (startIndex * elementSizeInBytes);
                        gl.TextureSubImage2D(OpenGLName, gl.GL_TEXTURE_2D, level, region.X, region.Y,
                            region.Width, region.Height, format, type, (void*)pData);
                        gl.ThrowIfError();
                    }
                }
                finally { dataHandle.Free(); }
            }).Wait();
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
        private Int32 boundRead;
        private Int32 boundWrite;
    }
}
