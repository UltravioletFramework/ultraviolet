using System;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Contains a standard set of commands used by the <see cref="TextEditor"/> primitive.
    /// </summary>
    public static class TextEditorCommands
    {
        /// <summary>
        /// Gets the value that represents the Select All command.
        /// </summary>
        public static RoutedUICommand SelectAll => ApplicationCommands.SelectAll;

        /// <summary>
        /// Gets the value that represents the Cut command.
        /// </summary>
        public static RoutedUICommand Cut => ApplicationCommands.Cut;

        /// <summary>
        /// Gets the value that represents the Copy command.
        /// </summary>
        public static RoutedUICommand Copy => ApplicationCommands.Copy;

        /// <summary>
        /// Gets the value that represents the Paste command.
        /// </summary>
        public static RoutedUICommand Paste => ApplicationCommands.Paste;

        /// <summary>
        /// Gets the value that represents the Move Left command.
        /// </summary>
        public static RoutedUICommand MoveLeft => ComponentCommands.MoveLeft;

        /// <summary>
        /// Gets the value that represents the Move Right command.
        /// </summary>
        public static RoutedUICommand MoveRight => ComponentCommands.MoveRight;

        /// <summary>
        /// Gets the value that represents the Move Up command.
        /// </summary>
        public static RoutedUICommand MoveUp => ComponentCommands.MoveUp;

        /// <summary>
        /// Gets the value that represents the Move Down command.
        /// </summary>
        public static RoutedUICommand MoveDown => ComponentCommands.MoveDown;

        /// <summary>
        /// Gets the value that represents the Move to Start of Line command.
        /// </summary>
        public static RoutedUICommand MoveToStartOfLine => moveToStartOfLine.Value;

        /// <summary>
        /// Gets the value that represents the Move to End of Line command.
        /// </summary>
        public static RoutedUICommand MoveToEndOfLine => moveToEndOfLine.Value;

        /// <summary>
        /// Gets the value that represents the Move to Start of Text command.
        /// </summary>
        public static RoutedUICommand MoveToStartOfText => moveToStartOfText.Value;

        /// <summary>
        /// Gets the value that represents the Move to End of Text command.
        /// </summary>
        public static RoutedUICommand MoveToEndOfText => moveToEndOfText.Value;

        /// <summary>
        /// Gets the value that represents the Move to Page Up command.
        /// </summary>
        public static RoutedUICommand MoveToPageUp => ComponentCommands.MoveToPageUp;

        /// <summary>
        /// Gets the value that represents the Move to Page Down command.
        /// </summary>
        public static RoutedUICommand MoveToPageDown => ComponentCommands.MoveToPageDown;

        /// <summary>
        /// Gets the value that represents the Extend Selection Left command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionLeft => ComponentCommands.ExtendSelectionLeft;

        /// <summary>
        /// Gets the value that represents the Extend Selection Right command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionRight => ComponentCommands.ExtendSelectionRight;

        /// <summary>
        /// Gets the value that represents the Extend Selection Up command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionUp => ComponentCommands.ExtendSelectionUp;

        /// <summary>
        /// Gets the value that represents the Extend Selection Down command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionDown => ComponentCommands.ExtendSelectionDown;

        /// <summary>
        /// Gets the value that represents the Extend Selection to Start of Line command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionToStartOfLine => extendSelectionToStartOfLine.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection to End of Line command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionToEndOfLine => extendSelectionToEndOfLine.Value;
        
        /// <summary>
        /// Gets the value that represents the Extend Selection to Start of Text command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionToStartOfText => extendSelectionToStartOfText.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection to End of Text command.
        /// </summary>
        public static RoutedUICommand ExtendSelectionToEndOfText => extendSelectionToEndOfText.Value;

        /// <summary>
        /// Gets the value that represents the Extend Selection to Page Up command.
        /// </summary>
        public static RoutedUICommand SelectToPageUp => ComponentCommands.SelectToPageUp;

        /// <summary>
        /// Gets the value that represents the Extend Selection to Page Down command.
        /// </summary>
        public static RoutedUICommand SelectToPageDown => ComponentCommands.SelectToPageDown;
                
        /// <summary>
        /// Gets the value that represents the Toggle Insertion Mode command.
        /// </summary>
        public static RoutedUICommand ToggleInsertionMode => toggleInsertionMode.Value;

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();

            switch (name)
            {
                case nameof(MoveToStartOfLine):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.None, "Home"));
                    break;

                case nameof(MoveToEndOfLine):
                    gestures.Add(new KeyGesture(Key.End, ModifierKeys.None, "End"));
                    break;

                case nameof(MoveToStartOfText):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));
                    break;

                case nameof(MoveToEndOfText):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+End"));
                    break;

                case nameof(ExtendSelectionToStartOfLine):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Shift, "Shift+Home"));
                    break;

                case nameof(ExtendSelectionToEndOfLine):
                    gestures.Add(new KeyGesture(Key.End, ModifierKeys.Shift, "Shift+End"));
                    break;

                case nameof(ExtendSelectionToStartOfText):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Home"));
                    break;

                case nameof(ExtendSelectionToEndOfText):
                    gestures.Add(new KeyGesture(Key.Home, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+End"));
                    break;
                    
                case nameof(ToggleInsertionMode):
                    gestures.Add(new KeyGesture(Key.Insert, ModifierKeys.None, "Insert"));
                    break;
            }

            return gestures;
        }

        // Property values.
        private static Lazy<RoutedUICommand> moveToStartOfLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_MOVE_TO_START_OF_LINE", nameof(MoveToStartOfLine), typeof(TextEditorCommands), GetInputGestures(nameof(MoveToStartOfLine))));
        private static Lazy<RoutedUICommand> moveToEndOfLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_MOVE_TO_END_OF_LINE", nameof(MoveToEndOfLine), typeof(TextEditorCommands), GetInputGestures(nameof(MoveToEndOfLine))));
        private static Lazy<RoutedUICommand> moveToStartOfText = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_MOVE_TO_START_OF_TEXT", nameof(MoveToStartOfText), typeof(TextEditorCommands), GetInputGestures(nameof(MoveToStartOfText))));
        private static Lazy<RoutedUICommand> moveToEndOfText = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_MOVE_TO_END_OF_TEXT", nameof(MoveToEndOfText), typeof(TextEditorCommands), GetInputGestures(nameof(MoveToEndOfText))));
        private static Lazy<RoutedUICommand> extendSelectionToStartOfLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_EXTEND_SELECTION_TO_START_OF_LINE", nameof(ExtendSelectionToStartOfLine), typeof(TextEditorCommands), GetInputGestures(nameof(ExtendSelectionToStartOfLine))));
        private static Lazy<RoutedUICommand> extendSelectionToEndOfLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_EXTEND_SELECTION_TO_END_OF_LINE", nameof(ExtendSelectionToEndOfLine), typeof(TextEditorCommands), GetInputGestures(nameof(ExtendSelectionToEndOfLine))));
        private static Lazy<RoutedUICommand> extendSelectionToStartOfText = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_EXTEND_SELECTION_TO_START_OF_TEXT", nameof(ExtendSelectionToStartOfText), typeof(TextEditorCommands), GetInputGestures(nameof(ExtendSelectionToStartOfText))));
        private static Lazy<RoutedUICommand> extendSelectionToEndOfText = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_EXTEND_SELECTION_TO_END_OF_TEXT", nameof(ExtendSelectionToEndOfText), typeof(TextEditorCommands), GetInputGestures(nameof(ExtendSelectionToEndOfText))));
        private static Lazy<RoutedUICommand> toggleInsertionMode = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("TEXT_EDITOR_COMMAND_TOGGLE_INSERTION_MODE", nameof(ToggleInsertionMode), typeof(TextEditorCommands), GetInputGestures(nameof(ToggleInsertionMode))));
    }
}
