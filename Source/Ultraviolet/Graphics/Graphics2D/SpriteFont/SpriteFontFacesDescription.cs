namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a sprite font's collection of faces used during content processing.
    /// </summary>
    internal sealed class SpriteFontFacesDescription
    {
        /// <summary>
        /// Gets or sets a description of the font's regular face.
        /// </summary>
        public SpriteFontFaceDescription Regular { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's bold face.
        /// </summary>
        public SpriteFontFaceDescription Bold { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's italic face.
        /// </summary>
        public SpriteFontFaceDescription Italic { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's bold-italic face.
        /// </summary>
        public SpriteFontFaceDescription BoldItalic { get; set; }
    }
}
