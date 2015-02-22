using System;
using System.Collections;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which presents the user with a list of items to select.
    /// </summary>
    public abstract class ItemsControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public ItemsControl(UltravioletContext uv, String id)
            : base(uv, id)
        {
            this.items = new ItemCollection(this);
            this.items.CollectionReset += ItemsCollectionReset;
            this.items.CollectionItemAdded += ItemsCollectionItemAdded;
            this.items.CollectionItemRemoved += ItemsCollectionItemRemoved;
        }

        /// <summary>
        /// Gets the collection that contains the control's items.
        /// </summary>
        public ItemCollection Items
        {
            get { return items; }
        }

        /// <summary>
        /// Gets or sets the collection which is used to generate the control's items.
        /// </summary>
        public IEnumerable ItemSource
        {
            get { return GetValue<IEnumerable>(ItemSourceProperty); }
            set { SetValue<IEnumerable>(ItemSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string which is used to format the control's items,
        /// if they are displayed as strings.
        /// </summary>
        public String ItemStringFormat
        {
            get { return GetValue<String>(ItemStringFormatProperty); }
            set { SetValue<String>(ItemStringFormatProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemSource"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemSourceChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ItemStringFormat"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemStringFormatChanged;

        /// <summary>
        /// Identifies the <see cref="ItemSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(ItemsControl),
            new DependencyPropertyMetadata(HandleItemSourceChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="ItemStringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(String), typeof(ItemsControl),
            new DependencyPropertyMetadata(HandleItemStringFormatChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Raises the <see cref="ItemSourceChanged"/> event.
        /// </summary>
        protected virtual void OnItemSourceChanged()
        {
            var temp = ItemSourceChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemStringFormatChanged"/> event.
        /// </summary>
        protected virtual void OnItemStringFormatChanged()
        {
            var temp = ItemStringFormatChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemSource"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleItemSourceChanged(DependencyObject dobj)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.OnItemSourceChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemStringFormat"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleItemStringFormatChanged(DependencyObject dobj)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.OnItemStringFormatChanged();
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionReset"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        private void ItemsCollectionReset(INotifyCollectionChanged collection)
        {
            // TODO
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was added to the collection.</param>
        private void ItemsCollectionItemAdded(INotifyCollectionChanged collection, Object item)
        {
            // TODO
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was removed from the collection.</param>
        private void ItemsCollectionItemRemoved(INotifyCollectionChanged collection, Object item)
        {
            // TODO
        }

        // Property values,
        private readonly ItemCollection items;
    }
}
