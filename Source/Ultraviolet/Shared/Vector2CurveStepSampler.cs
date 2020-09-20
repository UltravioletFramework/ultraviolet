using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs step sampling on a curve of <see cref="Vector2"/> values.
    /// </summary>
    public class Vector2CurveStepSampler : ICurveSampler<Vector2, CurveKey<Vector2>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2CurveStepSampler"/> class.
        /// </summary>
        private Vector2CurveStepSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Vector2 value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Vector2 value) { }

        /// <inheritdoc/>
        public Vector2 InterpolateKeyframes(CurveKey<Vector2> key1, CurveKey<Vector2> key2, Single t, Vector2 offset, in Vector2 existing) =>
            offset + (t >= 1 ? key2.Value : key1.Value);

        /// <inheritdoc/>
        public Vector2 CalculateLinearExtension(CurveKey<Vector2> key, Single position, CurvePositionType positionType, in Vector2 existing) =>
            key.Value;

        /// <inheritdoc/>
        public Vector2 CalculateCycleOffset(Vector2 first, Vector2 last, Int32 cycle, in Vector2 existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector2CurveStepSampler"/> class.
        /// </summary>
        public static Vector2CurveStepSampler Instance { get; } = new Vector2CurveStepSampler();
    }
}
