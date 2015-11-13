using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the output from lexing formatted text.
    /// </summary>
    public sealed class TextLexerResult : TextResult<TextLexerToken>
    {
        /// <inheritdoc/>
        public override void Clear()
        {
            SourceText = StringSegment.Empty;
            base.Clear();
        }
        
        /// <summary>
        /// Gets the source text that was lexed.
        /// </summary>
        public StringSegment SourceText
        {
            get;
            internal set;
        }
    }
}
