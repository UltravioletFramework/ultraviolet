using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// NYI
    /// </summary>
    public abstract class TextElement
    {
        /// <summary>
        /// Identifies the Font dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'font'.</remarks>
        public static readonly DependencyProperty FontProperty = DependencyProperty.RegisterAttached("Font", typeof(SourcedResource<SpriteFont>), typeof(TextElement),
            new PropertyMetadata<SourcedResource<SpriteFont>>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the FontStyle dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'font-style'.</remarks>
        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.RegisterAttached("FontStyle", typeof(SpriteFontStyle), typeof(TextElement),
           new PropertyMetadata<SpriteFontStyle>(UltravioletBoxedValues.SpriteFontStyle.Regular, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the Background dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'background'.</remarks>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the Foreground dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'foreground'.</remarks>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black));
    }
}
