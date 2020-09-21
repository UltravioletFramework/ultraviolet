using System;
using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for creating curves which are comprised of <see cref="Single"/> values.
    /// </summary>
    public static class SingleCurve
    {
        /// <summary>
        /// Creates a new curve with step sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Single, CurveKey<Single>> Step(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Single>> keys) =>
            new Curve<Single, CurveKey<Single>>(preLoop, postLoop, SingleCurveStepSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with linear sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Single, CurveKey<Single>> Linear(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<Single>> keys) =>
            new Curve<Single, CurveKey<Single>>(preLoop, postLoop, SingleCurveLinearSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with cubic spline sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CubicSplineCurveKey{Single}"/> objects from which to construct the curve.</param>
        public static Curve<Single, CubicSplineCurveKey<Single>> CubicSpline(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CubicSplineCurveKey<Single>> keys) =>
            new Curve<Single, CubicSplineCurveKey<Single>>(preLoop, postLoop, SingleCurveCubicSplineSampler.Instance, keys);
    }
}
