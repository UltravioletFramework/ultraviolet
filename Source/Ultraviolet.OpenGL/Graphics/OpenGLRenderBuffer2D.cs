using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the RenderBuffer2D class.
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

            var isSrgb = (options & RenderBufferOptions.SrgbColor) == RenderBufferOptions.SrgbColor;
            var isLinear = (options & RenderBufferOptions.LinearColor) == RenderBufferOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.BuffersCannotHaveMultipleEncodings);

            if ((isSrgb || isLinear) && format != RenderBufferFormat.Color)
                throw new ArgumentException(UltravioletStrings.EncodingSpecifiedForNonColorBuffer);

            var caps = uv.GetGraphics().Capabilities;
            var srgbEncoded = (isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForRenderBuffer2D)) && caps.SrgbEncodingEnabled;

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
                        {
                            var rhiFormat = TextureFormat.RGBA;
                            var texformat = OpenGLTextureUtil.GetGLFormatFromTextureFormat(rhiFormat);
                            var texinternalformat = OpenGLTextureUtil.GetInternalGLFormatFromTextureFormat(rhiFormat, srgbEncoded);
                            this.texture = new OpenGLTexture2D(uv, texinternalformat, width, height, texformat, GL.GL_UNSIGNED_BYTE, IntPtr.Zero, immutable, true);
                            this.SrgbEncoded = this.texture.SrgbEncoded;
                        }
                        break;

                    case RenderBufferFormat.Depth24Stencil8:
                        this.texture = new OpenGLTexture2D(uv, GL.GL_DEPTH24_STENCIL8, width, height, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Depth32:
                        this.texture = new OpenGLTexture2D(uv, GL.GL_DEPTH_COMPONENT32, width, height, GL.GL_DEPTH_COMPONENT, GL.GL_UNSIGNED_INT, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Depth16:
                        this.texture = new OpenGLTexture2D(uv, GL.GL_DEPTH_COMPONENT16, width, height, GL.GL_DEPTH_COMPONENT, GL.GL_UNSIGNED_SHORT, IntPtr.Zero, immutable, true);
                        break;

                    case RenderBufferFormat.Stencil8:
                        this.texture = new OpenGLTexture2D(uv, GL.GL_STENCIL_INDEX8, width, height, GL.GL_STENCIL, GL.GL_UNSIGNED_INT, IntPtr.Zero, immutable, true);
                        break;

                    default:
                        throw new NotSupportedException(nameof(format));
                }
            }
        }

        /// <inheritdoc/>
        public override Int32 CompareTo(Texture other)
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
        public override void SetData<T>(T[] data)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(data);
        }

        /// <inheritdoc/>
        public override void SetData<T>(T[] data, Int32 startIndex, Int32 elementCount)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, T[] data, Int32 startIndex, Int32 elementCount)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(level, rect, data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetData<T>(IntPtr data, Int32 startIndex, Int32 elementCount)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData<T>(data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetData<T>(Int32 level, Rectangle? rect, IntPtr data, Int32 startIndex, Int32 elementCount)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData<T>(level, rect, data, startIndex, elementCount);
        }

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetRawData(data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetRawData(Int32 level, Rectangle? rect, IntPtr data, Int32 offsetInBytes, Int32 sizeInBytes)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetRawData(level, rect, data, offsetInBytes, sizeInBytes);
        }

        /// <inheritdoc/>
        public override void SetData(Surface2D surface)
        {
            if (willNotBeSampled)
                throw new NotSupportedException(OpenGLStrings.RenderBufferWillNotBeSampled);

            texture.SetData(surface);
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
        public UInt32 OpenGLName => willNotBeSampled ? renderbuffer : texture.OpenGLName;

        /// <inheritdoc/>
        public override RenderBufferFormat Format => format;

        /// <inheritdoc/>
        public override Boolean SrgbEncoded { get; }

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
        public override Boolean WillNotBeSampled => willNotBeSampled;

        /// <inheritdoc/>
        public override Boolean Attached => attached;

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
                    var glname = renderbuffer;

                    if (!Ultraviolet.Disposed && glname != 0)
                    {
                        Ultraviolet.QueueWorkItem((state) =>
                        {
                            GL.DeleteRenderBuffers(glname);
                            GL.ThrowIfError();
                        }, null, WorkItemOptions.ReturnNullOnSynchronousExecution);
                    }

                    renderbuffer = 0;
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
                        {
                            var rhiFormat = TextureFormat.RGBA;
                            var internalformat = OpenGLTextureUtil.GetInternalGLFormatFromTextureFormat(rhiFormat, SrgbEncoded);
                            GL.RenderbufferStorage(GL.GL_RENDERBUFFER, internalformat, width, height);
                            GL.ThrowIfError();
                        }
                        break;

                    case RenderBufferFormat.Depth24Stencil8:
                        GL.RenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH24_STENCIL8, width, height);
                        GL.ThrowIfError();
                        break;

                    case RenderBufferFormat.Depth32:
                        GL.RenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH_COMPONENT32, width, height);
                        GL.ThrowIfError();
                        break;

                    case RenderBufferFormat.Depth16:
                        GL.RenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH_COMPONENT16, width, height);
                        GL.ThrowIfError();
                        break;

                    case RenderBufferFormat.Stencil8:
                        GL.RenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_STENCIL_INDEX8, width, height);
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
        private UInt32 renderbuffer;
    }
}
