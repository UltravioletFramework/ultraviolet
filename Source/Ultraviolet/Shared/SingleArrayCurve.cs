using System;
using System.Collections.Generic;

namespace Ultraviolet
{
    /// <summary>
    /// Contains methods for creating curves which are comprised of <see cref="Single"/> values.
    /// </summary>
    public static class SingleArrayCurve
    {
        /// <summary>
        /// Creates a new curve with step sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{T}"/> objects from which to construct the curve.</param>
        public static Curve<ArraySegment<Single>, CurveKey<ArraySegment<Single>>> Step(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<ArraySegment<Single>>> keys) =>
            new Curve<ArraySegment<Single>, CurveKey<ArraySegment<Single>>>(preLoop, postLoop, SingleArrayCurveStepSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with linear sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{T}"/> objects from which to construct the curve.</param>
        public static Curve<ArraySegment<Single>, CurveKey<ArraySegment<Single>>> Linear(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<CurveKey<ArraySegment<Single>>> keys) =>
            new Curve<ArraySegment<Single>, CurveKey<ArraySegment<Single>>>(preLoop, postLoop, SingleArrayCurveLinearSampler.Instance, keys);

        /// <summary>
        /// Creates a new curve with smooth (bicubic) sampling.
        /// </summary>
        /// <param name="preLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined 
        /// for points before the beginning of the curve.</param>
        /// <param name="postLoop">A <see cref="CurveLoopType"/> value indicating how the curve's values are determined
        /// for points after the end of the curve.</param>
        /// <param name="keys">A collection of <see cref="CurveKey{T}"/> objects from which to construct the curve.</param>
        public static Curve<ArraySegment<Single>, SmoothCurveKey<ArraySegment<Single>>> Smooth(CurveLoopType preLoop, CurveLoopType postLoop, IEnumerable<SmoothCurveKey<ArraySegment<Single>>> keys) =>
            new Curve<ArraySegment<Single>, SmoothCurveKey<ArraySegment<Single>>>(preLoop, postLoop, SingleArrayCurveSmoothSampler.Instance, keys);
    }
}
