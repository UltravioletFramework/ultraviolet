using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
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

            this.selection.CollectionItemAdded   += selection_CollectionItemAdded;
            this.selection.CollectionItemRemoved += selection_CollectionItemRemoved;
            this.selection.CollectionReset       += selection_CollectionReset;
        }

        /// <summary>
        /// Sets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="isSelected">A value indicating whether the specified element is selected.</param>
        public static void SetIsSelected(DependencyObject element, Boolean isSelected)
        {
            Contract.Require(element, "element");

            element.SetValue<Boolean>(IsSelectedProperty, isSelected);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is selected; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsSelected(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsSelectedProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the selected element contains keyboard focus.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is selected; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsSelectionActive(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsSelectionActiveProperty);
        }

        /// <summary>
        /// Adds a handler for the <see cref="SelectedEvent"/> attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public static void AddSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="UnselectedEvent"/> attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public static void AddUnselectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, UnselectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="SelectedEvent"/> attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the event handler.</param>
        /// <param name="handler">The event handler to remove from the element.</param>
        public static void RemoveSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.RemoveHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="UnselectedEvent"/> attached event from the specified element.
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
        public Int32 SelectedIndex
        {
            get { return GetValue<Int32>(SelectedIndexProperty); }
            set { SetValue<Int32>(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the first item in the current selection.
        /// </summary>
        public Object SelectedItem
        {
            get { return GetValue<Object>(SelectedItemProperty); }
            set { SetValue<Object>(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Occurs when the selector's selection is changed.
        /// </summary>
        public event UpfRoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selected-index'.</remarks>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(Selector),
            new PropertyMetadata<Int32>(CommonBoxedValues.Int32.NegativeOne, HandleSelectedIndexChanged));

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selected-item'.</remarks>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(Selector),
            new PropertyMetadata<Object>());

        /// <summary>
        /// Identifies the IsSelected attached property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selected'.</remarks>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(Boolean), typeof(Selector), 
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// The private access key for the IsSelectionActive read-only attached dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(Boolean), typeof(Selector),
            new PropertyMetadata<Boolean>());

        /// <summary>
        /// Identifies the IsSelectionActive attached property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'selection-active'.</remarks>
        public static readonly DependencyProperty IsSelectionActiveProperty = IsSelectionActivePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'selection-changed'.</remarks>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the Selected attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'selected'.</remarks>
        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the Unselected attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is 'unselected'.</remarks>
        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {

        }

        /// <summary>
        /// Occurs when an item is added to the list of selected items.
        /// </summary>
        /// <param name="item">The item that was selected.</param>
        protected virtual void OnSelectedItemAdded(Object item)
        {

        }

        /// <summary>
        /// Occurs when an item is removed from the list of selected items.
        /// </summary>
        /// <param name="item">The item that was unselected.</param>
        protected virtual void OnSelectedItemRemoved(Object item)
        {

        }

        /// <summary>
        /// Occurs when the list of selected items is significantly changed.
        /// </summary>
        protected virtual void OnSelectedItemsChanged()
        {

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

            foreach (var item in Items)
            {
                var container = ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    SetIsSelected(container, false);
                }
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Unselects a single one of the control's items.
        /// </summary>
        /// <param name="item">The item to unselect.</param>
        protected void UnselectItem(Object item)
        {
            Contract.Require(item, "item");

            var container = ItemContainerGenerator.ContainerFromItem(item);
            if (container == null)
                return;

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

            foreach (var item in Items)
            {
                var container = ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    SetIsSelected(container, true);
                }
            }

            EndChangeSelection();
        }

        /// <summary>
        /// Selects the specified item and unselects any other items.
        /// </summary>
        /// <param name="item">The item to select.</param>
        protected void SelectItem(Object item)
        {
            Contract.Require(item, "item");

            var container = ItemContainerGenerator.ContainerFromItem(item);
            if (container == null)
                return;

            if (selection.SelectedItem == item)
                return;

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
        protected void EndChangeSelection()
        {
            if (suspendSelectionChangedLevel == 0)
                throw new InvalidOperationException();

            if (--suspendSelectionChangedLevel == 0)
            {
                RaiseSelectedItemsChanged();
                UpdateSelectionPropertiesAndRaiseSelectionChanged();
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedIndex"/> dependency property changes.
        /// </summary>
        private static void HandleSelectedIndexChanged(DependencyObject dobj, Int32 oldValue, Int32 newValue)
        {
            var selector = (Selector)dobj;

            var index     = selector.SelectedIndex;
            var container = selector.ItemContainerGenerator.ContainerFromIndex(index);
            if (container == null)
                return;

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
        private static void HandleSelected(DependencyObject dobj, ref RoutedEventData data)
        {
            var selector = (Selector)dobj;
            var container = data.OriginalSource as DependencyObject;
            if (container != null)
            {
                selector.selection.Add(container);
            }
        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="UnselectedEvent"/> routed event.
        /// </summary>
        private static void HandleUnselected(DependencyObject dobj, ref RoutedEventData data)
        {
            var selector = (Selector)dobj;
            var container = data.OriginalSource as DependencyObject;
            if (container != null)
            {
                selector.selection.Remove(container);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionChanged"/> dependency property changes.
        /// </summary>
        private static void HandleSelectionChanged(DependencyObject dobj, ref RoutedEventData data)
        {
            if (data.OriginalSource == dobj)
            {
                ((Selector)dobj).OnSelectionChanged();
            }
        }

        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        private void RaiseSelectionChanged()
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectionChangedEvent);
            var evtData     = new RoutedEventData(this);
            evtDelegate(this, ref evtData);
        }

        /// <summary>
        /// Calls the <see cref="OnSelectedItemAdded"/> method.
        /// </summary>
        /// <param name="item">The item that was added to the selection.</param>
        private void RaiseSelectedItemAdded(Object item)
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            OnSelectedItemAdded(item);
        }

        /// <summary>
        /// Calls the <see cref="OnSelectedItemRemoved"/> method.
        /// </summary>
        /// <param name="item">The item that was removed from the selection.</param>
        private void RaiseSelectedItemRemoved(Object item)
        {
            if (suspendSelectionChangedLevel > 0)
                return;

            OnSelectedItemRemoved(item);
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
        private void selection_CollectionItemAdded(INotifyCollectionChanged collection, Object item)
        {
            RaiseSelectedItemAdded(item);
            UpdateSelectionPropertiesAndRaiseSelectionChanged();
        }

        /// <summary>
        /// Handles the selection collection's <see cref="INotifyCollectionChanged.CollectionItemRemoved"/> event.
        /// </summary>
        private void selection_CollectionItemRemoved(INotifyCollectionChanged collection, Object item)
        {
            RaiseSelectedItemRemoved(item);
            UpdateSelectionPropertiesAndRaiseSelectionChanged();
        }
        
        /// <summary>
        /// Handles the selection collection's <see cref="INotifyCollectionChanged.CollectionReset"/> event.
        /// </summary>
        private void selection_CollectionReset(INotifyCollectionChanged collection)
        {
            RaiseSelectedItemsChanged();
            UpdateSelectionPropertiesAndRaiseSelectionChanged();
        }

        // State values.
        private Int32 suspendSelectionChangedLevel;

        // The collection of selected items.
        private readonly SelectionCollection selection;
    }
}
