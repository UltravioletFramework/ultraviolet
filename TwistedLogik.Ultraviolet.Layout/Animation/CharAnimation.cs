using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between char values.
    /// </summary>
    public sealed class CharAnimation : Animation<Char>
    {
        /// <inheritdoc/>
        public override Char InterpolateValues(Char value1, Char value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
