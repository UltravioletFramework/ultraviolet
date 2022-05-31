using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextParserTokenStream : IEnumerable<TextParserToken>
    {
        /// <summary>
        /// Gets an enumerator for the result.
        /// </summary>
        /// <returns>An enumerator for the result.</returns>
        public List<TextParserToken>.Enumerator GetEnumerator() => tokens.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator<TextParserToken> IEnumerable<TextParserToken>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
