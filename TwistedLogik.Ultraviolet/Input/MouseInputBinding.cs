using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Input
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
            Contract.Require(element, "element");

            this.mouse = uv.GetInput().GetMouse();

            this.button = element.ElementValueEnum<MouseButton>("Button") ?? MouseButton.None;
            this.control = element.ElementValueBoolean("Control") ?? false;
            this.alt = element.ElementValueBoolean("Alt") ?? false;
            this.shift = element.ElementValueBoolean("Shift") ?? false; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="button">The <see cref="MouseButton"/> value that represents the binding's primary button.</param>
        public MouseInputBinding(UltravioletContext uv, MouseButton button)
        {
            Contract.Require(uv, "uv");

            if (!uv.GetInput().IsMouseSupported())
            {
                throw new NotSupportedException();
            }

            this.mouse  = uv.GetInput().GetMouse();
            this.button = button;
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

            this.mouse   = uv.GetInput().GetMouse();
            this.button  = button;
            this.control = control;
            this.alt     = alt;
            this.shift   = shift;
        }

        /// <summary>
        /// Updates the binding's state.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same button configuration as the specified input binding.
        /// </summary>
        /// <param name="binding">The <see cref="InputBinding"/> to compare against this input binding.</param>
        /// <returns><c>true</c> if the specified input binding uses the same device and the same button 
        /// configuration as this input binding; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether the input binding uses the same device 
        /// and the same primary buttons as the specified input binding.
        /// </summary>
        /// <param name="binding">The <see cref="InputBinding"/> to compare against this input binding.</param>
        /// <returns><c>true</c> if the specified input binding uses the same device and the same primary 
        /// buttons as this input binding; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether the binding is down.
        /// </summary>
        /// <returns><c>true</c> if the binding is down; otherwise, <c>false</c>.</returns>
        public override Boolean IsDown()
        {
            return pressed;
        }

        /// <summary>
        /// Gets a value indicating whether the binding is up.
        /// </summary>
        /// <returns><c>true</c> if the binding is up; otherwise, <c>false</c>.</returns>
        public override Boolean IsUp()
        {
            return !pressed;
        }

        /// <summary>
        /// Gets a value indicating whether the binding was pressed this frame.
        /// </summary>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns><c>true</c> if the binding was pressed this frame; otherwise, <c>false</c>.</returns>
        public override Boolean IsPressed(Boolean ignoreRepeats = true)
        {
            return pressed && mouse.IsButtonPressed(button, ignoreRepeats: ignoreRepeats);
        }

        /// <summary>
        /// Gets a value indicating whether the binding was released this frame.
        /// </summary>
        /// <returns><c>true</c> if the binding was released this frame; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Creates an XML element that represents the binding.
        /// </summary>
        /// <param name="name">The name to give to the created XML element.</param>
        /// <returns>An XML element that represents the binding.</returns>
        internal override XElement ToXml(String name = null)
        {
            return new XElement(name ?? "Binding", new XAttribute("Type", GetType().FullName),
                new XElement("Button", button),
                new XElement("Control", control),
                new XElement("Alt", alt),
                new XElement("Shift", shift)
            );
        }

        /// <summary>
        /// Calculates the binding's priority relative to other bindings with the same primary buttons.
        /// </summary>
        /// <returns>The binding's priority relative to other bindings with the same primary buttons.</returns>
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
        /// <returns><c>true</c> if the binding's modifier states are satisfied; otherwise, <c>false</c>.</returns>
        private Boolean AreModifiersSatisfied()
        {
            return
                (!control || mouse.IsControlDown) &&
                (!alt     || mouse.IsAltDown) &&
                (!shift   || mouse.IsShiftDown);
        }

        // Property values.
        private readonly MouseDevice mouse;
        private readonly MouseButton button;
        private readonly Boolean control;
        private readonly Boolean alt;
        private readonly Boolean shift;

        // State values.
        private Boolean pressed;
        private Boolean released;
    }
}
