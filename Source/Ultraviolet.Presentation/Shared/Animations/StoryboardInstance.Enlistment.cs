namespace Ultraviolet.Presentation.Animations
{
    partial class StoryboardInstance
    {
        /// <summary>
        /// Represents a dependency property which has been enlisted into the storyboard.
        /// </summary>
        private struct Enlistment
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Enlistment"/> structure.
            /// </summary>
            /// <param name="dpValue">The dependency property which has been enlisted.</param>
            /// <param name="animation">The animation to apply to the dependency property.</param>
            public Enlistment(IDependencyPropertyValue dpValue, AnimationBase animation)
            {
                this.dpValue = dpValue;
                this.animation = animation;
            }

            /// <summary>
            /// Gets the dependency property which has been enlisted.
            /// </summary>
            public IDependencyPropertyValue DependencyPropertyValue
            {
                get { return dpValue; }
            }

            /// <summary>
            /// Gets the animation to apply to the dependency property.
            /// </summary>
            public AnimationBase Animation
            {
                get { return animation; }
            }

            // Property values.
            private readonly IDependencyPropertyValue dpValue;
            private readonly AnimationBase animation;
        }
    }
}
