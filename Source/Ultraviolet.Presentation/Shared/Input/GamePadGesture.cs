using System;
using System.Globalization;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a game pad gesture.
    /// </summary>
    public sealed class GamePadGesture : InputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadGesture"/> class.
        /// </summary>
        /// <param name="button">The button associated with this game pad gesture.</param>
        public GamePadGesture(GamePadButton button)
            : this(button, AnyPlayerIndex, String.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadGesture"/> class.
        /// </summary>
        /// <param name="playerIndex">The player index of the game pad for this gesture.</param>
        /// <param name="button">The button associated with this game pad gesture.</param>
        public GamePadGesture(GamePadButton button, Int32 playerIndex)
            : this(button, playerIndex, String.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadGesture"/> class.
        /// </summary>
        /// <param name="button">The button associated with this game pad gesture.</param>
        /// <param name="displayString">The display string for this keyboard gesture.</param>
        public GamePadGesture(GamePadButton button, String displayString)
            : this(button, AnyPlayerIndex, displayString)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadGesture"/> class.
        /// </summary>
        /// <param name="button">The button associated with this game pad gesture.</param>
        /// <param name="playerIndex">The player index of the game pad for this gesture.</param>
        /// <param name="displayString">The display string for this keyboard gesture.</param>
        public GamePadGesture(GamePadButton button, Int32 playerIndex, String displayString)
        {
            Contract.EnsureRange(playerIndex >= AnyPlayerIndex, nameof(playerIndex));
            Contract.Require(displayString, nameof(displayString));

            this.Button = button;
            this.PlayerIndex = playerIndex;
            this.DisplayString = displayString;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="GamePadGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, out GamePadGesture gesture)
        {
            return TryParse(str, null, out gesture);
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="GamePadGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <returns>A instance of the <see cref="GamePadGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static GamePadGesture Parse(String str)
        {
            GamePadGesture gesture;
            if (!TryParse(str, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="GamePadGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, IFormatProvider provider, out GamePadGesture gesture)
        {
            Contract.Require(str, nameof(str));

            gesture = null;

            if (String.IsNullOrWhiteSpace(str))
                return false;

            var parts = str.Split(':');
            var partIndex = parts.Length == 1 ? null : parts[0].Trim();
            var partEnum = parts.Length == 1 ? parts[0].Trim() : parts[1].Trim();
            
            var index = AnyPlayerIndex;
            if (partIndex != null)
            {
                if (partIndex.StartsWith("P", StringComparison.OrdinalIgnoreCase))
                {
                    if (!Int32.TryParse(partIndex.Substring(1), out index) || index < 0)
                        return false;
                }
                else if (!String.Equals("ANY", partIndex, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            
            var button = GamePadButton.None;
            if (!Enum.TryParse(partEnum, true, out button))
                return false;

            gesture = new GamePadGesture(button, index);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="GamePadGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="GamePadGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static GamePadGesture Parse(String str, IFormatProvider provider)
        {
            GamePadGesture gesture;
            if (!TryParse(str, provider, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <inheritdoc/>
        public override Boolean MatchesGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            if (PlayerIndex == AnyPlayerIndex || PlayerIndex == device.PlayerIndex)
            {
                return Button == button;
            }
            return base.MatchesGamePadButtonDown(device, button, repeat, data);
        }

        /// <summary>
        /// Gets a string that represents the gesture. If the gesture has a valid value for 
        /// the <see cref="DisplayString"/> property, that value is returned; otherwise, a string
        /// is generated for the specified culture based on the <see cref="GamePadButton"/> value.
        /// </summary>
        /// <param name="culture">The culture for which to retrieve a display string.</param>
        /// <returns>The display string for the gesture in the context of the specified culture.</returns>
        public String GetDisplayStringForCulture(CultureInfo culture)
        {
            if (!String.IsNullOrEmpty(DisplayString))
                return DisplayString;

            return GenerateCanonicalDisplayString(culture, Button);
        }

        /// <summary>
        /// Gets the game pad button associated with this game pad gesture.
        /// </summary>
        public GamePadButton Button { get; private set; }
        
        /// <summary>
        /// Gets the player index of the game pad which is associated with this gesture, or -1 if
        /// any player can perform this gesture.
        /// </summary>
        public Int32 PlayerIndex { get; private set; }

        /// <summary>
        /// Gets the string representation of this game pad gesture.
        /// </summary>
        public String DisplayString { get; private set; }

        /// <summary>
        /// The player index which indicates that a gesture can be performed by any player.
        /// </summary>
        public const Int32 AnyPlayerIndex = -1;

        /// <summary>
        /// Generates a canonical display string for the specified key/modifier combination.
        /// </summary>
        private static String GenerateCanonicalDisplayString(CultureInfo culture, GamePadButton button)
        {
            return Localization.Get(culture?.Name ?? Localization.CurrentCulture, "GAME_PAD_BUTTON_" + button.ToString());
        }
    }
}
