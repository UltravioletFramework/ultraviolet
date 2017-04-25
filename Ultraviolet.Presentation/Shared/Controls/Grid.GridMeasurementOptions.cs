using System;

namespace Ultraviolet.Presentation.Controls
{
    partial class Grid
    {
        /// <summary>
        /// Represents the options used to measure various parts of the grid during its measurement phase.
        /// </summary>
        [Flags]
        private enum GridMeasurementOptions
        {
            /// <summary>
            /// No special options.
            /// </summary>
            None = 0,

            /// <summary>
            /// When measuring children, assuming that infinite width is available.
            /// </summary>
            AssumeInfiniteWidth = 1,

            /// <summary>
            /// When measuring children, assume that infinite height is available.
            /// </summary>
            AssumeInfiniteHeight = 2,

            /// <summary>
            /// When measuring children, don't include the child's desired width in
            /// calculations of the virtual cell's content width.
            /// </summary>
            DiscardDesiredWidth = 4,

            /// <summary>
            /// When measuring children, don't include the child's desired height in
            /// calculations of the virtual cell's content height.
            /// </summary>
            DiscardDesiredHeight=  8,
        }
    }
}
