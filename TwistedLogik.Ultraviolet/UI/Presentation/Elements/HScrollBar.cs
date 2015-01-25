using System;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UIElement("HScrollBar", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HScrollBar.xml")]
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

        /// <inheritdoc/>
        protected override void UpdateScrollbarComponents()
        {
            if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                return;

            var thumbMinLength = (Thumb == null) ? 0 : Thumb.MinWidth;

            if (LeftLarge != null)
                LeftLarge.Width = CalculateThumbOffset();

            if (Thumb != null)
                Thumb.Width = CalculateThumbLength(thumbMinLength);
        }

        /// <summary>
        /// Gets the offset in pixels from the left edge of the control to the left edge of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackOffsetX
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.ColumnDefinitions[0].MeasuredWidth;
            }
        }

        /// <summary>
        /// Gets the offset in pixels from the top edge of the control to the top edge of the scroll bar's track.
        /// </summary>
        protected override Double ActualTrackOffsetY
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the width of the scroll bar's track in pixels.
        /// </summary>
        protected override Double ActualTrackWidth
        {
            get { return ActualTrackLength; }
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
            get 
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return RenderSize.Width - (
                    LayoutRoot.ColumnDefinitions[0].MeasuredWidth + 
                    LayoutRoot.ColumnDefinitions[4].MeasuredWidth);
            }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected override Double ActualThumbLength
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.ColumnDefinitions[2].MeasuredWidth;
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
                var relX = x - (AbsolutePosition.X + ActualTrackOffsetX + thumbOffset);
                Value = OffsetToValue(relX); 
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
        {
            thumbOffset = Display.PixelsToDips(device.X) - element.AbsoluteBounds.X;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftSmall button.
        /// </summary>
        private void HandleClickLeftSmall(UIElement element)
        {
            DecreaseSmall();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftLarge button.
        /// </summary>
        private void HandleClickLeftLarge(UIElement element)
        {
            DecreaseLarge();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightSmall button.
        /// </summary>
        private void HandleClickRightSmall(UIElement small)
        {
            IncreaseSmall();
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
        private Double thumbOffset;
    }
}
