using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Documents
{
    /// <summary>
    /// Represents a layer for containing adorners
    /// </summary>
    [UvmlKnownType]
    public partial class AdornerLayer : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdornerLayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public AdornerLayer(UltravioletContext uv, String name)
            : base(uv, name)
        {
            adorners = new VisualCollection(this);
            adornersStates = new List<AdornerState>();
            adornersTemp = new List<Adorner>();
        }

        /// <summary>
        /// Walks up the visual tree from the specified visual and returns the nearest instance of <see cref="AdornerLayer"/>.
        /// </summary>
        /// <param name="visual">The visual at which to begin searching for adorner layers.</param>
        /// <returns>The nearest <see cref="AdornerLayer"/> which is above <paramref name="visual"/> in the visual tree.</returns>
        public static AdornerLayer GetAdornerLayer(Visual visual)
        {
            Contract.Require(visual, "visual");

            var current = (DependencyObject)visual;

            while (current != null)
            {
                if (current is AdornerDecorator)
                    return ((AdornerDecorator)current).AdornerLayer;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Performs a hit test to find the adorner at the specified point.
        /// </summary>
        /// <param name="point">The point to test, in element-local space.</param>
        /// <returns>The <see cref="Adorner"/> at the specified point, or <c>null</c> if there is no adorner at that point.</returns>
        public Adorner AdornerHitTest(Point2D point)
        {
            return HitTest(point) as Adorner;
        }

        /// <summary>
        /// Adds an adorner to the adorner layer.
        /// </summary>
        /// <param name="adorner">The adorner to add to the adorner layer.</param>
        public void Add(Adorner adorner)
        {
            Contract.Require(adorner, "adorner");

            var state = new AdornerState(adorner);
            state.LastAbsoluteX = adorner.AdornedElement.AbsolutePosition.X;
            state.LastAbsoluteY = adorner.AdornedElement.AbsolutePosition.Y;
            state.LastRenderWidth = adorner.AdornedElement.RenderSize.Width;
            state.LastRenderHeight = adorner.AdornedElement.RenderSize.Height;

            adorners.Add(adorner);
            adornersStates.Add(state);

            adorner.InvalidateMeasure();
            adorner.ChangeLogicalParent(this);

            InvalidateMeasure();
            InvalidateArrange(true);

            Measure(MostRecentAvailableSize);
            Arrange(MostRecentFinalRect, MostRecentArrangeOptions);
        }

        /// <summary>
        /// Removes an adorner from the adorner layer.
        /// </summary>
        /// <param name="adorner">The adorner to remove from the adorner layer.</param>
        public void Remove(Adorner adorner)
        {
            Contract.Require(adorner, "adorner");

            var index = adorners.IndexOf(adorner);
            if (index >= 0)
            {
                adorners.RemoveAt(index);
                adornersStates.RemoveAt(index);

                adorner.ChangeLogicalParent(null);
            }
        }

        /// <summary>
        /// Populates a list with the adorners for the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve adorners.</param>
        /// <param name="list">The list to populate with the adorners for <paramref name="element"/>.</param>
        public void GetAdorners(UIElement element, IList list)
        {
            Contract.Require(element, "element");
            Contract.Require(list, "list");

            list.Clear();

            foreach (Adorner adorner in adorners)
            {
                if (adorner.AdornedElement == element)
                    list.Add(adorner);
            }
        }

        /// <summary>
        /// Populates a list with the adorners for the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve adorners.</param>
        /// <param name="list">The list to populate with the adorners for <paramref name="element"/>.</param>
        public void GetAdorners(UIElement element, IList<Adorner> list)
        {
            Contract.Require(element, "element");
            Contract.Require(list, "list");

            list.Clear();

            foreach (Adorner adorner in adorners)
            {
                if (adorner.AdornedElement == element)
                    list.Add(adorner);
            }
        }

        /// <summary>
        /// Creates an array containing the adorners for the specified element.
        /// </summary>
        /// <param name="element">The element for which to retrieve adorners.</param>
        /// <returns>An array containing the adorners for the specified element, or <c>null</c> if there are no such adorners.</returns>
        public Adorner[] GetAdorners(UIElement element)
        {
            Contract.Require(element, "element");

            if (adorners.Count == 0)
                return null;

            var matches = from Adorner adorner in adorners
                          where
                              adorner.AdornedElement == element
                          select adorner;

            return matches.Any() ? matches.ToArray() : null;
        }
        
        /// <inheritdoc/>
        protected override void OnVisualParentChanged(Visual oldParent, Visual newParent)
        {
            if (oldParent == null && newParent != null)
            {
                LayoutUpdated += CheckAdornersForChangedOnLayoutUpdated;
            }

            if (oldParent != null && newParent == null)
            {
                LayoutUpdated -= CheckAdornersForChangedOnLayoutUpdated;
            }

            base.OnVisualParentChanged(oldParent, newParent);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            return (UIElement)adorners[childIndex];
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            return (UIElement)adorners[childIndex];
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return adorners.Count; }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return adorners.Count; }
        }
        
        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            CheckAdornersForChanges(false);
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            for (int i = 0; i < adorners.Count; i++)
            {
                var adorner = (Adorner)adorners[i];
                adorner.Measure(availableSize);
            }
            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var layerParent = VisualTreeHelper.GetParent(this) as Visual;

            for (int i = 0; i < adorners.Count; i++)
            {
                var state = adornersStates[i];

                var adorner = (Adorner)adorners[i];
                var adornedElement = adorner.AdornedElement;

                state.LastAbsoluteX = adornedElement.AbsolutePosition.X;
                state.LastAbsoluteY = adornedElement.AbsolutePosition.Y;
                state.LastRenderWidth = adornedElement.RenderSize.Width;
                state.LastRenderHeight = adornedElement.RenderSize.Height;

                var transformMatrix = adornedElement.GetTransformToAncestorMatrix(layerParent);
                state.LastTransform = transformMatrix;

                adorner.GetDesiredTransform(ref transformMatrix);

                var transformObject = adorner.RenderTransform as MatrixTransform ?? new MatrixTransform();
                transformObject.Matrix = transformMatrix;

                adorner.RenderTransformOrigin = Point2D.Zero;
                adorner.RenderTransform = transformObject;

                var adornerRect = new RectangleD(Point2D.Zero, adorner.DesiredSize);
                adorner.Arrange(adornerRect);
            }
            return finalSize;
        }
        
        /// <inheritdoc/>
        protected override Visual HitTestCore(Point2D point)
        {
            var result = base.HitTestCore(point);
            return (result == this) ? null : result;
        }

        /// <summary>
        /// Handles the adorner layer's <see cref="UIElement.LayoutUpdated"/> event.
        /// </summary>
        private void CheckAdornersForChangedOnLayoutUpdated(Object sender, EventArgs e)
        {
            CheckAdornersForChanges(true);
        }
        
        /// <summary>
        /// Checks the layers adorners to determine if their adorned elements underwent any changes that require
        /// us to perform a new layout pass.
        /// </summary>
        private void CheckAdornersForChanges(Boolean checkForLayoutChanges)
        {
            adornersTemp.Clear();
            try
            {
                var invalidateLayer = false;

                var layerParent = VisualTreeHelper.GetParent(this) as Visual;
                if (layerParent == null)
                    return;

                for (int i = 0; i < adorners.Count; i++)
                {
                    var state = adornersStates[i];

                    var adorner = (Adorner)adorners[i];
                    var adornedElement = adorner.AdornedElement;

                    if (!adornedElement.IsDescendantOf(layerParent))
                    {
                        adornersTemp.Add(adorner);
                        continue;
                    }

                    var invalidateAdorner = false;

                    if (checkForLayoutChanges && (
                        !MathUtil.AreApproximatelyEqual(state.LastAbsoluteX, adornedElement.AbsolutePosition.X) ||
                        !MathUtil.AreApproximatelyEqual(state.LastAbsoluteY, adornedElement.AbsolutePosition.Y) ||
                        !MathUtil.AreApproximatelyEqual(state.LastRenderWidth, adornedElement.RenderSize.Width) ||
                        !MathUtil.AreApproximatelyEqual(state.LastRenderHeight, adornedElement.RenderSize.Height)))
                    {
                        invalidateAdorner = true;
                    }
                    else
                    {
                        var transformMatrix = adornedElement.GetTransformToAncestorMatrix(layerParent);
                        if (!state.LastTransform.EqualsRef(ref transformMatrix))
                        {
                            invalidateAdorner = true;
                        }
                    }

                    if (invalidateAdorner)
                    {
                        adorner.InvalidateMeasure();
                        invalidateLayer = true;
                    }
                }

                foreach (var adorner in adornersTemp)
                    Remove(adorner);

                if (invalidateLayer)
                {
                    InvalidateMeasure();
                    InvalidateArrange(true);
                }
            }
            finally
            {
                adornersTemp.Clear();
            }
        }

        // State values.
        private readonly VisualCollection adorners;
        private readonly List<AdornerState> adornersStates;
        private readonly List<Adorner> adornersTemp;
    }
}
