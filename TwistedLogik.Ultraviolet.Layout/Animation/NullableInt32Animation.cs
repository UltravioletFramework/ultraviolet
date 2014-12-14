using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between nullable 32-bit integers.
    /// </summary>
    public sealed class NullableInt32Animation : Animation<Int32?>
    {
        /// <inheritdoc/>
        public override Int32? InterpolateValues(Int32? value1, Int32? value2, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Lerp(value1.GetValueOrDefault(), value2.GetValueOrDefault(), factor);
        }
    }
}
