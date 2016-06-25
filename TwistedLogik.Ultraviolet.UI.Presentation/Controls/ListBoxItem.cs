using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents an item in a <see cref="ListBox"/> control.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.ListBoxItem.xml")]
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

        /// <inheritdoc/>
        protected override void OnGenericInteraction(UltravioletResource device, RoutedEventData data)
        {
            if (!data.Handled)
            {
                Select();
                data.Handled = true;
            }
            base.OnGenericInteraction(device, data);
        }
        
        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseDevice device, RoutedEventData data)
        {
            if (HighlightOnMouseOver)
            {
                HighlightOpacity = 1.0;
            }
            base.OnMouseEnter(device, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseDevice device, RoutedEventData data)
        {
            if (HighlightOnMouseOver)
            {
                HighlightOpacity = 0.0;
            }
            base.OnMouseLeave(device, data);
        }

        /// <inheritdoc/>
        protected override void OnFingerDown(TouchDevice device, Int64 fingerID, Double x, Double y, Single pressure, RoutedEventData data)
        {
            HighlightOpacity = 1.0;
            base.OnFingerDown(device, fingerID, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnFingerUp(TouchDevice device, Int64 fingerID, Double x, Double y, Single pressure, RoutedEventData data)
        {
            HighlightOpacity = (HighlightOnSelect && IsSelected) || (HighlightOnMouseOver && IsMouseDirectlyOver) ? 1.0 : 0.0;
            base.OnFingerUp(device, fingerID, x, y, pressure, data);
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
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }
            else
            {
                var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(Selector.UnselectedEvent);
                var evtData = RoutedEventData.Retrieve(dobj);
                evtDelegate(dobj, evtData);
            }

            if (item.HighlightOnSelect)
                item.HighlightOpacity = newValue ? 1.0 : 0.0;
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        private void Select()
        {
            var list = ItemsControl.ItemsControlFromItemContainer(this) as ListBox;
            if (list != null)
            {
                Focus();
                list.HandleItemClicked(this);
            }
        }
    }
}
