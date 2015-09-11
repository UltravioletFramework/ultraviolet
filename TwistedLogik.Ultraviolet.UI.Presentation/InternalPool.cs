namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the Presentation Foundation's internal collection of object pools.
    /// </summary>
    public enum InternalPool
    {
        /// <summary>
        /// The pool which contains instances of the <see cref="Animations.SimpleClock"/> class used by the animation system.
        /// </summary>
        SimpleClocks,

        /// <summary>
        /// The pool which contains instances of the <see cref="Animations.StoryboardInstance"/> class used by the animation system.
        /// </summary>
        StoryboardInstances,

        /// <summary>
        /// The pool which contains instances of the <see cref="Animations.StoryboardClock"/> class used by the animation system.
        /// </summary>
        StoryboardClocks,

        /// <summary>
        /// The set of pools used by the out-of-band renderer, which is responsible for rendering elements with shader effects.
        /// </summary>
        OutOfBandRenderer,
    }
}
