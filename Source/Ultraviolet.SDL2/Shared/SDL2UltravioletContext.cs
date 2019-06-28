using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2.Platform;
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
            eventFilter = new SDL_EventFilter(SDLEventFilter);
            eventFilterPtr = Marshal.GetFunctionPointerForDelegate(eventFilter);
            SDL_SetEventFilter(eventFilterPtr, IntPtr.Zero);
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
        protected override void OnShutdown()
        {
            SDL_SetEventFilter(IntPtr.Zero, IntPtr.Zero);
            SDL_Quit();

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
        protected Boolean PumpEvents()
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
    }
}
