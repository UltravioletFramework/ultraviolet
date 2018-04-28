using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the collection of <see cref="GlyphShader"/> objects associated with a <see cref="TextStyle"/>.
    /// </summary>
    public sealed partial class TextStyleGlyphShaderCollection : IEnumerable<GlyphShader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextStyleGlyphShaderCollection"/> class.
        /// </summary>
        /// <param name="glyphShaders"></param>
        internal TextStyleGlyphShaderCollection(IEnumerable<GlyphShader> glyphShaders)
        {
            storage = (glyphShaders == null) ? new List<GlyphShader>() : new List<GlyphShader>(glyphShaders);
        }

        /// <summary>
        /// Gets the glyph shader at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the collection item to retrieve.</param>
        /// <returns>The glyph shader at the specified index within the collection.</returns>
        public GlyphShader this[Int32 index]
        {
            get { return storage[index]; }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return storage.Count; }
        }

        // The collection's underlying storage.
        private readonly List<GlyphShader> storage;
    }
}
