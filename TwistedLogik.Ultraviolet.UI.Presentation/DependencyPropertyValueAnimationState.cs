using Ultraviolet.Presentation.Animations;

namespace Ultraviolet.Presentation
{
    partial class DependencyObject
    {
        /// <summary>
        /// Represents the animation state of a <see cref="DependencyObject.DependencyPropertyValue{T}"/> object.
        /// </summary>
        internal class DependencyPropertyValueAnimationState<T>
        {
            /// <summary>
            /// Gets or sets the clock which drives the animation.
            /// </summary>
            public Clock Clock { get; set; }

            /// <summary>
            /// Gets or sets the animation which is being applied to the value.
            /// </summary>
            public Animation<T> Animation { get; set; }

            /// <summary>
            /// Gets or sets the current animated value.
            /// </summary>
            public T CurrentValue { get; set; }

            /// <summary>
            /// Gets or sets the target animated value.
            /// </summary>
            public T TargetValue { get; set; }

            /// <summary>
            /// Gets or sets the animation hand-off value.
            /// </summary>
            public T HandOffValue { get; set; }

            /// <summary>
            /// Gets or sets the easing function being applied to the value.
            /// </summary>
            public EasingFunction EasingFunction { get; set; }

            /// <summary>
            /// Gets or sets the storyboard instance which governs the animation.
            /// </summary>
            public StoryboardInstance StoryboardInstance { get; set; }
        }
    }
}
