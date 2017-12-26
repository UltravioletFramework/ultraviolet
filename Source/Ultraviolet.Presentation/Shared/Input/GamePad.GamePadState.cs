using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    partial class GamePad
    {
        /// <summary>
        /// Represents the game pad state of the current Ultraviolet context.
        /// </summary>
        private class GamePadState : UltravioletResource
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GamePadState"/> class.
            /// </summary>
            /// <param name="uv">The Ultraviolet context.</param>
            public GamePadState(UltravioletContext uv)
                : base(uv)
            {

            }

            /// <summary>
            /// Gets the game pad device that corresponds to the specified player index.
            /// </summary>
            /// <param name="playerIndex">The index of the player for which to retrieve a game pad device.</param>
            /// <returns>The game pad device for the specified player, or <see langword="null"/> if no such game pad is connected.</returns>
            public GamePadDevice GetGamePadForPlayer(Int32 playerIndex) =>
                Ultraviolet.GetInput().GetGamePadForPlayer(playerIndex);

            /// <summary>
            /// Gets the primary keyboard input device.
            /// </summary>
            public GamePadDevice PrimaryDevice =>
                Ultraviolet.GetInput().GetPrimaryGamePad();
        }
    }
}
