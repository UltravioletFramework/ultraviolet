
namespace Ultraviolet
{
    /// <summary>
    /// Represents the ways that a curve's values can loop after passing its beginning or end.
    /// </summary>
    public enum CurveLoopType
    {
        /// <summary>
        /// The curve's value is constant.
        /// </summary>
        Constant,

        /// <summary>
        /// The curve's values cycle around to the other end of the curve.
        /// </summary>
        Cycle,

        /// <summary>
        /// The curve's values cycle around to the other end of the curve, and are
        /// offset by the difference between the first and last values.
        /// </summary>
        CycleOffset,

        /// <summary>
        /// The curve's values are determined by linear interpolation.
        /// </summary>
        Linear,

        /// <summary>
        /// The curve's values oscillate from one end of the curve to the other.
        /// </summary>
        Oscillate,
    }
}
