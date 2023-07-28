using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the RenderTarget2D class.
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

            var framebuffer = 0u;

            uv.QueueWorkItem(state =>
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
            }).Wait();

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

            var oglBuffer = (OpenGLRenderBuffer2D)buffer;

            Ultraviolet.QueueWorkItem(state =>
            {
                using (OpenGLState.ScopedBindFramebuffer(framebuffer))
                {
                    AttachRenderBuffer(oglBuffer);

                    framebufferStatus = GL.CheckNamedFramebufferStatus(framebuffer, GL.GL_FRAMEBUFFER);
                    GL.ThrowIfError();
                }
            }).Wait();

            oglBuffer.MarkAttached();

            buffers.Add(oglBuffer);
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

        /// <inheritdoc/>
        public override void Invalidate(Boolean color, Boolean depth, Boolean stencil, Boolean depthStencil, Int32 colorOffset, Int32 colorCount)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            // If we don't have any support for framebuffer invalidation, simply do nothing.
            if (!GL.IsFramebufferInvalidationAvailable)
                return;

            // If we're relying on EXT_discard_framebuffer, then we can only discard the first color
            // attachment. If that leaves us with nothing to do, bail out.
            if (!GL.IsInvalidateSubdataAvailable)
            {
                if (colorOffset != 0)
                {
                    color = false;
                    if (!depth && !stencil && !depthStencil)
                        return;
                }

                if (colorCount > 1)
                    colorCount = 1;
            }

            // Bind the framebuffer (if we're not using DSA) and perform the invalidation.
            unsafe
            {
                var numAttachments =
                    (color ? colorCount : 0) +
                    (depth ? 1 : 0) +
                    (stencil ? 1 : 0) +
                    (depthStencil ? 1 : 0);

                var attachments = stackalloc UInt32[numAttachments];
                var attachmentsIx = 0;

                if (color)
                {
                    for (int i = 0; i < colorCount; i++)
                        attachments[attachmentsIx++] = (UInt32)(GL.GL_COLOR_ATTACHMENT0 + colorOffset + i);
                }

                if (depth)
                    attachments[attachmentsIx++] = GL.GL_DEPTH_ATTACHMENT;

                if (stencil)
                    attachments[attachmentsIx++] = GL.GL_STENCIL_ATTACHMENT;

                if (depthStencil)
                    attachments[attachmentsIx++] = GL.GL_DEPTH_STENCIL_ATTACHMENT;

                if (GL.IsInvalidateSubdataAvailable)
                {
                    // The EXT DSA extension doesn't seem to provide glInvalidateNamedFramebuffer, so...
                    using (OpenGLState.ScopedBindFramebuffer(framebuffer, GL.IsEXTDirectStateAccessAvailable))
                    {
                        if (GL.IsEXTDirectStateAccessAvailable)
                        {
                            GL.InvalidateFramebuffer(GL.GL_FRAMEBUFFER, numAttachments, (IntPtr)attachments);
                            GL.ThrowIfError();
                        }
                        else
                        {
                            GL.InvalidateNamedFramebufferData(GL.GL_FRAMEBUFFER, framebuffer, numAttachments, (IntPtr)attachments);
                            GL.ThrowIfError();
                        }
                    }
                }
                else
                {
                    using (OpenGLState.ScopedBindFramebuffer(framebuffer, true))
                    {
                        GL.DiscardFramebufferEXT(GL.GL_FRAMEBUFFER, numAttachments, (IntPtr)attachments);
                        GL.ThrowIfError();
                    }
                }
            }
        }

        /// <summary>
        /// Validates that the framebuffer is in a state which is ready for rendering.
        /// </summary>
        public void ValidateStatus()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (colorAttachments == 0 && depthAttachments == 0 && stencilAttachments == 0 && depthStencilAttachments == 0)
                throw new InvalidOperationException(OpenGLStrings.RenderTargetNeedsBuffers);

            if (framebufferStatus != GL.GL_FRAMEBUFFER_COMPLETE)
            {
                switch (framebufferStatus)
                {
                    case GL.GL_FRAMEBUFFER_UNDEFINED:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_UNDEFINED"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER"));

                    case GL.GL_FRAMEBUFFER_UNSUPPORTED:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_UNSUPPORTED"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE:
                        throw new InvalidOperationException(OpenGLStrings.RenderTargetFramebufferIsNotComplete.Format("GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE"));

                    case GL.GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS:
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

        /// <inheritdoc/>
        public UInt32 OpenGLName => framebuffer;

        /// <inheritdoc/>
        public override Size2 Size => new Size2(width, height);

        /// <inheritdoc/>
        public override Int32 Width => width;

        /// <inheritdoc/>
        public override Int32 Height => height;

        /// <inheritdoc/>
        public override Boolean BoundForReading
        {
            get
            {
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
                foreach (var buffer in buffers)
                {
                    if (buffer.BoundForWriting)
                        return true;
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public override Boolean HasColorBuffer => colorAttachments > 0;

        /// <inheritdoc/>
        public override Boolean HasSrgbEncodedColorBuffer => hasSrgbEncodedColorBuffer;

        /// <inheritdoc/>
        public override Boolean HasDepthBuffer => depthAttachments > 0;

        /// <inheritdoc/>
        public override Boolean HasStencilBuffer => stencilAttachments > 0;

        /// <inheritdoc/>
        public override Boolean HasDepthStencilBuffer => depthStencilAttachments > 0;

        /// <inheritdoc/>
        public override RenderTargetUsage RenderTargetUsage => renderTargetUsage;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                var glname = framebuffer;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        GL.DeleteFramebuffer(glname);
                        GL.ThrowIfError();
                    }, null, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }
                buffers.Clear();

                framebuffer = 0;
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

                case RenderBufferFormat.Stencil8:
                    AttachStencilBuffer(buffer);
                    break;

                case RenderBufferFormat.Depth24Stencil8:
                    AttachDepthStencilBuffer(buffer);
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

            if (colorAttachments > 0 && buffer.SrgbEncoded != hasSrgbEncodedColorBuffer)
                throw new InvalidOperationException(UltravioletStrings.TargetsCannotHaveMultipleEncodings);

            hasSrgbEncodedColorBuffer = buffer.SrgbEncoded;

            if (buffer.WillNotBeSampled)
            {
                GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER,
                    (uint)(GL.GL_COLOR_ATTACHMENT0 + colorAttachments), GL.GL_RENDERBUFFER, buffer.OpenGLName);
                GL.ThrowIfError();
            }
            else
            {
                if (!GL.IsFramebufferTextureAvailable)
                {
                    GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, 
                        (uint)(GL.GL_COLOR_ATTACHMENT0 + colorAttachments), GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    GL.ThrowIfError();
                }
                else
                {
                    GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER,
                        (uint)(GL.GL_COLOR_ATTACHMENT0 + colorAttachments), buffer.OpenGLName, 0);
                    GL.ThrowIfError();
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
            Contract.Ensure(depthAttachments == 0 && depthStencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_RENDERBUFFER, buffer.OpenGLName);
                GL.ThrowIfError();
            }
            else
            {
                if (!GL.IsFramebufferTextureAvailable)
                {
                    GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    GL.ThrowIfError();
                }
                else
                {
                    GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, buffer.OpenGLName, 0);
                    GL.ThrowIfError();
                }
            }

            depthAttachments++;
        }

        /// <summary>
        /// Attaches a stencil buffer to the render target.
        /// </summary>
        /// <param name="buffer">The stencil buffer to attach to the render target.</param>
        private void AttachStencilBuffer(OpenGLRenderBuffer2D buffer)
        {
            Contract.Ensure(stencilAttachments == 0 && depthStencilAttachments == 0, OpenGLStrings.RenderBufferExceedsTargetCapacity);

            if (buffer.WillNotBeSampled)
            {
                GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, buffer.OpenGLName);
                GL.ThrowIfError();
            }
            else
            {
                if (!GL.IsFramebufferTextureAvailable)
                {
                    GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                    GL.ThrowIfError();
                }
                else
                {
                    GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
                    GL.ThrowIfError();
                }
            }

            stencilAttachments++;
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
                if (GL.IsCombinedDepthStencilAvailable)
                {
                    GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, buffer.OpenGLName);
                    GL.ThrowIfError();
                }
                else
                {
                    GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_RENDERBUFFER, buffer.OpenGLName);
                    GL.ThrowIfError();

                    GL.NamedFramebufferRenderbuffer(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, buffer.OpenGLName);
                    GL.ThrowIfError();
                }
            }
            else
            {
                if (!GL.IsFramebufferTextureAvailable)
                {
                    if (GL.IsCombinedDepthStencilAvailable)
                    {
                        GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                        GL.ThrowIfError();
                    }
                    else
                    {
                        GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                        GL.ThrowIfError();

                        GL.FramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, buffer.OpenGLName, 0);
                        GL.ThrowIfError();
                    }
                }
                else
                {
                    if (GL.IsCombinedDepthStencilAvailable)
                    {
                        GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
                        GL.ThrowIfError();
                    }
                    else
                    {
                        GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_DEPTH_ATTACHMENT, buffer.OpenGLName, 0);
                        GL.ThrowIfError();

                        GL.NamedFramebufferTexture(framebuffer, GL.GL_FRAMEBUFFER, GL.GL_STENCIL_ATTACHMENT, buffer.OpenGLName, 0);
                        GL.ThrowIfError();
                    }
                }
            }

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
                    if (GL.IsReadBufferAvailable)
                    {
                        GL.ReadBuffer(GL.GL_COLOR_ATTACHMENT0);
                        GL.ThrowIfError();
                    }

                    GL.ReadPixels(region.X, region.Y, region.Width, region.Height, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, pData);
                    GL.ThrowIfError();
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
            if (!GL.IsDrawBuffersAvailable)
                return;

            if (colorAttachments == 0 && GL.IsDrawBufferAvailable)
            {
                GL.NamedFramebufferDrawBuffer(framebuffer, GL.GL_NONE);
            }
            else
            {
                var bufs = stackalloc uint[colorAttachments];
                for (int i = 0; i < colorAttachments; i++)
                    bufs[i] = (uint)(GL.GL_COLOR_ATTACHMENT0 + i);

                GL.NamedFramebufferDrawBuffers(framebuffer, colorAttachments, bufs);
            }
        }
        
        // Property values.
        private readonly RenderTargetUsage renderTargetUsage;
        private Int32 width;
        private Int32 height;

        // State values.
        private Boolean hasSrgbEncodedColorBuffer;
        private UInt32 framebuffer;
        private Int32 colorAttachments;
        private Int32 depthAttachments;
        private Int32 stencilAttachments;
        private Int32 depthStencilAttachments;
        private UInt32 framebufferStatus = GL.GL_FRAMEBUFFER_UNDEFINED;

        // The target's list of attached buffers.
        private readonly List<OpenGLRenderBuffer2D> buffers = new List<OpenGLRenderBuffer2D>();
    }
}
