using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a wrapper around the <see cref="StringSegment"/> structure which is suitable for use as a key in a dictionary or hashtable.
    /// </summary>
    public partial struct StringSegmentKey : IEquatable<StringSegmentKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegmentKey"/> structure.
        /// </summary>
        /// <param name="stringSegment">The string segment which this key represents.</param>
        public StringSegmentKey(StringSegment stringSegment)
        {
            this.StringSegment = stringSegment;
            this.cachedHashCode = CalculateHashCode(stringSegment.Source, stringSegment.Start, stringSegment.Length);
        }

        /// <summary>
        /// Implicitly converts a <see cref="StringSegment"/> instance to a new instance of the <see cref="StringSegmentKey"/> structure.
        /// </summary>
        /// <param name="stringSegment">The <see cref="StringSegment"/> instance to convert.</param>
        public static implicit operator StringSegmentKey(StringSegment stringSegment) => new StringSegmentKey(stringSegment);

        /// <summary>
        /// Implicitly converts a <see cref="String"/> instance to a new instance of the <see cref="StringSegmentKey"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="String"/> instance to convert.</param>
        public static implicit operator StringSegmentKey(String value) => new StringSegmentKey(new StringSegment(value));

        /// <summary>
        /// Implicitly converts a <see cref="StringBuilder"/> instance to a new instance of the <see cref="StringSegmentKey"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="StringBuilder"/> instance to convert.</param>
        public static implicit operator StringSegmentKey(StringBuilder value) => new StringSegmentKey(new StringSegment(value));

        /// <summary>
        /// Gets the string segment that this key represents.
        /// </summary>
        public StringSegment StringSegment { get; }

        // The hash code of the source segment is cached when the key is constructed to avoid unnecessary recalculations,
        // and because the value of the hash could potentially change in the case of a StringBuilder instance.
        private readonly Int32 cachedHashCode;
    }
}
