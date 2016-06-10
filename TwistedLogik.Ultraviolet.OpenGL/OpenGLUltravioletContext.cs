using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Platform;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the Ultraviolet context.
    /// </summary>
    [CLSCompliant(true)]
    public sealed class OpenGLUltravioletContext : UltravioletContext
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

            this.IsHardwareInputDisabled = configuration.IsHardwareInputDisabled;

            var sdlFlags = configuration.EnableServiceMode ?
                SDL_Init.TIMER | SDL_Init.EVENTS :
                SDL_Init.TIMER | SDL_Init.VIDEO | SDL_Init.JOYSTICK | SDL_Init.GAMECONTROLLER | SDL_Init.EVENTS;

            if (SDL.Init(sdlFlags) != 0)
                throw new SDL2Exception();

            var isGLES = (Platform == UltravioletPlatform.Android || Platform == UltravioletPlatform.iOS);

            var versionRequired = isGLES ? new Version(2, 0) : new Version(3, 1);
            var versionRequested = isGLES ? configuration.MinimumOpenGLESVersion : configuration.MinimumOpenGLVersion;
            if (versionRequested == null || versionRequested < versionRequired)
            {
                versionRequested = versionRequired;
            }

            if (!configuration.EnableServiceMode)
            {
                var profile = isGLES ? SDL_GLprofile.ES : SDL_GLprofile.CORE;

                if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_PROFILE_MASK, (Int32)profile) < 0)
                    throw new SDL2Exception();

                if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MAJOR_VERSION, versionRequested.Major) < 0)
                    throw new SDL2Exception();

                if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MINOR_VERSION, versionRequested.Minor) < 0)
                    throw new SDL2Exception();

                if (SDL.GL_SetAttribute(SDL_GLattr.DEPTH_SIZE, configuration.BackBufferDepthSize) < 0)
                    throw new SDL2Exception();

                if (SDL.GL_SetAttribute(SDL_GLattr.STENCIL_SIZE, configuration.BackBufferStencilSize) < 0)
                    throw new SDL2Exception();
            }

            this.platform = IsRunningInServiceMode ? (IUltravioletPlatform)(new DummyUltravioletPlatform(this)) : new OpenGLUltravioletPlatform(this, configuration);

            PumpEvents();

            this.graphics = IsRunningInServiceMode ? (IUltravioletGraphics)(new DummyUltravioletGraphics(this)) : new OpenGLUltravioletGraphics(this, configuration);
            this.audio    = IsRunningInServiceMode ? new DummyUltravioletAudio(this) : InitializeAudioSubsystem(configuration);
            this.input    = IsRunningInServiceMode ? (IUltravioletInput)(new DummyUltravioletInput(this)) : new SDL2UltravioletInput(this);
            this.content  = new UltravioletContent(this);
            this.ui       = new UltravioletUI(this, configuration);

            this.content.RegisterImportersAndProcessors(new[] 
            {
                typeof(SDL2.Native.SDL).Assembly,
                String.IsNullOrEmpty(configuration.AudioSubsystemAssembly) ? null : Assembly.Load(configuration.AudioSubsystemAssembly),
                String.IsNullOrEmpty(configuration.ViewProviderAssembly) ? null : Assembly.Load(configuration.ViewProviderAssembly)
            });
            this.content.Importers.RegisterImporter<XmlContentImporter>("prog");

            PumpEvents();

            InitializeContext();
            InitializeViewProvider(configuration);
        }

        /// <inheritdoc/>
        public override void UpdateSuspended()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!PumpEvents())
            {
                return;
            }

            ProcessMessages();

            base.UpdateSuspended();
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.Require(time, nameof(time));
            Contract.EnsureNotDisposed(this, Disposed);

            var sdlinput = input as SDL2UltravioletInput;
            if (sdlinput != null)
                sdlinput.ResetDeviceStates();

            if (!PumpEvents())
            {
                return;
            }

            ProcessMessages();

            OnUpdatingSubsystems(time);

            platform.Update(time);
            content.Update(time);

            if (!IsRunningInServiceMode)
            {
                graphics.Update(time);
            }

            audio.Update(time);
            input.Update(time);
            ui.Update(time);

            ProcessMessages();

            OnUpdating(time);

            UpdateContext(time);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnDrawing(time);

            var oglgfx = graphics as OpenGLUltravioletGraphics;
            if (oglgfx != null)
            {
                var glcontext = oglgfx.OpenGLContext;
                var windowInfo = ((OpenGLUltravioletWindowInfo)platform.Windows);
                foreach (OpenGLUltravioletWindow window in windowInfo)
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
                oglgfx.UpdateFrameRate();
            }

            base.Draw(time);
        }

        /// <inheritdoc/>
        public override IUltravioletPlatform GetPlatform()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return platform;
        }

        /// <inheritdoc/>
        public override IUltravioletContent GetContent()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return content;
        }

        /// <inheritdoc/>
        public override IUltravioletGraphics GetGraphics()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return graphics;
        }

        /// <inheritdoc/>
        public override IUltravioletAudio GetAudio()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return audio;
        }

        /// <inheritdoc/>
        public override IUltravioletInput GetInput()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return input;
        }

        /// <inheritdoc/>
        public override IUltravioletUI GetUI()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return ui;
        }

        /// <inheritdoc/>
        protected override void OnShutdown()
        {
            SDL.Quit();

            base.OnShutdown();
        }

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
        /// Pumps the SDL2 event queue.
        /// </summary>
        /// <returns><see langword="true"/> if the context should continue processing the frame; otherwise, <see langword="false"/>.</returns>
        private Boolean PumpEvents()
        {
            SDL_Event @event;
            while (SDL.PollEvent(out @event) > 0)
            {
                if (Disposed)
                    return false;

                switch (@event.type)
                {
                    case SDL_EventType.WINDOWEVENT:
                        if (@event.window.@event == SDL_WindowEventID.CLOSE)
                        {
                            var glWindowInfo = (OpenGLUltravioletWindowInfo)GetPlatform().Windows;
                            if (glWindowInfo.DestroyByID((int)@event.window.windowID))
                            {
                                Messages.Publish(UltravioletMessages.Quit, null);
                                return true;
                            }
                        }
                        break;

                    case SDL_EventType.KEYDOWN:
                    case SDL_EventType.KEYUP:
                    case SDL_EventType.MOUSEBUTTONDOWN:
                    case SDL_EventType.MOUSEBUTTONUP:
                    case SDL_EventType.MOUSEMOTION:
                    case SDL_EventType.MOUSEWHEEL:
                    case SDL_EventType.JOYAXISMOTION:
                    case SDL_EventType.JOYBALLMOTION:
                    case SDL_EventType.JOYBUTTONDOWN:
                    case SDL_EventType.JOYBUTTONUP:
                    case SDL_EventType.JOYHATMOTION:
                    case SDL_EventType.CONTROLLERAXISMOTION:
                    case SDL_EventType.CONTROLLERBUTTONDOWN:
                    case SDL_EventType.CONTROLLERBUTTONUP:
                        if (IsHardwareInputDisabled)
                        {
                            continue;
                        }
                        break;

                    case SDL_EventType.QUIT:
                        Messages.Publish(UltravioletMessages.Quit, null);
                        return true;
                }                

                // Publish any SDL events to the message queue.
                var data = Messages.CreateMessageData<SDL2EventMessageData>();
                data.Event = @event;
                Messages.Publish(SDL2UltravioletMessages.SDLEvent, data);
            }
            return !Disposed;
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
