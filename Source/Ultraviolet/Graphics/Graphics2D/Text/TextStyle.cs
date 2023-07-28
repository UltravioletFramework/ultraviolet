using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a preset text style.
    /// </summary>
    public sealed class TextStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextStyle"/> class.
        /// </summary>
        /// <param name="font">The font which is applied by this style, if any.</param>
        /// <param name="bold">A value indicating whether this style makes the current font bold.</param>
        /// <param name="italic">A value indicating whether this style makes the current font italic.</param>
        /// <param name="color">The color which is applied by this style, if any.</param>
        /// <param name="glyphShaders">The glyph shaders which are applied by this style, if any.</param>
        public TextStyle(UltravioletFont font, Boolean? bold, Boolean? italic, Color? color, params GlyphShader[] glyphShaders)
        {
            this.Font = font;
            this.Bold = bold;
            this.Italic = italic;
            this.Color = color;
            this.GlyphShaders = new TextStyleGlyphShaderCollection(glyphShaders);            
        }

        /// <summary>
        /// Gets the font which is applied by this style, if any.
        /// </summary>
        public UltravioletFont Font
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this style makes the current font bold.
        /// </summary>
        public Boolean? Bold
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this style makes the current font italic.
        /// </summary>
        public Boolean? Italic
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the color which is applied by this style, if any.
        /// </summary>
        public Color? Color
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of glyph shaders which are applied by this style.
        /// </summary>
        public TextStyleGlyphShaderCollection GlyphShaders
        {
            get;
            private set;
        }
    }
}
