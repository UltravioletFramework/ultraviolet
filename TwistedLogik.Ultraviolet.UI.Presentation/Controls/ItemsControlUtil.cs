using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Contains utility methods relating to the <see cref="ItemsControl"/> class.
    /// </summary>
    internal static class ItemsControlUtil
    {
        /// <summary>
        /// Finds the item container that contains the specified object.
        /// </summary>
        /// <param name="control">The items control within which to search.</param>
        /// <param name="item">The object for which to find a container.</param>
        /// <returns>The item container that contains the specified object, or <see langword="null"/>.</returns>
        public static TContainer FindContainer<TContainer>(ItemsControl control, Object item) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));
            Contract.Require(item, nameof(item));

            var current = item as DependencyObject;

            while (current != null)
            {
                if (current == control)
                    return null;

                if (current is TContainer)
                    return ItemsControl.ItemsControlFromItemContainer((TContainer)current) == control ? (TContainer)current : null;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Gets the item container which is currently focused, if any.
        /// </summary>
        /// <param name="control">The items control within which to search.</param>
        /// <returns>The item container which is currently focused, or <see langword="null"/>.</returns>
        public static TContainer FindFocusedContainer<TContainer>(ItemsControl control) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));
            
            var focused = Keyboard.GetFocusedElement(control.View);
            return (focused == null) ? null : FindContainer<TContainer>(control, focused);
        }
        
        /// <summary>
        /// Gets the first item in the list.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The first item in the list, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetFirstItem<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));
            
            if (scrollview == null || control.Items.Count == 0)
                return null;

            return FindContainer<TContainer>(control, control.Items[0]);
        }

        /// <summary>
        /// Gets the last item in the list.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The last item in the list, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetLastItem<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            return FindContainer<TContainer>(control, control.Items[control.Items.Count - 1]);
        }

        /// <summary>
        /// Gets the item at the specified offset which is displayed on the specified page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <param name="min">The minimum offset of the page.</param>
        /// <param name="max">The maximum offset of the page.</param>
        /// <param name="offset">The offset of the item to retrieve.</param>
        /// <returns>The item at the specified offset which is displayed on the specified page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetFirstItemOnPage<TContainer>(ItemsControl control, ScrollViewer scrollview, Double min, Double max, Double offset) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var firstCandidate = default(TContainer);

            foreach (var item in control.Items)
            {
                var container = FindContainer<TContainer>(control,
                    control.ItemContainerGenerator.ContainerFromItem(item));

                if (container.UntransformedRelativeBounds.Top < min || container.UntransformedRelativeBounds.Bottom > max)
                    continue;

                if (firstCandidate == null)
                    firstCandidate = container;

                if (container.UntransformedRelativeBounds.Top <= offset && container.UntransformedRelativeBounds.Bottom > offset)
                    return container;
            }

            return firstCandidate ?? FindContainer<TContainer>(control, control.Items[0]);
        }

        /// <summary>
        /// Gets the first item which is displayed on the current page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The first item which is displayed on the current page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetFirstItemOnCurrentPage<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var min = scrollview.ContentVerticalOffset;
            var max = min + scrollview.ViewportHeight;
            return GetFirstItemOnPage<TContainer>(control, scrollview, min, max, min);
        }

        /// <summary>
        /// Gets the first item which is displayed on the previous page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The first item which is displayed on the previous page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetFirstItemOnPreviousPage<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var min = scrollview.ContentVerticalOffset - scrollview.ViewportHeight;
            var max = min + scrollview.ViewportHeight;
            return GetFirstItemOnPage<TContainer>(control, scrollview, min, max, min);
        }

        /// <summary>
        /// Gets the item at the specified offset which is displayed on the specified page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <param name="min">The minimum offset of the page.</param>
        /// <param name="max">The maximum offset of the page.</param>
        /// <param name="offset">The offset of the item to retrieve.</param>
        /// <returns>The item at the specified offset which is displayed on the specified page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetLastItemOnPage<TContainer>(ItemsControl control, ScrollViewer scrollview, Double min, Double max, Double offset) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var lastCandidate = default(TContainer);

            foreach (var item in control.Items)
            {
                var container = FindContainer<TContainer>(control, 
                    control.ItemContainerGenerator.ContainerFromItem(item));

                if (container.UntransformedRelativeBounds.Top < min || container.UntransformedRelativeBounds.Bottom > max)
                    continue;

                lastCandidate = container;

                if (container.UntransformedRelativeBounds.Top < offset && container.UntransformedRelativeBounds.Bottom >= offset)
                    return container;
            }

            return lastCandidate ?? FindContainer<TContainer>(control, control.Items[control.Items.Count - 1]);
        }

        /// <summary>
        /// Gets the last item which is displayed on the current page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The last item which is displayed on the current page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetLastItemOnCurrentPage<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var min = scrollview.ContentVerticalOffset;
            var max = min + scrollview.ViewportHeight;
            return GetLastItemOnPage<TContainer>(control, scrollview, min, max, max);
        }

        /// <summary>
        /// Gets the last item which is displayed on the next page.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The last item which is displayed on the next page, or <see langword="null"/> if the list is empty.</returns>
        public static TContainer GetLastItemOnNextPage<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));

            if (scrollview == null || control.Items.Count == 0)
                return null;

            var min = scrollview.ContentVerticalOffset + scrollview.ViewportHeight;
            var max = min + scrollview.ViewportHeight;
            return GetLastItemOnPage<TContainer>(control, scrollview, min, max, max);
        }

        /// <summary>
        /// Gets the next container that should be navigated to when the Page Up key is pressed.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The next container that should be navigated to when the Page Up key is pressed.</returns>
        public static TContainer GetPageUpNext<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            var firstItemOnPage = GetFirstItemOnCurrentPage<TContainer>(control, scrollview);
            if (firstItemOnPage != null && Keyboard.GetFocusedElement(control.View) == firstItemOnPage)
            {
                firstItemOnPage = GetFirstItemOnPreviousPage<TContainer>(control, scrollview);
            }
            return firstItemOnPage;

        }

        /// <summary>
        /// Gets the next container that should be navigated to when the Page Down key is pressed.
        /// </summary>
        /// <param name="control">The control within which to search.</param>
        /// <param name="scrollview">The scroll viewer that contains the control's items.</param>
        /// <returns>The next container that should be navigated to when the Page Down key is pressed.</returns>
        public static TContainer GetPageDownNext<TContainer>(ItemsControl control, ScrollViewer scrollview) where TContainer : UIElement
        {
            var lastItemOnPage = GetLastItemOnCurrentPage<TContainer>(control, scrollview);
            if (lastItemOnPage != null && Keyboard.GetFocusedElement(control.View) == lastItemOnPage)
            {
                lastItemOnPage = GetLastItemOnNextPage<TContainer>(control, scrollview);
            }
            return lastItemOnPage;
        }

        /// <summary>
        /// Adjusts the scrolling position of an items control so that the specified item is in view.
        /// </summary>
        /// <param name="control">The items control to scroll.</param>
        /// <param name="scrollview">The scroll viewer to scroll.</param>
        /// <param name="item">The item to scroll into view.</param>
        /// <param name="buffer">A value indicating whether to leave a buffer space between the item and the edge of the scroll viewer.</param>
        public static void ScrollItemIntoView<TContainer>(ItemsControl control, ScrollViewer scrollview, Object item, Boolean buffer = true) where TContainer : UIElement
        {
            Contract.Require(control, nameof(control));
            Contract.Require(item, nameof(item));

            if (scrollview == null)
                return;

            var comboBoxItem = FindContainer<TContainer>(control, item);
            if (comboBoxItem == null)
                return;
            
            var minVisible = scrollview.ContentVerticalOffset;
            var maxVisible = minVisible + scrollview.ViewportHeight;

            var prev = buffer ? (VisualTreeHelper.GetPreviousSibling(comboBoxItem) as UIElement ?? comboBoxItem) : comboBoxItem;
            if (prev.UntransformedRelativeBounds.Top < minVisible)
            {
                scrollview.ScrollToVerticalOffset(prev.UntransformedRelativeBounds.Top);
                return;
            }

            var next = buffer ? (VisualTreeHelper.GetNextSibling(comboBoxItem) as UIElement ?? comboBoxItem) : comboBoxItem;
            if (next.UntransformedRelativeBounds.Bottom > maxVisible)
            {
                scrollview.ScrollToVerticalOffset(next.UntransformedRelativeBounds.Bottom - scrollview.ViewportHeight);
                return;
            }
        }
    }
}
