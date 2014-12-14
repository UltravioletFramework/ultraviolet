using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between Boolean values.
    /// </summary>
    public sealed class BooleanAnimation : Animation<Boolean>
    {
        /// <inheritdoc/>
        public override Boolean InterpolateValues(Boolean value1, Boolean value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
