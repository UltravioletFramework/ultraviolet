using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextRenderer
    {
        /// <summary>
        /// Represents the data required to draw text.
        /// </summary>
        private struct TextDrawState
        {
            /// <summary>
            /// Gets the source which contains the text being drawn.
            /// </summary>
            public StringSourceUnion Source;

            /// <summary>
            /// Gets the font with which text is rendered.
            /// </summary>
            public UltravioletFont Font;

            /// <summary>
            /// Gets the font face with which text is rendered.
            /// </summary>
            public UltravioletFontFace FontFace;

            /// <summary>
            /// Gets a value indicating whether the text is bold.
            /// </summary>
            public Boolean Bold;
            
            /// <summary>
            /// Gets a value indicating whether the text is italic.
            /// </summary>
            public Boolean Italic;

            /// <summary>
            /// The vertical offset of the block of text within its layout area.
            /// </summary>
            public Int32 BlockOffset;
        }
    }
}
