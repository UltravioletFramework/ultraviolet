using Ultraviolet;
using Ultraviolet.Input;

namespace UvDebug.Input
{
    /// <summary>
    /// Contains extension methods for the IUltravioletInput interface.
    /// </summary>
    public static class IUltravioletInputExtensions
    {
        /// <summary>
        /// Gets the game's input actions.
        /// </summary>
        /// <param name="this">The Ultraviolet Input subsystem.</param>
        /// <returns>The game's input actions.</returns>
        public static GameInputActions GetActions(this IUltravioletInput @this)
        {
            return actions;
        }

        // The singleton instance that represents the game's collection of input actions.
        // This instance is bound to the lifespan of the Ultraviolet context that creates it.
        private static readonly UltravioletSingleton<GameInputActions> actions = 
            InputActionCollection.CreateSingleton<GameInputActions>();
    }
}