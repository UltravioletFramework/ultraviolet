using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HScrollBar.xml")]
    public class HScrollBar : ScrollBarBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public HScrollBar(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineLeftButton.
        /// </summary>
        private void HandleClickLineLeft(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for LineRightButton.
        /// </summary>
        private void HandleClickLineRight(UIElement small)
        {
            IncreaseSmall();
        }
    }
}
