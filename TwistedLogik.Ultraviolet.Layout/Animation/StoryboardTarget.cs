using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.Animation
{
    /// <summary>
    /// Represents an element which is the target of an animation.
    /// </summary>
    public sealed class StoryboardTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardTarget"/> class.
        /// </summary>
        /// <param name="storyboard">The storyboard that owns the target.</param>
        internal StoryboardTarget(Storyboard storyboard)
        {
            Contract.Require(storyboard, "storyboard");

            this.storyboard = storyboard;
            this.animations = new StoryboardTargetAnimationCollection(this);
        }

        /// <summary>
        /// Gets the storyboard that owns the target.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return storyboard; }
        }

        /// <summary>
        /// Gets the target's collection of animations.
        /// </summary>
        public StoryboardTargetAnimationCollection Animations
        {
            get { return animations; }
        }

        // Property values.
        private readonly StoryboardTargetAnimationCollection animations;

        // State values.
        private readonly Storyboard storyboard;
    }
}
