using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="TabControl"/> control.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.TabItem.xml")]
    public class TabItem : HeaderedContentControl
    {
        /// <summary>
        /// Initializes the <see cref="TabItem"/> control.
        /// </summary>
        static TabItem()
        {
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(TabItem), new PropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(TabItem), new PropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Local));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabItem), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Contained));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TabItem(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether the tab item is currently selected.
        /// </summary>
        /// <value><see langword="true"/> if the tab item is currently selected; otherwise, 
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
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
        /// Occurs when the tab item is selected.
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
        /// Occurs when the tab item is selected as a direct result of a user interaction.
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
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(TabItem),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));

        /// <summary>
        /// Identifies the <see cref="Selected"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="Selected"/> event.</value>
        public static readonly RoutedEvent SelectedEvent = RoutedEventSystem.Register("Selected", "selected", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(TabItem));

        /// <summary>
        /// Identifies the <see cref="SelectedByUser"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedByUser"/> event.</value>
        public static readonly RoutedEvent SelectedByUserEvent = RoutedEventSystem.Register("SelectedByUser", "selected-by-user", RoutingStrategy.Direct,
            typeof(UpfRoutedEventHandler), typeof(TabItem));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left && !data.Handled)
            {
                Focus();
                OnSelectedByUser();

                data.Handled = true;
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchDown(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (!Ultraviolet.GetInput().IsMouseCursorAvailable)
            {
                if (!data.Handled && device.IsFirstTouchInGesture(id))
                {
                    Focus();
                    OnSelectedByUser();

                    data.Handled = true;
                }
            }
            base.OnTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnContentChanged(Object oldValue, Object newValue)
        {
            var tabControl = ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
            if (tabControl != null)
            {
                tabControl.HandleItemContentChanged(this);
            }
            base.OnContentChanged(oldValue, newValue);
        }

        /// <inheritdoc/>
        protected override void OnPreviewGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            if (!data.Handled && newFocus == this)
            {
                Select();
            }
            base.OnPreviewGotKeyboardFocus(device, oldFocus, newFocus, data);
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
        /// Occurs when the value of the <see cref="IsSelected"/> dependency property changes.
        /// </summary>
        private static void HandleIsSelectedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var item = (TabItem)dobj;
            if (item.IsSelected)
            {
                item.Classes.Set("selected");

                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.SelectedEvent);
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }
            else
            {
                item.Classes.Remove("selected");

                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        private void Select()
        {
            if (IsSelected)
                return;

            var tabControl = ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
            if (tabControl != null)
            {
                tabControl.HandleItemClicked(this);
                OnSelected();
            }
        }
    }
}
