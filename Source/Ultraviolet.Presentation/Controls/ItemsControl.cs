using System;
using System.Collections;
using System.Collections.Generic;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a control which presents the user with a list of items to select.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Items")]
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
        /// of <paramref name="container"/>, or <see langword="null"/> if there is no such control.</returns>
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
        /// <value>The <see cref="ItemContainerGenerator"/> which generates this control's item containers.</value>
        public ItemContainerGenerator ItemContainerGenerator
        {
            get { return itemContainerGenerator; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> used to display the control's items.
        /// </summary>
        /// <value>A <see cref="DataTemplate"/> which is used to display the control's items.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ItemTemplateProperty"/></dpropField>
        ///     <dpropStylingName>item-template</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public DataTemplate ItemTemplate
        {
            get { return GetValue<DataTemplate>(ItemTemplateProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom logic which determines which template is used to display each of the control's items.
        /// </summary>
        /// <value>A <see cref="DataTemplateSelector"/> which implements the control's selection logic for data templates.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ItemTemplateSelectorProperty"/></dpropField>
        ///     <dpropStylingName>item-template-selector</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return GetValue<DataTemplateSelector>(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets the collection that contains the control's items.
        /// </summary>
        /// <value>A <see cref="ItemCollection"/> that contains the control's items.</value>
        public ItemCollection Items
        {
            get { return items; }
        }

        /// <summary>
        /// Gets or sets the collection which is used to generate the control's items.
        /// </summary>
        /// <value>An <see cref="IEnumerable"/> which is used to generate the control's items.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ItemsSourceProperty"/></dpropField>
        ///     <dpropStylingName>items-source</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public IEnumerable ItemsSource
        {
            get { return GetValue<IEnumerable>(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string which is used to format the control's items,
        /// if they are displayed as strings.
        /// </summary>
        /// <value>A format string that specifies how to format the control's items. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ItemStringFormatProperty"/></dpropField>
        ///     <dpropStylingName>item-string-format</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String ItemStringFormat
        {
            get { return GetValue<String>(ItemStringFormatProperty); }
            set { SetValue(ItemStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the control has any items.
        /// </summary>
        /// <value><see langword="true"/> if the control has items; otherwise, <see langword="false"/>.
        /// The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HasItemsProperty"/></dpropField>
        ///     <dpropStylingName>has-items</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean HasItems
        {
            get { return GetValue<Boolean>(HasItemsProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ItemTemplate"/> dependency property.</value>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.RegisterAttached("ItemTemplate", typeof(DataTemplate), typeof(ItemsControl),
            new PropertyMetadata<DataTemplate>());

        /// <summary>
        /// Identifies the <see cref="ItemTemplateSelector"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ItemTemplateSelector"/> dependency property.</value>
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl),
            new PropertyMetadata<DataTemplateSelector>());

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ItemsSource"/> dependency property.</value>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl),
            new PropertyMetadata<IEnumerable>(HandleItemsSourceChanged));

        /// <summary>
        /// Identifies the <see cref="ItemStringFormat"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ItemStringFormat"/> dependency property.</value>
        public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(String), typeof(ItemsControl),
            new PropertyMetadata<String>(HandleItemStringFormatChanged));

        /// <summary>
        /// The private access key for the <see cref="HasItems"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasItems", typeof(Boolean), typeof(ItemsControl),
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// Identifies the <see cref="HasItems"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HasItems"/> dependency property.</value>
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
        /// <param name="obj">The object to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is an item container for this control; otherwise, <see langword="false"/>.</returns>
        protected abstract Boolean IsItemContainer(Object obj);

        /// <summary>
        /// Gets a value indicating whether the specified element is the container for the specified item.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="item">The item to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is the container for the specified item; otherwise, <see langword="false"/>.</returns>
        protected abstract Boolean IsItemContainerForItem(Object obj, Object item);

        /// <inheritdoc/>
        protected override void CacheLayoutParametersOverride()
        {
            DigestImmediatelyIfDataBound(ItemsSourceProperty);

            base.CacheLayoutParametersOverride();
        }

        /// <summary>
        /// Called when the <see cref="Items"/> property changes.
        /// </summary>
        protected virtual void OnItemsChanged()
        {

        }

        /// <summary>
        /// Called when the content of the <see cref="Items"/> collection is completely reset.
        /// </summary>
        protected virtual void OnItemsReset()
        {

        }

        /// <summary>
        /// Called when an item is added to the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="container">The container of the item that was added.</param>
        /// <param name="item">The item that was added.</param>
        protected virtual void OnItemAdded(DependencyObject container, Object item)
        {

        }

        /// <summary>
        /// Called when an item is removed from the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="container">The container of the item that was removed.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnItemRemoved(DependencyObject container, Object item)
        {

        }

        /// <summary>
        /// Called when the <see cref="ItemsSource"/> property changes.
        /// </summary>
        /// <param name="oldValue">The property's old value.</param>
        /// <param name="newValue">The property's new value.</param>
        protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {

        }

        /// <summary>
        /// Called when the <see cref="ItemStringFormat"/> property changes.
        /// </summary>
        /// <param name="oldValue">The property's old value.</param>
        /// <param name="newValue">The property's new value.</param>
        protected virtual void OnItemStringFormatChanged(String oldValue, String newValue)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemsSource"/> dependency property changes.
        /// </summary>
        private static void HandleItemsSourceChanged(DependencyObject dobj, IEnumerable oldValue, IEnumerable newValue)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.Items.SetItemsSource(itemControl.ItemsSource);
            itemControl.OnItemsSourceChanged(oldValue, newValue);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ItemStringFormat"/> dependency property changes.
        /// </summary>
        private static void HandleItemStringFormatChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var itemControl = (ItemsControl)dobj;
            itemControl.OnItemStringFormatChanged(oldValue, newValue);
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

            for (int i = 0; i < Items.Count; i++)
            {
                AddItemContainer(i, Items[i]);
            }

            OnItemsChanged();
            OnItemsReset();
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="index">The index of the item that was added to the collection.</param>
        /// <param name="item">The item that was added to the collection.</param>
        private void ItemsCollectionItemAdded(INotifyCollectionChanged collection, Int32? index, Object item)
        {
            var container = AddItemContainer(index.GetValueOrDefault(), item);
            OnItemsChanged();
            OnItemAdded(container, item);
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event for the control's item collection.
        /// </summary>
        /// <param name="collection">The collection that raised the event.</param>
        /// <param name="index">The index of the item thatg was removed from the collection.</param>
        /// <param name="item">The item that was removed from the collection.</param>
        private void ItemsCollectionItemRemoved(INotifyCollectionChanged collection, Int32? index, Object item)
        {
            var container = ItemContainers[index.Value];
            if (container != null)
            {
                RemoveItemContainer(container);
                OnItemsChanged();
                OnItemRemoved(container, item);
            }
        }

        /// <summary>
        /// Adds an item container to this control for the specified item.
        /// </summary>
        /// <param name="index">The index of the item being added.</param>
        /// <param name="item">The item for which to add an item container.</param>
        /// <returns>The item container that was added.</returns>
        private DependencyObject AddItemContainer(Int32 index, Object item)
        {
            if (ItemsPanelElement == null)
                return null;

            var container = default(DependencyObject);
            if (IsItemContainer(item))
            {
                container = (DependencyObject)item;
            }
            else
            {
                container = GetContainerForItemOverride();
                
                var template = ItemTemplateSelector?.SelectTemplate(item, container) ?? ItemTemplate;
                if (template != null)
                {
                    var templateInstance = template.LoadContent(item, item?.GetType());
                    PrepareContainerForItemOverride(container, templateInstance);
                }
                else
                {
                    PrepareContainerForItemOverride(container, item);
                }
            }

            itemContainerGenerator.AssociateContainerWithItem(container, item);
            itemContainers.Insert(index, container);

            var containerElement = container as UIElement;
            if (containerElement != null)
            {
                ItemsPanelElement.Children.Insert(index, containerElement);
                containerElement.ChangeLogicalParent(this);
            }

            SetValue(HasItemsPropertyKey, true);

            return container;
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

            SetValue(HasItemsPropertyKey, itemContainers.Count > 0);
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
        
        // Property values,
        private readonly ItemCollection items;
        private Panel itemsPanelElement;
        private ItemContainerGenerator itemContainerGenerator;

        // The control's item containers for its current item collection.
        private readonly List<DependencyObject> itemContainers = new List<DependencyObject>(8);
    }
}
