using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs linear sampling on a curve of <see cref="Quaternion"/> values.
    /// </summary>
    public class QuaternionCurveLinearSampler : ICurveSampler<Quaternion, CurveKey<Quaternion>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionCurveLinearSampler"/> class.
        /// </summary>
        private QuaternionCurveLinearSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Quaternion value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Quaternion value) { }

        /// <inheritdoc/>
        public Quaternion InterpolateKeyframes(CurveKey<Quaternion> key1, CurveKey<Quaternion> key2, Single t, Quaternion offset, in Quaternion existing)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;

            Quaternion.Slerp(ref key1Value, ref key2Value, t, out var result);
            return offset + result;
        }

        /// <inheritdoc/>
        public Quaternion CalculateLinearExtension(CurveKey<Quaternion> key, Single position, CurvePositionType positionType, in Quaternion existing) =>
            key.Value;

        /// <inheritdoc/>
        public Quaternion CalculateCycleOffset(Quaternion first, Quaternion last, Int32 cycle, in Quaternion existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="QuaternionCurveLinearSampler"/> class.
        /// </summary>
        public static QuaternionCurveLinearSampler Instance { get; } = new QuaternionCurveLinearSampler();
    }
}
