using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between unsigned 16-bit integers.
    /// </summary>
    public sealed class UInt16Animation : Animation<UInt16>
    {
        /// <inheritdoc/>
        public override UInt16 InterpolateValues(UInt16 value1, UInt16 value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
