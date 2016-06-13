using System;
using System.Collections.Generic;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the RenderTarget2D class.
    /// </summary>
    public sealed class OpenGLRenderTarget2D : RenderTarget2D, IOpenGLResource, IBindableResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLRenderTarget2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The render target's width in pixels.</param>
        /// <param name="height">The render target's height in pixels.</param>
        /// <param name="usage">A <see cref="RenderTargetUsage"/> value specifying whether the 
        /// render target's data is discarded or preserved when it is bound to the graphics device.</param>
        /// <param name="buffers">The collection of render buffers to attach to the target.</param>
        public OpenGLRenderTarget2D(UltravioletContext uv, Int32 width, Int32 height, RenderTargetUsage usage, IEnumerable<RenderBuffer2D> buffers = null)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            // NOTE: If we're in an older version of GLES, we need to use glFramebufferTexture2D()
            glFramebufferTextureIsSupported = !gl.IsGLES || gl.IsVersionAtLeast(3, 2);

            var framebuffer = 0u;

            uv.QueueWorkItemAndWait(() =>
            {
                using (OpenGLState.ScopedCreateFramebuffer(out framebuffer))
                {
                    if (buffers != null)
                    {
                        foreach (OpenGLRenderBuffer2D buffer in buffers)
                        {
                            AttachRenderBuffer(buffer);
                        }
                    }
                }
            });

            this.width = width;
            this.height = height;
            this.framebuffer = framebuffer;
            this.renderTargetUsage = usage;
        }

        /// <inheritdoc/>
        public override void Attach(RenderBuffer2D buffer)
        {
            Contract.Require(buffer, nameof(buffer));
            Contract.Ensure<ArgumentException>(
                buffer.Width == width && 
                buffer.Height == height, OpenGLStrings.RenderBufferIsWrongSize);
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(buffer);

            var sdlBuffer = (OpenGLRenderBuffer2D)buffer;

            Ultraviolet.QueueWorkItemAndWait(() =>
            {
                using (OpenGLState.ScopedBindFramebuffer(framebuffer))
                {
                    AttachRenderBuffer(sdlBuffer);

                    framebufferStatus = gl.CheckNamedFramebufferStatus(framebuffer, gl.GL_FRAMEBUFFER);
                    gl.ThrowIfError();
                }
            });

            sdlBuffer.MarkAttached();

            buffers.Add(sdlBuffer);
        }

        /// <inheritdoc/>
        public override void Resize(Int32 width, Int32 height)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(width >= 1, nameof(width));
            Contract.EnsureRange(height >= 1, nameof(height));

            if (this.width == width && this.height == height)
                return;

            foreach (var buffer in buffers)
            {
                buffer.ResizeInternal(width, height);
            }

            this.width  = width;
            this.height = height;
        }

        /// <inheritdoc/>
        public override void GetData(Color[] data)
        {
            Contract.Require(data, nameof(data));

            var bufferTargetSize = Width * Height;
            if (bufferTargetSize != data.Length)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            GetDataInternal(data, new Rectangle(0, 0, Width, Height));
        }

        /// <inheritdoc/>
        public override void GetData(Color[] data, Rectangle region)
        {
            Contract.Require(data, nameof(data));
            Contract.EnsureRange(region.X >= 0 && region.X < Width, nameof(region));
            Contract.EnsureRange(region.Y >= 0 && region.Y < Height, nameof(region));
            Contract.EnsureRange(region.Width > 0 && region.X + region.Width < Width, nameof(region));
            Contract.EnsureRange(region.Height > 0 && region.Y + region.Height < Height, nameof(region));

            var bufferTargetSize = region.Width * region.Height;
            if (bufferTargetSize != data.Length)
                throw new ArgumentException(UltravioletStrings.BufferIsWrongSize);

            GetDataInternal(data, region);
        }

        /// <summary>
        /// Validates that the framebuffer is in a state which is ready for rendering.
        /// </summary>
        public void ValidateStatus()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (colorAttachments == 0 && depthAttachments == 0 && stencilAttachments == 0)
                throw new InvalidOperationException(OpenGLStrings.RenderTargetNeedsBuffers);

            if (framebufferStatus != gl.GL_FRAMEBUFFER_COMPLETE)
            {
                switch (framebufferStatus)
                {
                    case gl.GL_FRAMEBUFFER_UNDEFINED:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_UNDEFINED"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER"));

                    case gl.GL_FRAMEBUFFER_UNSUPPORTED:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_UNSUPPORTED"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE"));

                    case gl.GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS"));

                    default:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format(framebufferStatus));
                }
            }
        }

        /// <summary>
        /// Binds the resource for reading.
        /// </summary>
        public void BindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(BoundForWriting, OpenGLStrings.ResourceCannotBeReadWhileWriting);

            foreach (var buffer in buffers)
                buffer.BindRead();
        }

        /// <summary>
        /// Binds the resource for writing.
        /// </summary>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(BoundForReading, OpenGLStrings.ResourceCannotBeWrittenWhileReading);

            foreach (var buffer in buffers)
                buffer.BindWrite();
        }

        /// <summary>
        /// Unbinds the resource for reading.
        /// </summary>
        public void UnbindRead()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(BoundForReading, OpenGLStrings.ResourceNotBound);

            foreach (var buffer in buffers)
                buffer.UnbindRead();
        }

        /// <summary>
        /// Unbinds the resource for reading.
        /// </summary>
        public void UnbindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(BoundForWriting, OpenGLStrings.ResourceNotBound);

            foreach (var buffer in buffers)
                buffer.UnbindWrite();
        }

        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return framebuffer; 
            }
        }

        /// <inheritdoc/>
        public override Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Size2(width, height);
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

                foreach (var buffer in buffers)
                {
                    if (buffer.BoundForReading)
                        return true;
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public override Boolean BoundForWriting
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                foreach (var buffer in buffers)
                {
                    if (buffer.BoundForWriting)
                        return true;
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public override RenderTargetUsage RenderTargetUsage
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return renderTargetUsage;
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
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteFramebuffer(((OpenGLRenderTarget2D)state).framebuffer);
                        gl.ThrowIfError();
                    }, this);
                }
                buffers.Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Attaches a render buffer to the render target.
        /// </summary>
        /// <param name="buffer">The render buffer to attach to the render target.</param>
        private void AttachRenderBuffer(OpenGLRenderBuffer2D buffer)
        {
            switch (buffer.Format)
            {
                case RenderBufferFormat.Color:
                    AttachColorBuffer(buffer);
                    break;

                case RenderBufferFormat.Depth32:
                case RenderBufferFormat.Depth16:
                    AttachDepthBuffer(buffer);
                    break;

                case RenderBufferFormat.Depth24Stencil8:
                    AttachDepthStencilBuffer(buffer);
                    break;

                case RenderBufferFormat.Stencil8:
                    AttachStencilBuffer(buffer);
                    break;

                default:
                    throw new NotSupportedException();
            }

            UpdateDrawBuffers();
        }
        
        /// <summary>
        /// Attaches a color buffer to the render target.
        /// </summary>
        /// <param name="buffer">The color buffer to attach to the render target.</param>
        private void AttachColorBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(colorAttachments < 16, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                gl.NamedFramebufferRenderbuffer(framebuffer, gl.GL_FRAMEBUFFER,
                    (uint)(gl.GL_COLOR_ATTACHMENT0 + colorAttachments), gl.GL_RENDERBUFFER, buffer.OpenGLName);
                gl.ThrowIfError();
            }
            else
            {
                if (!glFramebufferTextureIsSupported)
                {
                    gl.FramebufferTexture2D(gl.GL_FRAMEBUFFER, 
                        (uint)(gl.GL_COLOR_ATTACHMENT0 + colorAttachments), gl.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
                else
                {
                    gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER,
                        (uint)(gl.GL_COLOR_ATTACHMENT0 + colorAttachments), buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
            }
                        
            colorAttachments++;
        }

        /// <summary>
        /// Attaches a depth buffer to the render target.
        /// </summary>
        /// <param name="buffer">The depth buffer to attach to the render target.</param>
        private void AttachDepthBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(depthAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                gl.NamedFramebufferRenderbuffer(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_ATTACHMENT, gl.GL_RENDERBUFFER, buffer.OpenGLName);
                gl.ThrowIfError();
            }
            else
            {
                if (!glFramebufferTextureIsSupported)
                {
                    gl.FramebufferTexture2D(gl.GL_FRAMEBUFFER, gl.GL_DEPTH_ATTACHMENT, gl.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
                else
                {
                    gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_ATTACHMENT, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
            }

            depthAttachments++;
        }

        /// <summary>
        /// Attaches a depth/stencil buffer to the render target.
        /// </summary>
        /// <param name="buffer">The depth/stencil buffer to attach to the render target.</param>
        private void AttachDepthStencilBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(depthAttachments == 0 && stencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                gl.NamedFramebufferRenderbuffer(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_STENCIL_ATTACHMENT, gl.GL_RENDERBUFFER, buffer.OpenGLName);
                gl.ThrowIfError();
            }
            else
            {
                if (!glFramebufferTextureIsSupported)
                {
                    gl.FramebufferTexture2D(gl.GL_FRAMEBUFFER, gl.GL_DEPTH_STENCIL_ATTACHMENT, gl.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
                else
                {
                    gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
            }

            depthAttachments++;
            stencilAttachments++;
        }

        /// <summary>
        /// Attaches a stencil buffer to the render target.
        /// </summary>
        /// <param name="buffer">The stencil buffer to attach to the render target.</param>
        private void AttachStencilBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(stencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                gl.NamedFramebufferRenderbuffer(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_STENCIL_ATTACHMENT, gl.GL_RENDERBUFFER, buffer.OpenGLName);
                gl.ThrowIfError();
            }
            else
            {
                if (!glFramebufferTextureIsSupported)
                {
                    gl.FramebufferTexture2D(gl.GL_FRAMEBUFFER, gl.GL_STENCIL_ATTACHMENT, gl.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
                else
                {
                    gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
                    gl.ThrowIfError();
                }
            }

            stencilAttachments++;
        }

        /// <summary>
        /// Gets the render target's data.
        /// </summary>
        /// <param name="data">An array to populate with the render target's data.</param>
        /// <param name="region">The region of the render target from which to retrieve data.</param>
        private unsafe void GetDataInternal(Color[] data, Rectangle region)
        {
            using (OpenGLState.ScopedBindFramebuffer(framebuffer, true))
            {
                fixed (Color* pData = data)
                {
                    gl.ReadBuffer(gl.GL_COLOR_ATTACHMENT0);
                    gl.ThrowIfError();

                    gl.ReadPixels(region.X, region.Y, region.Width, region.Height, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, pData);
                    gl.ThrowIfError();
                }
            }

            // OpenGL texture data is stored upside down and in the wrong color format,
            // so we need to convert that to what our caller expects.
            var rowsToProcess = (Height % 2 == 0) ? Height / 2 : 1 + Height / 2;
            for (int y = 0; y < rowsToProcess; y++)
            {
                var ySrc = (y);
                var yDst = (Height - 1) - y;

                for (int x = 0; x < Width; x++)
                {
                    var ixSrc = (ySrc * Width) + x;
                    var ixDst = (yDst * Width) + x;

                    var colorSrc = new Color(data[ixSrc].PackedValue);
                    var colorDst = new Color(data[ixDst].PackedValue);

                    data[ixDst] = colorSrc;
                    data[ixSrc] = colorDst;
                }
            }
        }

        /// <summary>
        /// Updates the set of draw buffers which are enabled for this framebuffer.
        /// </summary>
        private unsafe void UpdateDrawBuffers()
        {
            if (colorAttachments == 0)
            {
                gl.NamedFramebufferDrawBuffer(framebuffer, gl.GL_NONE);
            }
            else
            {
                var bufs = stackalloc uint[colorAttachments];
                for (int i = 0; i < colorAttachments; i++)
                    bufs[i] = (uint)(gl.GL_COLOR_ATTACHMENT0 + i);

                gl.NamedFramebufferDrawBuffers(framebuffer, colorAttachments, bufs);
            }
        }
        
        // Property values.
        private readonly RenderTargetUsage renderTargetUsage;
        private Int32 width;
        private Int32 height;

        // State values.
        private readonly Boolean glFramebufferTextureIsSupported;
        private readonly UInt32 framebuffer;
        private Int32 colorAttachments;
        private Int32 depthAttachments;
        private Int32 stencilAttachments;
        private UInt32 framebufferStatus = gl.GL_FRAMEBUFFER_UNDEFINED;

        // The target's list of attached buffers.
        private readonly List<OpenGLRenderBuffer2D> buffers = new List<OpenGLRenderBuffer2D>();
    }
}
