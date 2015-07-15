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
    public class ComboBox : Selector
    {
        /// <summary>
        /// Initializes the <see cref="ComboBox"/> type.
        /// </summary>
        static ComboBox()
        {
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
            new PropertyMetadata<Double>(1080.0 / 3.0, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="ActualMaxDropDownHeight"/> read-only dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-box-item'.</remarks>
        private static readonly DependencyPropertyKey ActualMaxDropDownHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualMaxDropDownHeight", typeof(Double), typeof(ComboBox),
            new PropertyMetadata<Double>());

        /// <summary>
        /// Identifies the <see cref="ActualMaxDropDownHeight"/> dependency property.
        /// </summary>
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
        /// <remarks>The styling name of this dependency property is 'selection-box-item'.</remarks>
        private static readonly DependencyPropertyKey SelectionBoxItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItem", typeof(Object), typeof(ComboBox),
            new PropertyMetadata<Boolean>(null));

        /// <summary>
        /// Identifies the <see cref="SelectionBoxItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionBoxItemProperty = SelectionBoxItemPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="SelectionBoxItemStringFormat"/> read-only dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-box-item-string-format'.</remarks>
        private static readonly DependencyPropertyKey SelectionBoxItemStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemStringFormat", typeof(String), typeof(ComboBox),
            new PropertyMetadata<String>(null));

        /// <summary>
        /// Identifies the <see cref="SelectionBoxItemStringFormat"/> dependency property.
        /// </summary>
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

                var actualMaxDropDownHeight = Math.Min(comboBox.MaxDropDownHeight, primary.ClientSize.Height / 3.0);
                if (actualMaxDropDownHeight != comboBox.GetValue<Double>(ActualMaxDropDownHeightProperty))
                {
                    comboBox.SetValue<Double>(ActualMaxDropDownHeightPropertyKey, actualMaxDropDownHeight);
                }

                Mouse.Capture(comboBox.View, comboBox, CaptureMode.SubTree);

                if (comboBox.PART_Arrow != null)
                {
                    comboBox.PART_Arrow.Classes.Add("combobox-arrow-open");
                    comboBox.PART_Arrow.Classes.Remove("combobox-arrow-closed");
                }

                comboBox.UpdateVisualState();
                comboBox.OnDropDownOpened();
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
            }
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
        /// Occurs when the control handles a <see cref="Mouse.LostMouseCapture"/> routed event.
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
        /// Updates the selection box.
        /// </summary>
        private void UpdateSelectionBox()
        {
            var selectionBoxItem             = (Object)null;
            var selectionBoxItemStringFormat = ItemStringFormat;

            var contentControl = SelectedItem as ContentControl;
            if (contentControl != null)
            {
                selectionBoxItem             = contentControl.Content;
                selectionBoxItemStringFormat = contentControl.ContentStringFormat;
            }

            SetValue<Object>(SelectionBoxItemPropertyKey, selectionBoxItem ?? String.Empty);
            SetValue<String>(SelectionBoxItemStringFormatPropertyKey, selectionBoxItemStringFormat);
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
    }
}
