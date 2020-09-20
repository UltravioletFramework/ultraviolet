using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs step sampling on a curve of <see cref="Vector3"/> values.
    /// </summary>
    public class Vector3CurveStepSampler : ICurveSampler<Vector3, CurveKey<Vector3>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3CurveStepSampler"/> class.
        /// </summary>
        private Vector3CurveStepSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Vector3 value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Vector3 value) { }

        /// <inheritdoc/>
        public Vector3 InterpolateKeyframes(CurveKey<Vector3> key1, CurveKey<Vector3> key2, Single t, Vector3 offset, in Vector3 existing) =>
            offset + (t >= 1 ? key2.Value : key1.Value);

        /// <inheritdoc/>
        public Vector3 CalculateLinearExtension(CurveKey<Vector3> key, Single position, CurvePositionType positionType, in Vector3 existing) =>
            key.Value;

        /// <inheritdoc/>
        public Vector3 CalculateCycleOffset(Vector3 first, Vector3 last, Int32 cycle, in Vector3 existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector3CurveStepSampler"/> class.
        /// </summary>
        public static Vector3CurveStepSampler Instance { get; } = new Vector3CurveStepSampler();
    }
}
