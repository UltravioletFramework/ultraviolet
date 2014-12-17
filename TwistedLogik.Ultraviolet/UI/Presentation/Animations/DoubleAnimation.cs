using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between double-precision floating point values.
    /// </summary>
    public sealed class DoubleAnimation : Animation<Double>
    {
        /// <inheritdoc/>
        public override Double InterpolateValues(Double value1, Double value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
