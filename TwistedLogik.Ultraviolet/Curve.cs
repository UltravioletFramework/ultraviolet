using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a mathematical curve.
    /// </summary>
    [Serializable]
    public sealed class Curve
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey"/> objects from which to construct the curve.</param>
        public Curve(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey> keys)
        {
            this.preLoop = preLoop;
            this.postLoop = postLoop;
            
            this.keys     = new CurveKeyCollection(keys);
            this.keyFirst = (keys == null) ? null : this.keys[0];
            this.keyLast  = (keys == null) ? null : this.keys[this.keys.Count - 1];
            this.length   = (keys == null) ? 0f : keyLast.Position - keyFirst.Position;

            this.easingFunction = Evaluate;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Curve"/> to an <see cref="EasingFunction"/>.
        /// </summary>
        /// <param name="curve">The <see cref="Curve"/> to convert.</param>
        /// <returns>The converted <see cref="EasingFunction"/>.</returns>
        public static implicit operator EasingFunction(Curve curve)
        {
            return (curve == null) ? null : curve.easingFunction;
        }

        /// <summary>
        /// Calculates the value of the curve at the specified position.
        /// </summary>
        /// <param name="position">The position at which to calculate a value.</param>
        /// <returns>The value of the curve at the specified position.</returns>
        public Single Evaluate(Single position)
        {
            if (keys.Count == 0)
                return 0f;

            if (keys.Count == 1)
                return keys[0].Value;

            if (position < keys[0].Position)
            {
                return EvaluateOutside(PreLoop, keys[0], keys[0].TangentIn, position);
            }
            if (position > keys[keys.Count - 1].Position)
            {
                return EvaluateOutside(PostLoop, keys[keys.Count - 1], keys[keys.Count - 1].TangentOut, position);
            }
            return EvaluateInside(position);
        }

        /// <summary>
        /// Gets a value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.
        /// </summary>
        public CurveLoopType PreLoop
        {
            get { return preLoop; }
        }

        /// <summary>
        /// Gets a value indicating how the curve's values are determined
        /// for points after the end of the curve.
        /// </summary>
        public CurveLoopType PostLoop
        {
            get { return postLoop; }
        }

        /// <summary>
        /// Gets a value indicating whether the curve's value is constant.
        /// </summary>
        public Boolean IsConstant
        {
            get { return keys.Count == 0 || keys.Count == 1; }
        }

        /// <summary>
        /// Gets the curve's collection of keys.
        /// </summary>
        public CurveKeyCollection Keys
        {
            get { return keys; }
        }

        /// <summary>
        /// Gets the index of the cycle that contains the specified position.
        /// </summary>
        private Int32 GetCycleIndex(Single position)
        {
            var cycle = (position - keyFirst.Position) / length;
            return (cycle < 0) ? (int)cycle - 1 : (int)cycle;
        }

        /// <summary>
        /// Evaluates a position inside of the curve.
        /// </summary>
        private Single EvaluateInside(Single position)
        {
            CurveKey key1, key2;
            FindKeysAtCurvePosition(position, out key1, out key2);
            return InterpolateValue(key1, key2, (position - key1.Position) / (key2.Position - key1.Position));
        }

        /// <summary>
        /// Evaluates a position outside of the curve.
        /// </summary>
        private Single EvaluateOutside(CurveLoopType loop, CurveKey loopKey, Single loopKeyTangent, Single position)
        {
            var offset = 0f;
            switch (loop)
            {
                case CurveLoopType.Constant:
                    {
                        return loopKey.Value;
                    }

                case CurveLoopType.Linear:
                    {
                        return loopKey.Value - loopKeyTangent * (loopKey.Position - position);
                    }

                case CurveLoopType.Cycle:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * length);
                    }
                    break;

                case CurveLoopType.CycleOffset:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * length);
                        offset = (keyLast.Value - keyFirst.Value) * cycle;
                    }
                    break;

                case CurveLoopType.Oscillate:
                    {
                        var cycle = GetCycleIndex(position);
                        position = position - (cycle * length);
                        if (cycle % 2 != 0)
                        {
                            position = keyFirst.Position + (length - (position - keyFirst.Position));
                        }
                    }
                    break;
            }
            return offset + EvaluateInside(position);
        }

        /// <summary>
        /// Calculates a value between the specified curve keys based on the specified interpolation factor.
        /// </summary>
        private Single InterpolateValue(CurveKey key1, CurveKey key2, Single t)
        {
            switch (key1.Continuity)
            {
                case CurveContinuity.Step:
                    return t >= 1 ? key2.Value : key1.Value;

                default:
                    {
                        var t2         = t * t;
                        var t3         = t2 * t;
                        var key1Value  = key1.Value;
                        var key2Value  = key2.Value;
                        var tangentIn  = key2.TangentIn;
                        var tangentOut = key1.TangentOut;

                        var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1.0); // (2t^3 - 3t^2 + 1)
                        var polynomial2 = (t3 - 2.0 * t2 + t);         // (t3 - 2t^2 + t)  
                        var polynomial3 = (-2.0 * t3 + 3.0 * t2);      // (-2t^2 + 3t^2)
                        var polynomial4 = (t3 - t2);                   // (t^3 - t^2)

                        return (float)(key1Value * polynomial1 + tangentOut * polynomial2 + key2Value * polynomial3 + tangentIn * polynomial4);
                    }
            }
        }

        /// <summary>
        /// Finds the pair of keys which surrounds the specified position on the curve.
        /// </summary>
        private void FindKeysAtCurvePosition(Single position, out CurveKey key1, out CurveKey key2)
        {
            key1 = keys[0];
            key2 = keys[keys.Count - 1];
            for (var i = 1; i < keys.Count; i++)
            {
                if (keys[i].Position >= position && keys[i - 1].Position <= position)
                {
                    key1 = keys[i - 1];
                    key2 = keys[i];
                    return;
                }
            }
        }

        // Property values.
        private readonly CurveLoopType preLoop;
        private readonly CurveLoopType postLoop;

        // The curve's collection of keys.
        private readonly CurveKeyCollection keys;
        private readonly CurveKey keyFirst;
        private readonly CurveKey keyLast;
        private readonly Single length;

        // An easing function that represents the curve.
        private readonly EasingFunction easingFunction;
    }
}
