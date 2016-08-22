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
    public sealed class OpenGLUltravioletGraphics : UltravioletResource, IUltravioletGraphics
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletGraphics class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        /// <param name="versionRequested">The OpenGL context version which is required by the application.</param>
        [Preserve]
        public unsafe OpenGLUltravioletGraphics(OpenGLUltravioletContext uv, OpenGLUltravioletConfiguration configuration, Version versionRequested)
            : base(uv)
        {
            var masterptr = ((OpenGLUltravioletWindowInfo)uv.GetPlatform().Windows).GetMasterPointer();
            if (!TryInitializeGLContext(masterptr, configuration))
            {
                var attemptedVersionMajor = 0;
                var attemptedVersionMinor = 0;

                if (SDL.GL_GetAttribute(SDL_GLattr.CONTEXT_MAJOR_VERSION, &attemptedVersionMajor) < 0)
                    throw new SDL2Exception();
                if (SDL.GL_GetAttribute(SDL_GLattr.CONTEXT_MINOR_VERSION, &attemptedVersionMinor) < 0)
                    throw new SDL2Exception();
                
                var attemptedVersion = new Version(attemptedVersionMajor, attemptedVersionMinor, 0, 0);

                var isGLES = (uv.Platform == UltravioletPlatform.Android || uv.Platform == UltravioletPlatform.iOS);
                if (isGLES && attemptedVersion >= new Version(3, 0) && (configuration.MinimumOpenGLESVersion ?? new Version(2, 0)) <= new Version(2, 0))
                {
                    if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MAJOR_VERSION, 2) < 0)
                        throw new SDL2Exception();

                    if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MINOR_VERSION, 0) < 0)
                        throw new SDL2Exception();

                    if (!TryInitializeGLContext(masterptr, configuration))
                        throw new SDL2Exception();
                }
                else throw new SDL2Exception();
            }

            if (SDL.GL_SetSwapInterval(1) < 0 && uv.Platform != UltravioletPlatform.iOS)
                throw new SDL2Exception();

            if (gl.Initialized)
            {
                gl.Uninitialize();
            }
            gl.Initialize(new OpenGLInitializer());
            
            if (!gl.IsVersionAtLeast(versionRequested))
                throw new InvalidOperationException(OpenGLStrings.DoesNotMeetMinimumVersionRequirement.Format(gl.MajorVersion, gl.MinorVersion, versionRequested.Major, versionRequested.Minor));
            
            OpenGLState.ResetCache();

            if (!VerifyCapabilities())
                throw new NotSupportedException(OpenGLStrings.UnsupportedGraphicsDevice);

            if (configuration.Debug && configuration.DebugCallback != null)
            {
                InitializeDebugOutput(configuration);
            }
            
            this.capabilities = new OpenGLGraphicsCapabilities(configuration);

            this.maxTextureStages = gl.GetInteger(gl.GL_MAX_TEXTURE_IMAGE_UNITS);
            this.textures = new Texture2D[maxTextureStages];
            this.samplerStates = new SamplerState[maxTextureStages];
            this.samplerObjects = capabilities.SupportsIndependentSamplerState ? new OpenGLSamplerObject[maxTextureStages] : null;
            this.backBufferRenderTargetUsage = configuration.BackBufferRenderTargetUsage;

            if (samplerObjects != null)
            {
                for (int i = 0; i < samplerObjects.Length; i++)
                {
                    samplerObjects[i] = new OpenGLSamplerObject(Ultraviolet);
                    samplerObjects[i].Bind((uint)i);
                }
            }            

            OpenGLState.VerifyCache();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public void Clear(Color color)
        {
            Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, color, 1.0, 0);
        }

        /// <inheritdoc/>
        public void Clear(Color color, Double depth, Int32 stencil)
        {
            Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, color, depth, stencil);
        }

        /// <inheritdoc/>
        public void Clear(ClearOptions options, Color color, Double depth, Int32 stencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var mask = 0u;
            var resetColorWriteChannels = false;
            var resetDepthMask = false;

            if ((options & ClearOptions.Target) == ClearOptions.Target && (renderTarget == null || renderTarget.HasColorBuffer))
            {
                if (blendState.ColorWriteChannels != ColorWriteChannels.All)
                {
                    resetColorWriteChannels = true;
                    OpenGLState.ColorMask = ColorWriteChannels.All;
                }

                OpenGLState.ClearColor = color;
                mask |= gl.GL_COLOR_BUFFER_BIT;
            }

            if ((options & ClearOptions.DepthBuffer) == ClearOptions.DepthBuffer && (renderTarget == null || renderTarget.HasDepthBuffer))
            {
                if (!depthStencilState.DepthBufferEnable)
                {
                    resetDepthMask = true;
                    OpenGLState.DepthMask = true;
                }

                OpenGLState.ClearDepth = depth;
                mask |= gl.GL_DEPTH_BUFFER_BIT;
            }

            if ((options & ClearOptions.Stencil) == ClearOptions.Stencil && (renderTarget == null || renderTarget.HasStencilBuffer))
            {
                OpenGLState.ClearStencil = stencil;
                mask |= gl.GL_STENCIL_BUFFER_BIT;
            }

            gl.Clear(mask);
            gl.ThrowIfError();

            if (resetColorWriteChannels)
                OpenGLState.ColorMask = blendState.ColorWriteChannels;

            if (resetDepthMask)
                OpenGLState.DepthMask = depthStencilState.DepthBufferWriteEnable;
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
            if (currentWindow != null && rt == null)
                rt = currentWindow.Compositor.GetRenderTarget();

            SetRenderTargetInternal(rt);
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt, Color clearColor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
            if (currentWindow != null && rt == null)
                rt = currentWindow.Compositor.GetRenderTarget();

            this.SetRenderTargetInternal(rt, clearColor);
        }

        /// <inheritdoc/>
        public void SetRenderTarget(RenderTarget2D rt, Color clearColor, Double clearDepth, Int32 clearStencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
            if (currentWindow != null && rt == null)
                rt = currentWindow.Compositor.GetRenderTarget();

            this.SetRenderTargetInternal(rt, clearColor, clearDepth, clearStencil);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetRenderTargetInternal(null);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer(Color clearColor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetRenderTargetInternal(null, clearColor);
        }

        /// <inheritdoc/>
        public void SetRenderTargetToBackBuffer(Color clearColor, Double clearDepth, Int32 clearStencil)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetRenderTargetInternal(null, clearColor, clearDepth, clearStencil);
        }

        /// <inheritdoc/>
        public RenderTarget2D GetRenderTarget()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.renderTarget;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Viewport GetViewport()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.viewport;
        }

        /// <inheritdoc/>
        public void UnbindTexture(Texture2D texture)
        {
            Contract.Require(texture, nameof(texture));
            Contract.EnsureNotDisposed(this, Disposed);

            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] == texture)
                    SetTexture(i, null);
            }
        }

        /// <inheritdoc/>
        public void UnbindTextures(Object state, Func<Texture2D, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(predicate, nameof(predicate));

            for (int i = 0; i < textures.Length; i++)
            {
                if (predicate(textures[i], state))
                    SetTexture(i, null);
            }
        }

        /// <inheritdoc/>
        public void UnbindAllTextures()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            for (int i = 0; i < textures.Length; i++)
                SetTexture(i, null);
        }

        /// <inheritdoc/>
        public void SetTexture(Int32 sampler, Texture2D texture)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, nameof(sampler));
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(texture);

            if (texture != null && texture.BoundForWriting)
                throw new InvalidOperationException(OpenGLStrings.RenderBufferCannotBeUsedAsTexture);

            if (texture != null && texture.WillNotBeSampled)
                throw new InvalidOperationException(OpenGLStrings.RenderBufferWillNotBeSampled);

            if (this.textures[sampler] != texture)
            {
                var textureName = (texture == null) ? 0 : ((IOpenGLResource)texture).OpenGLName;
                OpenGLState.ActiveTexture((uint)(gl.GL_TEXTURE0 + sampler));
                OpenGLState.BindTexture2D(textureName);

                if (this.textures[sampler] != null)
                    ((IBindableResource)this.textures[sampler]).UnbindRead();

                this.textures[sampler] = texture;

                if (this.textures[sampler] != null)
                    ((IBindableResource)this.textures[sampler]).BindRead();

                if (!capabilities.SupportsIndependentSamplerState)
                {
                    var samplerState = (OpenGLSamplerState)(GetSamplerState(sampler) ?? SamplerState.LinearClamp);
                    for (int i = 0; i < samplerStates.Length; i++)
                    {
                        if (this.textures[i] == texture)
                            samplerState.Apply(sampler);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public Texture2D GetTexture(Int32 sampler)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, nameof(sampler));
            Contract.EnsureNotDisposed(this, Disposed);

            return this.textures[sampler];
        }

        /// <inheritdoc/>
        public void SetGeometryStream(GeometryStream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(stream);

            if (stream == null)
            {
                this.geometryStream = null;
                OpenGLState.BindVertexArrayObject(0, 0);
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

        /// <inheritdoc/>
        public GeometryStream GetGeometryStream()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.geometryStream;
        }

        /// <inheritdoc/>
        public void SetBlendState(BlendState state)
        {
            Contract.Require(state, nameof(state));
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.blendState != state)
            {
                this.blendState = (OpenGLBlendState)state;
                this.blendState.Apply();
            }
        }

        /// <inheritdoc/>
        public BlendState GetBlendState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.blendState;
        }

        /// <inheritdoc/>
        public void SetDepthStencilState(DepthStencilState state)
        {
            Contract.Require(state, nameof(state));
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.depthStencilState != state)
            {
                this.depthStencilState = (OpenGLDepthStencilState)state;
                this.depthStencilState.Apply();
            }
        }

        /// <inheritdoc/>
        public DepthStencilState GetDepthStencilState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.depthStencilState;
        }

        /// <inheritdoc/>
        public void SetRasterizerState(RasterizerState state)
        {
            Contract.Require(state, nameof(state));
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            if (this.rasterizerState != state)
            {
                this.rasterizerState = (OpenGLRasterizerState)state;
                this.rasterizerState.Apply();
            }
        }

        /// <inheritdoc/>
        public RasterizerState GetRasterizerState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return this.rasterizerState;
        }

        /// <inheritdoc/>
        public void SetSamplerState(Int32 sampler, SamplerState state)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, nameof(sampler));
            Contract.Require(state, nameof(state));
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(state);

            var oglstate = (OpenGLSamplerState)state;

            if (this.samplerStates[sampler] != state)
            {
                if (capabilities.SupportsIndependentSamplerState)
                {
                    var samplerObject = this.samplerObjects[sampler];
                    samplerObject.ApplySamplerState(state);
                    this.samplerStates[sampler] = state;
                }
                else
                {
                    oglstate.Apply(sampler);
                    this.samplerStates[sampler] = state;

                    var texture = this.textures[sampler];
                    if (texture != null)
                    {
                        for (int i = 0; i < samplerStates.Length; i++)
                        {
                            if (i == sampler)
                                continue;

                            if (this.textures[i] == texture && this.samplerStates[i] != oglstate)
                            {
                                oglstate.Apply(sampler);
                                this.samplerStates[sampler] = state;
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public SamplerState GetSamplerState(Int32 sampler)
        {
            Contract.EnsureRange(sampler >= 0 && sampler < maxTextureStages, nameof(sampler));
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

                    gl.Enable(gl.GL_SCISSOR_TEST, rasterizerState.ScissorTestEnable);
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

        /// <inheritdoc/>
        public void DrawPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            DrawPrimitives(type, 0, start, count);
        }

        /// <inheritdoc/>
        public void DrawPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count)
        {
            Contract.EnsureRange(offset >= 0, nameof(offset));
            Contract.EnsureRange(start >= 0, nameof(start));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureNotDisposed(this, Disposed);

            Contract.Ensure(geometryStream != null, OpenGLStrings.NoGeometryStream);
            Contract.Ensure(geometryStream.IsValid, OpenGLStrings.InvalidGeometryStream);

            Contract.EnsureNot(OpenGLState.GL_CURRENT_PROGRAM == 0, OpenGLStrings.NoEffect);

            geometryStream.ApplyAttributes(OpenGLState.GL_CURRENT_PROGRAM, (UInt32)offset);

            var glVerts = 0;
            var glPrimitiveType = GetPrimitiveTypeGL(type, count, out glVerts);
            gl.DrawArrays(glPrimitiveType, start, glVerts);
            gl.ThrowIfError();
        }

        /// <inheritdoc/>
        public void DrawIndexedPrimitives(PrimitiveType type, Int32 start, Int32 count)
        {
            DrawIndexedPrimitives(type, 0, start, count);
        }

        /// <inheritdoc/>
        public void DrawIndexedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count)
        {
            Contract.EnsureRange(offset >= 0, nameof(offset));
            Contract.EnsureRange(start >= 0, nameof(start));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureNotDisposed(this, Disposed);

            Contract.Ensure(geometryStream != null, OpenGLStrings.NoGeometryStream);
            Contract.Ensure(geometryStream.IsValid, OpenGLStrings.InvalidGeometryStream);
            Contract.Ensure(geometryStream.HasIndices, OpenGLStrings.InvalidGeometryStream);

            Contract.EnsureNot(OpenGLState.GL_CURRENT_PROGRAM == 0, OpenGLStrings.NoEffect);

            geometryStream.ApplyAttributes(OpenGLState.GL_CURRENT_PROGRAM, (UInt32)offset);

            unsafe
            {
                var glVerts = 0;
                var glPrimitiveType = GetPrimitiveTypeGL(type, count, out glVerts);
                var glIndexSize = sizeof(short);
                var glIndexType = GetIndexFormatGL(geometryStream.IndexBufferElementType, out glIndexSize);
                var glOffset = (void*)(start * glIndexSize);

                gl.DrawElements(glPrimitiveType, glVerts, glIndexType, glOffset);
                gl.ThrowIfError();
            }
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances)
        {
            DrawInstancedPrimitives(type, 0, start, count, instances, 0);
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 start, Int32 count, Int32 instances, Int32 baseInstance)
        {
            DrawInstancedPrimitives(type, 0, start, count, instances, baseInstance);
        }

        /// <inheritdoc/>
        public void DrawInstancedPrimitives(PrimitiveType type, Int32 offset, Int32 start, Int32 count, Int32 instances, Int32 baseInstance)
        {
            Contract.EnsureRange(offset >= 0, nameof(offset));
            Contract.EnsureRange(start >= 0, nameof(start));
            Contract.EnsureRange(count > 0, nameof(count));
            Contract.EnsureNotDisposed(this, Disposed);

            Contract.Ensure<NotSupportedException>(Capabilities.SupportsInstancedRendering);
            Contract.Ensure(geometryStream != null, OpenGLStrings.NoGeometryStream);
            Contract.Ensure(geometryStream.IsValid, OpenGLStrings.InvalidGeometryStream);
            Contract.Ensure(geometryStream.HasIndices, OpenGLStrings.InvalidGeometryStream);

            Contract.EnsureNot(OpenGLState.GL_CURRENT_PROGRAM == 0, OpenGLStrings.NoEffect);

            geometryStream.ApplyAttributes(OpenGLState.GL_CURRENT_PROGRAM, (UInt32)offset);

            unsafe
            {
                var glVerts = 0;
                var glPrimitiveType = GetPrimitiveTypeGL(type, count, out glVerts);
                var glIndexSize = sizeof(short);
                var glIndexType = GetIndexFormatGL(geometryStream.IndexBufferElementType, out glIndexSize);
                var glOffset = (void*)(start * glIndexSize);

                if (baseInstance == 0)
                {
                    gl.DrawElementsInstanced(glPrimitiveType, glVerts, glIndexType, glOffset, instances);
                    gl.ThrowIfError();
                }
                else
                {
                    if (!Capabilities.SupportsNonZeroBaseInstance)
                        throw new NotSupportedException();

                    gl.DrawElementsInstancedBaseInstance(glPrimitiveType, glVerts, glIndexType, glOffset, instances, (uint)baseInstance);
                    gl.ThrowIfError();
                }
            }
        }

        /// <inheritdoc/>
        public void UpdateFrameRate()
        {
            frameRateDelta = (frameRateDelta * frameRateSmoothing) + (frameRateTimer.Elapsed.TotalMilliseconds * (1.0 - frameRateSmoothing));
            frameRate = Math.Round(1000.0 / frameRateDelta, 0);
            frameRateTimer.Restart();
        }

        /// <inheritdoc/>
        public Single FrameRate
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return (Single)frameRate; 
            }
        }

        /// <inheritdoc/>
        public GraphicsCapabilities Capabilities
        {
            get { return capabilities; }
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;
        
        /// <summary>
        /// Resets the device states to their initial values.
        /// </summary>
        internal void ResetDeviceStates()
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

            if (samplerObjects != null)
            {
                for (int i = 0; i < samplerObjects.Length; i++)
                    SafeDispose.Dispose(samplerObjects[i]);
            }

            if (gl.Initialized)
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

            if (gl.IsVersionAtLeast(3, 1) || (
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
        /// Represents a thunk which allows the native OpenGL driver to call into the managed debug callback.
        /// </summary>
        [MonoPInvokeCallback(typeof(gl.DebugProc))]
        private static void DebugCallbackThunk(UInt32 source, UInt32 type, UInt32 id, UInt32 severity, Int32 length, IntPtr message, IntPtr userParam)
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

            var uv = UltravioletContext.RequestCurrent();
            if (uv == null)
                return;

            var gfx = uv.GetGraphics() as OpenGLUltravioletGraphics;
            if (gfx == null)
                return;

            gfx.debugCallback?.Invoke(uv, messageLevel, messageString);
        }

        /// <summary>
        /// Attempts to initialize the OpenGL context with the specified configuration.
        /// </summary>
        private Boolean TryInitializeGLContext(IntPtr masterptr, OpenGLUltravioletConfiguration configuration)
        {
            if (configuration.Debug)
                SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_FLAGS, (int)SDL_GLcontextFlag.DEBUG);

            if ((this.context = SDL.GL_CreateContext(masterptr)) == IntPtr.Zero)
            {
                if (configuration.Debug)
                {
                    if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_FLAGS, 0) < 0)
                        throw new SDL2Exception();

                    if ((this.context = SDL.GL_CreateContext(masterptr)) != IntPtr.Zero)
                        return true;
                }
                return false;
            }
            return true;
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
            debugCallbackOpenGL = DebugCallbackThunk;

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
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

        /// <summary>
        /// Converts an Ultraviolet screen region to OpenGL coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the region to convert.</param>
        /// <param name="y">The y-coordinate of the region to convert.</param>
        /// <param name="width">The width of the region to convert.</param>
        /// <param name="height">The height of the region to convert.</param>
        private void ConvertScreenRegionUvToGL(ref Int32 x, ref Int32 y, Int32 width, Int32 height)
        {
            var renderTargetHeight = 0;
            if (renderTarget != null)
            {
                renderTargetHeight = renderTarget.Height;
            }
            else
            {
                var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
                if (currentWindow == null)
                    return;

                renderTargetHeight = currentWindow.DrawableSize.Height;
            }

            y = renderTargetHeight - (height + y);
        }

        /// <summary>
        /// Sets the current render target.
        /// </summary>
        private void SetRenderTargetInternal(RenderTarget2D renderTarget,
            Color? clearColor = null, Double? clearDepth = null, Int32? clearStencil = null)
        {
            Ultraviolet.ValidateResource(renderTarget);

            var usage = renderTarget?.RenderTargetUsage ?? backBufferRenderTargetUsage;
            if (usage == RenderTargetUsage.PlatformContents)
            {
                usage = Capabilities.SupportsPreservingRenderTargetContentInHardware ?
                    RenderTargetUsage.PreserveContents :
                    RenderTargetUsage.DiscardContents;
            }

            var oglRenderTarget = (OpenGLRenderTarget2D)renderTarget;
            if (oglRenderTarget != this.renderTarget)
            {
                var targetName = gl.DefaultFramebuffer;
                var targetSize = Size2.Zero;

                if (oglRenderTarget != null)
                {
                    oglRenderTarget.ValidateStatus();

                    targetName = oglRenderTarget.OpenGLName;
                    targetSize = renderTarget.Size;
                }
                else
                {
                    var currentWindow = Ultraviolet.GetPlatform().Windows.GetCurrent();
                    if (currentWindow != null)
                        targetSize = currentWindow.DrawableSize;
                }

                OpenGLState.BindFramebuffer(targetName);

                if (this.renderTarget != null)
                    this.renderTarget.UnbindWrite();

                this.renderTarget = oglRenderTarget;

                if (this.renderTarget != null)
                    this.renderTarget.BindWrite();

                this.viewport = default(Viewport);
                SetViewport(new Viewport(0, 0, targetSize.Width, targetSize.Height));

                if (usage == RenderTargetUsage.DiscardContents)
                {
                    Clear(clearColor ?? Color.FromArgb(0xFF442288), clearDepth ?? 1.0, clearStencil ?? 0);
                }
            }
        }

        // Property values.
        private readonly GraphicsCapabilities capabilities;

        // Device state.
        private IntPtr context;
        private OpenGLRenderTarget2D renderTarget;
        private Viewport viewport;
        private OpenGLGeometryStream geometryStream;
        private OpenGLBlendState blendState;
        private OpenGLDepthStencilState depthStencilState;
        private OpenGLRasterizerState rasterizerState;
        private Rectangle? scissorRectangle;
        private RenderTargetUsage backBufferRenderTargetUsage;

        // Frame rate counter.
        private Stopwatch frameRateTimer = new Stopwatch();
        private Double frameRateSmoothing = 0.9;
        private Double frameRateDelta;
        private Double frameRate;

        // Current textures.
        private readonly Int32 maxTextureStages;
        private readonly Texture2D[] textures;
        private readonly SamplerState[] samplerStates;
        private readonly OpenGLSamplerObject[] samplerObjects;

        // Debug output callbacks.
        private DebugCallback debugCallback;
        private gl.DebugProc debugCallbackOpenGL;
    }
}
