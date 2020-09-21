using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for creating curves which are comprised of <see cref="Vector4"/> values.
    /// </summary>
    public static class Vector4Curve
    {
        /// <summary>
        /// Creates a new curve with step sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector4, CurveKey<Vector4>> Step(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Vector4>> keys) =>
            new Curve<Vector4, CurveKey<Vector4>>(preLoop, postLoop, Vector4CurveStepSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with linear sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector4, CurveKey<Vector4>> Linear(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Vector4>> keys) =>
            new Curve<Vector4, CurveKey<Vector4>>(preLoop, postLoop, Vector4CurveLinearSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with cubic spline sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CubicSplineCurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Vector4, CubicSplineCurveKey<Vector4>> CubicSpline(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CubicSplineCurveKey<Vector4>> keys) =>
            new Curve<Vector4, CubicSplineCurveKey<Vector4>>(preLoop, postLoop, Vector4CurveCubicSplineSampler.Instance, keys);

    }
}
