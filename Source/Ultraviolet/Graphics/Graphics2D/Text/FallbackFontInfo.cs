using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a set of metadata which associates a fallback font with a range of Unicode code points.
    /// </summary>
    public struct FallbackFontInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackFontInfo"/> structure.
        /// </summary>
        /// <param name="rangeStart">The first UTF-32 Unicode code point, inclusive, of the range which this font represents.</param>
        /// <param name="rangeEnd">The last UTF-32 Unicode code point, inclusive, of the range which this font represents.</param>
        /// <param name="font">The name of the font which is used to represent this range of code points.</param>
        public FallbackFontInfo(Int32 rangeStart, Int32 rangeEnd, StringSegment font)
        {
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.Font = font;
        }

        /// <summary>
        /// Gets the first UTF-32 Unicode code point, inclusive, of the range which this font represents.
        /// </summary>
        public Int32 RangeStart { get; }

        /// <summary>
        /// Gets the last UTF-32 Unicode code point, inclusive, of the range which this font represents.
        /// </summary>
        public Int32 RangeEnd { get; }

        /// <summary>
        /// Gets the font which is used to represent this range of code points.
        /// </summary>
        public StringSegment Font { get; }
    }
}
