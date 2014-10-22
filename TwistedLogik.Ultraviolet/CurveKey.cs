using System;
using System.Diagnostics;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a key point in a curve.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Position:{Position} Value:{Value} TangentIn:{TangentIn} TangentOut:{TangentOut} Continuity:{Continuity}\}")]
    public sealed class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
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
        /// Compares two curve keys for equality.
        /// </summary>
        /// <param name="key1">The first <see cref="CurveKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="CurveKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified curve keys are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(CurveKey key1, CurveKey key2)
        {
            if (key1 == null || key2 == null)
            {
                return key1 == key2;
            }
            return key1.Equals(key2);
        }

        /// <summary>
        /// Compares two curve keys for inequality.
        /// </summary>
        /// <param name="key1">The first <see cref="CurveKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="CurveKey"/> to compare.</param>
        /// <returns><c>true</c> if the specified curve keys are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(CurveKey key1, CurveKey key2)
        {
            if (key1 == null || key2 == null)
            {
                return key1 != key2;
            }
            return !key1.Equals(key2);
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + position.GetHashCode();
                hash = hash * 23 + value.GetHashCode();
                hash = hash * 23 + tangentIn.GetHashCode();
                hash = hash * 23 + tangentOut.GetHashCode();
                hash = hash * 23 + continuity.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format(provider, "{0}, {1}, {2}, {3}, {4}", position, value, tangentIn, tangentOut, Continuity);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return obj is CurveKey && Equals((CurveKey)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(CurveKey obj)
        {
            if (obj == null) 
                return false;

            return
                this.position   == obj.position &&
                this.value      == obj.value &&
                this.tangentIn  == obj.tangentIn &&
                this.tangentOut == obj.tangentOut &&
                this.continuity == obj.continuity;
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
            Contract.Require(other, "other");

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
