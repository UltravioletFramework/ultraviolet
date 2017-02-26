using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Represents an input device gesture.
    /// </summary>
    public abstract class InputGesture
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the gesture matches the specified key down input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesKeyDown(Object targetElement, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data) => false;

        /// <summary>
        /// When overridden in a derived class, determines whether the gesture matches the specified key up input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesKeyUp(Object targetElement, KeyboardDevice device, Key key, RoutedEventData data) => false;
        
        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse down input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseDown(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse up input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseUp(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse click input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseClick(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse double click input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseDoubleClick(Object targetElement, MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse wheel input event.
        /// </summary>
        /// <param name="targetElement">The target of the command.</param>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseWheel(Object targetElement, MouseDevice device, Double x, Double y, RoutedEventData data) => false;

        /// <summary>
        /// Gets the <see cref="ModifierKeys"/> value that corresponds to the specified string.
        /// </summary>
        /// <param name="str">The string to evaluate.</param>
        /// <returns>The <see cref="ModifierKeys"/> value that corresponds to the specified string, or <see langword="null"/>
        /// if the string does not correspond to any value.</returns>
        protected internal static ModifierKeys? GetModifierKeyFromString(String str)
        {
            switch (str?.ToLowerInvariant())
            {
                case "ctrl":
                    return ModifierKeys.Control;

                case "alt":
                    return ModifierKeys.Alt;

                case "shift":
                    return ModifierKeys.Shift;
            }

            return null;
        }
    }
}
