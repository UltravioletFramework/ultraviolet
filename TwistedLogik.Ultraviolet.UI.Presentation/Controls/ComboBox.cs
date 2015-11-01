using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a combo box with a drop down list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ComboBox.xml")]
    [UvmlPlaceholder("ItemsPanel", typeof(StackPanel))]
    public class ComboBox : Selector
    {
        /// <summary>
        /// Initializes the <see cref="ComboBox"/> type.
        /// </summary>
        static ComboBox()
        {
            EventManager.RegisterClassHandler(typeof(ComboBox), LoadedEvent, new UpfRoutedEventHandler(HandleLoaded));

            EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.LostMouseCaptureEvent, new UpfRoutedEventHandler(HandleLostMouseCapture));
            EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseDownEvent, new UpfMouseButtonEventHandler(HandleMouseDown));

            IsEnabledProperty.OverrideMetadata(typeof(ComboBox), new PropertyMetadata<Boolean>(HandleIsEnabledChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ComboBox(UltravioletContext uv, String name)
            : base(uv, name)
        {
            visualClone = new VisualClone(uv);

            VisualStateGroups.Create("common", new[] { "normal", "hover", "disabled" });
            VisualStateGroups.Create("opened", new[] { "closed", "open" });
        }

        /// <summary>
        /// Gets or sets the maximum height of the combo box's drop-down list.
        /// </summary>
        public Double MaxDropDownHeight
        {
            get { return GetValue<Double>(MaxDropDownHeightProperty); }
            set { SetValue<Double>(MaxDropDownHeightProperty, value); }
        }

        /// <summary>
        /// Gets the actual maximum height of the combo box's drop-down list, after
        /// considering the amount of available space in the current window.
        /// </summary>
        public Double ActualMaxDropDownHeight
        {
            get { return GetValue<Double>(ActualMaxDropDownHeightProperty); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combo box's drop-down list is currently open.
        /// </summary>
        public Boolean IsDropDownOpen
        {
            get { return GetValue<Boolean>(IsDropDownOpenProperty); }
            set { SetValue<Boolean>(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        /// Gets the item that is displayed in the combo box's selection box.
        /// </summary>
        public Object SelectionBoxItem
        {
            get { return GetValue<Object>(SelectionBoxItemProperty); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the item in the selection box.
        /// </summary>
        public String SelectionBoxItemStringFormat
        {
            get { return GetValue<String>(SelectionBoxItemStringFormatProperty); }
        }

        /// <summary>
        /// Occurs when the combo box's drop-down list is opened.
        /// </summary>
        public event UpfEventHandler DropDownOpened;

        /// <summary>
        /// Occurs when the combo box's drop-down list is closed.
        /// </summary>
        public event UpfEventHandler DropDownClosed;

        /// <summary>
        /// Identifies the <see cref="MaxDropDownHeight"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'max-dropdown-height'.</remarks>
        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", "max-dropdown-height", typeof(Double), typeof(ComboBox),
            new PropertyMetadata<Double>(Double.NaN, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="ActualMaxDropDownHeight"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualMaxDropDownHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualMaxDropDownHeight", typeof(Double), typeof(ComboBox),
            new PropertyMetadata<Double>());

        /// <summary>
        /// Identifies the <see cref="ActualMaxDropDownHeight"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-box-item'.</remarks>
        public static readonly DependencyProperty ActualMaxDropDownHeightProperty = ActualMaxDropDownHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'dropdown-open'.</remarks>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", "dropdown-open", typeof(Boolean), typeof(ComboBox),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleIsDropDownOpenChanged));

        /// <summary>
        /// The private access key for the <see cref="SelectionBoxItem"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey SelectionBoxItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItem", typeof(Object), typeof(ComboBox),
            new PropertyMetadata<Boolean>(null));

        /// <summary>
        /// Identifies the <see cref="SelectionBoxItem"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-box-item'.</remarks>
        public static readonly DependencyProperty SelectionBoxItemProperty = SelectionBoxItemPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="SelectionBoxItemStringFormat"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey SelectionBoxItemStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemStringFormat", typeof(String), typeof(ComboBox),
            new PropertyMetadata<String>(null));

        /// <summary>
        /// Identifies the <see cref="SelectionBoxItemStringFormat"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-box-item-string-format'.</remarks>
        public static readonly DependencyProperty SelectionBoxItemStringFormatProperty = SelectionBoxItemStringFormatPropertyKey.DependencyProperty;

        /// <summary>
        /// Called to inform the combo box that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(ComboBoxItem container)
        {
            var item = ItemContainerGenerator.ItemFromContainer(container);
            if (item == null)
                return;

            var dobj = item as DependencyObject;
            if (dobj == null || !GetIsSelected(dobj))
            {
                BeginChangeSelection();

                UnselectAllItems();
                SelectItem(item);

                EndChangeSelection();
            }

            IsDropDownOpen = false;
        }

        /// <summary>
        /// Called to inform the combo box that one of its items was changed.
        /// </summary>
        /// <param name="container">The item container that was changed.</param>
        internal void HandleItemChanged(ComboBoxItem container)
        {
            UpdateSelectionBox();
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
            return new ComboBoxItem(Ultraviolet, null);
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainer(DependencyObject element)
        {
            return element is ComboBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(DependencyObject container, Object item)
        {
            var cbi = container as ComboBoxItem;
            if (cbi == null)
                return false;

            return cbi.Content == item;
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            if (IsDropDownOpen)
            {
                if (View == null || (View.LayoutRoot.IsLoaded && !viewSize.Equals(View.Area.Size)))
                {
                    viewSize = View.Area.Size;
                    IsDropDownOpen = false;
                }
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            if (oldView == null && newView != null)
            {
                LayoutUpdated += ComboBox_LayoutUpdated;
            }

            if (oldView != null && newView == null)
            {
                LayoutUpdated -= ComboBox_LayoutUpdated;
            }

            base.OnViewChanged(oldView, newView);
        }

        /// <inheritdoc/>
        protected override void OnSelectionChanged()
        {
            UpdateSelectionBox();

            IsDropDownOpen = false;

            base.OnSelectionChanged();
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateVisualState();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            OnKeyDown_General(device, key, modifiers, ref data);

            if (!data.Handled)
            {
                if (IsDropDownOpen)
                {
                    OnKeyDown_DropDownOpen(device, key, modifiers, ref data);
                }
                else
                {
                    OnKeyDown_DropDownClosed(device, key, modifiers, ref data);
                }
            }

            base.OnKeyDown(device, key, modifiers, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            if (IsDropDownOpen)
            {
                OnGamePadAxisDown_DropDownOpen(device, axis, value, repeat, ref data);
            }
            else
            {
                OnGamePadAxisDown_DropDownClosed(device, axis, value, repeat, ref data);
            }

            base.OnGamePadAxisDown(device, axis, value, repeat, ref data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            if (IsDropDownOpen)
            {
                OnGamePadButtonDown_DropDownOpen(device, button, repeat, ref data);
            }
            else
            {
                OnGamePadButtonDown_DropDownClosed(device, button, repeat, ref data);
            }

            base.OnGamePadButtonDown(device, button, repeat, ref data);
        }
        
        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> both when the drop down is open and when it is closed.
        /// </summary>
        private void OnKeyDown_General(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.F4:
                    if ((modifiers & ModifierKeys.Alt) != ModifierKeys.Alt)
                    {
                        IsDropDownOpen = !IsDropDownOpen;
                        data.Handled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> when the drop down is open.
        /// </summary>
        private void OnKeyDown_DropDownOpen(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Tab:
                    PerformTabNavigation(key, modifiers);
                    data.Handled = true;
                    break;

                case Key.Escape:
                    IsDropDownOpen = false;
                    data.Handled = true;
                    break;

                case Key.Return:
                    SelectFocusedItem();
                    IsDropDownOpen = false;
                    data.Handled = true;
                    break;

                case Key.Up:
                    MoveItemFocus(-1);
                    data.Handled = true;
                    break;

                case Key.Down:
                    MoveItemFocus(1);
                    data.Handled = true;
                    break;

                case Key.PageUp:
                    var pageUpTarget = ItemsControlUtil.GetPageUpNext<ComboBoxItem>(this, PART_ScrollViewer);
                    if (pageUpTarget != null && pageUpTarget.Focus())
                    {
                        ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, pageUpTarget, false);
                    }
                    data.Handled = true;
                    break;

                case Key.PageDown:
                    var pageDownTarget = ItemsControlUtil.GetPageDownNext<ComboBoxItem>(this, PART_ScrollViewer);
                    if (pageDownTarget != null && pageDownTarget.Focus())
                    {
                        ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, pageDownTarget, false);
                    }
                    data.Handled = true;
                    break;

                case Key.Home:
                    var firstItem = ItemsControlUtil.GetFirstItem<ComboBoxItem>(this, PART_ScrollViewer);
                    if (firstItem != null && firstItem.Focus())
                    {
                        ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, firstItem);
                    }
                    data.Handled = true;
                    break;

                case Key.End:
                    var lastItem = ItemsControlUtil.GetLastItem<ComboBoxItem>(this, PART_ScrollViewer);
                    if (lastItem != null && lastItem.Focus())
                    {
                        ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, lastItem);
                    }
                    data.Handled = true;
                    break;

                case Key.Left:
                case Key.Right:
                    if (PART_ScrollViewer != null)
                    {
                        PART_ScrollViewer.HandleKeyScrolling(key, modifiers, ref data);
                    }
                    data.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> both when the drop down is closed and when it is closed.
        /// </summary>
        private void OnKeyDown_DropDownClosed(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            switch (key)
            {
                case Key.Home:
                    MoveItemSelection(Int32.MinValue);
                    break;

                case Key.End:
                    MoveItemSelection(Int32.MaxValue);
                    break;

                case Key.Up:
                case Key.Left:
                    MoveItemSelection(-1);
                    data.Handled = true;
                    break;

                case Key.Down:
                case Key.Right:
                    MoveItemSelection(1);
                    data.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles <see cref="GamePad.AxisDownEvent"/> when the drop down is open.
        /// </summary>
        private void OnGamePadAxisDown_DropDownOpen(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.UseAxisForDirectionalNavigation)
            {
                if (GamePad.DirectionalNavigationAxisX == axis || GamePad.DirectionalNavigationAxisY == axis)
                {
                    var direction = device.GetJoystickDirectionFromAxis(axis);
                    switch (direction)
                    {
                        case GamePadJoystickDirection.Up:
                            MoveItemFocus(-1);
                            break;

                        case GamePadJoystickDirection.Down:
                            MoveItemFocus(1);
                            break;

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
                    }
                    data.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles <see cref="GamePad.AxisDownEvent"/> when the drop down is closed.
        /// </summary>
        private void OnGamePadAxisDown_DropDownClosed(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.UseAxisForDirectionalNavigation)
            {
                if (GamePad.DirectionalNavigationAxisX == axis || GamePad.DirectionalNavigationAxisY == axis)
                {
                    var direction = device.GetJoystickDirectionFromAxis(axis);
                    switch (direction)
                    {
                        case GamePadJoystickDirection.Up:
                        case GamePadJoystickDirection.Left:
                            MoveItemSelection(-1);
                            break;

                        case GamePadJoystickDirection.Down:
                        case GamePadJoystickDirection.Right:
                            MoveItemSelection(1);
                            break;
                    }
                    data.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles <see cref="GamePad.ButtonDownEvent"/> when the drop down is open.
        /// </summary>
        private void OnGamePadButtonDown_DropDownOpen(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                SelectFocusedItem();
                IsDropDownOpen = false;
                data.Handled = true;
            }
            else if (GamePad.CancelButton == button)
            {
                IsDropDownOpen = false;
                data.Handled = true;
            }
            else if (GamePad.TabButton == button)
            {
                PerformTabNavigation(Key.Tab, ModifierKeys.None);
                data.Handled = true;
            }
            else if (GamePad.ShiftTabButton == button)
            {
                PerformTabNavigation(Key.Tab, ModifierKeys.Shift);
                data.Handled = true;
            }
            else
            {
                if (!GamePad.UseAxisForDirectionalNavigation)
                {
                    switch (button)
                    {
                        case GamePadButton.DPadUp:
                            MoveItemSelection(-1);
                            break;

                        case GamePadButton.DPadDown:
                            MoveItemSelection(1);
                            break;

                        case GamePadButton.DPadLeft:
                            if (PART_ScrollViewer != null)
                            {
                                PART_ScrollViewer.HandleKeyScrolling(Key.Left, ModifierKeys.None, ref data);
                            }
                            break;

                        case GamePadButton.DPadRight:
                            if (PART_ScrollViewer != null)
                            {
                                PART_ScrollViewer.HandleKeyScrolling(Key.Right, ModifierKeys.None, ref data);
                            }
                            break;
                    }
                    data.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles <see cref="GamePad.ButtonDownEvent"/> when the drop down is closed.
        /// </summary>
        private void OnGamePadButtonDown_DropDownClosed(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                IsDropDownOpen = true;
                data.Handled = true;
            }
            else
            {
                if (!GamePad.UseAxisForDirectionalNavigation)
                {
                    switch (button)
                    {
                        case GamePadButton.DPadUp:
                        case GamePadButton.DPadLeft:
                            MoveItemSelection(-1);
                            break;

                        case GamePadButton.DPadDown:
                        case GamePadButton.DPadRight:
                            MoveItemSelection(1);
                            break;
                    }
                    data.Handled = true;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownOpened"/> event.
        /// </summary>
        protected virtual void OnDropDownOpened()
        {
            var temp = DropDownOpened;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownClosed"/> event.
        /// </summary>
        protected virtual void OnDropDownClosed()
        {
            var temp = DropDownClosed;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UIElement.IsEnabledChanged"/> dependency property changes.
        /// </summary>
        private static void HandleIsEnabledChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var comboBox = (ComboBox)dobj;
            comboBox.UpdateVisualState();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsDropDownOpen"/> dependency property changes.
        /// </summary>
        private static void HandleIsDropDownOpenChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var comboBox = (ComboBox)dobj;

            if (newValue)
            {
                var primary = comboBox.Ultraviolet.GetPlatform().Windows.GetPrimary();

                var actualMaxDropDownHeight = Double.IsNaN(comboBox.MaxDropDownHeight) ? comboBox.Display.PixelsToDips(primary.ClientSize.Height) / 3.0 : comboBox.MaxDropDownHeight;
                if (actualMaxDropDownHeight != comboBox.GetValue<Double>(ActualMaxDropDownHeightProperty))
                {
                    comboBox.SetValue(ActualMaxDropDownHeightPropertyKey, actualMaxDropDownHeight);
                }

                Mouse.Capture(comboBox.View, comboBox, CaptureMode.SubTree);

                if (comboBox.PART_Arrow != null)
                {
                    comboBox.PART_Arrow.Classes.Add("combobox-arrow-open");
                    comboBox.PART_Arrow.Classes.Remove("combobox-arrow-closed");
                }

                comboBox.UpdateVisualState();
                comboBox.OnDropDownOpened();

                comboBox.viewSize = comboBox.View.Area.Size;

                var focused = (comboBox.SelectedIndex >= 0) ? comboBox.ItemContainers[comboBox.SelectedIndex] as UIElement : comboBox;
                if (focused != null)
                    focused.Focus();
            }
            else
            {
                if (comboBox.IsMouseCaptured)
                    Mouse.Capture(comboBox.View, null, CaptureMode.None);

                if (comboBox.PART_Arrow != null)
                {
                    comboBox.PART_Arrow.Classes.Remove("combobox-arrow-open");
                    comboBox.PART_Arrow.Classes.Add("combobox-arrow-closed");
                }

                comboBox.UpdateVisualState();
                comboBox.OnDropDownClosed();

                var focused = Keyboard.GetFocusedElement(comboBox.View) as DependencyObject;
                if (focused != null && ItemsControlFromItemContainer(focused) == comboBox)
                    comboBox.Focus();
            }
        }
        
        /// <summary>
        /// Occurs when the control handles a <see cref="FrameworkElement.Loaded"/> routed event.
        /// </summary>
        private static void HandleLoaded(DependencyObject dobj, ref RoutedEventData data)
        {
            var comboBox = (ComboBox)dobj;
            comboBox.viewSize = (comboBox.View == null) ? Size2.Zero : comboBox.View.Area.Size;
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="Mouse.MouseDownEvent"/> routed event.
        /// </summary>
        private static void HandleMouseDown(DependencyObject dobj, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button != MouseButton.Left)
                return;

            var comboBox = (ComboBox)dobj;
            if (comboBox == Mouse.GetCaptured(comboBox.View) && comboBox == data.OriginalSource)
            {
                comboBox.IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="Mouse.LostMouseCaptureEvent"/> routed event.
        /// </summary>
        private static void HandleLostMouseCapture(DependencyObject dobj, ref RoutedEventData data)
        {
            var comboBox = (ComboBox)dobj;
            if (comboBox != data.OriginalSource)
            {
                if (comboBox.ContainsDescendant(data.OriginalSource as DependencyObject))
                {
                    if (Mouse.GetCaptured(comboBox.View) == null)
                    {
                        Mouse.Capture(comboBox.View, comboBox, CaptureMode.SubTree);
                        data.Handled = true;
                    }
                }
                else
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.LayoutUpdated"/> event.
        /// </summary>
        private void ComboBox_LayoutUpdated(Object sender, EventArgs e)
        {
            var selectedVisualClone = SelectionBoxItem as VisualClone;
            if (selectedVisualClone != null)
            {
                selectedVisualClone.HandleLayoutUpdated();
            }
        }
        
        /// <summary>
        /// Updates the selection box.
        /// </summary>
        private void UpdateSelectionBox()
        {
            visualClone.ClonedElement = null;

            var selectionBoxItem             = (Object)null;
            var selectionBoxItemStringFormat = ItemStringFormat;

            var contentControl = SelectedItem as ContentControl;
            if (contentControl != null)
            {
                selectionBoxItem             = contentControl.Content;
                selectionBoxItemStringFormat = contentControl.ContentStringFormat;
            }

            if (selectionBoxItem is UIElement)
            {
                visualClone.ClonedElement = (UIElement)selectionBoxItem;
                selectionBoxItem = visualClone;
            }

            SetValue(SelectionBoxItemPropertyKey, selectionBoxItem ?? String.Empty);
            SetValue(SelectionBoxItemStringFormatPropertyKey, selectionBoxItemStringFormat);
        }

        /// <summary>
        /// Transitions the combo box into the appropriate visual states.
        /// </summary>
        private void UpdateVisualState()
        {
            if (IsEnabled)
            {
                if (IsMouseOver)
                {
                    VisualStateGroups.GoToState("common", "hover");
                }
                else
                {
                    VisualStateGroups.GoToState("common", "normal");
                }
            }
            else
            {
                VisualStateGroups.GoToState("common", "disabled");
            }

            if (IsDropDownOpen)
            {
                VisualStateGroups.GoToState("opened", "open");
            }
            else
            {
                VisualStateGroups.GoToState("opened", "closed");
            }
        }

        /// <summary>
        /// Performs tab navigation while focus is within the drop down.
        /// </summary>
        private void PerformTabNavigation(Key key, ModifierKeys modifiers)
        {
            var shift = ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
            var ctrl  = ((modifiers & ModifierKeys.Control) == ModifierKeys.Control);

            var focused = Keyboard.GetFocusedElement(View) as UIElement;
            if (FocusNavigator.PerformNavigation(View, focused, shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next, ctrl))
            {
                focused = Keyboard.GetFocusedElement(View) as UIElement;

                var container = (focused == null) ? null : ItemsControlUtil.FindContainer<ComboBoxItem>(this, focused);
                if (container != null)
                {
                    ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, container);
                }
            }
        }

        /// <summary>
        /// Moves keyboard focus the specified number of steps away from the currently focused item.
        /// </summary>
        /// <param name="step">The number of steps by which to move focus.</param>
        private void MoveItemFocus(Int32 step)
        {
            var selectedContainer = ItemsControlUtil.FindFocusedContainer<ComboBoxItem>(this);
            var selectedIndex = (selectedContainer == null) ? -1 : ItemContainers.IndexOf(selectedContainer);

            var targetIndex = selectedIndex + step;
            if (targetIndex < 0 || targetIndex >= Items.Count)
                return;

            var targetContainer = ItemContainers[targetIndex] as UIElement;
            if (targetContainer == null)
                return;

            targetContainer.Focus();

            ItemsControlUtil.ScrollItemIntoView<ComboBoxItem>(this, PART_ScrollViewer, targetContainer);
        }

        /// <summary>
        /// Moves item selection the specified number of steps away from the currently selected item.
        /// </summary>
        /// <param name="steps">The number of steps to move the selection.</param>
        private void MoveItemSelection(Int32 steps)
        {
            var index = SelectedIndex;
            if (steps < 0)
            {
                if (index >= 0)
                {
                    SelectedIndex = Math.Max(0, index + steps);
                }
            }
            else
            {
                SelectedIndex = Math.Min(Items.Count - 1, index + steps);
            }
        }

        /// <summary>
        /// Selects the item which currently has keyboard focus.
        /// </summary>
        private void SelectFocusedItem()
        {
            var container = ItemsControlUtil.FindFocusedContainer<ComboBoxItem>(this);
            SelectedIndex = (container == null) ? -1 : ItemContainers.IndexOf(container);
        }

        /// <summary>
        /// Gets a value indicating whether this combo box contains the specified object as a descendant.
        /// </summary>
        /// <param name="dobj">The object to evaluate.</param>
        /// <returns><c>true</c> if the specified object is a descendant of this combo box; otherwise, <c>false</c>.</returns>
        private Boolean ContainsDescendant(DependencyObject dobj)
        {
            var current = dobj;

            while (current != null)
            {
                if (current == this)
                    return true;

                var popupRoot = current as PopupRoot;
                if (popupRoot != null)
                {
                    var popup = popupRoot.Parent as Popup;
                    if (popup == null)
                        return false;

                    current = popup.Parent ?? popup.PlacementTarget;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return false;
        }
        
        // Component references.
        private readonly UIElement PART_Arrow = null;
        private readonly ScrollViewer PART_ScrollViewer = null;
        private readonly VisualClone visualClone;

        // State values.
        private Size2 viewSize;
    }
}
