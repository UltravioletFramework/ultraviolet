using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the track for a horizontal scroll bar.
    /// </summary>
    [UIElement("HScrollTrack", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HScrollTrack.xml")]
    public class HScrollTrack : ScrollTrackBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollTrack"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public HScrollTrack(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateThumbSize(availableSize);
            UpdateThumbOffset(availableSize);

            return base.MeasureOverride(availableSize);
        }
        
        /// <summary>
        /// Handles the <see cref="UIElement.MouseMotion"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseMotion(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy)
        {
            var button = element as Button;
            if (button != null && button.IsDepressed)
            {
                var relX = x - (AbsolutePosition.X + thumbDragOffset);
                Value = OffsetToValue(relX, RenderSize.Width, Thumb.RenderSize.Width);
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
        {
            thumbDragOffset = Display.PixelsToDips(device.X) - element.AbsoluteBounds.X;
        }

        /// <summary>
        /// Updates the size of the track's thumb.
        /// </summary>
        /// <param name="availableSize">The amount of space available to the track.</param>
        private void UpdateThumbSize(Size2D availableSize)
        {
            if (Thumb == null)
                return;

            Thumb.Width = CalculateThumbWidth(availableSize.Width, Thumb.MinWidth);
        }

        /// <summary>
        /// Updates the offset of the track's thumb.
        /// </summary>
        /// <param name="availableSize">The amount of space available to the track.</param>
        private void UpdateThumbOffset(Size2D availableSize)
        {
            if (Thumb == null || LeftLarge == null)
                return;

            LeftLarge.Width = CalculateThumbOffset(availableSize.Width, Thumb.RenderSize.Width);
        }

        // Component element references.
        private readonly ButtonBase Thumb = null;
        private readonly ButtonBase LeftLarge = null;

        // State values.
        private Double thumbDragOffset;
    }
}
