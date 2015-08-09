using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
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
        /// <param name="point">The point to evaluate.</param>
        /// <returns><c>true</c> if the specified point is a potential hit; otherwise, <c>false</c>.</returns>
        public static Boolean IsPotentialHit(UIElement element, Point2D point)
        {
            if (!element.IsHitTestVisible)
                return false;

            if (element.HasTransformedDescendants)
            {
                var clip = element.ClipRectangle;
                if (clip.HasValue)
                {
                    var absoluteClip = clip.Value;
                    var relativeClip = new RectangleD(
                        absoluteClip.X - element.AbsolutePosition.X, 
                        absoluteClip.Y - element.AbsolutePosition.Y, 
                        absoluteClip.Width, 
                        absoluteClip.Height);

                    if (!relativeClip.Contains(point))
                        return false;
                }
            }
            else
            {
                if (!element.Bounds.Contains(point))
                    return false;
            }

            return true;
        }
    }
}
