using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents an element which is the target of an animation.
    /// </summary>
    public sealed class StoryboardTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTarget"/> class.
        /// </summary>
        /// <param name="selector">The selector which specifies which elements this target applies to.</param>
        public StoryboardTarget(UvssSelector selector)
        {
            this.selector   = selector;
            this.animations = new StoryboardTargetAnimationCollection(this);
        }

        /// <summary>
        /// Gets the storyboard that owns the target.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return storyboard; }
            internal set { storyboard = value; }
        }

        /// <summary>
        /// Gets the selector that specifies which elements this target applies to.
        /// </summary>
        public UvssSelector Selector
        {
            get { return selector; }
        }

        /// <summary>
        /// Gets the target's collection of animations.
        /// </summary>
        public StoryboardTargetAnimationCollection Animations
        {
            get { return animations; }
        }

        // Property values.
        private Storyboard storyboard;
        private UvssSelector selector;
        private readonly StoryboardTargetAnimationCollection animations;
    }
}
