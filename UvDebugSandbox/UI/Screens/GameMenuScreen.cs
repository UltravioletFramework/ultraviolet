using TwistedLogik.Ultraviolet.Content;

namespace UvDebugSandbox.UI.Screens
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
            View.SetViewModel(new GameMenuViewModel(this));
        }        
    }
}
