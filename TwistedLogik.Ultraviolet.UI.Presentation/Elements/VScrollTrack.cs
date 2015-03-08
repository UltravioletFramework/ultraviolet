using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents the track for a vertical scroll bar.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.VScrollTrack.xml")]
    public class VScrollTrack : ScrollTrackBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VScrollTrack"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public VScrollTrack(UltravioletContext uv, String id)
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
        /// Handles the <see cref="Mouse.PreviewMouseMove"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbPreviewMouseMove(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            var button = element as Button;
            if (button != null && button.IsDepressed)
            {
                var relY = y - (AbsolutePosition.Y + thumbDragOffset);
                Value = OffsetToValue(relY, RenderSize.Height, Thumb.RenderSize.Height);
            }
        }

        /// <summary>
        /// Handles the <see cref="Mouse.PreviewMouseDown"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbPreviewMouseDown(UIElement element, MouseDevice device, MouseButton pressed, ref Boolean handled)
        {
            thumbDragOffset = Display.PixelsToDips(device.Y) - element.AbsoluteBounds.Y;
        }

        /// <summary>
        /// Updates the size of the track's thumb.
        /// </summary>
        /// <param name="availableSize">The amount of space available to the track.</param>
        private void UpdateThumbSize(Size2D availableSize)
        {
            if (Thumb == null)
                return;

            Thumb.Height = CalculateThumbWidth(availableSize.Height, Thumb.MinHeight);
        }

        /// <summary>
        /// Updates the offset of the track's thumb.
        /// </summary>
        /// <param name="availableSize">The amount of space available to the track.</param>
        private void UpdateThumbOffset(Size2D availableSize)
        {
            if (Thumb == null || UpLarge == null)
                return;

            UpLarge.Height = CalculateThumbOffset(availableSize.Height, Thumb.RenderSize.Height);
        }

        // Component element references.
        private readonly ButtonBase Thumb = null;
        private readonly ButtonBase UpLarge = null;

        // State values.
        private Double thumbDragOffset;
    }
}
