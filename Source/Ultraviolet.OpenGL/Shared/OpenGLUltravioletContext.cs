using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2.Platform;
using Ultraviolet.UI;
using static Ultraviolet.SDL2.Native.SDL_GLattr;
using static Ultraviolet.SDL2.Native.SDL_GLprofile;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL implementation of the Ultraviolet context.
    /// </summary>
    [CLSCompliant(true)]
    public sealed class OpenGLUltravioletContext : SDL2UltravioletContext
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletContext class.
        /// </summary>
        /// <param name="host">The object that is hosting the Ultraviolet context.</param>
        public OpenGLUltravioletContext(IUltravioletHost host)
            : this(host, OpenGLUltravioletConfiguration.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletContext class.
        /// </summary>
        /// <param name="host">The object that is hosting the Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        public OpenGLUltravioletContext(IUltravioletHost host, OpenGLUltravioletConfiguration configuration)
            : base(host, configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            IsHardwareInputDisabled = configuration.IsHardwareInputDisabled;

            if (!InitSDL(configuration))
                throw new SDL2Exception();

            InitOpenGLVersion(configuration, out var versionRequested, out var versionRequired, out var isGLEs);

            if (!configuration.EnableServiceMode)
                InitOpenGLAttributes(configuration, isGLEs);

            var sdlAssembly = typeof(SDLNative).Assembly;
            InitializeFactoryMethodsInAssembly(sdlAssembly);

            var sdlconfig = new SDL2PlatformConfiguration();
            sdlconfig.RenderingAPI = SDL2PlatformRenderingAPI.OpenGL;
            sdlconfig.MultiSampleBuffers = configuration.MultiSampleBuffers;
            sdlconfig.MultiSampleSamples = configuration.MultiSampleSamples;
            sdlconfig.SrgbBuffersEnabled = configuration.SrgbBuffersEnabled;
            this.platform = IsRunningInServiceMode ? (IUltravioletPlatform)new DummyUltravioletPlatform(this) : new SDL2UltravioletPlatform(this, configuration, sdlconfig);

            PumpEvents();

            if (IsRunningInServiceMode)
            {
                this.graphics = new DummyUltravioletGraphics(this);
                this.audio = new DummyUltravioletAudio(this);
                this.input = new DummyUltravioletInput(this);
            }
            else
            {
                this.graphics = new OpenGLUltravioletGraphics(this, configuration, versionRequested, versionRequired);
                ((OpenGLUltravioletGraphics)this.graphics).ResetDeviceStates();
                this.audio = InitializeAudioSubsystem(configuration);
                this.input = new SDL2UltravioletInput(this);
            }

            this.content = new UltravioletContent(this);
            this.content.RegisterImportersAndProcessors(new[] { sdlAssembly, AudioSubsystemAssembly });
            this.content.Importers.RegisterImporter<XmlContentImporter>("prog");

            this.ui = new UltravioletUI(this, configuration);

            PumpEvents();
            
            InitializeContext();
            InitializeViewProvider(configuration);
            InitializePlugins(configuration);
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            if (graphics is OpenGLUltravioletGraphics oglgfx)
                oglgfx.UpdateFrameRate(time);

            base.Update(time);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnDrawing(time);

            if (graphics is OpenGLUltravioletGraphics oglgfx)
            {
                var glcontext = oglgfx.OpenGLContext;
                var windowInfo = (SDL2UltravioletWindowInfoOpenGL)platform.Windows;
                foreach (var window in platform.Windows)
                {
                    windowInfo.DesignateCurrent(window, glcontext);

                    window.Compositor.BeginFrame();
                    window.Compositor.BeginContext(CompositionContext.Scene);

                    OnWindowDrawing(time, window);

                    windowInfo.Draw(time);

                    OnWindowDrawn(time, window);

                    window.Compositor.Compose();
                    window.Compositor.Present();

                    windowInfo.Swap();
                }

                windowInfo.DesignateCurrent(null, glcontext);

                oglgfx.SetRenderTargetToBackBuffer();
                oglgfx.UpdateFrameCount();
            }

            base.Draw(time);
        }

        /// <inheritdoc/>
        public override IUltravioletPlatform GetPlatform() => platform;

        /// <inheritdoc/>
        public override IUltravioletContent GetContent() => content;

        /// <inheritdoc/>
        public override IUltravioletGraphics GetGraphics() => graphics;

        /// <inheritdoc/>
        public override IUltravioletAudio GetAudio() => audio;

        /// <inheritdoc/>
        public override IUltravioletInput GetInput() => input;

        /// <inheritdoc/>
        public override IUltravioletUI GetUI() => ui;

        /// <summary>
        /// Gets the assembly that implements the audio subsystem.
        /// </summary>
        public Assembly AudioSubsystemAssembly { get; private set; }

        /// <summary>
        /// Initializes the context's audio subsystem.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns>The audio subsystem.</returns>
        private IUltravioletAudio InitializeAudioSubsystem(OpenGLUltravioletConfiguration configuration)
        {
            if (String.IsNullOrEmpty(configuration.AudioSubsystemAssembly))
                throw new InvalidOperationException(OpenGLStrings.InvalidAudioAssembly);

            Assembly asm;
            try
            {
                asm = Assembly.Load(configuration.AudioSubsystemAssembly);
                InitializeFactoryMethodsInAssembly(asm);
                AudioSubsystemAssembly = asm;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException ||
                    e is FileLoadException ||
                    e is BadImageFormatException)
                {
                    throw new InvalidOperationException(OpenGLStrings.InvalidAudioAssembly, e);
                }
                throw;
            }

            var types = (from t in asm.GetTypes()
                         where
                          t.IsClass && !t.IsAbstract &&
                          t.GetInterfaces().Contains(typeof(IUltravioletAudio))
                         select t).ToList();

            if (!types.Any() || types.Count > 1)
                throw new InvalidOperationException(OpenGLStrings.InvalidAudioAssembly);

            var type = types.Single();

            var ctorWithConfig = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(UltravioletConfiguration) });
            if (ctorWithConfig != null)
            {
                return (IUltravioletAudio)ctorWithConfig.Invoke(new object[] { this, configuration });
            }

            var ctorWithoutConfig = type.GetConstructor(new[] { typeof(UltravioletContext) });
            if (ctorWithoutConfig != null)
            {
                return (IUltravioletAudio)ctorWithoutConfig.Invoke(new object[] { this });
            }

            throw new InvalidOperationException(OpenGLStrings.InvalidAudioAssembly);
        }

        /// <summary>
        /// Determines which version of OpenGL will be used by the context.
        /// </summary>
        private void InitOpenGLVersion(OpenGLUltravioletConfiguration configuration,
            out Version versionRequested, out Version versionRequired, out Boolean isGLES)
        {
            isGLES = (Platform == UltravioletPlatform.Android || Platform == UltravioletPlatform.iOS);

            versionRequired = isGLES ? new Version(2, 0) : new Version(3, 1);
            versionRequested = isGLES ? configuration.MinimumOpenGLESVersion : configuration.MinimumOpenGLVersion;

            if (versionRequested != null && versionRequested < versionRequired)
                versionRequested = versionRequired;
        }

        /// <summary>
        /// Sets the SDL2 attributes which correspond to the application's OpenGL settings.
        /// </summary>
        private void InitOpenGLAttributes(OpenGLUltravioletConfiguration configuration, Boolean isGLES)
        {
            var profile = isGLES ? SDL_GL_CONTEXT_PROFILE_ES : SDL_GL_CONTEXT_PROFILE_CORE;

            if (SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, (Int32)profile) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetAttribute(SDL_GL_DEPTH_SIZE, configuration.BackBufferDepthSize) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetAttribute(SDL_GL_STENCIL_SIZE, configuration.BackBufferStencilSize) < 0)
                throw new SDL2Exception();

            if (SDL_GL_SetAttribute(SDL_GL_RETAINED_BACKING, 0) < 0)
                throw new SDL2Exception();

            if (configuration.Use32BitFramebuffer)
            {
                if (SDL_GL_SetAttribute(SDL_GL_RED_SIZE, 8) < 0)
                    throw new SDL2Exception();

                if (SDL_GL_SetAttribute(SDL_GL_GREEN_SIZE, 8) < 0)
                    throw new SDL2Exception();

                if (SDL_GL_SetAttribute(SDL_GL_BLUE_SIZE, 8) < 0)
                    throw new SDL2Exception();
            }
            else
            {
                if (SDL_GL_SetAttribute(SDL_GL_RED_SIZE, 5) < 0)
                    throw new SDL2Exception();

                if (SDL_GL_SetAttribute(SDL_GL_GREEN_SIZE, 6) < 0)
                    throw new SDL2Exception();

                if (SDL_GL_SetAttribute(SDL_GL_BLUE_SIZE, 5) < 0)
                    throw new SDL2Exception();
            }

            if (configuration.SrgbBuffersEnabled)
            {
                if (SDL_GL_SetAttribute(SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, 1) < 0)
                    throw new SDL2Exception();

                unsafe
                {
                    var value = 0;
                    if (SDL_GL_GetAttribute(SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, &value) < 0)
                        throw new SDL2Exception();

                    if (value != 1)
                        configuration.SrgbBuffersEnabled = false;
                }
            }
        }

        // Ultraviolet subsystems.
        private readonly IUltravioletPlatform platform;
        private readonly IUltravioletContent content;
        private readonly IUltravioletGraphics graphics;
        private readonly IUltravioletAudio audio;
        private readonly IUltravioletInput input;
        private readonly IUltravioletUI ui;
    }
}
