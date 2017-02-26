using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents a mouse gesture.
    /// </summary>
    public sealed class MouseGesture : InputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseGesture"/> class.
        /// </summary>
        public MouseGesture()
            : this(MouseAction.None, ModifierKeys.None)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseGesture"/> class.
        /// </summary>
        /// <param name="mouseAction">The mouse action associated with this mouse gesture.</param>
        public MouseGesture(MouseAction mouseAction)
            : this (mouseAction, ModifierKeys.None)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseGesture"/> class.
        /// </summary>
        /// <param name="mouseAction">The mouse action associated with this mouse gesture.</param>
        /// <param name="modifiers">The set of modifier keys associated with this mouse gesture.</param>
        public MouseGesture(MouseAction mouseAction, ModifierKeys modifiers)
        {
            this.MouseAction = mouseAction;
            this.Modifiers = modifiers;
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseClick(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.LeftClick:
                    return button == MouseButton.Left && Keyboard.Modifiers == Modifiers;

                case MouseAction.RightClick:
                    return button == MouseButton.Right && Keyboard.Modifiers == Modifiers;

                case MouseAction.MiddleClick:
                    return button == MouseButton.Middle && Keyboard.Modifiers == Modifiers;
            }
            return base.MatchesMouseClick(targetElement, device, button, data);
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseDoubleClick(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.LeftDoubleClick:
                    return button == MouseButton.Left && Keyboard.Modifiers == Modifiers;

                case MouseAction.RightDoubleClick:
                    return button == MouseButton.Right && Keyboard.Modifiers == Modifiers;

                case MouseAction.MiddleDoubleClick:
                    return button == MouseButton.Middle && Keyboard.Modifiers == Modifiers;
            }
            return base.MatchesMouseDoubleClick(targetElement, device, button, data);
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseWheel(Object targetElement, MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.WheelClick:
                    return Keyboard.Modifiers == Modifiers;
            }
            return base.MatchesMouseWheel(targetElement, device, x, y, data);
        }

        /// <summary>
        /// Gets the mouse action associated with this mouse gesture.
        /// </summary>
        public MouseAction MouseAction { get; private set; }

        /// <summary>
        /// Gets the set of modifier keys associated with this mouse gesture.
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }
    }
}
