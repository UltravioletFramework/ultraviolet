
namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Specifies how an animated value is calculated when the animation is outside of
    /// its active period; that is, when the storyboard time is past the end of the animation.
    /// </summary>
    public enum FillBehavior
    {
        /// <summary>
        /// The animation's final value is held until the storyboard is stopped
        /// or the animation re-enters its active period.
        /// </summary>
        HoldEnd,

        /// <summary>
        /// The animation's value ceases to be applied while the animation
        /// is outside of its active period, and the underlying value is
        /// used until the animation re-enters its active period.
        /// </summary>
        Stop,
    }
}
