namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the value which specifies the side of a <see cref="Slider"/> control on which to draw its tick marks.
    /// </summary>
    public enum TickPlacement
    {
        /// <summary>
        /// No ticks are shown.
        /// </summary>
        None,

        /// <summary>
        /// Ticks are shown above the track for horizontal sliders
        /// or to the left of the track for vertical sliders.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Ticks are shown below the track for horizontal sliders
        /// or to the right of the track for vertical sliders.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Ticks are shown on both sides of the track.
        /// </summary>
        Both,
    }
}
