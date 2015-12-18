using System;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the Texture2D class.
    /// </summary>
    public unsafe sealed class OpenGLTexture2D : Texture2D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="surface">The SDL surface from which to create the texture.</param>
        public OpenGLTexture2D(UltravioletContext uv, SDL_Surface surface)
            : base(uv)
        {
            Contract.Require(surface, "surface");

            var mode = gl.GL_NONE;
            var internalformat = gl.GL_NONE;
            if (surface.BytesPerPixel == 3)
            {
                mode = gl.GL_RGB;
                internalformat = gl.IsGLES2 ? gl.GL_RGB : gl.GL_RGB;
            }
            if (surface.BytesPerPixel == 4)
            {
                mode = gl.GL_RGBA;
                internalformat = gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8;
            }

            if (mode == gl.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            CreateNativeTexture(uv, internalformat, surface.Width, surface.Height, mode, 
                gl.GL_UNSIGNED_BYTE, surface.Native->pixels, true);
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

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture2D other)
        {
            var id1 = texture;
            var id2 = (other == null) ? 0 : ((IOpenGLResource)other).OpenGLName;
            return id1.CompareTo(id2);
        }

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(width >= 1, "width");
            Contract.EnsureRange(height >= 1, "height");

            if (BoundForReading || BoundForWriting)
                throw new InvalidOperationException(OpenGLStrings.InvalidOperationWhileBound);

            this.width = width;
            this.height = height;

            if (immutable)
                throw new InvalidOperationException(OpenGLStrings.TextureIsImmutable);

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
        public override void SetData<T>(T[] data, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            SetData(0, null, data, 0, (data == null) ? 0 : data.Length);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            SetData(0, null, data, offset, count);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);

                SetDataInternal(level, rect, handle.AddrOfPinnedObject(), offset, count, 0, TextureDataFormat.RGBA, origin);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, Int32 stride, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);

                SetDataInternal(level, rect, handle.AddrOfPinnedObject(), offset, count, stride, TextureDataFormat.RGBA, origin);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        /// <inheritdoc/>
        public override void SetData(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetDataInternal(level, rect, data, offset, count, stride, format, origin);
        }

        /// <inheritdoc/>
        public void BindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(boundRead, OpenGLStrings.ResourceAlreadyBound);

            boundRead = true;
        }

        /// <inheritdoc/>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(boundWrite, OpenGLStrings.ResourceAlreadyBound);

            boundWrite = true;
        }

        /// <inheritdoc/>
        public void UnbindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead, OpenGLStrings.ResourceNotBound);

            boundRead = false;
        }

        /// <inheritdoc/>
        public void UnbindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite, OpenGLStrings.ResourceNotBound);

            boundWrite = false;
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

                return boundRead;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForWriting
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundWrite;
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
                    ((OpenGLUltravioletGraphics)Ultraviolet.GetGraphics()).ReleaseReferences(this);
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
        /// Gets the OpenGL texture format flag that corresponds to the specified texture data format.
        /// </summary>
        /// <param name="format">The texture data format to convert.</param>
        /// <returns>The converted texture data format.</returns>
        private static UInt32 GetOpenGLTextureFormat(TextureDataFormat format)
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
        /// Converts the specified OpenGL sized texture format to its base format.
        /// </summary>
        /// <param name="internalformat">The sized internal texture format to convert.</param>
        /// <returns>The converted base internal texture format.</returns>
        private static UInt32 GetBaseInternalFormat(UInt32 internalformat)
        {
            switch (internalformat)
            {
                case gl.GL_RED:
                case gl.GL_R8:
                case gl.GL_R8_SNORM:
                case gl.GL_R16:
                case gl.GL_R16_SNORM:
                case gl.GL_R16F:
                case gl.GL_R32F:
                case gl.GL_R8I:
                case gl.GL_R8UI:
                case gl.GL_R16I:
                case gl.GL_R16UI:
                case gl.GL_R32I:
                case gl.GL_R32UI:
                    return gl.GL_RED;

                case gl.GL_RG:
                case gl.GL_RG8:
                case gl.GL_RG8_SNORM:
                case gl.GL_RG16:
                case gl.GL_RG16_SNORM:
                case gl.GL_RG16F:
                case gl.GL_RG32F:
                case gl.GL_RG8I:
                case gl.GL_RG8UI:
                case gl.GL_RG16I:
                case gl.GL_RG16UI:
                case gl.GL_RG32I:
                case gl.GL_RG32UI:
                    return gl.GL_RG;

                case gl.GL_RGB:
                case gl.GL_R3_G3_B2:
                case gl.GL_RGB4:
                case gl.GL_RGB5:
                case gl.GL_RGB8:
                case gl.GL_RGB8_SNORM:
                case gl.GL_RGB10:
                case gl.GL_RGB12:
                case gl.GL_RGB16_SNORM:
                case gl.GL_RGBA2:
                case gl.GL_RGBA4:
                case gl.GL_SRGB8:
                case gl.GL_RGB16F:
                case gl.GL_RGB32F:
                case gl.GL_R11F_G11F_B10F:
                case gl.GL_RGB9_E5:
                case gl.GL_RGB8I:
                case gl.GL_RGB8UI:
                case gl.GL_RGB16I:
                case gl.GL_RGB16UI:
                case gl.GL_RGB32I:
                case gl.GL_RGB32UI:
                    return gl.GL_RGB;

                case gl.GL_RGBA:
                case gl.GL_RGB5_A1:
                case gl.GL_RGBA8:
                case gl.GL_RGBA8_SNORM:
                case gl.GL_RGB10_A2:
                case gl.GL_RGB10_A2UI:
                case gl.GL_RGBA12:
                case gl.GL_RGBA16:
                case gl.GL_SRGB8_ALPHA8:
                case gl.GL_RGBA16F:
                case gl.GL_RGBA32F:
                case gl.GL_RGBA8I:
                case gl.GL_RGBA8UI:
                case gl.GL_RGBA16I:
                case gl.GL_RGBA16UI:
                case gl.GL_RGBA32I:
                case gl.GL_RGBA32UI:
                    return gl.GL_RGBA;

                default:
                    throw new ArgumentOutOfRangeException("internalformat");
            }
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

            this.width          = width;
            this.height         = height;
            this.internalformat = internalformat;
            this.format         = format;
            this.type           = type;
            this.immutable      = immutable;

            this.texture = uv.QueueWorkItemAndWait(() =>
            {
                uint glname;

                using (OpenGLState.ScopedCreateTexture2D(out glname))
                {
                    gl.TextureParameteri(glname, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_LEVEL, 0);
                    gl.ThrowIfError();

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
                            gl.TextureImage2D(glname, gl.GL_TEXTURE_2D, 0, (int)GetBaseInternalFormat(internalformat), 
                                width, height, 0, format, type, pixels);
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
            });
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
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data.</param>
        /// <param name="format">The format of the data being set.</param>
        /// <param name="origin">A <see cref="SetDataOrigin"/> value specifying the origin of the texture data in <paramref name="data"/>.</param>
        private unsafe void SetDataInternal(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format, SetDataOrigin origin)
        {
            var region = rect ?? new Rectangle(0, 0, width, height);
            if (region.Width * region.Height != count)
            {
                throw new InvalidOperationException(UltravioletStrings.BufferIsWrongSize);
            }

            const Int32 SizeOfTextureElementInBytes = 4;

            var flipHorizontally = (origin == SetDataOrigin.TopRight || origin == SetDataOrigin.BottomRight);
            var flipVertically   = (origin == SetDataOrigin.TopLeft || origin == SetDataOrigin.TopRight);

            TextureUtil.ReorientTextureData(data.ToPointer(), region.Width, region.Height, 
                SizeOfTextureElementInBytes, flipHorizontally, flipVertically);

            if (flipHorizontally)
            {
                region = new Rectangle((width - region.Width) - region.X, region.Y, region.Width, region.Height);
            }
            if (flipVertically)
            {
                region = new Rectangle(region.X, (height - region.Height) - region.Y, region.Width, region.Height);
            }

            using (OpenGLState.ScopedBindTexture2D(texture))
            {
                gl.PixelStorei(gl.GL_UNPACK_ROW_LENGTH, stride);
                gl.ThrowIfError();

                gl.PixelStorei(gl.GL_UNPACK_ALIGNMENT, 1);
                gl.ThrowIfError();

                gl.TextureSubImage2D(texture, gl.GL_TEXTURE_2D, level, region.X, region.Y, region.Width, region.Height, 
                    GetOpenGLTextureFormat(format), gl.GL_UNSIGNED_BYTE, data.ToPointer());
                gl.ThrowIfError();

                gl.PixelStorei(gl.GL_UNPACK_ROW_LENGTH, 0);
                gl.ThrowIfError();
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
        private Boolean boundRead;
        private Boolean boundWrite;
    }
}
