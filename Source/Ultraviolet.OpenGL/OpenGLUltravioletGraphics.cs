using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL implementation of the Ultraviolet Graphics subsystem.
    /// </summary>
    public sealed class OpenGLUltravioletGraphics : UltravioletResource, IUltravioletGraphics
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletGraphics class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public unsafe OpenGLUltravioletGraphics(UltravioletContext uv, UltravioletConfiguration configuration) : base(uv)
        {
            var glGraphicsConfiguration = configuration.GraphicsConfiguration as OpenGLGraphicsConfiguration;
            if (glGraphicsConfiguration == null)
                throw new InvalidOperationException(OpenGLStrings.InvalidGraphicsConfiguration);

            this.OpenGLEnvironment = uv.GetFactoryMethod<OpenGLEnvironmentFactory>()(uv);

            InitOpenGLVersion(glGraphicsConfiguration, out var versionRequested, out var versionRequired, out var isGLES);
            InitOpenGLEnvironment(glGraphicsConfiguration, isGLES);

            uv.GetPlatform().InitializePrimaryWindow(configuration);

            if (this.context == IntPtr.Zero && configuration.Debug)
                this.context = TryCreateOpenGLContext(uv, OpenGLEnvironment, versionRequested, versionRequired, true, false) ?? IntPtr.Zero;

            if (this.context == IntPtr.Zero)
                this.context = TryCreateOpenGLContext(uv, OpenGLEnvironment, versionRequested, versionRequired, false, true) ?? IntPtr.Zero;

            if (!OpenGLEnvironment.SetSwapInterval(1) && uv.Platform != UltravioletPlatform.iOS)
                OpenGLEnvironment.ThrowPlatformErrorException();

            if (GL.Initialized)
                GL.Uninitialize();

            GL.Initialize(new OpenGLInitializer(OpenGLEnvironment));
            
            if (!GL.IsVersionAtLeast(versionRequested ?? versionRequired))
                throw new InvalidOperationException(OpenGLStrings.DoesNotMeetMinimumVersionRequirement.Format(GL.MajorVersion, GL.MinorVersion, versionRequested.Major, versionRequested.Minor));
            
            OpenGLState.ResetCache();

            if (!VerifyCapabilities())
                throw new NotSupportedException(OpenGLStrings.UnsupportedGraphicsDevice);

            if (configuration.Debug && configuration.DebugCallback != null)
            {
                InitializeDebugOutput(configuration);
            }
            
            this.Capabilities = new OpenGLGraphicsCapabilities(configuration);

            if (Capabilities.SrgbEncodingEnabled && GL.IsFramebufferSrgbAvailable)
            {
                GL.Enable(GL.GL_FRAMEBUFFER_SRGB);
                GL.ThrowIfError();
            }

            this.maxTextureStages = GL.GetInteger(GL.GL_MAX_TEXTURE_IMAGE_UNITS);
            this.textures = new Texture[maxTextureStages];
            this.samplerStates = new SamplerState[maxTextureStages];
            this.samplerObjects = Capabilities.SupportsIndependentSamplerState ? new OpenGLSamplerObject[maxTextureStages] : null;
            this.backBufferRenderTargetUsage = glGraphicsConfiguration.BackBufferRenderTargetUsage;

            if (samplerObjects != null)
            {
                for (int i = 0; i < samplerObjects.Length; i++)
                {
                    samplerObjects[i] = new OpenGLSamplerObject(Ultraviolet);
                    samplerObjects[i].Bind((uint)i);
                }
            }            

            OpenGLState.VerifyCache();
            ResetDeviceStates();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            UpdateFrameRate(time);
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
                // From EXT_framebuffer_sRGB:
                // "The R, G, and B color components passed to glClearColor are assumed to be linear color components."
                if (CurrentRenderTargetIsSrgbEncoded)
                    color = Color.ConvertSrgbColorToLinear(color);

                if (blendState.ColorWriteChannels != ColorWriteChannels.All)
                {
                    resetColorWriteChannels = true;
                    OpenGLState.ColorMask = ColorWriteChannels.All;
                }

                OpenGLState.ClearColor = color;
                mask |= GL.GL_COLOR_BUFFER_BIT;
            }

            if ((options & ClearOptions.DepthBuffer) == ClearOptions.DepthBuffer && (renderTarget == null || renderTarget.HasDepthBuffer || renderTarget.HasDepthStencilBuffer))
            {
                if (!depthStencilState.DepthBufferEnable)
                {
                    resetDepthMask = true;
                    OpenGLState.DepthMask = true;
                }

                OpenGLState.ClearDepth = depth;
                mask |= GL.GL_DEPTH_BUFFER_BIT;
            }

            if ((options & ClearOptions.Stencil) == ClearOptions.Stencil && (renderTarget == null || renderTarget.HasStencilBuffer || renderTarget.HasDepthStencilBuffer))
            {
                OpenGLState.ClearStencil = stencil;
                mask |= GL.GL_STENCIL_BUFFER_BIT;
            }

            GL.Clear(mask);
            GL.ThrowIfError();

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

                GL.Viewport(x, y, viewport.Width, viewport.Height);
                GL.ThrowIfError();

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
        public void UnbindTexture(Texture texture)
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
        public void UnbindTextures(Object state, Func<Texture, Object, Boolean> predicate)
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
        public void UnbindTextures(Object state, Func<Texture2D, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(predicate, nameof(predicate));

            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] is Texture2D t2d)
                {
                    if (predicate(t2d, state))
                        SetTexture(i, null);
                }
            }
        }

        /// <inheritdoc/>
        public void UnbindTextures(Object state, Func<Texture3D, Object, Boolean> predicate)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(predicate, nameof(predicate));

            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] is Texture3D t3d)
                {
                    if (predicate(t3d, state))
                        SetTexture(i, null);
                }
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
        public void SetTexture(Int32 sampler, Texture texture)
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
                OpenGLState.ActiveTexture((uint)(GL.GL_TEXTURE0 + sampler));
                if (texture is Texture3D)
                {
                    OpenGLState.BindTexture3D(textureName);
                }
                else
                {
                    OpenGLState.BindTexture2D(textureName);
                }

                if (this.textures[sampler] != null)
                    ((IBindableResource)this.textures[sampler]).UnbindRead();

                this.textures[sampler] = texture;

                if (this.textures[sampler] != null)
                {
                    ((IBindableResource)this.textures[sampler]).BindRead();

                    if (texture is IOpenGLDynamicTexture textdyn)
                        textdyn.Flush();
                }

                if (!Capabilities.SupportsIndependentSamplerState)
                {
                    var samplerState = (OpenGLSamplerState)(GetSamplerState(sampler) ?? SamplerState.LinearClamp);
                    for (int i = 0; i < samplerStates.Length; i++)
                    {
                        if (this.textures[i] == texture)
                        {
                            var target = (texture is Texture3D) ? GL.GL_TEXTURE_3D : GL.GL_TEXTURE_2D;
                            samplerState.Apply(sampler, target);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public Texture GetTexture(Int32 sampler)
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
                if (Capabilities.SupportsIndependentSamplerState)
                {
                    var samplerObject = this.samplerObjects[sampler];
                    samplerObject.ApplySamplerState(state);
                    this.samplerStates[sampler] = state;
                }
                else
                {
                    this.samplerStates[sampler] = state;

                    var texture = this.textures[sampler];
                    if (texture != null)
                    {
                        var target = (texture is Texture3D) ? GL.GL_TEXTURE_3D : GL.GL_TEXTURE_2D;
                        oglstate.Apply(sampler, target);

                        for (int i = 0; i < samplerStates.Length; i++)
                        {
                            if (i == sampler)
                                continue;

                            if (this.textures[i] == texture && this.samplerStates[i] != oglstate)
                            {
                                oglstate.Apply(sampler, target);
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
                    GL.Disable(GL.GL_SCISSOR_TEST);
                    GL.ThrowIfError();
                }
                else
                {
                    var rectValue = rect.GetValueOrDefault();
                    if (rectValue.Width < 0 || rectValue.Height < 0)
                        throw new ArgumentOutOfRangeException("rect");

                    var x = rectValue.X;
                    var y = rectValue.Y;
                    ConvertScreenRegionUvToGL(ref x, ref y, rectValue.Width, rectValue.Height);

                    GL.Enable(GL.GL_SCISSOR_TEST, rasterizerState.ScissorTestEnable);
                    GL.ThrowIfError();

                    GL.Scissor(x, y, rectValue.Width, rectValue.Height);
                    GL.ThrowIfError();
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
            GL.DrawArrays(glPrimitiveType, start, glVerts);
            GL.ThrowIfError();
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

                GL.DrawElements(glPrimitiveType, glVerts, glIndexType, glOffset);
                GL.ThrowIfError();
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
                    GL.DrawElementsInstanced(glPrimitiveType, glVerts, glIndexType, glOffset, instances);
                    GL.ThrowIfError();
                }
                else
                {
                    if (!Capabilities.SupportsNonZeroBaseInstance)
                        throw new NotSupportedException(OpenGLStrings.NonZeroBaseInstanceNotSupported);

                    GL.DrawElementsInstancedBaseInstance(glPrimitiveType, glVerts, glIndexType, glOffset, instances, (uint)baseInstance);
                    GL.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Updates the graphics device's frame count.
        /// </summary>
        public void UpdateFrameCount()
        {
            frameCounter++;
            frameTimeInMilliseconds = (Single)frameTimer.Elapsed.TotalMilliseconds;
            frameTimer.Restart();
        }

        /// <summary>
        /// Updates the graphics device's calculated frame rate.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public void UpdateFrameRate(UltravioletTime time)
        {
            Contract.Require(time, nameof(time));

            frameCounterElapsed += time.ElapsedTime;
            if (frameCounterElapsed > TimeSpan.FromSeconds(1))
            {
                frameCounterElapsed -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        /// <inheritdoc/>
        public GraphicsCapabilities Capabilities { get; private set; }

        /// <inheritdoc/>
        public Single FrameRate => frameRate;

        /// <inheritdoc/>
        public Single FrameTimeInMilliseconds => frameTimeInMilliseconds;

        /// <inheritdoc/>
        public Boolean CurrentRenderTargetIsSrgbEncoded => 
            (renderTarget == null || renderTarget.HasSrgbEncodedColorBuffer) && Capabilities.SrgbEncodingEnabled;

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
        /// Gets the OpenGL platform environment.
        /// </summary>
        internal OpenGLEnvironment OpenGLEnvironment { get; }

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

            if (GL.Initialized)
                GL.Uninitialize();

            OpenGLEnvironment.DeleteOpenGLContext(context);
            context = IntPtr.Zero;

            SafeDispose.Dispose(OpenGLEnvironment);

            base.Dispose(disposing);
        }

        /// <summary>
        /// Attempts to create an OpenGL context.
        /// </summary>
        private static IntPtr? TryCreateOpenGLContext(UltravioletContext uv, OpenGLEnvironment environment,
            Version versionRequested, Version versionRequired, Boolean debug, Boolean throwOnFailure)
        {
            if (!environment.RequestDebugContext(debug))
                environment.ThrowPlatformErrorException();

            var gles = (uv.Platform == UltravioletPlatform.Android || uv.Platform == UltravioletPlatform.iOS);
            var versionArray = gles ? KnownOpenGLESVersions : KnownOpenGLVersions;
            var versionMin = versionRequested ?? versionRequired;
            var versionCurrent = versionRequested ?? versionArray[0];
            var versionCurrentIndex = Array.IndexOf(versionArray, versionCurrent);

            IntPtr context;
            do
            {
                if (versionCurrent < versionMin)
                {
                    if (throwOnFailure)
                        throw new InvalidOperationException(OpenGLStrings.DoesNotMeetMinimumVersionRequirement.Format(versionMin.Major, versionMin.Minor));

                    return null;
                }

                if (!environment.RequestOpenGLVersion(versionCurrent))
                    environment.ThrowPlatformErrorException();

                versionCurrent = versionArray[++versionCurrentIndex];
            }
            while ((context = environment.CreateOpenGLContext()) == IntPtr.Zero);

            return context;
        }

        /// <summary>
        /// Verifies that the graphics device supports all of the functionality required by Ultraviolet.
        /// </summary>
        /// <returns>true if the graphics device is supported; otherwise, false.</returns>
        private static Boolean VerifyCapabilities()
        {
            if (GL.IsGLES)
            {
                return GL.IsVersionAtLeast(2, 0);
            }

            if (GL.IsVersionAtLeast(3, 1) || (
                GL.IsExtensionSupported("GL_ARB_vertex_array_object") &&
                GL.IsExtensionSupported("GL_ARB_framebuffer_object")))
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
                    return GL.GL_UNSIGNED_SHORT;

                case IndexBufferElementType.Int32:
                    size = sizeof(int);
                    return GL.GL_UNSIGNED_INT;

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
                    return GL.GL_TRIANGLES;
                
                case PrimitiveType.TriangleStrip:
                    vertices = count + 2;
                    return GL.GL_TRIANGLE_STRIP;
                
                case PrimitiveType.LineList:
                    vertices = count * 2;
                    return GL.GL_LINES;
                
                case PrimitiveType.LineStrip:
                    vertices = count + 1;
                    return GL.GL_LINE_STRIP;

                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedPrimitiveType);
            }
        }

        /// <summary>
        /// Represents a thunk which allows the native OpenGL driver to call into the managed debug callback.
        /// </summary>
        [MonoPInvokeCallback(typeof(GL.DebugProc))]
        private static void DebugCallbackThunk(UInt32 source, UInt32 type, UInt32 id, UInt32 severity, Int32 length, IntPtr message, IntPtr userParam)
        {
            var messageString = Marshal.PtrToStringAnsi(message, length);
            var messageLevel = DebugLevels.Info;
            switch (severity)
            {
                case GL.DEBUG_SEVERITY_MEDIUM:
                    messageLevel = DebugLevels.Warning;
                    break;

                case GL.DEBUG_SEVERITY_HIGH:
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
        /// Determines which version of OpenGL will be used by the context.
        /// </summary>
        private void InitOpenGLVersion(OpenGLGraphicsConfiguration configuration, out Version versionRequested, out Version versionRequired, out Boolean isGLES)
        {
            isGLES = (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS);

            versionRequired = isGLES ? new Version(2, 0) : new Version(3, 1);
            versionRequested = isGLES ? configuration.MinimumOpenGLESVersion : configuration.MinimumOpenGLVersion;

            if (versionRequested != null && versionRequested < versionRequired)
                versionRequested = versionRequired;
        }

        /// <summary>
        /// Sets the OpenGL environment attributes which correspond to the application's OpenGL settings.
        /// </summary>
        private void InitOpenGLEnvironment(OpenGLGraphicsConfiguration configuration, Boolean isGLES)
        {
            if (!OpenGLEnvironment.RequestOpenGLProfile(isGLES))
                OpenGLEnvironment.ThrowPlatformErrorException();

            if (!OpenGLEnvironment.RequestDepthSize(configuration.BackBufferDepthSize))
                OpenGLEnvironment.ThrowPlatformErrorException();

            if (!OpenGLEnvironment.RequestStencilSize(configuration.BackBufferStencilSize))
                OpenGLEnvironment.ThrowPlatformErrorException();

            if (configuration.Use32BitFramebuffer)
            {
                if (!OpenGLEnvironment.Request32BitFramebuffer())
                    OpenGLEnvironment.ThrowPlatformErrorException();
            }
            else
            {
                if (!OpenGLEnvironment.Request24BitFramebuffer())
                    OpenGLEnvironment.ThrowPlatformErrorException();
            }

            if (configuration.SrgbBuffersEnabled)
            {
                if (!OpenGLEnvironment.RequestSrgbCapableFramebuffer())
                    OpenGLEnvironment.ThrowPlatformErrorException();

                if (!OpenGLEnvironment.IsFramebufferSrgbCapable)
                    configuration.SrgbBuffersEnabled = false;
            }
        }

        /// <summary>
        /// Initializes debug output for this context.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        private void InitializeDebugOutput(UltravioletConfiguration configuration)
        {
            if (!GL.IsExtensionSupported("GL_ARB_debug_output"))
            {
                Debug.WriteLine(OpenGLStrings.DebugOutputNotSupported);
                return;
            }

            debugCallback = configuration.DebugCallback;
            debugCallbackOpenGL = DebugCallbackThunk;

            GL.DebugMessageControl(GL.GL_DONT_CARE, GL.GL_DONT_CARE, GL.GL_DONT_CARE, 0, IntPtr.Zero, false);

            if ((configuration.DebugLevels & DebugLevels.Info) == DebugLevels.Info)
            {
                GL.DebugMessageControl(GL.GL_DONT_CARE, GL.GL_DONT_CARE, GL.DEBUG_SEVERITY_LOW, 0, IntPtr.Zero, true);
                GL.DebugMessageControl(GL.GL_DONT_CARE, GL.GL_DONT_CARE, GL.DEBUG_SEVERITY_NOTIFICATION, 0, IntPtr.Zero, true);
            }
            if ((configuration.DebugLevels & DebugLevels.Warning) == DebugLevels.Warning)
                GL.DebugMessageControl(GL.GL_DONT_CARE, GL.GL_DONT_CARE, GL.DEBUG_SEVERITY_MEDIUM, 0, IntPtr.Zero, true);
            if ((configuration.DebugLevels & DebugLevels.Error) == DebugLevels.Error)
                GL.DebugMessageControl(GL.GL_DONT_CARE, GL.GL_DONT_CARE, GL.DEBUG_SEVERITY_HIGH, 0, IntPtr.Zero, true);

            GL.DebugMessageCallback(debugCallbackOpenGL, IntPtr.Zero);
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
                var targetName = GL.DefaultFramebuffer;
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

        // Known versions of OpenGL in descending order.
        private static readonly Version[] KnownOpenGLVersions = new[]
        {
            new Version(4, 6),
            new Version(4, 5),
            new Version(4, 4),
            new Version(4, 3),
            new Version(4, 2),
            new Version(4, 1),
            new Version(4, 0),
            new Version(3, 3),
            new Version(3, 2),
            new Version(3, 1),
            new Version(1, 0),
        };

        // Known versions of OpenGL ES in descending order.
        private static readonly Version[] KnownOpenGLESVersions = new[]
        {
            new Version(3, 1),
            new Version(3, 0),
            new Version(2, 0),
            new Version(1, 0),
        };

        // Device state.
        private IntPtr context;
        private Viewport viewport;
        private OpenGLRenderTarget2D renderTarget;
        private OpenGLGeometryStream geometryStream;
        private OpenGLBlendState blendState;
        private OpenGLDepthStencilState depthStencilState;
        private OpenGLRasterizerState rasterizerState;
        private Rectangle? scissorRectangle;
        private RenderTargetUsage backBufferRenderTargetUsage;

        // Frame rate counter.
        private readonly Stopwatch frameTimer = new Stopwatch();
        private TimeSpan frameCounterElapsed;
        private Int32 frameCounter;
        private Int32 frameRate;
        private Single frameTimeInMilliseconds;

        // Current textures.
        private readonly Int32 maxTextureStages;
        private readonly Texture[] textures;
        private readonly SamplerState[] samplerStates;
        private readonly OpenGLSamplerObject[] samplerObjects;

        // Debug output callbacks.
        private DebugCallback debugCallback;
        private GL.DebugProc debugCallbackOpenGL;
    }
}
