using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable decimal values.
    /// </summary>
    public sealed class NullableDecimalAnimation : Animation<Decimal?>
    {
        /// <inheritdoc/>
        public override Decimal? InterpolateValues(Decimal? value1, Decimal? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
