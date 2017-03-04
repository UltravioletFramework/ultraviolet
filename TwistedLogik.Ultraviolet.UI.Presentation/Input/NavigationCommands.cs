using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains a standard set of navigation-related commands.
    /// </summary>
    public static class NavigationCommands
    {
        /// <summary>
        /// Gets the value that represents the Browse Back command.
        /// </summary>
        public static RoutedUICommand BrowseBack => browseBack.Value;

        /// <summary>
        /// Gets the value that represents the Browse Forward command.
        /// </summary>
        public static RoutedUICommand BrowseForward => browseForward.Value;

        /// <summary>
        /// Gets the value that represents the Browse Home command.
        /// </summary>
        public static RoutedUICommand BrowseHome => browseHome.Value;

        /// <summary>
        /// Gets the value that represents the Browse Stop command.
        /// </summary>
        public static RoutedUICommand BrowseStop => browseStop.Value;

        /// <summary>
        /// Gets the value that represents the Refresh command.
        /// </summary>
        public static RoutedUICommand Refresh => refresh.Value;

        /// <summary>
        /// Gets the value that represents the Favorites command.
        /// </summary>
        public static RoutedUICommand Favorites => favorites.Value;

        /// <summary>
        /// Gets the value that represents the Search command.
        /// </summary>
        public static RoutedUICommand Search => search.Value;

        /// <summary>
        /// Gets the value that represents the Increase Zoom command.
        /// </summary>
        public static RoutedUICommand IncreaseZoom => increaseZoom.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Zoom command.
        /// </summary>
        public static RoutedUICommand DecreaseZoom => decreaseZoom.Value;

        /// <summary>
        /// Gets the value that represents the Zoom command.
        /// </summary>
        public static RoutedUICommand Zoom => zoom.Value;

        /// <summary>
        /// Gets the value that represents the Next Page command.
        /// </summary>
        public static RoutedUICommand NextPage => nextPage.Value;

        /// <summary>
        /// Gets the value that represents the Previous Page command.
        /// </summary>
        public static RoutedUICommand PreviousPage => previousPage.Value;

        /// <summary>
        /// Gets the value that represents the First Page command.
        /// </summary>
        public static RoutedUICommand FirstPage => firstPage.Value;

        /// <summary>
        /// Gets the value that represents the Last Page command.
        /// </summary>
        public static RoutedUICommand LastPage => lastPage.Value;

        /// <summary>
        /// Gets the value that represents the Go to Page command.
        /// </summary>
        public static RoutedUICommand GoToPage => goToPage.Value;

        /// <summary>
        /// Gets the value that represents the Navigate Journal command.
        /// </summary>
        public static RoutedUICommand NavigateJournal => navigateJournal.Value;

        // Property values.
        private static Lazy<RoutedUICommand> browseBack = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_BACK", nameof(BrowseBack), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> browseForward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_FORWARD", nameof(BrowseForward), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> browseHome = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_HOME", nameof(BrowseHome), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> browseStop = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_STOP", nameof(BrowseStop), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> refresh = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_REFRESH", nameof(Refresh), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> favorites = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_FAVORITES", nameof(Favorites), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> search = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_SEARCH", nameof(Search), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> increaseZoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_INCREASE_ZOOM", nameof(IncreaseZoom), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> decreaseZoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_DECREASE_ZOOM", nameof(DecreaseZoom), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> zoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_ZOOM", nameof(Zoom), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> nextPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_NEXT_PAGE", nameof(NextPage), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> previousPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_PREVIOUS_PAGE", nameof(PreviousPage), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> firstPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_FIRST_PAGE", nameof(FirstPage), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> lastPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_LAST_PAGE", nameof(LastPage), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> goToPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_GO_TO_PAGE", nameof(GoToPage), typeof(MediaCommands)));
        private static Lazy<RoutedUICommand> navigateJournal = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_NAVIGATE_JOURNAL", nameof(NavigateJournal), typeof(MediaCommands)));
    }
}
