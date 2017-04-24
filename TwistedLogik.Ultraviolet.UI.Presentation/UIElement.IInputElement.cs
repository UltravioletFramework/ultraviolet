using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation
{
    partial class UIElement : IInputElement
    {
        /// <inheritdoc/>
        public Boolean IsKeyboardFocused
        {
            get { return GetValue<Boolean>(IsKeyboardFocusedProperty); }
            internal set { SetValue(IsKeyboardFocusedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsKeyboardFocusWithin
        {
            get { return GetValue<Boolean>(IsKeyboardFocusWithinProperty); }
            internal set { SetValue(IsKeyboardFocusWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseCaptured
        {
            get { return GetValue<Boolean>(IsMouseCapturedProperty); }
            internal set { SetValue(IsMouseCapturedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseCaptureWithin
        {
            get { return GetValue<Boolean>(IsMouseCaptureWithinProperty); }
            internal set { SetValue(IsMouseCaptureWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseOver
        {
            get { return GetValue<Boolean>(IsMouseOverProperty); }
            internal set { SetValue(IsMouseOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean IsMouseDirectlyOver
        {
            get { return GetValue<Boolean>(IsMouseDirectlyOverProperty); }
            internal set { SetValue(IsMouseDirectlyOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyTouchesCaptured
        {
            get { return GetValue<Boolean>(AreAnyTouchesCapturedProperty); }
            internal set { SetValue(AreAnyTouchesCapturedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyTouchesCapturedWithin
        {
            get { return GetValue<Boolean>(AreAnyTouchesCapturedWithinProperty); }
            internal set { SetValue(AreAnyTouchesCapturedWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreNewTouchesCaptured
        {
            get { return GetValue<Boolean>(AreNewTouchesCapturedProperty); }
            internal set { SetValue(AreNewTouchesCapturedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreNewTouchesCapturedWithin
        {
            get { return GetValue<Boolean>(AreNewTouchesCapturedWithinProperty); }
            internal set { SetValue(AreNewTouchesCapturedWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyTouchesOver
        {
            get { return GetValue<Boolean>(AreAnyTouchesOverProperty); }
            internal set { SetValue(AreAnyTouchesOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyTouchesDirectlyOver
        {
            get { return GetValue<Boolean>(AreAnyTouchesDirectlyOverProperty); }
            internal set { SetValue(AreAnyTouchesDirectlyOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyCursorsCaptured
        {
            get { return GetValue<Boolean>(AreAnyCursorsCapturedProperty); }
            internal set { SetValue(AreAnyCursorsCapturedPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyCursorsCapturedWithin
        {
            get { return GetValue<Boolean>(AreAnyCursorsCapturedWithinProperty); }
            internal set { SetValue(AreAnyCursorsCapturedWithinPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyCursorsOver
        {
            get { return GetValue<Boolean>(AreAnyCursorsOverProperty); }
            internal set { SetValue(AreAnyCursorsOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public Boolean AreAnyCursorsDirectlyOver
        {
            get { return GetValue<Boolean>(AreAnyCursorsDirectlyOverProperty); }
            internal set { SetValue(AreAnyCursorsDirectlyOverPropertyKey, value); }
        }

        /// <inheritdoc/>
        public TouchesCollection TouchesCaptured => touchesCaptured;

        /// <inheritdoc/>
        public TouchesCollection TouchesCapturedWithin => touchesCapturedWithin;

        /// <inheritdoc/>
        public TouchesCollection TouchesOver => touchesOver;

        /// <inheritdoc/>
        public TouchesCollection TouchesDirectlyOver => touchesDirectlyOver;

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
        /// Identifies the <see cref="AreAnyTouchesCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-captured'.</remarks>
        private static readonly DependencyPropertyKey AreAnyTouchesCapturedPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyTouchesCaptured", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyTouchesCapturedChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-captured'.</remarks>
        public static readonly DependencyProperty AreAnyTouchesCapturedProperty = AreAnyTouchesCapturedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-captured-within'.</remarks>
        private static readonly DependencyPropertyKey AreAnyTouchesCapturedWithinPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyTouchesCapturedWithin", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyTouchesCapturedWithinChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-captured-within'.</remarks>
        public static readonly DependencyProperty AreAnyTouchesCapturedWithinProperty = AreAnyTouchesCapturedWithinPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreNewTouchesCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'new-touches-captured'.</remarks>
        private static readonly DependencyPropertyKey AreNewTouchesCapturedPropertyKey = DependencyProperty.RegisterReadOnly("AreNewTouchesCaptured", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreNewTouchesCapturedChanged));

        /// <summary>
        /// Identifies the <see cref="AreNewTouchesCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'new-touches-captured'.</remarks>
        public static readonly DependencyProperty AreNewTouchesCapturedProperty = AreNewTouchesCapturedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreNewTouchesCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'new-touches-captured-within'.</remarks>
        private static readonly DependencyPropertyKey AreNewTouchesCapturedWithinPropertyKey = DependencyProperty.RegisterReadOnly("AreNewTouchesCapturedWithin", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreNewTouchesCapturedWithinChanged));

        /// <summary>
        /// Identifies the <see cref="AreNewTouchesCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'new-touches-captured-within'.</remarks>
        public static readonly DependencyProperty AreNewTouchesCapturedWithinProperty = AreNewTouchesCapturedWithinPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-over'.</remarks>
        private static readonly DependencyPropertyKey AreAnyTouchesOverPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyTouchesOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyTouchesOverChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-over'.</remarks>
        public static readonly DependencyProperty AreAnyTouchesOverProperty = AreAnyTouchesOverPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesDirectlyOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-directly-over'.</remarks>
        public static readonly DependencyPropertyKey AreAnyTouchesDirectlyOverPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyTouchesDirectlyOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyTouchesDirectlyOverChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-captured'.</remarks>
        private static readonly DependencyPropertyKey AreAnyCursorsCapturedPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyCursorsCaptured", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyCursorsCapturedChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsCaptured"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-captured'.</remarks>
        public static readonly DependencyProperty AreAnyCursorsCapturedProperty = AreAnyCursorsCapturedPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-captured-within'.</remarks>
        private static readonly DependencyPropertyKey AreAnyCursorsCapturedWithinPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyCursorsCapturedWithin", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyCursorsCapturedWithinChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsCapturedWithin"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-captured-within'.</remarks>
        public static readonly DependencyProperty AreAnyCursorsCapturedWithinProperty = AreAnyCursorsCapturedWithinPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-over'.</remarks>
        private static readonly DependencyPropertyKey AreAnyCursorsOverPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyCursorsOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyCursorsOverChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-over'.</remarks>
        public static readonly DependencyProperty AreAnyCursorsOverProperty = AreAnyCursorsOverPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsDirectlyOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-directly-over'.</remarks>
        private static readonly DependencyPropertyKey AreAnyCursorsDirectlyOverPropertyKey = DependencyProperty.RegisterReadOnly("AreAnyCursorsDirectlyOver", typeof(Boolean), typeof(UIElement),
            new PropertyMetadata<Boolean>(HandleAreAnyCursorsDirectlyOverChanged));

        /// <summary>
        /// Identifies the <see cref="AreAnyCursorsDirectlyOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-cursors-directly-over'.</remarks>
        public static readonly DependencyProperty AreAnyCursorsDirectlyOverProperty = AreAnyCursorsDirectlyOverPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="AreAnyTouchesDirectlyOver"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'any-touches-directly-over'.</remarks>
        public static readonly DependencyProperty AreAnyTouchesDirectlyOverProperty = AreAnyTouchesDirectlyOverPropertyKey.DependencyProperty;

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

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesCaptured"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyTouchesCapturedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesCapturedWithin"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyTouchesCapturedWithinChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreNewTouchesCaptured"/> property changes.
        /// </summary>
        public event UpfEventHandler AreNewTouchesCapturedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreNewTouchesCapturedWithin"/> property changes.
        /// </summary>
        public event UpfEventHandler AreNewTouchesCapturedWithinChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesOver"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyTouchesOverChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesDirectlyOver"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyTouchesDirectlyOverChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsCaptured"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyCursorsCapturedChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsCapturedWithin"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyCursorsCapturedWithinChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsOver"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyCursorsOverChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsDirectlyOver"/> property changes.
        /// </summary>
        public event UpfEventHandler AreAnyCursorsDirectlyOverChanged;

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
        public event UpfRoutedEventHandler GotMouseCapture
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
        public event UpfMouseEventHandler MouseEnter
        {
            add { AddHandler(Mouse.MouseEnterEvent, value); }
            remove { RemoveHandler(Mouse.MouseEnterEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMouseEventHandler MouseLeave
        {
            add { AddHandler(Mouse.MouseLeaveEvent, value); }
            remove { RemoveHandler(Mouse.MouseLeaveEvent, value); }
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
        public event UpfTouchEventHandler GotTouchCapture
        {
            add { AddHandler(Touch.GotTouchCaptureEvent, value); }
            remove { RemoveHandler(Touch.GotTouchCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler GotNewTouchCapture
        {
            add { AddHandler(Touch.GotNewTouchCaptureEvent, value); }
            remove { RemoveHandler(Touch.GotNewTouchCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler LostTouchCapture
        {
            add { AddHandler(Touch.LostTouchCaptureEvent, value); }
            remove { RemoveHandler(Touch.LostTouchCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler LostNewTouchCapture
        {
            add { AddHandler(Touch.LostNewTouchCaptureEvent, value); }
            remove { RemoveHandler(Touch.LostNewTouchCaptureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchMoveEventHandler PreviewTouchMove
        {
            add { AddHandler(Touch.PreviewTouchMoveEvent, value); }
            remove { RemoveHandler(Touch.PreviewTouchMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchDownEventHandler PreviewTouchDown
        {
            add { AddHandler(Touch.PreviewTouchDownEvent, value); }
            remove { RemoveHandler(Touch.PreviewTouchDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchUpEventHandler PreviewTouchUp
        {
            add { AddHandler(Touch.PreviewTouchUpEvent, value); }
            remove { RemoveHandler(Touch.PreviewTouchUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchTapEventHandler PreviewTouchTap
        {
            add { AddHandler(Touch.PreviewTouchTapEvent, value); }
            remove { RemoveHandler(Touch.PreviewTouchTapEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchLongPressEventHandler PreviewTouchLongPress
        {
            add { AddHandler(Touch.TouchLongPressEvent, value); }
            remove { RemoveHandler(Touch.TouchLongPressEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMultiGestureEventHandler PreviewMultiGesture
        {
            add { AddHandler(Touch.PreviewMultiGestureEvent, value); }
            remove { RemoveHandler(Touch.PreviewMultiGestureEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler TouchEnter
        {
            add { AddHandler(Touch.TouchEnterEvent, value); }
            remove { RemoveHandler(Touch.TouchEnterEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchEventHandler TouchLeave
        {
            add { AddHandler(Touch.TouchLeaveEvent, value); }
            remove { RemoveHandler(Touch.TouchLeaveEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchMoveEventHandler TouchMove
        {
            add { AddHandler(Touch.TouchMoveEvent, value); }
            remove { RemoveHandler(Touch.TouchMoveEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchDownEventHandler TouchDown
        {
            add { AddHandler(Touch.TouchDownEvent, value); }
            remove { RemoveHandler(Touch.TouchDownEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchUpEventHandler TouchUp
        {
            add { AddHandler(Touch.TouchUpEvent, value); }
            remove { RemoveHandler(Touch.TouchUpEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchTapEventHandler TouchTap
        {
            add { AddHandler(Touch.TouchTapEvent, value); }
            remove { RemoveHandler(Touch.TouchTapEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfTouchLongPressEventHandler TouchLongPress
        {
            add { AddHandler(Touch.TouchLongPressEvent, value); }
            remove { RemoveHandler(Touch.TouchLongPressEvent, value); }
        }

        /// <inheritdoc/>
        public event UpfMultiGestureEventHandler MultiGesture
        {
            add { AddHandler(Touch.MultiGestureEvent, value); }
            remove { RemoveHandler(Touch.MultiGestureEvent, value); }
        }

        /// <summary>
        /// Raises the <see cref="IsKeyboardFocusedChanged"/> event.
        /// </summary>
        protected virtual void OnIsKeyboardFocusedChanged() =>
            IsKeyboardFocusedChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="IsKeyboardFocusWithinChanged"/> event.
        /// </summary>
        protected virtual void OnIsKeyboardFocusWithinChanged() =>
            IsKeyboardFocusWithinChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="IsMouseCapturedChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseCapturedChanged() =>
            IsMouseCapturedChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="IsMouseCaptureWithinChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseCaptureWithinChanged() =>
            IsMouseCaptureWithinChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="IsMouseOverChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseOverChanged() =>
            IsMouseOverChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="IsMouseDirectlyOverChanged"/> event.
        /// </summary>
        protected virtual void OnIsMouseDirectlyOverChanged() =>
            IsMouseDirectlyOverChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyTouchesCapturedChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyTouchesCapturedChanged() =>
            AreAnyTouchesCapturedChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyTouchesCapturedWithinChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyTouchesCapturedWithinChanged() =>
            AreAnyTouchesCapturedWithinChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreNewTouchesCapturedChanged"/> event.
        /// </summary>
        protected virtual void OnAreNewTouchesCapturedChanged() =>
            AreNewTouchesCapturedChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreNewTouchesCapturedWithinChanged"/> event.
        /// </summary>
        protected virtual void OnAreNewTouchesCapturedWithinChanged() =>
            AreNewTouchesCapturedWithinChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyTouchesOverChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyTouchesOverChanged() =>
            AreAnyTouchesOverChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyTouchesDirectlyOverChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyTouchesDirectlyOverChanged() =>
            AreAnyTouchesDirectlyOverChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyCursorsCapturedChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyCursorsCapturedChanged() =>
            AreAnyCursorsCapturedChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyCursorsCapturedWithinChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyCursorsCapturedWithinChanged() =>
            AreAnyCursorsCapturedWithinChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyCursorsOverChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyCursorsOverChanged() =>
            AreAnyCursorsOverChanged?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AreAnyCursorsDirectlyOverChanged"/> event.
        /// </summary>
        protected virtual void OnAreAnyCursorsDirectlyOverChanged() =>
            AreAnyCursorsDirectlyOverChanged?.Invoke(this);

        /// <summary>
        /// Invoked when the <see cref="FocusManager.GotFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotFocus(RoutedEventData data)
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(GotFocusEvent);
            evtDelegate(this, data);
        }

        /// <summary>
        /// Invoked when the <see cref="FocusManager.LostFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostFocus(RoutedEventData data)
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(LostFocusEvent);
            evtDelegate(this, data);
        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewGotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewLostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.GotKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.LostKeyboardFocusEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The keyboard device that raised the event.</param>
        /// <param name="oldFocus">The element that previously had focus.</param>
        /// <param name="newFocus">The element that currently has focus.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {

        }
        
        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewTextInputEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTextInput(KeyboardDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewTextEditingEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTextEditing(KeyboardDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewKeyDownEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.PreviewKeyUpEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewKeyUp(KeyboardDevice device, Key key, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.TextInputEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTextInput(KeyboardDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.TextEditingEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTextEditing(KeyboardDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyDownEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked when a <see cref="Keyboard.KeyUpEvent"/> attached routed event occurs.
        /// </summary>
        /// <param name="device">The <see cref="KeyboardDevice"/> that raised the event.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnKeyUp(KeyboardDevice device, Key key, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.GotMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotMouseCapture(RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.LostMouseCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostMouseCapture(RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseMoveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The x-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the cursor in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {

        }
        
        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseDoubleClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.PreviewMouseWheelEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data)
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
        protected virtual void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseEnterEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseEnter(MouseDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseLeaveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseLeave(MouseDevice device, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseDoubleClickEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Mouse.MouseWheelEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.PreviewAxisChangedEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was changed.</param>
        /// <param name="value">The value to which the axis was changed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGamePadAxisChanged(GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.PreviewAxisDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was pressed.</param>
        /// <param name="value">The axis' value when it was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated axis press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.PreviewAxisUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGamePadAxisUp(GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.PreviewButtonDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.PreviewButtonUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewGamePadButtonUp(GamePadDevice device, GamePadButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.AxisChangedEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was changed.</param>
        /// <param name="value">The value to which the axis was changed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisChanged(GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.AxisDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was pressed.</param>
        /// <param name="value">The axis' value when it was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated axis press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisDown(GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.AxisUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="axis">The axis that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadAxisUp(GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.ButtonDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="GamePad.ButtonUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The button that was released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGamePadButtonUp(GamePadDevice device, GamePadButton button, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.GotTouchCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch that was captured.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotTouchCapture(TouchDevice device, Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.GotNewTouchCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnGotNewTouchCapture(RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.LostTouchCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch that was captured.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostTouchCapture(TouchDevice device, Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.LostNewTouchCaptureEvent"/> attached routed event.
        /// </summary>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnLostNewTouchCapture(RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchEnterEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch that was captured.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchEnter(TouchDevice device, Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchLeaveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch that was captured.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchLeave(TouchDevice device, Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewTouchMoveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the touch's 
        /// current position and the x-coordinate of the touch's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the touch's 
        /// current position and the y-coordinate of the touch's previous position.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTouchMove(TouchDevice device,
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewTouchDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTouchDown(TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewTouchUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTouchUp(TouchDevice device,
            Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewTouchTapEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTouchTap(TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewTouchLongPressEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewTouchLongPress(TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.PreviewMultiGestureEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="x">The x-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
        /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
        /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
        /// <param name="fingers">The number of fingers involved in the gesture.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnPreviewMultiGesture(TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchMoveEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="dx">The difference between the x-coordinate of the touch's 
        /// current position and the x-coordinate of the touch's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the touch's 
        /// current position and the y-coordinate of the touch's previous position.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchMove(TouchDevice device, 
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchDownEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchDown(TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchUpEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchUp(TouchDevice device,
            Int64 id, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchTapEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchTap(TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.TouchLongPressEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="id">The unique identifier of the touch.</param>
        /// <param name="x">The x-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the touch in device-independent screen coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnTouchLongPress(TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {

        }

        /// <summary>
        /// Invoked by the <see cref="Touch.MultiGestureEvent"/> attached routed event.
        /// </summary>
        /// <param name="device">The touch device.</param>
        /// <param name="x">The x-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
        /// <param name="y">The y-coordinate of the gesture's centroid in device-independent screen coordinates.</param>
        /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
        /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
        /// <param name="fingers">The number of fingers involved in the gesture.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        protected virtual void OnMultiGesture(TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
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

            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewTextInputEvent, new UpfKeyboardEventHandler(OnPreviewTextInputProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewTextEditingEvent, new UpfKeyboardEventHandler(OnPreviewTextEditingProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewKeyDownEvent, new UpfKeyDownEventHandler(OnPreviewKeyDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.PreviewKeyUpEvent, new UpfKeyEventHandler(OnPreviewKeyUpProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.TextInputEvent, new UpfKeyboardEventHandler(OnTextInputProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.TextEditingEvent, new UpfKeyboardEventHandler(OnTextEditingProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyDownEvent, new UpfKeyDownEventHandler(OnKeyDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Keyboard.KeyUpEvent, new UpfKeyEventHandler(OnKeyUpProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.GotMouseCaptureEvent, new UpfRoutedEventHandler(OnGotMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.LostMouseCaptureEvent, new UpfRoutedEventHandler(OnLostMouseCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseEnterEvent, new UpfMouseEventHandler(OnMouseEnterProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseLeaveEvent, new UpfMouseEventHandler(OnMouseLeaveProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseMoveEvent, new UpfMouseMoveEventHandler(OnPreviewMouseMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseDownEvent, new UpfMouseButtonEventHandler(OnPreviewMouseDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseUpEvent, new UpfMouseButtonEventHandler(OnPreviewMouseUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseClickEvent, new UpfMouseButtonEventHandler(OnPreviewMouseClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseDoubleClickEvent, new UpfMouseButtonEventHandler(OnPreviewMouseDoubleClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.PreviewMouseWheelEvent, new UpfMouseWheelEventHandler(OnPreviewMouseWheelProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseMoveEvent, new UpfMouseMoveEventHandler(OnMouseMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDownEvent, new UpfMouseButtonEventHandler(OnMouseDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseUpEvent, new UpfMouseButtonEventHandler(OnMouseUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseClickEvent, new UpfMouseButtonEventHandler(OnMouseClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseDoubleClickEvent, new UpfMouseButtonEventHandler(OnMouseDoubleClickProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Mouse.MouseWheelEvent, new UpfMouseWheelEventHandler(OnMouseWheelProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.PreviewAxisChangedEvent, new UpfGamePadAxisChangedEventHandler(OnPreviewGamePadAxisChangedProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.PreviewAxisDownEvent, new UpfGamePadAxisDownEventHandler(OnPreviewGamePadAxisDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.PreviewAxisUpEvent, new UpfGamePadAxisUpEventHandler(OnPreviewGamePadAxisUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.PreviewButtonDownEvent, new UpfGamePadButtonDownEventHandler(OnPreviewGamePadButtonDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.PreviewButtonUpEvent, new UpfGamePadButtonUpEventHandler(OnPreviewGamePadButtonUpProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisChangedEvent, new UpfGamePadAxisChangedEventHandler(OnGamePadAxisChangedProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisDownEvent, new UpfGamePadAxisDownEventHandler(OnGamePadAxisDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.AxisUpEvent, new UpfGamePadAxisUpEventHandler(OnGamePadAxisUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.ButtonDownEvent, new UpfGamePadButtonDownEventHandler(OnGamePadButtonDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), GamePad.ButtonUpEvent, new UpfGamePadButtonUpEventHandler(OnGamePadButtonUpProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Touch.GotTouchCaptureEvent, new UpfTouchEventHandler(OnGotTouchCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.GotNewTouchCaptureEvent, new UpfRoutedEventHandler(OnGotNewTouchCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.LostTouchCaptureEvent, new UpfTouchEventHandler(OnLostTouchCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.LostNewTouchCaptureEvent, new UpfRoutedEventHandler(OnLostNewTouchCaptureProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchEnterEvent, new UpfTouchEventHandler(OnTouchEnterProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchLeaveEvent, new UpfTouchEventHandler(OnTouchLeaveProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewTouchMoveEvent, new UpfTouchMoveEventHandler(OnPreviewTouchMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewTouchDownEvent, new UpfTouchDownEventHandler(OnPreviewTouchDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewTouchUpEvent, new UpfTouchUpEventHandler(OnPreviewTouchUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewTouchTapEvent, new UpfTouchTapEventHandler(OnPreviewTouchTapProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewTouchLongPressEvent, new UpfTouchLongPressEventHandler(OnPreviewTouchLongPressProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.PreviewMultiGestureEvent, new UpfMultiGestureEventHandler(OnPreviewMultiGestureProxy));

            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchMoveEvent, new UpfTouchMoveEventHandler(OnTouchMoveProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchDownEvent, new UpfTouchDownEventHandler(OnTouchDownProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchUpEvent, new UpfTouchUpEventHandler(OnTouchUpProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchTapEvent, new UpfTouchTapEventHandler(OnTouchTapProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.TouchLongPressEvent, new UpfTouchLongPressEventHandler(OnTouchLongPressProxy));
            EventManager.RegisterClassHandler(typeof(UIElement), Touch.MultiGestureEvent, new UpfMultiGestureEventHandler(OnMultiGestureProxy));
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnPreviewGotKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnPreviewLostKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotKeyboardFocus"/> method.
        /// </summary>
        private static void OnGotKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            ((UIElement)element).OnGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostKeyboardFocus"/> method.
        /// </summary>
        private static void OnLostKeyboardFocusProxy(DependencyObject element, KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            ((UIElement)element).OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewTextInput"/> method.
        /// </summary>
        private static void OnPreviewTextInputProxy(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTextInput(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewTextEditing"/> method.
        /// </summary>
        private static void OnPreviewTextEditingProxy(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTextEditing(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewKeyDown"/> method.
        /// </summary>
        private static void OnPreviewKeyDownProxy(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewKeyDown(device, key, modifiers, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewKeyUp"/> method.
        /// </summary>
        private static void OnPreviewKeyUpProxy(DependencyObject element, KeyboardDevice device, Key key, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewKeyUp(device, key, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextInput"/> method.
        /// </summary>
        private static void OnTextInputProxy(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            ((UIElement)element).OnTextInput(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnTextEditing"/> method.
        /// </summary>
        private static void OnTextEditingProxy(DependencyObject element, KeyboardDevice device, RoutedEventData data)
        {
            ((UIElement)element).OnTextEditing(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyDown"/> method.
        /// </summary>
        private static void OnKeyDownProxy(DependencyObject element, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            if (!data.Handled)
                CommandManager.HandleKeyDownTranslation(element, device, key, modifiers, data);

            if (!data.Handled)
                ((UIElement)element).OnKeyDown(device, key, modifiers, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnKeyUp"/> method.
        /// </summary>
        private static void OnKeyUpProxy(DependencyObject element, KeyboardDevice device, Key key, RoutedEventData data)
        {
            ((UIElement)element).OnKeyUp(device, key, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGotMouseCapture"/> method.
        /// </summary>
        private static void OnGotMouseCaptureProxy(DependencyObject element, RoutedEventData data)
        {
            ((UIElement)element).OnGotMouseCapture(data);
        }

        /// <summary>
        /// Invokes the <see cref="OnLostMouseCapture"/> method.
        /// </summary>
        private static void OnLostMouseCaptureProxy(DependencyObject element, RoutedEventData data)
        {
            ((UIElement)element).OnLostMouseCapture(data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseEnter"/> method.
        /// </summary>
        private static void OnMouseEnterProxy(DependencyObject element, MouseDevice device, RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseEnter(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseLeave"/> method.
        /// </summary>
        private static void OnMouseLeaveProxy(DependencyObject element, MouseDevice device, RoutedEventData data)
        {
            var uiElement = ((UIElement)element);
            uiElement.OnMouseLeave(device, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseMove"/> method.
        /// </summary>
        private static void OnPreviewMouseMoveProxy(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseMove(device, x, y, dx, dy, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseDown"/> method.
        /// </summary>
        private static void OnPreviewMouseDownProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseDown(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseUp"/> method.
        /// </summary>
        private static void OnPreviewMouseUpProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseUp(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseClick"/> method.
        /// </summary>
        private static void OnPreviewMouseClickProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseClick(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseDoubleClick"/> method.
        /// </summary>
        private static void OnPreviewMouseDoubleClickProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseDoubleClick(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewMouseWheel"/> method.
        /// </summary>
        private static void OnPreviewMouseWheelProxy(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMouseWheel(device, x, y, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseMove"/> method.
        /// </summary>
        private static void OnMouseMoveProxy(DependencyObject element, MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            ((UIElement)element).OnMouseMove(device, x, y, dx, dy, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDown"/> method.
        /// </summary>
        private static void OnMouseDownProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnMouseDown(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseUp"/> method.
        /// </summary>
        private static void OnMouseUpProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            ((UIElement)element).OnMouseUp(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseClick"/> method.
        /// </summary>
        private static void OnMouseClickProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (!data.Handled)
                CommandManager.HandleMouseClickTranslation(element, device, button, data);

            if (!data.Handled)
                ((UIElement)element).OnMouseClick(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseDoubleClick"/> method.
        /// </summary>
        private static void OnMouseDoubleClickProxy(DependencyObject element, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (!data.Handled)
                CommandManager.HandleMouseDoubleClickTranslation(element, device, button, data);

            if (!data.Handled)
                ((UIElement)element).OnMouseDoubleClick(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnMouseWheel"/> method.
        /// </summary>
        private static void OnMouseWheelProxy(DependencyObject element, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            if (!data.Handled)
                CommandManager.HandleMouseWheelTranslation(element, device, x, y, data);

            if (!data.Handled)
                ((UIElement)element).OnMouseWheel(device, x, y, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGamePadAxisChanged"/> method.
        /// </summary>
        private static void OnPreviewGamePadAxisChangedProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGamePadAxisChanged(device, axis, value, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGamePadAxisDown"/> method.
        /// </summary>
        private static void OnPreviewGamePadAxisDownProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGamePadAxisDown(device, axis, value, repeat, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGamePadAxisUp"/> method.
        /// </summary>
        private static void OnPreviewGamePadAxisUpProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGamePadAxisUp(device, axis, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGamePadButtonDown"/> method.
        /// </summary>
        private static void OnPreviewGamePadButtonDownProxy(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (!data.Handled)
                CommandManager.HandleGamePadButtonDownTranslation(element, device, button, repeat, data);

            if (!data.Handled)
                ((UIElement)element).OnPreviewGamePadButtonDown(device, button, repeat, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnPreviewGamePadButtonUp"/> method.
        /// </summary>
        private static void OnPreviewGamePadButtonUpProxy(DependencyObject element, GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewGamePadButtonUp(device, button, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisChanged"/> method.
        /// </summary>
        private static void OnGamePadAxisChangedProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisChanged(device, axis, value, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisDown"/> method.
        /// </summary>
        private static void OnGamePadAxisDownProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, Single value, Boolean repeat, RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisDown(device, axis, value, repeat, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadAxisUp"/> method.
        /// </summary>
        private static void OnGamePadAxisUpProxy(DependencyObject element, GamePadDevice device, GamePadAxis axis, RoutedEventData data)
        {
            ((UIElement)element).OnGamePadAxisUp(device, axis, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadButtonDown"/> method.
        /// </summary>
        private static void OnGamePadButtonDownProxy(DependencyObject element, GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            ((UIElement)element).OnGamePadButtonDown(device, button, repeat, data);
        }

        /// <summary>
        /// Invokes the <see cref="OnGamePadButtonUp"/> method.
        /// </summary>
        private static void OnGamePadButtonUpProxy(DependencyObject element, GamePadDevice device, GamePadButton button, RoutedEventData data)
        {
            ((UIElement)element).OnGamePadButtonUp(device, button, data);
        }
        
        /// <summary>
        /// Invokes the <see cref="Touch.GotTouchCaptureEvent"/> attached routed event.
        /// </summary>
        private static void OnGotTouchCaptureProxy(DependencyObject element, TouchDevice device, Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnGotTouchCapture(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.GotNewTouchCaptureEvent"/> attached routed event.
        /// </summary>
        private static void OnGotNewTouchCaptureProxy(DependencyObject element, RoutedEventData data)
        {
            ((UIElement)element).OnGotNewTouchCapture(data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.LostTouchCaptureEvent"/> attached routed event.
        /// </summary>
        private static void OnLostTouchCaptureProxy(DependencyObject element, TouchDevice device, Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnLostTouchCapture(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.LostNewTouchCaptureEvent"/> attached routed event.
        /// </summary>
        private static void OnLostNewTouchCaptureProxy(DependencyObject element, RoutedEventData data)
        {
            ((UIElement)element).OnLostNewTouchCapture(data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchEnterEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchEnterProxy(DependencyObject element, TouchDevice device, Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnTouchEnter(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchLeaveEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchLeaveProxy(DependencyObject element, TouchDevice device, Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnTouchLeave(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewTouchMoveEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewTouchMoveProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTouchMove(device, id, x, y, dx, dy, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewTouchDownEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewTouchDownProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTouchDown(device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewTouchUpEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewTouchUpProxy(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTouchUp(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewTouchTapEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewTouchTapProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTouchTap(device, id, x, y, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewTouchLongPressEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewTouchLongPressProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewTouchLongPress(device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.PreviewMultiGestureEvent"/> attached routed event.
        /// </summary>
        private static void OnPreviewMultiGestureProxy(DependencyObject element, TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
        {
            ((UIElement)element).OnPreviewMultiGesture(device, x, y, theta, distance, fingers, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchMoveEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchMoveProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnTouchMove(device, id, x, y, dx, dy, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchDownEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchDownProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnTouchDown(device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchUpEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchUpProxy(DependencyObject element, TouchDevice device,
            Int64 id, RoutedEventData data)
        {
            ((UIElement)element).OnTouchUp(device, id, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchTapEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchTapProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, RoutedEventData data)
        {
            ((UIElement)element).OnTouchTap(device, id, x, y, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.TouchLongPressEvent"/> attached routed event.
        /// </summary>
        private static void OnTouchLongPressProxy(DependencyObject element, TouchDevice device,
            Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            ((UIElement)element).OnTouchLongPress(device, id, x, y, pressure, data);
        }

        /// <summary>
        /// Invokes the <see cref="Touch.MultiGestureEvent"/> attached routed event.
        /// </summary>
        private static void OnMultiGestureProxy(DependencyObject element, TouchDevice device,
            Double x, Double y, Single theta, Single distance, Int32 fingers, RoutedEventData data)
        {
            ((UIElement)element).OnMultiGesture(device, x, y, theta, distance, fingers, data);
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
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsCaptured();
            uiElement.OnIsMouseCapturedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseCaptureWithin"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseCaptureWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsCapturedWithin();
            uiElement.OnIsMouseCaptureWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseOver"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsOver();
            uiElement.OnIsMouseOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsMouseDirectlyOver"/> dependency property changes.
        /// </summary>
        private static void HandleIsMouseDirectlyOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsDirectlyOver();
            uiElement.OnIsMouseDirectlyOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesCaptured"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyTouchesCapturedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsCaptured();
            uiElement.OnAreAnyTouchesCapturedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesCapturedWithin"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyTouchesCapturedWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsCapturedWithin();
            uiElement.OnAreAnyTouchesCapturedWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreNewTouchesCaptured"/> dependency property changes.
        /// </summary>
        private static void HandleAreNewTouchesCapturedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.OnAreNewTouchesCapturedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreNewTouchesCapturedWithin"/> dependency property changes.
        /// </summary>
        private static void HandleAreNewTouchesCapturedWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.OnAreNewTouchesCapturedWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesOver"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyTouchesOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsOver();
            uiElement.OnAreAnyTouchesOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyTouchesDirectlyOver"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyTouchesDirectlyOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var uiElement = (UIElement)dobj;
            uiElement.UpdateAreAnyCursorsDirectlyOver();
            uiElement.OnAreAnyTouchesDirectlyOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsCaptured"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyCursorsCapturedChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnAreAnyCursorsCapturedChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsCapturedWithin"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyCursorsCapturedWithinChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnAreAnyCursorsCapturedWithinChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsOver"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyCursorsOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnAreAnyCursorsOverChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AreAnyCursorsDirectlyOver"/> dependency property changes.
        /// </summary>
        private static void HandleAreAnyCursorsDirectlyOverChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            ((UIElement)dobj).OnAreAnyCursorsDirectlyOverChanged();
        }

        /// <summary>
        /// Updates the value of the <see cref="AreAnyCursorsCaptured"/> dependency property.
        /// </summary>
        private void UpdateAreAnyCursorsCaptured()
        {
            var value = IsMouseCaptured || AreAnyTouchesCaptured;
            SetValue(AreAnyCursorsCapturedPropertyKey, value);
        }

        /// <summary>
        /// Updates the value of the <see cref="AreAnyCursorsCapturedWithin"/> dependency property.
        /// </summary>
        private void UpdateAreAnyCursorsCapturedWithin()
        {
            var value = IsMouseCaptureWithin || AreAnyTouchesCapturedWithin;
            SetValue(AreAnyCursorsCapturedWithinPropertyKey, value);
        }

        /// <summary>
        /// Updates the value of the <see cref="AreAnyCursorsOver"/> dependency property.
        /// </summary>
        private void UpdateAreAnyCursorsOver()
        {
            var value = IsMouseOver || AreAnyTouchesOver;
            SetValue(AreAnyCursorsOverPropertyKey, value);
        }

        /// <summary>
        /// Updates the value of the <see cref="AreAnyCursorsDirectlyOver"/> dependency property.
        /// </summary>
        private void UpdateAreAnyCursorsDirectlyOver()
        {
            var value = IsMouseDirectlyOver || AreAnyTouchesDirectlyOver;
            SetValue(AreAnyCursorsDirectlyOverPropertyKey, value);
        }
    }
}
