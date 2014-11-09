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
