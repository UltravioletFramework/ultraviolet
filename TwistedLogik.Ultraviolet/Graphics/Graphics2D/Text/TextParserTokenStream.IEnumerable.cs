using System.Collections;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextParserTokenStream : IEnumerable<TextParserToken>
    {
        /// <summary>
        /// Gets an enumerator for the result.
        /// </summary>
        /// <returns>An enumerator for the result.</returns>
        public List<TextParserToken>.Enumerator GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<TextParserToken> IEnumerable<TextParserToken>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
