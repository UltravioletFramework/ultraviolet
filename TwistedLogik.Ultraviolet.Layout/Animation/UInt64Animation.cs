using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between unsigned 64-bit integers.
    /// </summary>
    public sealed class UInt64Animation : Animation<UInt64>
    {
        /// <inheritdoc/>
        public override UInt64 InterpolateValues(UInt64 value1, UInt64 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
