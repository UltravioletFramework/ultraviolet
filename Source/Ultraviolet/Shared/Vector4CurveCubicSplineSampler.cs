using System;
using System.Runtime.CompilerServices;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs cubic spline sampling on a curve of <see cref="Vector4"/> values.
    /// </summary>
    public class Vector4CurveCubicSplineSampler : ICurveSampler<Vector4, CubicSplineCurveKey<Vector4>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4CurveCubicSplineSampler"/> class.
        /// </summary>
        private Vector4CurveCubicSplineSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Vector4 value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Vector4 value) { }

        /// <inheritdoc/>
        public Vector4 InterpolateKeyframes(CubicSplineCurveKey<Vector4> key1, CubicSplineCurveKey<Vector4> key2, Single t, Vector4 offset, in Vector4 existing)
        {
            var t2 = t * t;
            var t3 = t2 * t;
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            var tangentIn = key2.TangentIn;
            var tangentOut = key1.TangentOut;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1.0); // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + t);         // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);      // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                   // (t^3 - t^2)

            return offset + (key1Value * (Single)polynomial1 + tangentOut * (Single)polynomial2 + key2Value * (Single)polynomial3 + tangentIn * polynomial4);
        }

        /// <inheritdoc/>
        public Vector4 CalculateLinearExtension(CubicSplineCurveKey<Vector4> key, Single position, CurvePositionType positionType, in Vector4 existing)
        {
            switch (positionType)
            {
                case CurvePositionType.BeforeCurve:
                    return key.Value - key.TangentIn * (key.Position - position);

                case CurvePositionType.AfterCurve:
                    return key.Value - key.TangentOut * (key.Position - position);

                default:
                    return key.Value;
            }
        }

        /// <inheritdoc/>
        public Vector4 CalculateCycleOffset(Vector4 first, Vector4 last, Int32 cycle, in Vector4 existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="Vector4CurveCubicSplineSampler"/> class.
        /// </summary>
        public static Vector4CurveCubicSplineSampler Instance { get; } = new Vector4CurveCubicSplineSampler();
    }
}
