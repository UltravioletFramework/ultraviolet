using System;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a token produced by parsing formatted text.
    /// </summary>
    public struct TextParserToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextParserToken"/> structure.
        /// </summary>
        /// <param name="text">The token's text.</param>
        /// <param name="style">The token's style index.</param>
        internal TextParserToken(StringSegment text, Int32 style)
        {
            this.text = text;
            this.style = style;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return Text.ToString();
        }

        /// <summary>
        /// Gets the token's text.
        /// </summary>
        public StringSegment Text
        {
            get { return text; }
        }

        /// <summary>
        /// Gets the token's style index.
        /// </summary>
        public Int32 Style
        {
            get { return style; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a white space character.
        /// </summary>
        public Boolean IsWhiteSpace
        {
            get { return !text.IsEmpty && Char.IsWhiteSpace(text[0]); }
        }

        /// <summary>
        /// Gets a value indicating whether this is a new line character.
        /// </summary>
        public Boolean IsNewLine
        {
            get { return !text.IsEmpty && text[0] == '\n'; }
        }

        // Property values.
        private readonly StringSegment text;
        private readonly Int32 style;
    }
}