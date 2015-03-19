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
    /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
    /// <param name="handled">A value indicating whether the event has been handled.</param>
    public delegate void UIElementKeyDownEventHandler(UIElement element, KeyboardDevice device, Key key, ModifierKeys modifiers, ref Boolean handled);

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
    public static partial class Keyboard
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
        /// Adds a handler for the PreviewTextInput attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTextInputHandler(UIElement element, UIElementKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewKeyDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyDownHandler(UIElement element, UIElementKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.AddHandler(PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewKeyUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyUpEventHandler(UIElement element, UIElementKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewKeyUpEvent, handler);
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
        /// Removes a handler for the PreviewTextInput attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTextInputHandler(UIElement element, UIElementKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewKeyDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyDownHandler(UIElement element, UIElementKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewKeyUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyUpHandler(UIElement element, UIElementKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            element.RemoveHandler(PreviewKeyUpEvent, handler);
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
        /// Gets the primary keyboard input device.
        /// </summary>
        public static KeyboardDevice PrimaryDevice
        {
            get { return keyboardState.Value.PrimaryDevice; }
        }

        /// <summary>
        /// Gets the set of <see cref="ModifierKeys"/> that are currently pressed.
        /// </summary>
        public static ModifierKeys Modifiers
        {
            get
            {
                var device = keyboardState.Value.PrimaryDevice;
                var alt    = device.IsAltDown;
                var ctrl   = device.IsControlDown;
                var shift  = device.IsShiftDown;
                return CreateModifierKeys(alt, ctrl, shift, false);
            }
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
        /// Identifies the PreviewTextInput attached event.
        /// </summary>
        public static readonly RoutedEvent PreviewTextInputEvent = RoutedEvent.Register("PreviewTextInput", RoutingStrategy.Tunnel,
            typeof(UIElementKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the PreviewKeyDown attached event.
        /// </summary>
        public static readonly RoutedEvent PreviewKeyDownEvent = RoutedEvent.Register("PreviewKeyDown", RoutingStrategy.Tunnel,
            typeof(UIElementKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the PreviewKeyUp attached event.
        /// </summary>
        public static readonly RoutedEvent PreviewKeyUpEvent = RoutedEvent.Register("PreviewKeyUp", RoutingStrategy.Tunnel,
            typeof(UIElementKeyEventHandler), typeof(Keyboard));

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
        internal static void RaiseGotKeyboardFocus(UIElement element, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(GotKeyboardFocusEvent);
            if (temp != null)
            {
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the LostKeyboardFocus attached event for the specified element.
        /// </summary>
        internal static void RaiseLostKeyboardFocus(UIElement element, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementRoutedEventHandler>(LostKeyboardFocusEvent);
            if (temp != null)
            {
                temp(element, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewTextInput attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTextInput(UIElement element, KeyboardDevice device, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyboardEventHandler>(PreviewTextInputEvent);
            if (temp != null)
            {
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the KeyDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyDown(UIElement element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyDownEventHandler>(PreviewKeyDownEvent);
            if (temp != null)
            {
                var modifiers = CreateModifierKeys(ctrl, alt, shift, repeat);
                temp(element, device, key, Keyboard.Modifiers, ref handled);
            }
        }

        /// <summary>
        /// Raises the PreviewKeyUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyUp(UIElement element, KeyboardDevice device, Key key, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyEventHandler>(PreviewKeyUpEvent);
            if (temp != null)
            {
                temp(element, device, key, ref handled);
            }
        }

        /// <summary>
        /// Raises the TextInput attached event for the specified element.
        /// </summary>
        internal static void RaiseTextInput(UIElement element, KeyboardDevice device, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyboardEventHandler>(TextInputEvent);
            if (temp != null)
            {
                temp(element, device, ref handled);
            }
        }

        /// <summary>
        /// Raises the KeyDown attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyDown(UIElement element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyDownEventHandler>(KeyDownEvent);
            if (temp != null)
            {
                var modifiers = CreateModifierKeys(ctrl, alt, shift, repeat);
                temp(element, device, key, modifiers, ref handled);
            }
        }

        /// <summary>
        /// Raises the KeyUp attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyUp(UIElement element, KeyboardDevice device, Key key, ref Boolean handled)
        {
            var temp = RoutedEvent.GetInvocationDelegate<UIElementKeyEventHandler>(KeyUpEvent);
            if (temp != null)
            {
                temp(element, device, key, ref handled);
            }
        }

        /// <summary>
        /// Creates a <see cref="ModifierKeys"/> value that corresponds to the specified modifier key states.
        /// </summary>
        private static ModifierKeys CreateModifierKeys(Boolean alt, Boolean ctrl, Boolean shift, Boolean repeat)
        {
            var modifiers = ModifierKeys.None;
            if (alt)
            {
                modifiers |= ModifierKeys.Alt;
            }
            if (ctrl)
            {
                modifiers |= ModifierKeys.Control;
            }
            if (shift)
            {
                modifiers |= ModifierKeys.Shift;
            }
            if (repeat)
            {
                modifiers |= ModifierKeys.Repeat;
            }
            return modifiers;
        }

        // Represents the device state of the current Ultraviolet context.
        private static readonly UltravioletSingleton<KeyboardState> keyboardState = 
            new UltravioletSingleton<KeyboardState>((uv) => new KeyboardState(uv));
    }
}
