using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a string source which can be broken into instances of the <see cref="ShapedStringSegment"/> structure.
    /// </summary>
    public interface ISegmentableShapedStringSource : IStringSource<ShapedChar>
    {
        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure that represents this string source.
        /// </summary>
        /// <returns>The <see cref="ShapedStringSegment"/> that was created.</returns>
        ShapedStringSegment CreateShapedStringSegment();

        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure that represents a substring of
        /// this string source.
        /// </summary>
        /// <param name="start">The index of the first character in the substring that will 
        /// be represented by the string segment.</param>
        /// <param name="length">The length of the substring that will be represented by 
        /// the string segment.</param>
        /// <returns>The <see cref="ShapedStringSegment"/> that was created.</returns>
        ShapedStringSegment CreateShapedStringSegmentFromSubstring(Int32 start, Int32 length);

        /// <summary>
        /// Creates a <see cref="ShapedStringSegment"/> structure with the same origin as this 
        /// string source but a different character range. This method only differs from
        /// the <see cref="CreateShapedStringSegmentFromSameOrigin(Int32, Int32)"/> method if this
        /// string source represents a substring of some other, larger string.
        /// </summary>
        /// <param name="start">The index of the first character in the created segment.</param>
        /// <param name="length">The number of characters in the created segment.</param>
        /// <returns>The <see cref="ShapedStringSegment"/> structure that was created.</returns>
        ShapedStringSegment CreateShapedStringSegmentFromSameOrigin(Int32 start, Int32 length);
    }
}
