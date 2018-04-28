using System;
using System.Text;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents a keyboard input binding.
    /// </summary>
    public sealed class KeyboardInputBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="element">The XML element that contains the binding data.</param>
        internal KeyboardInputBinding(UltravioletContext uv, XElement element)
        {
            Contract.Require(element, nameof(element));

            this.keyboard = uv.GetInput().GetKeyboard();

            this.key = element.ElementValueEnum<Key>("Key") ?? Key.None;
            this.control = element.ElementValueBoolean("Control") ?? false;
            this.alt = element.ElementValueBoolean("Alt") ?? false;
            this.shift = element.ElementValueBoolean("Shift") ?? false;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="key">A <see cref="Key"/> value representing the binding's primary key.</param>
        public KeyboardInputBinding(UltravioletContext uv, Key key)
        {
            Contract.Require(uv, nameof(uv));

            if (!uv.GetInput().IsKeyboardSupported())
            {
                throw new NotSupportedException();
            }

            this.keyboard = uv.GetInput().GetKeyboard();
            this.key = key;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="key">A <see cref="Key"/> value representing the binding's primary key.</param>
        /// <param name="control">A value indicating whether the binding requires the Control modifier.</param>
        /// <param name="alt">A value indicating whether the binding requires the Alt modifier.</param>
        /// <param name="shift">A value indicating whether the binding requires the Shift modifier.</param>
        public KeyboardInputBinding(UltravioletContext uv, Key key, Boolean control, Boolean alt, Boolean shift)
        {
            if (!uv.GetInput().IsKeyboardSupported())
            {
                throw new NotSupportedException();
            }

            this.keyboard = uv.GetInput().GetKeyboard();
            this.key = key;
            this.control = control;
            this.alt = alt;
            this.shift = shift;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return stringRepresentation;
        }

        /// <inheritdoc/>
        public override void Update()
        {
            released = false;
            if (pressed)
            {
                if (!Enabled || keyboard.IsKeyReleased(key) || !AreModifiersSatisfied())
                {
                    pressed = false;
                    released = true;
                    OnReleased();
                }
            }
            else
            {
                if (Enabled && keyboard.IsKeyPressed(key) && AreModifiersSatisfied())
                {
                    pressed = true;
                    OnPressed();
                }
            }
        }

        /// <inheritdoc/>
        public override Boolean UsesSameButtons(InputBinding binding)
        {
            if (ReferenceEquals(binding, null)) return false;
            if (ReferenceEquals(binding, this)) return true;

            var kbib = binding as KeyboardInputBinding;
            if (kbib != null)
            {
                return
                    this.Keyboard == kbib.Keyboard &&
                    this.Key == kbib.Key &&
                    this.IsControlRequired == kbib.IsControlRequired &&
                    this.IsAltRequired == kbib.IsAltRequired &&
                    this.IsShiftRequired == kbib.IsShiftRequired;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean UsesSamePrimaryButtons(InputBinding binding)
        {
            if (ReferenceEquals(binding, null)) return false;
            if (ReferenceEquals(binding, this)) return true;

            var kbib = binding as KeyboardInputBinding;
            if (kbib != null)
            {
                return 
                    this.Keyboard == kbib.Keyboard &&
                    this.Key == kbib.Key;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean IsDown()
        {
            return pressed;
        }

        /// <inheritdoc/>
        public override Boolean IsUp()
        {
            return !pressed;
        }

        /// <inheritdoc/>
        public override Boolean IsPressed(Boolean ignoreRepeats = true)
        {
            return pressed && keyboard.IsKeyPressed(key, ignoreRepeats: ignoreRepeats);
        }

        /// <inheritdoc/>
        public override Boolean IsReleased()
        {
            return released;
        }

        /// <summary>
        /// Gets the <see cref="KeyboardDevice"/> that created this input binding.
        /// </summary>
        public KeyboardDevice Keyboard
        {
            get { return keyboard; }
        }

        /// <summary>
        /// Gets the <see cref="Key"/> value that represents the binding's primary key.
        /// </summary>
        public Key Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets a value indicating whether this binding requires the Control modifier.
        /// </summary>
        public Boolean IsControlRequired
        {
            get { return control; }
        }

        /// <summary>
        /// Gets a value indicating whether this binding requires the Alt modifier.
        /// </summary>
        public Boolean IsAltRequired
        {
            get { return alt; }
        }

        /// <summary>
        /// Gets a value indicating whether this binding requires the Shift modifier.
        /// </summary>
        public Boolean IsShiftRequired
        {
            get { return shift; }
        }

        /// <inheritdoc/>
        internal override XElement ToXml(String name = null)
        {
            return new XElement(name ?? "Binding", new XAttribute("Type", GetType().FullName),
                new XElement("Key", key),
                new XElement("Control", control),
                new XElement("Alt", alt),
                new XElement("Shift", shift)
            );
        }

        /// <inheritdoc/>
        protected override Int32 CalculatePriority()
        {
            return
                (control ? 1 : 0) +
                (alt ? 1 : 0) + 
                (shift ? 1 : 0);
        }

        /// <summary>
        /// Gets a value indicating whether the binding's modifier states are satisfied.
        /// </summary>
        /// <returns><see langword="true"/> if the binding's modifier states are satisfied; otherwise, <see langword="false"/>.</returns>
        private Boolean AreModifiersSatisfied()
        {
            return
                (!control || keyboard.IsControlDown) &&
                (!alt     || keyboard.IsAltDown) &&
                (!shift   || keyboard.IsShiftDown);

        }

        /// <summary>
        /// Appends a separator to the specified string builder if the builder already contains text.
        /// </summary>
        private Boolean AppendSeparatorIfNecessary(StringBuilder builder, String separator)
        {
            if (builder.Length == 0)
                return false;

            builder.Append(separator);
            return true;
        }

        /// <summary>
        /// Builds a string representation of the key binding.
        /// </summary>
        private String BuildStringRepresentation()
        {
            var separator = Localization.Get("INPUT_BINDING_SEPARATOR");
            var builder = new StringBuilder();

            if (IsControlRequired)
            {
                AppendSeparatorIfNecessary(builder, separator);
                builder.Append(Localization.Get("KEY_MODIFIER_CONTROL"));
            }

            if (IsAltRequired)
            {
                AppendSeparatorIfNecessary(builder, separator);
                builder.Append(Localization.Get("KEY_MODIFIER_ALT"));
            }

            if (IsShiftRequired)
            {
                AppendSeparatorIfNecessary(builder, separator);
                builder.Append(Localization.Get("KEY_MODIFIER_SHIFT"));
            }

            AppendSeparatorIfNecessary(builder, separator);
            builder.Append(Localization.Get("KEY_" + Key));

            return builder.ToString();
        }

        // Property values.
        private readonly KeyboardDevice keyboard;
        private readonly Key key;
        private readonly Boolean control;
        private readonly Boolean alt;
        private readonly Boolean shift;
        private readonly String stringRepresentation;

        // State values.
        private Boolean pressed;
        private Boolean released;
    }
}
