using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    partial class Keyboard
    {
        /// <summary>
        /// Represents the keyboard state of the current Ultraviolet context.
        /// </summary>
        private class KeyboardState : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyboardState"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public KeyboardState(UltravioletContext uv)
                : base(uv)
            {

            }

            /// <summary>
            /// Gets the primary keyboard input device.
            /// </summary>
            public KeyboardDevice PrimaryDevice
            {
                get { return Ultraviolet.GetInput().GetKeyboard(); }
            }
        }
    }
}
