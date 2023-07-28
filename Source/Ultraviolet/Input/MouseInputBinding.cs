using System;
using System.Text;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents a mouse input binding.
    /// </summary>
    public sealed class MouseInputBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="element">The XML element that contains the binding data.</param>
        internal MouseInputBinding(UltravioletContext uv, XElement element)
        {
            Contract.Require(element, nameof(element));

            this.mouse = uv.GetInput().GetMouse();

            this.button = element.ElementValueEnum<MouseButton>("Button") ?? MouseButton.None;
            this.control = element.ElementValueBoolean("Control") ?? false;
            this.alt = element.ElementValueBoolean("Alt") ?? false;
            this.shift = element.ElementValueBoolean("Shift") ?? false;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that represents the binding's primary button.</param>
        public MouseInputBinding(UltravioletContext uv, MouseButton button)
        {
            Contract.Require(uv, nameof(uv));

            if (!uv.GetInput().IsMouseSupported())
            {
                throw new NotSupportedException();
            }

            this.mouse = uv.GetInput().GetMouse();
            this.button = button;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that represents the binding's primary button.</param>
        /// <param name="control">A value indicating whether the binding requires the Control modifier.</param>
        /// <param name="alt">A value indicating whether the binding requires the Alt modifier.</param>
        /// <param name="shift">A value indicating whether the binding requires the Shift modifier.</param>
        public MouseInputBinding(UltravioletContext uv, MouseButton button, Boolean control, Boolean alt, Boolean shift)
        {
            if (!uv.GetInput().IsMouseSupported())
            {
                throw new NotSupportedException();
            }

            this.mouse = uv.GetInput().GetMouse();
            this.button = button;
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
                if (!Enabled || mouse.IsButtonReleased(button) || !AreModifiersSatisfied())
                {
                    pressed = false;
                    released = true;
                    OnReleased();
                }
            }
            else
            {
                if (Enabled && mouse.IsButtonPressed(button) && AreModifiersSatisfied())
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

            var mib = binding as MouseInputBinding;
            if (mib != null)
            {
                return
                    this.Mouse == mib.Mouse &&
                    this.Button == mib.Button &&
                    this.IsControlRequired == mib.IsControlRequired &&
                    this.IsAltRequired == mib.IsAltRequired &&
                    this.IsShiftRequired == mib.IsShiftRequired;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean UsesSamePrimaryButtons(InputBinding binding)
        {
            if (ReferenceEquals(binding, null)) return false;
            if (ReferenceEquals(binding, this)) return true;

            var mib = binding as MouseInputBinding;
            if (mib != null)
            {
                return
                    this.Mouse == mib.Mouse &&
                    this.Button == mib.Button;
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
            return pressed && mouse.IsButtonPressed(button, ignoreRepeats: ignoreRepeats);
        }

        /// <inheritdoc/>
        public override Boolean IsReleased()
        {
            return released;
        }

        /// <summary>
        /// Gets the <see cref="MouseDevice"/> that created this input binding.
        /// </summary>
        public MouseDevice Mouse
        {
            get { return mouse; }
        }

        /// <summary>
        /// Gets the <see cref="MouseButton"/> value that represents the binding's primary button.
        /// </summary>
        public MouseButton Button
        {
            get { return button; }
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
                new XElement("Button", button),
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
                (!control || mouse.IsControlDown) &&
                (!alt     || mouse.IsAltDown) &&
                (!shift   || mouse.IsShiftDown);
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
        /// Builds a string representation of the mouse binding.
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
            builder.Append(Localization.Get("MOUSE_BUTTON_" + Button));

            return builder.ToString();
        }

        // Property values.
        private readonly MouseDevice mouse;
        private readonly MouseButton button;
        private readonly Boolean control;
        private readonly Boolean alt;
        private readonly Boolean shift;
        private readonly String stringRepresentation;

        // State values.
        private Boolean pressed;
        private Boolean released;
    }
}
