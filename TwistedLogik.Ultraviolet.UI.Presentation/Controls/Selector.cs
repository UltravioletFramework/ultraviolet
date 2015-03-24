using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a control that allows the user to select items.
    /// </summary>
    [UvmlKnownType]
    public abstract class Selector : ItemsControl
    {
        /// <summary>
        /// Initializes the <see cref="Selector"/> type.
        /// </summary>
        static Selector()
        {
            RoutedEvent.RegisterClassHandler(typeof(Selector), SelectedEvent, new UpfRoutedEventHandler(HandleSelected));
            RoutedEvent.RegisterClassHandler(typeof(Selector), UnselectedEvent, new UpfRoutedEventHandler(HandleUnselected));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Selector"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The unique identifier of this element within a layout.</param>
        public Selector(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Sets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="isSelected">A value indicating whether the specified element is selected.</param>
        public void SetIsSelected(DependencyObject element, Boolean isSelected)
        {
            Contract.Require(element, "element");

            element.SetValue<Boolean>(IsSelectedProperty, isSelected);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is selected.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is selected; otherwise, <c>false</c>.</returns>
        public Boolean GetIsSelected(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsSelectedProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the selected element contains keyboard focus.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is selected; otherwise, <c>false</c>.</returns>
        public Boolean GetIsSelectionActive(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsSelectionActiveProperty);
        }

        /// <summary>
        /// Adds a handler for the <see cref="SelectedEvent"/> attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public void AddSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="UnselectedEvent"/> attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the event handler.</param>
        /// <param name="handler">The event handler to add to the element.</param>
        public void AddUnselectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.AddHandler(element, UnselectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="SelectedEvent"/> attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the event handler.</param>
        /// <param name="handler">The event handler to remove from the element.</param>
        public void RemoveSelectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            UIElementHelper.RemoveHandler(element, SelectedEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="UnselectedEvent"/> attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the event handler.</param>
        /// <param name="handler">The event handler to remove from the element.</param>
        public void RemoveUnselectedHandler(DependencyObject element, UpfRoutedEventHandler handler)
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
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(Int32), typeof(Selector),
            new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(Selector),
            new PropertyMetadata());

        /// <summary>
        /// Identifies the IsSelected attached property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(Boolean), typeof(Selector), 
            new PropertyMetadata());

        /// <summary>
        /// Identifies the IsSelectionActive attached property.
        /// </summary>
        public static readonly DependencyProperty IsSelectionActiveProperty = DependencyProperty.Register("IsSelectionActive", typeof(Boolean), typeof(Selector),
            new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent SelectionChangedEvent = RoutedEvent.Register("SelectionChanged", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the Selected attached event.
        /// </summary>
        public static readonly RoutedEvent SelectedEvent = RoutedEvent.Register("Selected", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Identifies the Unselected attached event.
        /// </summary>
        public static readonly RoutedEvent UnselectedEvent = RoutedEvent.Register("Unselected", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Selector));

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {

        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="SelectedEvent"/> routed event.
        /// </summary>
        private static void HandleSelected(DependencyObject dobj, ref RoutedEventData data)
        {
            // TODO: Update selector
        }

        /// <summary>
        /// Represents the <see cref="Selector"/> class' class handler for the <see cref="UnselectedEvent"/> routed event.
        /// </summary>
        private static void HandleUnselected(DependencyObject dobj, ref RoutedEventData data)
        {
            // TODO: Update selector
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionChanged"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleSelectionChanged(DependencyObject dobj)
        {
            var evtDelegate = RoutedEvent.GetInvocationDelegate<UpfRoutedEventHandler>(SelectionChangedEvent);
            var evtData     = new RoutedEventData(dobj);

            evtDelegate(dobj, ref evtData);
        }
    }
}
