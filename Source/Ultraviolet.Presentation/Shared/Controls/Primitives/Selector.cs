using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a control that allows the user to select items.
    /// </summary>
    [UvmlKnownType]
    public abstract partial class Selector : ItemsControl
    {
        /// <summary>
        /// Initializes the <see cref="Selector"/> type.
        /// </summary>
        static Selector()
        {
            EventManager.RegisterClassHandler(typeof(Selector), SelectionChangedEvent, new UpfRoutedEventHandler(HandleSelectionChanged));
            EventManager.RegisterClassHandler(typeof(Selector), SelectedEvent, new UpfRoutedEventHandler(HandleSelected));
            EventManager.RegisterClassHandler(typeof(Selector), UnselectedEvent, new UpfRoutedEventHandler(HandleUnselected));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Selector"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The identifying name of this element within a layout.</param>
        public Selector(UltravioletContext uv, String name)
            : base(uv, name)
        {
            this.selection = new SelectionCollection(this);

            this.selection.CollectionItemAdded += selection_CollectionItemAdded;
            this.selection.CollectionItemRemoved += selection_CollectionItemRemoved;
            this.selection.CollectionReset += selection_CollectionReset;
        }

        /// <summary>
        /// Sets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="isSelected">A value indicating whether the specified element is selected.</param>
        public static void SetIsSelected(DependencyObject element, Boolean isSelected)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(IsSelectedProperty, isSelected);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is selected; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetIsSelected(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsSelectedProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the selected element contains keyboard focus.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is selected; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetIsSelectionActive(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsSelectionActiveProperty);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Selected"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public static void AddSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Unselected"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public static void AddUnselectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, UnselectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Selected"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the event handler.</param>
        /// <param name="handler">The event handler to remove from the element.</param>
        public static void RemoveSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.RemoveHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Unselected"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the event handler.</param>
        /// <param name="handler">The event handler to remove from the element.</param>
        public static void RemoveUnselectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.RemoveHandler(element, UnselectedEvent, handler);
        }

        /// <summary>
        /// Gets or sets the index of the first item in the current selection. If nothing is currently
        /// selected, this property will return negative one (-1).
        /// </summary>
        /// <value>A <see cref="Int32"/> representing the index of the first item in the current selection,
        /// or -1 if nothing is currently selected. The default value is -1.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectedIndexProperty"/></dpropField>
        ///		<dpropStylingName>selected-index</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Int32 SelectedIndex
        {
            get { return GetValue<Int32>(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the first item in the current selection.
        /// </summary>
        /// <value>The <see cref="Object"/> that is the first item in the current selection,
        /// or <see langword="null"/> if nothing is currently selected. The default value is <see langword="null"/></value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="SelectedItemProperty"/></dpropField>
        ///		<dpropStylingName>selected-item</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Object SelectedItem
        {
            get { return GetValue<Object>(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Occurs when the selector's selection is changed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="SelectionChangedEvent"/></revtField>
        ///		<revtStylingName>selection-changed</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedIndex"/> dependency property.</value>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(Selector),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.NegativeOne, HandleSelectedIndexChanged));

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectedItem"/> dependency property.</value>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(Selector),
            new PropertyMetadata<Object>(null, HandleSelectedItemChanged));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Primitives.Selector.IsSelected"/> attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Primitives.Selector.IsSelected"/> attached property.</value>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(Boolean), typeof(Selector), 
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// The private access key for the <see cref="P:Ultraviolet.Presentation.Controls.Primitives.Selector.IsSelectionActive"/> read-only attached dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(Boolean), typeof(Selector),
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.Primitives.Selector.IsSelectionActive"/> 
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.Primitives.Selector.IsSelectionActive"/> 
        /// attached property.</value>
        public static readonly DependencyProperty IsSelectionActiveProperty = IsSelectionActivePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionChanged"/> dependency property.</value>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Selected"/> attached event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Selected"/> attached event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when an item within a selector is selected.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="SelectedEvent"/></revtField>
        ///		<revtStylingName>selected</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Unselected"/> attached event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Controls.Primitives.Selector.Unselected"/> attached event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when an item within a selector is unselected.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="UnselectedEvent"/></revtField>
        ///		<revtStylingName>unselected</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));
        
        /// <inheritdoc/>
        protected override void OnItemsReset()
        {
            BeginChangeSelection();

            selection.Clear();

            EndChangeSelection();

            base.OnItemsReset();
        }

        /// <inheritdoc/>
        protected override void OnItemRemoved(DependencyObject container, Object item)
        {
            BeginChangeSelection();

            selection.Remove(container);

            EndChangeSelection();

            base.OnItemRemoved(container, item);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {

        }

        /// <summary>
        /// Occurs when an item is added to the list of selected items.
        /// </summary>
        /// <param name="container">The container of the item that was selected.</param>
        /// <param name="item">The item that was selected.</param>
        protected virtual void OnSelectedItemAdded(DependencyObject container, Object item)
        {

        }

        /// <summary>
        /// Occurs when an item is removed from the list of selected items.
        /// </summary>
        /// <param name="container">The container of the item that was unselected.</param>
        /// <param name="item">The item that was unselected.</param>
        protected virtual void OnSelectedItemRemoved(DependencyObject container, Object item)
        {

        }

        /// <summary>
        /// Occurs when the list of selected items is significantly changed.
        /// </summary>
        protected virtual void OnSelectedItemsChanged()
        {

        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            if (newView != null)
            {
                RebuildSelection();
            }
            base.OnViewChanged(oldView, newView);
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged()
        {
            base.OnItemsChanged();
        }

        /// <inheritdoc/>
        protected override void OnIsKeyboardFocusWithinChanged()
        {
            SetValue<Boolean>(IsSelectionActivePropertyKey, IsKeyboardFocusWithin);

            base.OnIsKeyboardFocusWithinChanged();
        }

        /// <summary>
        /// Unselects all of the control's items.
        /// </summary>
        protected void UnselectAllItems()
        {
            if (selection.Count == 0)
                return;

            BeginChangeSelection();

            foreach (var container in ItemContainers)
            {
                SetIsSelected(container, false);
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Unselects a single one of the control's items.
        /// </summary>
        /// <param name="item">The item to unselect.</param>
        protected void UnselectItem(Object item)
        {
            Contract.Require(item, nameof(item));

            var container = ItemContainerGenerator.ContainerFromItem(item);
            if (container == null)
                return;

            SetIsSelected(container, false);
        }

        /// <summary>
        /// Unselects a single one of the control's item containers.
        /// </summary>
        /// <param name="container">The item container to unselect.</param>
        protected void UnselectContainer(DependencyObject container)
        {
            Contract.Require(container, nameof(container));

            SetIsSelected(container, false);
        }

        /// <summary>
        /// Selects all of the control's items.
        /// </summary>
        protected void SelectAllItems()
        {
            if (selection.Count == Items.Count)
                return;

            BeginChangeSelection();

            foreach (var container in ItemContainers)
            {
                SetIsSelected(container, false);
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item to select.</param>
        protected void SelectItem(Object item)
        {
            Contract.Require(item, nameof(item));

            var container = ItemContainerGenerator.ContainerFromItem(item);
            if (container == null)
                return;

            if (selection.SelectedItem == item)
                return;

            SetIsSelected(container, true);
        }

        /// <summary>
        /// Selects the specified item container.
        /// </summary>
        /// <param name="container">The item container to select.</param>
        protected void SelectContainer(DependencyObject container)
        {
            Contract.Require(container, nameof(container));
            
            SetIsSelected(container, true);
        }

        /// <summary>
        /// Indicates that several changes are about to made to the control's selection, and that 
        /// only a single SelectionChange event should be raised.
        /// </summary>
        protected void BeginChangeSelection()
        {
            suspendSelectionChangedLevel++;
        }

        /// <summary>
        /// Indicates that the control is done changing its collection and raises a SelectionChange event.
        /// </summary>
        /// <param name="raiseChangedEvents">A value indicating whether events should be raised indicating that the selection changed.</param>
        protected void EndChangeSelection(Boolean raiseChangedEvents = true)
        {
            if (suspendSelectionChangedLevel == 0)
                throw new InvalidOperationException();

            if (--suspendSelectionChangedLevel == 0 && raiseChangedEvents)
            {
                UpdateSelectionPropertiesAndRaiseSelectionChanged();
                RaiseSelectedItemsChanged();
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex"/> dependency property changes.
        /// </summary>
        private static void HandleSelectedIndexChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var selector = (Selector)dobj;
            selector.BeginChangeSelection();
            selector.DigestImmediately(ItemsSourceProperty);
            selector.EndChangeSelection(false);

            var container = selector.ItemContainerGenerator.ContainerFromIndex(newValue);
            if (container == null)
                return;

            HandleSelectedContainerChanged(selector, container);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedItem"/> dependency property changes.
        /// </summary>
        private static void HandleSelectedItemChanged(DependencyObject dobj, Object oldValue, Object newValue)
        {
            var selector = (Selector)dobj;
            selector.BeginChangeSelection();
            selector.DigestImmediately(ItemsSourceProperty);
            selector.EndChangeSelection(false);

            var container = selector.ItemContainerGenerator.ContainerFromItem(newValue);
            if (container == null)
                return;

            HandleSelectedContainerChanged(selector, container);
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedIndex"/> or <see cref="SelectedItem"/>
        /// dependency properties change.
        /// </summary>
        private static void HandleSelectedContainerChanged(Selector selector, DependencyObject container)
        {
            var item = selector.ItemContainerGenerator.ItemFromContainer(container);
            if (item == null)
                return;

            if (GetIsSelected(container))
                return;

            selector.BeginChangeSelection();

            selector.UnselectAllItems();
            selector.SelectItem(item);

            selector.EndChangeSelection();
        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="SelectedEvent"/> routed event.
        /// </summary>
        private static void HandleSelected(DependencyObject dobj, RoutedEventData data)
        {
            var originalSource = data.OriginalSource as DependencyObject;
            if (originalSource == null || LogicalTreeHelper.GetParent(originalSource) != dobj)
                return;

            ((Selector)dobj).selection.Add(originalSource);
        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="UnselectedEvent"/> routed event.
        /// </summary>
        private static void HandleUnselected(DependencyObject dobj, RoutedEventData data)
        {
            var originalSource = data.OriginalSource as DependencyObject;
            if (originalSource == null || LogicalTreeHelper.GetParent(originalSource) != dobj)
                return;

            ((Selector)dobj).selection.Remove(originalSource);
        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        private static void HandleSelectionChanged(DependencyObject dobj, RoutedEventData data)
        {
            if (data.OriginalSource != dobj)
                return;

            ((Selector)dobj).OnSelectionChanged();
        }

        /// <summary>
        /// Rebuilds the selector's selection collection.
        /// </summary>
        private void RebuildSelection()
        {
            var wasChanged = false;
            BeginChangeSelection();

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i] as DependencyObject;
                if (item == null)
                    continue;
                
                var shouldBeSelected = (SelectedIndex == i);
                if (shouldBeSelected != item.GetValue<Boolean>(IsSelectedProperty))
                {
                    wasChanged = true;
                    item.SetValue(IsSelectedProperty, shouldBeSelected);
                }
            }

            EndChangeSelection(wasChanged);
        }

        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        private void RaiseSelectionChanged()
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectionChangedEvent);
            var evtData = RoutedEventData.Retrieve(this);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Calls the <see cref="OnSelectedItemAdded"/> method.
        /// </summary>
        /// <param name="container">The item's container.</param>
        /// <param name="item">The item that was added to the selection.</param>
        private void RaiseSelectedItemAdded(DependencyObject container, Object item)
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            OnSelectedItemAdded(container, item);
        }

        /// <summary>
        /// Calls the <see cref="OnSelectedItemRemoved"/> method.
        /// </summary>
        /// <param name="container">The item's container.</param>
        /// <param name="item">The item that was removed from the selection.</param>
        private void RaiseSelectedItemRemoved(DependencyObject container, Object item)
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            OnSelectedItemRemoved(container, item);
        }

        /// <summary>
        /// Calls the <see cref="OnSelectedItemsChanged"/> method.
        /// </summary>
        private void RaiseSelectedItemsChanged()
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            OnSelectedItemsChanged();
        }

        /// <summary>
        /// Updates the selector's selection properties.
        /// </summary>
        private void UpdateSelectionProperties()
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            SelectedIndex = selection.SelectedIndex;
            SelectedItem  = selection.SelectedItem;
        }

        /// <summary>
        /// Updates the selector's selection properties and immediately raises a SelectionChanged event.
        /// </summary>
        private void UpdateSelectionPropertiesAndRaiseSelectionChanged()
        {
            UpdateSelectionProperties();
            RaiseSelectionChanged();
        }

        /// <summary>
        /// Handles the selection collection's <see cref="INotifyCollectionChanged.CollectionItemAdded"/> event.
        /// </summary>
        private void selection_CollectionItemAdded(INotifyCollectionChanged collection, Int32? index, Object item)
        {
            UpdateSelectionPropertiesAndRaiseSelectionChanged();

            var container = ItemContainers[index.Value];
            RaiseSelectedItemAdded(container, item);
        }

        /// <summary>
        /// Handles the selection collection's <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event.
        /// </summary>
        private void selection_CollectionItemRemoved(INotifyCollectionChanged collection, Int32? index, Object item)
        {
            UpdateSelectionPropertiesAndRaiseSelectionChanged();

            var container = ItemContainers[index.Value];
            RaiseSelectedItemRemoved(container, item);
        }
        
        /// <summary>
        /// Handles the selection collection's <see cref="INotifyCollectionChanged.CollectionReset"/> event.
        /// </summary>
        private void selection_CollectionReset(INotifyCollectionChanged collection)
        {
            UpdateSelectionPropertiesAndRaiseSelectionChanged();
            RaiseSelectedItemsChanged();
        }

        // State values.
        private Int32 suspendSelectionChangedLevel;

        // The collection of selected items.
        private readonly SelectionCollection selection;
    }
}
