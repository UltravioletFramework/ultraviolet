using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2;
using Ultraviolet.SDL2.Native;
using Ultraviolet.UI;

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

            var sdlAssembly = typeof(SDLNative).Assembly;
            InitializeFactoryMethodsInAssembly(sdlAssembly);

            this.environment = Factory.GetFactoryMethod<OpenGLEnvironmentFactory>()(this);
            InitOpenGLVersion(configuration, out var versionRequested, out var versionRequired, out var isGLEs);

            if (!configuration.EnableServiceMode)
                InitOpenGLEnvironment(configuration, isGLEs);

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
                this.graphics = new OpenGLUltravioletGraphics(this, environment, configuration, versionRequested, versionRequired);
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
                foreach (var window in platform.Windows)
                {
                    environment.DesignateCurrentWindow(window, glcontext);

                    window.Compositor.BeginFrame();
                    window.Compositor.BeginContext(CompositionContext.Scene);

                    OnWindowDrawing(time, window);

                    environment.DrawFramebuffer(time);

                    OnWindowDrawn(time, window);

                    window.Compositor.Compose();
                    window.Compositor.Present();

                    environment.SwapFramebuffers();
                }

                environment.DesignateCurrentWindow(null, glcontext);

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

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(environment);
            }
            base.Dispose(disposing);
        }

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
                throw new InvalidOperationException(OpenGLStrings.MissingAudioAssembly);

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
        /// Sets the OpenGL environment attributes which correspond to the application's OpenGL settings.
        /// </summary>
        private void InitOpenGLEnvironment(OpenGLUltravioletConfiguration configuration, Boolean isGLES)
        {
            if (!environment.RequestOpenGLProfile(isGLES))
                environment.ThrowPlatformErrorException();

            if (!environment.RequestDepthSize(configuration.BackBufferDepthSize))
                environment.ThrowPlatformErrorException();

            if (!environment.RequestStencilSize(configuration.BackBufferStencilSize))
                environment.ThrowPlatformErrorException();

            if (configuration.Use32BitFramebuffer)
            {
                if (!environment.Request32BitFramebuffer())
                    environment.ThrowPlatformErrorException();
            }
            else
            {
                if (!environment.Request24BitFramebuffer())
                    environment.ThrowPlatformErrorException();
            }

            if (configuration.SrgbBuffersEnabled)
            {
                if (!environment.RequestSrgbCapableFramebuffer())
                    environment.ThrowPlatformErrorException();

                if (!environment.IsFramebufferSrgbCapable)
                    configuration.SrgbBuffersEnabled = false;
            }
        }

        // Ultraviolet subsystems.
        private readonly OpenGLEnvironment environment;
        private readonly IUltravioletPlatform platform;
        private readonly IUltravioletContent content;
        private readonly IUltravioletGraphics graphics;
        private readonly IUltravioletAudio audio;
        private readonly IUltravioletInput input;
        private readonly IUltravioletUI ui;
    }
}
