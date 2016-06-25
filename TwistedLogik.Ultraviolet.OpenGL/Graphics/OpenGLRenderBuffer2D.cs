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
        /// <param name="options">The render buffer's configuration options.</param>
        public OpenGLRenderBuffer2D(UltravioletContext uv, RenderBufferFormat format, Int32 width, Int32 height, RenderBufferOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            this.format = format;
            this.width = width;
            this.height = height;
            this.immutable = (options & RenderBufferOptions.ImmutableStorage) == RenderBufferOptions.ImmutableStorage;
            this.willNotBeSampled = (options & RenderBufferOptions.WillNotBeSampled) == RenderBufferOptions.WillNotBeSampled;

            if (willNotBeSampled)
            {
                using (var state = OpenGLState.ScopedCreateRenderbuffer(out renderbuffer))
                {
                    AllocateRenderbufferStorage(width, height);
                }
            }
            else
            {
                switch (format)
                {
                    case RenderBufferFormat.Color:
                        this.texture = new OpenGLTexture2D(uv, gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8, width, height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Depth24Stencil8:
                        this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH24_STENCIL8, width, height, gl.GL_DEPTH_STENCIL, gl.GL_UNSIGNED_INT_24_8, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Depth32:
                        this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH_COMPONENT32, width, height, gl.GL_DEPTH_COMPONENT, gl.GL_UNSIGNED_INT, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Depth16:
                        this.texture = new OpenGLTexture2D(uv, gl.GL_DEPTH_COMPONENT16, width, height, gl.GL_DEPTH_COMPONENT, gl.GL_UNSIGNED_SHORT, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Stencil8:
                        this.texture = new OpenGLTexture2D(uv, gl.GL_STENCIL_INDEX8, width, height, gl.GL_STENCIL, gl.GL_UNSIGNED_INT, IntPtr.Zero, immutable, true);
                        break;

                    default:
                        throw new NotSupportedException(nameof(format));
                }
            }
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture2D other)
        {
            return texture.CompareTo(other);
        }

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(attached, OpenGLStrings.CannotResizeAttachedRenderBuffer);

            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            this.texture.Resize(width, height);
            
            this.width  = width;
            this.height = height;
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData<T>(data, origin);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 offset, Int32 count, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(data, offset, count, origin);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(level, rect, data, offset, count, 0, origin);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 offset, Int32 count, Int32 stride, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(level, rect, data, offset, count, stride, origin);
        }

        /// <inheritdoc/>
        public override void SetData(Int32 level, Rectangle? rect, IntPtr data, Int32 offset, Int32 count, Int32 stride, TextureDataFormat format, SetDataOrigin origin = SetDataOrigin.TopLeft)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(level, rect, data, offset, count, stride, format, origin);
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

        /// <summary>
        /// Marks the render buffer as being attached to a render target.
        /// </summary>
        public void MarkAttached()
        {
            if (attached)
                throw new InvalidOperationException(OpenGLStrings.RenderBufferAlreadyAttached);

            attached = true;
        }

        /// <inheritdoc/>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return willNotBeSampled ? renderbuffer : texture.OpenGLName;
            }
        }

        /// <inheritdoc/>
        public override RenderBufferFormat Format
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return format;
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

                return willNotBeSampled;
            }
        }

        /// <inheritdoc/>
        public override Boolean Attached
        {
            get { return attached; }
        }

        /// <summary>
        /// Resizes the render buffer. This method should only be called by the render target
        /// to which this buffer is attached.
        /// </summary>
        /// <param name="width">The render buffer's new width in pixels.</param>
        /// <param name="height">The render target's new width in pixels.</param>
        internal void ResizeInternal(Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (willNotBeSampled)
            {
                AllocateRenderbufferStorage(width, height);
            }
            else
            {
                this.texture.Resize(width, height);
            }

            this.width  = width;
            this.height = height;
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
                    ((OpenGLUltravioletGraphics)Ultraviolet.GetGraphics()).UnbindTexture(this);

                if (willNotBeSampled)
                {
                    if (!Ultraviolet.Disposed && renderbuffer != 0)
                    {
                        Ultraviolet.QueueWorkItem((state) =>
                        {
                            gl.DeleteRenderBuffers(((OpenGLRenderBuffer2D)state).renderbuffer);
                            gl.ThrowIfError();
                        }, this);
                    }
                }
                else
                {
                    SafeDispose.Dispose(texture);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Reallocates the renderbuffer's storage.
        /// </summary>
        /// <param name="width">The renderbuffer's width in pixels.</param>
        /// <param name="height">The renderbuffer's height in pixels.</param>
        private void AllocateRenderbufferStorage(Int32 width, Int32 height)
        {
            using (OpenGLState.ScopedBindRenderbuffer(renderbuffer, true))
            {
                switch (format)
                {
                    case RenderBufferFormat.Color:
                        gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.IsGLES2 ? gl.GL_RGBA4 : gl.GL_RGBA8, width, height);
                        gl.ThrowIfError();
                        break;

                    case RenderBufferFormat.Depth24Stencil8:
                        gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_DEPTH24_STENCIL8, width, height);
                        gl.ThrowIfError();
                        break;

                    case RenderBufferFormat.Depth32:
                        gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_DEPTH_COMPONENT32, width, height);
                        gl.ThrowIfError();
                        break;

                    case RenderBufferFormat.Depth16:
                        gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_DEPTH_COMPONENT16, width, height);
                        gl.ThrowIfError();
                        break;

                    case RenderBufferFormat.Stencil8:
                        gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_STENCIL_INDEX8, width, height);
                        break;

                    default:
                        throw new NotSupportedException("format");
                }
            }
        }
        
        // Property values.
        private readonly RenderBufferFormat format;
        private Int32 width;
        private Int32 height;
        private Int32 boundRead;
        private Int32 boundWrite;
        private Boolean attached;
        private readonly Boolean immutable;
        private readonly Boolean willNotBeSampled;

        // State values.
        private readonly OpenGLTexture2D texture;
        private readonly UInt32 renderbuffer;
    }
}
