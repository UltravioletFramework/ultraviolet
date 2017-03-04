using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains a standard set of component-related commands.
    /// </summary>
    public static class ComponentCommands
    {
        /// <summary>
        /// Gets the value that represents the Extend Selection Down command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionDown => extendSelectionDown.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection Left command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionLeft => extendSelectionLeft.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection Right command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionRight => extendSelectionRight.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection Up command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionUp => extendSelectionUp.Value;

        /// <summary>
        /// Gets the value that represents the Move Down command.
        /// </summary>
        public static RoutedUICommand MoveDown => moveDown.Value;

        /// <summary>
        /// Gets the value that represents the Move Left command.
        /// </summary>
        public static RoutedUICommand MoveLeft => moveLeft.Value;

        /// <summary>
        /// Gets the value that represents the Move Right command.
        /// </summary>
        public static RoutedUICommand MoveRight => moveRight.Value;

        /// <summary>
        /// Gets the value that represents the Move Up command.
        /// </summary>
        public static RoutedUICommand MoveUp => moveUp.Value;

        /// <summary>
        /// Gets the value that represents the Move to End command.
        /// </summary>
        public static RoutedUICommand MoveToEnd => moveToEnd.Value;

        /// <summary>
        /// Gets the value that represents the Move to Home command.
        /// </summary>
        public static RoutedUICommand MoveToHome => moveToHome.Value;
        
        /// <summary>
        /// Gets the value that represents the Move to Page Down command.
        /// </summary>
        public static RoutedUICommand MoveToPageDown => moveToPageDown.Value;

        /// <summary>
        /// Gets the value that represents the Move to Page Up command.
        /// </summary>
        public static RoutedUICommand MoveToPageUp => moveToPageUp.Value;

        /// <summary>
        /// Gets the value that represents the Move Focus Back command.
        /// </summary>
        public static RoutedUICommand MoveFocusBack => moveFocusBack.Value;

        /// <summary>
        /// Gets the value that represents the Move Focus Forward command.
        /// </summary>
        public static RoutedUICommand MoveFocusForward => moveFocusForward.Value;

        /// <summary>
        /// Gets the value that represents the Move Focus Up command.
        /// </summary>
        public static RoutedUICommand MoveFocusUp => moveFocusUp.Value;

        /// <summary>
        /// Gets the value that represents the Move Focus Down command.
        /// </summary>
        public static RoutedUICommand MoveFocusDown => moveFocusDown.Value;
        
        /// <summary>
        /// Gets the value that represents the Scroll Down command.
        /// </summary>
        public static RoutedUICommand ScrollDown => moveDown.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Left command.
        /// </summary>
        public static RoutedUICommand ScrollLeft => moveLeft.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Right command.
        /// </summary>
        public static RoutedUICommand ScrollRight => moveRight.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Up command.
        /// </summary>
        public static RoutedUICommand ScrollUp => moveUp.Value;

        /// <summary>
        /// Gets the value that represents the Select to End command.
        /// </summary>
        public static RoutedUICommand SelectToEnd => selectToEnd.Value;

        /// <summary>
        /// Gets the value that represents the Select to Home command.
        /// </summary>
        public static RoutedUICommand SelectToHome => selectToHome.Value;

        /// <summary>
        /// Gets the value that represents the Select to Page Down command.
        /// </summary>
        public static RoutedUICommand SelectToPageDown => selectToPageDown.Value;

        /// <summary>
        /// Gets the value that represents the Select to Page Up command.
        /// </summary>
        public static RoutedUICommand SelectToPageUp => selectToPageUp.Value;

        // Property values.
        private static Lazy<RoutedUICommand> extendSelectionDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_EXTEND_SELECTION_DOWN", nameof(ExtendSelectionDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> extendSelectionLeft = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_EXTEND_SELECTION_LEFT", nameof(ExtendSelectionLeft), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> extendSelectionRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_EXTEND_SELECTION_RIGHT", nameof(ExtendSelectionRight), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> extendSelectionUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_EXTEND_SELECTION_UP", nameof(ExtendSelectionUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_DOWN", nameof(MoveDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveLeft = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_LEFT", nameof(MoveLeft), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_RIGHT", nameof(MoveRight), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_UP", nameof(MoveUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveToEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_TO_END", nameof(MoveToEnd), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveToHome = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_TO_HOME", nameof(MoveToHome), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveToPageDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_TO_PAGE_DOWN", nameof(MoveToPageDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveToPageUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_TO_PAGE_UP", nameof(MoveToPageUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveFocusBack = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_BACK", nameof(MoveFocusBack), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveFocusForward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_FORWARD", nameof(MoveFocusForward), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveFocusUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_UP", nameof(MoveFocusUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveFocusDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_DOWN", nameof(MoveFocusDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_DOWN", nameof(ScrollDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollLeft = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_LEFT", nameof(ScrollLeft), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_RIGHT", nameof(ScrollRight), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_UP", nameof(ScrollUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> selectToEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SELECT_TO_END", nameof(SelectToEnd), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> selectToHome = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SELECT_TO_HOME", nameof(SelectToHome), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> selectToPageDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SELECT_TO_PAGE_DOWN", nameof(SelectToPageDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> selectToPageUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SELECT_TO_PAGE_UP", nameof(SelectToPageUp), typeof(ComponentCommands)));
    }
}
