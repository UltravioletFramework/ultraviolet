
namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet document's representation of a storyboard target.
    /// </summary>
    public sealed class UvssStoryboardTarget 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardTarget"/> class.
        /// </summary>
        /// <param name="selector">The target's selector.</param>
        /// <param name="filter">The storyboard target's type filter.</param>
        /// <param name="animations">The target's collection of animations.</param>
        internal UvssStoryboardTarget(UvssSelector selector, UvssStoryboardTargetFilter filter, UvssStoryboardAnimationCollection animations)
        {
            this.selector   = selector;
            this.filter     = filter;
            this.animations = animations;
        }

        /// <summary>
        /// Gets the selector which specifies which elements the storyboard is animating.
        /// </summary>
        public UvssSelector Selector
        {
            get { return selector; }
        }

        /// <summary>
        /// Gets the storyboard target's type filter.
        /// </summary>
        public UvssStoryboardTargetFilter Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Gets the storyboard target's collection of animations.
        /// </summary>
        public UvssStoryboardAnimationCollection Animations
        {
            get { return animations; }
        }

        // Property values.
        private readonly UvssSelector selector;
        private readonly UvssStoryboardTargetFilter filter;
        private readonly UvssStoryboardAnimationCollection animations;
    }
}
