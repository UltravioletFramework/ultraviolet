using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
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

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="MouseGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, out MouseGesture gesture)
        {
            return TryParse(str, null, out gesture);
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="MouseGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <returns>A instance of the <see cref="MouseGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static MouseGesture Parse(String str)
        {
            MouseGesture gesture;
            if (!TryParse(str, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="MouseGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, IFormatProvider provider, out MouseGesture gesture)
        {
            Contract.Require(str, nameof(str));

            gesture = null;

            if (String.IsNullOrWhiteSpace(str))
                return false;

            var mouseAction = MouseAction.None;
            var modifiers = ModifierKeys.None;

            var parts = str.Split('+').Select(x => x.Trim()).ToArray();
            for (int i = 0; i < parts.Length; i++)
            {
                var isModifier = (i + 1 < parts.Length);
                if (isModifier)
                {
                    var modFromStr = GetModifierKeyFromString(parts[i]);
                    if (modFromStr == null || (modifiers & modFromStr.GetValueOrDefault()) != 0)
                        return false;

                    modifiers |= modFromStr.GetValueOrDefault();
                }
                else
                {
                    if (!Enum.TryParse(parts[i], true, out mouseAction))
                        return false;
                }
            }

            gesture = new MouseGesture(mouseAction, modifiers);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="MouseGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="MouseGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static MouseGesture Parse(String str, IFormatProvider provider)
        {
            MouseGesture gesture;
            if (!TryParse(str, provider, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.LeftClick:
                    return button == MouseButton.Left && Keyboard.Modifiers.IsEquivalentTo(Modifiers);

                case MouseAction.RightClick:
                    return button == MouseButton.Right && Keyboard.Modifiers.IsEquivalentTo(Modifiers);

                case MouseAction.MiddleClick:
                    return button == MouseButton.Middle && Keyboard.Modifiers.IsEquivalentTo(Modifiers);
            }
            return base.MatchesMouseClick(device, button, data);
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseDoubleClick(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.LeftDoubleClick:
                    return button == MouseButton.Left && Keyboard.Modifiers.IsEquivalentTo(Modifiers);

                case MouseAction.RightDoubleClick:
                    return button == MouseButton.Right && Keyboard.Modifiers.IsEquivalentTo(Modifiers);

                case MouseAction.MiddleDoubleClick:
                    return button == MouseButton.Middle && Keyboard.Modifiers.IsEquivalentTo(Modifiers);
            }
            return base.MatchesMouseDoubleClick(device, button, data);
        }

        /// <inheritdoc/>
        public override Boolean MatchesMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            switch (MouseAction)
            {
                case MouseAction.WheelClick:
                    return Keyboard.Modifiers.IsEquivalentTo(Modifiers);
            }
            return base.MatchesMouseWheel(device, x, y, data);
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
