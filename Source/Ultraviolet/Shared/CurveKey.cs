using System;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a key point in a curve which uses linear sampling.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    public class CurveKey<TValue> : IComparable<CurveKey<TValue>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurveKey{TValue}"/> class.
        /// </summary>
        /// <param name="position">The key's position on the curve.</param>
        /// <param name="value">The key's value.</param>
        public CurveKey(Single position, TValue value)
        {
            this.Position = position;
            this.Value = value;
        }

        /// <summary>
        /// Compares this instance to the specified <see cref="CurveKey{TValue}"/> and returns an integer that indicates whether the position
        /// of this instance is less than, equal to, or greater than the value of the specified key.
        /// </summary>
        /// <param name="other">The key to compare to this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <remarks>If the keys have the same position, this method returns zero.  If this key comes before <paramref name="other"/>,
        /// then this method returns -1.  Otherwise, this method returns 1.</remarks>
        public Int32 CompareTo(CurveKey<TValue> other)
        {
            Contract.Require(other, nameof(other));

            if (this.Position == other.Position) return 0;
            if (this.Position <  other.Position) return -1;

            return 1;
        }

        /// <summary>
        /// Gets the key's position on the curve.
        /// </summary>
        public Single Position { get; }

        /// <summary>
        /// Gets the key's value.
        /// </summary>
        public TValue Value { get; }
    }
}
