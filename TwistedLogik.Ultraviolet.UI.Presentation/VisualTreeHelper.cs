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
        /// Performs a hit test against the specified visual and returns the topmost descendant
        /// which contains the specified point.
        /// </summary>
        /// <param name="reference">The visual on which to perform a hit test.</param>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> which contains the specified point, or <c>null</c>.</returns>
        public static Visual HitTest(Visual reference, Point2D point)
        {
            Contract.Require(reference, "reference");

            return reference.HitTest(point);
        }

        /// <summary>
        /// Performs an action for each of the specified object's visual children.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's visual children.</param>
        public static void ForEachChild(DependencyObject dobj, Object state, Action<DependencyObject, Object> action)
        {
            ForEachChild<DependencyObject>(dobj, state, action);
        }

        /// <summary>
        /// Performs an action for each of the specified object's visual children which are of the specified type.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's visual children.</param>
        public static void ForEachChild<T>(DependencyObject dobj, Object state, Action<T, Object> action) where T : class
        {
            Contract.Require(dobj, "dobj");
            Contract.Require(action, "action");

            var children = GetChildrenCount(dobj);
            for (int i = 0; i < children; i++)
            {
                var child = GetChild(dobj, i) as T;
                if (child != null)
                {
                    action(child, state);
                }
            }
        }

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
