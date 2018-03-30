using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Input;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDL_Keymod;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the KeyboardDevice class.
    /// </summary>
    public sealed class SDL2KeyboardDevice : KeyboardDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the SDL2KeyboardDevice class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SDL2KeyboardDevice(UltravioletContext uv)
            : base(uv)
        {
            Int32 numkeys;
            SDL_GetKeyboardState(out numkeys);

            this.states = new InternalButtonState[numkeys];

            uv.Messages.Subscribe(this,
                UltravioletMessages.SoftwareKeyboardShown);
            uv.Messages.Subscribe(this,
                UltravioletMessages.SoftwareKeyboardHidden);

            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.SoftwareKeyboardShown)
            {
                SDL_StartTextInput();
            }
            else if (type == UltravioletMessages.SoftwareKeyboardHidden)
            {
                SDL_StopTextInput();
            }
            else if (type == SDL2UltravioletMessages.SDLEvent)
            {
                var evt = ((SDL2EventMessageData)data).Event;
                switch (evt.type)
                {
                    case SDL_KEYDOWN:
                        {
                            if (!isRegistered)
                                Register();

                            OnKeyDown(ref evt.key);
                        }
                        break;

                    case SDL_KEYUP:
                        {
                            if (!isRegistered)
                                Register();

                            OnKeyUp(ref evt.key);
                        }
                        break;

                    case SDL_TEXTEDITING:
                        {
                            if (!isRegistered)
                                Register();

                            OnTextEditing(ref evt.edit);
                        }
                        break;

                    case SDL_TEXTINPUT:
                        {
                            if (isRegistered)
                                Register();

                            OnTextInput(ref evt.text);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Resets the device's state in preparation for the next frame.
        /// </summary>
        public void ResetDeviceState()
        {
            for (int i = 0; i < states.Length; i++)
            {
                states[i].Reset();
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {

        }

        /// <inheritdoc/>
        public override void GetTextInput(StringBuilder sb, Boolean append = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (!append)
                sb.Length = 0;

            for (int i = 0; i < textInputLength; i++)
            {
                sb.Append(textUtf16[i]);
            }
        }

        /// <inheritdoc/>
        public override Boolean IsButtonDown(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return states[scancode].Down;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonUp(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return states[scancode].Up;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonPressed(Scancode button, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return states[scancode].Pressed || (!ignoreRepeats && states[scancode].Repeated);
        }

        /// <inheritdoc/>
        public override Boolean IsButtonReleased(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return states[scancode].Released;
        }

        /// <inheritdoc/>
        public override Boolean IsKeyDown(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL_GetScancodeFromKey((SDL_Keycode)key);
            return states[scancode].Down;
        }

        /// <inheritdoc/>
        public override Boolean IsKeyUp(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL_GetScancodeFromKey((SDL_Keycode)key);
            return states[scancode].Up;
        }

        /// <inheritdoc/>
        public override Boolean IsKeyPressed(Key key, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL_GetScancodeFromKey((SDL_Keycode)key);
            return states[scancode].Pressed || (!ignoreRepeats && states[scancode].Repeated);
        }

        /// <inheritdoc/>
        public override Boolean IsKeyReleased(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL_GetScancodeFromKey((SDL_Keycode)key);
            return states[scancode].Released;
        }

        /// <inheritdoc/>
        public override ButtonState GetKeyState(Key key)
        {
            var state = IsKeyDown(key) ? ButtonState.Down : ButtonState.Up;

            if (IsKeyPressed(key))
                state |= ButtonState.Pressed;

            if (IsKeyReleased(key))
                state |= ButtonState.Released;

            return state;
        }

        /// <inheritdoc/>
        public override Boolean IsNumLockDown =>
            Disposed ? throw new ObjectDisposedException(GetType().Name) : (SDL_GetModState() & KMOD_NUM) == KMOD_NUM;

        /// <inheritdoc/>
        public override Boolean IsCapsLockDown =>
            Disposed ? throw new ObjectDisposedException(GetType().Name) : (SDL_GetModState() & KMOD_CAPS) == KMOD_CAPS;

        /// <inheritdoc/>
        public override Boolean IsRegistered => isRegistered;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

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
        /// Handles SDL2's KEYDOWN event.
        /// </summary>
        private void OnKeyDown(ref SDL_KeyboardEvent evt)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            var mods   = evt.keysym.mod;
            var ctrl   = (mods & KMOD_CTRL) != 0;
            var alt    = (mods & KMOD_ALT) != 0;
            var shift  = (mods & KMOD_SHIFT) != 0;
            var repeat = evt.repeat > 0;

            states[(int)evt.keysym.scancode].OnDown(repeat);

            if (!repeat)
            {
                OnButtonPressed(window, (Scancode)evt.keysym.scancode);
            }
            OnKeyPressed(window, (Key)evt.keysym.keycode, ctrl, alt, shift, repeat);
        }

        /// <summary>
        /// Handles SDL2's KEYUP event.
        /// </summary>
        private void OnKeyUp(ref SDL_KeyboardEvent evt)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);

            states[(int)evt.keysym.scancode].OnUp();

            OnButtonReleased(window, (Scancode)evt.keysym.scancode);
            OnKeyReleased(window, (Key)evt.keysym.keycode);
        }

        /// <summary>
        /// Handles SDL2's TEXTEDITING event.
        /// </summary>
        private unsafe void OnTextEditing(ref SDL_TextEditingEvent evt)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            fixed (byte* input = evt.text)
            {
                if (ConvertTextInputToUtf16(input))
                {
                    OnTextEditing(window);
                }
            }
        }

        /// <summary>
        /// Handles SDL2's TEXTINPUT event.
        /// </summary>
        private unsafe void OnTextInput(ref SDL_TextInputEvent evt)
        {
            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            fixed (byte* input = evt.text)
            {
                if (ConvertTextInputToUtf16(input))
                {
                    OnTextInput(window);
                }
            }
        }

        /// <summary>
        /// Converts inputted text (which is in UTF-8 format) to UTF-16.
        /// </summary>
        /// <param name="input">A pointer to the inputted text.</param>
        /// <returns><see langword="true"/> if the input data was successfully converted; otherwise, <see langword="false"/>.</returns>
        private unsafe Boolean ConvertTextInputToUtf16(byte* input)
        {
            // Count the number of bytes in the UTF-8 text.
            var p = input;
            var byteCount = 0;
            while (*p++ != 0)
            {
                byteCount++;
            }

            if (byteCount == 0)
                return false;

            // Convert the UTF-8 characters to C#'s expected UTF-16 characters.
            var bytesUsed = 0;
            var charsUsed = 0;
            var completed = false;
            fixed (char* pTextUtf16 = textUtf16)
            {
                Encoding.UTF8.GetDecoder().Convert(input, byteCount, pTextUtf16, textUtf16.Length, true,
                    out bytesUsed, out charsUsed, out completed);
                if (!completed)
                {
                    return false;
                }
            }
            textInputLength = charsUsed;
            return true;
        }

        /// <summary>
        /// Flags the device as registered.
        /// </summary>
        private void Register()
        {
            var input = (SDL2UltravioletInput)Ultraviolet.GetInput();
            if (input.RegisterKeyboardDevice(this))
                isRegistered = true;
        }

        // State values.
        private readonly InternalButtonState[] states;
        private readonly Char[] textUtf16 = new Char[32];
        private Int32 textInputLength;
        private Boolean isRegistered;
    }
}
