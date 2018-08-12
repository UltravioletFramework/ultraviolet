using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation.Documents
{
    /// <summary>
    /// Represents an element which draws text.
    /// </summary>
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
        /// Gets or sets a value indicating whether to perform text shaping on the element's text.
        /// </summary>
        /// <value><see langword="true"/> to perform text shaping; otherwise, <see langword="false"/>.
        /// The default value is <see langword="false"/>.</value>
        public Boolean TextIsShaped
        {
            get { return GetValue<Boolean>(TextIsShapedProperty); }
            set { SetValue(TextIsShapedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction in which the element's text is laid out.
        /// </summary>
        /// <value>The <see cref="Graphics.Graphics2D.Text.TextScript"/> value that specifies the type of script used to draw the element's text.
        /// The default value is <see cref="TextScript.Latin"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="TextScriptProperty"/></dpropField>
        ///		<dpropStylingName>text-script</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public TextScript TextScript
        {
            get { return GetValue<TextScript>(TextScriptProperty); }
            set { SetValue(TextScriptProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ISO 639 language code which specifies the language of the element's text.
        /// </summary>
        /// <value>The ISO 639 language code which specifies the language of the element's text.
        /// The default value is "en" for English.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="TextLanguageProperty"/></dpropField>
        ///		<dpropStylingName>text-script</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String TextLanguage
        {
            get { return GetValue<String>(TextLanguageProperty); }
            set { SetValue(TextLanguageProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Font"/> dependency property.</value>
        public static readonly DependencyProperty FontProperty = DependencyProperty.RegisterAttached("Font", typeof(SourcedResource<UltravioletFont>), typeof(TextElement),
            new PropertyMetadata<SourcedResource<UltravioletFont>>(null, PropertyMetadataOptions.AffectsMeasure, HandleFontChanged));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FontStyle"/> dependency property.</value>
        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.RegisterAttached("FontStyle", typeof(UltravioletFontStyle), typeof(TextElement),
           new PropertyMetadata<UltravioletFontStyle>(UltravioletBoxedValues.SpriteFontStyle.Regular, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Background"/> dependency property.</value>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Foreground"/> dependency property.</value>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black));

        /// <summary>
        /// Identifies the <see cref="TextIsShaped"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TextIsShaped"/> dependency property.</value>
        public static readonly DependencyProperty TextIsShapedProperty = DependencyProperty.RegisterAttached("TextIsShaped", typeof(Boolean), typeof(TextElement),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="TextScript"/> text property.
        /// </summary>
        /// <value>The identifier for the <see cref="TextScript"/> dependency property.</value>
        public static readonly DependencyProperty TextScriptProperty = DependencyProperty.RegisterAttached("TextScript", typeof(TextScript), typeof(TextElement),
            new PropertyMetadata<String>(TextScript.Latin, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="TextLanguage"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="TextLanguage"/> dependency property.</value>
        public static readonly DependencyProperty TextLanguageProperty = DependencyProperty.RegisterAttached("TextLanguage", typeof(String), typeof(TextElement),
            new PropertyMetadata<String>("en", PropertyMetadataOptions.None));

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
