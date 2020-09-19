using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs linear sampling on a curve of <see cref="Vector4"/> values.
    /// </summary>
    public class Vector4CurveLinearSampler : ICurveSampler<Vector4, CurveKey<Vector4>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4CurveLinearSampler"/> class.
        /// </summary>
        private Vector4CurveLinearSampler() { }

        /// <inheritdoc/>
        public Vector4 InterpolateKeyframes(CurveKey<Vector4> key1, CurveKey<Vector4> key2, Single t, Vector4 offset)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            return offset + (key1Value + ((key2Value - key1Value) * t));
        }

        /// <inheritdoc/>
        public Vector4 CalculateLinearExtension(CurveKey<Vector4> key, Single position, CurvePositionType positionType) =>
            key.Value;

        /// <inheritdoc/>
        public Vector4 CalculateCycleOffset(Vector4 first, Vector4 last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector4CurveLinearSampler"/> class.
        /// </summary>
        public static Vector4CurveLinearSampler Instance { get; } = new Vector4CurveLinearSampler();
    }
}
