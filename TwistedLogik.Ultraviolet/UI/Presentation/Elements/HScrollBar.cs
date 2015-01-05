using System;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a button on a user interface.
    /// </summary>
    [UIElement("HScrollBar")]
    public class HScrollBar : RangeBase
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
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public HScrollBar(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            SetDefaultValue<Double>(MinimumProperty, 0.0);
            SetDefaultValue<Double>(MaximumProperty, 100.0);
            SetDefaultValue<Double>(SmallChangeProperty, 1.0);
            SetDefaultValue<Double>(LargeChangeProperty, 10.0);

            if (ComponentTemplate != null)
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseMotion"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseMotion(UIElement element, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            var button = element as Button;
            if (button == null)
                return;

            if (button.Depressed)
            {
                var relX = x - (AbsoluteScreenX + LayoutRoot.ColumnDefinitions[0].ActualWidth + thumbOffset);
                var relY = y - AbsoluteScreenY;

                var value    = PixelsToValue(relX);
                
                Value = value; 
                UpdateComponentLayout();
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.MouseButtonPressed"/> event for the Thumb button.
        /// </summary>
        private void HandleThumbMouseButtonPressed(UIElement element, MouseDevice device, MouseButton pressed)
        {
            if (Thumb == null)
                return;

            thumbOffset = device.X - Thumb.AbsoluteScreenX;
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftSmall button.
        /// </summary>
        private void HandleClickLeftSmall(UIElement element)
        {
            DecreaseSmall();
            UpdateComponentLayout();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the LeftLarge button.
        /// </summary>
        private void HandleClickLeftLarge(UIElement element)
        {
            DecreaseLarge();
            UpdateComponentLayout();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightSmall button.
        /// </summary>
        private void HandleClickRightSmall(UIElement small)
        {
            IncreaseSmall();
            UpdateComponentLayout();
        }

        /// <summary>
        /// Handles the <see cref="ButtonBase.Click"/> event for the RightLarge button.
        /// </summary>
        private void HandleClickRightLarge(UIElement small)
        {
            IncreaseLarge();
            UpdateComponentLayout();
        }

        /// <summary>
        /// Updates the layout of the scroll bar's components.
        /// </summary>
        private void UpdateComponentLayout()
        {
            if (LayoutRoot == null || LayoutRoot.ColumnDefinitions.Count < 5)
                return;

            var available = ActualWidth - (
                LayoutRoot.ColumnDefinitions[0].ActualWidth + 
                LayoutRoot.ColumnDefinitions[2].ActualWidth +
                LayoutRoot.ColumnDefinitions[4].ActualWidth);

            var val = Value;
            var min = Minimum;
            var max = Maximum;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent = (val - min) / (max - min);
            var used    = display.PixelsToDips(available * percent);

            LayoutRoot.ColumnDefinitions[1].Width = new GridLength(used);
        }

        /// <summary>
        /// Converts a range value to a pixel offset.
        /// </summary>
        /// <param name="value">The range value to convert.</param>
        /// <returns>The converted pixel value.</returns>
        private Double ValueToPixels(Double value)
        {
            var available = ActualWidth - (
                LayoutRoot.ColumnDefinitions[0].ActualWidth + 
                LayoutRoot.ColumnDefinitions[2].ActualWidth +
                LayoutRoot.ColumnDefinitions[4].ActualWidth);

            var min = Minimum;
            var max = Maximum;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent = (value - min) / (max - min);
            var used    = display.PixelsToDips(available * percent);

            return used;
        }

        /// <summary>
        /// Converts a pixel offset to a range value.
        /// </summary>
        /// <param name="pixels">The pixel value to convert.</param>
        /// <returns>The converted range value.</returns>
        private Double PixelsToValue(Double pixels)
        {
            var available = ActualWidth - (
                LayoutRoot.ColumnDefinitions[0].ActualWidth + 
                LayoutRoot.ColumnDefinitions[2].ActualWidth +
                LayoutRoot.ColumnDefinitions[4].ActualWidth);

            var min = Minimum;
            var max = Maximum;

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var percent = pixels / available;
            var value   = (percent * (Maximum - Minimum)) + Minimum;

            return value;
        }

        // Control component references.
        private readonly Grid LayoutRoot = null;
        private readonly Button Thumb = null;

        // State values.
        private Int32 thumbOffset;
    }
}
