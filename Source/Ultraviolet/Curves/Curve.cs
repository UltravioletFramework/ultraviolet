using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a mathematical curve.
    /// </summary>
    /// <typeparam name="TValue">The type of value which comprises the curve.</typeparam>
    /// <typeparam name="TKey">The type of keyframe which defines the shape of the curve.</typeparam>
    public class Curve<TValue, TKey> : Curve<TValue>
        where TKey : CurveKey<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Curve{TValue, TKey}"/> class.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="sampler">The <see cref="ICurveSampler{TValue, TKey}"/> to use when sampling this curve's values.</param>
        /// <param name="keys">A collection of <typeparamref name="TKey"/> objects from which to construct the curve.</param>
        public Curve(CurveLoopType preLoop, CurveLoopType postLoop, ICurveSampler<TValue, TKey> sampler, IEnumerable<TKey> keys)
            : base(preLoop, postLoop)
        {
            Contract.Require(sampler, nameof(sampler));

            this.Sampler = sampler;
            this.Keys = new CurveKeyCollection<TValue, TKey>(keys);
            this.keyFirst = null;
            this.keyLast = null;

            if (this.Keys.Count > 0)
            {
                this.keyFirst = this.Keys[0].Key;
                this.StartPosition = this.keyFirst.Position;
                this.keyLast = this.Keys[this.Keys.Count - 1].Key;
                this.EndPosition = this.keyLast.Position;
                this.Length = this.EndPosition - this.StartPosition;
            }
        }

        /// <inheritdoc/>
        public override TValue Evaluate(Single position, in TValue existing)
        {
            var keys = Keys;

            if (keys.Count == 0)
                return default(TValue);

            if (keys.Count == 1)
                return keyFirst.Value;

            if (position < keyFirst.Position)
                return EvaluateOutside(PreLoop, keyFirst, position, CurvePositionType.BeforeCurve, in existing);

            if (position > keyLast.Position)
                return EvaluateOutside(PostLoop, keyLast, position, CurvePositionType.AfterCurve, in existing);

            return EvaluateInside(position, default(TValue), in existing);
        }

        /// <inheritdoc/>
        public override Single StartPosition { get; }

        /// <inheritdoc/>
        public override Single EndPosition { get; }

        /// <inheritdoc/>
        public override Single Length { get; }

        /// <summary>
        /// Gets a value indicating whether the curve's value is constant.
        /// </summary>
        public Boolean IsConstant => Keys.IsConstant;

        /// <summary>
        /// Gets the sampler for this curve.
        /// </summary>
        public ICurveSampler<TValue, TKey> Sampler { get; }

        /// <summary>
        /// Gets the curve's collection of keys.
        /// </summary>
        public CurveKeyCollection<TValue, TKey> Keys { get; }

        /// <summary>
        /// Gets the index of the cycle that contains the specified position.
        /// </summary>
        private Int32 GetCycleIndex(Single position)
        {
            var cycle = (position - keyFirst.Position) / Length;
            return (cycle < 0) ? (int)cycle - 1 : (int)cycle;
        }

        /// <summary>
        /// Evaluates a position inside of the curve.
        /// </summary>
        private TValue EvaluateInside(Single position, TValue offset, in TValue existing)
        {
            FindKeyRecordsAtCurvePosition(position, out var record1, out var record2);

            var key1 = record1.Key;
            var key2 = record2.Key;

            var factor = (position - key1.Position) / (key2.Position - key1.Position);
            var result = (record1.SamplerOverride ?? Sampler).InterpolateKeyframes(key1, key2, factor, offset, existing);

            return result;
        }

        /// <summary>
        /// Evaluates a position outside of the curve.
        /// </summary>
        private TValue EvaluateOutside(CurveLoopType loop, TKey loopKey, Single position, CurvePositionType positionType, in TValue existing)
        {
            var offsetTemp = default(TValue);
            var offset = default(TValue);

            switch (loop)
            {
                case CurveLoopType.Constant:
                    {
                        return loopKey.Value;
                    }

                case CurveLoopType.Linear:
                    {
                        return Sampler.CalculateLinearExtension(loopKey, position, positionType, existing);
                    }

                case CurveLoopType.Cycle:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * Length);
                    }
                    break;

                case CurveLoopType.CycleOffset:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * Length);
                        Sampler.CreateTemporaryValue(Keys.ElementCount, out offsetTemp);
                        offset = Sampler.CalculateCycleOffset(keyFirst.Value, keyLast.Value, cycle, offsetTemp);
                    }
                    break;

                case CurveLoopType.Oscillate:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * Length);
                        if (cycle % 2 != 0)
                        {
                            position = keyFirst.Position + (Length - (position - keyFirst.Position));
                        }
                    }
                    break;
            }

            var result = EvaluateInside(position, offset, existing);
            Sampler.ReleaseTemporaryValue(offsetTemp);
            return result;
        }

        /// <summary>
        /// Finds the pair of keys which surrounds the specified position on the curve.
        /// </summary>
        private void FindKeyRecordsAtCurvePosition(Single position, out CurveKeyRecord<TValue, TKey> key1, out CurveKeyRecord<TValue, TKey> key2)
        {
            var keys = Keys;
            key1 = keys[0];
            key2 = keys[keys.Count - 1];
            for (var i = 1; i < keys.Count; i++)
            {
                if (keys[i].Key.Position >= position && keys[i - 1].Key.Position <= position)
                {
                    key1 = keys[i - 1];
                    key2 = keys[i];
                    return;
                }
            }
        }

        // The curve's collection of keys.
        private readonly TKey keyFirst;
        private readonly TKey keyLast;
    }
}
