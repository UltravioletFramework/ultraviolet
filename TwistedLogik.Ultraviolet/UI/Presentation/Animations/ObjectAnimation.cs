using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents a generic animation for arbitrary object types.
    /// </summary>
    /// <typeparam name="T">The type of value being animated.</typeparam>
    public sealed class ObjectAnimation<T> : Animation<T>
    {
        /// <inheritdoc/>
        public override T InterpolateValues(T value1, T value2, EasingFunction easing, Single factor)
        {
            return (factor >= 1) ? value2 : value1;
        }
    }
}
