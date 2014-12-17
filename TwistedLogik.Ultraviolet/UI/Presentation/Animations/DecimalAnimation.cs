using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between decimal values.
    /// </summary>
    public sealed class DecimalAnimation : Animation<Decimal>
    {
        /// <inheritdoc/>
        public override Decimal InterpolateValues(Decimal value1, Decimal value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
