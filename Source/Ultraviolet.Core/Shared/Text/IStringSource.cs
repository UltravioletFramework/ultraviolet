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
