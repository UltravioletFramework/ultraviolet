using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable Boolean values.
    /// </summary>
    public sealed class NullableBooleanAnimation : Animation<Boolean?>
    {
        /// <inheritdoc/>
        public override Boolean? InterpolateValues(Boolean? value1, Boolean? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
