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
            HighlightOnSelect = true;
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
        /// <remarks>The styling name of this dependency property is 'selected'.</remarks>
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsSelectedChanged));

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            if (!data.Handled && button == MouseButton.Left)
            {
                var list = ItemsControl.ItemsControlFromItemContainer(this) as ListBox;
                if (list != null)
                {
                    list.HandleItemClicked(this);
                }
            }

            data.Handled = true;

            base.OnMouseDown(device, button, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, ref RoutedEventData data)
        {
            if (HighlightOnMouseOver)
            {
                HighlightOpacity = 1.0;
            }
            base.OnMouseEnter(device, ref data);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseDevice device, ref RoutedEventData data)
        {
            if (HighlightOnMouseOver)
            {
                HighlightOpacity = 0.0;
            }
            base.OnMouseLeave(device, ref data);
        }

        /// <summary>
        /// Gets the opacity of the list box item's selection highlight.
        /// </summary>
        protected Double HighlightOpacity
        {
            get { return GetValue<Double>(HighlightOpacityProperty); }
            private set { SetValue<Double>(HighlightOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this list box is highlighted when it is selected.
        /// </summary>
        protected Boolean HighlightOnSelect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this list box is highlighted when the mouse enters its bounds.
        /// </summary>
        protected Boolean HighlightOnMouseOver
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
                var evtData     = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
            }
            else
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData     = new RoutedEventData(dobj);

                evtDelegate(dobj, ref evtData);
            }

            if (item.HighlightOnSelect)
                item.HighlightOpacity = newValue ? 1.0 : 0.0;
        }

        /// <summary>
        /// Identifies the <see cref="HighlightOpacity"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty HighlightOpacityProperty = DependencyProperty.Register("HighlightOpacity", typeof(Double), typeof(ListBoxItem),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero));
    }
}
