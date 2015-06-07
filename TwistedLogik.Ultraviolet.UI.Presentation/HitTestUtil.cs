using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains helper methods for performing hit testing.
    /// </summary>
    internal static class HitTestUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified point is potentially a hit for the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <param name="point">The point to evaluate. This point will be transformed into the element's render space.</param>
        /// <returns><c>true</c> if the specified point is a potential hit; otherwise, <c>false</c>.</returns>
        public static Boolean IsPotentialHit(UIElement element, ref Point2D point)
        {
            if (!element.IsHitTestVisible)
                return false;

            Point2D transformedPoint;
            if (!element.PointToRender(point, out transformedPoint))
                return false;

            if (!element.Bounds.Contains(transformedPoint))
                return false;

            point = transformedPoint;
            return true;
        }
    }
}
