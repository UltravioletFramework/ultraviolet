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
            get { return (View == null) ? false : View.ElementWithFocus == this; }
        }

        /// <inheritdoc/>
        public Boolean IsKeyboardFocusWithin
        {
            get;
            internal set;
        }

        /// <inheritdoc/>
        public Boolean IsMouseCaptured
        {
            get
            {
                return (View == null) ? false : View.ElementWithMouseCapture == this;
            }
        }

        /// <inheritdoc/>
        public Boolean IsMouseOver
        {
            get
            {
                var position = Mouse.GetPosition(this);
                return Bounds.Contains(position);
            }
        }

        /// <inheritdoc/>
        public Boolean IsMouseDirectlyOver
        {
            get { return (View == null) ? false : View.ElementUnderMouse == this; }
        }

        /// <inheritdoc/>
        public event UIElementRoutedEventHandler GotKeyboardFocus
        {
            add { AddHandler(Keyboard.GotKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.GotKeyboardFocusEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementRoutedEventHandler LostKeyboardFocus
        {
            add { AddHandler(Keyboard.LostKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.LostKeyboardFocusEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyboardEventHandler PreviewTextInput
        {
            add { AddHandler(Keyboard.PreviewTextInputEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewTextInputEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyDownEventHandler PreviewKeyDown
        {
            add { AddHandler(Keyboard.PreviewKeyDownEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewKeyDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyEventHandler PreviewKeyUp
        {
            add { AddHandler(Keyboard.PreviewKeyUpEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewKeyUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyboardEventHandler TextInput
        {
            add { AddHandler(Keyboard.TextInputEvent, value); }
            remove { RemoveHandler(Keyboard.TextInputEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyDownEventHandler KeyDown
        {
            add { AddHandler(Keyboard.KeyDownEvent, value); }
            remove { RemoveHandler(Keyboard.KeyDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementKeyEventHandler KeyUp
        {
            add { AddHandler(Keyboard.KeyUpEvent, value); }
            remove { RemoveHandler(Keyboard.KeyUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseEventHandler GotMouseCapture
        {
            add { AddHandler(Mouse.GotMouseCaptureEvent, value); }
            remove { RemoveHandler(Mouse.GotMouseCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementRoutedEventHandler LostMouseCapture
        {
            add { AddHandler(Mouse.LostMouseCaptureEvent, value); }
            remove { RemoveHandler(Mouse.LostMouseCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseMoveEventHandler PreviewMouseMoveEvent
        {
            add { AddHandler(Mouse.PreviewMouseMoveEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler PreviewMouseDownEvent
        {
            add { AddHandler(Mouse.PreviewMouseDownEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler PreviewMouseUpEvent
        {
            add { AddHandler(Mouse.PreviewMouseUpEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler PreviewMouseClickEvent
        {
            add { AddHandler(Mouse.PreviewMouseClickEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler PreviewMouseDoubleClickEvent
        {
            add { AddHandler(Mouse.PreviewMouseDoubleClickEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseDoubleClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseWheelEventHandler PreviewMouseWheelEvent
        {
            add { AddHandler(Mouse.PreviewMouseWheelEvent, value); }
            remove { RemoveHandler(Mouse.PreviewMouseWheelEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseMoveEventHandler MouseMoveEvent
        {
            add { AddHandler(Mouse.MouseMoveEvent, value); }
            remove { RemoveHandler(Mouse.MouseMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler MouseDownEvent
        {
            add { AddHandler(Mouse.MouseDownEvent, value); }
            remove { RemoveHandler(Mouse.MouseDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler MouseUpEvent
        {
            add { AddHandler(Mouse.MouseUpEvent, value); }
            remove { RemoveHandler(Mouse.MouseUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler MouseClickEvent
        {
            add { AddHandler(Mouse.MouseClickEvent, value); }
            remove { RemoveHandler(Mouse.MouseClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseButtonEventHandler MouseDoubleClickEvent
        {
            add { AddHandler(Mouse.MouseDoubleClickEvent, value); }
            remove { RemoveHandler(Mouse.MouseDoubleClickEvent, value); }
        }

        /// <inheritdoc/>
        public event UIElementMouseWheelEventHandler MouseWheelEvent
        {
            add { AddHandler(Mouse.MouseWheelEvent, value); }
            remove { RemoveHandler(Mouse.MouseWheelEvent, value); }
        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.GotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnGotKeyboardFocus(ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.LostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnLostKeyboardFocus(ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyDownEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyUpEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnKeyUp(KeyboardDevice device, Key key, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.TextInputEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnTextInput(KeyboardDevice device, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.GotMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnGotMouseCapture(ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.LostMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnLostMouseCapture(ref Boolean handled)
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
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseEnterEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseEnter(MouseDevice device, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseLeaveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseLeave(MouseDevice device, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseUp(MouseDevice device, MouseButton button, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseDown(MouseDevice device, MouseButton button, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseClick(MouseDevice device, MouseButton button, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDoubleClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseDoubleClick(MouseDevice device, MouseButton button, ref Boolean handled)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseWheelEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        protected virtual void OnMouseWheel(MouseDevice device, Double x, Double y, ref Boolean handled)
        {

        }

        /// <summary>
        /// Registers class handlers for this type's input events.
        /// </summary>
        private static void RegisterInputClassHandlers()
        {
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Keyboard.GotKeyboardFocusEvent, new UIElementRoutedEventHandler(OnGotKeyboardFocusProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Keyboard.LostKeyboardFocusEvent, new UIElementRoutedEventHandler(OnLostKeyboardFocusProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Keyboard.KeyDownEvent, new UIElementKeyDownEventHandler(OnKeyDownProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Keyboard.KeyUpEvent, new UIElementKeyEventHandler(OnKeyUpProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Keyboard.TextInputEvent, new UIElementKeyboardEventHandler(OnTextInputProxy));

            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.GotMouseCaptureEvent, new UIElementRoutedEventHandler(OnGotMouseCaptureProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.LostMouseCaptureEvent, new UIElementRoutedEventHandler(OnLostMouseCaptureProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseMoveEvent, new UIElementMouseMoveEventHandler(OnMouseMoveProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseEnterEvent, new UIElementMouseEventHandler(OnMouseEnterProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseLeaveEvent, new UIElementMouseEventHandler(OnMouseLeaveProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseDownEvent, new UIElementMouseButtonEventHandler(OnMouseDownProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseUpEvent, new UIElementMouseButtonEventHandler(OnMouseUpProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseClickEvent, new UIElementMouseButtonEventHandler(OnMouseClickProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseDoubleClickEvent, new UIElementMouseButtonEventHandler(OnMouseDoubleClickProxy));
            RoutedEvent.RegisterClassHandler(typeof(UIElement), Mouse.MouseWheelEvent, new UIElementMouseWheelEventHandler(OnMouseWheelProxy));
        }

        /// <summary>
        /// Invokes the <see cref="OnGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnGotKeyboardFocusProxy(UIElement element, ref Boolean handled)
        {
            element.OnGotKeyboardFocus(ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostKeyboardFocus"/> method.
        /// </summary>
        private static void OnLostKeyboardFocusProxy(UIElement element, ref Boolean handled)
        {
            element.OnLostKeyboardFocus(ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyDown"/> method.
        /// </summary>
        private static void OnKeyDownProxy(UIElement element, KeyboardDevice device, Key key, ModifierKeys modifiers, ref Boolean handled)
        {
            element.OnKeyDown(device, key, modifiers, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyUp"/> method.
        /// </summary>
        private static void OnKeyUpProxy(UIElement element, KeyboardDevice device, Key key, ref Boolean handled)
        {
            element.OnKeyUp(device, key, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextInput"/> method.
        /// </summary>
        private static void OnTextInputProxy(UIElement element, KeyboardDevice device, ref Boolean handled)
        {
            element.OnTextInput(device, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotMouseCapture"/> method.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnGotMouseCaptureProxy(UIElement element, ref Boolean handled)
        {
            element.OnGotMouseCapture(ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostMouseCapture"/> method.
        /// </summary>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnLostMouseCaptureProxy(UIElement element, ref Boolean handled)
        {
            element.OnLostMouseCapture(ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseMove"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseMoveProxy(UIElement element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref Boolean handled)
        {
            element.OnMouseMove(device, x, y, dx, dy, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseEnter"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseEnterProxy(UIElement element, MouseDevice device, ref Boolean handled)
        {
            element.IsHovering = true;
            element.OnMouseEnter(device, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseLeave"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseLeaveProxy(UIElement element, MouseDevice device, ref Boolean handled)
        {
            element.IsHovering = false;
            element.OnMouseLeave(device, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDown"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseDownProxy(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            element.OnMouseDown(device, button, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseUp"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was released.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseUpProxy(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            element.OnMouseUp(device, button, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseClick"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseClickProxy(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            element.OnMouseClick(device, button, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseDoubleClickProxy(UIElement element, MouseDevice device, MouseButton button, ref Boolean handled)
        {
            element.OnMouseDoubleClick(device, button, ref handled);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseWheel"/> method.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="handled">A value indicating whether the event has been handled.</param>
        private static void OnMouseWheelProxy(UIElement element, MouseDevice device, Double x, Double y, ref Boolean handled)
        {
            element.OnMouseWheel(device, x, y, ref handled);
        }
    }
}
