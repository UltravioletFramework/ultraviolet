using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents an immutable string composed of shaped characters.
    /// </summary>
    public sealed partial class ShapedString : ISegmentableShapedStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedString"/> class.
        /// </summary>
        /// <param name="fontFace">The font face with which the string was created.</param>
        /// <param name="language">The name of the language which this string contains.</param>
        /// <param name="script">A <see cref="TextScript"/> value specifying which script which this string contains.</param>
        /// <param name="direction">A <see cref="TextDirection"/> value specifying the direction in which this string should be written.</param>
        /// <param name="value">An array of <see cref="ShapedChar"/> values.</param>
        public ShapedString(UltravioletFontFace fontFace, String language, TextScript script, TextDirection direction, ShapedChar[] value)
        {
            Contract.Require(fontFace, nameof(fontFace));
            Contract.Require(language, nameof(language));
            Contract.Require(value, nameof(value));

            this.FontFace = fontFace;
            this.Language = language;
            this.Script = script;
            this.Direction = direction;
            this.buffer = CreateShapedString(value, 0, value?.Length ?? 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedString"/> class.
        /// </summary>
        /// <param name="fontFace">The font face with which the string was created.</param>
        /// <param name="language">The name of the language which this string contains.</param>
        /// <param name="script">A <see cref="TextScript"/> value specifying which script which this string contains.</param>
        /// <param name="direction">A <see cref="TextDirection"/> value specifying the direction in which this string should be written.</param>
        /// <param name="value">An array of <see cref="ShapedChar"/> values.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters within <paramref name="value"/> to use.</param>
        public ShapedString(UltravioletFontFace fontFace, String language, TextScript script, TextDirection direction, ShapedChar[] value, Int32 startIndex, Int32 count)
        {
            Contract.Require(fontFace, nameof(fontFace));
            Contract.Require(language, nameof(language));
            Contract.Require(value, nameof(value));

            this.FontFace = fontFace;
            this.Language = language;
            this.Script = script;
            this.Direction = direction;
            this.buffer = CreateShapedString(value, startIndex, count);
        }

        /// <summary>
        /// Indicates whether the specified string is <see langword="null"/> or an empty string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is <see langword="null"/>
        /// or an empty string; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsNullOrEmpty(ShapedString value) =>
            (value == null || value.buffer.Length == 0);

        /// <summary>
        /// Creates a new instance of <see cref="ShapedString"/> with the same value as a specified <see cref="ShapedString"/>.
        /// </summary>
        /// <param name="str">The string to copy.</param>
        /// <returns>A new string with the same value as <paramref name="str"/>.</returns>
        public static ShapedString Copy(ShapedString str)
        {
            Contract.Require(str, nameof(str));

            return new ShapedString(str.FontFace, str.Language, str.Script, str.Direction, str.buffer);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character
        /// position and continues to the end of the string.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>A shaped string that is equivalent to the substring that begins at <paramref name="startIndex"/> 
        /// in this instance.</returns>
        public ShapedString Substring(Int32 startIndex) =>
            Substring(startIndex, Length - startIndex);

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character
        /// position and has a specified length.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A shaped string that is equivalent to the substring of length <paramref name="length"/> that begins
        /// at <paramref name="startIndex"/> in this instance.</returns>
        public ShapedString Substring(Int32 startIndex, Int32 length)
        {
            Contract.EnsureRange(startIndex >= 0 && startIndex <= Length, nameof(startIndex));
            Contract.EnsureRange(length > 0 && startIndex + length <= Length, nameof(length));

            return new ShapedString(FontFace, Language, Script, Direction, buffer, startIndex, length);
        }

        /// <summary>
        /// Copies a specified number of shaped characters from a specified position in this instance to a
        /// specified position in an array of shaped characters.
        /// </summary>
        /// <param name="sourceIndex">The index of the first character in this instance to copy.</param>
        /// <param name="destination">An array of shaped characters to which characters in this instance are copied.</param>
        /// <param name="destinationIndex">The index in <paramref name="destination"/> at which the copy operation begins.</param>
        /// <param name="count">The number of shaped characters in this instance to copy to <paramref name="destination"/>.</param>
        public void CopyTo(Int32 sourceIndex, ShapedChar[] destination, Int32 destinationIndex, Int32 count) =>
            Array.Copy(buffer, sourceIndex, destination, destinationIndex, count);

        /// <summary>
        /// Gets the <see cref="ShapedChar"/> object at the specified position 
        /// in the current <see cref="ShapedString"/> object.
        /// </summary>
        /// <param name="index">A position in the current string.</param>
        /// <returns>The object at position <paramref name="index"/>.</returns>
        [IndexerName("Chars")]
        public ShapedChar this[Int32 index] => buffer[index];

        /// <summary>
        /// Gets the font face with which the string was created.
        /// </summary>
        public UltravioletFontFace FontFace { get; }

        /// <summary>
        /// Gets the name of the language which this string contains.
        /// </summary>
        public String Language { get; }

        /// <summary>
        /// Gets a <see cref="TextScript"/> value specifying the script which this string contains.
        /// </summary>
        public TextScript Script { get; }

        /// <summary>
        /// Gets a <see cref="TextDirection"/> value specifying the direction in which this string should be written.
        /// </summary>
        public TextDirection Direction { get; }

        /// <summary>
        /// Gets the length of the string in characters.
        /// </summary>
        public Int32 Length => buffer.Length;

        /// <summary>
        /// Creates the internal buffer for a <see cref="ShapedString"/> instance based on the specified input parameters.
        /// </summary>
        private static ShapedChar[] CreateShapedString(ShapedChar[] value, Int32 startIndex, Int32 count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (startIndex < 0 || startIndex >= value.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (count < 0 || startIndex + count > value.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            var buffer = new ShapedChar[count];
            Array.Copy(value, startIndex, buffer, 0, count);
            return buffer;
        }

        // The buffer that contains the string data.
        private readonly ShapedChar[] buffer;
    }
}
