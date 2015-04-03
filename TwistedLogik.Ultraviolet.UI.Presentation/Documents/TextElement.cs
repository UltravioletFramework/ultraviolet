using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// NYI
    /// </summary>
    public abstract class TextElement
    {
        /// <summary>
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        [Styled("font")]
        public static readonly DependencyProperty FontProperty = DependencyProperty.Register("Font", typeof(SourcedResource<SpriteFont>), typeof(TextElement),
            new PropertyMetadata<SourcedResource<SpriteFont>>(null, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="FontColor"/> dependency property.
        /// </summary>
        [Styled("font-color")]
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register("FontColor", typeof(Color), typeof(TextElement),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>
        [Styled("font-style")]
        public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(SpriteFontStyle), typeof(TextElement),
           new PropertyMetadata<SpriteFontStyle>(UltravioletBoxedValues.SpriteFontStyle.Regular, PropertyMetadataOptions.AffectsMeasure));
    }
}
