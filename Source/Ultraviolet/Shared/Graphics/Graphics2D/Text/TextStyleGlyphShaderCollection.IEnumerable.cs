using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextStyleGlyphShaderCollection : IEnumerable<GlyphShader>
    {
        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public List<GlyphShader>.Enumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<GlyphShader> IEnumerable<GlyphShader>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
