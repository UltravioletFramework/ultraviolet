using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains utility methods useful for measuring framework elements.
    /// </summary>
    public static class MeasureUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified element is currently being drawn.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is being drawn; otherwise, false.</returns>
        public static Boolean IsDrawn(UIElement element)
        {
            Contract.Require(element, "element");

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is currently filling space in the layout.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is filling space; otherwise, <c>false</c>.</returns>
        public static Boolean IsSpaceFilling(UIElement element)
        {
            Contract.Require(element, "element");

            return true;
        }

        /// <summary>
        /// Gets the upper and lower bounds of the specified measure value, taking its minimum and
        /// maximum values into account.
        /// </summary>
        /// <param name="measure">The measure value to evaluate.</param>
        /// <param name="min">The measure value's minimum allowed value.</param>
        /// <param name="max">The measure value's maximum allowed value.</param>
        /// <param name="lower">The lower bound of possible values for the specified measure.</param>
        /// <param name="upper">The upper bound of possible values for the specified measure.</param>
        public static void GetBoundedMeasure(Double measure, Double min, Double max, out Double lower, out Double upper)
        {
            upper = Math.Max(Math.Min(Double.IsNaN(measure) ? Double.PositiveInfinity : measure, max), min);
            lower = Math.Max(Math.Min(max, Double.IsNaN(measure) ? 0.0 : measure), min);
        }
    }
}
