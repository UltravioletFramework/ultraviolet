using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between colors.
    /// </summary>
    public sealed class ColorAnimation : Animation<Color>
    {
        /// <inheritdoc/>
        public override Color InterpolateValues(Color value1, Color value2, Single factor)
        {
            return value1.Interpolate(value2, factor);
        }
    }
}
