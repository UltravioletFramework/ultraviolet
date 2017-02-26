using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
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

        /// <inheritdoc/>
        public override Boolean MatchesKeyDown(Object targetElement, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            return key == Key && modifiers == Modifiers;
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
    }
}
