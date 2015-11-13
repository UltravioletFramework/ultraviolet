using System;
using TwistedLogik.Nucleus.Text;

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
            SourceText = StringSegment.Empty;
            base.Clear();
        }

        /// <summary>
        /// Gets the source text that was parsed.
        /// </summary>
        public StringSegment SourceText
        {
            get;
            internal set;
        }
    }
}
