using System;

namespace Ultraviolet.Presentation.Media
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
        /// <returns><see langword="true"/> if the specified point is a potential hit; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsPotentialHit(UIElement element, Point2D point)
        {
            if (element.Visibility != Visibility.Visible)
                return false;

            if (!element.IsHitTestVisible)
                return false;

            if (!element.VisualBounds.Contains(point))
                return false;

            var clip = element.ClipRectangle;
            if (clip.HasValue)
            {
                var absoluteClip = clip.Value;
                var relativeClip = new RectangleD(
                    absoluteClip.X - element.UntransformedAbsolutePosition.X,
                    absoluteClip.Y - element.UntransformedAbsolutePosition.Y,
                    absoluteClip.Width,
                    absoluteClip.Height);

                if (!relativeClip.Contains(point))
                    return false;
            }

            return true;
        }
    }
}
