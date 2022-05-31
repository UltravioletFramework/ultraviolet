namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the set of text layout directions which are recognized by the Ultraviolet layout engine.
    /// </summary>
    public enum TextDirection
    {
        /// <summary>
        /// An invalid layout direction.
        /// </summary>
        Invalid,

        /// <summary>
        /// Text is written left to right.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Text is written right to left.
        /// </summary>
        RightToLeft,

        /// <summary>
        /// Text is written top to bottom.
        /// </summary>
        TopToBottom,

        /// <summary>
        /// Text is written bottom to top.
        /// </summary>
        BottomToTop,
    }
}
