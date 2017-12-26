
namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the placement of a <see cref="Popup"/> control.
    /// </summary>
    public enum PlacementMode
    {
        /// <summary>
        /// Positions the popup relative to the top-left corner of the screen, offset by the values
        /// of the <see cref="Popup.HorizontalOffset"/> and <see cref="Popup.VerticalOffset"/> properties.
        /// </summary>
        Absolute,

        /// <summary>
        /// Positions the popup relative to the top-left corner of the placement target, offset by the
        /// values of the <see cref="Popup.HorizontalOffset"/> and <see cref="Popup.VerticalOffset"/> properties.
        /// </summary>
        Relative,

        /// <summary>
        /// Positions the popup below its parent, aligning their left edges.
        /// </summary>
        Bottom,

        /// <summary>
        /// Centers the popup over its parent.
        /// </summary>
        Center,

        /// <summary>
        /// Positions the popup to the right of its parent, aligning their top edges.
        /// </summary>
        Right,

        /// <summary>
        /// Like <see cref="Absolute"/>, but if the popup would cross the edge of the screen, it flips
        /// to the other side of the placement point.
        /// </summary>
        AbsolutePoint,

        /// <summary>
        /// Like <see cref="Relative"/>, but if the popup would cross the edge of the screen, it flips
        /// to the other side of the placement point.
        /// </summary>
        RelativePoint,

        /// <summary>
        /// Like <see cref="Bottom"/>, but the popup is placed relative to the mouse cursor,
        /// rather than its parent control.
        /// </summary>
        Mouse,

        /// <summary>
        /// Like <see cref="RelativePoint"/>, but the popup is placed relative to the mouse cursor,
        /// rather than its parent control.
        /// </summary>
        MousePoint,

        /// <summary>
        /// Positions the popup to the left of its parent, aligning their top edges.
        /// </summary>
        Left,

        /// <summary>
        /// Positions the popup above its parent, aligning their left edges.
        /// </summary>
        Top,
    }
}
