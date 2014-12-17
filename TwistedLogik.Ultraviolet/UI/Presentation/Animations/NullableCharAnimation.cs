using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable char values.
    /// </summary>
    public sealed class NullableCharAnimation : Animation<Char?>
    {
        /// <inheritdoc/>
        public override Char? InterpolateValues(Char? value1, Char? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
