using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between unsigned 32-bit integers.
    /// </summary>
    public sealed class UInt32Animation : Animation<UInt32>
    {
        /// <inheritdoc/>
        public override UInt32 InterpolateValues(UInt32 value1, UInt32 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
