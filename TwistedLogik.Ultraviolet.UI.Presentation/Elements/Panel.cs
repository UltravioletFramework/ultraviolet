using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
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
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Panel(UltravioletContext uv, String id)
            : base(uv, id)
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

        /// <inheritdoc/>
        protected override UIElement GetElementAtPointCore(Double x, Double y, Boolean isHitTest)
        {
            if (!Bounds.Contains(x, y))
                return null;

            var childMatch = GetChildAtPoint(x, y, isHitTest);
            if (childMatch != null)
            {
                return childMatch;
            }

            return this;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavUp(UIElement current)
        {
            if (current == null)
                return null;

            var bounds = current.AbsoluteBounds;
            var query  = new RectangleD(bounds.X, 0, bounds.Width, bounds.Top);
            var result = default(UIElement);

            foreach (var child in Children)
            {
                if (child == current || !LayoutUtil.IsValidForNav(child))
                    continue;

                if (child.AbsoluteBounds.Y < current.AbsoluteBounds.Y && child.AbsoluteBounds.Intersects(query))
                {
                    if (result == null || (child.AbsoluteBounds.Top > result.AbsoluteBounds.Top))
                    {
                        result = child;
                    }
                }
            }
            return result;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavDown(UIElement current)
        {
            if (current == null)
                return null;

            var bounds = current.AbsoluteBounds;
            var query  = new RectangleD(bounds.X, bounds.Bottom, bounds.Width, Double.PositiveInfinity);
            var result = default(UIElement);

            foreach (var child in Children)
            {
                if (child == current || !LayoutUtil.IsValidForNav(child))
                    continue;

                if (child.AbsoluteBounds.Y > current.AbsoluteBounds.Y && child.AbsoluteBounds.Intersects(query))
                {
                    if (result == null || (child.AbsoluteBounds.Top < result.AbsoluteBounds.Top))
                    {
                        result = child;
                    }
                }
            }
            return result;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavLeft(UIElement current)
        {
            if (current == null)
                return null;

            var bounds = current.AbsoluteBounds;
            var query  = new RectangleD(0, bounds.Top, bounds.Left, bounds.Height);
            var result = default(UIElement);

            foreach (var child in Children)
            {
                if (child == current || !LayoutUtil.IsValidForNav(child))
                    continue;

                if (child.AbsoluteBounds.X < current.AbsoluteBounds.X && child.AbsoluteBounds.Intersects(query))
                {
                    if (result == null || (child.AbsoluteBounds.Left > result.AbsoluteBounds.Left))
                    {
                        result = child;
                    }
                }
            }
            return result;
        }

        /// <inheritdoc/>
        protected override UIElement GetNextNavRight(UIElement current)
        {
            if (current == null)
                return null;

            var bounds = current.AbsoluteBounds;
            var query  = new RectangleD(bounds.Right, bounds.Top, Double.PositiveInfinity, bounds.Height);
            var result = default(UIElement);

            foreach (var child in Children)
            {
                if (child == current || !LayoutUtil.IsValidForNav(child))
                    continue;

                if (child.AbsoluteBounds.X > current.AbsoluteBounds.X && child.AbsoluteBounds.Intersects(query))
                {
                    if (result == null || (child.AbsoluteBounds.Left < result.AbsoluteBounds.Left))
                    {
                        result = child;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the child element at the specified device-independent coordinates relative to this element's bounds.
        /// </summary>
        /// <param name="x">The element-relative x-coordinate of the point to evaluate.</param>
        /// <param name="y">The element-relative y-coordinate of the point to evaluate.</param>
        /// <param name="isHitTest">A value indicating whether this test should respect the value of the <see cref="UIElement.IsHitTestVisible"/> property.</param>
        /// <returns>The child element at the specified coordinates, or <c>null</c> if no such element exists.</returns>
        protected virtual UIElement GetChildAtPoint(Double x, Double y, Boolean isHitTest)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                var childRelX = x - child.RelativeBounds.X;
                var childRelY = y - child.RelativeBounds.Y;

                var childMatch = child.GetElementAtPoint(childRelX, childRelY, isHitTest);
                if (childMatch != null)
                {
                    return childMatch;
                }
            }
            return null;
        }
        
        // Property values.
        private readonly UIElementCollection children;
    }
}
