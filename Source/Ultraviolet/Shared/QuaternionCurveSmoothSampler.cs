using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs smooth (bicubic) sampling on a curve of <see cref="Quaternion"/> values.
    /// </summary>
    public class QuaternionCurveSmoothSampler : ICurveSampler<Quaternion, SmoothCurveKey<Quaternion>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionCurveSmoothSampler"/> class.
        /// </summary>
        private QuaternionCurveSmoothSampler() { }

        /// <inheritdoc/>
        public Quaternion InterpolateKeyframes(SmoothCurveKey<Quaternion> key1, SmoothCurveKey<Quaternion> key2, Single t, Quaternion offset)
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
        public Quaternion CalculateLinearExtension(SmoothCurveKey<Quaternion> key, Single position, CurvePositionType positionType)
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
        public Quaternion CalculateCycleOffset(Quaternion first, Quaternion last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="QuaternionCurveSmoothSampler"/> class.
        /// </summary>
        public static QuaternionCurveSmoothSampler Instance { get; } = new QuaternionCurveSmoothSampler();
    }
}
