using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the time at which a button should raise its <see cref="ButtonBase.Click"/> event.
    /// </summary>
    public enum ClickMode
    {
        /// <summary>
        /// The event is raised when the button is released.
        /// </summary>
        Release,
        
        /// <summary>
        /// The event is raised when the button is pressed.
        /// </summary>
        Press,

        /// <summary>
        /// The event is raised then the cursor enters the button.
        /// </summary>
        Hover,
    }
}
