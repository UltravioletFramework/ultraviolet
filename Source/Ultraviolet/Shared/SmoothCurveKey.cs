using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a key point in a curve which uses smooth (bicubic) sampling.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    public partial class SmoothCurveKey<TValue> : CurveKey<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmoothCurveKey{TValue}"/> class.
        /// </summary>
        /// <param name="position">The key's position on the curve.</param>
        /// <param name="value">The key's value.</param>
        /// <param name="tangentIn">The value of the tangent when approaching this key from the previous key.</param>
        /// <param name="tangentOut">The value of the tangent when approaching this key from the next key.</param>
        public SmoothCurveKey(Single position, TValue value, TValue tangentIn, TValue tangentOut)
            : base(position, value)
        {
            this.TangentIn = tangentIn;
            this.TangentOut = tangentOut;
        }

        /// <summary>
        /// Gets the value of the tangent when approaching this key from the previous key.
        /// </summary>
        public TValue TangentIn { get; }

        /// <summary>
        /// Gets the value of the tangent when approaching this key from the next key.
        /// </summary>
        public TValue TangentOut { get; }
    }
}
