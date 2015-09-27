using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ListBox.xml")]
    public class ListBox : Selector
    {
        /// <summary>
        /// Initializes the <see cref="ListBox"/> type.
        /// </summary>
        static ListBox()
        {
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBox), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBox), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Once));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(ListBox), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ListBox(UltravioletContext uv, String name)
            : base(uv, name)
        {
            SetValue<ListBoxSelectedItems>(SelectedItemsPropertyKey, selectedItems);
        }

        /// <summary>
        /// Gets or sets the list box's selection mode.
        /// </summary>
        public SelectionMode SelectionMode
        {
            get { return GetValue<SelectionMode>(SelectionModeProperty); }
            set { SetValue<SelectionMode>(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets the list box's collection of selected items.
        /// </summary>
        public ListBoxSelectedItems SelectedItems
        {
            get { return GetValue<ListBoxSelectedItems>(SelectedItemsProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectionMode"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-mode'.</remarks>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(ListBox),
            new PropertyMetadata<SelectionMode>(SelectionMode.Single));

        /// <summary>
        /// The private access key for the <see cref="SelectedItems"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(ListBoxSelectedItems), typeof(ListBox),
            new PropertyMetadata<ListBoxSelectedItems>());

        /// <summary>
        /// Identifies the <see cref="SelectedItems"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selected-items'.</remarks>
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Called to inform the list box that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(ListBoxItem container)
        {
            var item = ItemContainerGenerator.ItemFromContainer(container);
            if (item == null)
                return;

            switch (SelectionMode)
            {
                case SelectionMode.Single:
                    HandleItemClickedSingle(item);
                    break;

                case SelectionMode.Multiple:
                    HandleItemClickedMultiple(item);
                    break;
            }
        }

        /// <inheritdoc/>
        protected internal override Panel CreateItemsPanel()
        {
            return new StackPanel(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected internal override Boolean HandlesScrolling
        {
            get { return true; }
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListBoxItem(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainer(DependencyObject element)
        {
            return element is ListBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(DependencyObject container, Object item)
        {
            var lbi = container as ListBoxItem;
            if (lbi == null)
                return false;

            return lbi.Content == item;
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemAdded(Object item)
        {
            selectedItems.Add(item);
            base.OnSelectedItemAdded(item);
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemRemoved(Object item)
        {
            selectedItems.Remove(item);
            base.OnSelectedItemRemoved(item);
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemsChanged()
        {
            selectedItems.Clear();
            foreach (var item in Items)
            {
                var container = ItemContainerGenerator.ContainerFromItem(item);
                if (container != null && GetIsSelected(container))
                {
                    selectedItems.Add(container);
                }
            }

            base.OnSelectedItemsChanged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            var selection = Keyboard.GetFocusedElement(View) as UIElement;
            var navdir = (FocusNavigationDirection?)null;

            switch (key)
            {
                case Key.Space:
                case Key.Return:
                    if (key == Key.Return && !GetValue<Boolean>(KeyboardNavigation.AcceptsReturnProperty))
                        break;

                    var listBoxItem = data.OriginalSource as ListBoxItem;
                    if (listBoxItem != null && ItemsControlFromItemContainer(listBoxItem) == this)
                    {
                        HandleItemClicked(listBoxItem);
                        data.Handled = true;
                    }
                    break;

                case Key.Left:
                case Key.Right:
                    if (PART_ScrollViewer != null)
                    {
                        PART_ScrollViewer.HandleKeyScrolling(device, key, modifiers, ref data);
                    }
                    break;

                case Key.Up:
                    if (SelectionMode == SelectionMode.Single)
                    {
                        navdir = FocusNavigationDirection.Up;
                    }
                    break;

                case Key.Down:
                    if (SelectionMode == SelectionMode.Single)
                    {
                        navdir = FocusNavigationDirection.Down;
                    }
                    break;

                case Key.Home:
                    var firstItem = GetFirstItem();
                    if (firstItem != null && firstItem.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(firstItem);
                    }
                    break;

                case Key.End:
                    var lastItem = GetLastItem();
                    if (lastItem != null && lastItem.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(lastItem);
                    }
                    break;

                case Key.PageUp:
                    var firstItemOnPage = GetFirstItemOnCurrentPage();
                    if (firstItemOnPage != null)
                    {
                        if (Keyboard.GetFocusedElement(View) == firstItemOnPage)
                            firstItemOnPage = GetFirstItemOnPreviousPage();

                        if (firstItemOnPage.Focus())
                        {
                            HandleItemClickedAndScrollIntoView(firstItemOnPage, false);
                        }
                    }
                    break;

                case Key.PageDown:
                    var lastItemOnPage = GetLastItemOnCurrentPage();
                    if (lastItemOnPage != null)
                    {
                        if (Keyboard.GetFocusedElement(View) == lastItemOnPage)
                            lastItemOnPage = GetLastItemOnNextPage();

                        if (lastItemOnPage.Focus())
                        {
                            HandleItemClickedAndScrollIntoView(lastItemOnPage, false);
                        }
                    }
                    break;
            }

            if (navdir.HasValue)
            {
                if (selection != null && selection.MoveFocus(navdir.Value))
                {
                    var focused = Keyboard.GetFocusedElement(View) as UIElement;
                    var listBoxItem = FindContainer(focused);
                    if (listBoxItem != null && (modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                    {
                        HandleItemClickedAndScrollIntoView(listBoxItem);
                    }
                    data.Handled = true;
                }
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <summary>
        /// Finds the <see cref="ListBoxItem"/> that contains the specified object.
        /// </summary>
        /// <param name="item">The object for which to find a container.</param>
        /// <returns>The <see cref="ListBoxItem"/> that contains the specified object, or <c>null</c>.</returns>
        private ListBoxItem FindContainer(Object item)
        {
            var current = item as DependencyObject;

            while (current != null)
            {
                if (current == this)
                    return null;

                if (current is ListBoxItem)
                    return ItemsControlFromItemContainer((ListBoxItem)current) == this ? (ListBoxItem)current : null;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Selects the specified item and scrolls it into view.
        /// </summary>
        private void HandleItemClickedAndScrollIntoView(ListBoxItem item, Boolean buffer = true)
        {
            HandleItemClicked(item);
            ScrollItemIntoView(item, buffer);
        }

        /// <summary>
        /// Handles clicking on an item when the list box is in single selection mode.
        /// </summary>
        /// <param name="item">The item that was clicked.</param>
        private void HandleItemClickedSingle(Object item)
        {
            var dobj = item as DependencyObject;
            if (dobj == null)
                return;

            BeginChangeSelection();

            if (GetIsSelected(dobj))
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    UnselectItem(item);
                }
            }
            else
            {
                UnselectAllItems();
                SelectItem(item);
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Handles clicking on an item when the list box is in multiple selection mode.
        /// </summary>
        /// <param name="item">The item that was clicked.</param>
        private void HandleItemClickedMultiple(Object item)
        {
            var dobj = item as DependencyObject;
            if (dobj == null)
                return;

            var selected = GetIsSelected(dobj);
            if (selected)
            {
                UnselectItem(item);
            }
            else
            {
                SelectItem(item);
            }
        }
        
        /// <summary>
        /// Adjusts the scrolling position so that the specified item is in view.
        /// </summary>
        /// <param name="item">The item to scroll into view.</param>
        private void ScrollItemIntoView(Object item, Boolean buffer = true)
        {
            var listBoxItem = FindContainer(item);
            if (listBoxItem == null)
                return;

            if (PART_ScrollViewer == null)
                return;

            var minVisible = PART_ScrollViewer.VerticalOffset;
            var maxVisible = minVisible + PART_ScrollViewer.ViewportHeight;

            var prev = buffer ? (VisualTreeHelper.GetPreviousSibling(listBoxItem) as UIElement ?? listBoxItem) : listBoxItem;
            if (prev.UntransformedRelativeBounds.Top < minVisible)
            {
                PART_ScrollViewer.ScrollToVerticalOffset(prev.UntransformedRelativeBounds.Top);
                return;
            }

            var next = buffer ? (VisualTreeHelper.GetNextSibling(listBoxItem) as UIElement ?? listBoxItem) : listBoxItem;
            if (next.UntransformedRelativeBounds.Bottom > maxVisible)
            {
                PART_ScrollViewer.ScrollToVerticalOffset(next.UntransformedRelativeBounds.Bottom - PART_ScrollViewer.ViewportHeight);
                return;
            }
        }

        /// <summary>
        /// Gets the first item in the list.
        /// </summary>
        /// <returns>The first item in the list, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetFirstItem()
        {
            if (Items.Count == 0)
                return null;

            return FindContainer(Items[0]);
        }

        /// <summary>
        /// Gets the last item in the list.
        /// </summary>
        /// <returns>The last item in the list, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetLastItem()
        {
            if (Items.Count == 0)
                return null;

            return FindContainer(Items[Items.Count - 1]);
        }

        /// <summary>
        /// Gets the item at the specified offset which is displayed on the specified page.
        /// </summary>
        /// <param name="min">The minimum offset of the page.</param>
        /// <param name="max">The maximum offset of the page.</param>
        /// <param name="offset">The offset of the item to retrieve.</param>
        /// <returns>The item at the specified offset which is displayed on the specified page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetFirstItemOnPage(Double min, Double max, Double offset)
        {
            if (Items.Count == 0)
                return null;

            var firstCandidate = default(ListBoxItem);

            foreach (var item in Items)
            {
                var container = FindContainer(item);

                if (container.UntransformedRelativeBounds.Top < min || container.UntransformedRelativeBounds.Bottom > max)
                    continue;

                if (firstCandidate == null)
                    firstCandidate = container;

                if (container.UntransformedRelativeBounds.Top <= offset && container.UntransformedRelativeBounds.Bottom > offset)
                    return container;
            }

            return firstCandidate ?? FindContainer(Items[0]);
        }

        /// <summary>
        /// Gets the first item which is displayed on the current page.
        /// </summary>
        /// <returns>The first item which is displayed on the current page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetFirstItemOnCurrentPage()
        {
            if (Items.Count == 0)
                return null;

            var min = PART_ScrollViewer.VerticalOffset;
            var max = min + PART_ScrollViewer.ViewportHeight;
            return GetFirstItemOnPage(min, max, min);
        }

        /// <summary>
        /// Gets the first item which is displayed on the previous page.
        /// </summary>
        /// <returns>The first item which is displayed on the previous page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetFirstItemOnPreviousPage()
        {
            if (Items.Count == 0)
                return null;

            var min = PART_ScrollViewer.VerticalOffset - PART_ScrollViewer.ViewportHeight;
            var max = min + PART_ScrollViewer.ViewportHeight;
            return GetFirstItemOnPage(min, max, min);
        }

        /// <summary>
        /// Gets the item at the specified offset which is displayed on the specified page.
        /// </summary>
        /// <param name="min">The minimum offset of the page.</param>
        /// <param name="max">The maximum offset of the page.</param>
        /// <param name="offset">The offset of the item to retrieve.</param>
        /// <returns>The item at the specified offset which is displayed on the specified page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetLastItemOnPage(Double min, Double max, Double offset)
        {
            if (Items.Count == 0)
                return null;

            var lastCandidate = default(ListBoxItem);

            foreach (var item in Items)
            {
                var container = FindContainer(item);

                if (container.UntransformedRelativeBounds.Top < min || container.UntransformedRelativeBounds.Bottom > max)
                    continue;

                lastCandidate = container;

                if (container.UntransformedRelativeBounds.Top < offset && container.UntransformedRelativeBounds.Bottom >= offset)
                    return container;
            }

            return lastCandidate ?? FindContainer(Items[Items.Count - 1]);
        }

        /// <summary>
        /// Gets the last item which is displayed on the current page.
        /// </summary>
        /// <returns>The last item which is displayed on the current page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetLastItemOnCurrentPage()
        {
            if (PART_ScrollViewer == null)
                return null;

            var min = PART_ScrollViewer.VerticalOffset;
            var max = min + PART_ScrollViewer.ViewportHeight;
            return GetLastItemOnPage(min, max, max);
        }

        /// <summary>
        /// Gets the last item which is displayed on the next page.
        /// </summary>
        /// <returns>The last item which is displayed on the next page, or <c>null</c> if the list is empty.</returns>
        private ListBoxItem GetLastItemOnNextPage()
        {
            if (PART_ScrollViewer == null)
                return null;

            var min = PART_ScrollViewer.VerticalOffset + PART_ScrollViewer.ViewportHeight;
            var max = min + PART_ScrollViewer.ViewportHeight;
            return GetLastItemOnPage(min, max, max);
        }

        // Property values.
        private readonly ListBoxSelectedItems selectedItems = 
            new ListBoxSelectedItems();

        // Component references.
        private readonly ScrollViewer PART_ScrollViewer = null;
    }
}
