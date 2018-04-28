using System;
using System.Globalization;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents a keyboard gesture.
    /// </summary>
    public sealed class KeyGesture : InputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class.
        /// </summary>
        /// <param name="key">The key associated with this keyboard gesture.</param>
        public KeyGesture(Key key)
            : this(key, ModifierKeys.None, String.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class.
        /// </summary>
        /// <param name="key">The key associated with this keyboard gesture.</param>
        /// <param name="modifiers">The set of modifier keys associated with this keyboard gesture.</param>
        public KeyGesture(Key key, ModifierKeys modifiers)
            : this(key, modifiers, String.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGesture"/> class.
        /// </summary>
        /// <param name="key">The key associated with this keyboard gesture.</param>
        /// <param name="modifiers">The set of modifier keys associated with this keyboard gesture.</param>
        /// <param name="displayString">The display string for this keyboard gesture.</param>
        public KeyGesture(Key key, ModifierKeys modifiers, String displayString)
        {
            Contract.Require(displayString, nameof(displayString));

            this.Key = key;
            this.Modifiers = modifiers;
            this.DisplayString = displayString;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="KeyGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, out KeyGesture gesture)
        {
            return TryParse(str, null, out gesture);
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="KeyGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <returns>A instance of the <see cref="KeyGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static KeyGesture Parse(String str)
        {
            KeyGesture gesture;
            if (!TryParse(str, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="KeyGesture"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="gesture">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryParse(String str, IFormatProvider provider, out KeyGesture gesture)
        {
            Contract.Require(str, nameof(str));

            gesture = null;

            if (String.IsNullOrWhiteSpace(str))
                return false;

            var key = Key.None;
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
                    if (!Enum.TryParse(parts[i], true, out key))
                        return false;
                }
            }

            gesture = new KeyGesture(key, modifiers);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a gesture into an instance of the <see cref="KeyGesture"/> structure.
        /// </summary>
        /// <param name="str">A string containing a gesture to convert.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="KeyGesture"/> structure equivalent to the gesture contained in <paramref name="str"/>.</returns>
        public static KeyGesture Parse(String str, IFormatProvider provider)
        {
            KeyGesture gesture;
            if (!TryParse(str, provider, out gesture))
            {
                throw new FormatException();
            }
            return gesture;
        }

        /// <inheritdoc/>
        public override Boolean MatchesKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            return key == Key && modifiers.IsEquivalentTo(Modifiers);
        }

        /// <summary>
        /// Gets a string that represents the gesture. If the gesture has a valid value for 
        /// the <see cref="DisplayString"/> property, that value is returned; otherwise, a string
        /// is generated for the specified culture based on the <see cref="Key"/> and <see cref="Modifiers"/> values.
        /// </summary>
        /// <param name="culture">The culture for which to retrieve a display string.</param>
        /// <returns>The display string for the gesture in the context of the specified culture.</returns>
        public String GetDisplayStringForCulture(CultureInfo culture)
        {
            if (!String.IsNullOrEmpty(DisplayString))
                return DisplayString;

            return GenerateCanonicalDisplayString(culture, Key, Modifiers);
        }

        /// <summary>
        /// Gets the key associated with this keyboard gesture.
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// Gets the set of modifier keys associated with this keyboard gesture.
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }

        /// <summary>
        /// Gets the string representation of this keyboard gesture.
        /// </summary>
        public String DisplayString { get; private set; }

        /// <summary>
        /// Generates a canonical display string for the specified key/modifier combination.
        /// </summary>
        private static String GenerateCanonicalDisplayString(CultureInfo culture, Key key, ModifierKeys modifiers)
        {
            var strCtrl = (modifiers & ModifierKeys.Control) != 0 ? 
                Localization.Get(culture?.Name ?? Localization.CurrentCulture, "KEY_MODIFIER_CONTROL") : String.Empty;
            var strAlt = (modifiers & ModifierKeys.Alt) != 0 ? 
                Localization.Get(culture?.Name ?? Localization.CurrentCulture, "KEY_MODIFIER_ALT") : String.Empty;
            var strShift = (modifiers & ModifierKeys.Shift) != 0 ? 
                Localization.Get(culture?.Name ?? Localization.CurrentCulture, "KEY_MODIFIER_SHIFT") : String.Empty;
            var strKey = Localization.Get(culture?.Name ?? Localization.CurrentCulture, "KEY_" + key.ToString());

            return String.Join("+", new[] { strCtrl, strAlt, strShift, strKey }.Where(x => !String.IsNullOrEmpty(x)));
        }
    }
}
