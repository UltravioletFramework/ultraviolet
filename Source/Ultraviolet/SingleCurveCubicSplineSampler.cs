using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs cubic spline sampling on a curve of <see cref="Single"/> values.
    /// </summary>
    public class SingleCurveCubicSplineSampler : ICurveSampler<Single, CubicSplineCurveKey<Single>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleCurveCubicSplineSampler"/> class.
        /// </summary>
        private SingleCurveCubicSplineSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out Single value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Single value) { }

        /// <inheritdoc/>
        public Single InterpolateKeyframes(CubicSplineCurveKey<Single> key1, CubicSplineCurveKey<Single> key2, Single t, Single offset, in Single existing)
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

            return offset + (Single)(key1Value * polynomial1 + tangentOut * polynomial2 + key2Value * polynomial3 + tangentIn * polynomial4);
        }

        /// <inheritdoc/>
        public Single CalculateLinearExtension(CubicSplineCurveKey<Single> key, Single position, CurvePositionType positionType, in Single existing)
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
        public Single CalculateCycleOffset(Single first, Single last, Int32 cycle, in Single existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="SingleCurveCubicSplineSampler"/> class.
        /// </summary>
        public static SingleCurveCubicSplineSampler Instance { get; } = new SingleCurveCubicSplineSampler();
    }
}
