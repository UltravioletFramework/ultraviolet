using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents an input device gesture.
    /// </summary>
    public abstract class InputGesture
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the gesture matches the specified key down input event.
        /// </summary>
        /// <param name="device">The keyboard device.</param>
        /// <param name="key">The <see cref="Key"/> value that represents the key that was pressed.</param>
        /// <param name="modifiers">A <see cref="ModifierKeys"/> value indicating which of the key modifiers are currently active.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data) => false;
        
        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse click input event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseClick(MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse double click input event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="button">The mouse button that was pressed or released.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified mouse wheel input event.
        /// </summary>
        /// <param name="device">The mouse device.</param>
        /// <param name="x">The amount that the wheel was scrolled along the x-axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the y-axis.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data) => false;

        /// <summary>
        /// When overriden in a derived class, determines whether the gesture matches the specified game pad button down input event.
        /// </summary>
        /// <param name="device">The game pad device.</param>
        /// <param name="button">The game pad button that was pressed.</param>
        /// <param name="repeat">A value indicating whether this is a repeated button press.</param>
        /// <param name="data">The routed event metadata for this event invocation.</param>
        /// <returns><see langword="true"/> if the gesture matches the event; otherwise, <see langword="false"/>.</returns>
        public virtual Boolean MatchesGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data) => false;

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
