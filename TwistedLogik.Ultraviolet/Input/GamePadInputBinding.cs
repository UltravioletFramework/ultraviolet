using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents a game pad input binding.
    /// </summary>
    public sealed class GamePadInputBinding : InputBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="element">The XML element that contains the binding data.</param>
        internal GamePadInputBinding(UltravioletContext uv, XElement element)
        {
            Contract.Require(element, "element");

            this.gamePad = uv.GetInput().GetFirstGamePad();
            // TODO: What if there are no game pads?
            this.button = element.ElementValueEnum<GamePadButton>("Button") ?? GamePadButton.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadInputBinding"/> class.
        /// </summary>
        /// <param name="gamePad">The game pad device with which the binding is associated.</param>
        /// <param name="button">A <see cref="GamePadButton"/> value representing the binding's primary button.</param>
        public GamePadInputBinding(GamePadDevice gamePad, GamePadButton button)
            : base()
        {
            Contract.Require(gamePad, "gamePad");

            this.gamePad = gamePad;
            this.button  = button;
        }

        /// <inheritdoc/>
        public override void Update()
        {
            released = false;
            if (pressed)
            {
                if (!Enabled || gamePad.IsButtonReleased(button))
                {
                    pressed = false;
                    released = true;
                    OnReleased();
                }
            }
            else
            {
                if (Enabled && gamePad.IsButtonPressed(button))
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

            var gpib = binding as GamePadInputBinding;
            if (gpib != null)
            {
                return
                    this.GamePad == gpib.GamePad &&
                    this.Button == gpib.Button;
            }

            return false;
        }

        /// <inheritdoc/>
        public override Boolean UsesSamePrimaryButtons(InputBinding binding)
        {
            if (ReferenceEquals(binding, null)) return false;
            if (ReferenceEquals(binding, this)) return true;

            var gpib = binding as GamePadInputBinding;
            if (gpib != null)
            {
                return
                    this.GamePad == gpib.GamePad &&
                    this.Button == gpib.Button;
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
            return pressed && gamePad.IsButtonPressed(button, ignoreRepeats: ignoreRepeats);
        }

        /// <inheritdoc/>
        public override Boolean IsReleased()
        {
            return released && gamePad.IsButtonReleased(button);
        }

        /// <summary>
        /// Gets the <see cref="GamePadDevice"/> that created this input binding.
        /// </summary>
        public GamePadDevice GamePad
        {
            get { return gamePad; }
        }

        /// <summary>
        /// Gets the <see cref="GamePadButton"/> value that represents the binding's primary button.
        /// </summary>
        public GamePadButton Button
        {
            get { return button; }
        }

        /// <inheritdoc/>
        internal override XElement ToXml(string name = null)
        {
            return new XElement(name ?? "Binding", new XAttribute("Type", GetType().FullName),
                new XElement("Button", button)
            );
        }

        /// <inheritdoc/>
        protected override Int32 CalculatePriority()
        {
            return 0;
        }

        // Property values.
        private readonly GamePadDevice gamePad;
        private readonly GamePadButton button;

        // State values.
        private Boolean pressed;
        private Boolean released;
    }
}
