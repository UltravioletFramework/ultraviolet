using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the result of parsing formatted text.
    /// </summary>
    public sealed class TextParserResult2 : TextResult<TextParserToken2>
    {
        /// <inheritdoc/>
        public override void Clear()
        {
            Source = null;
            base.Clear();
        }

        /// <summary>
        /// Gets or sets the lexer's source string.
        /// </summary>
        internal StringSource? Source
        {
            get;
            set;
        }
    }
}
