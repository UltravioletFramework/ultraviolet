using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a particular instance of a style being applied to formatted text.
    /// </summary>
    internal struct TextStyleInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextStyleInstance"/> structure.
        /// </summary>
        /// <param name="style">The style that was applied.</param>
        /// <param name="bold">A value indicating whether the font was bold before the style was applied.</param>
        /// <param name="italic">A value indicating whether the font was italic before the style was applied.</param>
        public TextStyleInstance(TextStyle style, Boolean bold, Boolean italic)
        {
            this.style = style;
            this.bold = bold;
            this.italic = italic;
        }

        /// <summary>
        /// Gets the style that was applied.
        /// </summary>
        public TextStyle Style
        {
            get { return style; }
        }

        /// <summary>
        /// Gets a value indicating whether the font was bold before the style was applied.
        /// </summary>
        public Boolean Bold
        {
            get { return bold; }
        }

        /// <summary>
        /// Gets a value indicating whether the font was italic before the style was applied.
        /// </summary>
        public Boolean Italic
        {
            get { return italic; }
        }

        // Property values.
        private readonly TextStyle style;
        private readonly Boolean bold;
        private readonly Boolean italic;
    }
}
