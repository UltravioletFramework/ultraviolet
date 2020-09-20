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
        public void CreateTemporaryValue(Int32 elementCount, out Single value) => value = default;

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in Single value) { }

        /// <inheritdoc/>
        public Single InterpolateKeyframes(CurveKey<Single> key1, CurveKey<Single> key2, Single t, Single offset, in Single existing)
        {
            var key1Value = key1.Value;
            var key2Value = key2.Value;
            return offset + (Single)(key1Value + ((key2Value - key1Value) * t));
        }

        /// <inheritdoc/>
        public Single CalculateLinearExtension(CurveKey<Single> key, Single position, CurvePositionType positionType, in Single existing) =>
            key.Value;

        /// <inheritdoc/>
        public Single CalculateCycleOffset(Single first, Single last, Int32 cycle, in Single existing) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="SingleCurveLinearSampler"/> class.
        /// </summary>
        public static SingleCurveLinearSampler Instance { get; } = new SingleCurveLinearSampler();
    }
}
