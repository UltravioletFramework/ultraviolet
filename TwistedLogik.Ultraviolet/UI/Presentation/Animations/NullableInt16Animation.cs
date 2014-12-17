using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable 16-bit integers.
    /// </summary>
    public sealed class NullableInt16Animation : Animation<Int16?>
    {
        /// <inheritdoc/>
        public override Int16? InterpolateValues(Int16? value1, Int16? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
