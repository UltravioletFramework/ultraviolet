using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable unsigned 16-bit integers.
    /// </summary>
    public sealed class NullableUInt16Animation : Animation<UInt16?>
    {
        /// <inheritdoc/>
        public override UInt16? InterpolateValues(UInt16? value1, UInt16? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
