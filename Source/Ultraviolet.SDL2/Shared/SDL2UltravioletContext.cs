using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2.Platform;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the base class for Ultraviolet implementations which use SDL2.
    /// </summary>
    public abstract class SDL2UltravioletContext : UltravioletContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2UltravioletContext"/> class.
        /// </summary>
        /// <param name="host">The object that is hosting the Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        protected unsafe SDL2UltravioletContext(IUltravioletHost host, UltravioletConfiguration configuration)
            : base(host, configuration)
        {
            eventFilter = new SDL.EventFilter(SDLEventFilter);
            eventFilterPtr = Marshal.GetFunctionPointerForDelegate(eventFilter);
            SDL.SetEventFilter(eventFilterPtr, IntPtr.Zero);
        }
        
        /// <inheritdoc/>
        public override void UpdateSuspended()
        {
            SDL.PumpEvents();

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
        protected override void OnShutdown()
        {
            SDL.SetEventFilter(IntPtr.Zero, IntPtr.Zero);
            SDL.Quit();

            base.OnShutdown();
        }

        /// <summary>
        /// Initializes SDL2.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for this context.</param>
        /// <returns><see langword="true"/> if SDL2 was successfully initialized; otherwise, <see langword="false"/>.</returns>
        protected Boolean InitSDL(UltravioletConfiguration configuration)
        {
            var sdlFlags = configuration.EnableServiceMode ?
                SDL_Init.TIMER | SDL_Init.EVENTS :
                SDL_Init.TIMER | SDL_Init.VIDEO | SDL_Init.JOYSTICK | SDL_Init.GAMECONTROLLER | SDL_Init.EVENTS;

            if (Platform == UltravioletPlatform.Windows)
                SDL.SetHint(SDL_Hint.HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");

            return SDL.Init(sdlFlags) == 0;
        }

        /// <summary>
        /// Pumps the SDL2 event queue.
        /// </summary>
        /// <returns><see langword="true"/> if the context should continue processing the frame; otherwise, <see langword="false"/>.</returns>
        protected Boolean PumpEvents()
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
                            var glWindowInfo = (SDL2UltravioletWindowInfo)GetPlatform().Windows;
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
        
        /// <summary>
        /// Filters SDL2 events.
        /// </summary>
        [MonoPInvokeCallback(typeof(SDL.EventFilter))]
        private static unsafe Int32 SDLEventFilter(IntPtr userdata, SDL_Event* @event)
        {
            var uv = RequestCurrent();
            if (uv == null)
                return 1;

            switch (@event->type)
            {
                case SDL_EventType.APP_TERMINATING:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationTerminating, null);
                    return 0;

                case SDL_EventType.APP_WILLENTERBACKGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationSuspending, null);
                    return 0;

                case SDL_EventType.APP_DIDENTERBACKGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationSuspended, null);
                    return 0;

                case SDL_EventType.APP_WILLENTERFOREGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationResuming, null);
                    return 0;

                case SDL_EventType.APP_DIDENTERFOREGROUND:
                    uv.Messages.PublishImmediate(UltravioletMessages.ApplicationResumed, null);
                    return 0;

                case SDL_EventType.APP_LOWMEMORY:
                    uv.Messages.PublishImmediate(UltravioletMessages.LowMemory, null);
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                    return 0;
            }

            return 1;
        }
        
        // The SDL event filter.
        private readonly SDL.EventFilter eventFilter;
        private readonly IntPtr eventFilterPtr;
    }
}
