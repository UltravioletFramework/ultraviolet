using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
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

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();

            switch (name)
            {
                case nameof(BrowseBack):
                    gestures.Add(new KeyGesture(Key.Left, ModifierKeys.Alt, "Alt+Left"));
                    gestures.Add(new KeyGesture(Key.Backspace, ModifierKeys.None, "Backspace"));
                    gestures.Add(new KeyGesture(Key.AppControlBack, ModifierKeys.None, "BrowserBack"));
                    break;

                case nameof(BrowseForward):
                    gestures.Add(new KeyGesture(Key.Right, ModifierKeys.Alt, "Alt+Right"));
                    gestures.Add(new KeyGesture(Key.Backspace, ModifierKeys.Shift, "Shift+Backspace"));
                    gestures.Add(new KeyGesture(Key.AppControlForward, ModifierKeys.None, "BrowserForward"));
                    break;

                case nameof(BrowseStop):
                    gestures.Add(new KeyGesture(Key.Escape, ModifierKeys.Alt, "Alt+Esc"));
                    gestures.Add(new KeyGesture(Key.AppControlHome, ModifierKeys.None, "BrowserHome"));
                    break;

                case nameof(Refresh):
                    gestures.Add(new KeyGesture(Key.F5, ModifierKeys.None, "F5"));
                    gestures.Add(new KeyGesture(Key.AppControlRefresh, ModifierKeys.None, "BrowserRefresh"));
                    break;

                case nameof(Favorites):
                    gestures.Add(new KeyGesture(Key.I, ModifierKeys.Control, "Ctrl+I"));
                    gestures.Add(new KeyGesture(Key.AppControlBookmarks, ModifierKeys.None, "BrowserFavorites"));
                    break;

                case nameof(Search):
                    gestures.Add(new KeyGesture(Key.F3, ModifierKeys.None, "F3"));
                    gestures.Add(new KeyGesture(Key.AppControlSearch, ModifierKeys.None, "BrowserSearch"));
                    break;
            }

            return gestures;
        }

        // Property values.
        private static Lazy<RoutedUICommand> browseBack = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_BACK", nameof(BrowseBack), typeof(MediaCommands), GetInputGestures(nameof(BrowseBack))));
        private static Lazy<RoutedUICommand> browseForward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_FORWARD", nameof(BrowseForward), typeof(MediaCommands), GetInputGestures(nameof(BrowseForward))));
        private static Lazy<RoutedUICommand> browseHome = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_HOME", nameof(BrowseHome), typeof(MediaCommands), GetInputGestures(nameof(BrowseHome))));
        private static Lazy<RoutedUICommand> browseStop = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_BROWSE_STOP", nameof(BrowseStop), typeof(MediaCommands), GetInputGestures(nameof(BrowseStop))));
        private static Lazy<RoutedUICommand> refresh = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_REFRESH", nameof(Refresh), typeof(MediaCommands), GetInputGestures(nameof(Refresh))));
        private static Lazy<RoutedUICommand> favorites = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_FAVORITES", nameof(Favorites), typeof(MediaCommands), GetInputGestures(nameof(Favorites))));
        private static Lazy<RoutedUICommand> search = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_SEARCH", nameof(Search), typeof(MediaCommands), GetInputGestures(nameof(Search))));
        private static Lazy<RoutedUICommand> increaseZoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_INCREASE_ZOOM", nameof(IncreaseZoom), typeof(MediaCommands), GetInputGestures(nameof(IncreaseZoom))));
        private static Lazy<RoutedUICommand> decreaseZoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_DECREASE_ZOOM", nameof(DecreaseZoom), typeof(MediaCommands), GetInputGestures(nameof(DecreaseZoom))));
        private static Lazy<RoutedUICommand> zoom = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_ZOOM", nameof(Zoom), typeof(MediaCommands), GetInputGestures(nameof(Zoom))));
        private static Lazy<RoutedUICommand> nextPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_NEXT_PAGE", nameof(NextPage), typeof(MediaCommands), GetInputGestures(nameof(NextPage))));
        private static Lazy<RoutedUICommand> previousPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_PREVIOUS_PAGE", nameof(PreviousPage), typeof(MediaCommands), GetInputGestures(nameof(PreviousPage))));
        private static Lazy<RoutedUICommand> firstPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_FIRST_PAGE", nameof(FirstPage), typeof(MediaCommands), GetInputGestures(nameof(FirstPage))));
        private static Lazy<RoutedUICommand> lastPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_LAST_PAGE", nameof(LastPage), typeof(MediaCommands), GetInputGestures(nameof(LastPage))));
        private static Lazy<RoutedUICommand> goToPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_GO_TO_PAGE", nameof(GoToPage), typeof(MediaCommands), GetInputGestures(nameof(GoToPage))));
        private static Lazy<RoutedUICommand> navigateJournal = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("NAVIGATION_COMMAND_NAVIGATE_JOURNAL", nameof(NavigateJournal), typeof(MediaCommands), GetInputGestures(nameof(NavigateJournal))));
    }
}
