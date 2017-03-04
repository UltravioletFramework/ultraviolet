using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
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

        // Property values.
        private static Lazy<RoutedUICommand> cut = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CUT", nameof(Cut), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> copy = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_COPY", nameof(Copy), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> paste = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PASTE", nameof(Paste), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> undo = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_UNDO", nameof(Undo), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> redo = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_REDO", nameof(Redo), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> delete = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_DELETE", nameof(Delete), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> find = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_FIND", nameof(Find), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> replace = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_REPLACE", nameof(Replace), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> help = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_HELP", nameof(Help), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> selectAll = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SELECT_ALL", nameof(SelectAll), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> @new = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_NEW", nameof(New), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> open = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_OPEN", nameof(Open), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> save = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SAVE", nameof(Save), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> saveAs = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_SAVE_AS", nameof(SaveAs), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> print = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PRINT", nameof(Print), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> cancelPrint = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CANCEL_PRINT", nameof(cancelPrint), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> printPreview = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PRINT_PREVIEW", nameof(PrintPreview), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> close = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CLOSE", nameof(Close), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> properties = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_PROPERTIES", nameof(Properties), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> contextMenu = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CONTEXT_MENU", nameof(ContextMenu), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> correctionList = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_CORRECTION_LIST", nameof(CorrectionList), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> stop = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_STOP", nameof(Stop), typeof(ApplicationCommands)));
        private static Lazy<RoutedUICommand> notACommand = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("APPLICATION_COMMAND_NOT_A_COMMAND", nameof(NotACommand), typeof(ApplicationCommands)));
    }
}
