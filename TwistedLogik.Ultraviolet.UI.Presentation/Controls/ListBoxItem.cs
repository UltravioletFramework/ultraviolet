using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="ListBox"/> control.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ListBoxItem.xml")]
    public class ListBoxItem : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItem"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ListBoxItem(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether the list box item is currently selected.
        /// </summary>
        public Boolean IsSelected
        {
            get { return GetValue<Boolean>(IsSelectedProperty); }
            set { SetValue<Boolean>(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem),
            new PropertyMetadata(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                var list = ItemsControl.ItemsControlFromItemContainer(this) as ListBox;
                if (list != null)
                {
                    list.HandleItemClicked(this);
                }
            }
            base.OnMouseDown(device, button, ref data);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsSelected"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleIsSelectedChanged(DependencyObject dobj)
        {
            var item = (ListBoxItem)dobj;
            if (item.IsSelected)
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.SelectedEvent);
                var evtData     = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
            }
            else
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData     = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
            }
        }

        /// <summary>
        /// Gets the opacity of the list box item's selection highlight.
        /// </summary>
        private Double HighlightOpacity
        {
            get { return IsSelected ? 1 : 0; }
        }
    }
}
