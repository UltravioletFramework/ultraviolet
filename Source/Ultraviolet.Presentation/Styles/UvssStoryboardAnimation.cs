namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet document's representation of a storyboard animation.
    /// </summary>
    public sealed partial class UvssStoryboardAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardAnimation"/> class.
        /// </summary>
        /// <param name="animatedProperty">The name of the animated property.</param>
        /// <param name="navigationExpression">The animated property's navigation expression.</param>
        /// <param name="keyframes">The animation's collection of keyframes.</param>
        internal UvssStoryboardAnimation(DependencyName animatedProperty, UvssNavigationExpression navigationExpression, UvssStoryboardKeyframeCollection keyframes)
        {
            this.AnimatedProperty = animatedProperty;
            this.NavigationExpression = navigationExpression;
            this.Keyframes = keyframes;
        }
        
        /// <summary>
        /// Gets the name of the animated dependency property.
        /// </summary>
        public DependencyName AnimatedProperty { get; }

        /// <summary>
        /// Gets the animation's navigation expression, if it has one.
        /// </summary>
        public UvssNavigationExpression NavigationExpression { get; }

        /// <summary>
        /// Gets the animation's collection of keyframes.
        /// </summary>
        public UvssStoryboardKeyframeCollection Keyframes { get; }
    }
}
