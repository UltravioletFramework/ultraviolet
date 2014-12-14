using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between byte values.
    /// </summary>
    public sealed class ByteAnimation : Animation<Byte>
    {
        /// <inheritdoc/>
        public override Byte InterpolateValues(Byte value1, Byte value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
