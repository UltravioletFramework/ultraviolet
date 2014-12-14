using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable unsigned 64-bit integers.
    /// </summary>
    public sealed class NullableUInt64Animation : Animation<UInt64?>
    {
        /// <inheritdoc/>
        public override UInt64? InterpolateValues(UInt64? value1, UInt64? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
