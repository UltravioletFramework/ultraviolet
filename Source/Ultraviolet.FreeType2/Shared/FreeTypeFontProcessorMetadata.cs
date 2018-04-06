using System;
using Newtonsoft.Json;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="FreeTypeFontProcessor"/> class.
    /// </summary>
    public sealed class FreeTypeFontProcessorMetadata
    {
        /// <summary>
        /// Gets a value which specifies whether to use the nearest matching pixel size if <see cref="SizeInPixels"/> is specified
        /// but the font face does not contain that pixel size.
        /// </summary>
        public Boolean UseClosestPixelSize { get; private set; } = false;

        /// <summary>
        /// Gets the size of the font in points. If <see cref="SizeInPixels"/> has a non-zero value, this value must be zero.
        /// </summary>
        public Int32 SizeInPoints { get; private set; } = 16;

        /// <summary>
        /// Gets the size of the font in pixels. If <see cref="SizeInPoints"/> has a non-zero value, this value must be zero.
        /// </summary>
        public Int32 SizeInPixels { get; private set; } = 0;

        /// <summary>
        /// Gets the font face's substitution character.
        /// </summary>
        public Char? Substitution { get; private set; } = null;

        /// <summary>
        /// Gets a string representing the list of glyphs which should be prepopulated on the font's texture atlas.
        /// </summary>
        public String PrepopulatedGlyphs { get; private set; } = "ASCII";
    }
}
