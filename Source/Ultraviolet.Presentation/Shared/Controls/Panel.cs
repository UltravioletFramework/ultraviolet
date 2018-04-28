using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element with child elements.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Children")]
    public abstract class Panel : FrameworkElement
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
        /// Gets the relative order of the specified element on its z-plane.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The relative order of the specified element on its z-plane.</returns>
        public static Double GetZIndex(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Int32>(ZIndexProperty);
        }

        /// <summary>
        /// Sets the relative order of the specified element on its z-plane.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The relative order of the specified element on its z-plane.</param>
        public static void SetZIndex(DependencyObject element, Int32 value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(ZIndexProperty, value);
        }

        /// <summary>
        /// Gets the panel's collection of children.
        /// </summary>
        /// <value>A <see cref="UIElementCollection"/> that contains the panel's children.</value>
        public UIElementCollection Children
        {
            get { return children; }
        }

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Panel.ZIndex"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Panel.ZIndex"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the element's z-index.
        /// </summary>
        /// <value>A <see cref="Int32"/> value that specifies the relative layer on which the child of
        /// a <see cref="Panel"/> control is drawn. The default value is 0.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ZIndexProperty"/></dpropField>
        ///     <dpropStylingName>z-index</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.RegisterAttached("ZIndex", typeof(Int32), typeof(Panel),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.Zero, PropertyMetadataOptions.None, HandleZIndexChanged));

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
            Contract.EnsureRange(childIndex >= 0 && childIndex < children.Count + 1, nameof(childIndex));

            return children[childIndex];
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            return GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChildByZOrder(Int32 childIndex)
        {
            Contract.EnsureRange(childIndex >= 0 && childIndex < children.Count + 1, nameof(childIndex));

            return children.GetByZOrder(childIndex);
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
            if (!HitTestUtil.IsPotentialHit(this, point))
                return null;

            var childMatch = HitTestChildren(point);
            if (childMatch != null)
            {
                return childMatch;
            }

            return Bounds.Contains(point) ? this : null;
        }

        /// <summary>
        /// Performs a hit test against the panel's children and returns the topmost
        /// child which contains the specified point.
        /// </summary>
        /// <param name="point">The point in element space to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> child which contains the specified point, or <see langword="null"/>.</returns>
        protected virtual Visual HitTestChildren(Point2D point)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children.GetByZOrder(i);

                var childMatch = child.HitTest(TransformToDescendant(child, point));
                if (childMatch != null)
                {
                    return childMatch;
                }
            }
            return null;
        }

        /// <summary>
        /// Occurs when the value of the ZIndex attached property changes.
        /// </summary>
        private static void HandleZIndexChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var parent = VisualTreeHelper.GetParent(dobj) as Panel;
            if (parent == null)
                return;

            parent.HandleChildZIndexChanged();
        }

        /// <summary>
        /// Called when the z-index of one of the panel's children is changed.
        /// </summary>
        private void HandleChildZIndexChanged()
        {
            children.SortVisualChildrenByZIndex();
        }

        // Property values.
        private readonly UIElementCollection children;
    }
}
