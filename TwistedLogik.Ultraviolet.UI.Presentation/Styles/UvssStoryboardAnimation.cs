using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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
        /// <param name="keyframes">The animation's collection of keyframes.</param>
        internal UvssStoryboardAnimation(String animatedProperty, UvssStoryboardKeyframeCollection keyframes)
        {
            this.animatedProperty = animatedProperty;
            this.keyframes        = keyframes;
        }

        /// <summary>
        /// Gets the name of the animated dependency property.
        /// </summary>
        public String AnimatedProperty
        {
            get { return animatedProperty; }
        }

        /// <summary>
        /// Gets the animation's collection of keyframes.
        /// </summary>
        public UvssStoryboardKeyframeCollection Keyframes
        {
            get { return keyframes; }
        }

        // Property values.
        private readonly String animatedProperty;
        private readonly UvssStoryboardKeyframeCollection keyframes;
    }
}
