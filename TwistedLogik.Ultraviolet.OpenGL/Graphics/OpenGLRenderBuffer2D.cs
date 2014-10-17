using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the RenderBuffer2D class.
    /// </summary>
    public sealed class OpenGLRenderBuffer2D : RenderBuffer2D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLRenderBuffer2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="format">The render buffer's format.</param>
        /// <param name="width">The render buffer's width in pixels.</param>
        /// <param name="height">The render buffer's height in pixels.</param>
        public OpenGLRenderBuffer2D(UltravioletContext uv, RenderBufferFormat format, Int32 width, Int32 height)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            this.format = format;
            this.width = width;
            this.height = height;

            switch (format)
            {
                case RenderBufferFormat.Color:
                    this.texture = new OpenGLTexture2D(uv, gl.GL_RGBA8, width, height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, IntPtr.Zero);
                    break;

                case RenderBufferFormat.Depth24Stencil8:
                    this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH24_STENCIL8, width, height, gl.GL_DEPTH_STENCIL, gl.GL_UNSIGNED_INT_24_8, IntPtr.Zero);
                    break;

                case RenderBufferFormat.Depth32:
                    this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH_COMPONENT32, width, height, gl.GL_DEPTH_COMPONENT, gl.GL_UNSIGNED_INT, IntPtr.Zero);
                    break;

                case RenderBufferFormat.Depth16:
                    this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH_COMPONENT16, width, height, gl.GL_DEPTH_COMPONENT, gl.GL_UNSIGNED_SHORT, IntPtr.Zero);
                    break;

                default:
                    throw new NotSupportedException("format");
            }
        }

        /// <summary>
        /// Compares the texture with another texture and returns a value indicating whether the current
        /// instance comes before, after, or in the same position as the specified texture.
        /// </summary>
        /// <param name="other">The texture to compare to this instance.</param>
        /// <returns>A value indicating the relative order of the objects being compared.</returns>
        public override Int32 CompareTo(Texture2D other)
        {
            return texture.CompareTo(other);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        public override void SetData<T>(T[] data)
        {
            texture.SetData<T>(data);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count)
        {
            texture.SetData(data, offset, count);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set, or null to set the entire texture.</param>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data, or zero to use the width of <paramref name="rect"/>.</param>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, Int32 stride = 0)
        {
            texture.SetData(level, rect, data, offset, count, stride);
        }

        /// <summary>
        /// Sets the texture's data.
        /// </summary>
        /// <param name="level">The mipmap level for which to set data.</param>
        /// <param name="rect">A rectangle describing the position and size of the data to set, or null to set the entire texture.</param>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="offset">The index of the first element to set.</param>
        /// <param name="count">The number of elements to set.</param>
        /// <param name="stride">The number of elements in one row of data, or zero to use the width of <paramref name="rect"/>.</param>
        /// <param name="format">The format of the data being set.</param>
        public override void SetData(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format)
        {
            texture.SetData(level, rect, data, offset, count, stride, format);
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
        /// Gets the OpenGL renderbuffer handle.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return texture.OpenGLName;
            }
        }

        /// <summary>
        /// Gets the render buffer's format.
        /// </summary>
        public override RenderBufferFormat Format
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return format;
            }
        }

        /// <summary>
        /// Gets the render buffer's width.
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
        /// Gets the render buffer's height.
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
                SafeDispose.Dispose(texture);
            }

            base.Dispose(disposing);
        }

        // Property values.
        private readonly RenderBufferFormat format;
        private readonly Int32 width;
        private readonly Int32 height;
        private Boolean boundRead;
        private Boolean boundWrite;

        // State values.
        private readonly OpenGLTexture2D texture;
    }
}
