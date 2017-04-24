using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains data relating to changes in a <see cref="Size2D"/> structure.
    /// </summary>
    public struct SizeChangedInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SizeChangedInfo"/> structure.
        /// </summary>
        /// <param name="previousSize">The size's previous value.</param>
        /// <param name="newSize">The size's new value.</param>
        public SizeChangedInfo(Size2D previousSize, Size2D newSize)
        {
            this.PreviousSize = previousSize;
            this.NewSize = newSize;
        }

        /// <summary>
        /// Gets the size's previous value.
        /// </summary>
        public Size2D PreviousSize { get; }

        /// <summary>
        /// Gets the size's new value.
        /// </summary>
        public Size2D NewSize { get; }

        /// <summary>
        /// Gets a value indicating whether the size's width has changed.
        /// </summary>
        public Boolean WidthChanged => !MathUtil.AreApproximatelyEqual(NewSize.Width, PreviousSize.Width);

        /// <summary>
        /// Gets a value indicating whether the size's height has changed.
        /// </summary>
        public Boolean HeightChanged => !MathUtil.AreApproximatelyEqual(NewSize.Height, PreviousSize.Height);
    }
}
