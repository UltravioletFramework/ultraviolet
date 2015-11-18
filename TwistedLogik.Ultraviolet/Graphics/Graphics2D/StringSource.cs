using System;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a source of string data.
    /// </summary>
    internal struct StringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSource"/> structure.
        /// </summary>
        /// <param name="s">The <see cref="String"/> that contains the string data.</param>
        public StringSource(String s)
        {
            Contract.Require(s, "s");

            this.str     = s;
            this.builder = null;
            this.Start   = 0;
            this.Length  = s.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSource"/> structure.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> that contains the string data.</param>
        public StringSource(StringBuilder sb)
        {
            Contract.Require(sb, "sb");

            this.str     = null;
            this.builder = sb;
            this.Start   = 0;
            this.Length  = sb.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSource"/> structure.
        /// </summary>
        /// <param name="segment">The <see cref="StringSegment"/> that contains the string data.</param>
        public StringSource(StringSegment segment)
        {
            this.str     = segment.SourceString;
            this.builder = segment.SourceStringBuilder;
            this.Start   = segment.Start;
            this.Length  = segment.Length;
        }
        
        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents this string source.
        /// </summary>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        public StringSegment CreateStringSegment()
        {
            if (str != null)
                return new StringSegment(str, Start, Length);

            if (builder != null)
                return new StringSegment(builder, Start, Length);
            
            return StringSegment.Empty;
        }

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure that represents a substring of this string source.
        /// </summary>
        /// <param name="start">The index of the first character in the substring that will be represented by the string segment.</param>
        /// <param name="length">The length of the substring that will be represented by the string segment.</param>
        /// <returns>The <see cref="StringSegment"/> that was created.</returns>
        public StringSegment CreateStringSegmentFromSubstring(Int32 start, Int32 length)
        {
            if (str != null)
                return new StringSegment(str, Start + start, length);

            if (builder != null)
                return new StringSegment(str, Start + start, length);

            if (start != 0)
                throw new ArgumentOutOfRangeException("start");

            if (length != 0)
                throw new ArgumentOutOfRangeException("length");

            return StringSegment.Empty;
        }

        /// <summary>
        /// Creates a <see cref="StringSegment"/> structure with the same string source as this <see cref="StringSegment"/> but a different character range.
        /// </summary>
        /// <param name="start">The index of the first character in the created segment.</param>
        /// <param name="length">The number of characters in the created segment.</param>
        /// <returns>The <see cref="StringSegment"/> structure that was created.</returns>
        public StringSegment CreateStringSegmentFromSameSource(Int32 start, Int32 length)
        {
            if (str != null)
                return new StringSegment(str, start, length);

            if (builder != null)
                return new StringSegment(builder, start, length);

            return StringSegment.Empty;
        }

        /// <summary>
        /// Gets the character at the specified index within the string source.
        /// </summary>
        /// <param name="ix">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index within the string source.</returns>
        public Char this[Int32 ix]
        {
            get { return (str == null) ? builder[Start + ix] : str[Start + ix]; }
        }

        /// <summary>
        /// Gets the <see cref="String"/> that this source represents.
        /// </summary>
        public String String
        {
            get { return str; }
        }

        /// <summary>
        /// Gets the <see cref="StringBuilder"/> that this source represents.
        /// </summary>
        public StringBuilder StringBuilder
        {
            get { return builder; }
        }

        /// <summary>
        /// The starting index of the source substring.
        /// </summary>
        public readonly Int32 Start;

        /// <summary>
        /// The length of the source substring.
        /// </summary>
        public readonly Int32 Length;

        // State values.
        private readonly String str;
        private readonly StringBuilder builder;
    }
}
