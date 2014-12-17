using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable byte values.
    /// </summary>
    public sealed class NullableByteAnimation : Animation<Byte?>
    {
        /// <inheritdoc/>
        public override Byte? InterpolateValues(Byte? value1, Byte? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
