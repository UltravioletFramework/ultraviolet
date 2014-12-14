using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between 16-bit integers.
    /// </summary>
    public sealed class Int16Animation : Animation<Int16>
    {
        /// <inheritdoc/>
        public override Int16 InterpolateValues(Int16 value1, Int16 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
