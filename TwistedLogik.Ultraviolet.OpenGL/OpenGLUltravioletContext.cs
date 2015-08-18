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
            Contract.Require(configuration, "configuration");

            if (SDL.Init(SDL_Init.TIMER | SDL_Init.VIDEO | SDL_Init.JOYSTICK | SDL_Init.GAMECONTROLLER | SDL_Init.EVENTS) != 0)
                throw new SDL2Exception();

            var versionRequired  = new Version(3, 0);
            var versionRequested = configuration.MinimumOpenGLVersion;
            if (versionRequested == null || versionRequested < versionRequired)
            {
                versionRequested = versionRequired;
            }

            if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_PROFILE_MASK, (int)SDL_GLprofile.CORE) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MAJOR_VERSION, 3) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_MINOR_VERSION, versionRequested.Minor) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetAttribute(SDL_GLattr.DEPTH_SIZE, configuration.BackBufferDepthSize) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetAttribute(SDL_GLattr.STENCIL_SIZE, configuration.BackBufferStencilSize) < 0)
                throw new SDL2Exception();

            this.platform = new OpenGLUltravioletPlatform(this, configuration);

            PumpEvents();

            this.graphics = new OpenGLUltravioletGraphics(this, configuration);
            this.audio = InitializeAudioSubsystem(configuration);
            this.input = new SDL2UltravioletInput(this);
            this.content = new UltravioletContent(this);
            this.ui = new UltravioletUI(this, configuration);

            this.content.RegisterImportersAndProcessors(new[] 
            {
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
            Contract.Require(time, "time");
            Contract.EnsureNotDisposed(this, Disposed);

            input.ResetDeviceStates();

            if (!PumpEvents())
            {
                return;
            }

            ProcessMessages();

            OnUpdatingSubsystems(time);

            platform.Update(time);
            content.Update(time);
            graphics.Update(time);
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

            graphics.SetRenderTarget(null);

            var glcontext = graphics.OpenGLContext;
            var windowInfo = ((OpenGLUltravioletWindowInfo)platform.Windows);
            foreach (OpenGLUltravioletWindow window in windowInfo)
            {
                windowInfo.DesignateCurrent(window, glcontext);

                var size = window.ClientSize;
                graphics.SetViewport(new Viewport(0, 0, size.Width, size.Height));
                graphics.Clear(Color.CornflowerBlue, 1.0, 0);

                windowInfo.Draw(time);
            }

            windowInfo.DesignateCurrent(null, glcontext);
            graphics.UpdateFrameRate();

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
        /// <returns><c>true</c> if the context should continue processing the frame; otherwise, <c>false</c>.</returns>
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
        private readonly OpenGLUltravioletPlatform platform;
        private readonly IUltravioletContent content;
        private readonly OpenGLUltravioletGraphics graphics;
        private readonly IUltravioletAudio audio;
        private readonly SDL2UltravioletInput input;
        private readonly IUltravioletUI ui;
    }
}
