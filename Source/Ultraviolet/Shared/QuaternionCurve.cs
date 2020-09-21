using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for creating curves which are comprised of <see cref="Quaternion"/> values.
    /// </summary>
    public static class QuaternionCurve
    {
        /// <summary>
        /// Creates a new curve with step sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Quaternion, CurveKey<Quaternion>> Step(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Quaternion>> keys) =>
            new Curve<Quaternion, CurveKey<Quaternion>>(preLoop, postLoop, QuaternionCurveStepSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with linear sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Quaternion, CurveKey<Quaternion>> Linear(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Quaternion>> keys) =>
            new Curve<Quaternion, CurveKey<Quaternion>>(preLoop, postLoop, QuaternionCurveLinearSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with cubic spline sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CubicSplineCurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Quaternion, CubicSplineCurveKey<Quaternion>> CubicSpline(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CubicSplineCurveKey<Quaternion>> keys) =>
            new Curve<Quaternion, CubicSplineCurveKey<Quaternion>>(preLoop, postLoop, QuaternionCurveCubicSplineSampler.Instance, keys);
    }
}
