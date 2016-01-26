using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;

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
        public TextBlockBase(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the font used to draw the element's text.
        /// </summary>
		/// <value>A <see cref="SourcedResource{T}"/> value that represents the font used to draw
		/// the element's text. The default value is an invalid resource.</value>
		/// <remarks>
		/// <dprop>
		///		<dpropField><see cref="FontProperty"/></dpropField>
		///		<dpropStylingName>font</dpropStylingName>
		///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
		/// </dprop>
		/// </remarks>
        public SourcedResource<SpriteFont> Font
        {
            get { return GetValue<SourcedResource<SpriteFont>>(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font style which is used to draw the element's text.
        /// </summary>
		/// <value>A <see cref="SpriteFontStyle"/> value that represents the style which is used
		/// to draw the element's text. The default value is <see cref="SpriteFontStyle.Regular"/>.</value>
		/// <remarks>
		/// <dprop>
		///		<dpropField><see cref="FontStyleProperty"/></dpropField>
		///		<dpropStylingName>font-style</dpropStylingName>
		///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
		/// </dprop>
		/// </remarks>
        public SpriteFontStyle FontStyle
        {
            get { return GetValue<SpriteFontStyle>(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's foreground color.
        /// </summary>
		/// <value>The <see cref="Color"/> value that is used to draw the control's foreground elements,
		/// such as text. The default value is <see cref="Color.Black"/>.</value>
		/// <remarks>
		/// <dprop>
		///		<dpropField><see cref="ForegroundProperty"/></dpropField>
		///		<dpropStylingName>foreground</dpropStylingName>
		///		<dpropMetadata>None</dpropMetadata>
		/// </dprop>
		/// </remarks>
        public Color Foreground
        {
            get { return GetValue<Color>(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

		/// <summary>
		/// Gets or sets the element's background color.
		/// </summary>
		/// <value>The <see cref="Color"/> value that is used to draw the control's background elements.
		/// The default color is <see cref="Color.White"/>.</value>
		/// <remarks>
		/// <dprop>
		///		<dpropField><see cref="BackgroundProperty"/></dpropField>
		///		<dpropStylingName>background</dpropStylingName>
		///		<dpropMetadata>None</dpropMetadata>
		/// </dprop>
		/// </remarks>
		public Color Background
        {
            get { return GetValue<Color>(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
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
		/// Identifies the <see cref="Font"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Font"/> dependency property.</value>
		public static readonly DependencyProperty FontProperty = TextElement.FontProperty.AddOwner(typeof(TextBlockBase),
            new PropertyMetadata<SourcedResource<SpriteFont>>(null, PropertyMetadataOptions.AffectsArrange, HandleFontChanged));

		/// <summary>
		/// Identifies the <see cref="FontStyle"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="FontStyle"/> dependency property.</value>
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextBlockBase),
            new PropertyMetadata<SpriteFontStyle>(null, PropertyMetadataOptions.AffectsArrange));

		/// <summary>
		/// Identifies the <see cref="Foreground"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Foreground"/> dependency property.</value>
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(TextBlockBase));

		/// <summary>
		/// Identifies the <see cref="Background"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Background"/> dependency property.</value>
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(TextBlockBase));

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

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadFont();

            base.ReloadContentOverride(recursive);
        }

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
        /// Reloads the <see cref="Font"/> resource.
        /// </summary>
        protected void ReloadFont()
        {
            LoadResource(Font);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> dependency property changes.
        /// </summary>
        private static void HandleFontChanged(DependencyObject dobj, SourcedResource<SpriteFont> oldValue, SourcedResource<SpriteFont> newValue)
        {
            ((TextBlockBase)dobj).ReloadFont();
        }

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
