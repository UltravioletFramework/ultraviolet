using System;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    partial class Popup
    {
        /// <summary>
        /// Represents the edges of the screen to which a popup can be aligned.
        /// </summary>
        [Flags]
        private enum PopupScreenEdges
        {
            /// <summary>
            /// The left edge of the screen.
            /// </summary>
            Left = 0x01,

            /// <summary>
            /// The top edge of the screen.
            /// </summary>
            Top = 0x02,

            /// <summary>
            /// The right edge of the screen.
            /// </summary>
            Right = 0x04,

            /// <summary>
            /// The bottom edge of the screen.
            /// </summary>
            Bottom = 0x08,

            /// <summary>
            /// All four of the screen's edges.
            /// </summary>
            All = Top | Left | Right | Bottom,
        }
    }
}
