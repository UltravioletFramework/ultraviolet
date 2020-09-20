using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a sampler which can interpolate between two successive keyframes of a curve.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    /// <typeparam name="TKey">The type of keyframe used by the curve.</typeparam>
    public interface ICurveSampler<TValue, in TKey>
        where TKey : CurveKey<TValue>
    {
        /// <summary>
        /// Interpolates between the specified keyframes.
        /// </summary>
        /// <param name="key1">The first keyframe.</param>
        /// <param name="key2">The second keyframe.</param>.
        /// <param name="t">The interpolation factor.</param>
        /// <param name="offset">The current offset value.</param>
        /// <returns>The interpolated value.</returns>
        TValue InterpolateKeyframes(TKey key1, TKey key2, Single t, TValue offset);

        /// <summary>
        /// Calculates a linear extension of the specified keyframe.
        /// </summary>
        /// <param name="key">The key frame to extend.</param>
        /// <param name="position">The position to which to extend the curve.</param>
        /// <param name="positionType">A <see cref="CurvePositionType"/> value which categorizes the value of <paramref name="position"/>.</param>
        /// <returns>The extended value.</returns>
        TValue CalculateLinearExtension(TKey key, Single position, CurvePositionType positionType);

        /// <summary>
        /// Calculates the offset to apply to a curve's values when using <see cref="CurveLoopType.CycleOffset"/>.
        /// </summary>
        /// <param name="first">The first value in the curve.</param>
        /// <param name="last">The last value in the curve.</param>
        /// <param name="cycle">The cycle index.</param>
        /// <returns>The offset value for the specified cycle.</returns>
        TValue CalculateCycleOffset(TValue first, TValue last, Int32 cycle);
    }
}
