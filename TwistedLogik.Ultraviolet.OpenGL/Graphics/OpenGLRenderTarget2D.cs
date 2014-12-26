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
        /// <param name="buffers">The collection of render buffers to attach to the target.</param>
        public OpenGLRenderTarget2D(UltravioletContext uv, Int32 width, Int32 height, IEnumerable<RenderBuffer2D> buffers = null)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

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
        }

        /// <summary>
        /// Attaches a render buffer to this render target.
        /// </summary>
        /// <param name="buffer">The render buffer to attach to this render target.</param>
        public override void Attach(RenderBuffer2D buffer)
        {
            Contract.Require(buffer, "buffer");
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

            buffers.Add(sdlBuffer);
        }

        /// <summary>
        /// Gets the render target's data.
        /// </summary>
        /// <param name="data">An array to populate with the render target's data.</param>
        public override unsafe void GetData(Color[] data)
        {
            Contract.Require(data, "data");

            if (data.Length != Width * Height)
                throw new ArgumentException(OpenGLStrings.BufferIsTooSmall.Format("data"));

            GetDataInternal(data, new Rectangle(0, 0, Width, Height));
        }

        /// <summary>
        /// Gets the render target's data.
        /// </summary>
        /// <param name="data">An array to populate with the render target's data.</param>
        /// <param name="region">The region of the render target from which to retrieve data.</param>
        public override unsafe void GetData(Color[] data, Rectangle region)
        {
            Contract.Require(data, "data");
            Contract.EnsureRange(region.X >= 0 && region.X < Width, "region");
            Contract.EnsureRange(region.Y >= 0 && region.Y < Height, "region");
            Contract.EnsureRange(region.Width > 0 && region.X + region.Width < Width, "region");
            Contract.EnsureRange(region.Height > 0 && region.Y + region.Height < Height, "region");

            if (data.Length != region.Width * region.Height)
                throw new ArgumentException(OpenGLStrings.BufferIsTooSmall.Format("data"));

            GetDataInternal(data, region);
        }

        /// <summary>
        /// Validates that the framebuffer is in a state which is ready for rendering.
        /// </summary>
        public void ValidateStatus()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (colorAttachments == 0 && depthStencilAttachments == 0)
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
            Contract.EnsureNot(BoundForReading, OpenGLStrings.ResourceAlreadyBound);

            foreach (var buffer in buffers)
                buffer.BindRead();
        }

        /// <summary>
        /// Binds the resource for writing.
        /// </summary>
        public void BindWrite()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(BoundForWriting, OpenGLStrings.ResourceAlreadyBound);

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

        /// <summary>
        /// Gets the render target's size in pixels.
        /// </summary>
        public override Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Size2(width, height);
            }
        }

        /// <summary>
        /// Gets the render target's width in pixels.
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
        /// Gets the render target's height in pixels.
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
        /// Gets a value indicating whether the render target is bound to the device for reading.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the render target is bound to the device for writing.
        /// </summary>
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

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Attaches a color buffer to the render target.
        /// </summary>
        /// <param name="buffer">The color buffer to attach to the render target.</param>
        private void AttachColorBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(colorAttachments < 16, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, 
                (uint)(gl.GL_COLOR_ATTACHMENT0 + colorAttachments), buffer.OpenGLName, 0);
            gl.ThrowIfError();

            colorAttachments++;
        }

        /// <summary>
        /// Attaches a depth buffer to the render target.
        /// </summary>
        /// <param name="buffer">The depth buffer to attach to the render target.</param>
        private void AttachDepthBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(depthStencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_ATTACHMENT, buffer.OpenGLName, 0);
            gl.ThrowIfError();

            depthStencilAttachments++;
        }

        /// <summary>
        /// Attaches a depth/stencil buffer to the render target.
        /// </summary>
        /// <param name="buffer">The depth/stencil buffer to attach to the render target.</param>
        private void AttachDepthStencilBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(depthStencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            gl.NamedFramebufferTexture(framebuffer, gl.GL_FRAMEBUFFER, gl.GL_DEPTH_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
            gl.ThrowIfError();

            depthStencilAttachments++;
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

                    var colorSrc = Color.FromRgba(data[ixSrc].PackedValue);
                    var colorDst = Color.FromRgba(data[ixDst].PackedValue);

                    data[ixDst] = colorSrc;
                    data[ixSrc] = colorDst;
                }
            }
        }

        // Property values.
        private readonly Int32 width;
        private readonly Int32 height;

        // State values.
        private readonly UInt32 framebuffer;
        private Int32 colorAttachments;
        private Int32 depthStencilAttachments;
        private UInt32 framebufferStatus = gl.GL_FRAMEBUFFER_UNDEFINED;

        // The target's list of attached buffers.
        private readonly List<OpenGLRenderBuffer2D> buffers = new List<OpenGLRenderBuffer2D>();
    }
}
