using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs cubic spline sampling on a curve of <see cref="Quaternion"/> values.
    /// </summary>
    public class QuaternionCurveCubicSplineSampler : ICurveSampler<Quaternion, CubicSplineCurveKey<Quaternion>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionCurveCubicSplineSampler"/> class.
        /// </summary>
        private QuaternionCurveCubicSplineSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Quaternion value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Quaternion value) { }

        /// <inheritdoc/>
        public Quaternion InterpolateKeyframes(CubicSplineCurveKey<Quaternion> key1, CubicSplineCurveKey<Quaternion> key2, Single t, Quaternion offset, in Quaternion existing)
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
        public Quaternion CalculateLinearExtension(CubicSplineCurveKey<Quaternion> key, Single position, CurvePositionType positionType, in Quaternion existing)
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
        public Quaternion CalculateCycleOffset(Quaternion first, Quaternion last, Int32 cycle, in Quaternion existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="QuaternionCurveCubicSplineSampler"/> class.
        /// </summary>
        public static QuaternionCurveCubicSplineSampler Instance { get; } = new QuaternionCurveCubicSplineSampler();
    }
}
