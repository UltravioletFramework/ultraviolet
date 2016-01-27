using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="TabControl"/> control.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.TabItem.xml")]
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
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsSelected"/> dependency property.</value>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(TabItem),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));

        /// <inheritdoc/>
        protected override void OnGenericInteraction(UltravioletResource device, ref RoutedEventData data)
        {
            if (!data.Handled)
            {
                Focus();
                data.Handled = true;
            }
            base.OnGenericInteraction(device, ref data);
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
        protected override void OnPreviewGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            if (!data.Handled && newFocus == this)
            {
                Select();
            }
            base.OnPreviewGotKeyboardFocus(device, oldFocus, newFocus, ref data);
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
                var evtData = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
            }
            else
            {
                item.Classes.Remove("selected");

                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
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
            }
        }
    }
}
