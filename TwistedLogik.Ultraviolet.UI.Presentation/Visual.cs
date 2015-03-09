using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the base class for all visually rendered objects
    /// in the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract class Visual : StyledDependencyObject
    {
        /// <summary>
        /// Determines whether the specified point falls within this visual object and, if so,
        /// returns the topmost visual object which contains the point, which may be a descendant of this object.
        /// </summary>
        /// <param name="pt">The point to evaluate.</param>
        /// <returns>The visual object which contains the point, or <c>null</c> if no descendant of this object contains the point.</returns>
        public Visual HitTest(Point2D pt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the object's visual parent.
        /// </summary>
        internal Visual VisualParent
        {
            get { return visualParent; }
        }

        /// <summary>
        /// Adds a visual as a child of this object.
        /// </summary>
        /// <param name="child">The child object to add to this object.</param>
        protected internal void AddVisualChild(Visual child)
        {
            Contract.Require(child, "child");

            if (child.visualParent != null)
                throw new InvalidOperationException("TODO");

            child.visualParent = this;
        }

        /// <summary>
        /// Removes a visual child from this object.
        /// </summary>
        /// <param name="child">The child object to remove from this object.</param>
        protected internal void RemoveVisualChild(Visual child)
        {
            Contract.Require(child, "child");

            if (child.visualParent == this)
            {
                child.visualParent = null;
            }
        }

        /// <summary>
        /// Gets the specified visual child of this element.
        /// </summary>
        /// <param name="childIndex">The index of the visual child to retrieve.</param>
        /// <returns>The visual child of this element with the specified index.</returns>
        protected internal virtual UIElement GetVisualChild(Int32 childIndex)
        {
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the number of visual children which belong to this element.
        /// </summary>
        protected internal virtual Int32 VisualChildrenCount
        {
            get { return 0; }
        }

        /// <summary>
        /// When overridden in a derived class, determines whether the specified point falls within this visual object and, if so,
        /// returns the topmost visual object which contains the point, which may be a descendant of this object.
        /// </summary>
        /// <param name="pt">The point to evaluate.</param>
        /// <returns>The visual object which contains the point, or <c>null</c> if no descendant of this object contains the point.</returns>
        protected virtual Visual HitTestCore(Point2D pt)
        {
            throw new NotImplementedException();
        }

        // State values.
        private Visual visualParent;
    }
}
