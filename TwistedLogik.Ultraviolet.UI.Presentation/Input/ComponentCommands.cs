using System;
using TwistedLogik.Ultraviolet.Input;

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
        /// Gets the value that represents the Move Focus Page Up command.
        /// </summary>
        public static RoutedUICommand MoveFocusPageUp => moveFocusPageUp.Value;

        /// <summary>
        /// Gets the value that represents the Move Focus Page Down command.
        /// </summary>
        public static RoutedUICommand MoveFocusPageDown => moveFocusPageDown.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Down command.
        /// </summary>
        public static RoutedUICommand ScrollDown => scrollDown.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Left command.
        /// </summary>
        public static RoutedUICommand ScrollLeft => scrollLeft.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Right command.
        /// </summary>
        public static RoutedUICommand ScrollRight => scrollRight.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Up command.
        /// </summary>
        public static RoutedUICommand ScrollUp => scrollUp.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Page Down command.
        /// </summary>
        public static RoutedUICommand ScrollPageDown => scrollPageDown.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Page Left command.
        /// </summary>
        public static RoutedUICommand ScrollPageLeft => scrollPageLeft.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Page Right command.
        /// </summary>
        public static RoutedUICommand ScrollPageRight => scrollPageRight.Value;

        /// <summary>
        /// Gets the value that represents the Scroll Page Up command.
        /// </summary>
        public static RoutedUICommand ScrollPageUp => scrollPageUp.Value;

        /// <summary>
        /// Gets the value that represents the Scroll by Line command.
        /// </summary>
        public static RoutedUICommand ScrollByLine => scrollByLine.Value;

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

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();

            switch (name)
            {
                case nameof(ScrollPageUp):
                    gestures.Add(new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
                    break;

                case nameof(ScrollPageDown):
                    gestures.Add(new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
                    break;

                case nameof(MoveLeft):
                    gestures.Add(new KeyGesture(Key.Left, ModifierKeys.None, "Left"));
                    break;

                case nameof(MoveRight):
                    gestures.Add(new KeyGesture(Key.Right, ModifierKeys.None, "Right"));
                    break;

                case nameof(MoveUp):
                    gestures.Add(new KeyGesture(Key.Up, ModifierKeys.None, "Up"));
                    break;

                case nameof(MoveDown):
                    gestures.Add(new KeyGesture(Key.Down, ModifierKeys.None, "Down"));
                    break;

                case nameof(MoveToHome):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.None, "Home"));
                    break;

                case nameof(MoveToEnd):
                    gestures.Add(new KeyGesture(Key.End, ModifierKeys.None, "End"));
                    break;

                case nameof(MoveToPageUp):
                    gestures.Add(new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
                    break;

                case nameof(MoveToPageDown):
                    gestures.Add(new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
                    break;

                case nameof(ExtendSelectionUp):
                    gestures.Add(new KeyGesture(Key.Up, ModifierKeys.Shift, "Shift+Up"));
                    break;

                case nameof(ExtendSelectionDown):
                    gestures.Add(new KeyGesture(Key.Down, ModifierKeys.Shift, "Shift+Down"));
                    break;

                case nameof(ExtendSelectionLeft):
                    gestures.Add(new KeyGesture(Key.Left, ModifierKeys.Shift, "Shift+Left"));
                    break;

                case nameof(ExtendSelectionRight):
                    gestures.Add(new KeyGesture(Key.Right, ModifierKeys.Shift, "Shift+Right"));
                    break;

                case nameof(SelectToHome):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Shift, "Shift+Home"));
                    break;

                case nameof(SelectToEnd):
                    gestures.Add(new KeyGesture(Key.End, ModifierKeys.Shift, "Shift+End"));
                    break;

                case nameof(SelectToPageUp):
                    gestures.Add(new KeyGesture(Key.PageUp, ModifierKeys.Shift, "Shift+PageUp"));
                    break;

                case nameof(SelectToPageDown):
                    gestures.Add(new KeyGesture(Key.PageDown, ModifierKeys.Shift, "Shift+PageDown"));
                    break;

                case nameof(MoveFocusUp):
                    gestures.Add(new KeyGesture(Key.Up, ModifierKeys.Control, "Ctrl+Up"));
                    break;

                case nameof(MoveFocusDown):
                    gestures.Add(new KeyGesture(Key.Down, ModifierKeys.Control, "Ctrl+Down"));
                    break;

                case nameof(MoveFocusForward):
                    gestures.Add(new KeyGesture(Key.Right, ModifierKeys.Control, "Ctrl+Right"));
                    break;

                case nameof(MoveFocusBack):
                    gestures.Add(new KeyGesture(Key.Left, ModifierKeys.Control, "Ctrl+Left"));
                    break;

                case nameof(MoveFocusPageUp):
                    gestures.Add(new KeyGesture(Key.PageUp, ModifierKeys.Control, "Ctrl+PageUp"));
                    break;

                case nameof(MoveFocusPageDown):
                    gestures.Add(new KeyGesture(Key.PageDown, ModifierKeys.Control, "Ctrl+PageDown"));
                    break;
            }

            return gestures;
        }

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
        private static Lazy<RoutedUICommand> moveFocusPageUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_PAGE_UP", nameof(MoveFocusPageUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> moveFocusPageDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_MOVE_FOCUS_PAGE_DOWN", nameof(MoveFocusPageDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_DOWN", nameof(ScrollDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollLeft = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_LEFT", nameof(ScrollLeft), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_RIGHT", nameof(ScrollRight), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_UP", nameof(ScrollUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollPageDown = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_PAGE_DOWN", nameof(ScrollPageDown), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollPageLeft = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_PAGE_LEFT", nameof(ScrollPageLeft), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollPageRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_PAGE_RIGHT", nameof(ScrollPageRight), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollPageUp = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_PAGE_UP", nameof(ScrollPageUp), typeof(ComponentCommands)));
        private static Lazy<RoutedUICommand> scrollByLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("COMPONENT_COMMAND_SCROLL_BY_LINE", nameof(ScrollByLine), typeof(ComponentCommands)));
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
