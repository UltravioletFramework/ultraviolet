
namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the playback state of a <see cref="Clock"/> instance.
    /// </summary>
    public enum ClockState
    {
        /// <summary>
        /// The clock is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The clock is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The clock is paused.
        /// </summary>
        Paused,
    }
}
