using System;
using System.Collections;
using System.Collections.Generic;
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
        public IEnumerable ItemsSource
        {
            get { return GetValue<IEnumerable>(ItemsSourceProperty); }
            set { SetValue<IEnumerable>(ItemsSourceProperty, value); }
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
        /// Occurs when the value of the <see cref="ItemsSource"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemsSourceChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ItemStringFormat"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItemStringFormatChanged;

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl),
            new DependencyPropertyMetadata(HandleItemsSourceChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="ItemStringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(String), typeof(ItemsControl),
            new DependencyPropertyMetadata(HandleItemStringFormatChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets the element which controls the layout of the control's item containers.
        /// </summary>
        protected internal Panel ItemsPanelElement
        {
            get { return itemsPanelElement; }
            internal set
            {
                if (itemsPanelElement != null)
                {
                    itemsPanelElement.Children.Clear();
                    itemsPanelElement.Parent = null;
                }

                itemsPanelElement = value;

                if (itemsPanelElement != null)
                {
                    itemsPanelElement.Parent = this;
                    foreach (var container in itemContainers)
                    {
                        itemsPanelElement.Children.Add(container);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the control's items panel, which is the panel that controls the layout of the control's item containers.
        /// </summary>
        /// <returns>The control's item panel.</returns>
        protected internal abstract Panel CreateItemsPanel();

        /// <summary>
        /// Creates a new item container element for this control.
        /// </summary>
        /// <returns>A <see cref="UIElement"/> which can be used to contain this control's items.</returns>
        protected abstract UIElement CreateItemContainer();

        /// <summary>
        /// Gets a value indicating whether the specified element is an item container for this control.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to evaluate.</param>
        /// <returns><c>true</c> if the specified element is an item container for this control; otherwise, <c>false</c>.</returns>
        protected abstract Boolean IsItemContainer(UIElement element);

        /// <summary>
        /// Gets a value indicating whether the specified element is the container for the specified item.
        /// </summary>
        /// <param name="container">The <see cref="UIElement"/> to evaluate.</param>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the specified element is the container for the specified item; otherwise, <c>false</c>.</returns>
        protected abstract Boolean IsItemContainerForItem(UIElement container, Object item);

        /// <summary>
        /// Associates an item container with the specified item.
        /// </summary>
        /// <param name="container">The item container element to associate with an item.</param>
        /// <param name="item">The item to associate with the specified item container.</param>
        protected abstract void AssociateItemContainerWithItem(UIElement container, Object item);

        /// <summary>
        /// Raises the <see cref="ItemsSourceChanged"/> event.
        /// </summary>
        protected virtual void OnItemsSourceChanged()
        {
            var temp = ItemsSourceChanged;
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
        /// Occurs when the value of the <see cref="ItemsSource"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleItemsSourceChanged(DependencyObject dobj)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.Items.SetItemsSource(itemControl.ItemsSource);
            itemControl.OnItemsSourceChanged();
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
            var current = itemContainers.First;
            var next    = current;
            
            while (current != null)
            {
                next = current.Next;
                RemoveItemContainer(current.Value, current);
                current = next;
            }

            foreach (var item in Items)
            {
                AddItemContainer(item);
            }
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was added to the collection.</param>
        private void ItemsCollectionItemAdded(INotifyCollectionChanged collection, Object item)
        {
            AddItemContainer(item);
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="item">The item that was removed from the collection.</param>
        private void ItemsCollectionItemRemoved(INotifyCollectionChanged collection, Object item)
        {
            var container = FindItemContainerForItem(item);
            if (container != null)
            {
                RemoveItemContainer(container);
            }
        }

        /// <summary>
        /// Adds an item container to this control for the specified item.
        /// </summary>
        /// <param name="item">The item for which to add an item container.</param>
        private void AddItemContainer(Object item)
        {
            var element   = item as UIElement;
            var container = element;

            // NOTE: visual parent is set when container is added to items panel.
            element.ChangeLogicalAndVisualParents(this, null);

            if (!IsItemContainer(element))
            {
                container = CreateItemContainer();
                PrepareContainerForItemOverride(container, item);
                AssociateItemContainerWithItem(container, item);
            }

            if (ItemsPanelElement != null)
                ItemsPanelElement.Children.Add(container);

            itemContainers.AddLast(container);
        }

        /// <summary>
        /// Removes an item container from this control.
        /// </summary>
        /// <param name="container">The item container to remove from the control.</param>
        /// <param name="node">Optionally, the linked list node that contains the item container.</param>
        private void RemoveItemContainer(UIElement container, LinkedListNode<UIElement> node = null)
        {
            AssociateItemContainerWithItem(container, null);

            if (ItemsPanelElement != null)
                ItemsPanelElement.Children.Remove(container);

            if (node != null)
                itemContainers.Remove(node);
            else
                itemContainers.Remove(container);
        }

        /// <summary>
        /// Searches the control's item containers for the container corresponding to the specified item.
        /// </summary>
        /// <param name="item">The item for which to find a container.</param>
        /// <returns>The item container for the specified item, or <c>null</c> if no such container exists in this control.</returns>
        private UIElement FindItemContainerForItem(Object item)
        {
            foreach (var container in itemContainers)
            {
                if (IsItemContainerForItem(container, item))
                    return container;
            }
            return null;
        }

        // Property values,
        private readonly ItemCollection items;
        private Panel itemsPanelElement;

        // The control's item containers for its current item collection.
        private readonly PooledLinkedList<UIElement> itemContainers = 
            new PooledLinkedList<UIElement>(8);
    }
}
