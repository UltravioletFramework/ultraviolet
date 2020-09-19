using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a mathematical curve.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    public abstract class Curve<TValue>
        where TValue : struct, IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Curve{TValue, TKey}"/> class.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        public Curve(CurveLoopType preLoop, CurveLoopType postLoop)
        {
            this.PreLoop = preLoop;
            this.PostLoop = postLoop;
        }

        /// <summary>
        /// Calculates the value of the curve at the specified position.
        /// </summary>
        /// <param name="position">The position at which to calculate a value.</param>
        /// <returns>The value of the curve at the specified position.</returns>
        public abstract TValue Evaluate(Single position);

        /// <summary>
        /// Gets a value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.
        /// </summary>
        public CurveLoopType PreLoop { get; }

        /// <summary>
        /// Gets a value indicating how the curve's values are determined
        /// for points after the end of the curve.
        /// </summary>
        public CurveLoopType PostLoop { get; }
    }
}
