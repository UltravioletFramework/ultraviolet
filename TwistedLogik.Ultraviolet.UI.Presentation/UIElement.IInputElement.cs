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
        public event UpfRoutedEventHandler PreviewGotKeyboardFocus
        {
            add { AddHandler(Keyboard.PreviewGotKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewGotKeyboardFocusEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfRoutedEventHandler PreviewLostKeyboardFocus
        {
            add { AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, value); }
        }

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
        public event UpfKeyboardEventHandler PreviewTextEditing
        {
            add { AddHandler(Keyboard.PreviewTextEditingEvent, value); }
            remove { RemoveHandler(Keyboard.PreviewTextEditingEvent, value); }
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
        public event UpfKeyboardEventHandler TextEditing
        {
            add { AddHandler(Keyboard.TextEditingEvent, value); }
            remove { RemoveHandler(Keyboard.TextEditingEvent, value); }
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

        /// <inheritdoc/>
        public event UpfTouchTapEventHandler PreviewTap
        {
            add { AddHandler(Touch.PreviewTapEvent, value); }
            remove { RemoveHandler(Touch.PreviewTapEvent, value); }
        }
        
        /// <inheritdoc/>
        public event UpfTouchEventHandler PreviewFingerDown
        {
            add { AddHandler(Touch.PreviewFingerDownEvent, value); }
            remove { RemoveHandler(Touch.PreviewFingerDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler PreviewFingerUp
        {
            add { AddHandler(Touch.PreviewFingerUpEvent, value); }
            remove { RemoveHandler(Touch.PreviewFingerUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchMotionEventHandler PreviewFingerMotion
        {
            add { AddHandler(Touch.PreviewFingerMotionEvent, value); }
            remove { RemoveHandler(Touch.PreviewFingerMotionEvent, value); }
        }
        
        /// <inheritdoc/>
        public event UpfTouchTapEventHandler Tap
        {
            add { AddHandler(Touch.TapEvent, value); }
            remove { RemoveHandler(Touch.TapEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler FingerDown
        {
            add { AddHandler(Touch.FingerDownEvent, value); }
            remove { RemoveHandler(Touch.FingerDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler FingerUp
        {
            add { AddHandler(Touch.FingerUpEvent, value); }
            remove { RemoveHandler(Touch.FingerUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchMotionEventHandler FingerMotion
        {
            add { AddHandler(Touch.FingerMotionEvent, value); }
            remove { RemoveHandler(Touch.FingerMotionEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadAxisChangedEventHandler PreviewGamePadAxisChanged
        {
            add { AddHandler(GamePad.PreviewAxisChangedEvent, value); }
            remove { RemoveHandler(GamePad.PreviewAxisChangedEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadButtonDownEventHandler PreviewGamePadButtonDown
        {
            add { AddHandler(GamePad.PreviewButtonDownEvent, value); }
            remove { RemoveHandler(GamePad.PreviewButtonDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadButtonUpEventHandler PreviewGamePadButtonUp
        {
            add { AddHandler(GamePad.PreviewButtonUpEvent, value); }
            remove { RemoveHandler(GamePad.PreviewButtonUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadAxisChangedEventHandler GamePadAxisChanged
        {
            add { AddHandler(GamePad.AxisChangedEvent, value); }
            remove { RemoveHandler(GamePad.AxisChangedEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadAxisDownEventHandler GamePadAxisDown
        {
            add { AddHandler(GamePad.AxisDownEvent, value); }
            remove { RemoveHandler(GamePad.AxisDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadAxisUpEventHandler GamePadAxisUp
        {
            add { AddHandler(GamePad.AxisUpEvent, value); }
            remove { RemoveHandler(GamePad.AxisUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadButtonDownEventHandler GamePadButtonDown
        {
            add { AddHandler(GamePad.ButtonDownEvent, value); }
            remove { RemoveHandler(GamePad.ButtonDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGamePadButtonUpEventHandler GamePadButtonUp
        {
            add { AddHandler(GamePad.ButtonUpEvent, value); }
            remove { RemoveHandler(GamePad.ButtonUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGenericInteractionEventHandler PreviewGenericInteraction
        {
            add { AddHandler(Generic.PreviewGenericInteractionEvent, value); }
            remove { RemoveHandler(Generic.PreviewGenericInteractionEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfGenericInteractionEventHandler GenericInteraction
        {
            add { AddHandler(Generic.GenericInteractionEvent, value); }
            remove { RemoveHandler(Generic.GenericInteractionEvent, value); }
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
        /// Invoked whien the <see cref="FocusManager.GotFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotFocus(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked whien the <see cref="FocusManager.LostFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostFocus(ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewGotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewLostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.GotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.LostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
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
        /// Invoked when a <see cref="Keyboard.TextEditingEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTextEditing(KeyboardDevice device, ref RoutedEventData data)
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
        /// Invoked by the <see cref="Touch.TapEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
        /// <param name="x">The x-coordinate of the touch.</param>
        /// <param name="y">The y-coordinate of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTap(TouchDevice device, Int64 fingerID,
            Double x, Double y, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the the <see cref="Touch.FingerDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
        /// <param name="x">The x-coordinate of the touch.</param>
        /// <param name="y">The y-coordinate of the touch.</param>
        /// <param name="pressure">The normalized pressure value of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnFingerDown(TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.FingerUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
        /// <param name="x">The x-coordinate of the touch.</param>
        /// <param name="y">The y-coordinate of the touch.</param>
        /// <param name="pressure">The normalized pressure value of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnFingerUp(TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
        }

        /// <summary>
        /// Invoked by the <see cref="Touch.FingerMotionEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="fingerID">The identifier of the finger that caused the touch.</param>
        /// <param name="x">The x-coordinate of the touch.</param>
        /// <param name="y">The y-coordinate of the touch.</param>
        /// <param name="dx">The amount of motion along the x-axis.</param>
        /// <param name="dy">The amount of motion along the y-axis.</param>
        /// <param name="pressure">The normalized pressure value of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnFingerMotion(TouchDevice device, Int64 fingerID,
            Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invokes by the <see cref="GamePad.AxisChangedEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was changed.</param>
        /// <param name="value">The value to which the axis was changed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisChanged(GamePadDevice device, GamePadAxis axis, Single value, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invokes by the <see cref="GamePad.AxisDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was pressed.</param>
        /// <param name="value">The axis' value when it was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated axis press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invokes by the <see cref="GamePad.AxisUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisUp(GamePadDevice device, GamePadAxis axis, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invokes by the <see cref="GamePad.ButtonDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invokes by the <see cref="GamePad.ButtonUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadButtonUp(GamePadDevice device, GamePadButton button, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Generic.GenericInteractionEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The input device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGenericInteraction(UltravioletResource device, ref RoutedEventData data)
        {

        }

        /// <summary>
        /// Registers class handlers for this type's input events.
        /// </summary>
        private static void RegisterInputClassHandlers()
        {
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewGotKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(OnPreviewGotKeyboardFocusProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewLostKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(OnPreviewLostKeyboardFocusProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.GotKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(OnGotKeyboardFocusProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.LostKeyboardFocusEvent, new UpfKeyboardFocusChangedEventHandler(OnLostKeyboardFocusProxy));
            
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyDownEvent, new UpfKeyDownEventHandler(OnKeyDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyUpEvent, new UpfKeyEventHandler(OnKeyUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.TextInputEvent, new UpfKeyboardEventHandler(OnTextInputProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.TextEditingEvent, new UpfKeyboardEventHandler(OnTextEditingProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.GotMouseCaptureEvent, new UpfRoutedEventHandler(OnGotMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.LostMouseCaptureEvent, new UpfRoutedEventHandler(OnLostMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseEnterEvent, new UpfMouseEventHandler(OnMouseEnterProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseLeaveEvent, new UpfMouseEventHandler(OnMouseLeaveProxy));
            
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseMoveEvent, new UpfMouseMoveEventHandler(OnMouseMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDownEvent, new UpfMouseButtonEventHandler(OnMouseDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseUpEvent, new UpfMouseButtonEventHandler(OnMouseUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseClickEvent, new UpfMouseButtonEventHandler(OnMouseClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDoubleClickEvent, new UpfMouseButtonEventHandler(OnMouseDoubleClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseWheelEvent, new UpfMouseWheelEventHandler(OnMouseWheelProxy));
            
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TapEvent, new UpfTouchTapEventHandler(OnTapProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.FingerDownEvent, new UpfTouchEventHandler(OnFingerDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.FingerUpEvent, new UpfTouchEventHandler(OnFingerUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.FingerMotionEvent, new UpfTouchMotionEventHandler(OnFingerMotionProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisChangedEvent, new UpfGamePadAxisChangedEventHandler(OnGamePadAxisChangedProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisDownEvent, new UpfGamePadAxisDownEventHandler(OnGamePadAxisDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisUpEvent, new UpfGamePadAxisUpEventHandler(OnGamePadAxisUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.ButtonDownEvent, new UpfGamePadButtonDownEventHandler(OnGamePadButtonDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.ButtonUpEvent, new UpfGamePadButtonUpEventHandler(OnGamePadButtonUpProxy));
            
            EventManager.RegisterClassHandler(typeof(UIElement), Generic.GenericInteractionEvent, new UpfGenericInteractionEventHandler(OnGenericInteractionProxy));
        }
        
        /// <summary>
        /// Invokes the <see cref="OnPreviewGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnPreviewGotKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGotKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnPreviewLostKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            ((UIElement)element).OnPreviewLostKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnGotKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            ((UIElement)element).OnGotKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostKeyboardFocus"/> method.
        /// </summary>
        private static void OnLostKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, ref RoutedEventData data)
        {
            ((UIElement)element).OnLostKeyboardFocus(device, oldFocus, newFocus, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyDown"/> method.
        /// </summary>
        private static void OnKeyDownProxy(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, ref RoutedEventData data)
        {
            ((UIElement)element).OnKeyDown(device, key, modifiers, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyUp"/> method.
        /// </summary>
        private static void OnKeyUpProxy(DependencyObject element, KeyboardDevice device, Key key, ref RoutedEventData data)
        {
            ((UIElement)element).OnKeyUp(device, key, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextInput"/> method.
        /// </summary>
        private static void OnTextInputProxy(DependencyObject element, KeyboardDevice device, ref RoutedEventData data)
        {
            ((UIElement)element).OnTextInput(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextEditing"/> method.
        /// </summary>
        private static void OnTextEditingProxy(DependencyObject element, KeyboardDevice device, ref RoutedEventData data)
        {
            ((UIElement)element).OnTextEditing(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotMouseCapture"/> method.
        /// </summary>
        private static void OnGotMouseCaptureProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnGotMouseCapture(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostMouseCapture"/> method.
        /// </summary>
        private static void OnLostMouseCaptureProxy(DependencyObject element, ref RoutedEventData data)
        {
            ((UIElement)element).OnLostMouseCapture(ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseEnter"/> method.
        /// </summary>
        private static void OnMouseEnterProxy(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseEnter(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseLeave"/> method.
        /// </summary>
        private static void OnMouseLeaveProxy(DependencyObject element, MouseDevice device, ref RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseLeave(device, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseMove"/> method.
        /// </summary>
        private static void OnMouseMoveProxy(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseMove(device, x, y, dx, dy, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDown"/> method.
        /// </summary>
        private static void OnMouseDownProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseDown(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseUp"/> method.
        /// </summary>
        private static void OnMouseUpProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseUp(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseClick"/> method.
        /// </summary>
        private static void OnMouseClickProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseClick(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        private static void OnMouseDoubleClickProxy(DependencyObject element, MouseDevice device, MouseButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseDoubleClick(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseWheel"/> method.
        /// </summary>
        private static void OnMouseWheelProxy(DependencyObject element, MouseDevice device, Double x, Double y, ref RoutedEventData data)
        {
            ((UIElement)element).OnMouseWheel(device, x, y, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTap"/> method.
        /// </summary>
        private static void OnTapProxy(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, ref RoutedEventData data)
        {
            ((UIElement)element).OnTap(device, fingerID, x, y, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnFingerDown"/> method.
        /// </summary>
        private static void OnFingerDownProxy(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            ((UIElement)element).OnFingerDown(device, fingerID, x, y, pressure, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnFingerUp"/> method.
        /// </summary>
        private static void OnFingerUpProxy(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Single pressure, ref RoutedEventData data)
        {
            ((UIElement)element).OnFingerUp(device, fingerID, x, y, pressure, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnFingerMotion"/> method.
        /// </summary>
        private static void OnFingerMotionProxy(DependencyObject element, TouchDevice device, Int64 fingerID,
            Double x, Double y, Double dx, Double dy, Single pressure, ref RoutedEventData data)
        {
            ((UIElement)element).OnFingerMotion(device, fingerID, x, y, dx, dy, pressure, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisChanged"/> method.
        /// </summary>
        private static void OnGamePadAxisChangedProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, ref RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisChanged(device, axis, value, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisDown"/> method.
        /// </summary>
        private static void OnGamePadAxisDownProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, ref RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisDown(device, axis, value, repeat, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisUp"/> method.
        /// </summary>
        private static void OnGamePadAxisUpProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, ref RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisUp(device, axis, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadButtonDown"/> method.
        /// </summary>
        private static void OnGamePadButtonDownProxy(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, ref RoutedEventData data)
        {
            ((UIElement)element).OnGamePadButtonDown(device, button, repeat, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadButtonUp"/> method.
        /// </summary>
        private static void OnGamePadButtonUpProxy(DependencyObject element, GamePadDevice device, GamePadButton button, ref RoutedEventData data)
        {
            ((UIElement)element).OnGamePadButtonUp(device, button, ref data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGenericInteraction"/> method.
        /// </summary>
        private static void OnGenericInteractionProxy(DependencyObject element, UltravioletResource device, ref RoutedEventData data)
        {
            ((UIElement)element).OnGenericInteraction(device, ref data);
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
