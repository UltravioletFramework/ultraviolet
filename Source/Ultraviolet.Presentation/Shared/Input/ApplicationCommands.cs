using System;
using Ultraviolet.Input;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Contains a standard set of application commands.
    /// </summary>
    public static class ApplicationCommands
    {
        /// <summary>
        /// Gets the value that represents the Cut command.
        /// </summary>
        public static RoutedUICommand Cut => cut.Value;

        /// <summary>
        /// Gets the value that represents the Copy command.
        /// </summary>
        public static RoutedUICommand Copy => copy.Value;

        /// <summary>
        /// Gets the value that represents the Paste command.
        /// </summary>
        public static RoutedUICommand Paste => paste.Value;

        /// <summary>
        /// Gets the value that represents the Undo command.
        /// </summary>
        public static RoutedUICommand Undo => undo.Value;

        /// <summary>
        /// Gets the value that represents the Redo command.
        /// </summary>
        public static RoutedUICommand Redo => redo.Value;

        /// <summary>
        /// Gets the value that represents the Delete command.
        /// </summary>
        public static RoutedUICommand Delete => delete.Value;

        /// <summary>
        /// Gets the value that represents the Find command.
        /// </summary>
        public static RoutedUICommand Find => find.Value;

        /// <summary>
        /// Gets the value that represents the Replace command.
        /// </summary>
        public static RoutedUICommand Replace => replace.Value;

        /// <summary>
        /// Gets the value that represents the Help command.
        /// </summary>
        public static RoutedUICommand Help => help.Value;

        /// <summary>
        /// Gets the value that represents the Select All command.
        /// </summary>
        public static RoutedUICommand SelectAll => selectAll.Value;

        /// <summary>
        /// Gets the value that represents the New command.
        /// </summary>
        public static RoutedUICommand New => @new.Value;

        /// <summary>
        /// Gets the value that represents the Open command.
        /// </summary>
        public static RoutedUICommand Open => open.Value;

        /// <summary>
        /// Gets the value that represents the Save command.
        /// </summary>
        public static RoutedUICommand Save => save.Value;

        /// <summary>
        /// Gets the value that represents the Save As command.
        /// </summary>
        public static RoutedUICommand SaveAs => saveAs.Value;

        /// <summary>
        /// Gets the value that represents the Print command.
        /// </summary>
        public static RoutedUICommand Print => print.Value;

        /// <summary>
        /// Gets the value that represents the Cancel Print command.
        /// </summary>
        public static RoutedUICommand CancelPrint => cancelPrint.Value;

        /// <summary>
        /// Gets the value that represents the Print Preview command.
        /// </summary>
        public static RoutedUICommand PrintPreview => printPreview.Value;

        /// <summary>
        /// Gets the value that represents the Close command.
        /// </summary>
        public static RoutedUICommand Close => close.Value;

        /// <summary>
        /// Gets the value that represents the Properties command.
        /// </summary>
        public static RoutedUICommand Properties => properties.Value;

        /// <summary>
        /// Gets the value that represents the Context Menu command.
        /// </summary>
        public static RoutedUICommand ContextMenu => contextMenu.Value;

        /// <summary>
        /// Gets the value that represents the Correction List command.
        /// </summary>
        public static RoutedUICommand CorrectionList => correctionList.Value;

        /// <summary>
        /// Gets the value that represents the Stop command.
        /// </summary>
        public static RoutedUICommand Stop => stop.Value;

        /// <summary>
        /// Represents a command which is always ignored.
        /// </summary>
        public static RoutedUICommand NotACommand => notACommand.Value;

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();

            switch (name)
            {
                case nameof(Cut):
                    gestures.Add(new KeyGesture(Key.X, ModifierKeys.Control, "Ctrl+X"));
                    gestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Shift, "Shift+Delete"));
                    break;

                case nameof(Copy):
                    gestures.Add(new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C"));
                    gestures.Add(new KeyGesture(Key.Insert, ModifierKeys.Control, "Ctrl+Insert"));
                    break;

                case nameof(Paste):
                    gestures.Add(new KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V"));
                    gestures.Add(new KeyGesture(Key.Insert, ModifierKeys.Shift, "Shift+Insert"));
                    break;

                case nameof(Delete):
                    gestures.Add(new KeyGesture(Key.Delete, ModifierKeys.None, "Del"));
                    break;

                case nameof(Undo):
                    gestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control, "Ctrl+Z"));
                    break;

                case nameof(Redo):
                    gestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control, "Ctrl+Y"));
                    break;

                case nameof(Find):
                    gestures.Add(new KeyGesture(Key.F, ModifierKeys.Control, "Ctrl+F"));
                    break;

                case nameof(Replace):
                    gestures.Add(new KeyGesture(Key.H, ModifierKeys.Control, "Ctrl+H"));
                    break;

                case nameof(SelectAll):
                    gestures.Add(new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl+A"));
                    break;

                case nameof(Help):
                    gestures.Add(new KeyGesture(Key.F1, ModifierKeys.None, "F1"));
                    break;

                case nameof(New):
                    gestures.Add(new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N"));
                    break;

                case nameof(Open):
                    gestures.Add(new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl+O"));
                    break;

                case nameof(Save):
                    gestures.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));
                    break;

                case nameof(Print):
                    gestures.Add(new KeyGesture(Key.P, ModifierKeys.Control, "Ctrl+P"));
                    break;

                case nameof(PrintPreview):
                    gestures.Add(new KeyGesture(Key.F2, ModifierKeys.Control, "Ctrl+F2"));
                    break;

                case nameof(Properties):
                    gestures.Add(new KeyGesture(Key.F4, ModifierKeys.None, "F4"));
                    break;

                case nameof(ContextMenu):
                    gestures.Add(new KeyGesture(Key.F10, ModifierKeys.Shift, "Shift+F10"));
                    gestures.Add(new KeyGesture(Key.Application, ModifierKeys.None, "Apps"));
                    break;

                case nameof(Stop):
                    gestures.Add(new KeyGesture(Key.Escape, ModifierKeys.None, "Esc"));
                    break;
            }

            return gestures;
        }

        // Property values.
        private static Lazy<RoutedUICommand> cut = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CUT", nameof(Cut), typeof(ApplicationCommands), GetInputGestures(nameof(Cut))));
        private static Lazy<RoutedUICommand> copy = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_COPY", nameof(Copy), typeof(ApplicationCommands), GetInputGestures(nameof(Copy))));
        private static Lazy<RoutedUICommand> paste = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PASTE", nameof(Paste), typeof(ApplicationCommands), GetInputGestures(nameof(Paste))));
        private static Lazy<RoutedUICommand> undo = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_UNDO", nameof(Undo), typeof(ApplicationCommands), GetInputGestures(nameof(Undo))));
        private static Lazy<RoutedUICommand> redo = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_REDO", nameof(Redo), typeof(ApplicationCommands), GetInputGestures(nameof(Redo))));
        private static Lazy<RoutedUICommand> delete = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_DELETE", nameof(Delete), typeof(ApplicationCommands), GetInputGestures(nameof(Delete))));
        private static Lazy<RoutedUICommand> find = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_FIND", nameof(Find), typeof(ApplicationCommands), GetInputGestures(nameof(Find))));
        private static Lazy<RoutedUICommand> replace = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_REPLACE", nameof(Replace), typeof(ApplicationCommands), GetInputGestures(nameof(Replace))));
        private static Lazy<RoutedUICommand> help = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_HELP", nameof(Help), typeof(ApplicationCommands), GetInputGestures(nameof(Help))));
        private static Lazy<RoutedUICommand> selectAll = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SELECT_ALL", nameof(SelectAll), typeof(ApplicationCommands), GetInputGestures(nameof(SelectAll))));
        private static Lazy<RoutedUICommand> @new = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_NEW", nameof(New), typeof(ApplicationCommands), GetInputGestures(nameof(New))));
        private static Lazy<RoutedUICommand> open = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_OPEN", nameof(Open), typeof(ApplicationCommands), GetInputGestures(nameof(Open))));
        private static Lazy<RoutedUICommand> save = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SAVE", nameof(Save), typeof(ApplicationCommands), GetInputGestures(nameof(Save))));
        private static Lazy<RoutedUICommand> saveAs = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SAVE_AS", nameof(SaveAs), typeof(ApplicationCommands), GetInputGestures(nameof(SaveAs))));
        private static Lazy<RoutedUICommand> print = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PRINT", nameof(Print), typeof(ApplicationCommands), GetInputGestures(nameof(Print))));
        private static Lazy<RoutedUICommand> cancelPrint = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CANCEL_PRINT", nameof(CancelPrint), typeof(ApplicationCommands), GetInputGestures(nameof(CancelPrint))));
        private static Lazy<RoutedUICommand> printPreview = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PRINT_PREVIEW", nameof(PrintPreview), typeof(ApplicationCommands), GetInputGestures(nameof(PrintPreview))));
        private static Lazy<RoutedUICommand> close = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CLOSE", nameof(Close), typeof(ApplicationCommands), GetInputGestures(nameof(Close))));
        private static Lazy<RoutedUICommand> properties = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PROPERTIES", nameof(Properties), typeof(ApplicationCommands), GetInputGestures(nameof(Properties))));
        private static Lazy<RoutedUICommand> contextMenu = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CONTEXT_MENU", nameof(ContextMenu), typeof(ApplicationCommands), GetInputGestures(nameof(ContextMenu))));
        private static Lazy<RoutedUICommand> correctionList = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CORRECTION_LIST", nameof(CorrectionList), typeof(ApplicationCommands), GetInputGestures(nameof(CorrectionList))));
        private static Lazy<RoutedUICommand> stop = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_STOP", nameof(Stop), typeof(ApplicationCommands), GetInputGestures(nameof(Stop))));
        private static Lazy<RoutedUICommand> notACommand = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_NOT_A_COMMAND", nameof(NotACommand), typeof(ApplicationCommands), GetInputGestures(nameof(NotACommand))));
    }
}
