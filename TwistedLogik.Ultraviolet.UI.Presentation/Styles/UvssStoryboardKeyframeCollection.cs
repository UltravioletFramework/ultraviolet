using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardKeyframe"/> objects which belog to an instance of <see cref="UvssStoryboardAnimation"/>.
    /// </summary>
    public sealed partial class UvssStoryboardKeyframeCollection : IEnumerable<UvssStoryboardKeyframe>
    {
        /// <summary>
        /// Adds an animation keyframe to the collection.
        /// </summary>
        /// <param name="keyframe">The animation keyframe to add to the collection.</param>
        internal void Add(UvssStoryboardKeyframe keyframe)
        {
            Contract.Require(keyframe, "keyframe");

            this.keyframes.Add(keyframe);
        }

        /// <summary>
        /// Gets the number of keyframes in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return keyframes.Count; }
        }

        // State values.
        private readonly List<UvssStoryboardKeyframe> keyframes = 
            new List<UvssStoryboardKeyframe>();
    }
}
