
namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the supported types of continuity between a curve's keys.
    /// </summary>
    public enum CurveContinuity
    {
        /// <summary>
        /// The curve transitions smoothly from one key to the next.
        /// </summary>
        Smooth,

        /// <summary>
        /// The curve transitions in steps.
        /// </summary>
        Step,
    }
}
