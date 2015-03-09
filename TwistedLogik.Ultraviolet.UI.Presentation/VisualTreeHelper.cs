using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains helper methods for interacting with the visual tree.
    /// </summary>
    public static class VisualTreeHelper
    {
        /// <summary>
        /// Gets the parent of the specified visual object.
        /// </summary>
        /// <param name="dobj">The object for which to retrieve a parent.</param>
        /// <returns>The parent of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetParent(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var visual = dobj as Visual;
            if (visual == null)
                throw new ArgumentException(PresentationStrings.NotVisualObject);

            return visual.VisualParent;
        }

        /// <summary>
        /// Gets the specified visual child of a specified parent object.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="childIndex">The index of the child to retrieve.</param>
        /// <returns>The specified visual child of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetChild(DependencyObject dobj, Int32 childIndex)
        {
            Contract.Require(dobj, "dobj");

            var visual = dobj as Visual;
            if (visual == null)
                throw new ArgumentException(PresentationStrings.NotVisualObject);

            return visual.GetVisualChild(childIndex);
        }

        /// <summary>
        /// Gets the number of visual children belonging to the specified parent.
        /// </summary>
        /// <param name="dobj">The parent object to evaluate.</param>
        /// <returns>The number of visual children belonging to <paramref name="dobj"/>.</returns>
        public static Int32 GetChildrenCount(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var visual = dobj as Visual;
            if (visual == null)
                throw new ArgumentException(PresentationStrings.NotVisualObject);

            return visual.VisualChildrenCount;
        }
    }
}
