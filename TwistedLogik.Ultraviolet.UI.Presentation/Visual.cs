using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the base class for all visually rendered objects
    /// in the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract class Visual : DependencyObject
    {
        /// <summary>
        /// Performs a hit test against this and returns the topmost descendant
        /// which contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTest(Point2D point)
        {
            return HitTestCore(point);
        }

        /// <summary>
        /// Invokes the <see cref="OnVisualParentChanged()"/> method.
        /// </summary>
        internal virtual void OnVisualParentChangedInternal()
        {
            OnVisualParentChanged();
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
                throw new InvalidOperationException(PresentationStrings.VisualAlreadyHasAParent);

            if (child.visualParent != this)
            {
                child.visualParent = this;
                child.OnVisualParentChangedInternal();
            }
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
                child.OnVisualParentChangedInternal();
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
        /// Occurs when the object's visual parent is changed.
        /// </summary>
        protected virtual void OnVisualParentChanged()
        {

        }

        /// <summary>
        /// When overridden in a derived class, performs a hit test against this and returns the topmost descendant
        /// which contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> which contains the specified point, or <c>null</c>.</returns>
        protected virtual Visual HitTestCore(Point2D point)
        {
            return null;
        }

        // State values.
        private Visual visualParent;
    }
}
