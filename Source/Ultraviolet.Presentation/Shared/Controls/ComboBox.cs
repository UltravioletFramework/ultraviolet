using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a combo box with a drop down list of selectable items.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.ComboBox.xml")]
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

            EventManager.RegisterClassHandler(typeof(ComboBox), Touch.LostNewTouchCaptureEvent, new UpfRoutedEventHandler(HandleLostNewTouchCapture));
            EventManager.RegisterClassHandler(typeof(ComboBox), Touch.TouchDownEvent, new UpfTouchDownEventHandler(HandleTouchDown));

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
        /// <value>A <see cref="Double"/> value that specifies the maximum height of the combo
        /// box's drop down list in device-independent pixels. The default value is <see cref="Double.NaN"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="MaxDropDownHeightProperty"/></dpropField>
        ///		<dpropStylingName>max-drop-down-height</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double MaxDropDownHeight
        {
            get { return GetValue<Double>(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        /// <summary>
        /// Gets the actual maximum height of the combo box's drop-down list, after
        /// considering the amount of available space in the current window.
        /// </summary>
        /// <value>A <see cref="Double"/> value that specifies the actual height of the combo
        /// box's drop down list in device-independent pixels after layout has been performed.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="ActualMaxDropDownHeightProperty"/></dpropField>
        ///		<dpropStylingName>actual-drop-down-height</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double ActualMaxDropDownHeight
        {
            get { return GetValue<Double>(ActualMaxDropDownHeightProperty); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combo box's drop-down list is currently open.
        /// </summary>
        /// <value><see langword="true"/> if the drop down is open; otherwise, <see langword="false"/>. The
        /// default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsDropDownOpenProperty"/></dpropField>
        ///		<dpropStylingName>drop-down-open</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsDropDownOpen
        {
            get { return GetValue<Boolean>(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        /// Gets the item that is displayed in the combo box's selection box.
        /// </summary>
        /// <value>The <see cref="Object"/> that is currently being displayed
        /// in the combo box's selection box. The default value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectionBoxItemProperty"/></dpropField>
        ///		<dpropStylingName>selection-box-item</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Object SelectionBoxItem
        {
            get { return GetValue<Object>(SelectionBoxItemProperty); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the item in the selection box.
        /// </summary>
        /// <value>A format string that specifies how to format the combo box's selected item. The
        /// default value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectionBoxItemStringFormatProperty"/></dpropField>
        ///		<dpropStylingName>selection-box-item-string-format</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
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
        /// <value>The identifier for the <see cref="MaxDropDownHeight"/> dependency property.</value>
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
        /// <value>The identifier for the <see cref="ActualMaxDropDownHeight"/> dependency property.</value>
        public static readonly DependencyProperty ActualMaxDropDownHeightProperty = ActualMaxDropDownHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDropDownOpen"/> dependency property.</value>
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
        /// <value>The identifier for the <see cref="SelectionBoxItem"/> dependency property.</value>
        public static readonly DependencyProperty SelectionBoxItemProperty = SelectionBoxItemPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="SelectionBoxItemStringFormat"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey SelectionBoxItemStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemStringFormat", typeof(String), typeof(ComboBox),
            new PropertyMetadata<String>(null));

        /// <summary>
        /// Identifies the <see cref="SelectionBoxItemStringFormat"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionBoxItemStringFormatProperty"/> dependency property.</value>
        public static readonly DependencyProperty SelectionBoxItemStringFormatProperty = SelectionBoxItemStringFormatPropertyKey.DependencyProperty;

        /// <summary>
        /// Called to inform the combo box that one of its items was clicked.
        /// </summary>
        /// <param name="container">The item container that was clicked.</param>
        internal void HandleItemClicked(ComboBoxItem container)
        {
            if (container != null && !GetIsSelected(container))
            {
                BeginChangeSelection();

                UnselectAllItems();
                SelectContainer(container);

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
        protected override Boolean IsItemContainer(Object obj)
        {
            return obj is ComboBoxItem;
        }

        /// <inheritdoc/>
        protected override Boolean IsItemContainerForItem(Object obj, Object item)
        {
            var cbi = obj as ComboBoxItem;
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

            if (!isDropDownOpenChanging)
            {
                IsDropDownOpen = false;
            }

            base.OnSelectionChanged();
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            UpdateVisualState();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            OnKeyDown_General(device, key, modifiers, data);

            if (!data.Handled)
            {
                if (IsDropDownOpen)
                {
                    OnKeyDown_DropDownOpen(device, key, modifiers, data);
                }
                else
                {
                    OnKeyDown_DropDownClosed(device, key, modifiers, data);
                }
            }

            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (IsDropDownOpen)
            {
                OnGamePadButtonDown_DropDownOpen(device, button, repeat, data);
            }
            else
            {
                OnGamePadButtonDown_DropDownClosed(device, button, repeat, data);
            }

            base.OnGamePadButtonDown(device, button, repeat, data);
        }
        
        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> both when the drop down is open and when it is closed.
        /// </summary>
        private void OnKeyDown_General(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
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
        private void OnKeyDown_DropDownOpen(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
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
                    PART_ScrollViewer?.LineLeft();
                    data.Handled = true;
                    break;

                case Key.Right:
                    PART_ScrollViewer?.LineRight();
                    data.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> both when the drop down is closed and when it is closed.
        /// </summary>
        private void OnKeyDown_DropDownClosed(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
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
        /// Handles <see cref="GamePad.ButtonDownEvent"/> when the drop down is open.
        /// </summary>
        private void OnGamePadButtonDown_DropDownOpen(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
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
                switch (button)
                {
                    case GamePadButton.LeftStickUp:
                        MoveItemFocus(-1);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickDown:
                        MoveItemFocus(1);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickLeft:
                        PART_ScrollViewer?.LineLeft();
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickRight:
                        PART_ScrollViewer?.LineRight();
                        data.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles <see cref="GamePad.ButtonDownEvent"/> when the drop down is closed.
        /// </summary>
        private void OnGamePadButtonDown_DropDownClosed(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (GamePad.ConfirmButton == button)
            {
                IsDropDownOpen = true;
                data.Handled = true;
            }
            else
            {
                switch (button)
                {
                    case GamePadButton.LeftStickUp:
                    case GamePadButton.LeftStickLeft:
                        MoveItemSelection(-1);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickDown:
                    case GamePadButton.LeftStickRight:
                        MoveItemSelection(1);
                        data.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="DropDownOpened"/> event.
        /// </summary>
        protected virtual void OnDropDownOpened() =>
            DropDownOpened?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="DropDownClosed"/> event.
        /// </summary>
        protected virtual void OnDropDownClosed() =>
            DropDownClosed?.Invoke(this);

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
            comboBox.isDropDownOpenChanging = true;

            if (newValue)
            {
                var selectedIndex = comboBox.SelectedIndex;

                comboBox.DigestImmediately(SelectedIndexProperty);
                comboBox.DigestImmediately(ItemsSourceProperty);

                if (comboBox.IsLoaded)
                {
                    if (comboBox.View != null)
                        comboBox.viewSize = comboBox.View.Area.Size;

                    comboBox.UpdateActualMaxDropDownHeight();
                    comboBox.UpdateInputCapture(true);
                }

                if (comboBox.PART_Arrow != null)
                {
                    comboBox.PART_Arrow.Classes.Add("combobox-arrow-open");
                    comboBox.PART_Arrow.Classes.Remove("combobox-arrow-closed");
                }

                comboBox.UpdateVisualState();
                comboBox.UpdateSelectionBox();
                comboBox.OnDropDownOpened();
                
                var focused = (comboBox.SelectedIndex >= 0) ? comboBox.ItemContainers[selectedIndex] as UIElement : comboBox;
                if (focused != null)
                    focused.Focus();                
            }
            else
            {
                comboBox.UpdateInputCapture(false);

                if (comboBox.PART_Arrow != null)
                {
                    comboBox.PART_Arrow.Classes.Remove("combobox-arrow-open");
                    comboBox.PART_Arrow.Classes.Add("combobox-arrow-closed");
                }

                comboBox.UpdateVisualState();
                comboBox.OnDropDownClosed();

                if (comboBox.IsLoaded)
                {
                    var focused = Keyboard.GetFocusedElement(comboBox.View) as DependencyObject;
                    if (focused != null && ItemsControlFromItemContainer(focused) == comboBox)
                        comboBox.Focus();
                }
            }

            comboBox.isDropDownOpenChanging = false;
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="FrameworkElement.Loaded"/> routed event.
        /// </summary>
        private static void HandleLoaded(DependencyObject dobj, RoutedEventData data)
        {
            var comboBox = (ComboBox)dobj;
            comboBox.viewSize = (comboBox.View == null) ? Size2.Zero : comboBox.View.Area.Size;
            comboBox.UpdateInputCapture(comboBox.IsDropDownOpen);
            comboBox.UpdateActualMaxDropDownHeight();
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="Mouse.MouseDownEvent"/> routed event.
        /// </summary>
        private static void HandleMouseDown(DependencyObject dobj, MouseDevice device, MouseButton button, RoutedEventData data)
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
        private static void HandleLostMouseCapture(DependencyObject dobj, RoutedEventData data)
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
        /// Occurs when the control handles a <see cref="Touch.TouchDownEvent"/> routed event.
        /// </summary>
        private static void HandleTouchDown(DependencyObject dobj, TouchDevice device, Int64 touchID, Double x, Double y, Single pressure, RoutedEventData data)
        {
            var comboBox = (ComboBox)dobj;
            if (comboBox.Ultraviolet.GetInput().IsMouseCursorAvailable)
                return;

            if (comboBox == Touch.GetCapturedNew(comboBox.View) && comboBox == data.OriginalSource)
            {
                comboBox.IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="Touch.LostNewTouchCaptureEvent"/> routed event.
        /// </summary>
        private static void HandleLostNewTouchCapture(DependencyObject dobj, RoutedEventData data)
        {
            var comboBox = (ComboBox)dobj;
            if (comboBox.Ultraviolet.GetInput().IsMouseCursorAvailable)
                return;

            if (comboBox != data.OriginalSource)
            {
                if (comboBox.ContainsDescendant(data.OriginalSource as DependencyObject))
                {
                    if (Touch.GetCapturedNew(comboBox.View) == null)
                    {
                        Touch.CaptureNew(comboBox.View, comboBox, CaptureMode.SubTree);
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

            var selectionBoxItem = (Object)null;
            var selectionBoxItemStringFormat = ItemStringFormat;

            var selectedItem = SelectedItem;
            var selectedContainer = (SelectedIndex < 0) ? null : ItemContainers[SelectedIndex];
            
            var contentControl = selectedContainer as ContentControl;
            if (contentControl != null)
            {
                selectionBoxItem = contentControl.Content;
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
        /// Updates the combo box's input capture state.
        /// </summary>
        private void UpdateInputCapture(Boolean captured)
        {
            if (captured)
            {
                if (Ultraviolet.GetInput().IsMouseCursorAvailable)
                {
                    Mouse.Capture(View, this, CaptureMode.SubTree);
                }
                else
                {
                    Touch.CaptureNew(View, this, CaptureMode.SubTree);
                }
            }
            else
            {
                if (Ultraviolet.GetInput().IsMouseCursorAvailable)
                {
                    if (IsMouseCaptured)
                        Mouse.Capture(View, null, CaptureMode.None);
                }
                else
                {
                    if (AreNewTouchesCaptured)
                        Touch.CaptureNew(View, null, CaptureMode.None);
                }
            }
        }

        /// <summary>
        /// Updates the value of the <see cref="ActualMaxDropDownHeight"/> property.
        /// </summary>
        private void UpdateActualMaxDropDownHeight()
        {
            var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var actualMaxDropDownHeight = Double.IsNaN(MaxDropDownHeight) ? Display.PixelsToDips(primary.Compositor.Height) / 3.0 : MaxDropDownHeight;
            if (actualMaxDropDownHeight != GetValue<Double>(ActualMaxDropDownHeightProperty))
            {
                SetValue(ActualMaxDropDownHeightPropertyKey, actualMaxDropDownHeight);
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
        /// <returns><see langword="true"/> if the specified object is a descendant of this combo box; otherwise, <see langword="false"/>.</returns>
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
        private Boolean isDropDownOpenChanging;
    }
}
