
namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the display state of a framework element.
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Indicates that the element is displayed.
        /// </summary>
        Visible,

        /// <summary>
        /// Indicates that the element is not displayed, but still reserves space in the layout.
        /// </summary>
        Hidden,

        /// <summary>
        /// Indicates that the element is not displayed and does not reserve space in the layout.
        /// </summary>
        Collapsed,
    }
}
