
namespace Ultraviolet.Presentation.Controls.Primitives
{
    partial class Popup
    {
        /// <summary>
        /// Represents the points in a popup target to which the popup can align itself.
        /// </summary>
        private enum PopupAlignmentPoint
        {
            /// <summary>
            /// The top-left corner of an alignment area.
            /// </summary>
            TopLeft,

            /// <summary>
            /// The center of the top edge of an alignment area.
            /// </summary>
            TopCenter,

            /// <summary>
            /// The top-right corner of an alignment area.
            /// </summary>
            TopRight,

            /// <summary>
            /// The middle of the left edge of an alignment area.
            /// </summary>
            MiddleLeft,

            /// <summary>
            /// The center of an alignment area.
            /// </summary>
            Center,

            /// <summary>
            /// The middle of the right edge of an alignment area.
            /// </summary>
            MiddleRight,

            /// <summary>
            /// The bottom-left corner of an alignment area.
            /// </summary>
            BottomLeft,

            /// <summary>
            /// The center of the bottom edge of an alignment area.
            /// </summary>
            BottomCenter,

            /// <summary>
            /// The bottom-right corner of an alignment area.
            /// </summary>
            BottomRight,
        }
    }
}
