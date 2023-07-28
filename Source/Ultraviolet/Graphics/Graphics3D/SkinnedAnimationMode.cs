namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the different modes in which a <see cref="SkinnedAnimationTrack"/> can play a particular animation.
    /// </summary>
    public enum SkinnedAnimationMode
    {
        /// <summary>
        /// Loop the animation continuously.
        /// </summary>
        Loop,

        /// <summary>
        /// Play the animation once and then stop.
        /// </summary>
        FireAndForget,

        /// <summary>
        /// Update the animation manually.
        /// </summary>
        Manual,
    }
}
