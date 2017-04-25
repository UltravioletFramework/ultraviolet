
namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the Ultraviolet Presentation Foundation's supported mouse capture behaviors.
    /// </summary>
    public enum CaptureMode
    {
        /// <summary>
        /// The mouse is not captured.
        /// </summary>
        None,

        /// <summary>
        /// The mouse is captured by a single element.
        /// </summary>
        Element,

        /// <summary>
        /// The mouse is captured by a subtree of elements. If the element under the mouse is within the subtree,
        /// that element receives mouse input events. Otherwise, the subtree root receives the events.
        /// </summary>
        SubTree,
    }
}
