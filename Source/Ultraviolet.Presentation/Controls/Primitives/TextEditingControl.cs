using System;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Documents;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a control which hosts a text editor.
    /// </summary>
    [UvmlKnownType]
    public abstract class TextEditingControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditingControl"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextEditingControl(UltravioletContext uv, String name)
            : base(uv, name)
        {
            var canExecuteIsEditable = new UpfCanExecuteRoutedEventHandler(CanExecuteIsEditable);
            var canExecuteIsCaretVisible = new UpfCanExecuteRoutedEventHandler(CanExecuteIsCaretVisible);

            // Selection commands
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), ApplicationCommands.SelectAll,
                ExecutedSelectAll, canExecuteIsCaretVisible, new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl+A"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectRightByCharacter,
                ExecutedSelectRightByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Shift, "Shift+Right"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectLeftByCharacter,
                ExecutedSelectLeftByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Shift, "Shift+Left"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectRightByWord,
                ExecutedSelectRightByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Right"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectLeftByWord,
                ExecutedSelectLeftByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Left"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectDownByLine,
                ExecutedSelectDownByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Down, ModifierKeys.Shift, "Shift+Down"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectUpByLine,
                ExecutedSelectUpByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Up, ModifierKeys.Shift, "Shift+Up"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectDownByPage,
                ExecutedSelectDownByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageDown, ModifierKeys.Shift, "Shift+PageDown"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectUpByPage,
                ExecutedSelectUpByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageUp, ModifierKeys.Shift, "Shift+PageUp"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectToLineStart,
                ExecutedSelectToLineStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Shift, "Shift+Home"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectToLineEnd,
                ExecutedSelectToLineEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Shift, "Shift+End"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectToDocumentStart,
                ExecutedSelectToDocumentStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Home"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.SelectToDocumentEnd,
                ExecutedSelectToDocumentEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+End"));

            // Movement commands
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveRightByCharacter,
                ExecutedMoveRightByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.None, "Right"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveLeftByCharacter,
                ExecutedMoveLeftByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.None, "Left"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveRightByWord,
                ExecutedMoveRightByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Control, "Ctrl+Right"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveLeftByWord,
                ExecutedMoveLeftByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Control, "Ctrl+Left"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveDownByLine,
                ExecutedMoveDownByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Down, ModifierKeys.None, "Down"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveUpByLine,
                ExecutedMoveUpByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Up, ModifierKeys.None, "Up"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveDownByPage,
                ExecutedMoveDownByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveUpByPage,
                ExecutedMoveUpByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveToLineStart,
                ExecutedMoveToLineStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.None, "Home"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveToLineEnd,
                ExecutedMoveToLineEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveToDocumentStart,
                ExecutedMoveToDocumentStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.MoveToDocumentEnd,
                ExecutedMoveToDocumentEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Control, "Ctrl+End"));

            // Text editing commands
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.ToggleInsert,
                ExecutedToggleInsert, canExecuteIsCaretVisible, new KeyGesture(Key.Insert, ModifierKeys.None, "Insert"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.Backspace,
                ExecutedBackspace, canExecuteIsEditable, new KeyGesture(Key.Backspace, ModifierKeys.None, "Backspace"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.Delete,
                ExecutedDelete, canExecuteIsEditable, new KeyGesture(Key.Delete, ModifierKeys.None, "Delete"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.DeleteNextWord,
                ExecutedDeleteNextWord, canExecuteIsEditable, new KeyGesture(Key.Delete, ModifierKeys.Control, "Ctrl+Delete"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.DeletePreviousWord,
                ExecutedDeletePreviousWord, canExecuteIsEditable, new KeyGesture(Key.Backspace, ModifierKeys.Control, "Ctrl+Backspace"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.EnterParagraphBreak,
                ExecutedEnterParagraphBreak, CanExecuteEnterParagraphBreak, new KeyGesture(Key.Return, ModifierKeys.None, "Return"), new KeyGesture(Key.KeypadEnter, ModifierKeys.None, "KeypadEnter"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.EnterLineBreak,
                ExecutedEnterLineBreak, CanExecuteEnterLineBreak, new KeyGesture(Key.Return, ModifierKeys.Shift, "Shift+Return"), new KeyGesture(Key.KeypadEnter, ModifierKeys.Shift, "Shift+KeypadEnter"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.TabForward,
                ExecutedTabForward, CanExecuteTabForward, new KeyGesture(Key.Tab, ModifierKeys.None, "Tab"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), EditingCommands.TabBackward,
                ExecutedTabBackward, CanExecuteTabBackward, new KeyGesture(Key.Tab, ModifierKeys.Shift, "Shift+Tab"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), ApplicationCommands.Copy,
                ExecutedCopy, CanExecuteCopy, new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), ApplicationCommands.Cut,
                ExecutedCut, CanExecuteCut, new KeyGesture(Key.X, ModifierKeys.Control, "Ctrl+X"));
            CommandManager.RegisterClassBindings(typeof(TextEditingControl), ApplicationCommands.Paste,
                ExecutedPaste, CanExecutePaste, new KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V"));
        }
        
        /// <summary>
        /// Gets the control's text editor.
        /// </summary>
        protected abstract TextEditor TextEditor { get; }
        
        /// <summary>
        /// Executes the <see cref="ApplicationCommands.SelectAll"/> command.
        /// </summary>
        private static void ExecutedSelectAll(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.SelectAll();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectRightByCharacter"/> command.
        /// </summary>
        private static void ExecutedSelectRightByCharacter(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretLeft(true);
            else
                textEditor.MoveCaretRight(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectLeftByCharacter"/> command.
        /// </summary>
        private static void ExecutedSelectLeftByCharacter(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretRight(true);
            else
                textEditor.MoveCaretLeft(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectRightByWord"/> command.
        /// </summary>
        private static void ExecutedSelectRightByWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretToPreviousWord(true);
            else
                textEditor.MoveCaretToNextWord(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectLeftByWord"/> command.
        /// </summary>
        private static void ExecutedSelectLeftByWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretToNextWord(true);
            else
                textEditor.MoveCaretToPreviousWord(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectUpByLine"/> command.
        /// </summary>
        private static void ExecutedSelectDownByLine(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretDown(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectDownByLine"/> command.
        /// </summary>
        private static void ExecutedSelectUpByLine(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretUp(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectDownByPage"/> command.
        /// </summary>
        private static void ExecutedSelectDownByPage(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretPageDown(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectUpByPage"/> command.
        /// </summary>
        private static void ExecutedSelectUpByPage(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretPageUp(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToLineStart"/> command.
        /// </summary>
        private static void ExecutedSelectToLineStart(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToHome(true, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToLineEnd"/> command.
        /// </summary>
        private static void ExecutedSelectToLineEnd(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToEnd(true, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToDocumentStart"/> command.
        /// </summary>
        private static void ExecutedSelectToDocumentStart(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToHome(true, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToDocumentEnd"/> command.
        /// </summary>
        private static void ExecutedSelectToDocumentEnd(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToEnd(true, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveRightByCharacter"/> command.
        /// </summary>
        private static void ExecutedMoveRightByCharacter(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretLeft(false);
            else
                textEditor.MoveCaretRight(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveLeftByCharacter"/> command.
        /// </summary>
        private static void ExecutedMoveLeftByCharacter(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretRight(false);
            else
                textEditor.MoveCaretLeft(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveRightByWord"/> command.
        /// </summary>
        private static void ExecutedMoveRightByWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretToPreviousWord(false);
            else
                textEditor.MoveCaretToNextWord(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveLeftByWord"/> command.
        /// </summary>
        private static void ExecutedMoveLeftByWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            if (textEditor.IsRightToLeft())
                textEditor.MoveCaretToNextWord(false);
            else
                textEditor.MoveCaretToPreviousWord(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveDownByLine"/> command.
        /// </summary>
        private static void ExecutedMoveDownByLine(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretDown(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveUpByLine"/> command.
        /// </summary>
        private static void ExecutedMoveUpByLine(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretUp(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveDownByPage"/> command.
        /// </summary>
        private static void ExecutedMoveDownByPage(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretPageDown(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveUpByPage"/> command.
        /// </summary>
        private static void ExecutedMoveUpByPage(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretPageUp(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToLineStart"/> command.
        /// </summary>
        private static void ExecutedMoveToLineStart(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToHome(false, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToLineEnd"/> command.
        /// </summary>
        private static void ExecutedMoveToLineEnd(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToEnd(false, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToDocumentStart"/> command.
        /// </summary>
        private static void ExecutedMoveToDocumentStart(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToHome(false, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToDocumentEnd"/> command.
        /// </summary>
        private static void ExecutedMoveToDocumentEnd(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor.IsEnabled)
                return;

            textEditor.MoveCaretToEnd(false, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.ToggleInsert"/> command.
        /// </summary>
        private static void ExecutedToggleInsert(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.ToggleInsertionMode();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.Backspace"/> command.
        /// </summary>
        private static void ExecutedBackspace(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.Backspace();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.Delete"/> command.
        /// </summary>
        private static void ExecutedDelete(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.Delete();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.DeleteNextWord"/> command.
        /// </summary>
        private static void ExecutedDeleteNextWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.DeleteNextWord();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.DeletePreviousWord"/> command.
        /// </summary>
        private static void ExecutedDeletePreviousWord(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.DeletePreviousWord();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.EnterParagraphBreak"/> command.
        /// </summary>
        private static void ExecutedEnterParagraphBreak(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.InsertNewLine();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.EnterLineBreak"/> command.
        /// </summary>
        private static void ExecutedEnterLineBreak(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.InsertNewLine();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.TabForward"/> command.
        /// </summary>
        private static void ExecutedTabForward(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.InsertTab();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.TabBackward"/> command.
        /// </summary>
        private static void ExecutedTabBackward(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.InsertTab();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Copy"/> command.
        /// </summary>
        private static void ExecutedCopy(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsMasked || textEditor.SelectionLength == 0 || !textEditor.IsEnabled)
                return;

            textEditor.Copy();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Cut"/> command.
        /// </summary>
        private static void ExecutedCut(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsMasked || textEditor.SelectionLength == 0 || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.Cut();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Paste"/> command.
        /// </summary>
        private static void ExecutedPaste(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            textEditor.Paste();
        }

        /// <summary>
        /// Specifies that a command can execute if the text box is enabled and editable.
        /// </summary>
        private static void CanExecuteIsEditable(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || textEditor.IsReadOnly || !textEditor.IsEnabled)
                return;

            data.CanExecute = true;
        }

        /// <summary>
        /// Specifies that a command can execute if the text box is enabled and the caret is visible.
        /// </summary>
        private static void CanExecuteIsCaretVisible(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null || !textEditor.IsEnabled)
                return;

            if (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible)
                return;

            data.CanExecute = true;
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.EnterLineBreak"/> command can execute.
        /// </summary>
        private static void CanExecuteEnterLineBreak(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor != null && textEditor.AcceptsReturn && textEditor.IsEnabled && !textEditor.IsReadOnly)
            {
                data.CanExecute = true;
            }
            else
            {
                data.ContinueRouting = true;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.EnterParagraphBreak"/> command can execute.
        /// </summary>
        private static void CanExecuteEnterParagraphBreak(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor != null && textEditor.AcceptsReturn && textEditor.IsEnabled && !textEditor.IsReadOnly)
            {
                data.CanExecute = true;
            }
            else
            {
                data.ContinueRouting = true;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.TabForward"/> command can execute.
        /// </summary>
        private static void CanExecuteTabForward(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor != null && textEditor.AcceptsTab && textEditor.IsEnabled)
            {
                data.CanExecute = true;
            }
            else
            {
                data.ContinueRouting = true;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.TabBackward"/> command can execute.
        /// </summary>
        private static void CanExecuteTabBackward(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor != null && textEditor.AcceptsTab && textEditor.IsEnabled)
            {
                data.CanExecute = true;
            }
            else
            {
                data.ContinueRouting = true;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Copy"/> command can execute.
        /// </summary>
        private static void CanExecuteCopy(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null)
                return;

            if (textEditor.IsMasked)
            {
                data.CanExecute = false;
                data.Handled = true;
                return;
            }

            data.CanExecute = textEditor.IsEnabled && textEditor.SelectionLength > 0;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Cut"/> command can execute.
        /// </summary>
        private static void CanExecuteCut(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null)
                return;

            if (textEditor.IsMasked)
            {
                data.CanExecute = false;
                data.Handled = true;
                return;
            }
            
            data.CanExecute = textEditor.IsEnabled && !textEditor.IsReadOnly && textEditor.SelectionLength > 0;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Paste"/> command can execute.
        /// </summary>
        private static void CanExecutePaste(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textEditor = ((TextEditingControl)element).TextEditor;
            if (textEditor == null)
                return;

            data.CanExecute = textEditor.IsEnabled && !textEditor.IsReadOnly;
            data.Handled = true;
        }
    }
}
