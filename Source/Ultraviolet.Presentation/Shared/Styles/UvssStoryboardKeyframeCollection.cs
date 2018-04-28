using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a collection of <see cref="UvssStoryboardKeyframe"/> objects which belog to an instance of <see cref="UvssStoryboardAnimation"/>.
    /// </summary>
    public sealed partial class UvssStoryboardKeyframeCollection : IEnumerable<UvssStoryboardKeyframe>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardKeyframeCollection"/> class
        /// </summary>
        internal UvssStoryboardKeyframeCollection()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssStoryboardKeyframeCollection"/> class
        /// by populating it with the keyframes in the specified collection.
        /// </summary>
        /// <param name="keyframes">A collection containing the keyframes with which to
        /// populate this collection instance.</param>
        internal UvssStoryboardKeyframeCollection(IEnumerable<UvssStoryboardKeyframe> keyframes)
        {
            this.keyframes.AddRange(keyframes);
        }

        /// <summary>
        /// Adds an animation keyframe to the collection.
        /// </summary>
        /// <param name="keyframe">The animation keyframe to add to the collection.</param>
        internal void Add(UvssStoryboardKeyframe keyframe)
        {
            Contract.Require(keyframe, nameof(keyframe));

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
