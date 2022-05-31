using System;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an animated instance of a <see cref="SkinnedModel"/>. Each instance represents a particular 
    /// skinned animation at a particular point in time.
    /// </summary>
    public class SkinnedModelInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedModelInstance"/> class.
        /// </summary>
        /// <param name="template">The <see cref="SkinnedModel"/> which serves as this instance's template.</param>
        /// <param name="tracks">The number of animation tracks to allocate for the model instance.</param>
        public SkinnedModelInstance(SkinnedModel template, Int32 tracks = 4)
        {
            Contract.Require(template, nameof(template));
            Contract.EnsureRange(tracks >= 1, nameof(tracks));

            this.Template = template;
            this.Skins = new SkinInstanceCollection(template.Skins.Select(x => new SkinInstance(x, this)));
            this.Scenes = new SkinnedModelSceneInstanceCollection(template.Scenes.Select(x => new SkinnedModelSceneInstance(x, this)), 
                template.Scenes.DefaultScene.LogicalIndex);

            this.controller = new SkinnedAnimationController(this, tracks);
            this.nodeManager = new SkinnedModelInstanceNodeManager(this);
        }

        /// <summary>
        /// Advances time for the model's animation tracks.
        /// </summary>
        /// <param name="elapsedSeconds">The number of seconds by which to advance the model's animation tracks.</param>
        /// <returns><see langword="true"/> if advancing time caused the model's animation state to change; otherwise, <see langword="false"/>.</returns>
        public void AdvanceTime(Double elapsedSeconds)
        {
            if (controller.AdvanceTime(elapsedSeconds))
            {
                foreach (var skin in Skins)
                    skin.Update();
            }
            else
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Updates the model instance's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.Require(time, nameof(time));

            if (controller.Update(time))
            {
                foreach (var skin in Skins)
                    skin.Update();
            }
        }

        /// <summary>
        /// Updates the model instance's animation state.
        /// </summary>
        public void UpdateAnimationState()
        {
            controller.UpdateAnimationState();

            foreach (var skin in Skins)
                skin.Update();
        }

        /// <summary>
        /// Resets all of the instance's animations.
        /// </summary>
        public void ResetAnimations()
        {
            controller.ResetAnimations();
        }

        /// <summary>
        /// Plays the specified animation on this instance, if it exists. If the animation is already playing,
        /// it will be restarted using the specified mode.
        /// </summary>
        /// <param name="mode">A <see cref="SkinnedAnimationMode"/> value which describes the animation mode.</param>
        /// <param name="animationName">The name of the animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the animation, or <see langword="null"/> if the animation could not be played.</returns>
        public SkinnedAnimationTrack PlayAnimation(SkinnedAnimationMode mode, String animationName, Single speedMultiplier = 1.0f, SkinnedAnimationCallbacks? callbacks = null)
        {
            Contract.Require(animationName, nameof(animationName));

            var animation = Template.Animations.TryGetAnimationByName(animationName);
            if (animation == null)
                return null;

            return controller.PlayAnimation(mode, animation, speedMultiplier, callbacks);
        }

        /// <summary>
        /// Plays the specified animation on this instance, if it exists. If the animation is already playing,
        /// it will be restarted using the specified mode.
        /// </summary>
        /// <param name="mode">A <see cref="SkinnedAnimationMode"/> value which describes the animation mode.</param>
        /// <param name="animationName">The name of the animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="easeInFunction">The easing function to apply when easing in the animation.</param>
        /// <param name="easeInDuration">The number of seconds over which to ease in the animation.</param>
        /// <param name="easeOutFunction">The easing function to apply when easing out the animation.</param>
        /// <param name="easeOutDuration">The number of seconds over which to ease out the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the animation, or <see langword="null"/> if the animation could not be played.</returns>
        public SkinnedAnimationTrack PlayAnimation(SkinnedAnimationMode mode, String animationName, Single speedMultiplier,
            EasingFunction easeInFunction, Double easeInDuration, EasingFunction easeOutFunction, Double easeOutDuration, SkinnedAnimationCallbacks? callbacks = null)
        {
            Contract.Require(animationName, nameof(animationName));

            var animation = Template.Animations.TryGetAnimationByName(animationName);
            if (animation == null)
                return null;

            return controller.PlayAnimation(mode, animation, speedMultiplier,
                easeInFunction, easeInDuration, easeOutFunction, easeOutDuration, callbacks);
        }

        /// <summary>
        /// Plays the specified animation on this instance, if it exists. If the animation is already playing,
        /// it will be restarted using the specified mode.
        /// </summary>
        /// <param name="mode">A <see cref="SkinnedAnimationMode"/> value which describes the animation mode.</param>
        /// <param name="animationIndex">The index of the animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the animation, or <see langword="null"/> if the animation could not be played.</returns>
        public SkinnedAnimationTrack PlayAnimation(SkinnedAnimationMode mode, Int32 animationIndex, Single speedMultiplier = 1.0f, SkinnedAnimationCallbacks? callbacks = null)
        {
            Contract.EnsureRange(animationIndex >= 0, nameof(animationIndex));

            if (animationIndex >= Template.Animations.Count)
                return null;

            var animation = Template.Animations[animationIndex];
            return controller.PlayAnimation(mode, animation, speedMultiplier, callbacks);
        }

        /// <summary>
        /// Plays the specified animation on this instance, if it exists. If the animation is already playing,
        /// it will be restarted using the specified mode.
        /// </summary>
        /// <param name="mode">A <see cref="SkinnedAnimationMode"/> value which describes the animation mode.</param>
        /// <param name="animationIndex">The index of the animation to play.</param>
        /// <param name="speedMultiplier">The relative speed at which to play the animation.</param>
        /// <param name="easeInFunction">The easing function to apply when easing in the animation.</param>
        /// <param name="easeInDuration">The number of seconds over which to ease in the animation.</param>
        /// <param name="easeOutFunction">The easing function to apply when easing out the animation.</param>
        /// <param name="easeOutDuration">The number of seconds over which to ease out the animation.</param>
        /// <param name="callbacks">The set of callbacks to invoke for this animation.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the animation, or <see langword="null"/> if the animation could not be played.</returns>
        public SkinnedAnimationTrack PlayAnimation(SkinnedAnimationMode mode, Int32 animationIndex, Single speedMultiplier,
            EasingFunction easeInFunction, Double easeInDuration, EasingFunction easeOutFunction, Double easeOutDuration, SkinnedAnimationCallbacks? callbacks = null)
        {
            Contract.EnsureRange(animationIndex >= 0, nameof(animationIndex));

            if (animationIndex >= Template.Animations.Count)
                return null;

            var animation = Template.Animations[animationIndex];
            return controller.PlayAnimation(mode, animation, speedMultiplier, 
                easeInFunction, easeInDuration, easeOutFunction, easeOutDuration, callbacks);
        }

        /// <summary>
        /// Stops the specified animation on this instance, if it is playing.
        /// </summary>
        /// <param name="animationName">The name of the animation to stop playing.</param>
        /// <returns><see langword="true"/> if the animation was stopped; otherwise, <see langword="false"/>.</returns>
        public Boolean StopAnimation(String animationName)
        {
            Contract.Require(animationName, nameof(animationName));

            var animation = Template.Animations.TryGetAnimationByName(animationName);
            if (animation == null)
                return false;

            return controller.StopAnimation(animation) != null;
        }

        /// <summary>
        /// Immediately stops the specified animation on this instance, if it is playing, without performing any easing.
        /// </summary>
        /// <param name="animationName">The name of the animation to stop playing.</param>
        /// <returns><see langword="true"/> if the animation was stopped; otherwise, <see langword="false"/>.</returns>
        public Boolean StopAnimationImmediate(String animationName)
        {
            Contract.Require(animationName, nameof(animationName));

            var animation = Template.Animations.TryGetAnimationByName(animationName);
            if (animation == null)
                return false;

            return controller.StopAnimationImmediate(animation) != null;
        }

        /// <summary>
        /// Stops the specified animation on this instance, if it is playing.
        /// </summary>
        /// <param name="animationIndex">The index of the animation to stop playing.</param>
        /// <returns><see langword="true"/> if the animation was stopped; otherwise, <see langword="false"/>.</returns>
        public Boolean StopAnimation(Int32 animationIndex)
        {
            Contract.EnsureRange(animationIndex >= 0, nameof(animationIndex));

            if (animationIndex >= Template.Animations.Count)
                return false;

            var animation = Template.Animations[animationIndex];
            return controller.StopAnimation(animation) != null;
        }

        /// <summary>
        /// Immediately stops the specified animation on this instance, if it is playing, without performing any easing.
        /// </summary>
        /// <param name="animationIndex">The index of the animation to stop playing.</param>
        /// <returns><see langword="true"/> if the animation was stopped; otherwise, <see langword="false"/>.</returns>
        public Boolean StopAnimationImmediate(Int32 animationIndex)
        {
            Contract.EnsureRange(animationIndex >= 0, nameof(animationIndex));

            if (animationIndex >= Template.Animations.Count)
                return false;

            var animation = Template.Animations[animationIndex];
            return controller.StopAnimationImmediate(animation) != null;
        }

        /// <summary>
        /// Gets the <see cref="SkinnedAnimationTrack"/> that is playing the specified animation,
        /// if that animation is currently being played.
        /// </summary>
        /// <param name="animationName">The name of the animation for which to retrieve a controller.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the specified animation,
        /// or <see langword="null"/> if no controller is playing the animation.</returns>
        public SkinnedAnimationTrack GetAnimationController(String animationName)
        {
            Contract.Require(animationName, nameof(animationName));

            var animation = Template.Animations.TryGetAnimationByName(animationName);
            if (animation == null)
                return null;

            return controller.GetTrackForAnimation(animation);
        }

        /// <summary>
        /// Gets the <see cref="SkinnedAnimationTrack"/> that is playing the specified animation,
        /// if that animation is currently being played.
        /// </summary>
        /// <param name="animationIndex">The index of the animation for which to retrieve a controller.</param>
        /// <returns>The <see cref="SkinnedAnimationTrack"/> which is playing the specified animation,
        /// or <see langword="null"/> if no controller is playing the animation.</returns>
        public SkinnedAnimationTrack GetAnimationController(Int32 animationIndex)
        {
            Contract.EnsureRange(animationIndex >= 0, nameof(animationIndex));

            if (animationIndex >= Template.Animations.Count)
                return null;

            var animation = Template.Animations[animationIndex];
            return controller.GetTrackForAnimation(animation);
        }

        /// <summary>
        /// Gets the node instance with the specified logical index.
        /// </summary>
        /// <param name="logicalIndex">The logical index of the node instance to retrieve.</param>
        /// <returns>The <see cref="SkinnedModelNodeInstance"/> with the specified logical index.</returns>
        public SkinnedModelNodeInstance GetNodeInstanceByLogicalIndex(Int32 logicalIndex) => nodeManager.GetNodeInstanceByLogicalIndex(logicalIndex);

        /// <summary>
        /// Performs an action on all nodes in the model.
        /// </summary>
        /// <param name="action">The action to perform on each node.</param>
        /// <param name="state">An arbitrary state objece to pass to <paramref name="action"/>.</param>
        public void TraverseNodes(Action<SkinnedModelNodeInstance, Object> action, Object state)
        {
            Contract.Require(action, nameof(action));

            foreach (var scene in Scenes)
                scene.TraverseNodes(action, state);
        }

        /// <summary>
        /// Gets the <see cref="SkinnedModel"/> which serves as this instance's template.
        /// </summary>
        public SkinnedModel Template { get; }

        /// <summary>
        /// Gets the instance's collection of skins.
        /// </summary>
        public SkinInstanceCollection Skins { get; }

        /// <summary>
        /// Gets the instance's collection of scenes.
        /// </summary>
        public SkinnedModelSceneInstanceCollection Scenes { get; }

        // Internal state trackers.
        private readonly SkinnedAnimationController controller;
        private readonly SkinnedModelInstanceNodeManager nodeManager;
    }
}
