using System;

namespace Ultraviolet.FreeType2
{
    partial class FreeTypeFontProcessor
    {
        /// <summary>
        /// Represents a range of glyphs which need to be prepopulated on a font face's atlases.
        /// </summary>
        private struct PrepopulatedGlyphRange
        {
            /// <summary>
            /// The first glyph in the range to populate.
            /// </summary>
            public Char Start;

            /// <summary>
            /// The last glyph in the range to populate.
            /// </summary>
            public Char End;
        }
    }
}
