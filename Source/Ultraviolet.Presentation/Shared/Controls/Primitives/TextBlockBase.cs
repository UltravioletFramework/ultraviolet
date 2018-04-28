using System;
using Ultraviolet.Presentation.Documents;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for text blocks.
    /// </summary>
    [UvmlKnownType]
    public abstract class TextBlockBase : TextElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlockBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBlockBase(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the label's content.
        /// </summary>
        /// <value>A <see cref="HorizontalAlignment"/> value which specifies the horizontal alignment of
        /// the label's text. The default value is <see cref="HorizontalAlignment.Left"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="HorizontalContentAlignmentProperty"/></dpropField>
        ///		<dpropStylingName>content-halign</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue<HorizontalAlignment>(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the label's content.
        /// </summary>
        /// <value>A <see cref="VerticalAlignment"/> value which specifies the vertical alignment of
        /// the label's text. The default value is <see cref="VerticalAlignment.Top"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="VerticalContentAlignmentProperty"/></dpropField>
        ///		<dpropStylingName>content-valign</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue<VerticalAlignment>(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
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
        /// <value>The identifier for the <see cref="HorizontalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", "content-halign",
            typeof(HorizontalAlignment), typeof(TextBlockBase), new PropertyMetadata<HorizontalAlignment>(PresentationBoxedValues.HorizontalAlignment.Left, PropertyMetadataOptions.AffectsArrange, HandleHorizontalContentAlignmentChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalContentAlignment"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalContentAlignment"/> dependency property.</value>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", "content-valign",
            typeof(VerticalAlignment), typeof(TextBlockBase), new PropertyMetadata<VerticalAlignment>(PresentationBoxedValues.VerticalAlignment.Top, PropertyMetadataOptions.AffectsArrange, HandleVerticalContentAlignmentChanged));

        /// <summary>
        /// Raises the <see cref="HorizontalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalContentAlignmentChanged() =>
            HorizontalContentAlignmentChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="VerticalContentAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalContentAlignmentChanged() =>
            VerticalContentAlignmentChanged?.Invoke(this);

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalContentAlignment"/> dependency property changes.
        /// </summary>
        private static void HandleHorizontalContentAlignmentChanged(DependencyObject dobj, HorizontalAlignment oldValue, HorizontalAlignment newValue)
        {
            var label = (TextBlockBase)dobj;
            label.OnHorizontalContentAlignmentChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalContentAlignment"/> dependency property changes.
        /// </summary>
        private static void HandleVerticalContentAlignmentChanged(DependencyObject dobj, VerticalAlignment oldValue, VerticalAlignment newValue)
        {
            var label = (TextBlockBase)dobj;
            label.OnVerticalContentAlignmentChanged();
        }
    }
}
