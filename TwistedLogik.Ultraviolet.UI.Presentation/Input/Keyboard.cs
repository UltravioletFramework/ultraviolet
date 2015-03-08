using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives an event from a keyboard device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The keyboard device.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementKeyboardEventHandler(UIElement element, KeyboardDevice device, ref Boolean handled);

    /// <summary>
    /// Represents the method that is called when a keyboard key is pressed while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="modifiers">A <see cref="KeyModifiers"/> value indicating which of the key modifiers are currently active.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementKeyDownEventHandler(UIElement element, KeyboardDevice device, Key key, KeyModifiers modifiers, ref Boolean handled);

    /// <summary>
    /// Represents the method that is called when a keyboard key is released while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementKeyEventHandler(UIElement element, KeyboardDevice device, Key key, ref Boolean handled);

    /// <summary>
    /// Represents the keyboard device.
    /// </summary>
    [UvmlKnownType]
    public static class Keyboard
    {
        /// <summary>
        /// Adds a handler for the GotKeyboardFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotKeyboardFocusHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the LostKeyboardFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostKeyboardFocusHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the TextInput attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTextInputHandler(UIElement element, UIElementKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(TextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the KeyDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyDownHandler(UIElement element, UIElementKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(KeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the KeyUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyUpEventHandler(UIElement element, UIElementKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(KeyUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GotKeyboardFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotKeyboardFocusHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the LostKeyboardFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostKeyboardFocusHandler(UIElement element, UIElementRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the TextInput attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTextInputHandler(UIElement element, UIElementKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(TextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the KeyDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyDownHandler(UIElement element, UIElementKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(KeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the KeyUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyUpHandler(UIElement element, UIElementKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(KeyUpEvent, handler);
        }

        /// <summary>
        /// Identifies the GotKeyboardFocus attached event.
        /// </summary>
        public static readonly RoutedEvent GotKeyboardFocusEvent = RoutedEvent.Register("GotKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UIElementRoutedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the LostKeyboardFocus attached event.
        /// </summary>
        public static readonly RoutedEvent LostKeyboardFocusEvent = RoutedEvent.Register("LostKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UIElementRoutedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the TextInput attached event.
        /// </summary>
        public static readonly RoutedEvent TextInputEvent = RoutedEvent.Register("TextInput", RoutingStrategy.Bubble,
            typeof(UIElementKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the KeyDown attached event.
        /// </summary>
        public static readonly RoutedEvent KeyDownEvent = RoutedEvent.Register("KeyDown", RoutingStrategy.Bubble,
            typeof(UIElementKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the KeyUp attached event.
        /// </summary>
        public static readonly RoutedEvent KeyUpEvent = RoutedEvent.Register("KeyUp", RoutingStrategy.Bubble,
            typeof(UIElementKeyEventHandler), typeof(Keyboard));

        /// <summary>
        /// Raises the GotKeyboardFocus attached event for the specified element.
        /// </summary>
        internal static void RaiseGotKeyboardFocus(UIElement element)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(GotKeyboardFocusEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the LostKeyboardFocus attached event for the specified element.
        /// </summary>
        internal static void RaiseLostKeyboardFocus(UIElement element)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(LostKeyboardFocusEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the TextInput attached event for the specified element.
        /// </summary>
        internal static void RaiseTextInput(UIElement element, KeyboardDevice device)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyboardEventHandler>(TextInputEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the KeyDown attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyDown(UIElement element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyDownEventHandler>(KeyDownEvent);
            if (temp != null)
            {
                var modifiers = new KeyModifiers(ctrl, alt, shift, repeat);
                var handled = false;
                temp(element, device, key, modifiers, ref handled);
            }
        }

        /// <summary>
        /// Raises the KeyUp attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyUp(UIElement element, KeyboardDevice device, Key key)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyEventHandler>(KeyUpEvent);
            if (temp != null)
            {
                var handled = false;
                temp(element, device, key, ref handled);
            }
        }
    }
}
