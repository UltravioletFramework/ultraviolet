using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    partial class Mouse
    {
        /// <summary>
        /// Represents the mouse state of the current Ultraviolet context.
        /// </summary>
        private class MouseState : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MouseState"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public MouseState(UltravioletContext uv)
                : base(uv)
            {

            }

            /// <summary>
            /// Gets the primary mouse input device.
            /// </summary>
            public MouseDevice PrimaryDevice
            {
                get { return Ultraviolet.GetInput().GetMouse(); }
            }
        }
    }
}
