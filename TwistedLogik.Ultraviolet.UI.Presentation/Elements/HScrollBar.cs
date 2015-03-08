using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UvmlKnownType("HScrollBar", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HScrollBar.xml")]
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
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftSmall button.
        /// </summary>
        private void HandleClickLeftSmall(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightSmall button.
        /// </summary>
        private void HandleClickRightSmall(UIElement small)
        {
            IncreaseSmall();
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (Track != null)
            {
                Track.InvalidateMeasure();
            }
            return base.MeasureOverride(availableSize);
        }

        // Component references.
        private readonly HScrollTrack Track = null;
    }
}
