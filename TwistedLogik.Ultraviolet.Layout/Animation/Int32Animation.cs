using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between 32-bit integers.
    /// </summary>
    public sealed class Int32Animation : Animation<Int32>
    {
        /// <inheritdoc/>
        public override Int32 InterpolateValues(Int32 value1, Int32 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
