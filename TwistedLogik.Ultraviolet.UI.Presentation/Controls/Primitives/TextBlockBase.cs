using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for text blocks.
    /// </summary>
    [UvmlKnownType]
    public abstract class TextBlockBase : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlockBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBlockBase(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the label's content.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty); }
            set { SetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the label's content.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalContentAlignmentProperty); }
            set { SetValue<VerticalAlignment>(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment"/> property changes.
        /// </summary>
        public event UpfEventHandler HorizontalContentAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment"/> property changes.
        /// </summary>
        public event UpfEventHandler VerticalContentAlignmentChanged;

        /// <summary>
        /// Identifies the <see cref="HorizontalContentAlignment"/> dependency property.
        /// </summary>
        [Styled("content-halign")]
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(TextBlockBase),
            new PropertyMetadata(PresentationBoxedValues.HorizontalAlignment.Left, PropertyMetadataOptions.AffectsArrange, HandleHorizontalContentAlignmentChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        [Styled("content-valign")]
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(TextBlockBase),
            new PropertyMetadata(PresentationBoxedValues.VerticalAlignment.Top, PropertyMetadataOptions.AffectsArrange, HandleVerticalContentAlignmentChanged));

        /// <summary>
        /// Raises the <see cref="HorizontalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalContentAlignmentChanged()
        {
            var temp = HorizontalContentAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VerticalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalContentAlignmentChanged()
        {
            var temp = VerticalContentAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleHorizontalContentAlignmentChanged(DependencyObject dobj)
        {
            var label = (TextBlockBase)dobj;
            label.OnHorizontalContentAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleVerticalContentAlignmentChanged(DependencyObject dobj)
        {
            var label = (TextBlockBase)dobj;
            label.OnVerticalContentAlignmentChanged();
        }
    }
}
