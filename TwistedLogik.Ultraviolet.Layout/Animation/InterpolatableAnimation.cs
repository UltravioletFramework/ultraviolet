using System;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an animation between values which implement the <see cref="IInterpolatable{T}"/> interface.
    /// </summary>
    public sealed class InterpolatableAnimation<T> : Animation<T> where T : IInterpolatable<T>
    {
        /// <inheritdoc/>
        public override T InterpolateValues(T value1, T value2, EasingFunction easing, Single factor)
        {
            return Tweening.Tween<T>(value1, value2, easing, factor);
        }
    }
}
