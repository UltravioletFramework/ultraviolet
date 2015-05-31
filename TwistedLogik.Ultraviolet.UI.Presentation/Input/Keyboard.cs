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
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyboardEventHandler(DependencyObject element, KeyboardDevice device, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a keyboard key is pressed while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyDownEventHandler(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a keyboard key is released while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyEventHandler(DependencyObject element, KeyboardDevice device, Key key, ref RoutedEventData data);

    /// <summary>
    /// Represents the keyboard device.
    /// </summary>
    [UvmlKnownType]
    public static partial class Keyboard
    {
        /// <summary>
        /// Gets the element which has keyboard focus within the specified view.
        /// </summary>
        /// <param name="view">The view to evaluate.</param>
        /// <returns>The element which has keyboard focus within the specified view, or <c>null</c>
        /// if no element currently has focus.</returns>
        public static IInputElement GetFocusedElement(PresentationFoundationView view)
        {
            Contract.Require(view, "view");

            return view.ElementWithFocus;
        }

        /// <summary>
        /// Adds a handler for the GotKeyboardFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the LostKeyboardFocus attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewTextInput attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewKeyDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the PreviewKeyUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyUpEventHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, PreviewKeyUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the TextInput attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, TextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the KeyDown attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, KeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the KeyUp attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyUpEventHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.AddHandler(element, KeyUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the GotKeyboardFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the LostKeyboardFocus attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewTextInput attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewKeyDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the PreviewKeyUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, PreviewKeyUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the TextInput attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, TextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the KeyDown attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, KeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the KeyUp attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, "element");
            Contract.Require(handler, "handler");

            IInputElementHelper.RemoveHandler(element, KeyUpEvent, handler);
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
        /// <remarks>The styling name of this routed event is got-keyboard-focus.</remarks>
        public static readonly RoutedEvent GotKeyboardFocusEvent = EventManager.RegisterRoutedEvent("GotKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the LostKeyboardFocus attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is lost-keyboard-focus.</remarks>
        public static readonly RoutedEvent LostKeyboardFocusEvent = EventManager.RegisterRoutedEvent("LostKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UpfRoutedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the PreviewTextInput attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-text-input.</remarks>
        public static readonly RoutedEvent PreviewTextInputEvent = EventManager.RegisterRoutedEvent("PreviewTextInput", RoutingStrategy.Tunnel,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the PreviewKeyDown attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-key-down.</remarks>
        public static readonly RoutedEvent PreviewKeyDownEvent = EventManager.RegisterRoutedEvent("PreviewKeyDown", RoutingStrategy.Tunnel,
            typeof(UpfKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the PreviewKeyUp attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is preview-key-up.</remarks>
        public static readonly RoutedEvent PreviewKeyUpEvent = EventManager.RegisterRoutedEvent("PreviewKeyUp", RoutingStrategy.Tunnel,
            typeof(UpfKeyEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the TextInput attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is text-input.</remarks>
        public static readonly RoutedEvent TextInputEvent = EventManager.RegisterRoutedEvent("TextInput", RoutingStrategy.Bubble,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the KeyDown attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is key-down.</remarks>
        public static readonly RoutedEvent KeyDownEvent = EventManager.RegisterRoutedEvent("KeyDown", RoutingStrategy.Bubble,
            typeof(UpfKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the KeyUp attached event.
        /// </summary>
        /// <remarks>The styling name of this routed event is key-up.</remarks>
        public static readonly RoutedEvent KeyUpEvent = EventManager.RegisterRoutedEvent("KeyUp", RoutingStrategy.Bubble,
            typeof(UpfKeyEventHandler), typeof(Keyboard));

        /// <summary>
        /// Raises the GotKeyboardFocus attached event for the specified element.
        /// </summary>
        internal static void RaiseGotKeyboardFocus(DependencyObject element, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(GotKeyboardFocusEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }

        /// <summary>
        /// Raises the LostKeyboardFocus attached event for the specified element.
        /// </summary>
        internal static void RaiseLostKeyboardFocus(DependencyObject element, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(LostKeyboardFocusEvent);
            if (temp != null)
            {
                temp(element, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewTextInput attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTextInput(DependencyObject element, KeyboardDevice device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(PreviewTextInputEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }

        /// <summary>
        /// Raises the KeyDown attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyDown(DependencyObject element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyDownEventHandler>(PreviewKeyDownEvent);
            if (temp != null)
            {
                temp(element, device, key, Keyboard.Modifiers, ref data);
            }
        }

        /// <summary>
        /// Raises the PreviewKeyUp attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyUp(DependencyObject element, KeyboardDevice device, Key key, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyEventHandler>(PreviewKeyUpEvent);
            if (temp != null)
            {
                temp(element, device, key, ref data);
            }
        }

        /// <summary>
        /// Raises the TextInput attached event for the specified element.
        /// </summary>
        internal static void RaiseTextInput(DependencyObject element, KeyboardDevice device, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(TextInputEvent);
            if (temp != null)
            {
                temp(element, device, ref data);
            }
        }

        /// <summary>
        /// Raises the KeyDown attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyDown(DependencyObject element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyDownEventHandler>(KeyDownEvent);
            if (temp != null)
            {
                var modifiers = CreateModifierKeys(alt, ctrl, shift, repeat);
                temp(element, device, key, modifiers, ref data);
            }
        }

        /// <summary>
        /// Raises the KeyUp attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyUp(DependencyObject element, KeyboardDevice device, Key key, ref RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyEventHandler>(KeyUpEvent);
            if (temp != null)
            {
                temp(element, device, key, ref data);
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
