using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a generic source of text characters.
    /// </summary>
    /// <typeparam name="TChar">The type of character which makes up this string.</typeparam>
    public interface IStringSource<TChar>
    {
        /// <summary>
        /// Gets the character at the specified index within the string.
        /// If the character cannot be retrieved, an exception is thrown.
        /// </summary>
        /// <param name="index">The index of the character to retrieve.</param>
        /// <param name="ch">The character at the specified index within the string.</param>
        void GetChar(Int32 index, out TChar ch);

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents this string source.
        /// </summary>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        StringSegment CreateStringSegment();

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents a substring of
        /// this string source.
        /// </summary>
        /// <param name="start">The index of the first character in the substring that will 
        /// be represented by the string segment.</param>
        /// <param name="length">The length of the substring that will be represented by 
        /// the string segment.</param>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        StringSegment CreateStringSegmentFromSubstring(Int32 start, Int32 length);

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure with the same origin as this 
        /// string source but a different character range. This method only differs from
        /// the <see cref="CreateStringSegmentFromSubstring(Int32, Int32)"/> method if this
        /// string source represents a substring of some other, larger string.
        /// </summary>
        /// <param name="start">The index of the first character in the created segment.</param>
        /// <param name="length">The number of characters in the created segment.</param>
        /// <returns>The <see cref="StringSegment"/> structure that was created.</returns>
        StringSegment CreateStringSegmentFromSameOrigin(Int32 start, Int32 length);

        /// <summary>
        /// Gets the character at the specified index within the string.
        /// </summary>
        /// <param name="index">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the string.</returns>
        TChar this[Int32 index] { get; }

        /// <summary>
        /// Gets the length of the string in characters.
        /// </summary>
        Int32 Length { get; }

        /// <summary>
        /// Gets a value indicating whether this string source represents a null object.
        /// </summary>
        Boolean IsNull { get; }

        /// <summary>
        /// Gets a value indicating whether this string source represents an empty string.
        /// </summary>
        Boolean IsEmpty { get; }
    }
}
