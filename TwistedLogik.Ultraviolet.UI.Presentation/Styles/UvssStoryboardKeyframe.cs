using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet document's representation of an animation keyframe.
    /// </summary>
    public sealed class UvssStoryboardKeyframe
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardKeyframe"/> class.
        /// </summary>
        /// <param name="easing">The keyframe's easing.</param>
        /// <param name="value">The keyframe's value.</param>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        internal UvssStoryboardKeyframe(String easing, String value, Double time)
        {
            this.easing = easing;
            this.value  = value;
            this.time   = time;
        }

        /// <summary>
        /// Gets the keyframe's easing.
        /// </summary>
        public String Easing
        {
            get { return easing; }
        }

        /// <summary>
        /// Gets the keyframe's value.
        /// </summary>
        public String Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the keyframe's time in milliseconds.
        /// </summary>
        public Double Time
        {
            get { return time; }
        }

        // Property values.
        private readonly String easing;
        private readonly String value;
        private readonly Double time;
    }
}
