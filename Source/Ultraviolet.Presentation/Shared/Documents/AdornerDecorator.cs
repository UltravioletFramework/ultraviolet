using System;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Documents
{
    /// <summary>
    /// Represents a decorator which adds an <see cref="AdornerLayer"/> to the visual tree.
    /// </summary>
    [UvmlKnownType]
    public class AdornerDecorator : Decorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerDecorator"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public AdornerDecorator(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.adornerLayer = new AdornerLayer(uv, null);
        }

        /// <summary>
        /// Gets the decorator's adorner layer.
        /// </summary>
        public AdornerLayer AdornerLayer
        {
            get { return adornerLayer; }
        }

        /// <inheritdoc/>
        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (base.Child == value)
                    return;

                if (value == null)
                {
                    base.Child = null;

                    adornerLayer.ChangeLogicalAndVisualParents(null, null);
                }
                else
                {
                    base.Child = value;

                    if (VisualTreeHelper.GetParent(adornerLayer) != this)
                        adornerLayer.ChangeLogicalAndVisualParents(null, this);
                }
            }
        }

        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            var adorner = adornerLayer.HitTest(TransformToDescendant(adornerLayer, point));
            if (adorner != null)
                return adorner;

            var child = Child;
            if (child == null)
                return null;

            return child.HitTest(TransformToDescendant(child, point));
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var desiredSize = base.MeasureOverride(availableSize);
            if (VisualTreeHelper.GetParent(adornerLayer) == this)
            {
                adornerLayer.Measure(availableSize);
            }
            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var arrangedSize = base.ArrangeOverride(finalSize, options);
            if (VisualTreeHelper.GetParent(adornerLayer) == this)
            {
                adornerLayer.Arrange(new RectangleD(Point2D.Zero, finalSize));
            }
            return arrangedSize;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            var child = Child;
            if (child == null)
                throw new ArgumentOutOfRangeException("childIndex");

            switch (childIndex)
            {
                case 0:
                    return child;

                case 1:
                    return adornerLayer;
            }

            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get
            {
                var child = Child;
                return (child == null) ? 0 : 2;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get
            {
                return base.LogicalChildrenCount;
            }
        }

        // The adorner layer which this decorator adds to the visual tree.
        private readonly AdornerLayer adornerLayer;
    }
}
