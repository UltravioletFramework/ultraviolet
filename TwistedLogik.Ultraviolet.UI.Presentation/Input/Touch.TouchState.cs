using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    partial class Touch
    {
        /// <summary>
        /// Represents the touch state of the current Ultraviolet context.
        /// </summary>
        private class TouchState : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TouchState"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public TouchState(UltravioletContext uv)
                : base(uv)
            {

            }

            /// <summary>
            /// Gets the primary touch input device.
            /// </summary>
            public TouchDevice PrimaryDevice
            {
                get { return Ultraviolet.GetInput().GetTouchDevice(); }
            }
        }
    }
}
