using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an item in a <see cref="ListBox"/> control.
    /// </summary>
    [UIElement("ListBoxItem", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ListBoxItem.xml")]
    public class ListBoxItem : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ListBoxItem(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<HorizontalAlignment>(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            SetDefaultValue<Thickness>(MarginProperty, Thickness.Parse("10"));
        }

        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            return base.ArrangeOverride(finalSize, options);
        }

        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            DrawImage(dc, View.Resources.BlankImage, Color.Red, true);
            base.DrawOverride(time, dc);
        }
    }
}
