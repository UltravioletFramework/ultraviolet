using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Input
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
            Contract.Require(element, "element");

            this.keyboard = uv.GetInput().GetKeyboard();

            this.key = element.ElementValueEnum<Key>("Key") ?? Key.None;
            this.control = element.ElementValueBoolean("Control") ?? false;
            this.alt = element.ElementValueBoolean("Alt") ?? false;
            this.shift = element.ElementValueBoolean("Shift") ?? false; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="key">A <see cref="Key"/> value representing the binding's primary key.</param>
        public KeyboardInputBinding(UltravioletContext uv, Key key)
        {
            Contract.Require(uv, "uv");

            if (!uv.GetInput().IsKeyboardSupported())
            {
                throw new NotSupportedException();
            }

            this.keyboard = uv.GetInput().GetKeyboard();
            this.key      = key;
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
            this.key      = key;
            this.control  = control;
            this.alt      = alt;
            this.shift    = shift;
        }

        /// <summary>
        /// Updates the binding's state.
        /// </summary>
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

            var kbib = binding as KeyboardInputBinding;
            if (kbib != null)
            {
                return 
                    this.Keyboard == kbib.Keyboard &&
                    this.Key == kbib.Key;
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
            return pressed && keyboard.IsKeyPressed(key, ignoreRepeats: ignoreRepeats);
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

        /// <summary>
        /// Creates an XML element that represents the binding.
        /// </summary>
        /// <param name="name">The name to give to the created XML element.</param>
        /// <returns>An XML element that represents the binding.</returns>
        internal override XElement ToXml(String name = null)
        {
            return new XElement(name ?? "Binding", new XAttribute("Type", GetType().FullName),
                new XElement("Key", key),
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
                (!control || keyboard.IsControlDown) &&
                (!alt     || keyboard.IsAltDown) &&
                (!shift   || keyboard.IsShiftDown);

        }

        // Property values.
        private readonly KeyboardDevice keyboard;
        private readonly Key key;
        private readonly Boolean control;
        private readonly Boolean alt;
        private readonly Boolean shift;

        // State values.
        private Boolean pressed;
        private Boolean released;
    }
}
