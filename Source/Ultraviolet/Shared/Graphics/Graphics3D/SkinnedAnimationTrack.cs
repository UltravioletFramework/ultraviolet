using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the state of a particular <see cref="SkinnedAnimation"/> as applied to one <see cref="SkinnedModelInstance"/>.
    /// </summary>
    public class SkinnedAnimationTrack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedAnimationTrack"/> class.
        /// </summary>
        /// <param name="model">The model that owns this track.</param>
        public SkinnedAnimationTrack(SkinnedModelInstance model)
        {
            Contract.Require(model, nameof(model));

            this.Model = model;
        }

        /// <summary>
        /// Sets the track's position within the current animation's timeline.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <returns><see langword="true"/> if the position was set; otherwise, <see langword="false"/>.</returns>
        public Boolean SetPosition(Double position)
        {
            if (CurrentAnimation == null)
                return false;

            if (position < 0)
                position = 0;

            if (position > CurrentAnimation.Duration)
                position = CurrentAnimation.Duration;

            currentAnimationTime = position;

            return true;
        }

        /// <summary>
        /// Updates the animation track's state.
        /// </summary>
        /// <param name="elapsedSeconds">The number of seconds by which to advance the animation time.</param>
        /// <returns><see langword="true"/> if the track is playing and updated its state; otherwise, <see langword="false"/>.</returns>
        public Boolean AdvanceTime(Double elapsedSeconds)
        {
            if (!IsPlaying || IsPaused)
                return false;

            var effectiveElapsedSeconds = elapsedSeconds * SpeedMultiplier;

            switch (currentAnimationMode)
            {
                case SkinnedAnimationMode.Manual:
                case SkinnedAnimationMode.Loop:
                    {
                        var updatedAnimationTime = (currentAnimationTime + effectiveElapsedSeconds) % CurrentAnimation.Duration;
                        currentAnimationTime = updatedAnimationTime;
                    }
                    break;

                case SkinnedAnimationMode.FireAndForget:
                    {
                        var updatedAnimationTime = (currentAnimationTime + effectiveElapsedSeconds);
                        if (updatedAnimationTime >= CurrentAnimation.Duration)
                        {
                            Stop();
                        }
                        else
                        {
                            currentAnimationTime = updatedAnimationTime;
                        }
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="mode">The animation mode.</param>
        /// <param name="animation">The animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        public void Play(SkinnedAnimationMode mode, SkinnedAnimation animation, Single speedMultiplier)
        {
            Contract.Require(animation, nameof(animation));

            this.currentAnimationMode = mode;
            this.CurrentAnimation = animation;
            this.currentAnimationTime = 0.0;
            this.SpeedMultiplier = speedMultiplier;
        }

        /// <summary>
        /// Stops the currently playing animation.
        /// </summary>
        public void Stop()
        {
            this.currentAnimationMode = SkinnedAnimationMode.Loop;
            this.CurrentAnimation = null;
            this.currentAnimationTime = 0.0;
            this.SpeedMultiplier = 0.0;
        }

        /// <summary>
        /// Resets the currently playing animation to the beginning.
        /// </summary>
        public void Reset()
        {
            currentAnimationTime = 0.0;
        }

        /// <summary>
        /// Gets the <see cref="SkinnedModelInstance"/> that owns this track.
        /// </summary>
        public SkinnedModelInstance Model { get; }

        /// <summary>
        /// Gets the animation which is currently playing on this track.
        /// </summary>
        public SkinnedAnimation CurrentAnimation { get; private set; }

        /// <summary>
        /// Gets the track's current animation mode.
        /// </summary>
        public SkinnedAnimationMode CurrentAnimationMode => (CurrentAnimation == null) ? SkinnedAnimationMode.Loop : currentAnimationMode;

        /// <summary>
        /// Gets a value indicating whether the track is currently playing the specified animation.
        /// </summary>
        /// <param name="animation">The animation to evaluate.</param>
        /// <returns><see langword="true"/> if the track is currently playing the specified animation; otherwise, <see langword="false"/>.</returns>
        public Boolean IsPlayingAnimation(SkinnedAnimation animation)
        {
            Contract.Require(animation, nameof(animation));

            return animation == CurrentAnimation;
        }

        /// <summary>
        /// Gets a value indicating whether the track is currently playing an animation.
        /// </summary>
        public Boolean IsPlaying => CurrentAnimation != null;

        /// <summary>
        /// Gets or sets a value indicating whether the animation is currently paused.
        /// </summary>
        public Boolean IsPaused { get; set; }

        /// <summary>
        /// Gets the track's current position within its animation timeline.
        /// </summary>
        public Double Position => (CurrentAnimation == null) ? 0.0 : currentAnimationTime;

        /// <summary>
        /// Gets the duration of the track's current animation.
        /// </summary>
        public Double Duration => CurrentAnimation?.Duration ?? 0.0;

        /// <summary>
        /// Gets the speed multiplier which is being applied to the current animation.
        /// </summary>
        public Double SpeedMultiplier { get; set; }

        // The current animation state for this track.
        private SkinnedAnimationMode currentAnimationMode;
        private Double currentAnimationTime;
    }
}
