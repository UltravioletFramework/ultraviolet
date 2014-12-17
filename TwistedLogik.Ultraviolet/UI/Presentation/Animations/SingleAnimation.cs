using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between single-precision floating point values.
    /// </summary>
    public sealed class SingleAnimation : Animation<Single>
    {
        /// <inheritdoc/>
        public override Single InterpolateValues(Single value1, Single value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
