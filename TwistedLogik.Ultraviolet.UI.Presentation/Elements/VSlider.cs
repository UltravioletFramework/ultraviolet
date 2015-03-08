using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a vertical slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.VSlider.xml")]
    public class VSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public VSlider(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
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
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftLarge button.
        /// </summary>
        private void HandleClickUpLarge(UIElement element)
        {
            DecreaseLarge();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightLarge button.
        /// </summary>
        private void HandleClickDownLarge(UIElement small)
        {
            IncreaseLarge();
        }

        /// <summary>
        /// Updates the offset of the slider's thumb.
        /// </summary>
        /// <param name="availableSize">The amount of space available to the slider.</param>
        private void UpdateThumbOffset(Size2D availableSize)
        {
            if (Thumb == null || UpLarge == null)
                return;

            UpLarge.Height = CalculateThumbOffset(availableSize.Height, Thumb.RenderSize.Height);
        }
        
        // Control component references.
        private readonly RepeatButton UpLarge = null;
        private readonly Button Thumb = null;

        // State values.
        private Double thumbDragOffset;
    }
}
