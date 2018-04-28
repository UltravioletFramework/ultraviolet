using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents an animation's collection of keyframes.
    /// </summary>
    /// <typeparam name="T">The type of value being animated.</typeparam>
    public sealed partial class AnimationKeyframeCollection<T> : IEnumerable<AnimationKeyframe<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationKeyframeCollection{T}"/> class.
        /// </summary>
        /// <param name="animation">The animation that owns the collection.</param>
        public AnimationKeyframeCollection(Animation<T> animation)
        {
            Contract.Require(animation, nameof(animation));

            this.animation = animation;
        }

        /// <summary>
        /// Adds a keyframe to the collection.
        /// </summary>
        /// <param name="keyframe">The keyframe to add to the collection.</param>
        /// <returns><see langword="true"/> if the keyframe was added to the collection; otherwise, <see langword="false"/>.</returns>
        public Boolean Add(AnimationKeyframe<T> keyframe)
        {
            Contract.Require(keyframe, nameof(keyframe));

            if (Contains(keyframe))
                return false;

            var index = 0;
            if (Count > 0)
            {
                for (int i = 0; i < keyframes.Count - 1; i++)
                {
                    if (keyframes[i].Time < keyframe.Time && keyframes[i + 1].Time >= keyframe.Time)
                    {
                        index = i + 1;
                        break;
                    }
                }
                index = keyframes.Count;
            }
            keyframes.Insert(index, keyframe);

            animation.RecalculateDuration();
            return true;
        }

        /// <summary>
        /// Removes the specified keyframe from the collection.
        /// </summary>
        /// <param name="keyframe">The keyframe to remove from the collection.</param>
        /// <returns><see langword="true"/> if the keyframe was removed from the collection; otherwise, false.</returns>
        public Boolean Remove(AnimationKeyframe<T> keyframe)
        {
            Contract.Require(keyframe, nameof(keyframe));

            if (keyframes.Remove(keyframe))
            {
                animation.RecalculateDuration();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified keyframe.
        /// </summary>
        /// <param name="keyframe">The keyframe to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified keyframe; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(AnimationKeyframe<T> keyframe)
        {
            Contract.Require(keyframe, nameof(keyframe));

            return keyframes.Contains(keyframe);
        }

        /// <summary>
        /// Gets the keyframe with the specified index within the collection.
        /// </summary>
        /// <param name="ix">The index of the keyframe to retrieve.</param>
        /// <returns>The keyframe with the specified index within the collection.</returns>
        public AnimationKeyframe<T> this[Int32 ix]
        {
            get { return keyframes[ix]; }
        }

        /// <summary>
        /// Gets the animation that owns the collection.
        /// </summary>
        public Animation<T> Animation
        {
            get { return animation; }
        }

        /// <summary>
        /// Gets the number of keyframes in the collection.
        /// </summary>
        public Int32 Count
        {
            get { return keyframes.Count; }
        }

        // Property values.
        private readonly Animation<T> animation;

        // State values.
        private readonly List<AnimationKeyframe<T>> keyframes = 
            new List<AnimationKeyframe<T>>();
    }
}
