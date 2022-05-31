using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;
using Ultraviolet.Platform;
using Ultraviolet.Presentation.Media;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        /// <summary>
        /// Attempts to set the global style sheet used by all Presentation Foundation views. If any exceptions are thrown
        /// during this process, the previous style sheet will be automatically restored.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        /// <returns><see langword="true"/> if the style sheet was set successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean TrySetGlobalStyleSheet(UvssDocument styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var previous = globalStyleSheet;
            try
            {
                SetGlobalStyleSheetInternal(styleSheet);
            }
            catch
            {
                SetGlobalStyleSheetInternal(previous);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Attempts to set the global style sheet used by all Presentation Foundation views. If any exceptions are thrown
        /// during this process, the previous style sheet will be automatically restored.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        /// <returns><see langword="true"/> if the style sheet was set successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean TrySetGlobalStyleSheet(GlobalStyleSheet styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var previous = globalStyleSheet;
            try
            {
                SetGlobalStyleSheetInternal(styleSheet);
            }
            catch
            {
                SetGlobalStyleSheetInternal(previous);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the global style sheet used by all Presentation Foundation views.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        public void SetGlobalStyleSheet(UvssDocument styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetGlobalStyleSheetInternal(styleSheet);
        }

        /// <summary>
        /// Sets the global style sheet used by all Presentation Foundation views.
        /// </summary>
        /// <param name="styleSheet">The global style sheet to set.</param>
        public void SetGlobalStyleSheet(GlobalStyleSheet styleSheet)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetGlobalStyleSheetInternal(styleSheet);
        }

        /// <summary>
        /// Resolves the current global style sheet to a <see cref="UvssDocument"/> instance which
        /// is appropriate for the screen density of the primary display.
        /// </summary>
        /// <returns>The <see cref="UvssDocument"/> instance that was created.</returns>
        public UvssDocument ResolveGlobalStyleSheet()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (globalStyleSheet == null)
                return null;

            if (globalStyleSheet is UvssDocument doc)
                return doc;

            return ((GlobalStyleSheet)globalStyleSheet).ToUvssDocument();
        }

        /// <summary>
        /// Resolves the current global style sheet to a <see cref="UvssDocument"/> instance which
        /// is appropriate for the specified screen density.
        /// </summary>
        /// <param name="density">The screen density for which to resolve the style sheet.</param>
        /// <returns>The <see cref="UvssDocument"/> instance that was created.</returns>
        public UvssDocument ResolveGlobalStyleSheet(ScreenDensityBucket density)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (globalStyleSheet == null)
                return null;

            if (globalStyleSheet is UvssDocument doc)
                return doc;

            return ((GlobalStyleSheet)globalStyleSheet).ToUvssDocument(density);
        }

        /// <summary>
        /// Occurs when the Presentation Foundation's global style sheet is changed.
        /// </summary>
        public event EventHandler GlobalStyleSheetChanged;

        /// <summary>
        /// Performs the layout process for any elements which currently need it.
        /// </summary>
        internal void PerformLayout()
        {
            using (UltravioletProfiler.Section(PresentationProfilerSections.Layout))
            {
                while (ElementNeedsLayout)
                {
                    digestCycleIDOfLastLayout = digestCycleID;

                    // 1. Style
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Style))
                    {
                        while (ElementNeedsStyle)
                        {
                            var element = styleQueue.Dequeue();
                            if (element.IsStyleValid || element.View == null)
                                continue;

                            element.Style(element.View.StyleSheet);
                            element.InvalidateMeasure();
                        }
                    }

                    // 2. Measure
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Measure))
                    {
                        while (ElementNeedsMeasure && !ElementNeedsStyle)
                        {
                            var element = measureQueue.Dequeue();
                            if (element.IsMeasureValid || element.View == null)
                                continue;

                            element.Measure(element.MostRecentAvailableSize);
                            element.InvalidateArrange();
                        }
                    }

                    // 3. Arrange
                    using (UltravioletProfiler.Section(PresentationProfilerSections.Arrange))
                    {
                        while (ElementNeedsArrange && !ElementNeedsStyle && !ElementNeedsMeasure)
                        {
                            var element = arrangeQueue.Dequeue();
                            if (element.IsArrangeValid || element.View == null)
                                continue;

                            element.Arrange(element.MostRecentFinalRect, element.MostRecentArrangeOptions);
                        }
                    }

                    // 4. Raise events.
                    if (ElementNeedsLayout)
                        continue;

                    RaiseRenderSizeChanged();

                    if (ElementNeedsLayout)
                        continue;

                    RaiseLayoutUpdated();
                }
            }
        }

        /// <summary>
        /// Removes the specified UI element from all of the Foundation's processing queues.
        /// </summary>
        /// <param name="element">The element to remove from the queues.</param>
        internal void RemoveFromQueues(UIElement element)
        {
            Contract.Require(element, nameof(element));

            StyleQueue.Remove(element);
            MeasureQueue.Remove(element);
            ArrangeQueue.Remove(element);
        }

        /// <summary>
        /// Removes all elements associated with the specified view from the Foundation's processing queues.
        /// </summary>
        /// <param name="view">The view to remove from the queues.</param>
        internal void RemoveFromQueues(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            StyleQueue.Remove(view);
            MeasureQueue.Remove(view);
            ArrangeQueue.Remove(view);
        }

        /// <summary>
        /// Enqueues any elements within the specified view that have invalid measure, arrange, or styles, but
        /// were previously removed from the queues as a result of being orphaned by a destroyed window.
        /// </summary>
        /// <param name="view">The view to restore.</param>
        internal void RestoreToQueues(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            RestoreToQueues(view.LayoutRoot);
        }

        /// <summary>
        /// Enqueues any elements within the specified tree that have invalid measure, arrange, or styles, but
        /// were previously removed from the queues as a result of being orphaned by a destroyed window.
        /// </summary>
        /// <param name="root">The root of the tree to restore.</param>
        internal void RestoreToQueues(DependencyObject root)
        {
            Contract.Require(root, nameof(root));

            if (root is UIElement uiElement)
            {
                if (!uiElement.IsMeasureValid)
                    MeasureQueue.Enqueue(uiElement);

                if (!uiElement.IsArrangeValid)
                    ArrangeQueue.Enqueue(uiElement);

                if (!uiElement.IsStyleValid)
                    StyleQueue.Enqueue(uiElement);
            }

            VisualTreeHelper.ForEachChild(root, this, (child, state) =>
            {
                ((PresentationFoundation)state).RestoreToQueues(child);
            });
        }

        /// <summary>
        /// Adds the specified element to the queue of elements which will receive a RenderSizeChanged event.
        /// </summary>
        /// <param name="element">The element to register.</param>
        /// <param name="previousSize">The previous size of the element.</param>
        internal void RegisterRenderSizeChanged(UIElement element, Size2D previousSize)
        {
            Contract.Require(element, nameof(element));

            var entry = new RenderSizeChangedEntry(element, previousSize);
            elementsPendingRenderSizeChanged.AddLast(entry);
        }

        /// <summary>
        /// Indicates that the specified element is interested in receiving <see cref="UIElement.LayoutUpdated"/> events.
        /// </summary>
        /// <param name="element">The element to register.</param>
        internal void RegisterForLayoutUpdated(UIElement element)
        {
            elementsWithLayoutUpdatedHandlers.AddLast(element);
        }

        /// <summary>
        /// Indicates that the specified element is no longer interested in receiving <see cref="UIElement.LayoutUpdated"/> events.
        /// </summary>
        /// <param name="element">The element to unregister.</param>
        internal void UnregisterForLayoutUpdated(UIElement element)
        {
            elementsWithLayoutUpdatedHandlers.Remove(element);
        }

        /// <summary>
        /// Gets the queue of elements with invalid styling states.
        /// </summary>
        internal LayoutQueue StyleQueue
        {
            get { return styleQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid measurement states.
        /// </summary>
        internal LayoutQueue MeasureQueue
        {
            get { return measureQueue; }
        }

        /// <summary>
        /// Gets the queue of elements with invalid arrangement states.
        /// </summary>
        internal LayoutQueue ArrangeQueue
        {
            get { return arrangeQueue; }
        }

        /// <summary>
        /// Invalidates the specified element's style.
        /// </summary>
        private void InvalidateStyle(UIElement element)
        {
            if (element.IsStyleValid)
            {
                element.InvalidateStyleInternal();
                PerformanceStats.InvalidateStyleCount++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's measure.
        /// </summary>
        private void InvalidateMeasure(UIElement element)
        {
            if (element.IsMeasureValid)
            {
                element.InvalidateMeasureInternal();
                PerformanceStats.InvalidateMeasureCount++;
            }
        }

        /// <summary>
        /// Invalidates the specified element's arrangement.
        /// </summary>
        private void InvalidateArrange(UIElement element)
        {
            if (element.IsArrangeValid)
            {
                element.InvalidateArrangeInternal();
                PerformanceStats.InvalidateArrangeCount++;
            }
        }

        /// <summary>
        /// Raises the RenderSizeChanged event for elements which have changed size.
        /// </summary>
        private void RaiseRenderSizeChanged()
        {
            try
            {
                if (elementsPendingRenderSizeChangedTemp.Capacity < elementsPendingRenderSizeChanged.Count)
                    elementsPendingRenderSizeChangedTemp.Capacity = elementsPendingRenderSizeChanged.Count;

                foreach (var entry in elementsPendingRenderSizeChanged)
                    elementsPendingRenderSizeChangedTemp.Add(entry);

                elementsPendingRenderSizeChanged.Clear();

                foreach (var entry in elementsPendingRenderSizeChangedTemp)
                {
                    var sizeChangedInfo = new SizeChangedInfo(entry.PreviousSize, entry.CurrentSize);
                    entry.Element.OnRenderSizeChanged(sizeChangedInfo);
                }
            }
            finally
            {
                elementsPendingRenderSizeChangedTemp.Clear();
            }
        }

        /// <summary>
        /// Raises the <see cref="UIElement.LayoutUpdated"/> event for elements which have registered handlers.
        /// </summary>
        private void RaiseLayoutUpdated()
        {
            elementsWithLayoutUpdatedHandlers.ForEach((element) =>
            {
                element.RaiseLayoutUpdated();
                return !Instance.ElementNeedsLayout;
            });
        }
        
        /// <summary>
        /// Sets the global style sheet used by all Presentation Foundation views.
        /// </summary>
        private void SetGlobalStyleSheetInternal(Object styleSheet)
        {
            this.globalStyleSheet = styleSheet;
            GlobalStyleSheetChanged?.Invoke(this, EventArgs.Empty);
        }           

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting layout.
        /// </summary>
        private Boolean ElementNeedsLayout
        {
            get { return ElementNeedsStyle || ElementNeedsMeasure || ElementNeedsArrange; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting styling.
        /// </summary>
        private Boolean ElementNeedsStyle
        {
            get { return styleQueue.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting measurement.
        /// </summary>
        private Boolean ElementNeedsMeasure
        {
            get { return measureQueue.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether any elements are awaiting arrangement.
        /// </summary>
        private Boolean ElementNeedsArrange
        {
            get { return arrangeQueue.Count > 0; }
        }

        // The global style sheet.
        private Object globalStyleSheet;

        // The queues of elements with invalid layouts.
        private readonly LayoutQueue styleQueue;
        private readonly LayoutQueue measureQueue;
        private readonly LayoutQueue arrangeQueue;
        private readonly WeakLinkedList<UIElement> elementsWithLayoutUpdatedHandlers =
            new WeakLinkedList<UIElement>();

        // The list of elements waiting for a RenderSizeChanged event.
        private readonly PooledLinkedList<RenderSizeChangedEntry> elementsPendingRenderSizeChanged =
            new PooledLinkedList<RenderSizeChangedEntry>();
        private readonly List<RenderSizeChangedEntry> elementsPendingRenderSizeChangedTemp =
            new List<RenderSizeChangedEntry>();
    }
}
