using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Platform;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the Ultraviolet Graphics subsystem.
    /// </summary>
    public sealed unsafe class OpenGLUltravioletGraphics : UltravioletResource, IUltravioletGraphics
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletGraphics class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public OpenGLUltravioletGraphics(OpenGLUltravioletContext uv, UltravioletConfiguration configuration)
            : base(uv)
        {
            if (configuration.Debug)
            {
                SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_FLAGS, (int)SDL_GLcontextFlag.DEBUG);
            }

            var masterptr = ((OpenGLUltravioletWindowInfo)uv.GetPlatform().Windows).GetMasterPointer();            
            if ((this.context = SDL.GL_CreateContext(masterptr)) == IntPtr.Zero)
                throw new SDL2Exception();

            if (SDL.GL_SetSwapInterval(0) < 0)
                throw new SDL2Exception();

            if (gl.Initialized)
            {
                gl.Uninitialize();
            }
            gl.Initialize(new OpenGLInitializer());

            OpenGLState.ResetCache();

            if (!VerifyCapabilities())
                throw new NotSupportedException(OpenGLStrings.UnsupportedGraphicsDevice);

            if (configuration.Debug && configuration.DebugCallback != null)
            {
                InitializeDebugOutput(configuration);
            }

            this.maxTextureStages = Math.Min(16, gl.GetInteger(gl.GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS));
            this.textures = new Texture2D[maxTextureStages];
            this.samplerStates = new SamplerState[maxTextureStages];

            ResetDeviceStates();
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <summary>
        /// Clears the back buffer to the specified color.
        /// </summary>
        /// <param name="color">The color to which to clear the buffer.</param>
        public void Clear(Color color)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            gl.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            gl.Clear(gl.GL_COLOR_BUFFER_BIT);
        }

        /// <summary>
        /// Clears the back buffer to the specified color, depth, and stencil values.
        /// </summary>
        /// <param name="color">The color to which to clear the buffer.</param>
        /// <param name="depth">The depth value to which to clear the buffer.</param>
        /// <param name="stencil">The stencil value to which to clear the buffer.</param>
        public void Clear(Color color, Double depth, Int32 stencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            gl.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            gl.ClearDepth(depth);
            gl.ClearStencil(stencil);
            gl.Clear(gl.GL_COLOR_BUFFER_BIT | gl.GL_DEPTH_BUFFER_BIT | gl.GL_STENCIL_BUFFER_BIT);
        }

        /// <summary>
        /// Sets the render target.
        /// </summary>
        /// <param name="renderTarget">The render target to set, or null to revert to the default render target.</param>
        public void SetRenderTarget(RenderTarget2D renderTarget)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(renderTarget);

            var oglRenderTarget = (OpenGLRenderTarget2D)renderTarget;
            if (oglRenderTarget != this.renderTarget)
            {
                var targetName = 0u;
                var targetSize = Size2.Zero;

                var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
                if (oglRenderTarget != null || currentWindow == null)
                {
                    oglRenderTarget.ValidateStatus();

                    targetName = oglRenderTarget.OpenGLName;
                    targetSize = renderTarget.Size;
                }
                else
                {
                    targetSize = currentWindow.ClientSize;
                }

                OpenGLState.BindFramebuffer(targetName);

                if (this.renderTarget != null)
                    this.renderTarget.UnbindWrite();

                this.renderTarget = oglRenderTarget;

                if (this.renderTarget != null)
                    this.renderTarget.BindWrite();

                this.viewport = default(Viewport);
                SetViewport(new Viewport(0, 0, targetSize.Width, targetSize.Height));

                if (this.renderTarget != null)
                {
                    Clear(Color.FromArgb(0xFF442288));
                }
            }
        }    

        /// <summary>
        /// Gets the device's render target.
        /// </summary>
        /// <returns>The device's render target.</returns>
        public RenderTarget2D GetRenderTarget()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.renderTarget;
        }

        /// <summary>
        /// Sets the viewport.
        /// </summary>
        /// <param name="viewport">The viewport to set.</param>
        public void SetViewport(Viewport viewport)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (this.viewport != viewport)
            {
                var x = viewport.X;
                var y = viewport.Y;
                ConvertScreenRegionUvToGL(ref x, ref y, viewport.Width, viewport.Height);

                gl.Viewport(x, y, viewport.Width, viewport.Height);
                gl.ThrowIfError();

                this.viewport = viewport;

                SetScissorRectangle(null);
            }
        }

        /// <summary>
        /// Gets the device's viewport.
        /// </summary>
        /// <returns>The device's viewport.</returns>
        public Viewport GetViewport()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.viewport;
        }

        /// <summary>
        /// Binds a texture to the specified texture state.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <param name="texture">The texture to bind to the specified texture stage.</param>
        public void SetTexture(Int32 sampler, Texture2D texture)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, "sampler");
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(texture);

            if (texture != null && texture.BoundForWriting)
                throw new InvalidOperationException(OpenGLStrings.RenderTargetCannotBeUsedAsTexture);

            if (this.textures[sampler] != texture)
            {
                var textureName = (texture == null) ? 0 : ((IOpenGLResource)texture).OpenGLName;

                gl.ActiveTexture((uint)(gl.GL_TEXTURE0 + sampler));
                gl.ThrowIfError();

                OpenGLState.Texture2DImmediate(textureName);

                if (this.textures[sampler] != null)
                    ((IBindableResource)this.textures[sampler]).UnbindRead();

                this.textures[sampler] = texture;

                if (this.textures[sampler] != null)
                    ((IBindableResource)this.textures[sampler]).BindRead();

                var samplerState = (OpenGLSamplerState)(GetSamplerState(sampler) ?? SamplerState.LinearClamp);
                if (samplerState != null)
                {
                    samplerState.Apply(sampler);
                }
            }
        }

        /// <summary>
        /// Gets the texture that is bound to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <returns>The texture that is bound to the specified sampler.</returns>
        public Texture2D GetTexture(Int32 sampler)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, "sampler");
            Contract.EnsureNotDisposed(this, Disposed);

            return this.textures[sampler];
        }

        /// <summary>
        /// Binds a geometry stream to the graphics device.
        /// </summary>
        /// <param name="stream">The geometry stream to bind to the graphics device.</param>
        public void SetGeometryStream(GeometryStream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(stream);

            if (stream == null)
            {
                this.geometryStream = null;
                OpenGLState.BindVertexArrayObject(0, 0, 0);
            }
            else
            {
                if (this.geometryStream != stream)
                {
                    this.geometryStream = (OpenGLGeometryStream)stream;
                    this.geometryStream.Apply();
                }
            }
        }

        /// <summary>
        /// Gets the geometry stream that is bound to the graphics device.
        /// </summary>
        /// <returns>The geometry stream that is bound to the graphics device.</returns>
        public GeometryStream GetGeometryStream()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.geometryStream;
        }

        /// <summary>
        /// Binds a blend state to the graphics device.
        /// </summary>
        /// <param name="state">The blend state to bind to the graphics device.</param>
        public void SetBlendState(BlendState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.blendState != state)
            {
                this.blendState = (OpenGLBlendState)state;
                this.blendState.Apply();
            }
        }

        /// <summary>
        /// Gets the blend state that is bound to the device.
        /// </summary>
        /// <returns>The blend state that is bound to the device.</returns>
        public BlendState GetBlendState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.blendState;
        }

        /// <summary>
        /// Binds a depth/stencil state to the graphics device.
        /// </summary>
        /// <param name="state">The depth/stencil state to bind to the graphics device.</param>
        public void SetDepthStencilState(DepthStencilState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.depthStencilState != state)
            {
                this.depthStencilState = (OpenGLDepthStencilState)state;
                this.depthStencilState.Apply();
            }
        }

        /// <summary>
        /// Gets the depth/stencil state that is bound to the device.
        /// </summary>
        /// <returns>The depth/stencil state that is bound to the device.</returns>
        public DepthStencilState GetDepthStencilState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.depthStencilState;
        }

        /// <summary>
        /// Binds a rasterizer state to the graphics device.
        /// </summary>
        /// <param name="state">The rasterizer state to bind to the graphics device.</param>
        public void SetRasterizerState(RasterizerState state)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.rasterizerState != state)
            {
                this.rasterizerState = (OpenGLRasterizerState)state;
                this.rasterizerState.Apply();
            }
        }

        /// <summary>
        /// Gets the rasterizer state that is bound to the device.
        /// </summary>
        /// <returns>The rasterizer state that is bound to the device.</returns>
        public RasterizerState GetRasterizerState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.rasterizerState;
        }

        /// <summary>
        /// Binds a sampler state to the graphics device.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <param name="state">The sampler state to bind to the graphics device.</param>
        public void SetSamplerState(Int32 sampler, SamplerState state)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, "sampler");
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.samplerStates[sampler] != state)
            {
                ((OpenGLSamplerState)state).Apply(sampler);
                this.samplerStates[sampler] = state;
            }
        }

        /// <summary>
        /// Gets the sampler state that is bound to the specified sampler.
        /// </summary>
        /// <param name="sampler">The sampler index.</param>
        /// <returns></returns>
        public SamplerState GetSamplerState(Int32 sampler)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, "sampler");
            Contract.EnsureNotDisposed(this, Disposed);

            return this.samplerStates[sampler];
        }

        /// <inheritdoc/>
        public void SetScissorRectangle(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            var rect = new Rectangle(x, y, width, height);
            SetScissorRectangle(rect);
        }

        /// <inheritdoc/>
        public void SetScissorRectangle(Rectangle? rect)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (this.scissorRectangle != rect)
            {
                if (rect == null)
                {
                    gl.Disable(gl.GL_SCISSOR_TEST);
                    gl.ThrowIfError();
                }
                else
                {
                    var rectValue = rect.GetValueOrDefault();
                    if (rectValue.Width < 0 || rectValue.Height < 0)
                        throw new ArgumentOutOfRangeException("rect");

                    var x = rectValue.X;
                    var y = rectValue.Y;
                    ConvertScreenRegionUvToGL(ref x, ref y, rectValue.Width, rectValue.Height);

                    gl.Enable(gl.GL_SCISSOR_TEST);
                    gl.ThrowIfError();

                    gl.Scissor(x, y, rectValue.Width, rectValue.Height);
                    gl.ThrowIfError();
                }
                this.scissorRectangle = rect;
            }
        }

        /// <inheritdoc/>
        public Rectangle? GetScissorRectangle()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.scissorRectangle;
        }

        /// <summary>
        /// Draws a collection of non-indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">The type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        public void DrawPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count > 0, "count");
            Contract.EnsureNotDisposed(this, Disposed);

            Contract.Ensure(geometryStream != null, OpenGLStrings.NoGeometryStream);
            Contract.Ensure(geometryStream.IsValid, OpenGLStrings.InvalidGeometryStream);
            
            Contract.EnsureNot(OpenGLState.GL_CURRENT_PROGRAM == 0, OpenGLStrings.NoEffect);

            geometryStream.ApplyAttributes(OpenGLState.GL_CURRENT_PROGRAM);

            var glVerts = 0;
            var glPrimitiveType = GetPrimitiveTypeGL(type, count, out glVerts);
            gl.DrawArrays(glPrimitiveType, start, glVerts);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Draws a collection of indexed geometric primitives of the specified type from the currently bound buffers.
        /// </summary>
        /// <param name="type">The type of primitive to render.</param>
        /// <param name="start">The index of the first vertex to render.</param>
        /// <param name="count">The number of primitives to render.</param>
        public void DrawIndexedPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            Contract.EnsureRange(start >= 0, "start");
            Contract.EnsureRange(count > 0, "count");
            Contract.EnsureNotDisposed(this, Disposed);

            Contract.Ensure(geometryStream != null, OpenGLStrings.NoGeometryStream);
            Contract.Ensure(geometryStream.IsValid, OpenGLStrings.InvalidGeometryStream);
            Contract.Ensure(geometryStream.HasIndices, OpenGLStrings.InvalidGeometryStream);

            Contract.EnsureNot(OpenGLState.GL_CURRENT_PROGRAM == 0, OpenGLStrings.NoEffect);

            geometryStream.ApplyAttributes(OpenGLState.GL_CURRENT_PROGRAM);

            var glVerts = 0;
            var glPrimitiveType = GetPrimitiveTypeGL(type, count, out glVerts);
            var glIndexSize = sizeof(short);
            var glIndexType = GetIndexFormatGL(geometryStream.IndexBufferElementType, out glIndexSize);
            var glOffset = (void*)(start * glIndexSize);

            gl.DrawElements(glPrimitiveType, glVerts, glIndexType, glOffset);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Advances the device by one frame and updates the frame rate.
        /// </summary>
        public void UpdateFrameRate()
        {
            frameRateDelta = (frameRateDelta * frameRateSmoothing) + (frameRateTimer.Elapsed.TotalMilliseconds * (1.0 - frameRateSmoothing));
            frameRate = Math.Round(1000.0 / frameRateDelta, 0);
            frameRateTimer.Restart();
        }

        /// <summary>
        /// Gets the current frame rate.
        /// </summary>
        public Single FrameRate
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return (Single)frameRate; 
            }
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Releases any references to the specified texture.
        /// </summary>
        /// <param name="texture">The texture to which to release references.</param>
        internal void ReleaseReferences(Texture2D texture)
        {
            Contract.Require(texture, "texture");

            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] == texture)
                {
                    SetTexture(i, null);
                }
            }
        }

        /// <summary>
        /// Gets the OpenGL context.
        /// </summary>
        internal IntPtr OpenGLContext
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return context;
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

            /* FIX:
             * Without this line, Intel HD 4000 throws an AccessViolationException
             * when we call GL_DeleteContext(). Weird, huh? */
            gl.BindVertexArray(0);
            gl.Uninitialize();

            SDL.GL_DeleteContext(context);

            base.Dispose(disposing);
        }

        /// <summary>
        /// Verifies that the graphics device supports all of the functionality required by Ultraviolet.
        /// </summary>
        /// <returns>true if the graphics device is supported; otherwise, false.</returns>
        private static Boolean VerifyCapabilities()
        {
            if (gl.IsGLES)
            {
                return gl.IsVersionAtLeast(2, 0);
            }

            if (gl.IsVersionAtLeast(3, 0) || (
                gl.IsExtensionSupported("GL_ARB_vertex_array_object") &&
                gl.IsExtensionSupported("GL_ARB_framebuffer_object")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the OpenGL index element format that corresponds to the specified Ultraviolet index element type.
        /// </summary>
        /// <param name="type">The index element type to convert.</param>
        /// <param name="size">The index element size in bytes.</param>
        /// <returns>The converted index element format.</returns>
        private static UInt32 GetIndexFormatGL(IndexBufferElementType type, out Int32 size)
        {
            switch (type)
            {
                case IndexBufferElementType.Int16:
                    size = sizeof(short);
                    return gl.GL_UNSIGNED_SHORT;

                case IndexBufferElementType.Int32:
                    size = sizeof(int);
                    return gl.GL_UNSIGNED_INT;

                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedIndexFormat);
            }
        }

        /// <summary>
        /// Gets the OpenGL primitive type that corresponds to the specified Ultraviolet primitive type.
        /// </summary>
        /// <param name="type">The primitive type to convert.</param>
        /// <param name="count">The number of polygons to render.</param>
        /// <param name="vertices">The number of vertices to render.</param>
        /// <returns>The converted primitive type.</returns>
        private static UInt32 GetPrimitiveTypeGL(PrimitiveType type, Int32 count, out Int32 vertices)
        {
            switch (type)
            {
                case PrimitiveType.TriangleList:
                    vertices = count * 3;
                    return gl.GL_TRIANGLES;
                
                case PrimitiveType.TriangleStrip:
                    vertices = count + 2;
                    return gl.GL_TRIANGLE_STRIP;
                
                case PrimitiveType.LineList:
                    vertices = count * 2;
                    return gl.GL_LINES;
                
                case PrimitiveType.LineStrip:
                    vertices = count + 1;
                    return gl.GL_LINE_STRIP;

                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedPrimitiveType);
            }
        }

        /// <summary>
        /// Initializes debug output for this context.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        private void InitializeDebugOutput(UltravioletConfiguration configuration)
        {
            if (!gl.IsExtensionSupported("GL_ARB_debug_output"))
            {
                Debug.WriteLine(OpenGLStrings.DebugOutputNotSupported);
                return;
            }

            debugCallback = configuration.DebugCallback;
            debugCallbackOpenGL = (source, type, id, severity, length, message, userParam) =>
            {
                var messageString = Marshal.PtrToStringAnsi(message, length);
                var messageLevel = DebugLevels.Info;
                switch (severity)
                {
                    case gl.DEBUG_SEVERITY_MEDIUM:
                        messageLevel = DebugLevels.Warning;
                        break;

                    case gl.DEBUG_SEVERITY_HIGH:
                        messageLevel = DebugLevels.Error;
                        break;
                }
                debugCallback(Ultraviolet, messageLevel, messageString);
            };

            gl.DebugMessageControl(gl.GL_DONT_CARE, gl.GL_DONT_CARE, gl.GL_DONT_CARE, 0, IntPtr.Zero, false);

            if ((configuration.DebugLevels & DebugLevels.Info) == DebugLevels.Info)
            {
                gl.DebugMessageControl(gl.GL_DONT_CARE, gl.GL_DONT_CARE, gl.DEBUG_SEVERITY_LOW, 0, IntPtr.Zero, true);
                gl.DebugMessageControl(gl.GL_DONT_CARE, gl.GL_DONT_CARE, gl.DEBUG_SEVERITY_NOTIFICATION, 0, IntPtr.Zero, true);
            }
            if ((configuration.DebugLevels & DebugLevels.Warning) == DebugLevels.Warning)
                gl.DebugMessageControl(gl.GL_DONT_CARE, gl.GL_DONT_CARE, gl.DEBUG_SEVERITY_MEDIUM, 0, IntPtr.Zero, true);
            if ((configuration.DebugLevels & DebugLevels.Error) == DebugLevels.Error)
                gl.DebugMessageControl(gl.GL_DONT_CARE, gl.GL_DONT_CARE, gl.DEBUG_SEVERITY_HIGH, 0, IntPtr.Zero, true);

            gl.DebugMessageCallback(debugCallbackOpenGL, IntPtr.Zero);
        }

        /// <summary>
        /// Resets the device states to their initial values.
        /// </summary>
        private void ResetDeviceStates()
        {
            SetBlendState(BlendState.AlphaBlend);
            SetDepthStencilState(DepthStencilState.Default);
            SetRasterizerState(RasterizerState.CullCounterClockwise);
            for (int i = 0; i < maxTextureStages; i++)
            {
                SetSamplerState(i, SamplerState.LinearClamp);
            }
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Converts an Ultraviolet screen region to OpenGL coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the region to convert.</param>
        /// <param name="y">The y-coordinate of the region to convert.</param>
        /// <param name="width">The width of the region to convert.</param>
        /// <param name="height">The height of the region to convert.</param>
        private void ConvertScreenRegionUvToGL(ref Int32 x, ref Int32 y, Int32 width, Int32 height)
        {
            var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
            if (currentWindow == null)
                return;

            var targetHeight = (renderTarget == null) ? currentWindow.ClientSize.Height : renderTarget.Height;
            y = targetHeight - (height + y);
        }

        // Device state.
        private IntPtr context;
        private OpenGLRenderTarget2D renderTarget;
        private Viewport viewport;
        private OpenGLGeometryStream geometryStream;
        private OpenGLBlendState blendState;
        private OpenGLDepthStencilState depthStencilState;
        private OpenGLRasterizerState rasterizerState;
        private Rectangle? scissorRectangle;

        // Frame rate counter.
        private Stopwatch frameRateTimer = new Stopwatch();
        private Double frameRateSmoothing = 0.9;
        private Double frameRateDelta;
        private Double frameRate;

        // Current textures.
        private readonly Int32 maxTextureStages;
        private readonly Texture2D[] textures;
        private readonly SamplerState[] samplerStates;

        // Debug output callbacks.
        private DebugCallback debugCallback;
        private gl.DebugProc debugCallbackOpenGL;
    }
}
