using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal slider.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HSlider.xml")]
    public class HSlider : SliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSlider"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public HSlider(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override void PositionSliderComponents()
        {
            var thumbLength = (Thumb == null) ? 0 : Thumb.RenderSize.Width;
            var thumbOffset = CalculateThumbOffset(thumbLength);

            if (Thumb != null)
                Thumb.Width = thumbLength;

            if (LeftLarge != null)
                LeftLarge.Width = thumbOffset;
        }

        /// <summary>
        /// Gets the width of the scroll bar's track in pixels.
        /// </summary>
        protected override Double ActualTrackWidth
        {
            get { return RenderSize.Width; }
        }

        /// <summary>
        /// Gets the height of the scroll bar's track in pixels.
        /// </summary>
        protected override Double ActualTrackHeight
        {
            get { return RenderSize.Height; }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackLength
        {
            get { return RenderSize.Width; }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected override Double ActualThumbLength
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 3)
                    return 0;

                return LayoutRoot.ColumnDefinitions[1].ActualWidth;
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseMove"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseMove(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            var button = element as Button;
            if (button != null && button.IsDepressed)
            {
                var relX = x - (AbsolutePosition.X + thumbDragOffset);
                Value = OffsetToValue(relX);

                handled = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="Mouse.MouseDown"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseDown(UIElement element, MouseDevice device, MouseButton pressed, ref Boolean handled)
        {
            thumbDragOffset = Display.PixelsToDips(device.X) - element.AbsoluteBounds.X;
            handled = true;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftLarge button.
        /// </summary>
        private void HandleClickLeftLarge(UIElement element)
        {
            DecreaseLarge();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightLarge button.
        /// </summary>
        private void HandleClickRightLarge(UIElement small)
        {
            IncreaseLarge();
        }

        // Control component references.
        private readonly Grid LayoutRoot = null;
        private readonly RepeatButton LeftLarge = null;
        private readonly Button Thumb = null;

        // State values.
        private Double thumbDragOffset;
    }
}
