using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a vertical scroll bar.
    /// </summary>
    [UIElement("VScrollBar", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.VScrollBar.xml")]
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

        /// <inheritdoc/>
        protected override void PositionScrollBarComponents()
        {
            if (LayoutRoot == null || LayoutRoot.RowDefinitions.Count < 5)
                return;

            var thumbMinLength = (Thumb == null) ? 0 : Thumb.MinHeight;

            if (UpLarge != null)
                UpLarge.Height = CalculateThumbOffset();

            if (Thumb != null)
                Thumb.Height = CalculateThumbLength(thumbMinLength);
        }

        /// <summary>
        /// Gets the offset in pixels from the left edge of the control to the left edge of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackOffsetX
        {
            get 
            { 
                return 0; 
            }
        }

        /// <summary>
        /// Gets the offset in pixels from the top edge of the control to the top edge of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackOffsetY
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.RowDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.RowDefinitions[0].ActualHeight; 
            }
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
            get { return ActualTrackLength; }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackLength
        {
            get 
            {
                if (LayoutRoot == null || LayoutRoot.RowDefinitions.Count < 5)
                    return 0;

                return RenderSize.Height - (
                    LayoutRoot.RowDefinitions[0].ActualHeight + 
                    LayoutRoot.RowDefinitions[4].ActualHeight);
            }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected override Double ActualThumbLength
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.RowDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.RowDefinitions[2].ActualHeight;
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
                var relY = y - (AbsolutePosition.Y + ActualTrackOffsetY + thumbOffset);
                Value = OffsetToValue(relY); 
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
        {
            thumbOffset = Display.PixelsToDips(device.Y) - element.AbsolutePosition.Y;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the UpSmall button.
        /// </summary>
        private void HandleClickUpSmall(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the UpLarge button.
        /// </summary>
        private void HandleClickUpLarge(UIElement element)
        {
            DecreaseLarge();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the DownSmall button.
        /// </summary>
        private void HandleClickDownSmall(UIElement small)
        {
            IncreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the DownLarge button.
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
        private Double thumbOffset;
    }
}
