using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs step sampling on a curve of <see cref="Vector4"/> values.
    /// </summary>
    public class QuaternionCurveStepSampler : ICurveSampler<Quaternion, CurveKey<Quaternion>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionCurveStepSampler"/> class.
        /// </summary>
        private QuaternionCurveStepSampler() { }

        /// <inheritdoc/>
        public Quaternion InterpolateKeyframes(CurveKey<Quaternion> key1, CurveKey<Quaternion> key2, Single t, Quaternion offset) =>
            offset + (t >= 1 ? key2.Value : key1.Value);

        /// <inheritdoc/>
        public Quaternion CalculateLinearExtension(CurveKey<Quaternion> key, Single position, CurvePositionType positionType) =>
            key.Value;

        /// <inheritdoc/>
        public Quaternion CalculateCycleOffset(Quaternion first, Quaternion last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="QuaternionCurveStepSampler"/> class.
        /// </summary>
        public static QuaternionCurveStepSampler Instance { get; } = new QuaternionCurveStepSampler();
    }
}
