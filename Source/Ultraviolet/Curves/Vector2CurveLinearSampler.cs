using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs linear sampling on a curve of <see cref="Vector2"/> values.
    /// </summary>
    public class Vector2CurveLinearSampler : ICurveSampler<Vector2, CurveKey<Vector2>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2CurveLinearSampler"/> class.
        /// </summary>
        private Vector2CurveLinearSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Vector2 value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Vector2 value) { }

        /// <inheritdoc/>
        public Vector2 InterpolateKeyframes(CurveKey<Vector2> key1, CurveKey<Vector2> key2, Single t, Vector2 offset, in Vector2 existing)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            return offset + (key1Value + ((key2Value - key1Value) * t));
        }

        /// <inheritdoc/>
        public Vector2 CalculateLinearExtension(CurveKey<Vector2> key, Single position, CurvePositionType positionType, in Vector2 existing) =>
            key.Value;

        /// <inheritdoc/>
        public Vector2 CalculateCycleOffset(Vector2 first, Vector2 last, Int32 cycle, in Vector2 existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector2CurveLinearSampler"/> class.
        /// </summary>
        public static Vector2CurveLinearSampler Instance { get; } = new Vector2CurveLinearSampler();
    }
}
