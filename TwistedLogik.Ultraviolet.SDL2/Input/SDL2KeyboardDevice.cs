using System;
using System.Runtime.InteropServices;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the KeyboardDevice class.
    /// </summary>
    public sealed unsafe class SDL2KeyboardDevice : KeyboardDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the SDL2KeyboardDevice class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SDL2KeyboardDevice(UltravioletContext uv)
            : base(uv)
        {
            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        unsafe void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == SDL2UltravioletMessages.SDLEvent)
            {
                var evt = ((SDL2EventMessageData)data).Event;
                switch (evt.type)
                {
                    case SDL_EventType.KEYDOWN:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.key.windowID);
                            var mods = evt.key.keysym.mod;
                            var ctrl = (mods & SDL_Keymod.CTRL) != 0;
                            var alt = (mods & SDL_Keymod.ALT) != 0;
                            var shift = (mods & SDL_Keymod.SHIFT) != 0;
                            var repeat = evt.key.repeat > 0;
                            if (repeat)
                            {
                                repeats[(int)evt.key.keysym.scancode] = true;
                            }
                            else
                            {
                                OnButtonPressed(window, (Scancode)evt.key.keysym.scancode);
                            }
                            OnKeyPressed(window, (Key)evt.key.keysym.keycode, ctrl, alt, shift, repeat);
                        }
                        break;

                    case SDL_EventType.KEYUP:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.key.windowID);
                            OnButtonReleased(window, (Scancode)evt.key.keysym.scancode);
                            OnKeyReleased(window, (Key)evt.key.keysym.keycode);
                        }
                        break;

                    case SDL_EventType.TEXTINPUT:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.text.windowID);
                            if (ConvertTextInputToUtf16(evt.text.text))
                            {
                                OnTextInput(window);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the device's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public override void Update(UltravioletTime time)
        {
            var numkeys = 0;
            var state = SDL.GetKeyboardState(out numkeys);

            if (keyboardStateOld == null || keyboardStateNew == null || repeats == null)
            {
                keyboardStateOld = new byte[numkeys];
                keyboardStateNew = new byte[numkeys];
                repeats = new bool[numkeys];
            }

            var temp = keyboardStateNew;
            keyboardStateNew = keyboardStateOld;
            keyboardStateOld = temp;

            Marshal.Copy(state, keyboardStateNew, 0, numkeys);
            Array.Clear(repeats, 0, repeats.Length);
        }

        /// <summary>
        /// Populates the specified string builder with the most recent text input.
        /// </summary>
        /// <param name="sb">The string builder to populate with text input data.</param>
        /// <param name="append">A value indicating whether to append the text input data to the string builder's existing data.</param>
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

        /// <summary>
        /// Gets a value indicating whether the specified button is currently down.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is down; otherwise, false.</returns>
        public override Boolean IsButtonDown(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return keyboardStateNew[scancode] != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently up.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is up; otherwise, false.</returns>
        public override Boolean IsButtonUp(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return keyboardStateNew[scancode] == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently pressed.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>true if the button is pressed; otherwise, false.</returns>        
        public override Boolean IsButtonPressed(Scancode button, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return keyboardStateNew[scancode] != 0 && keyboardStateOld[scancode] == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently released.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is released; otherwise, false.</returns>
        public override Boolean IsButtonReleased(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)button;
            return keyboardStateNew[scancode] == 0 && keyboardStateOld[scancode] != 0;
        }

        /// <summary>
        /// Gets the current state of the specified button.
        /// </summary>
        /// <param name="button">The button for which to retrieve a state.</param>
        /// <returns>The current state of the specified button.</returns>
        public override ButtonState GetButtonState(Scancode button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var state = IsButtonDown(button) ? ButtonState.Down : ButtonState.Up;

            if (IsButtonPressed(button))
                state |= ButtonState.Pressed;

            if (IsButtonReleased(button))
                state |= ButtonState.Released;

            return state;
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is currently down.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <returns>true if the key is down; otherwise, false.</returns>
        public override Boolean IsKeyDown(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL.GetScancodeFromKey((SDL_Keycode)key);
            return keyboardStateNew[scancode] != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is currently up.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <returns>true if the key is up; otherwise, false.</returns>
        public override Boolean IsKeyUp(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL.GetScancodeFromKey((SDL_Keycode)key);
            return keyboardStateNew[scancode] == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is currently pressed.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>true if the key is pressed; otherwise, false.</returns>        
        public override Boolean IsKeyPressed(Key key, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL.GetScancodeFromKey((SDL_Keycode)key);
            return (!ignoreRepeats && repeats[scancode]) || (keyboardStateNew[scancode] != 0 && keyboardStateOld[scancode] == 0);
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is currently released.
        /// </summary>
        /// <param name="key">The key to evaluate.</param>
        /// <returns>true if the key is released; otherwise, false.</returns>
        public override Boolean IsKeyReleased(Key key)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var scancode = (int)SDL.GetScancodeFromKey((SDL_Keycode)key);
            return keyboardStateNew[scancode] == 0 && keyboardStateOld[scancode] != 0;
        }

        /// <summary>
        /// Gets the current state of the specified key.
        /// </summary>
        /// <param name="key">The key for which to retrieve a state.</param>
        /// <returns>The current state of the specified key.</returns>
        public override ButtonState GetKeyState(Key key)
        {
            var state = IsKeyDown(key) ? ButtonState.Down : ButtonState.Up;

            if (IsKeyPressed(key))
                state |= ButtonState.Pressed;

            if (IsKeyReleased(key))
                state |= ButtonState.Released;

            return state;
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
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
        /// Converts inputted text (which is in UTF-8 format) to UTF-16.
        /// </summary>
        /// <param name="input">A pointer to the inputted text.</param>
        /// <returns><c>true</c> if the input data was successfully converted; otherwise, <c>false</c>.</returns>
        private unsafe Boolean ConvertTextInputToUtf16(byte* input)
        {
            // Count the number of bytes in the UTF-8 text.
            var p = input;
            var byteCount = 0;
            while (*p++ != 0)
            {
                byteCount++;
            }

            // Convert the UTF-8 characters to C#'s expected UTF-16 characters.
            var bytesUsed = 0;
            var charsUsed = 0;
            var completed = false;
            fixed (char* pTextUtf16 = textUtf16)
            {
                Encoding.UTF8.GetDecoder().Convert((byte*)input, byteCount, pTextUtf16, textUtf16.Length, true,
                    out bytesUsed, out charsUsed, out completed);
                if (!completed)
                {
                    return false;
                }
            }
            textInputLength = charsUsed;
            return true;
        }

        // State values.
        private byte[] keyboardStateOld;
        private byte[] keyboardStateNew;
        private bool[] repeats;
        private char[] textUtf16 = new char[32];
        private Int32 textInputLength;
    }
}
