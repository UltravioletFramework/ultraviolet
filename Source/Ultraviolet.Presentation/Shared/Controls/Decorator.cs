using System;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an element which contains a single child element.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Child")]
    public class Decorator : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Decorator"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Decorator(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
        
        /// <summary>
        /// Gets or sets the single child of the <see cref="Decorator"/> instance.
        /// </summary>
        public virtual UIElement Child
        {
            get { return child; }
            set
            {
                if (child == value)
                    return;

                var oldChild = this.child;
                var newChild = value;

                if (oldChild != null)
                    oldChild.ChangeLogicalAndVisualParents(null, null);

                this.child = newChild;

                if (newChild != null)
                    newChild.ChangeLogicalAndVisualParents(HooksChildIntoLogicalTree ? this : Parent, this);

                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (child != null)
            {
                child.Measure(availableSize);
                return child.DesiredSize;
            }
            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (child != null)
            {
                child.Arrange(new RectangleD(Point2D.Zero, finalSize), options);
            }
            return finalSize;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            if (child != null && childIndex == 0)
            {
                return child;
            }
            return base.GetVisualChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            if (child != null && childIndex == 0)
            {
                return child;
            }
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return (child == null) ? 0 : 1; }
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return (child == null) ? 0 : 1; }
        }

        /// <inheritdoc/>
        protected internal override void RemoveLogicalChild(UIElement child)
        {
            if (child != null && Child == child)
            {
                Child = null;
            }
            base.RemoveLogicalChild(child);
        }

        /// <summary>
        /// Gets a value indicating whether this decorator hooks its children into the logical tree.
        /// </summary>
        internal virtual Boolean HooksChildIntoLogicalTree
        {
            get { return true; }
        }

        // Property values.
        private UIElement child;
    }
}
