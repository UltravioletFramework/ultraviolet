using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable double-precision floating point values.
    /// </summary>
    public sealed class NullableDoubleAnimation : Animation<Double?>
    {
        /// <inheritdoc/>
        public override Double? InterpolateValues(Double? value1, Double? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
