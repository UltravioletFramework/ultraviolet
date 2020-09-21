
namespace Ultraviolet
{
    /// <summary>
    /// Represents the supported types of continuity between a curve's keys.
    /// </summary>
    public enum CurveContinuity
    {
        /// <summary>
        /// The curve transitions using cubic splines.
        /// </summary>
        CubicSpline,

        /// <summary>
        /// The curve transitions in steps.
        /// </summary>
        Step,

        /// <summary>
        /// The curse transitions linearly from one key to the next.
        /// </summary>
        Linear,
    }
}
