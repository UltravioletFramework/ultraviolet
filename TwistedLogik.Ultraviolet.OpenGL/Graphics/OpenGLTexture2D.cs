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
            if (surface.BytesPerPixel == 3) mode = gl.GL_RGB;
            if (surface.BytesPerPixel == 4) mode = gl.GL_RGBA;

            if (mode == gl.GL_NONE)
                throw new NotSupportedException(OpenGLStrings.UnsupportedImageType);

            this.width = surface.Width;
            this.height = surface.Height;            
            this.texture = CreateNativeTexture(uv, gl.GL_RGBA8, width, height, mode, gl.GL_UNSIGNED_BYTE, surface.Native->pixels);
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLTexture2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <returns>The instance of Texture2D that was created.</returns>
        public OpenGLTexture2D(UltravioletContext uv, Int32 width, Int32 height)
            : base(uv)
        {
            this.width = width;
            this.height = height;
            this.texture = CreateNativeTexture(uv, gl.GL_RGBA8, width, height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, null);
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
        /// <returns>The instance of Texture2D that was created.</returns>
        public OpenGLTexture2D(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, IntPtr data)
            : base(uv)
        {
            this.width = width;
            this.height = height;
            this.texture = CreateNativeTexture(uv, internalformat, width, height, format, type, (void*)data);
        }

        /// <summary>
        /// Compares the texture with another texture and returns a value indicating whether the current
        /// instance comes before, after, or in the same position as the specified texture.
        /// </summary>
        /// <param name="other">The texture to compare to this instance.</param>
        /// <returns>A value indicating the relative order of the objects being compared.</returns>
        public override Int32 CompareTo(Texture2D other)
        {
            var id1 = texture;
            var id2 = (other == null) ? 0 : ((IOpenGLResource)other).OpenGLName;
            return id1.CompareTo(id2);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        public override void SetData<T>(T[] data)
        {
            SetData(0, null, data, 0, (data == null) ? 0 : data.Length);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count)
        {
            SetData(0, null, data, offset, count);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set.</param>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data.</param>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, Int32 stride = 0)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                SetDataInternal(level, rect, pData.AddrOfPinnedObject(), offset, count, stride, TextureDataFormat.RGBA);
            }
            finally
            {
                pData.Free();
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
        public override void SetData(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetDataInternal(level, rect, data, offset, count, stride, format);
        }

        /// <summary>
        /// Binds the resource for reading.
        /// </summary>
        public void BindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(boundRead, OpenGLStrings.ResourceAlreadyBound);

            boundRead = true;
        }

        /// <summary>
        /// Binds the resource for writing.
        /// </summary>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(boundWrite, OpenGLStrings.ResourceAlreadyBound);

            boundWrite = true;
        }

        /// <summary>
        /// Unbinds the resource for reading.
        /// </summary>
        public void UnbindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundRead, OpenGLStrings.ResourceNotBound);

            boundRead = false;
        }

        /// <summary>
        /// Unbinds the resource for reading.
        /// </summary>
        public void UnbindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(boundWrite, OpenGLStrings.ResourceNotBound);

            boundWrite = false;
        }

        /// <summary>
        /// Gets the OpenGL texture handle.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return texture;
            }
        }

        /// <summary>
        /// Gets the texture's width in pixels.
        /// </summary>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return width; 
            }
        }

        /// <summary>
        /// Gets the texture's height in pixels.
        /// </summary>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return height;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the texture is bound to the device for reading.
        /// </summary>
        public override Boolean BoundForReading
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundRead;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the texture is bound to the device for writing.
        /// </summary>
        public override Boolean BoundForWriting
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return boundWrite;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
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
        /// Creates a native OpenGL texture with the specified format and data.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="internalformat">The texture's internal format.</param>
        /// <param name="width">The texture's width in pixels.</param>
        /// <param name="height">The texture's height in pixels.</param>
        /// <param name="format">The texel format.</param>
        /// <param name="type">The texel data type.</param>
        /// <param name="pixels">A pointer to the beginning of the texture's pixel data.</param>
        /// <returns>The identifier of the OpenGL texture resource that was created.</returns>
        private static UInt32 CreateNativeTexture(UltravioletContext uv, UInt32 internalformat, Int32 width, Int32 height, UInt32 format, UInt32 type, void* pixels)
        {
            return uv.QueueWorkItemAndWait(() =>
            {
                uint texture;

                using (OpenGLState.ScopedCreateTexture2D(out texture))
                {
                    gl.TextureParameteri(texture, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAX_LEVEL, 0);
                    gl.ThrowIfError();

                    gl.TextureParameteri(texture, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(texture, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, (int)gl.GL_LINEAR);
                    gl.ThrowIfError();

                    gl.TextureParameteri(texture, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_S, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    gl.TextureParameteri(texture, gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_T, (int)gl.GL_CLAMP_TO_EDGE);
                    gl.ThrowIfError();

                    if (gl.IsTextureStorageAvailable)
                    {
                        gl.TextureStorage2D(texture, gl.GL_TEXTURE_2D, 1, internalformat, width, height);
                        gl.ThrowIfError();

                        if (pixels != null)
                        {
                            gl.TextureSubImage2D(texture, gl.GL_TEXTURE_2D, 0, 0, 0, width, height, format, type, pixels);
                            gl.ThrowIfError();
                        }
                    }
                    else
                    {
                        gl.TextureImage2D(texture, gl.GL_TEXTURE_2D, 0, (int)internalformat, width, height, 0, format, type, pixels);
                        gl.ThrowIfError();
                    }
                }

                return texture;
            });
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
        private void SetDataInternal(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format)
        {
            int xoffset = 0;
            int yoffset = 0;
            int width = this.width;
            int height = this.height;
            if (rect.HasValue)
            {
                var rectval = rect.GetValueOrDefault();
                xoffset = rectval.X;
                yoffset = rectval.Y;
                width = rectval.Width;
                height = rectval.Height;
            }

            using (OpenGLState.ScopedBindTexture2D(texture))
            {
                gl.PixelStorei(gl.GL_UNPACK_ROW_LENGTH, stride);
                gl.ThrowIfError();

                gl.PixelStorei(gl.GL_UNPACK_ALIGNMENT, 1);
                gl.ThrowIfError();

                gl.TextureSubImage2D(texture, gl.GL_TEXTURE_2D, level, xoffset, yoffset, width, height, GetOpenGLTextureFormat(format), gl.GL_UNSIGNED_BYTE, data.ToPointer());
                gl.ThrowIfError();

                gl.PixelStorei(gl.GL_UNPACK_ROW_LENGTH, 0);
                gl.ThrowIfError();
            }
        }

        // Property values.
        private readonly UInt32 texture;
        private readonly Int32 width;
        private readonly Int32 height;
        private Boolean boundRead;
        private Boolean boundWrite;
    }
}
