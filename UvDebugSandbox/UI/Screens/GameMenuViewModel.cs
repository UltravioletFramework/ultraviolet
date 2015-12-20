using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Screens
{
    /// <summary>
    /// Represents the view model for <see cref="GameMenuScreen"/>.
    /// </summary>
    public sealed class GameMenuViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="GameMenuScreen"/> that owns this view model.</param>
        public GameMenuViewModel(GameMenuScreen owner)
        {
            Contract.Require(owner, "owner");

            this.owner = owner;
        }
        
        /// <summary>
        /// Handles the Click event for the "Start" button.
        /// </summary>
        public void Click_Start(DependencyObject element, ref RoutedEventData data)
        {
            var playScreen = owner.UIScreenService.Get<GamePlayScreen>();
            owner.Ultraviolet.GetUI().GetScreens().CloseThenOpen(owner, playScreen);
        }

        /// <summary>
        /// Handles the Click event for the "Exit" button.
        /// </summary>
        public void Click_Exit(DependencyObject element, ref RoutedEventData data)
        {
            owner.Ultraviolet.Host.Exit();
        }
        
        /// <summary>
        /// Gets the <see cref="GameMenuScreen"/> that owns the view model.
        /// </summary>
        public GameMenuScreen Owner
        {
            get { return owner; }
        }

        // Property values.
        private readonly GameMenuScreen owner;
    }
}
