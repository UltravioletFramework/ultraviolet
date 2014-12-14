using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable single-precision floating point values.
    /// </summary>
    public sealed class NullableSingleAnimation : Animation<Single?>
    {
        /// <inheritdoc/>
        public override Single? InterpolateValues(Single? value1, Single? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
