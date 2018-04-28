using System;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the method that is called when a UI element receives an event from a keyboard device.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The keyboard device.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyboardEventHandler(DependencyObject element, KeyboardDevice device, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a keyboard key is pressed while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyDownEventHandler(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when a keyboard key is released while an element has focus.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyEventHandler(DependencyObject element, KeyboardDevice device, Key key, RoutedEventData data);

    /// <summary>
    /// Represents the method that is called when keyboard focus is changed.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
    /// <param name="oldFocus">The element that previously had focus.</param>
    /// <param name="newFocus">The element that currently has focus.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfKeyboardFocusChangedEventHandler(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data);

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
        /// <returns>The element which has keyboard focus within the specified view, or <see langword="null"/>
        /// if no element currently has focus.</returns>
        public static IInputElement GetFocusedElement(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            return view.ElementWithFocus;
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified element can receive keyboard focus.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is focusable; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsFocusable(IInputElement element)
        {
            if (element == null)
                return false;
            
            if (!element.IsEnabled)
                return false;

            var isFocusScope = false;

            var uiElement = element as UIElement;
            if (uiElement != null)
            {
                if (!uiElement.IsVisible)
                    return false;

                isFocusScope = FocusManager.GetIsFocusScope(uiElement);
            }

            return isFocusScope || element.Focusable;
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewGotKeyboardFocusHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewGotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewLostKeyboardFocusHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewLostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddGotKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddLostKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewTextEditingHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewTextEditingEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/>
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddPreviewKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, PreviewKeyUpEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TextInputEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddTextEditingHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, TextEditingEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, KeyDownEvent, handler);
        }

        /// <summary>
        /// Adds a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/> 
        /// attached event to the specified element.
        /// </summary>
        /// <param name="element">The element to which to add the handler.</param>
        /// <param name="handler">The handler to add to the specified element.</param>
        public static void AddKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.AddHandler(element, KeyUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewGotKeyboardFocusHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewGotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewLostKeyboardFocusHandler(DependencyObject element, UpfKeyboardFocusChangedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewLostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveGotKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, GotKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveLostKeyboardFocusHandler(DependencyObject element, UpfRoutedEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, LostKeyboardFocusEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewTextEditingHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewTextEditingEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewKeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemovePreviewKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, PreviewKeyUpEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTextInputHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TextInputEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveTextEditingHandler(DependencyObject element, UpfKeyboardEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, TextEditingEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/>
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyDownHandler(DependencyObject element, UpfKeyDownEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

            IInputElementHelper.RemoveHandler(element, KeyDownEvent, handler);
        }

        /// <summary>
        /// Removes a handler for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/> 
        /// attached event from the specified element.
        /// </summary>
        /// <param name="element">The element from which to remove the handler.</param>
        /// <param name="handler">The handler to remove from the specified element.</param>
        public static void RemoveKeyUpHandler(DependencyObject element, UpfKeyEventHandler handler)
        {
            Contract.Require(element, nameof(element));
            Contract.Require(handler, nameof(handler));

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
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element gets keyboard focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewGotKeyboardFocusEvent"/></revtField>
        ///     <revtStylingName>preview-got-keyboard-focus</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardFocusChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewGotKeyboardFocusEvent = EventManager.RegisterRoutedEvent("PreviewGotKeyboardFocus", RoutingStrategy.Tunnel,
            typeof(UpfKeyboardFocusChangedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element loses keyboard focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewLostKeyboardFocusEvent"/></revtField>
        ///     <revtStylingName>preview-lost-keyboard-focus</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardFocusChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewLostKeyboardFocusEvent = EventManager.RegisterRoutedEvent("PreviewLostKeyboardFocus", RoutingStrategy.Tunnel,
            typeof(UpfKeyboardFocusChangedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element gets keyboard focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="GotKeyboardFocusEvent"/></revtField>
        ///     <revtStylingName>got-keyboard-focus</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardFocusChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent GotKeyboardFocusEvent = EventManager.RegisterRoutedEvent("GotKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UpfKeyboardFocusChangedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/>
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the element loses keyboard focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="LostKeyboardFocusEvent"/></revtField>
        ///     <revtStylingName>lost-keyboard-focus</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardFocusChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent LostKeyboardFocusEvent = EventManager.RegisterRoutedEvent("LostKeyboardFocus", RoutingStrategy.Bubble,
            typeof(UpfKeyboardFocusChangedEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the user enters text while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTextInputEvent"/></revtField>
        ///     <revtStylingName>preview-text-input</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTextInputEvent = EventManager.RegisterRoutedEvent("PreviewTextInput", RoutingStrategy.Tunnel,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the user edits text (for example, when composing text with the Android 
        /// software keyboard) while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewTextEditingEvent"/></revtField>
        ///     <revtStylingName>preview-text-editing</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewTextEditingEvent = EventManager.RegisterRoutedEvent("PreviewTextEditing", RoutingStrategy.Tunnel,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a key enters the "down" state while the element has focus.
        /// </summary>
        /// </AttachedEventComments>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewKeyDownEvent"/></revtField>
        ///     <revtStylingName>preview-key-down</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static readonly RoutedEvent PreviewKeyDownEvent = EventManager.RegisterRoutedEvent("PreviewKeyDown", RoutingStrategy.Tunnel,
            typeof(UpfKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a key enters the "up" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="PreviewKeyUpEvent"/></revtField>
        ///     <revtStylingName>preview-key-up</revtStylingName>
        ///     <revtStrategy>Tunneling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding bubbling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent PreviewKeyUpEvent = EventManager.RegisterRoutedEvent("PreviewKeyUp", RoutingStrategy.Tunnel,
            typeof(UpfKeyEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the user enters text while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TextInputEvent"/></revtField>
        ///     <revtStylingName>text-input</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TextInputEvent = EventManager.RegisterRoutedEvent("TextInput", RoutingStrategy.Bubble,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when the user edits text (for example, when composing text with the Android 
        /// software keyboard) while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="TextEditingEvent"/></revtField>
        ///     <revtStylingName>text-editing</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyboardEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent TextEditingEvent = EventManager.RegisterRoutedEvent("TextEditing", RoutingStrategy.Bubble,
            typeof(UpfKeyboardEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a key enters the "down" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="KeyDownEvent"/></revtField>
        ///     <revtStylingName>key-down</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyDownEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent KeyDownEvent = EventManager.RegisterRoutedEvent("KeyDown", RoutingStrategy.Bubble,
            typeof(UpfKeyDownEventHandler), typeof(Keyboard));

        /// <summary>
        /// Identifies the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/> 
        /// attached routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/>
        /// attached routed event.</value>
        /// <AttachedEventComments>
        /// <summary>
        /// Occurs when a key enters the "up" state while the element has focus.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="KeyUpEvent"/></revtField>
        ///     <revtStylingName>key-up</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfKeyEventHandler"/></revtDelegate>
        /// </revt>
        /// <list type="bullet">
        ///     <item>
        ///         <description>The corresponding tunneling event is 
        ///         <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// </AttachedEventComments>
        public static readonly RoutedEvent KeyUpEvent = EventManager.RegisterRoutedEvent("KeyUp", RoutingStrategy.Bubble,
            typeof(UpfKeyEventHandler), typeof(Keyboard));

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewGotKeyboardFocus"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewGotKeyboardFocus(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardFocusChangedEventHandler>(PreviewGotKeyboardFocusEvent);
            evt?.Invoke(element, device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewLostKeyboardFocus"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewLostKeyboardFocus(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardFocusChangedEventHandler>(PreviewLostKeyboardFocusEvent);
            evt?.Invoke(element, device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.GotKeyboardFocus"/> 
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseGotKeyboardFocus(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardFocusChangedEventHandler>(GotKeyboardFocusEvent);
            evt?.Invoke(element, device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.LostKeyboardFocus"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseLostKeyboardFocus(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardFocusChangedEventHandler>(LostKeyboardFocusEvent);
            evt?.Invoke(element, device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextInput"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTextInput(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(PreviewTextInputEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewTextEditing"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewTextEditing(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(PreviewTextEditingEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyDown"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyDown(DependencyObject element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyDownEventHandler>(PreviewKeyDownEvent);
            evt?.Invoke(element, device, key, Modifiers, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.PreviewKeyUp"/> attached event for the specified element.
        /// </summary>
        internal static void RaisePreviewKeyUp(DependencyObject element, KeyboardDevice device, Key key, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyEventHandler>(PreviewKeyUpEvent);
            evt?.Invoke(element, device, key, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextInput"/> 
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTextInput(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(TextInputEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.TextEditing"/>
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseTextEditing(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyboardEventHandler>(TextEditingEvent);
            evt?.Invoke(element, device, data);
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyDown"/> 
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyDown(DependencyObject element, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat, RoutedEventData data)
        {
            var temp = EventManager.GetInvocationDelegate<UpfKeyDownEventHandler>(KeyDownEvent);
            if (temp != null)
            {
                var modifiers = CreateModifierKeys(alt, ctrl, shift, repeat);
                temp(element, device, key, modifiers, data);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Ultraviolet.Presentation.Input.Keyboard.KeyUp"/> 
        /// attached event for the specified element.
        /// </summary>
        internal static void RaiseKeyUp(DependencyObject element, KeyboardDevice device, Key key, RoutedEventData data)
        {
            var evt = EventManager.GetInvocationDelegate<UpfKeyEventHandler>(KeyUpEvent);
            evt?.Invoke(element, device, key, data);
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
            new UltravioletSingleton<KeyboardState>(uv => new KeyboardState(uv));
    }
}
