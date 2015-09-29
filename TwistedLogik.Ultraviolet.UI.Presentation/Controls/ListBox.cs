using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

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
                case Key.Right:
                    if (PART_ScrollViewer != null)
                    {
                        PART_ScrollViewer.HandleKeyScrolling(key, modifiers, ref data);
                    }
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

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.UseAxisForDirectionalNavigation && (axis == GamePad.DirectionalNavigationAxisX || axis == GamePad.DirectionalNavigationAxisY))
            {
                var direction = device.GetJoystickDirectionFromAxis(axis);
                switch (direction)
                {
                    case GamePadJoystickDirection.Left:
                        if (PART_ScrollViewer != null)
                        {
                            PART_ScrollViewer.HandleKeyScrolling(Key.Left, ModifierKeys.None, ref data);
                        }
                        break;

                    case GamePadJoystickDirection.Right:
                        if (PART_ScrollViewer != null)
                        {
                            PART_ScrollViewer.HandleKeyScrolling(Key.Right, ModifierKeys.None, ref data);
                        }
                        break;

                    case GamePadJoystickDirection.Up:
                        if (SelectionMode == SelectionMode.Single)
                        {
                            MoveSelectedItem(FocusNavigationDirection.Up);
                        }
                        break;

                    case GamePadJoystickDirection.Down:
                        if (SelectionMode == SelectionMode.Single)
                        {
                            MoveSelectedItem(FocusNavigationDirection.Down);
                        }
                        break;
                }
                data.Handled = true;
            }
            
            base.OnGamePadAxisDown(device, axis, value, repeat, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
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
                if (!GamePad.UseAxisForDirectionalNavigation)
                {
                    switch (button)
                    {
                        case GamePadButton.DPadLeft:
                            if (PART_ScrollViewer != null)
                            {
                                PART_ScrollViewer.HandleKeyScrolling(Key.Left, ModifierKeys.None, ref data);
                            }
                            data.Handled = true;
                            break;

                        case GamePadButton.DPadRight:
                            if (PART_ScrollViewer != null)
                            {
                                PART_ScrollViewer.HandleKeyScrolling(Key.Right, ModifierKeys.None, ref data);
                            }
                            data.Handled = true;
                            break;

                        case GamePadButton.DPadUp:
                            if (SelectionMode == SelectionMode.Single)
                            {
                                MoveSelectedItem(FocusNavigationDirection.Up);
                            }
                            data.Handled = true;
                            break;

                        case GamePadButton.DPadDown:
                            if (SelectionMode == SelectionMode.Single)
                            {
                                MoveSelectedItem(FocusNavigationDirection.Down);
                            }
                            data.Handled = true;
                            break;
                    }
                }
            }

            base.OnGamePadButtonDown(device, button, repeat, ref data);
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
