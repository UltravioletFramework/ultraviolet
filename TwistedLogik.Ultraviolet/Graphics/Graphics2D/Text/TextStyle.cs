using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the style information for formatted text.
    /// </summary>
    public struct TextStyle
    {
        /// <summary>
        /// Gets or sets a value indicating what color the text should be rendered with.
        /// </summary>
        public Color? Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered in a bold style.
        /// </summary>
        public Boolean? Bold
        {
            get { return bold; }
            set { bold = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered in an italic style.
        /// </summary>
        public Boolean? Italic
        {
            get { return italic; }
            set { italic = value; }
        }

        /// <summary>
        /// Gets or sets the font with which this text should be rendered.
        /// </summary>
        public StringSegment? Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// Gets or sets the icon that should be rendered in place of this token.
        /// </summary>
        public StringSegment? Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// Gets or sets the name of the token's associated style preset.
        /// </summary>
        public StringSegment? Style
        {
            get { return style; }
            set { style = value; }
        }

        // Property values.
        private Color? color;
        private Boolean? bold;
        private Boolean? italic;
        private StringSegment? font;
        private StringSegment? icon;
        private StringSegment? style;
    }
}
