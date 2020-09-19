using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs linear sampling on a curve of <see cref="Single"/> values.
    /// </summary>
    public class SingleCurveLinearSampler : ICurveSampler<Single, CurveKey<Single>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleCurveLinearSampler"/> class.
        /// </summary>
        private SingleCurveLinearSampler() { }

        /// <inheritdoc/>
        public Single InterpolateKeyframes(CurveKey<Single> key1, CurveKey<Single> key2, Single t, Single offset)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            return offset + (Single)(key1Value + ((key2Value - key1Value) * t));
        }

        /// <inheritdoc/>
        public Single CalculateLinearExtension(CurveKey<Single> key, Single position, CurvePositionType positionType) =>
            key.Value;

        /// <inheritdoc/>
        public Single CalculateCycleOffset(Single first, Single last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="SingleCurveLinearSampler"/> class.
        /// </summary>
        public static SingleCurveLinearSampler Instance { get; } = new SingleCurveLinearSampler();
    }
}
