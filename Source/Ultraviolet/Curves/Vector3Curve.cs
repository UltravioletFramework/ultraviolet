using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for creating curves which are comprised of <see cref="Vector3"/> values.
    /// </summary>
    public static class Vector3Curve
    {
        /// <summary>
        /// Creates a new curve with step sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector3, CurveKey<Vector3>> Step(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Vector3>> keys) =>
            new Curve<Vector3, CurveKey<Vector3>>(preLoop, postLoop, Vector3CurveStepSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with linear sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector3, CurveKey<Vector3>> Linear(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Vector3>> keys) =>
            new Curve<Vector3, CurveKey<Vector3>>(preLoop, postLoop, Vector3CurveLinearSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with cubic spline sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CubicSplineCurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector3, CubicSplineCurveKey<Vector3>> CubicSpline(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CubicSplineCurveKey<Vector3>> keys) =>
            new Curve<Vector3, CubicSplineCurveKey<Vector3>>(preLoop, postLoop, Vector3CurveCubicSplineSampler.Instance, keys);

    }
}
