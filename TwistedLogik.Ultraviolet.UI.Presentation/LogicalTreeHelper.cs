using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains helper methods for interacting with the logical tree.
    /// </summary>
    public static class LogicalTreeHelper
    {
        /// <summary>
        /// Gets the parent of the specified logical object.
        /// </summary>
        /// <param name="dobj">The object for which to retrieve a parent.</param>
        /// <returns>The parent of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetParent(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as UIElement;
            if (element != null)
            {
                return element.Parent;
            }

            return null;
        }

        /// <summary>
        /// Gets the specified logical child of a specified parent object.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="childIndex">The index of the child to retrieve.</param>
        /// <returns>The specified logical child of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetChild(DependencyObject dobj, Int32 childIndex)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.GetLogicalChild(childIndex);
            }

            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the number of logical children belonging to the specified parent.
        /// </summary>
        /// <param name="dobj">The parent object to evaluate.</param>
        /// <returns>The number of logical children belonging to <paramref name="dobj"/>.</returns>
        public static Int32 GetChildrenCount(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.LogicalChildrenCount;
            }

            return 0;
        }
    }
}
