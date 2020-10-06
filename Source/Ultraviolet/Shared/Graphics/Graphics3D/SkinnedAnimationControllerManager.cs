using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Manages a collection of <see cref="SkinnedAnimationController"/> instances which are associated
    /// with a particular skinned model instance.
    /// </summary>
    internal class SkinnedAnimationControllerManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedAnimationControllerManager"/> class.
        /// </summary>
        /// <param name="model">The model instance that owns this manager.</param>
        /// <param name="maxSimultaneousAnimations">The maximum number of animations that can be played simultaneously.</param>
        public SkinnedAnimationControllerManager(SkinnedModelInstance model, Int32 maxSimultaneousAnimations)
        {
            Contract.Require(model, nameof(model));

            this.controllers = new SkinnedAnimationController[maxSimultaneousAnimations];
            this.ordering = new Int64[maxSimultaneousAnimations];

            for (var i = 0; i < maxSimultaneousAnimations; i++)
                this.controllers[i] = new SkinnedAnimationController(model);
        }

        /// <summary>
        /// Updates the controller manager's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public void Update(UltravioletTime time)
        {
            for (var i = 0; i < controllers.Length; i++)
                controllers[i].Update(time);
        }

        /// <summary>
        /// Resets all of the manager's animations.
        /// </summary>
        public void ResetAnimations()
        {
            foreach (var controller in controllers)
                controller.Reset();
        }

        /// <summary>
        /// Plays the specified animation. If the animation is already playing,
        /// it will be restarted using the specified mode.
        /// </summary>
        /// <param name="mode">A <see cref="SkinnedAnimationMode"/> value which describes the animation mode.</param>
        /// <param name="animation">The animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <returns>The <see cref="SkinnedAnimationController"/> which is playing the animation, or <see langword="null"/> if the animation could not be played.</returns>
        public SkinnedAnimationController PlayAnimation(SkinnedAnimationMode mode, SkinnedAnimation animation, Single speedMultiplier)
        {
            var controllerAllocation = AllocateController(animation);
            var controller = controllerAllocation.Value;
            controller.Play(mode, animation, speedMultiplier);
            ordering[controllerAllocation.Key] = ++orderingCounter;
            return controller;
        }

        /// <summary>
        /// Stops the specified animation.
        /// </summary>
        /// <param name="animation">The animation to stop.</param>
        /// <returns>The <see cref="SkinnedAnimationController"/> which was playing the animation, or <see langword="null"/> if the animation was not being played.</returns>
        public SkinnedAnimationController StopAnimation(SkinnedAnimation animation)
        {
            var controller = GetControllerForAnimation(animation);
            if (controller != null)
            {
                controller.Stop();
                return controller;
            }
            return null;
        }

        /// <summary>
        /// Gets the controller that is currently playing the specified animation.
        /// </summary>
        /// <param name="animation">The animation to evaluate.</param>
        /// <returns>The <see cref="SkinnedAnimationController"/> which is currently playing the specified 
        /// animation, or <see langword="null"/> if no controller is playing the animation.</returns>
        public SkinnedAnimationController GetControllerForAnimation(SkinnedAnimation animation)
        {
            return GetControllerForAnimationInternal(animation).Value;
        }

        /// <summary>
        /// Gets the controller that is currently playing the specified animation.
        /// </summary>
        private KeyValuePair<Int32, SkinnedAnimationController> GetControllerForAnimationInternal(SkinnedAnimation animation)
        {
            for (var i = 0; i < controllers.Length; i++)
            {
                var controller = controllers[i];
                if (controller.IsPlayingAnimation(animation))
                {
                    return new KeyValuePair<Int32, SkinnedAnimationController>(i, controller);
                }
            }
            return new KeyValuePair<Int32, SkinnedAnimationController>(-1, null);
        }

        /// <summary>
        /// Allocates a controller to play the specified animation.
        /// </summary>
        private KeyValuePair<Int32, SkinnedAnimationController> AllocateController(SkinnedAnimation animation)
        {
            var existing = GetControllerForAnimationInternal(animation);
            if (existing.Value != null)
                return existing;

            // If no existing controller, find one that isn't playing.
            var leastRecentlyPlayed = default(SkinnedAnimationController);
            var leastRecentlyPlayedIndex = -1;
            var leastRecentlyPlayedOrder = Int64.MaxValue;
            for (var i = 0; i < controllers.Length; i++)
            {
                var controller = controllers[i];
                var order = ordering[i];

                if (leastRecentlyPlayed == null || order < leastRecentlyPlayedOrder)
                {
                    leastRecentlyPlayed = controller;
                    leastRecentlyPlayedIndex = i;
                    leastRecentlyPlayedOrder = order;
                }

                if (!controller.IsPlaying)
                    return new KeyValuePair<Int32, SkinnedAnimationController>(i, controller);
            }

            // If no stopped controller, override the one that was least recently played.
            return new KeyValuePair<Int32, SkinnedAnimationController>(leastRecentlyPlayedIndex, leastRecentlyPlayed);
        }

        // The controllers which are managed by this instance.
        private readonly SkinnedAnimationController[] controllers;
        private readonly Int64[] ordering;
        private Int32 orderingCounter;
    }
}
