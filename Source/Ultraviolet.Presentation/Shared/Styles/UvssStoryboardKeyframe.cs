using System;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents an Ultraviolet Style Sheet document's representation of an animation keyframe.
    /// </summary>
    public sealed class UvssStoryboardKeyframe
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardKeyframe"/> class.
        /// </summary>
        /// <param name="time">The keyframe's time in milliseconds.</param>
        /// <param name="value">The keyframe's value.</param>
        /// <param name="easing">The keyframe's easing.</param>
        internal UvssStoryboardKeyframe(Double time, DependencyValue value, String easing)
        {
            this.time = time;
            this.value = value;
            this.easing = easing;
        }

        /// <summary>
        /// Gets the keyframe's time in milliseconds.
        /// </summary>
        /// <value>A <see cref="Double"/> value specifying the time in milliseconds at which the keyframe
        /// is located within its animation.</value>
        public Double Time
        {
            get { return time; }
        }

        /// <summary>
        /// Gets the keyframe's value.
        /// </summary>
        /// <value>A <see cref="DependencyValue"/> value that will be applied to the animated property by the keyframe.
        /// The value will be parsed into the appropriate type via the <see cref="ObjectLoader"/> class.</value>
        public DependencyValue Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the keyframe's easing.
        /// </summary>
        /// <value>A <see cref="String"/> that contains the name of the keyframe's easing function,
        /// or <see langword="null"/> if the keyframe is using linear easing.</value>
        public String Easing
        {
            get { return easing; }
        }

        // Property values.
        private readonly Double time;
        private readonly DependencyValue value;
        private readonly String easing;
    }
}
