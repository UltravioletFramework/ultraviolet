using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="ListBox"/> control.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.ListBoxItem.xml")]
    public class ListBoxItem : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="ListBoxItem"/> type.
        /// </summary>
        static ListBoxItem()
        {
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Once));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Local));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ListBoxItem(UltravioletContext uv, String name)
            : base(uv, name)
        {
            HighlightOnSelect = true;
            HighlightOnTouchOver = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the list box item is currently selected.
        /// </summary>
        /// <value><see langword="true"/> if the item is currently selected; otherwise, <see langword="false"/>.
        /// The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="IsSelectedProperty"/></dpropField>
        ///     <dpropStylingName>selected</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsSelected
        {
            get { return GetValue<Boolean>(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the list box item is currently highlighted.
        /// </summary>
        public Boolean IsHighlighted
        {
            get { return HighlightOpacity > 0; }
        }
        
        /// <summary>
        /// Gets the opacity of the list box item's selection highlight.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the opacity of the list box item's
        /// selection highlight. The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HighlightOpacityProperty"/></dpropField>
        ///     <dpropStylingName>highlight-opacity</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double HighlightOpacity
        {
            get { return GetValue<Double>(HighlightOpacityProperty); }
            private set { SetValue(HighlightOpacityPropertyKey, value); }
        }

        /// <summary>
        /// Occurs when the list box item is selected.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="SelectedEvent"/></revtField>
        ///     <revtStylingName>selected</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        /// <summary>
        /// Occurs when the list box item is selected as a direct result of a user interaction.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="SelectedByUserEvent"/></revtField>
        ///     <revtStylingName>selected-by-user</revtStylingName>
        ///     <revtStrategy>Direct</revtStrategy>
        ///     <revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler SelectedByUser
        {
            add { AddHandler(SelectedByUserEvent, value); }
            remove { RemoveHandler(SelectedByUserEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsSelected"/> dependency property.</value>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));
        
        /// <summary>
        /// The private access key for the <see cref="HighlightOpacity"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey HighlightOpacityPropertyKey = DependencyProperty.RegisterReadOnly("HighlightOpacity", typeof(Double), typeof(ListBoxItem),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero));

        /// <summary>
        /// Identifies the <see cref="HighlightOpacity"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HighlightOpacity"/> dependency property.</value>
        public static readonly DependencyProperty HighlightOpacityProperty = HighlightOpacityPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="Selected"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Selected"/> event.</value>
        public static readonly RoutedEvent SelectedEvent = RoutedEventSystem.Register("Selected", "selected", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(ListBoxItem));

        /// <summary>
        /// Identifies the <see cref="SelectedByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedByUser"/> event.</value>
        public static readonly RoutedEvent SelectedByUserEvent = RoutedEventSystem.Register("SelectedByUser", "selected-by-user", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(ListBoxItem));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left && !data.Handled)
            {
                Select();
                OnSelectedByUser();

                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }
                
        /// <inheritdoc/>       
        protected override void OnIsMouseOverChanged()
        {
            UpdateHighlightOpacity();
            base.OnIsMouseOverChanged();
        }

        /// <inheritdoc/>       
        protected override void OnAreAnyTouchesOverChanged()
        {
            UpdateHighlightOpacity();
            base.OnAreAnyTouchesOverChanged();
        }
        
        /// <inheritdoc/>
        protected override void OnTouchTap(TouchDevice device, Int64 id, Double x, Double y, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (device.IsFirstTouchInGesture(id) && !data.Handled)
                {
                    Select();
                    OnSelectedByUser();

                    data.Handled = true;
                }
            }
            base.OnTouchTap(device, id, x, y, data);
        }

        /// <summary>
        /// Raises the <see cref="Selected"/> event.
        /// </summary>
        protected virtual void OnSelected()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectedEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="SelectedByUser"/> event.
        /// </summary>
        protected virtual void OnSelectedByUser()
        {
            var evtData = RoutedEventData.Retrieve(this);
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectedByUserEvent);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this list box item is highlighted when it is selected.
        /// </summary>
        protected Boolean HighlightOnSelect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this list box item is highlighted when the mouse enters its bounds.
        /// </summary>
        protected Boolean HighlightOnMouseOver
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this list box item is highlighted when a touch enters its bounds.
        /// </summary>
        protected Boolean HighlightOnTouchOver
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsSelected"/> dependency property changes.
        /// </summary>
        private static void HandleIsSelectedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var item = (ListBoxItem)dobj;
            if (item.IsSelected)
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.SelectedEvent);
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }
            else
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }

            item.UpdateHighlightOpacity();
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        private void Select()
        {
            if (IsSelected)
                return;

            var list = ItemsControl.ItemsControlFromItemContainer(this) as ListBox;
            if (list != null)
            {
                Focus();
                list.HandleItemClicked(this);
                OnSelected();
            }
        }

        /// <summary>
        /// Updates the opacity of the item's highlight.
        /// </summary>
        private void UpdateHighlightOpacity()
        {
            var isHighlit = false;

            if (!isHighlit && HighlightOnSelect && IsSelected)
                isHighlit = true;

            if (!isHighlit && HighlightOnMouseOver && IsMouseOver)
                isHighlit = true;

            if (!isHighlit && HighlightOnTouchOver && AreAnyTouchesOver)
            {
                var touchDevice = Ultraviolet.GetInput().GetFirstRegisteredTouchDevice();
                if (touchDevice != null)
                {
                    foreach (var touch in TouchesOver)
                    {
                        if (touchDevice.IsFirstTouchInGesture(touch))
                        {
                            isHighlit = true;
                            break;
                        }
                    }
                }
            }

            HighlightOpacity = isHighlit ? 1.0 : 0.0;
        }
    }
}
