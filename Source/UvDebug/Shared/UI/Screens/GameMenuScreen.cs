using System;
using Ultraviolet.Content;
using Ultraviolet.UI;

namespace UvDebug.UI.Screens
{
    /// <summary>
    /// Represents the game's main menu screen.
    /// </summary>
    public class GameMenuScreen : GameScreenBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuScreen"/> class.
        /// </summary>
        /// <param name="globalContent">The content manager with which to load globally-available assets.</param>
        /// <param name="uiScreenService">The screen service which created this screen.</param>
        public GameMenuScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/GameMenuScreen", "GameMenuScreen", globalContent, uiScreenService)
        {

        }

        /// <inheritdoc/>
        protected override Object CreateViewModel(UIView view)
        {
            return new GameMenuViewModel(this);
        }

        /// <inheritdoc/>
        protected override void OnOpening()
        {
            ResetViewModel();
            base.OnOpening();
        }
    }
}
