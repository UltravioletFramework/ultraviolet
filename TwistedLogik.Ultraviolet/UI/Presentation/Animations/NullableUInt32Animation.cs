using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable unsigned 32-bit integers.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class NullableUInt32Animation : Animation<UInt32?>
    {
        /// <inheritdoc/>
        public override UInt32? InterpolateValues(UInt32? value1, UInt32? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
