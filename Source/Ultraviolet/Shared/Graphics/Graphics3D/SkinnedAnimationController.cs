using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the state of a particular <see cref="SkinnedAnimation"/> as applied to one <see cref="SkinnedModelInstance"/>.
    /// </summary>
    public class SkinnedAnimationController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedAnimationController"/> class.
        /// </summary>
        /// <param name="model">The model that owns this controller.</param>
        public SkinnedAnimationController(SkinnedModelInstance model)
        {
            Contract.Require(model, nameof(model));

            this.Model = model;
        }

        /// <summary>
        /// Updates the animation controller's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.Require(time, nameof(time));

            if (!IsPlaying || IsPaused)
                return;

            switch (currentAnimationMode)
            {
                case SkinnedAnimationMode.Loop:
                    {
                        var updatedAnimationTime = (currentAnimationTime + time.ElapsedTime.TotalSeconds) % currentAnimation.Duration;
                        currentAnimationTime = updatedAnimationTime;
                    }
                    break;

                case SkinnedAnimationMode.FireAndForget:
                    {
                        var updatedAnimationTime = (currentAnimationTime + time.ElapsedTime.TotalSeconds);
                        if (updatedAnimationTime >= currentAnimation.Duration)
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

            UpdateAnimationState();
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="mode">The animation mode.</param>
        /// <param name="animation">The animation to play.</param>
        public void Play(SkinnedAnimationMode mode, SkinnedAnimation animation)
        {
            Contract.Require(animation, nameof(animation));

            this.currentAnimationMode = mode;
            this.currentAnimation = animation;
            this.currentAnimationTime = 0.0;
        }

        /// <summary>
        /// Stops the currently playing animation.
        /// </summary>
        public void Stop()
        {
            this.currentAnimationMode = SkinnedAnimationMode.Loop;
            this.currentAnimation = null;
            this.currentAnimationTime = 0.0;
        }

        /// <summary>
        /// Resets the currently playing animation to the beginning.
        /// </summary>
        public void Reset()
        {
            currentAnimationTime = 0.0;
        }

        /// <summary>
        /// Gets the <see cref="SkinnedModelInstance"/> that owns this controller.
        /// </summary>
        public SkinnedModelInstance Model { get; }

        /// <summary>
        /// Gets a value indicating whether the controller is currently playing the specified animation.
        /// </summary>
        /// <param name="animation">The animation to evaluate.</param>
        /// <returns><see langword="true"/> if the controller is currently playing the specified animation; otherwise, <see langword="false"/>.</returns>
        public Boolean IsPlayingAnimation(SkinnedAnimation animation)
        {
            Contract.Require(animation, nameof(animation));

            return animation == currentAnimation;
        }

        /// <summary>
        /// Gets a value indicating whether the controller is currently playing an animation.
        /// </summary>
        public Boolean IsPlaying => currentAnimation != null;

        /// <summary>
        /// Gets or sets a value indicating whether the animation is currently paused.
        /// </summary>
        public Boolean IsPaused { get; set; }

        /// <summary>
        /// Updates the animation state for all affected nodes.
        /// </summary>
        private void UpdateAnimationState()
        {
            Model.TraverseNodes((node, state) =>
            {
                var controller = (SkinnedAnimationController)state;
                var controllerTime = controller.currentAnimationTime;

                var nodeAnimation = controller.currentAnimation.GetNodeAnimation(node.Template.LogicalIndex);
                if (nodeAnimation != null)
                {
                    node.UpdateAnimationState(nodeAnimation, controllerTime);
                }
            }, this);
        }

        // The current animation state for this controller.
        private SkinnedAnimationMode currentAnimationMode;
        private SkinnedAnimation currentAnimation;
        private Double currentAnimationTime;
    }
}
