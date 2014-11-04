using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="GamePadDevice"/> class.
    /// </summary>
    public sealed class SDL2GamePadDevice : GamePadDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes the <see cref="SDL2GamePadDevice"/> type.
        /// </summary>
        static SDL2GamePadDevice()
        {
            sdlButtons = (SDL_GameControllerButton[])Enum.GetValues(typeof(SDL_GameControllerButton));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2GamePadDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="index">The index of the SDL2 joystick device.</param>
        public SDL2GamePadDevice(UltravioletContext uv, Int32 index)
            : base(uv)
        {
            this.index = index;
            this.controller = SDL.GameControllerOpen(index);

            if (this.controller == IntPtr.Zero)
            {
                throw new SDL2Exception();
            }

            this.gamePadStateOld = new Boolean[sdlButtons.Length];
            this.gamePadStateNew = new Boolean[sdlButtons.Length];

            this.name = SDL.GameControllerNameForIndex(index);

            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <summary>
        /// Gets the pointer to the native SDL2 object that this object represents.
        /// </summary>
        /// <returns>A pointer to the native SDL2 object that this object represents.</returns>
        public IntPtr ToNative()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return controller;
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type != SDL2UltravioletMessages.SDLEvent)
                return;

            var evt = ((SDL2EventMessageData)data).Event;
            switch (evt.type)
            {
                case SDL_EventType.CONTROLLERBUTTONDOWN:
                    {
                        var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                        OnButtonPressed(button);
                    }
                    break;

                case SDL_EventType.CONTROLLERBUTTONUP:
                    {
                        var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                        OnButtonReleased(button);
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var temp = this.gamePadStateOld;
            this.gamePadStateOld = this.gamePadStateNew;
            this.gamePadStateNew = temp;

            foreach (var sdlButton in sdlButtons)
            {
                var state = SDL.GameControllerGetButton(controller, sdlButton);
                var btnval = (Int32)SDLToUltravioletButton(sdlButton);

                this.gamePadStateNew[btnval] = state;
            }
        }

        /// <inheritdoc/>
        public override bool IsButtonDown(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return gamePadStateNew[btnval];
        }

        /// <inheritdoc/>
        public override bool IsButtonUp(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return !gamePadStateNew[btnval];
        }

        /// <inheritdoc/>
        public override bool IsButtonPressed(GamePadButton button, bool ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return gamePadStateNew[btnval] && !gamePadStateOld[btnval];
        }

        /// <inheritdoc/>
        public override bool IsButtonReleased(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return !gamePadStateNew[btnval] && gamePadStateOld[btnval];
        }

        /// <inheritdoc/>
        public override ButtonState GetButtonState(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var state = IsButtonDown(button) ? ButtonState.Down : ButtonState.Up;

            if (IsButtonPressed(button))
                state |= ButtonState.Pressed;

            if (IsButtonReleased(button))
                state |= ButtonState.Released;

            return state;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.Messages.Unsubscribe(this);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Converts an SDL2 SDL_GameControllerButton value to an Ultraviolet GamePadButton value.
        /// </summary>
        /// <param name="button">The <see cref="SDL_GameControllerButton"/> value to convert.</param>
        /// <returns>The converted <see cref="GamePadButton"/> value.</returns>
        private static GamePadButton SDLToUltravioletButton(SDL_GameControllerButton button)
        {
            return (GamePadButton)(1 + (int)button);
        }

        // State values.
        private static readonly SDL_GameControllerButton[] sdlButtons;
        private readonly Int32 index;
        private readonly IntPtr controller;
        private readonly String name;
        private Boolean[] gamePadStateOld;
        private Boolean[] gamePadStateNew;
    }
}
