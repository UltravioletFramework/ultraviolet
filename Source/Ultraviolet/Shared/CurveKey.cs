using System;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a key point in a curve.
    /// </summary>
    [Serializable]
    public sealed partial class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        /// <param name="position">The key's position on the curve.</param>
        /// <param name="value">The key's value.</param>
        /// <param name="tangentIn">The value of the tangent when approaching this key from the previous key.</param>
        /// <param name="tangentOut">The value of the tangent when approaching this key from the next key.</param>
        /// <param name="continuity">A value describing the continuity between this key and the next.</param>
        public CurveKey(Single position, Single value, Single tangentIn, Single tangentOut, CurveContinuity continuity)
        {
            this.position = position;
            this.value = value;
            this.tangentIn = tangentIn;
            this.tangentOut = tangentOut;
            this.continuity = continuity;
        }
        
        /// <summary>
        /// Compares this instance to the specified <see cref="CurveKey"/> and returns an integer that indicates whether the position
        /// of this instance is less than, equal to, or greater than the value of the specified key.
        /// </summary>
        /// <param name="other">The key to compare to this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <remarks>If the keys have the same position, this method returns zero.  If this key comes before <paramref name="other"/>,
        /// then this method returns -1.  Otherwise, this method returns 1.</remarks>
        public Int32 CompareTo(CurveKey other)
        {
            Contract.Require(other, nameof(other));

            if (this.position == other.position) return 0;
            if (this.position <  other.position) return -1;
            return 1;
        }

        /// <summary>
        /// Gets the key's position on the curve.
        /// </summary>
        public Single Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the key's value.
        /// </summary>
        public Single Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the value of the tangent when approaching this key from the previous key.
        /// </summary>
        public Single TangentIn
        {
            get { return tangentIn; }
        }

        /// <summary>
        /// Gets the value of the tangent when approaching this key from the next key.
        /// </summary>
        public Single TangentOut
        {
            get { return tangentOut; }
        }

        /// <summary>
        /// Gets a value describing the continuity between this key and the next.
        /// </summary>
        public CurveContinuity Continuity
        {
            get { return continuity; }
        }

        // Property values.
        private readonly Single position;
        private readonly Single value;
        private readonly Single tangentIn;
        private readonly Single tangentOut;
        private readonly CurveContinuity continuity;
    }
}
