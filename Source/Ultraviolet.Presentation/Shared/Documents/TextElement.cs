using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation.Documents
{
    /// <summary>
    /// Represents an element which draws text.
    /// </summary>
    [UvmlKnownType]
    public abstract class TextElement : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextElement"/> class.
        /// </summary>
        /// <param name="uv"></param>
        /// <param name="name"></param>
        public TextElement(UltravioletContext uv, String name)
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
        public SourcedResource<UltravioletFont> Font
        {
            get { return GetValue<SourcedResource<UltravioletFont>>(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font style which is used to draw the element's text.
        /// </summary>
        /// <value>A <see cref="UltravioletFontStyle"/> value that represents the style which is used
        /// to draw the element's text. The default value is <see cref="UltravioletFontStyle.Regular"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="FontStyleProperty"/></dpropField>
        ///		<dpropStylingName>font-style</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public UltravioletFontStyle FontStyle
        {
            get { return GetValue<UltravioletFontStyle>(FontStyleProperty); }
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
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Font"/> dependency property.</value>
        public static readonly DependencyProperty FontProperty = DependencyProperty.RegisterAttached("Font", typeof(SourcedResource<UltravioletFont>), typeof(TextElement),
            new PropertyMetadata<SourcedResource<UltravioletFont>>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.Inherits, HandleFontChanged));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FontStyle"/> dependency property.</value>
        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.RegisterAttached("FontStyle", typeof(UltravioletFontStyle), typeof(TextElement),
           new PropertyMetadata<UltravioletFontStyle>(UltravioletBoxedValues.SpriteFontStyle.Regular, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Background"/> dependency property.</value>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White, PropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Foreground"/> dependency property.</value>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black, PropertyMetadataOptions.Inherits));

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadFont();

            base.ReloadContentOverride(recursive);
        }

        /// <summary>
        /// Reloads the <see cref="Font"/> resource.
        /// </summary>
        protected void ReloadFont()
        {
            LoadResource(Font);
        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="TextElement.Font"/> dependency property changes.
        /// </summary>
        private static void HandleFontChanged(DependencyObject dobj, SourcedResource<UltravioletFont> oldValue, SourcedResource<UltravioletFont> newValue)
        {
            var textElement = dobj as TextElement;
            if (textElement != null)
                textElement.ReloadFont();
        }
    }
}
