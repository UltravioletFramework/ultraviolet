using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between signed byte values.
    /// </summary>
    public sealed class SByteAnimation : Animation<SByte>
    {
        /// <inheritdoc/>
        public override SByte InterpolateValues(SByte value1, SByte value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween(value1, value2, easing, factor);
        }
    }
}
