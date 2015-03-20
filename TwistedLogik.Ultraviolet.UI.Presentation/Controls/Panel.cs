using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element with child elements.
    /// </summary>
    public abstract class Panel : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Panel(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.children = new UIElementCollection(this, this);
        }

        /// <summary>
        /// Gets the panel's collection of children.
        /// </summary>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <inheritdoc/>
        protected internal override void RemoveLogicalChild(UIElement child)
        {
            if (child != null)
            {
                children.Remove(child);
            }
            base.RemoveLogicalChild(child);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            Contract.EnsureRange(childIndex >= 0 && childIndex < children.Count + 1, "childIndex");

            if (ComponentRoot != null)
            {
                if (childIndex == 0)
                {
                    return ComponentRoot;
                }
                childIndex--;
            }
            return children[childIndex];
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            return GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get 
            {
                return base.LogicalChildrenCount + children.Count;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get
            {
                return base.VisualChildrenCount + children.Count;
            }
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            if (!IsHitTestVisible || !Bounds.Contains(point))
                return null;

            var childMatch = HitTestChildren(point);
            if (childMatch != null)
            {
                return childMatch;
            }

            return this;
        }

        /// <summary>
        /// Performs a hit test against the panel's children and returns the topmost
        /// child which contains the specified point.
        /// </summary>
        /// <param name="point">The point in element space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> child which contains the specified point, or <c>null</c>.</returns>
        protected virtual Visual HitTestChildren(Point2D point)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                var childMatch = child.HitTest(point - child.RelativeBounds.Location);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipCore()
        {
            foreach (var child in children)
            {
                if (child.RelativeBounds.Left < 0 || child.RelativeBounds.Top < 0 ||
                    child.RelativeBounds.Right > RenderSize.Width || child.RelativeBounds.Bottom > RenderSize.Height)
                {
                    return AbsoluteBounds;
                }
            }
            return null;
        }
        
        // Property values.
        private readonly UIElementCollection children;
    }
}
