namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents the different modes in which a <see cref="SkinnedAnimationController"/> can play a particular animation.
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
    }
}
