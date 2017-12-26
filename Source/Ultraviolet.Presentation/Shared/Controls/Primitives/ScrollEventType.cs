
namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the different events which can raise a <see cref="ScrollBar.Scroll"/> event.
    /// </summary>
    public enum ScrollEventType
    {
        /// <summary>
        /// The thumb was dragged to a new position and is no longer being dragged by the user.
        /// </summary>
        EndScroll,

        /// <summary>
        /// The thumb was moved to the minimum position.
        /// </summary>
        First,

        /// <summary>
        /// The thumb position was decremented by <see cref="RangeBase.LargeChange"/>.
        /// </summary>
        LargeDecrement,

        /// <summary>
        /// The thumb position was incremented by <see cref="RangeBase.LargeChange"/>.
        /// </summary>
        LargeIncrement,

        /// <summary>
        /// The thumb was moved to the maximum position.
        /// </summary>
        Last,

        /// <summary>
        /// The thumb position was decremented by <see cref="RangeBase.SmallChange"/>.
        /// </summary>
        SmallDecrement,

        /// <summary>
        /// The thumb position was incremented by <see cref="RangeBase.SmallChange"/>.
        /// </summary>
        SmallIncrement,

        /// <summary>
        /// The thumb was moved.
        /// </summary>
        ThumbPosition,

        /// <summary>
        /// The thumb position was changed because it was dragged by the user.
        /// </summary>
        ThumbTrack,
    }
}
