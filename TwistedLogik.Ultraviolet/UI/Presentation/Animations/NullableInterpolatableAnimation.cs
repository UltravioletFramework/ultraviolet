using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents an animation between nullable values which implement the <see cref="IInterpolatable{T}"/> interface.
    /// </summary>
    public sealed class NullableInterpolatableAnimation<T> : Animation<T?> where T : struct, IInterpolatable<T>
    {
        /// <inheritdoc/>
        public override T? InterpolateValues(T? value1, T? value2, EasingFunction easing, Single factor)
        {
            if (value1 == null || value2 == null)
                return null;

            return Tweening.Tween(value1.GetValueOrDefault(), value2.GetValueOrDefault(), easing, factor);
        }
    }
}
