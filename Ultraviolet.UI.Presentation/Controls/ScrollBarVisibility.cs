
namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a value indicating whether the scroll bars on a <see cref="ScrollViewer"/> are visible.
    /// </summary>
    public enum ScrollBarVisibility
    {
        /// <summary>
        /// The scroll bar does not appear under any circumstance, even if the control
        /// cannot display all of its content within the available content area,
        /// and the viewer's content cannot be scrolled.
        /// </summary>
        Disabled,

        /// <summary>
        /// The scroll bar appears only when the content being displayed is
        /// too large for the available content area.
        /// </summary>
        Auto,

        /// <summary>
        /// The scroll bar does not appear, even if the control
        /// cannot display all of its content within the available content area,
        /// but the viewer's content can be scrolled by other means.
        /// </summary>
        Hidden,

        /// <summary>
        /// The scroll bar always appears.
        /// </summary>
        Visible,
    }
}
