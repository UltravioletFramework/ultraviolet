﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2.Platform;
using Ultraviolet.UI;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDL_Hint;
using static Ultraviolet.SDL2.Native.SDL_Init;
using static Ultraviolet.SDL2.Native.SDL_WindowEventID;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the base class for Ultraviolet implementations which use SDL2.
    /// </summary>
    [CLSCompliant(true)]
    public class SDL2UltravioletContext : UltravioletContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2UltravioletContext"/> class.
        /// </summary>
        /// <param name="host">The object that is hosting the Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        public unsafe SDL2UltravioletContext(IUltravioletHost host, SDL2UltravioletConfiguration configuration)
            : base(host, configuration)
        {
            Contract.Require(configuration, nameof(configuration));

            if (!InitSDL(configuration))
                throw new SDL2Exception();

            this.onWindowDrawing = (context, time, window) => 
                ((SDL2UltravioletContext)context).OnWindowDrawing(time, window);
            this.onWindowDrawn = (context, time, window) => 
                ((SDL2UltravioletContext)context).OnWindowDrawn(time, window);

            eventFilter = new SDL_EventFilter(SDLEventFilter);
            eventFilterPtr = Marshal.GetFunctionPointerForDelegate(eventFilter);
            SDL_SetEventFilter(eventFilterPtr, IntPtr.Zero);

            LoadSubsystemAssemblies(configuration);
            this.swapChainManager = IsRunningInServiceMode ? new DummySwapChainManager(this) : SwapChainManager.Create();
            
            this.platform = InitializePlatformSubsystem(configuration);
            this.graphics = InitializeGraphicsSubsystem(configuration);

            if (!this.platform.IsPrimaryWindowInitialized)
                throw new InvalidOperationException(UltravioletStrings.PrimaryWindowMustBeInitialized);

            this.audio = InitializeAudioSubsystem(configuration);
            this.input = InitializeInputSubsystem();
            this.content = InitializeContentSubsystem();
            this.ui = InitializeUISubsystem(configuration);

            PumpEvents();

            InitializeContext();
            InitializeViewProvider(configuration);
            InitializePlugins(configuration);
        }
        
        /// <inheritdoc/>
        public override void UpdateSuspended()
        {
            SDL_PumpEvents();

            base.UpdateSuspended();
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.Require(time, nameof(time));
            Contract.EnsureNotDisposed(this, Disposed);

            var sdlinput = GetInput() as SDL2UltravioletInput;
            if (sdlinput != null)
                sdlinput.ResetDeviceStates();

            if (!PumpEvents())
            {
                return;
            }

            ProcessMessages();

            OnUpdatingSubsystems(time);

            GetPlatform().Update(time);
            GetContent().Update(time);

            if (!IsRunningInServiceMode)
                GetGraphics().Update(time);

            GetAudio().Update(time);
            GetInput().Update(time);
            GetUI().Update(time);

            ProcessMessages();

            OnUpdating(time);

            UpdateContext(time);
        }

        /// <inheritdoc/>
        public override void Draw(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnDrawing(time);
            swapChainManager.DrawAndSwap(time, onWindowDrawing, onWindowDrawn);

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
        /// Gets the assembly that implements the graphics subsystem.
        /// </summary>
        public Assembly GraphicsSubsystemAssembly { get; private set; }

        /// <summary>
        /// Gets the assembly that implements the audio subsystem.
        /// </summary>
        public Assembly AudioSubsystemAssembly { get; private set; }

        /// <inheritdoc/>
        protected override void OnShutdown()
        {
            SDL_SetEventFilter(IntPtr.Zero, IntPtr.Zero);
            SDL_Quit();

            base.OnShutdown();
        }

        /// <summary>
        /// Loads the context's subsystem assemblies.
        /// </summary>
        /// <param name="configuration">The context's configuration settings.</param>
        private void LoadSubsystemAssemblies(UltravioletConfiguration configuration)
        {
            if (IsRunningInServiceMode)
                return;

            Assembly LoadSubsystemAssembly(String name)
            {
                try
                {
                    return Assembly.Load(name);
                }
                catch (Exception e)
                {
                    if (e is FileNotFoundException || e is FileLoadException || e is BadImageFormatException)
                    {
                        return null;
                    }
                    throw;
                }
            }

            if (String.IsNullOrEmpty(configuration.GraphicsSubsystemAssembly))
                throw new InvalidOperationException(SDL2Strings.MissingGraphicsAssembly);

            if (String.IsNullOrEmpty(configuration.AudioSubsystemAssembly))
                throw new InvalidOperationException(SDL2Strings.MissingAudioAssembly);

            this.GraphicsSubsystemAssembly = LoadSubsystemAssembly(configuration.GraphicsSubsystemAssembly);
            if (this.GraphicsSubsystemAssembly == null)
                throw new InvalidOperationException(SDL2Strings.InvalidGraphicsAssembly);

            this.AudioSubsystemAssembly = LoadSubsystemAssembly(configuration.AudioSubsystemAssembly);
            if (this.AudioSubsystemAssembly == null)
                throw new InvalidOperationException(SDL2Strings.InvalidAudioAssembly);

            var distinctAssemblies = new[] { this.GraphicsSubsystemAssembly, this.AudioSubsystemAssembly }.Distinct();
            foreach (var distinctAssembly in distinctAssemblies)
                InitializeFactoryMethodsInAssembly(distinctAssembly);
        }

        /// <summary>
        /// Attempts to create a new instance of the specified Ultraviolet subsystem by dynamically loading it from the specified assembly.
        /// </summary>
        /// <typeparam name="TSubsystem">The subsystem interface type.</typeparam>
        /// <param name="assembly">The assembly from which to load the subsystem implementation.</param>
        /// <param name="configuration">The Ultraviolet context configuration.</param>
        /// <param name="instance">The subsystem instance that was created.</param>
        /// <returns><see langword="true"/> if the subsystem instance was created; otherwise, <see langword="false"/>.</returns>
        private Boolean TryCreateSubsystemInstance<TSubsystem>(Assembly assembly, UltravioletConfiguration configuration, out TSubsystem instance)
        {
            var types = (from t in assembly.GetTypes()
                         where
                          t.IsClass && !t.IsAbstract &&
                          t.GetInterfaces().Contains(typeof(TSubsystem))
                         select t).ToList();

            if (!types.Any() || types.Count > 1)
                throw new InvalidOperationException(SDL2Strings.InvalidAudioAssembly);

            var type = types.Single();

            var ctorWithConfig = type.GetConstructor(new[] { typeof(UltravioletContext), typeof(UltravioletConfiguration) });
            if (ctorWithConfig != null)
            {
                instance = (TSubsystem)ctorWithConfig.Invoke(new object[] { this, configuration });
                return true;
            }

            var ctorWithoutConfig = type.GetConstructor(new[] { typeof(UltravioletContext) });
            if (ctorWithoutConfig != null)
            {
                instance = (TSubsystem)ctorWithoutConfig.Invoke(new object[] { this });
                return true;
            }

            instance = default(TSubsystem);
            return false;
        }

        /// <summary>
        /// Initializes the context's platform subsystem.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns>The platform subsystem.</returns>
        private IUltravioletPlatform InitializePlatformSubsystem(UltravioletConfiguration configuration)
        {
            if (IsRunningInServiceMode)
            {
                return new DummyUltravioletPlatform(this);
            }
            else
            {
                var platform = new SDL2UltravioletPlatform(this, configuration);
                PumpEvents();
                return platform;
            }
        }

        /// <summary>
        /// Initializes the context's content subsystem.
        /// </summary>
        /// <returns>The content subsystem.</returns>
        private IUltravioletContent InitializeContentSubsystem()
        {
            var content = new UltravioletContent(this);
            content.RegisterImportersAndProcessors(new[] { GraphicsSubsystemAssembly, AudioSubsystemAssembly }.Distinct());
            content.Importers.RegisterImporter<XmlContentImporter>("prog");
            return content;
        }

        /// <summary>
        /// Initializes the context's graphics subsystem.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns>The graphics subsystem.</returns>
        private IUltravioletGraphics InitializeGraphicsSubsystem(UltravioletConfiguration configuration)
        {
            if (IsRunningInServiceMode)
                return new DummyUltravioletGraphics(this);

            if (!TryCreateSubsystemInstance<IUltravioletGraphics>(GraphicsSubsystemAssembly, configuration, out var graphics))
                throw new InvalidOperationException(SDL2Strings.InvalidGraphicsAssembly);

            return graphics;
        }

        /// <summary>
        /// Initializes the context's audio subsystem.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns>The audio subsystem.</returns>
        private IUltravioletAudio InitializeAudioSubsystem(UltravioletConfiguration configuration)
        {
            if (IsRunningInServiceMode)
                return new DummyUltravioletAudio(this);

            if (!TryCreateSubsystemInstance<IUltravioletAudio>(AudioSubsystemAssembly, configuration, out var audio))
                throw new InvalidOperationException(SDL2Strings.InvalidAudioAssembly);

            return audio;
        }

        /// <summary>
        /// Initializes the context's input subsystem.
        /// </summary>
        /// <returns>The input subsystem.</returns>
        private IUltravioletInput InitializeInputSubsystem()
        {
            if (IsRunningInServiceMode)
                return new DummyUltravioletInput(this);

            return new SDL2UltravioletInput(this);
        }

        /// <summary>
        /// Initializes the context's UI subsystem.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns>The UI subsystem.</returns>
        private IUltravioletUI InitializeUISubsystem(UltravioletConfiguration configuration)
        {
            return new UltravioletUI(this, configuration);
        }

        /// <summary>
        /// Initializes SDL2.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns><see langword="true"/> if SDL2 was successfully initialized; otherwise, <see langword="false"/>.</returns>
        private Boolean InitSDL(UltravioletConfiguration configuration)
        {
            var sdlFlags = configuration.EnableServiceMode ?
                SDL_INIT_TIMER | SDL_INIT_EVENTS :
                SDL_INIT_TIMER | SDL_INIT_VIDEO | SDL_INIT_JOYSTICK | SDL_INIT_GAMECONTROLLER | SDL_INIT_EVENTS;

            if (Platform == UltravioletPlatform.Windows)
            {
                if (!SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1"))
                    throw new SDL2Exception();
            }

            return SDL_Init(sdlFlags) == 0;
        }

        /// <summary>
        /// Pumps the SDL2 event queue.
        /// </summary>
        /// <returns><see langword="true"/> if the context should continue processing the frame; otherwise, <see langword="false"/>.</returns>
        private Boolean PumpEvents()
        {
            SDL_Event @event;
            while (SDL_PollEvent(out @event) > 0)
            {
                if (Disposed)
                    return false;

                switch (@event.type)
                {
                    case SDL_WINDOWEVENT:
                        if (@event.window.@event == SDL_WINDOWEVENT_CLOSE)
                        {
                            var glWindowInfo = (SDL2UltravioletWindowInfo)GetPlatform().Windows;
                            if (glWindowInfo.DestroyByID((int)@event.window.windowID))
                            {
                                Messages.Publish(UltravioletMessages.Quit, null);
                                return true;
                            }
                        }
                        break;

                    case SDL_KEYDOWN:
                    case SDL_KEYUP:
                    case SDL_MOUSEBUTTONDOWN:
                    case SDL_MOUSEBUTTONUP:
                    case SDL_MOUSEMOTION:
                    case SDL_MOUSEWHEEL:
                    case SDL_JOYAXISMOTION:
                    case SDL_JOYBALLMOTION:
                    case SDL_JOYBUTTONDOWN:
                    case SDL_JOYBUTTONUP:
                    case SDL_JOYHATMOTION:
                    case SDL_CONTROLLERAXISMOTION:
                    case SDL_CONTROLLERBUTTONDOWN:
                    case SDL_CONTROLLERBUTTONUP:
                        if (IsHardwareInputDisabled)
                        {
                            continue;
                        }
                        break;

                    case SDL_QUIT:
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
        
        /// <summary>
        /// Filters SDL2 events.
        /// </summary>
        [MonoPInvokeCallback(typeof(SDL_EventFilter))]
        private static unsafe Int32 SDLEventFilter(IntPtr userdata, SDL_Event* @event)
        {
            var uv = RequestCurrent();
            if (uv == null)
                return 1;

            switch (@event->type)
            {
                case SDL_APP_TERMINATING:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationTerminating, null);
                    return 0;

                case SDL_APP_WILLENTERBACKGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationSuspending, null);
                    return 0;

                case SDL_APP_DIDENTERBACKGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationSuspended, null);
                    return 0;

                case SDL_APP_WILLENTERFOREGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationResuming, null);
                    return 0;

                case SDL_APP_DIDENTERFOREGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationResumed, null);
                    return 0;

                case SDL_APP_LOWMEMORY:
                    uv.Messages.PublishImmediate(UltravioletMessages.LowMemory, null);
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                    return 0;
            }

            return 1;
        }
        
        // The SDL event filter.
        private readonly SDL_EventFilter eventFilter;
        private readonly IntPtr eventFilterPtr;

        // Ultraviolet subsystems.
        private readonly SwapChainManager swapChainManager;
        private readonly IUltravioletPlatform platform;
        private readonly IUltravioletContent content;
        private readonly IUltravioletGraphics graphics;
        private readonly IUltravioletAudio audio;
        private readonly IUltravioletInput input;
        private readonly IUltravioletUI ui;

        // Delegate caches.
        private readonly Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawing;
        private readonly Action<UltravioletContext, UltravioletTime, IUltravioletWindow> onWindowDrawn;
    }
}
