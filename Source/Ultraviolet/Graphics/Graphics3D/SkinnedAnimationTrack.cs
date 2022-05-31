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

            var shouldBeStopped = false;
            var previousAnimationTime = currentAnimationTime;
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
                        if (updatedAnimationTime >= CurrentAnimation.Duration - easeOutDuration && !IsStopping)
                            Stop();

                        if (updatedAnimationTime >= CurrentAnimation.Duration)
                            shouldBeStopped = true;

                        currentAnimationTime = updatedAnimationTime;
                    }
                    break;
            }

            if (callbacks.HasValue)
                callbacks.Value.OnAdvanced?.Invoke(Model, previousAnimationTime, currentAnimationTime, callbacks.Value.OnAdvancedState);

            if (shouldBeStopped || ApplyEasing(effectiveElapsedSeconds))
                StopImmediate();

            return true;
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="mode">The animation mode.</param>
        /// <param name="animation">The animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        public void Play(SkinnedAnimationMode mode, SkinnedAnimation animation, Single speedMultiplier = 1f, SkinnedAnimationCallbacks? callbacks = null)
        {
            Play(mode, animation, speedMultiplier, Easings.EaseInLinear, 0.0, Easings.EaseOutLinear, 0.0, callbacks);
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="mode">The animation mode.</param>
        /// <param name="animation">The animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="easeInFunction">The easing function to apply when easing in the animation.</param>
        /// <param name="easeInDuration">The number of seconds over which to ease in the animation.</param>
        /// <param name="easeOutFunction">The easing function to apply when easing out the animation.</param>
        /// <param name="easeOutDuration">The number of seconds over which to ease out the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        public void Play(SkinnedAnimationMode mode, SkinnedAnimation animation, Single speedMultiplier,
            EasingFunction easeInFunction, Double easeInDuration, EasingFunction easeOutFunction, Double easeOutDuration, SkinnedAnimationCallbacks? callbacks = null)
        {
            Contract.Require(animation, nameof(animation));
            Contract.Require(easeInFunction, nameof(easeInFunction));
            Contract.EnsureRange(easeInDuration >= 0, nameof(easeInDuration));
            Contract.Require(easeOutFunction, nameof(easeOutFunction));
            Contract.EnsureRange(easeOutDuration >= 0, nameof(easeOutDuration));

            this.currentAnimationMode = mode;
            this.CurrentAnimation = animation;
            this.currentAnimationTime = 0.0;
            this.SpeedMultiplier = speedMultiplier;

            if (mode != SkinnedAnimationMode.Manual)
            {
                this.easeInFunction = easeInFunction;
                this.easeInDuration = easeInDuration;
                this.easeOutFunction = easeOutFunction;
                this.easeOutDuration = easeOutDuration;
            }
            else
            {
                this.easeInFunction = null;
                this.easeInDuration = 0;
                this.easeOutFunction = null;
                this.easeOutDuration = 0;
            }

            this.easeInElasped = 0;
            this.easeOutElapsed = 0;
            this.BlendingWeight = 1f;

            this.callbacks = callbacks;
        }

        /// <summary>
        /// Stops the currently playing animation using the current easing settings.
        /// </summary>
        public void Stop()
        {
            if (!IsPlaying)
                return;

            if (this.easeOutDuration == 0.0)
            {
                StopImmediate();
            }
            else
            {
                IsStopping = true;
            }
        }

        /// <summary>
        /// Immediately stops the currently playing animation without performing any easing.
        /// </summary>
        public void StopImmediate()
        {
            if (!IsPlaying)
                return;

            if (callbacks.HasValue)
                callbacks.Value.OnStopped?.Invoke(Model, callbacks.Value.OnStoppedState);

            this.currentAnimationMode = SkinnedAnimationMode.Loop;
            this.CurrentAnimation = null;
            this.currentAnimationTime = 0.0;
            this.SpeedMultiplier = 0.0;
            this.BlendingWeight = 0.0f;

            this.easeInFunction = null;
            this.easeInDuration = 0.0;
            this.easeOutFunction = null;
            this.easeOutDuration = 0.0;

            this.callbacks = null;

            this.IsStopping = false;
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
        /// Gets a value indicating whether the track is currently stopping.
        /// </summary>
        public Boolean IsStopping { get; private set; }

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

        /// <summary>
        /// Gets the blending weight for this track.
        /// </summary>
        public Single BlendingWeight { get; private set; }

        /// <summary>
        /// Applies easing to the animation track and calculates its blend weight.
        /// </summary>
        private Boolean ApplyEasing(Double elapsedSeconds)
        {
            BlendingWeight = 1f;

            if (!IsPlaying)
                return false;

            if (easeInDuration > 0 && easeInElasped < easeInDuration)
            {
                easeInElasped += elapsedSeconds;
                if (easeInElasped > easeInDuration)
                    easeInElasped = easeInDuration;

                var t = (Single)(easeInElasped / easeInDuration);
                BlendingWeight = MathUtil.Clamp(easeInFunction(t), 0f, 1f);
            }

            if (!IsStopping)
                return false;

            if (easeOutDuration > 0 && easeOutElapsed < easeInDuration)
            {
                easeOutElapsed += elapsedSeconds;
                if (easeOutElapsed > easeOutDuration)
                    easeOutElapsed = easeOutDuration;

                var t = 1f - (Single)(easeOutElapsed / easeOutDuration);
                BlendingWeight = MathUtil.Clamp(easeOutFunction(t), 0f, 1f);
            }

            return easeOutElapsed >= easeOutDuration;
        }

        // The current animation state for this track.
        private SkinnedAnimationMode currentAnimationMode;
        private Double currentAnimationTime;

        // Easing data.
        private EasingFunction easeInFunction;
        private Double easeInDuration;
        private Double easeInElasped;
        private EasingFunction easeOutFunction;
        private Double easeOutDuration;
        private Double easeOutElapsed;

        // Callbacks.
        private SkinnedAnimationCallbacks? callbacks;
    }
}
