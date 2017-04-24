using System;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    partial class TextEditor
    {
        /// <summary>
        /// Used to track the state of pending scrolls in a <see cref="TextEditor"/> control.
        /// </summary>
        [Flags]
        private enum PendingScrollState : byte
        {
            /// <summary>
            /// The scroll has no associated state.
            /// </summary>
            None = 0x00,

            /// <summary>
            /// The scroll should jump to the left.
            /// </summary>
            JumpLeft = 0x01,

            /// <summary>
            /// The scroll should jump to the right.
            /// </summary>
            JumpRight = 0x02,

            /// <summary>
            /// The scroll should show the maximum width of the line.
            /// </summary>
            ShowMaximumLineWidth = 0x04,
        }

        /// <summary>
        /// Creates a <see cref="PendingScrollState"/> value that corresponds to the specified boolean values.
        /// </summary>
        private static PendingScrollState PendingScrollStateFromValues(Boolean showMaximumLineWidth, Boolean jumpLeft, Boolean jumpRight)
        {
            var value = PendingScrollState.None;

            if (showMaximumLineWidth)
                value |= PendingScrollState.ShowMaximumLineWidth;

            if (jumpLeft)
                value |= PendingScrollState.JumpLeft;

            if (jumpRight)
                value |= PendingScrollState.JumpRight;

            return value;
        }
    }
}
