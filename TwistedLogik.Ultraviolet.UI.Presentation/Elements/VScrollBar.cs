using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [UvmlKnownType("VScrollBar", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.VScrollBar.xml")]
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
        /// Handles the <see cref="ButtonBase.Click"/> event for the UpSmall button.
        /// </summary>
        private void HandleClickUpSmall(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the DownSmall button.
        /// </summary>
        private void HandleClickDownSmall(UIElement small)
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
        private readonly VScrollTrack Track = null;
    }
}
