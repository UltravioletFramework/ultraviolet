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
        /// Creates a temporary value for use in calculations. The value may be rented from a pool, as it will be released
        /// by a call to the <see cref="ReleaseTemporaryValue(in TValue)"/> method when it is no longer required.
        /// </summary>
        /// <param name="elementCount">The number of elements in each value in this curve.</param>
        /// <param name="value">The value which was created.</param>
        void CreateTemporaryValue(Int32 elementCount, out TValue value);

        /// <summary>
        /// Releases a temporary value created by the <see cref="CreateTemporaryValue(Int32, out TValue)"/> method that is no longer being used.
        /// </summary>
        /// <param name="value">The value to release.</param>
        void ReleaseTemporaryValue(in TValue value);

        /// <summary>
        /// Interpolates between the specified keyframes.
        /// </summary>
        /// <param name="key1">The first keyframe.</param>
        /// <param name="key2">The second keyframe.</param>.
        /// <param name="t">The interpolation factor.</param>
        /// <param name="offset">The current offset value.</param>
        /// <param name="existing">An existing value which may be modified to produce the result in order to reduce allocation pressure.</param>
        /// <returns>The interpolated value.</returns>
        TValue InterpolateKeyframes(TKey key1, TKey key2, Single t, TValue offset, in TValue existing);

        /// <summary>
        /// Calculates a linear extension of the specified keyframe.
        /// </summary>
        /// <param name="key">The key frame to extend.</param>
        /// <param name="position">The position to which to extend the curve.</param>
        /// <param name="positionType">A <see cref="CurvePositionType"/> value which categorizes the value of <paramref name="position"/>.</param>
        /// <param name="existing">An existing value which may be modified to produce the result in order to reduce allocation pressure.</param>
        /// <returns>The extended value.</returns>
        TValue CalculateLinearExtension(TKey key, Single position, CurvePositionType positionType, in TValue existing);

        /// <summary>
        /// Calculates the offset to apply to a curve's values when using <see cref="CurveLoopType.CycleOffset"/>.
        /// </summary>
        /// <param name="first">The first value in the curve.</param>
        /// <param name="last">The last value in the curve.</param>
        /// <param name="cycle">The cycle index.</param>
        /// <param name="existing">An existing value which may be modified to produce the result in order to reduce allocation pressure.</param>
        /// <returns>The offset value for the specified cycle.</returns>
        TValue CalculateCycleOffset(TValue first, TValue last, Int32 cycle, in TValue existing);
    }
}
