using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
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
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ItemsControl(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.itemContainerGenerator = new ItemContainerGenerator(this);

            this.items = new ItemCollection(this);
            this.items.CollectionReset += ItemsCollectionReset;
            this.items.CollectionItemAdded += ItemsCollectionItemAdded;
            this.items.CollectionItemRemoved += ItemsCollectionItemRemoved;
        }

        /// <summary>
        /// Gets the <see cref="ItemsControl"/> which is logically the owner of the specified item container.
        /// </summary>
        /// <param name="container">The item container to evaluate.</param>
        /// <returns>The <see cref="ItemsControl"/> which is logically the owner 
        /// of <paramref name="container"/>, or <c>null</c> if there is no such control.</returns>
        public static ItemsControl ItemsControlFromItemContainer(DependencyObject container)
        {
            var uiElement = container as UIElement;
            if (uiElement == null)
                return null;

            var parent = LogicalTreeHelper.GetParent(uiElement);
            while (parent != null)
            {
                var itemsControl = parent as ItemsControl;
                if (itemsControl != null && itemsControl.IsItemContainer(uiElement))
                    return itemsControl;

                parent = LogicalTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="ItemContainerGenerator"/> for this control.
        /// </summary>
        public ItemContainerGenerator ItemContainerGenerator
        {
            get { return itemContainerGenerator; }
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
        /// Gets a value indicating whether the control has any items.
        /// </summary>
        public Boolean HasItems
        {
            get { return GetValue<Boolean>(HasItemsProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'items-source'.</remarks>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl),
            new PropertyMetadata<IEnumerable>(HandleItemsSourceChanged));

        /// <summary>
        /// Identifies the <see cref="ItemStringFormat"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'item-string-format'.</remarks>
        public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(String), typeof(ItemsControl),
            new PropertyMetadata<String>());

        /// <summary>
        /// The private access key for the <see cref="HasItems"/> read-only dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'has-items'.</remarks>
        private static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasItems", typeof(Boolean), typeof(ItemsControl),
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// Identifies the <see cref="HasItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasItemsProperty = HasItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the control's list of item containers.
        /// </summary>
        internal List<DependencyObject> ItemContainers
        {
            get { return itemContainers; }
        }

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
                        var uiElement = container as UIElement;
                        if (uiElement != null)
                        {
                            itemsPanelElement.Children.Add(uiElement);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return itemContainers.Count; }
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            var container = itemContainers[childIndex];
            return (UIElement)ItemContainerGenerator.ItemFromContainer(container);
        }

        /// <summary>
        /// Creates the control's items panel, which is the panel that controls the layout of the control's item containers.
        /// </summary>
        /// <returns>The control's item panel.</returns>
        protected internal abstract Panel CreateItemsPanel();

        /// <summary>
        /// Creates an item container which can be used to display an item for this control.
        /// </summary>
        /// <returns>The item container that was created.</returns>
        protected abstract DependencyObject GetContainerForItemOverride();

        /// <summary>
        /// Gets a value indicating whether the specified element is an item container for this control.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to evaluate.</param>
        /// <returns><c>true</c> if the specified element is an item container for this control; otherwise, <c>false</c>.</returns>
        protected abstract Boolean IsItemContainer(DependencyObject element);

        /// <summary>
        /// Gets a value indicating whether the specified element is the container for the specified item.
        /// </summary>
        /// <param name="container">The <see cref="UIElement"/> to evaluate.</param>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><c>true</c> if the specified element is the container for the specified item; otherwise, <c>false</c>.</returns>
        protected abstract Boolean IsItemContainerForItem(DependencyObject container, Object item);

        /// <summary>
        /// Occurs when the value of the <see cref="ItemsSource"/> dependency property changes.
        /// </summary>
        private static void HandleItemsSourceChanged(DependencyObject dobj, IEnumerable oldValue, IEnumerable newValue)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.Items.SetItemsSource(itemControl.ItemsSource);
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionReset"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        private void ItemsCollectionReset(INotifyCollectionChanged collection)
        {
            foreach (var item in itemContainers)
            {
                RemoveItemContainer(item, true);
            }

            itemContainers.Clear();

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
            var uiElement = item as UIElement;
            if (uiElement == null)
                return;

            var container = (DependencyObject)uiElement;

            // NOTE: visual parent is set when container is added to items panel.
            uiElement.ChangeLogicalAndVisualParents(this, null);

            if (!IsItemContainer(uiElement))
            {
                container = GetContainerForItemOverride();
                PrepareContainerForItemOverride(container, item);
            }
            itemContainerGenerator.AssociateContainerWithItem(container, item);

            if (ItemsPanelElement != null)
            {
                var containerElement = container as UIElement;
                if (containerElement != null)
                {
                    ItemsPanelElement.Children.Add(containerElement);
                }
            }

            itemContainers.Add(container);

            SetValue<Boolean>(HasItemsPropertyKey, true);
        }

        /// <summary>
        /// Removes an item container from this control.
        /// </summary>
        /// <param name="container">The item container to remove from the control.</param>
        /// <param name="preserveInCollection">A value used to indicate whether to keep the removed item in the containers list.</param>
        private void RemoveItemContainer(DependencyObject container, Boolean preserveInCollection = false)
        {
            if (!IsItemContainer(container))
            {
                PrepareContainerForItemOverride(container, null);
            }
            itemContainerGenerator.AssociateContainerWithItem(container, null);

            if (ItemsPanelElement != null)
            {
                var uiElement = container as UIElement;
                if (uiElement != null)
                {
                    ItemsPanelElement.Children.Remove(uiElement);
                }
            }

            if (!preserveInCollection)
                itemContainers.Remove(container);

            SetValue<Boolean>(HasItemsPropertyKey, itemContainers.Count > 0);
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element used to display the specified item.</param>
        /// <param name="item">The item being displayed by the specified element.</param>
        private void PrepareContainerForItemOverride(DependencyObject element, Object item)
        {
            var container = element as IItemContainer;
            if (container != null)
            {
                container.PrepareItemContainer(item);
            }
        }

        /// <summary>
        /// Searches the control's item containers for the container corresponding to the specified item.
        /// </summary>
        /// <param name="item">The item for which to find a container.</param>
        /// <returns>The item container for the specified item, or <c>null</c> if no such container exists in this control.</returns>
        private DependencyObject FindItemContainerForItem(Object item)
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
        private ItemContainerGenerator itemContainerGenerator;

        // The control's item containers for its current item collection.
        private readonly List<DependencyObject> itemContainers = 
            new List<DependencyObject>(8);
    }
}
