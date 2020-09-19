using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs step sampling on a curve of <see cref="Single"/> values.
    /// </summary>
    public class SingleCurveStepSampler : ICurveSampler<Single, CurveKey<Single>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleCurveStepSampler"/> class.
        /// </summary>
        private SingleCurveStepSampler() { }

        /// <inheritdoc/>
        public Single InterpolateKeyframes(CurveKey<Single> key1, CurveKey<Single> key2, Single t, Single offset) =>
            offset + (t >= 1 ? key2.Value : key1.Value);

        /// <inheritdoc/>
        public Single CalculateLinearExtension(CurveKey<Single> key, Single position, CurvePositionType positionType) =>
            key.Value;

        /// <inheritdoc/>
        public Single CalculateCycleOffset(Single first, Single last, Int32 cycle) => 
            (last - first) * cycle;

        /// <summary>
        /// Gets the singleton instance of the <see cref="SingleCurveStepSampler"/> class.
        /// </summary>
        public static SingleCurveStepSampler Instance { get; } = new SingleCurveStepSampler();
    }
}
