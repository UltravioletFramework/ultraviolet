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
        public StoryboardTarget()
        {
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
        /// Gets the target's collection of animations.
        /// </summary>
        public StoryboardTargetAnimationCollection Animations
        {
            get { return animations; }
        }

        // Property values.
        private Storyboard storyboard;
        private readonly StoryboardTargetAnimationCollection animations;
    }
}
