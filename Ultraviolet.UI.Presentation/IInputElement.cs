using System;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an element capable of basic input handling.
    /// </summary>
    public interface IInputElement
    {
        /// <summary>
        /// Adds a handler for a routed event to the element.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to add a handler.</param>
        /// <param name="handler">A delegate that represents the handler to add to the element for the specified routed event.</param>
        void AddHandler(RoutedEvent evt, Delegate handler);

        /// <summary>
        /// Removes a handler for a routed event from the element.
        /// </summary>
        /// <param name="evt">A <see cref="RoutedEvent"/> that identifies the routed event for which to remove a handler.</param>
        /// <param name="handler">A delegate that represents the handler to remove from the element for the specified routed event.</param>
        void RemoveHandler(RoutedEvent evt, Delegate handler);

        /// <summary>
        /// Sets focus on this element.
        /// </summary>
        /// <returns><see langword="true"/> if focus was successfully moved to this element; otherwise, <see langword="false"/>.</returns>
        Boolean Focus();

        /// <summary>
        /// Gets or sets a value that indicates whether this element can receive focus.
        /// </summary>
        Boolean Focusable { get; set; }

        /// <summary>
        /// Gets a value indicating whether this element is enabled.
        /// </summary>
        Boolean IsEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this element has keyboard focus.
        /// </summary>
        Boolean IsKeyboardFocused { get; }

        /// <summary>
        /// Gets a value indicating whether this element or any of its descendants have keyboard focus.
        /// </summary>
        Boolean IsKeyboardFocusWithin { get; }

        /// <summary>
        /// Gets a value indicating whether this element has captured the mouse.
        /// </summary>
        Boolean IsMouseCaptured { get; }

        /// <summary>
        /// Gets a value indicating whether this element or any of its descendants have mouse capture.
        /// </summary>
        Boolean IsMouseCaptureWithin { get; }

        /// <summary>
        /// Gets a value indicating whether the cursor is located over this element or any of its descendants.
        /// </summary>
        Boolean IsMouseOver { get; }

        /// <summary>
        /// Gets a value indicating whether this element is the topmost element which is directly under the mouse cursor.
        /// </summary>
        Boolean IsMouseDirectlyOver { get; }

        /// <summary>
        /// Gets a value indicating whether at least one touch is captured by this element.
        /// </summary>
        Boolean AreAnyTouchesCaptured { get; }

        /// <summary>
        /// Gets a value indicating whether at least one touch is captured by this element or any of its descendants.
        /// </summary>
        Boolean AreAnyTouchesCapturedWithin { get; }

        /// <summary>
        /// Gets a value indicating whether new touches are captured by this element.
        /// </summary>
        Boolean AreNewTouchesCaptured { get; }

        /// <summary>
        /// Gets a value indicating whether new touches are captured by this element or any of its descendants.
        /// </summary>
        Boolean AreNewTouchesCapturedWithin { get; }

        /// <summary>
        /// Gets a value indicating whether at least one touch is over this element or any of its descendants.
        /// </summary>
        Boolean AreAnyTouchesOver { get; }

        /// <summary>
        /// Gets a value indicating whether at least one touch is directly over this element.
        /// </summary>
        Boolean AreAnyTouchesDirectlyOver { get; }

        /// <summary>
        /// Gets a value indicating whether at least one cursor (mouse or touch) is captured by this element.
        /// </summary>
        Boolean AreAnyCursorsCaptured { get; }

        /// <summary>
        /// Gets a value indicating whether at least one cursor (mouse or touch) is captured by this element or any of its descendants.
        /// </summary>
        Boolean AreAnyCursorsCapturedWithin { get; }

        /// <summary>
        /// Gets a value indicating whether at least one cursor (mouse or touch) is over this element or any of its descendants.
        /// </summary>
        Boolean AreAnyCursorsOver { get; }

        /// <summary>
        /// Gets a value indicating whether at least one cursor (mouse or touch) is directly over this element.
        /// </summary>
        Boolean AreAnyCursorsDirectlyOver { get; }

        /// <summary>
        /// Gets a collection of touches which are captured by this element.
        /// </summary>
        TouchesCollection TouchesCaptured { get; }

        /// <summary>
        /// Gets a collection of touches which are captured by this element or any of its descendants.
        /// </summary>
        TouchesCollection TouchesCapturedWithin { get; }

        /// <summary>
        /// Gets a collection of touches which are over this element or any of its descendants.
        /// </summary>
        TouchesCollection TouchesOver { get; }

        /// <summary>
        /// Gets a collection of touches which are directly over this element.
        /// </summary>
        TouchesCollection TouchesDirectlyOver { get; }

        /// <summary>
        /// Occurs when the element gains keyboard focus.
        /// </summary>
        event UpfRoutedEventHandler PreviewGotKeyboardFocus;

        /// <summary>
        /// Occurs when the element loses keyboard focus.
        /// </summary>
        event UpfRoutedEventHandler PreviewLostKeyboardFocus;

        /// <summary>
        /// Occurs when the element gains keyboard focus.
        /// </summary>
        event UpfRoutedEventHandler GotKeyboardFocus;

        /// <summary>
        /// Occurs when the element loses keyboard focus.
        /// </summary>
        event UpfRoutedEventHandler LostKeyboardFocus;

        /// <summary>
        /// Occurs when the element receives text input from the keyboard.
        /// </summary>
        event UpfKeyboardEventHandler PreviewTextInput;

        /// <summary>
        /// Occurs when the element receives text editing information from the keyboard.
        /// </summary>
        event UpfKeyboardEventHandler PreviewTextEditing;

        /// <summary>
        /// Occurs when a key is pressed while the element has focus.
        /// </summary>
        event UpfKeyDownEventHandler PreviewKeyDown;

        /// <summary>
        /// Occurs when a key is released while the element has focus.
        /// </summary>
        event UpfKeyEventHandler PreviewKeyUp;

        /// <summary>
        /// Occurs when the element receives text input from the keyboard.
        /// </summary>
        event UpfKeyboardEventHandler TextInput;

        /// <summary>
        /// Occurs when the element receives text editing information from the keyboard.
        /// </summary>
        event UpfKeyboardEventHandler TextEditing;

        /// <summary>
        /// Occurs when a key is pressed while the element has focus.
        /// </summary>
        event UpfKeyDownEventHandler KeyDown;

        /// <summary>
        /// Occurs when a key is released while the element has focus.
        /// </summary>
        event UpfKeyEventHandler KeyUp;

        /// <summary>
        /// Occurs when the element gains mouse capture.
        /// </summary>
        event UpfRoutedEventHandler GotMouseCapture;

        /// <summary>
        /// Occurs when the element loses mouse capture.
        /// </summary>
        event UpfRoutedEventHandler LostMouseCapture;

        /// <summary>
        /// Occurs when the mouse cursor moves over the element.
        /// </summary>
        event UpfMouseMoveEventHandler PreviewMouseMove;

        /// <summary>
        /// Occurs when a mouse button is pressed while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler PreviewMouseDown;

        /// <summary>
        /// Occurs when a mouse button is released while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler PreviewMouseUp;

        /// <summary>
        /// Occurs when a mouse button is clicked while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler PreviewMouseClick;

        /// <summary>
        /// Occurs when a mouse button is double clicked while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler PreviewMouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse wheel is scrolled while the cursor is over the element.
        /// </summary>
        event UpfMouseWheelEventHandler PreviewMouseWheel;

        /// <summary>
        /// Occurs when the mouse cursor enters the element.
        /// </summary>
        event UpfMouseEventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse cursor leaves the element.
        /// </summary>
        event UpfMouseEventHandler MouseLeave;

        /// <summary>
        /// Occurs when the mouse cursor moves over the element.
        /// </summary>
        event UpfMouseMoveEventHandler MouseMove;

        /// <summary>
        /// Occurs when a mouse button is pressed while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler MouseDown;

        /// <summary>
        /// Occurs when a mouse button is released while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler MouseUp;

        /// <summary>
        /// Occurs when a mouse button is clicked while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler MouseClick;

        /// <summary>
        /// Occurs when a mouse button is double clicked while the cursor is over the element.
        /// </summary>
        event UpfMouseButtonEventHandler MouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse wheel is scrolled while the cursor is over the element.
        /// </summary>
        event UpfMouseWheelEventHandler MouseWheel;

        /// <summary>
        /// Occurs when a game pad axis changes value while the element has focus.
        /// </summary>
        event UpfGamePadAxisChangedEventHandler PreviewGamePadAxisChanged;

        /// <summary>
        /// Occurs when a game pad button is pressed while the element has focus.
        /// </summary>
        event UpfGamePadButtonDownEventHandler PreviewGamePadButtonDown;

        /// <summary>
        /// Occurs when a game pad button is released while the element has focus.
        /// </summary>
        event UpfGamePadButtonUpEventHandler PreviewGamePadButtonUp;

        /// <summary>
        /// Occurs when a game pad axis changes value while the element has focus.
        /// </summary>
        event UpfGamePadAxisChangedEventHandler GamePadAxisChanged;

        /// <summary>
        /// Occurs when a game pad axis is pressed while the element has focus.
        /// </summary>
        event UpfGamePadAxisDownEventHandler GamePadAxisDown;

        /// <summary>
        /// Occurs when a game pad axis is released while the element has focus.
        /// </summary>
        event UpfGamePadAxisUpEventHandler GamePadAxisUp;

        /// <summary>
        /// Occurs when a game pad button is pressed while the element has focus.
        /// </summary>
        event UpfGamePadButtonDownEventHandler GamePadButtonDown;

        /// <summary>
        /// Occurs when a game pad button is released while the element has focus.
        /// </summary>
        event UpfGamePadButtonUpEventHandler GamePadButtonUp;

        /// <summary>
        /// Occurs when the element gains touch capture.
        /// </summary>
        event UpfTouchEventHandler GotTouchCapture;

        /// <summary>
        /// Occurs when the element gains new touch capture.
        /// </summary>
        event UpfTouchEventHandler GotNewTouchCapture;

        /// <summary>
        /// Occurs when the element loses touch capture.
        /// </summary>
        event UpfTouchEventHandler LostTouchCapture;

        /// <summary>
        /// Occurs when the element loses new touch capture.
        /// </summary>
        event UpfTouchEventHandler LostNewTouchCapture;

        /// <summary>
        /// Occurs when a touch moves over the element.
        /// </summary>
        event UpfTouchMoveEventHandler PreviewTouchMove;

        /// <summary>
        /// Occurs when a touch begins over the element.
        /// </summary>
        event UpfTouchDownEventHandler PreviewTouchDown;

        /// <summary>
        /// Occurs when a touch ends over the element.
        /// </summary>
        event UpfTouchUpEventHandler PreviewTouchUp;

        /// <summary>
        /// Occurs when the element is tapped.
        /// </summary>
        event UpfTouchTapEventHandler PreviewTouchTap;

        /// <summary>
        /// Occurs when the element is long pressed.
        /// </summary>
        event UpfTouchLongPressEventHandler PreviewTouchLongPress;

        /// <summary>
        /// Occurs when a multiple-finger gesture is performed over the element.
        /// </summary>
        event UpfMultiGestureEventHandler PreviewMultiGesture;

        /// <summary>
        /// Occurs when a touch enters the element.
        /// </summary>
        event UpfTouchEventHandler TouchEnter;

        /// <summary>
        /// Occurs when a touch leaves the element.
        /// </summary>
        event UpfTouchEventHandler TouchLeave;

        /// <summary>
        /// Occurs when a touch moves over the element.
        /// </summary>
        event UpfTouchMoveEventHandler TouchMove;

        /// <summary>
        /// Occurs when a touch begins over the element.
        /// </summary>
        event UpfTouchDownEventHandler TouchDown;

        /// <summary>
        /// Occurs when a touch ends over the element.
        /// </summary>
        event UpfTouchUpEventHandler TouchUp;

        /// <summary>
        /// Occurs when the element is tapped.
        /// </summary>
        event UpfTouchTapEventHandler TouchTap;

        /// <summary>
        /// Occurs when the element is long pressed.
        /// </summary>
        event UpfTouchLongPressEventHandler TouchLongPress;

        /// <summary>
        /// Occurs when a multiple-finger gesture is performed over the element.
        /// </summary>
        event UpfMultiGestureEventHandler MultiGesture;
    }
}
