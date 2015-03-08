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
        protected override void PositionSliderComponents()
        {
            var thumbLength = (Thumb == null) ? 0 : Thumb.RenderSize.Height;
            var thumbOffset = CalculateThumbOffset(thumbLength);

            if (Thumb != null)
                Thumb.Height = thumbLength;

            if (UpLarge != null)
                UpLarge.Height = thumbOffset;
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
            get { return RenderSize.Height; }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected override Double ActualThumbLength
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.RowDefinitions.Count < 3)
                    return 0;

                return LayoutRoot.RowDefinitions[1].ActualHeight;
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseMotion"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseMotion(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy)
        {
            var button = element as Button;
            if (button != null && button.IsDepressed)
            {
                var relY = y - (AbsolutePosition.Y + thumbDragOffset);
                Value = OffsetToValue(relY);
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
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

        // Control component references.
        private readonly Grid LayoutRoot = null;
        private readonly RepeatButton UpLarge = null;
        private readonly Button Thumb = null;

        // State values.
        private Double thumbDragOffset;
    }
}
