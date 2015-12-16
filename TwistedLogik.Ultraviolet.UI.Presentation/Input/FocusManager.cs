using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains attached properties relating to logical focus and focus scopes.
    /// </summary>
    [UvmlKnownType]
    public static class FocusManager
    {
        /// <summary>
        /// Gets the closest ancestor of the specified element which is a focus scope.
        /// </summary>
        /// <param name="element">The element for which to find a focus scope.</param>
        /// <returns>The closest ancestor of the specified element which is a focus scope.</returns>
        public static DependencyObject GetFocusScope(DependencyObject element)
        {
            Contract.Require(element, "element");

            if (element.GetValue<Boolean>(IsFocusScopeProperty))
                return element;

            var current = element as UIElement;
            if (current != null)
            {
                var logicalParent = LogicalTreeHelper.GetParent(current);
                if (logicalParent != null)
                {
                    return GetFocusScope(logicalParent);
                }

                var visualParent = VisualTreeHelper.GetParent(current);
                if (visualParent != null)
                {
                    return GetFocusScope(visualParent);
                }
            }

            return current;
        }

        /// <summary>
        /// Gets the element with logical focus within the specified focus scope.
        /// </summary>
        /// <param name="element">The focus scope to evaluate.</param>
        /// <returns>The element with logical focus within the specified focus scope.</returns>
        public static IInputElement GetFocusedElement(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<IInputElement>(FocusedElementProperty);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is a focus scope.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><c>true</c> if the specified element is a focus scope; otherwise, <c>false</c>.</returns>
        public static Boolean GetIsFocusScope(DependencyObject element)
        {
            Contract.Require(element, "element");

            return element.GetValue<Boolean>(IsFocusScopeProperty);
        }

        /// <summary>
        /// Sets the element with logical focus within the specified focus scope.
        /// </summary>
        /// <param name="element">The focus scope to update.</param>
        /// <param name="value">The element with logical focus within the specified focus scope.</param>
        public static void SetFocusedElement(DependencyObject element, IInputElement value)
        {
            Contract.Require(element, "element");

            element.SetValue(FocusedElementProperty, value);
        }

        /// <summary>
        /// Sets a value indicating whether the specified element is a focus scope.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <param name="value">A value indicating whether the specified element is a focus scope.</param>
        public static void SetIsFocusScope(DependencyObject element, Boolean value)
        {
            Contract.Require(element, "element");

            element.SetValue(IsFocusScopeProperty, value);
        }

        /// <summary>
        /// Adds a handler for the GotFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            UIElementHelper.AddHandler(element, GotFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the LostFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            UIElementHelper.AddHandler(element, LostFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GotFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            UIElementHelper.RemoveHandler(element, GotFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the LostFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            UIElementHelper.RemoveHandler(element, LostFocusEvent, handler);
        }

        /// <summary>
        /// Identifies the FocusedElement attached property.
        /// </summary>
        public static readonly DependencyProperty FocusedElementProperty = DependencyProperty.RegisterAttached("FocusedElement", typeof(IInputElement), typeof(FocusManager),
            new PropertyMetadata<IInputElement>(null, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the IsFocusScope attached property.
        /// </summary>
        public static readonly DependencyProperty IsFocusScopeProperty = DependencyProperty.RegisterAttached("IsFocusScope", typeof(Boolean), typeof(FocusManager),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the GotFocus routed event.
        /// </summary>
        public static readonly RoutedEvent GotFocusEvent = EventManager.RegisterRoutedEvent("GotFocus", RoutingStrategy.Bubble, 
            typeof(UpfRoutedEventHandler), typeof(FocusManager));

        /// <summary>
        /// Identifies the LostFocus routed event.
        /// </summary>
        public static readonly RoutedEvent LostFocusEvent = EventManager.RegisterRoutedEvent("LostFocus", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(FocusManager));
    }
}
