using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class UIElement : IInputElement
    {
        /// <inheritdoc/>
        public Boolean IsKeyboardFocused
        {
            get { return GetValue<Boolean>(IsKeyboardFocusedProperty); }
            internal set { SetValue<Boolean>(IsKeyboardFocusedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsKeyboardFocusWithin
        {
            get { return GetValue<Boolean>(IsKeyboardFocusWithinProperty); }
            internal set { SetValue<Boolean>(IsKeyboardFocusWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseCaptured
        {
            get { return GetValue<Boolean>(IsMouseCapturedProperty); }
            internal set { SetValue<Boolean>(IsMouseCapturedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseCaptureWithin
        {
            get { return GetValue<Boolean>(IsMouseCaptureWithinProperty); }
            internal set { SetValue<Boolean>(IsMouseCaptureWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseOver
        {
            get { return GetValue<Boolean>(IsMouseOverProperty); }
            internal set { SetValue<Boolean>(IsMouseOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseDirectlyOver
        {
            get { return GetValue<Boolean>(IsMouseDirectlyOverProperty); }
            internal set { SetValue<Boolean>(IsMouseDirectlyOverPropertyKey, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="IsKeyboardFocused"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsKeyboardFocusedPropertyKey = DependencyProperty.RegisterReadOnly("IsKeyboardFocused", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsKeyboardFocusedChanged));

        /// <summary>
        /// Identifies the <see cref="IsKeyboardFocused"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'keyboard-focused'.</remarks>
        public static readonly DependencyProperty IsKeyboardFocusedProperty = IsKeyboardFocusedPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsKeyboardFocusWithin"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsKeyboardFocusWithinPropertyKey = DependencyProperty.RegisterReadOnly("IsKeyboardFocusWithin", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsKeyboardFocusWithinChanged));

        /// <summary>
        /// Identifies the <see cref="IsKeyboardFocusWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'keyboard-focus-within'.</remarks>
        public static readonly DependencyProperty IsKeyboardFocusWithinProperty = IsKeyboardFocusWithinPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsMouseCaptured"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsMouseCapturedPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseCaptured", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsMouseCapturedChanged));

        /// <summary>
        /// Identifies the <see cref="IsMouseCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'mouse-captured'.</remarks>
        public static readonly DependencyProperty IsMouseCapturedProperty = IsMouseCapturedPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsMouseCaptureWithin"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsMouseCaptureWithinPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseCaptureWithin", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsMouseCaptureWithinChanged));

        /// <summary>
        /// Identifies the <see cref="IsMouseCaptureWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'mouse-capture-within'.</remarks>
        public static readonly DependencyProperty IsMouseCaptureWithinProperty = IsMouseCaptureWithinPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsMouseOver"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsMouseOverPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsMouseOverChanged));

        /// <summary>
        /// Identifies the <see cref="IsMouseOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'mouse-over'.</remarks>
        public static readonly DependencyProperty IsMouseOverProperty = IsMouseOverPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="IsMouseDirectlyOver"/> read-only dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsMouseDirectlyOverPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseDirectlyOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleIsMouseDirectlyOverChanged));

        /// <summary>
        /// Identifies the <see cref="IsMouseDirectlyOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'mouse-directly-over'.</remarks>
        public static readonly DependencyProperty IsMouseDirectlyOverProperty = IsMouseDirectlyOverPropertyKey.DependencyProperty;

        /// <summary>
        /// Occurs when the value of the <see cref="IsKeyboardFocused"/> property changes.
        /// </summary>
        public event UpfEventHandler IsKeyboardFocusedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsKeyboardFocusWithin"/> property changes.
        /// </summary>
        public event UpfEventHandler IsKeyboardFocusWithinChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseCaptured"/> property changes.
        /// </summary>
        public event UpfEventHandler IsMouseCapturedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseCaptureWithin"/> property changes.
        /// </summary>
        public event UpfEventHandler IsMouseCaptureWithinChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseOver"/> property changes.
        /// </summary>
        public event UpfEventHandler IsMouseOverChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseDirectlyOver"/> property changes.
        /// </summary>
        public event UpfEventHandler IsMouseDirectlyOverChanged;

        /// <inheritdoc/>
        public event UpfRoutedEventHandler GotKeyboardFocus
        {
            add { AddHandler(Keyboard.GotKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.GotKeyboardFocusEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfRoutedEventHandler LostKeyboardFocus
        {
            add { AddHandler(Keyboard.LostKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.LostKeyboardFocusEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyboardEventHandler PreviewTextInput
        {
            add { AddHandler(Keyboard.PreviewTextInputEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewTextInputEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyDownEventHandler PreviewKeyDown
        {
            add { AddHandler(Keyboard.PreviewKeyDownEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewKeyDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyEventHandler PreviewKeyUp
        {
            add { AddHandler(Keyboard.PreviewKeyUpEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewKeyUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyboardEventHandler TextInput
        {
            add { AddHandler(Keyboard.TextInputEvent, value); }
            remove { RemoveHandler(Keyboard.TextInputEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyDownEventHandler KeyDown
        {
            add { AddHandler(Keyboard.KeyDownEvent, value); }
            remove { RemoveHandler(Keyboard.KeyDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfKeyEventHandler KeyUp
        {
            add { AddHandler(Keyboard.KeyUpEvent, value); }
            remove { RemoveHandler(Keyboard.KeyUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseEventHandler GotMouseCapture
        {
            add { AddHandler(Mouse.GotMouseCaptureEvent, value); }
            remove { RemoveHandler(Mouse.GotMouseCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfRoutedEventHandler LostMouseCapture
        {
            add { AddHandler(Mouse.LostMouseCaptureEvent, value); }
            remove { RemoveHandler(Mouse.LostMouseCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseMoveEventHandler PreviewMouseMove
        {
            add { AddHandler(Mouse.PreviewMouseMoveEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler PreviewMouseDown
        {
            add { AddHandler(Mouse.PreviewMouseDownEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler PreviewMouseUp
        {
            add { AddHandler(Mouse.PreviewMouseUpEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler PreviewMouseClick
        {
            add { AddHandler(Mouse.PreviewMouseClickEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler PreviewMouseDoubleClick
        {
            add { AddHandler(Mouse.PreviewMouseDoubleClickEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseDoubleClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseWheelEventHandler PreviewMouseWheel
        {
            add { AddHandler(Mouse.PreviewMouseWheelEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseWheelEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseMoveEventHandler MouseMove
        {
            add { AddHandler(Mouse.MouseMoveEvent, value); }
            remove { RemoveHandler(Mouse.MouseMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler MouseDown
        {
            add { AddHandler(Mouse.MouseDownEvent, value); }
            remove { RemoveHandler(Mouse.MouseDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler MouseUp
        {
            add { AddHandler(Mouse.MouseUpEvent, value); }
            remove { RemoveHandler(Mouse.MouseUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler MouseClick
        {
            add { AddHandler(Mouse.MouseClickEvent, value); }
            remove { RemoveHandler(Mouse.MouseClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseButtonEventHandler MouseDoubleClick
        {
            add { AddHandler(Mouse.MouseDoubleClickEvent, value); }
            remove { RemoveHandler(Mouse.MouseDoubleClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseWheelEventHandler MouseWheel
        {
            add { AddHandler(Mouse.MouseWheelEvent, value); }
            remove { RemoveHandler(Mouse.MouseWheelEvent, value); }
        }

        /// <summary>
        /// Raises the <see cref="IsKeyboardFocusedChanged"/> event.
        /// </summary>
        protected virtual void OnIsKeyboardFocusedChanged()
        {
            var temp = IsKeyboardFocusedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsKeyboardFocusWithinChanged"/> event.
        /// </summary>
        protected virtual void OnIsKeyboardFocusWithinChanged()
        {
            var temp = IsKeyboardFocusWithinChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsMouseCapturedChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseCapturedChanged()
        {
            var temp = IsMouseCapturedChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsMouseCaptureWithinChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseCaptureWithinChanged()
        {
            var temp = IsMouseCaptureWithinChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsMouseOverChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseOverChanged()
        {
            var temp = IsMouseOverChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="IsMouseDirectlyOverChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseDirectlyOverChanged()
        {
            var temp = IsMouseDirectlyOverChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.GotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotKeyboardFocus(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.LostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostKeyboardFocus(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyDownEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyUpEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnKeyUp(KeyboardDevice device, Key key, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.TextInputEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTextInput(KeyboardDevice device, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.GotMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotMouseCapture(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.LostMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostMouseCapture(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseMoveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseEnterEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseEnter(MouseDevice device, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseLeaveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseLeave(MouseDevice device, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseUp(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseDown(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDoubleClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseDoubleClick(MouseDevice device, MouseButton button, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseWheelEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseWheel(MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Registers class handlers for this type's input events.
        /// </summary>
        private static void RegisterInputClassHandlers()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.GotKeyboardFocusEvent, new UpfRoutedEventHandler(OnGotKeyboardFocusProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.LostKeyboardFocusEvent, new UpfRoutedEventHandler(OnLostKeyboardFocusProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyDownEvent, new UpfKeyDownEventHandler(OnKeyDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyUpEvent, new UpfKeyEventHandler(OnKeyUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.TextInputEvent, new UpfKeyboardEventHandler(OnTextInputProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.GotMouseCaptureEvent, new UpfRoutedEventHandler(OnGotMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.LostMouseCaptureEvent, new UpfRoutedEventHandler(OnLostMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseMoveEvent, new UpfMouseMoveEventHandler(OnMouseMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseEnterEvent, new UpfMouseEventHandler(OnMouseEnterProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseLeaveEvent, new UpfMouseEventHandler(OnMouseLeaveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDownEvent, new UpfMouseButtonEventHandler(OnMouseDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseUpEvent, new UpfMouseButtonEventHandler(OnMouseUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseClickEvent, new UpfMouseButtonEventHandler(OnMouseClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDoubleClickEvent, new UpfMouseButtonEventHandler(OnMouseDoubleClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseWheelEvent, new UpfMouseWheelEventHandler(OnMouseWheelProxy));
        }

        /// <summary>
        /// Invokes the <see cref="OnGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnGotKeyboardFocusProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnGotKeyboardFocus(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostKeyboardFocus"/> method.
        /// </summary>
        private static void OnLostKeyboardFocusProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnLostKeyboardFocus(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyDown"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> that was pressed.</param>
        /// <param name="modifiers">The current set of <see cref="ModifierKeys"/> values.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnKeyDownProxy(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            ((UIElement)element).OnKeyDown(device, key, modifiers, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyUp"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnKeyUpProxy(DependencyObject element, KeyboardDevice device, Key key, ref RoutedEventData data)
        {
            ((UIElement)element).OnKeyUp(device, key, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextInput"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The keyboard device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnTextInputProxy(DependencyObject element, KeyboardDevice device, ref RoutedEventData data)
        {
            ((UIElement)element).OnTextInput(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotMouseCapture"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnGotMouseCaptureProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnGotMouseCapture(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostMouseCapture"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnLostMouseCaptureProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnLostMouseCapture(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseMove"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseMoveProxy(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseEnter"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseEnterProxy(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseEnter(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseLeave"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseLeaveProxy(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseLeave(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDown"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseDownProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseDown(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseUp"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseUpProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseUp(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseClick"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseClickProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseClick(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseDoubleClickProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseDoubleClick(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseWheel"/> method.
        /// </summary>
        /// <param name="element">The element that raised the event.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        private static void OnMouseWheelProxy(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseWheel(device, x, y, ref data);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsKeyboardFocused"/> dependency property changes.
        /// </summary>
        private static void HandleIsKeyboardFocusedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsKeyboardFocusedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsKeyboardFocusWithin"/> dependency property changes.
        /// </summary>
        private static void HandleIsKeyboardFocusWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsKeyboardFocusWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseCaptured"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseCapturedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsMouseCapturedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseCaptureWithin"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseCaptureWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsMouseCaptureWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseOver"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsMouseOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseDirectlyOver"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseDirectlyOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnIsMouseDirectlyOverChanged();
        }
    }
}
