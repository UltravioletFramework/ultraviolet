namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the types of parser tokens produced by <see cref="TextParser"/>.
    /// </summary>
    public enum TextParserTokenType : byte
    {
        /// <summary>
        /// The token contains text to be rendered.
        /// </summary>
        Text,

        /// <summary>
        /// The token contains an icon to be drawn.
        /// </summary>
        Icon,
        
        /// <summary>
        /// The token toggles whether the current font is bold.
        /// </summary>
        ToggleBold,

        /// <summary>
        /// The token toggles whether the current font is italic.
        /// </summary>
        ToggleItalic,

        /// <summary>
        /// The token pushes a font onto the font stack.
        /// </summary>
        PushFont,

        /// <summary>
        /// The token pushes a color onto the color stack.
        /// </summary>
        PushColor,

        /// <summary>
        /// The token pushes a style onto the style stack.
        /// </summary>
        PushStyle,

        /// <summary>
        /// The token pushes a glyph shader onto the glyph shader stack.
        /// </summary>
        PushGlyphShader,

        /// <summary>
        /// The token pops a font off of the font stack.
        /// </summary>
        PopFont,

        /// <summary>
        /// The token pops a color off of the color stack.
        /// </summary>
        PopColor,

        /// <summary>
        /// The token pops a style off of the style stack.
        /// </summary>
        PopStyle,

        /// <summary>
        /// The token pops a glyph shader off of the glyph shader stack.
        /// </summary>
        PopGlyphShader,

        /// <summary>
        /// The token pushes a link onto the link stack.
        /// </summary>
        PushLink,

        /// <summary>
        /// The token pops a link off of the link stack.
        /// </summary>
        PopLink,

        /// <summary>
        /// The token represents a custom command.
        /// </summary>
        Custom = 128,
    }
}
