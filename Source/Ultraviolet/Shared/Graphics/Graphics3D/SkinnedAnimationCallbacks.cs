using System;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the method that is called in response to an animation event.
    /// </summary>
    /// <param name="model">The skinned model that is being animated.</param>
    /// <param name="state">A custom state object that is passed to the callback.</param>
    public delegate void SkinnedAnimationCallback(SkinnedModelInstance model, Object state);

    /// <summary>
    /// Represents the method that is called in response to an animation being advanced.
    /// </summary>
    /// <param name="model">The skinned model that is being animated.</param>
    /// <param name="timePrevious">The animation's time as of the previous update.</param>
    /// <param name="timeCurrent">The animation's time as of the current update.</param>
    /// <param name="state">A custom state object that is passed to the callback.</param>
    public delegate void SkinnedAnimationAdvancedCallback(SkinnedModelInstance model, Double timePrevious, Double timeCurrent, Object state);

    /// <summary>
    /// Represents the set of callbacks which can be invoked in the process of running a skinned animation.
    /// </summary>
    public struct SkinnedAnimationCallbacks
    {
        /// <summary>
        /// The callback which is invoked when the animation is stopped.
        /// </summary>
        public SkinnedAnimationCallback OnStopped;

        /// <summary>
        /// The custom state object which is passed to the <see cref="OnStopped"/> delegate.
        /// </summary>
        public Object OnStoppedState;

        /// <summary>
        /// The callback which is invoked when the animation state is advanced.
        /// </summary>
        public SkinnedAnimationAdvancedCallback OnAdvanced;

        /// <summary>
        /// The custom state object which is passed to the <see cref="OnAdvanced"/> delegate.
        /// </summary>
        public Object OnAdvancedState;
    }
}
