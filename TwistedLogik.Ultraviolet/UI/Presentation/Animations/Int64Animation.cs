using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between 64-bit integers.
    /// </summary>
    public sealed class Int64Animation : Animation<Int64>
    {
        /// <inheritdoc/>
        public override Int64 InterpolateValues(Int64 value1, Int64 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
