using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs linear sampling on a curve of <see cref="Vector3"/> values.
    /// </summary>
    public class Vector3CurveLinearSampler : ICurveSampler<Vector3, CurveKey<Vector3>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3CurveLinearSampler"/> class.
        /// </summary>
        private Vector3CurveLinearSampler() { }

        /// <inheritdoc/>
        public Vector3 InterpolateKeyframes(CurveKey<Vector3> key1, CurveKey<Vector3> key2, Single t, Vector3 offset)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            return offset + (key1Value + ((key2Value - key1Value) * t));
        }

        /// <inheritdoc/>
        public Vector3 CalculateLinearExtension(CurveKey<Vector3> key, Single position, CurvePositionType positionType) =>
            key.Value;

        /// <inheritdoc/>
        public Vector3 CalculateCycleOffset(Vector3 first, Vector3 last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector3CurveLinearSampler"/> class.
        /// </summary>
        public static Vector3CurveLinearSampler Instance { get; } = new Vector3CurveLinearSampler();
    }
}
