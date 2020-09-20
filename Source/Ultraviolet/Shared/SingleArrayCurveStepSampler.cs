using System;
using System.Buffers;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="ICurveSampler{TValue, TKey}"/> which performs step sampling on a curve of arrays of <see cref="Single"/> values.
    /// </summary>
    public class SingleArrayCurveStepSampler : ICurveSampler<ArraySegment<Single>, CurveKey<ArraySegment<Single>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleArrayCurveStepSampler"/> class.
        /// </summary>
        private SingleArrayCurveStepSampler() { }

        /// <inheritdoc/>
        public void CreateTemporaryValue(Int32 elementCount, out ArraySegment<Single> value) => value = new ArraySegment<Single>(ArrayPool<Single>.Shared.Rent(elementCount));

        /// <inheritdoc/>
        public void ReleaseTemporaryValue(in ArraySegment<Single> value) => ArrayPool<Single>.Shared.Return(value.Array);

        /// <inheritdoc/>
        public ArraySegment<Single> InterpolateKeyframes(CurveKey<ArraySegment<Single>> key1, CurveKey<ArraySegment<Single>> key2, Single t, ArraySegment<Single> offset, in ArraySegment<Single> existing)
        {
            // NOTE: Candidate for SIMD optimization in .NET 5.

            var count = key1.Value.Count;
            if (count != key2.Value.Count || count != existing.Count)
                throw new ArgumentException(UltravioletStrings.SamplerArgumentsMustHaveSameLength);

            var chosen = (t >= 1 ? key2.Value : key1.Value);
            for (var i = 0; i < count; i++)
                existing.GetItemRef(i) = offset.GetItemRef(i) + chosen.GetItemRef(i);

            return existing;
        }

        /// <inheritdoc/>
        public ArraySegment<Single> CalculateLinearExtension(CurveKey<ArraySegment<Single>> key, Single position, CurvePositionType positionType, in ArraySegment<Single> existing)
        {
            if (key.Value.Count != existing.Count)
                throw new ArgumentException(UltravioletStrings.SamplerArgumentsMustHaveSameLength);

            key.Value.CopyTo(existing);
            return existing;
        }

        /// <inheritdoc/>
        public ArraySegment<Single> CalculateCycleOffset(ArraySegment<Single> first, ArraySegment<Single> last, Int32 cycle, in ArraySegment<Single> existing)
        {
            // NOTE: Candidate for SIMD optimization in .NET 5.

            var count = first.Count;
            if (count != last.Count || count != existing.Count)
                throw new ArgumentException(UltravioletStrings.SamplerArgumentsMustHaveSameLength);

            for (var i = 0; i < count; i++)
                existing.GetItemRef(i) = (last.GetItemRef(i) - first.GetItemRef(i)) * cycle;

            return existing;
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="SingleArrayCurveStepSampler"/> class.
        /// </summary>
        public static SingleArrayCurveStepSampler Instance { get; } = new SingleArrayCurveStepSampler();
    }
}
