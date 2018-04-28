using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.ListBox.xml")]
    [UvmlPlaceholder("ItemsPanel", typeof(StackPanel))]
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
            SetValue(SelectedItemsPropertyKey, selectedItems);
        }

        /// <summary>
        /// Gets or sets the list box's selection mode.
        /// </summary>
        /// <value>A <see cref="Controls.SelectionMode"/> value indicating how items in the list box are selected.
        /// The default value is <see cref="Controls.SelectionMode.Single"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SelectionModeProperty"/></dpropField>
        ///     <dpropStylingName>selection-mode</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SelectionMode SelectionMode
        {
            get { return GetValue<SelectionMode>(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets the list box's collection of selected items.
        /// </summary>
        /// <value>A <see cref="ListBoxSelectedItems"/> collection which represents the items
        /// in the list box that are currently selected.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SelectedItemsProperty"/></dpropField>
        ///     <dpropStylingName>selected-items</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ListBoxSelectedItems SelectedItems
        {
            get { return GetValue<ListBoxSelectedItems>(SelectedItemsProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectionMode"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionMode"/> dependency property.</value>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(ListBox),
            new PropertyMetadata<SelectionMode>(SelectionMode.Single));

        /// <summary>
        /// The private access key for the <see cref="SelectedItems"/> read-only dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedItems"/> dependency property.</value>
        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(ListBoxSelectedItems), typeof(ListBox),
            new PropertyMetadata<ListBoxSelectedItems>());

        /// <summary>
        /// Identifies the <see cref="SelectedItems"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedItems"/> dependency property.</value>
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Called to inform the list box that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(ListBoxItem container)
        {
            switch (SelectionMode)
            {
                case SelectionMode.Single:
                    HandleItemClickedSingle(container);
                    break;

                case SelectionMode.Multiple:
                    HandleItemClickedMultiple(container);
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
        protected override Boolean IsItemContainer(Object obj)
        {
            return obj is ListBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(Object obj, Object item)
        {
            var lbi = obj as ListBoxItem;
            if (lbi == null)
                return false;

            return lbi.Content == item;
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemAdded(DependencyObject container, Object item)
        {
            selectedItems.Add(item);
            base.OnSelectedItemAdded(container, item);
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemRemoved(DependencyObject container, Object item)
        {
            selectedItems.Remove(item);
            base.OnSelectedItemRemoved(container, item);
        }

        /// <inheritdoc/>
        protected override void OnSelectedItemsChanged()
        {
            selectedItems.Clear();
            foreach (var container in ItemContainers)
            {
                if (GetIsSelected(container))
                {
                    selectedItems.Add(container);
                }
            }
            base.OnSelectedItemsChanged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            switch (key)
            {
                case Key.Space:
                case Key.Return:
                    if (key == Key.Return && !GetValue<Boolean>(KeyboardNavigation.AcceptsReturnProperty))
                        break;

                    var listBoxItem = ItemsControlUtil.FindContainer<ListBoxItem>(this, data.OriginalSource);
                    if (listBoxItem != null)
                    {
                        HandleItemClicked(listBoxItem);
                        data.Handled = true;
                    }
                    break;

                case Key.Left:
                    PART_ScrollViewer?.LineLeft();
                    data.Handled = true;
                    break;

                case Key.Right:
                    PART_ScrollViewer?.LineRight();
                    data.Handled = true;
                    break;

                case Key.Up:
                    if (SelectionMode == SelectionMode.Single)
                    {
                        MoveSelectedItem(FocusNavigationDirection.Up, modifiers);
                    }
                    data.Handled = true;
                    break;

                case Key.Down:
                    if (SelectionMode == SelectionMode.Single)
                    {
                        MoveSelectedItem(FocusNavigationDirection.Down, modifiers);
                    }
                    data.Handled = true;
                    break;

                case Key.Home:
                    var firstItem = ItemsControlUtil.GetFirstItem<ListBoxItem>(this, PART_ScrollViewer);
                    if (firstItem != null && firstItem.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(firstItem);
                    }
                    data.Handled = true;
                    break;

                case Key.End:
                    var lastItem = ItemsControlUtil.GetLastItem<ListBoxItem>(this, PART_ScrollViewer);
                    if (lastItem != null && lastItem.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(lastItem);
                    }
                    data.Handled = true;
                    break;

                case Key.PageUp:
                    var pageUpTarget = ItemsControlUtil.GetPageUpNext<ListBoxItem>(this, PART_ScrollViewer);
                    if (pageUpTarget != null && pageUpTarget.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(pageUpTarget, false);
                    }
                    data.Handled = true;
                    break;

                case Key.PageDown:
                    var pageDownTarget = ItemsControlUtil.GetPageDownNext<ListBoxItem>(this, PART_ScrollViewer);
                    if (pageDownTarget != null && pageDownTarget.Focus())
                    {
                        HandleItemClickedAndScrollIntoView(pageDownTarget, false);
                    }
                    data.Handled = true;
                    break;
            }

            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                var listBoxItem = ItemsControlUtil.FindContainer<ListBoxItem>(this, data.OriginalSource);
                if (listBoxItem != null)
                {
                    HandleItemClicked(listBoxItem);
                    data.Handled = true;
                }
            }
            else
            {
                switch (button)
                {
                    case GamePadButton.LeftStickLeft:
                        PART_ScrollViewer?.LineLeft();
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickRight:
                        PART_ScrollViewer?.LineRight();
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickUp:
                        if (SelectionMode == SelectionMode.Single)
                        {
                            MoveSelectedItem(FocusNavigationDirection.Up);
                        }
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickDown:
                        if (SelectionMode == SelectionMode.Single)
                        {
                            MoveSelectedItem(FocusNavigationDirection.Down);
                        }
                        data.Handled = true;
                        break;
                }
            }

            base.OnGamePadButtonDown(device, button, repeat, data);
        }

        /// <summary>
        /// Selects the specified item and scrolls it into view.
        /// </summary>
        private void HandleItemClickedAndScrollIntoView(ListBoxItem item, Boolean buffer = true)
        {
            HandleItemClicked(item);
            ItemsControlUtil.ScrollItemIntoView<ListBoxItem>(this, PART_ScrollViewer, item, buffer);
        }

        /// <summary>
        /// Handles clicking on an item when the list box is in single selection mode.
        /// </summary>
        /// <param name="item">The item that was clicked.</param>
        private void HandleItemClickedSingle(ListBoxItem item)
        {
            BeginChangeSelection();
            
            if (GetIsSelected(item))
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    UnselectContainer(item);
                }
            }
            else
            {
                UnselectAllItems();
                SelectContainer(item);
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Handles clicking on an item when the list box is in multiple selection mode.
        /// </summary>
        /// <param name="item">The item that was clicked.</param>
        private void HandleItemClickedMultiple(ListBoxItem item)
        {
            var selected = GetIsSelected(item);
            if (selected)
            {
                UnselectContainer(item);
            }
            else
            {
                SelectContainer(item);
            }
        }

        /// <summary>
        /// Moves the selected item in the specified direction.
        /// </summary>
        private void MoveSelectedItem(FocusNavigationDirection direction, ModifierKeys modifiers = ModifierKeys.None)
        {
            var selection = Keyboard.GetFocusedElement(View) as UIElement;
            if (selection != null && selection.MoveFocus(direction))
            {
                var focused = Keyboard.GetFocusedElement(View) as UIElement;
                var listBoxItem = ItemsControlUtil.FindContainer<ListBoxItem>(this, focused);
                if (listBoxItem != null && (modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                {
                    HandleItemClickedAndScrollIntoView(listBoxItem);
                }
            }
        }

        // Property values.
        private readonly ListBoxSelectedItems selectedItems = 
            new ListBoxSelectedItems();

        // Component references.
        private readonly ScrollViewer PART_ScrollViewer = null;
    }
}
