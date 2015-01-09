using System;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a horizontal scroll bar.
    /// </summary>
    [UIElement("HScrollBar")]
    public class HScrollBar : ScrollBarBase
    {
        /// <summary>
        /// Initializes the <see cref="HScrollBar"/> type.
        /// </summary>
        static HScrollBar()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(HScrollBar).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.HScrollBar.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public HScrollBar(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void UpdateComponentLayout()
        {
            if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                return;

            var thumbMinLength = (Thumb == null) ? 0 : Thumb.MinWidth;

            LayoutRoot.ColumnDefinitions[1].Width = CalculateThumbOffset();
            LayoutRoot.ColumnDefinitions[2].Width = CalculateThumbLength(thumbMinLength);
        }

        /// <summary>
        /// Gets the offset in pixels from the left edge of the control to the left edge of the scroll bar's track.
        /// </summary>
        protected override Int32 ActualTrackOffsetX
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.ColumnDefinitions[0].ActualWidth;
            }
        }

        /// <summary>
        /// Gets the offset in pixels from the top edge of the control to the top edge of the scroll bar's track.
        /// </summary>
        protected override Int32 ActualTrackOffsetY
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the width of the scroll bar's track in pixels.
        /// </summary>
        protected override Int32 ActualTrackWidth
        {
            get { return ActualTrackLength; }
        }

        /// <summary>
        /// Gets the height of the scroll bar's track in pixels.
        /// </summary>
        protected override Int32 ActualTrackHeight
        {
            get { return ActualHeight; }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's track.
        /// </summary>
        protected override Int32 ActualTrackLength
        {
            get 
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return ActualWidth - (
                    LayoutRoot.ColumnDefinitions[0].ActualWidth + 
                    LayoutRoot.ColumnDefinitions[4].ActualWidth);
            }
        }

        /// <summary>
        /// Gets the length in pixels of the scroll bar's thumb.
        /// </summary>
        protected override Int32 ActualThumbLength
        {
            get
            {
                if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                    return 0;

                return LayoutRoot.ColumnDefinitions[2].ActualWidth;
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseMotion"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseMotion(UIElement element, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            var button = element as Button;
            if (button != null && button.Depressed)
            {
                var relX = x - (AbsoluteScreenX + ActualTrackOffsetX + thumbOffset);
                Value = OffsetToValue(relX); 
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
        {
            thumbOffset = device.X - element.AbsoluteScreenX;
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
        private readonly Button Thumb = null;

        // State values.
        private Int32 thumbOffset;
    }
}
