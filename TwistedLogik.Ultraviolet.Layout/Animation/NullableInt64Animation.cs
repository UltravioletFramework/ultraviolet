using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable 64-bit integers.
    /// </summary>
    public sealed class NullableInt64Animation : Animation<Int64?>
    {
        /// <inheritdoc/>
        public override Int64? InterpolateValues(Int64? value1, Int64? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
