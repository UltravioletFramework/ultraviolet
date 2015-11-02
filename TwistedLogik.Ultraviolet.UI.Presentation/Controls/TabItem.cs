using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Nucleus;

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
        public Boolean IsSelected
        {
            get { return GetValue<Boolean>(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selected'.</remarks>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(TabItem),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));

        /// <inheritdoc/>
        protected override void OnGenericInteraction(UltravioletResource device, ref RoutedEventData data)
        {
            if (!data.Handled)
            {
                Select();
                data.Handled = true;
            }
            base.OnGenericInteraction(device, ref data);
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
            var list = ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
            if (list != null)
            {
                Focus();
                list.HandleItemClicked(this);
            }
        }
    }
}
