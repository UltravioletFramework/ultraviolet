using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents any element which directly renders text.
    /// </summary>
    public interface ITextHost
    {
        /// <summary>
        /// Gets or sets the font used to draw the element's text.
        /// </summary>
        SourcedResource<SpriteFont> Font
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color used to draw the element's text.
        /// </summary>
        Color FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style which is used to draw the element's text.
        /// </summary>
        SpriteFontStyle FontStyle
        {
            get;
            set;
        }
    }
}
