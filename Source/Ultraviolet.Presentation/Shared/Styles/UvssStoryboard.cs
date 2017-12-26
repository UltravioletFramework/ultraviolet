using System;
using Ultraviolet.Presentation.Animations;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet document's representation of a storyboard animation.
    /// </summary>
    public sealed class UvssStoryboard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboard"/> class.
        /// </summary>
        /// <param name="name">The storyboard's name.</param>
        /// <param name="loopBehavior">The storyboard's loop behavior.</param>
        /// <param name="targets">The storyboard's collection of targets.</param>
        internal UvssStoryboard(String name, LoopBehavior loopBehavior, UvssStoryboardTargetCollection targets)
        {
            this.name = name;
            this.loopBehavior = loopBehavior;
            this.targets = targets;
        }

        /// <summary>
        /// Gets the storyboard's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the storyboard's loop behavior.
        /// </summary>
        public LoopBehavior LoopBehavior
        {
            get { return loopBehavior; }
        }

        /// <summary>
        /// Gets the storyboard's collection of targets.
        /// </summary>
        public UvssStoryboardTargetCollection Targets
        {
            get { return targets; }
        }

        // Property values.
        private readonly String name;
        private readonly LoopBehavior loopBehavior;
        private readonly UvssStoryboardTargetCollection targets;
    }
}
