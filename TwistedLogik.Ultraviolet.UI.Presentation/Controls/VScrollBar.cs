using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.VScrollBar.xml")]
    public class VScrollBar : ScrollBarBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public VScrollBar(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineUpButton.
        /// </summary>
        private void HandleClickLineUp(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineDownButton.
        /// </summary>
        private void HandleClickLineDown(UIElement small)
        {
            IncreaseSmall();
        }
    }
}
