using System;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Documents
{
    /// <summary>
    /// Contains commands used to edit text.
    /// </summary>
    public static class EditingCommands
    {
        /// <summary>
        /// Gets the value that represents the Toggle Insert command.
        /// </summary>
        public static RoutedUICommand ToggleInsert => toggleInsert.Value;

        /// <summary>
        /// Gets the value that represents the Delete command.
        /// </summary>
        public static RoutedUICommand Delete => delete.Value;

        /// <summary>
        /// Gets the value that represents the Backspace command.
        /// </summary>
        public static RoutedUICommand Backspace => backspace.Value;

        /// <summary>
        /// Gets the value that represents the Delete Next Word command.
        /// </summary>
        public static RoutedUICommand DeleteNextWord => deleteNextWord.Value;

        /// <summary>
        /// Gets the value that represents the Delete Previous Word command.
        /// </summary>
        public static RoutedUICommand DeletePreviousWord => deletePreviousWord.Value;

        /// <summary>
        /// Gets the value that represents the Enter Paragraph Break command.
        /// </summary>
        public static RoutedUICommand EnterParagraphBreak => enterParagraphBreak.Value;

        /// <summary>
        /// Gets the value that represents the Enter Line Break command.
        /// </summary>
        public static RoutedUICommand EnterLineBreak => enterLineBreak.Value;

        /// <summary>
        /// Gets the value that represents the Tab Forward command.
        /// </summary>
        public static RoutedUICommand TabForward => tabForward.Value;

        /// <summary>
        /// Gets the value that represents the Tab Backward command.
        /// </summary>
        public static RoutedUICommand TabBackward => tabBackward.Value;

        /// <summary>
        /// Gets the value that represents the Move Right by Character command.
        /// </summary>
        public static RoutedUICommand MoveRightByCharacter => moveRightByCharacter.Value;

        /// <summary>
        /// Gets the value that represents the Move Left by Character command.
        /// </summary>
        public static RoutedUICommand MoveLeftByCharacter => moveLeftByCharacter.Value;

        /// <summary>
        /// Gets the value that represents the Move Right by Word command.
        /// </summary>
        public static RoutedUICommand MoveRightByWord => moveRightByWord.Value;

        /// <summary>
        /// Gets the value that represents the Move Left by Word command.
        /// </summary>
        public static RoutedUICommand MoveLeftByWord => moveLeftByWord.Value;

        /// <summary>
        /// Gets the value that represents the Move Down by Line command.
        /// </summary>
        public static RoutedUICommand MoveDownByLine => moveDownByLine.Value;

        /// <summary>
        /// Gets the value that represents the Move Up by Line command.
        /// </summary>
        public static RoutedUICommand MoveUpByLine => moveUpByLine.Value;

        /// <summary>
        /// Gets the value that represents the Move Down by Paragraph command.
        /// </summary>
        public static RoutedUICommand MoveDownByParagraph => moveDownByParagraph.Value;

        /// <summary>
        /// Gets the value that represents the Move Up by Paragraph command.
        /// </summary>
        public static RoutedUICommand MoveUpByParagraph => moveUpByParagraph.Value;

        /// <summary>
        /// Gets the value that represents the Move Down by Page command.
        /// </summary>
        public static RoutedUICommand MoveDownByPage => moveDownByPage.Value;

        /// <summary>
        /// Gets the value that represents the Move Up by Page command.
        /// </summary>
        public static RoutedUICommand MoveUpByPage => moveUpByPage.Value;

        /// <summary>
        /// Gets the value that represents the Move to Line Start command.
        /// </summary>
        public static RoutedUICommand MoveToLineStart => moveToLineStart.Value;

        /// <summary>
        /// Gets the value that represents the Move to Line End command.
        /// </summary>
        public static RoutedUICommand MoveToLineEnd => moveToLineEnd.Value;

        /// <summary>
        /// Gets the value that represents the Move to Document Start command.
        /// </summary>
        public static RoutedUICommand MoveToDocumentStart => moveToDocumentStart.Value;

        /// <summary>
        /// Gets the value that represents the Move to Document End command.
        /// </summary>
        public static RoutedUICommand MoveToDocumentEnd => moveToDocumentEnd.Value;
        
        /// <summary>
        /// Gets the value that represents the Select Right by Character command.
        /// </summary>
        public static RoutedUICommand SelectRightByCharacter => selectRightByCharacter.Value;

        /// <summary>
        /// Gets the value that represents the Select Left by Character command.
        /// </summary>
        public static RoutedUICommand SelectLeftByCharacter => selectLeftByCharacter.Value;

        /// <summary>
        /// Gets the value that represents the Select Right by Word command.
        /// </summary>
        public static RoutedUICommand SelectRightByWord => selectRightByWord.Value;

        /// <summary>
        /// Gets the value that represents the Select Left by Word command.
        /// </summary>
        public static RoutedUICommand SelectLeftByWord => selectLeftByWord.Value;

        /// <summary>
        /// Gets the value that represents the Select Down by Line command.
        /// </summary>
        public static RoutedUICommand SelectDownByLine => selectDownByLine.Value;

        /// <summary>
        /// Gets the value that represents the Select Up by Line command.
        /// </summary>
        public static RoutedUICommand SelectUpByLine => selectUpByLine.Value;

        /// <summary>
        /// Gets the value that represents the Select Down by Paragraph command.
        /// </summary>
        public static RoutedUICommand SelectDownByParagraph => selectDownByParagraph.Value;

        /// <summary>
        /// Gets the value that represents the Select Up by Paragraph command.
        /// </summary>
        public static RoutedUICommand SelectUpByParagraph => selectUpByParagraph.Value;

        /// <summary>
        /// Gets the value that represents the Select Down by Page command.
        /// </summary>
        public static RoutedUICommand SelectDownByPage => selectDownByPage.Value;

        /// <summary>
        /// Gets the value that represents the Select Up by Page command.
        /// </summary>
        public static RoutedUICommand SelectUpByPage => selectUpByPage.Value;

        /// <summary>
        /// Gets the value that represents the Select to Line Start command.
        /// </summary>
        public static RoutedUICommand SelectToLineStart => selectToLineStart.Value;

        /// <summary>
        /// Gets the value that represents the Select to Line End command.
        /// </summary>
        public static RoutedUICommand SelectToLineEnd => selectToLineEnd.Value;

        /// <summary>
        /// Gets the value that represents the Select to Document Start command.
        /// </summary>
        public static RoutedUICommand SelectToDocumentStart => selectToDocumentStart.Value;

        /// <summary>
        /// Gets the value that represents the Select to Document End command.
        /// </summary>
        public static RoutedUICommand SelectToDocumentEnd => selectToDocumentEnd.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Bold command.
        /// </summary>
        public static RoutedUICommand ToggleBold => toggleBold.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Italic command.
        /// </summary>
        public static RoutedUICommand ToggleItalic => toggleItalic.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Underline command.
        /// </summary>
        public static RoutedUICommand ToggleUnderline => toggleUnderline.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Subscript command.
        /// </summary>
        public static RoutedUICommand ToggleSubscript => toggleSubscript.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Superscript command.
        /// </summary>
        public static RoutedUICommand ToggleSuperscript => toggleSuperscript.Value;

        /// <summary>
        /// Gets the value that represents the Increase Font Size command.
        /// </summary>
        public static RoutedUICommand IncreaseFontSize => increaseFontSize.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Font Size command.
        /// </summary>
        public static RoutedUICommand DecreaseFontSize => decreaseFontSize.Value;

        /// <summary>
        /// Gets the value that represents the Align Left command.
        /// </summary>
        public static RoutedUICommand AlignLeft => alignLeft.Value;

        /// <summary>
        /// Gets the value that represents the Align Center command.
        /// </summary>
        public static RoutedUICommand AlignCenter => alignCenter.Value;

        /// <summary>
        /// Gets the value that represents the Align Right command.
        /// </summary>
        public static RoutedUICommand AlignRight => alignRight.Value;

        /// <summary>
        /// Gets the value that represents the Align Justify command.
        /// </summary>
        public static RoutedUICommand AlignJustify => alignJustify.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Bullets command.
        /// </summary>
        public static RoutedUICommand ToggleBullets => toggleBullets.Value;

        /// <summary>
        /// Gets the value that represents the Toggle Numbering command.
        /// </summary>
        public static RoutedUICommand ToggleNumbering => toggleNumbering.Value;

        /// <summary>
        /// Gets the value that represents the Increase Indentation command.
        /// </summary>
        public static RoutedUICommand IncreaseIndentation => increaseIndentation.Value;

        /// <summary>
        /// Gets the value that represents the Decrease Indentation command.
        /// </summary>
        public static RoutedUICommand DecreaseIndentation => decreaseIndentation.Value;

        /// <summary>
        /// Gets the value that represents the Correct Spelling Error command.
        /// </summary>
        public static RoutedUICommand CorrectSpellingError => correctSpellingError.Value;

        /// <summary>
        /// Gets the value that represents the IgnoreSpellingError command.
        /// </summary>
        public static RoutedUICommand IgnoreSpellingError => ignoreSpellingError.Value;

        /// <summary>
        /// Gets the collection of default gestures for the specified command.
        /// </summary>
        private static InputGestureCollection GetInputGestures(String name)
        {
            var gestures = new InputGestureCollection();
            return gestures;
        }

        // Property values.
        private static Lazy<RoutedUICommand> toggleInsert = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_INSERT", nameof(ToggleInsert), typeof(EditingCommands), GetInputGestures(nameof(ToggleInsert))));
        private static Lazy<RoutedUICommand> delete = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_DELETE", nameof(Delete), typeof(EditingCommands), GetInputGestures(nameof(Delete))));
        private static Lazy<RoutedUICommand> backspace = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_BACKSPACE", nameof(Backspace), typeof(EditingCommands), GetInputGestures(nameof(Backspace))));
        private static Lazy<RoutedUICommand> deleteNextWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_DELETE_NEXT_WORD", nameof(DeleteNextWord), typeof(EditingCommands), GetInputGestures(nameof(DeleteNextWord))));
        private static Lazy<RoutedUICommand> deletePreviousWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_DELETE_PREVIOUS_WORD", nameof(DeletePreviousWord), typeof(EditingCommands), GetInputGestures(nameof(DeletePreviousWord))));
        private static Lazy<RoutedUICommand> enterParagraphBreak = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ENTER_PARAGRAPH_BREAK", nameof(EnterParagraphBreak), typeof(EditingCommands), GetInputGestures(nameof(EnterParagraphBreak))));
        private static Lazy<RoutedUICommand> enterLineBreak = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ENTER_LINE_BREAK", nameof(EnterLineBreak), typeof(EditingCommands), GetInputGestures(nameof(EnterLineBreak))));
        private static Lazy<RoutedUICommand> tabForward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TAB_FORWARD", nameof(TabForward), typeof(EditingCommands), GetInputGestures(nameof(TabForward))));
        private static Lazy<RoutedUICommand> tabBackward = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TAB_BACKWARD", nameof(TabBackward), typeof(EditingCommands), GetInputGestures(nameof(TabBackward))));
        private static Lazy<RoutedUICommand> moveRightByCharacter = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_RIGHT_BY_CHARACTER", nameof(MoveRightByCharacter), typeof(EditingCommands), GetInputGestures(nameof(MoveRightByCharacter))));
        private static Lazy<RoutedUICommand> moveLeftByCharacter = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_LEFT_BY_CHARACTER", nameof(MoveLeftByCharacter), typeof(EditingCommands), GetInputGestures(nameof(MoveLeftByCharacter))));
        private static Lazy<RoutedUICommand> moveRightByWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_RIGHT_BY_WORD", nameof(MoveRightByWord), typeof(EditingCommands), GetInputGestures(nameof(MoveRightByWord))));
        private static Lazy<RoutedUICommand> moveLeftByWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_LEFT_BY_WORD", nameof(MoveLeftByWord), typeof(EditingCommands), GetInputGestures(nameof(MoveLeftByWord))));
        private static Lazy<RoutedUICommand> moveDownByLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_DOWN_BY_LINE", nameof(MoveDownByLine), typeof(EditingCommands), GetInputGestures(nameof(MoveDownByLine))));
        private static Lazy<RoutedUICommand> moveUpByLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_UP_BY_LINE", nameof(MoveUpByLine), typeof(EditingCommands), GetInputGestures(nameof(MoveUpByLine))));
        private static Lazy<RoutedUICommand> moveDownByParagraph = new Lazy<RoutedUICommand>(() =>
           new RoutedUICommand("EDITING_COMMAND_MOVE_DOWN_BY_PARAGRAPH", nameof(MoveDownByParagraph), typeof(EditingCommands), GetInputGestures(nameof(MoveDownByParagraph))));
        private static Lazy<RoutedUICommand> moveUpByParagraph = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_UP_BY_PARAGRAPH", nameof(MoveUpByParagraph), typeof(EditingCommands), GetInputGestures(nameof(MoveUpByParagraph))));
        private static Lazy<RoutedUICommand> moveDownByPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_DOWN_BY_PAGE", nameof(MoveDownByPage), typeof(EditingCommands), GetInputGestures(nameof(MoveDownByPage))));
        private static Lazy<RoutedUICommand> moveUpByPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_UP_BY_PAGE", nameof(MoveUpByPage), typeof(EditingCommands), GetInputGestures(nameof(MoveUpByPage))));
        private static Lazy<RoutedUICommand> moveToLineStart = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_TO_LINE_START", nameof(MoveToLineStart), typeof(EditingCommands), GetInputGestures(nameof(MoveToLineStart))));
        private static Lazy<RoutedUICommand> moveToLineEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_TO_LINE_END", nameof(MoveToLineEnd), typeof(EditingCommands), GetInputGestures(nameof(MoveToLineEnd))));
        private static Lazy<RoutedUICommand> moveToDocumentStart = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_TO_DOCUMENT_START", nameof(MoveToDocumentStart), typeof(EditingCommands), GetInputGestures(nameof(MoveToDocumentStart))));
        private static Lazy<RoutedUICommand> moveToDocumentEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_MOVE_TO_DOCUMENT_END", nameof(MoveToDocumentEnd), typeof(EditingCommands), GetInputGestures(nameof(MoveToDocumentEnd))));
        private static Lazy<RoutedUICommand> selectRightByCharacter = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_RIGHT_BY_CHARACTER", nameof(SelectRightByCharacter), typeof(EditingCommands), GetInputGestures(nameof(SelectRightByCharacter))));
        private static Lazy<RoutedUICommand> selectLeftByCharacter = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_LEFT_BY_CHARACTER", nameof(SelectLeftByCharacter), typeof(EditingCommands), GetInputGestures(nameof(SelectLeftByCharacter))));
        private static Lazy<RoutedUICommand> selectRightByWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_RIGHT_BY_WORD", nameof(SelectRightByWord), typeof(EditingCommands), GetInputGestures(nameof(SelectRightByWord))));
        private static Lazy<RoutedUICommand> selectLeftByWord = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_LEFT_BY_WORD", nameof(SelectLeftByWord), typeof(EditingCommands), GetInputGestures(nameof(SelectLeftByWord))));
        private static Lazy<RoutedUICommand> selectDownByLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_DOWN_BY_LINE", nameof(SelectDownByLine), typeof(EditingCommands), GetInputGestures(nameof(SelectDownByLine))));
        private static Lazy<RoutedUICommand> selectUpByLine = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_UP_BY_LINE", nameof(SelectUpByLine), typeof(EditingCommands), GetInputGestures(nameof(SelectUpByLine))));
        private static Lazy<RoutedUICommand> selectDownByParagraph = new Lazy<RoutedUICommand>(() =>
           new RoutedUICommand("EDITING_COMMAND_SELECT_DOWN_BY_PARAGRAPH", nameof(SelectDownByParagraph), typeof(EditingCommands), GetInputGestures(nameof(SelectDownByParagraph))));
        private static Lazy<RoutedUICommand> selectUpByParagraph = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_UP_BY_PARAGRAPH", nameof(SelectUpByParagraph), typeof(EditingCommands), GetInputGestures(nameof(SelectUpByParagraph))));
        private static Lazy<RoutedUICommand> selectDownByPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_DOWN_BY_PAGE", nameof(SelectDownByPage), typeof(EditingCommands), GetInputGestures(nameof(SelectDownByPage))));
        private static Lazy<RoutedUICommand> selectUpByPage = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_UP_BY_PAGE", nameof(SelectUpByPage), typeof(EditingCommands), GetInputGestures(nameof(SelectUpByPage))));
        private static Lazy<RoutedUICommand> selectToLineStart = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_TO_LINE_START", nameof(SelectToLineStart), typeof(EditingCommands), GetInputGestures(nameof(SelectToLineStart))));
        private static Lazy<RoutedUICommand> selectToLineEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_TO_LINE_END", nameof(SelectToLineEnd), typeof(EditingCommands), GetInputGestures(nameof(SelectToLineEnd))));
        private static Lazy<RoutedUICommand> selectToDocumentStart = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_TO_DOCUMENT_START", nameof(SelectToDocumentStart), typeof(EditingCommands), GetInputGestures(nameof(SelectToDocumentStart))));
        private static Lazy<RoutedUICommand> selectToDocumentEnd = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_SELECT_TO_DOCUMENT_END", nameof(SelectToDocumentEnd), typeof(EditingCommands), GetInputGestures(nameof(SelectToDocumentEnd))));
        private static Lazy<RoutedUICommand> toggleBold = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_BOLD", nameof(ToggleBold), typeof(EditingCommands), GetInputGestures(nameof(ToggleBold))));
        private static Lazy<RoutedUICommand> toggleItalic = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_ITALIC", nameof(ToggleItalic), typeof(EditingCommands), GetInputGestures(nameof(ToggleItalic))));
        private static Lazy<RoutedUICommand> toggleUnderline = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_UNDERLINE", nameof(ToggleUnderline), typeof(EditingCommands), GetInputGestures(nameof(ToggleUnderline))));
        private static Lazy<RoutedUICommand> toggleSubscript = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_SUBSCRIPT", nameof(ToggleSubscript), typeof(EditingCommands), GetInputGestures(nameof(ToggleSubscript))));
        private static Lazy<RoutedUICommand> toggleSuperscript = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_SUPERSCRIPT", nameof(ToggleSuperscript), typeof(EditingCommands), GetInputGestures(nameof(ToggleSuperscript))));
        private static Lazy<RoutedUICommand> increaseFontSize = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_INCREASE_FONT_SIZE", nameof(IncreaseFontSize), typeof(EditingCommands), GetInputGestures(nameof(IncreaseFontSize))));
        private static Lazy<RoutedUICommand> decreaseFontSize = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_DECREASE_FONT_SIZE", nameof(DecreaseFontSize), typeof(EditingCommands), GetInputGestures(nameof(DecreaseFontSize))));
        private static Lazy<RoutedUICommand> alignRight = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ALIGN_RIGHT", nameof(AlignRight), typeof(EditingCommands), GetInputGestures(nameof(AlignRight))));
        private static Lazy<RoutedUICommand> alignLeft= new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ALIGN_LEFT", nameof(AlignLeft), typeof(EditingCommands), GetInputGestures(nameof(AlignLeft))));
        private static Lazy<RoutedUICommand> alignCenter = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ALIGN_CENTER", nameof(AlignCenter), typeof(EditingCommands), GetInputGestures(nameof(AlignCenter))));
        private static Lazy<RoutedUICommand> alignJustify = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_ALIGN_JUSTIFY", nameof(AlignJustify), typeof(EditingCommands), GetInputGestures(nameof(AlignJustify))));
        private static Lazy<RoutedUICommand> toggleBullets = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_BULLETS", nameof(ToggleBullets), typeof(EditingCommands), GetInputGestures(nameof(ToggleBullets))));
        private static Lazy<RoutedUICommand> toggleNumbering = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_TOGGLE_NUMBERS", nameof(ToggleNumbering), typeof(EditingCommands), GetInputGestures(nameof(ToggleNumbering))));
        private static Lazy<RoutedUICommand> increaseIndentation = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_INCREASE_INDENTATION", nameof(IncreaseIndentation), typeof(EditingCommands), GetInputGestures(nameof(IncreaseIndentation))));
        private static Lazy<RoutedUICommand> decreaseIndentation = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_DECREASE_INDENTATION", nameof(DecreaseIndentation), typeof(EditingCommands), GetInputGestures(nameof(DecreaseIndentation))));
        private static Lazy<RoutedUICommand> correctSpellingError = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_CORRECT_SPELLING_ERROR", nameof(CorrectSpellingError), typeof(EditingCommands), GetInputGestures(nameof(CorrectSpellingError))));
        private static Lazy<RoutedUICommand> ignoreSpellingError = new Lazy<RoutedUICommand>(() =>
            new RoutedUICommand("EDITING_COMMAND_IGNORE_SPELLING_ERROR", nameof(IgnoreSpellingError), typeof(EditingCommands), GetInputGestures(nameof(IgnoreSpellingError))));
    }
}
