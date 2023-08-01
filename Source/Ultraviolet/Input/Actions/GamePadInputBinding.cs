using System;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Core.Xml;

namespace Ultraviolet.Input
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
            Contract.Require(uv, nameof(uv));
            Contract.Require(element, nameof(element));

            this.uv = uv;
            this.playerIndex = element.ElementValueInt32("Player") ?? 0;
            this.button = element.ElementValueEnum<GamePadButton>("Button") ?? GamePadButton.None;

            this.stringRepresentation = BuildStringRepresentation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadInputBinding"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="playerIndex">The index of the player for which to create the binding.</param>
        /// <param name="button">A <see cref="GamePadButton"/> value representing the binding's primary button.</param>
        public GamePadInputBinding(UltravioletContext uv, Int32 playerIndex, GamePadButton button)
            : base()
        {
            Contract.Require(uv, nameof(uv));
            Contract.EnsureRange(playerIndex >= 0, nameof(playerIndex));

            this.uv = uv;
            this.playerIndex = playerIndex;
            this.button = button;

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
            var gamePad = uv.GetInput().GetGamePadForPlayer(playerIndex);

            released = false;
            if (pressed)
            {
                if (!Enabled || gamePad == null || gamePad.IsButtonReleased(button))
                {
                    pressed = false;
                    released = true;
                    OnReleased();
                }
            }
            else
            {
                if (Enabled && gamePad != null && gamePad.IsButtonPressed(button))
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
                    this.PlayerIndex == gpib.PlayerIndex &&
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
                    this.PlayerIndex == gpib.PlayerIndex &&
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
            var gamePad = uv.GetInput().GetGamePadForPlayer(playerIndex);
            return pressed && (gamePad != null && gamePad.IsButtonPressed(button, ignoreRepeats: ignoreRepeats));
        }

        /// <inheritdoc/>
        public override Boolean IsReleased()
        {
            var gamePad = uv.GetInput().GetGamePadForPlayer(playerIndex);
            return released && (gamePad == null || gamePad.IsButtonReleased(button));
        }

        /// <summary>
        /// Gets the index of the 
        /// </summary>
        public Int32 PlayerIndex
        {
            get { return playerIndex; }
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
                new XElement("Player", playerIndex),
                new XElement("Button", button)
            );
        }

        /// <inheritdoc/>
        protected override Int32 CalculatePriority()
        {
            return 0;
        }
        
        /// <summary>
        /// Builds a string representation of the game pad binding.
        /// </summary>
        private String BuildStringRepresentation()
        {
            return Localization.Get("GAME_PAD_BUTTON_" + Button);
        }

        // Property values.
        private readonly Int32 playerIndex;
        private readonly GamePadButton button;
        private readonly String stringRepresentation;

        // State values.
        private readonly UltravioletContext uv;
        private Boolean pressed;
        private Boolean released;
    }
}
