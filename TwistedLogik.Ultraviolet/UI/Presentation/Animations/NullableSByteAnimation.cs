using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable signed byte values.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class NullableSByteAnimation : Animation<SByte?>
    {
        /// <inheritdoc/>
        public override SByte? InterpolateValues(SByte? value1, SByte? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
