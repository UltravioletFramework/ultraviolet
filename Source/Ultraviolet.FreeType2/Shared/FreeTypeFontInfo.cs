using System;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the information required to load a FreeType font.
    /// </summary>
    public class FreeTypeFontInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeFontInfo"/> class.
        /// </summary>
        /// <param name="fontPath">The path to the font to load.</param>
        /// <param name="fontType">The type of font represented by the specified path.</param>
        public FreeTypeFontInfo(String fontPath, FreeTypeFontType fontType)
        {
            this.FontPath = fontPath;
            this.FontType = fontType;
        }

        /// <summary>
        /// Gets the path to the font to load.
        /// </summary>
        public String FontPath { get; }

        /// <summary>
        /// Gets the type of font represented by the specified path.
        /// </summary>
        public FreeTypeFontType FontType { get; }
    }
}
