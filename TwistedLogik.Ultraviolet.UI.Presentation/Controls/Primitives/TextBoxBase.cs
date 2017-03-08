using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for text box controls.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    public abstract class TextBoxBase : Control
    {
        /// <summary>
        /// Initializes the <see cref="TextBoxBase"/> type.
        /// </summary>
        static TextBoxBase()
        {
            EventManager.RegisterClassHandler(typeof(TextBoxBase), SelectionChangedEvent, new UpfRoutedEventHandler(HandleSelectionChanged));

            var canExecuteIsEnabled = new CanExecuteRoutedEventHandler(CanExecuteIsEnabled);
            var canExecuteIsEditable = new CanExecuteRoutedEventHandler(CanExecuteIsEditable);
            var canExecuteIsCaretVisible = new CanExecuteRoutedEventHandler(CanExecuteIsCaretVisible);

            // Selection commands
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), ApplicationCommands.SelectAll,
                ExecutedSelectAll, canExecuteIsCaretVisible, new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl+A"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectRightByCharacter,
                ExecutedSelectRightByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Shift, "Shift+Right"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectLeftByCharacter,
                ExecutedSelectLeftByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Shift, "Shift+Left"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectRightByWord,
                ExecutedSelectRightByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Right"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectLeftByWord,
                ExecutedSelectLeftByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Left"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectDownByLine,
                ExecutedSelectDownByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Down, ModifierKeys.Shift, "Shift+Down"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectUpByLine,
                ExecutedSelectUpByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Up, ModifierKeys.Shift, "Shift+Up"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectDownByPage,
                ExecutedSelectDownByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageDown, ModifierKeys.Shift, "Shift+PageDown"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectUpByPage,
                ExecutedSelectUpByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageUp, ModifierKeys.Shift, "Shift+PageUp"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectToLineStart,
                ExecutedSelectToLineStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Shift, "Shift+Home"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectToLineEnd,
                ExecutedSelectToLineEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Shift, "Shift+End"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectToDocumentStart,
                ExecutedSelectToDocumentStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+Home"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.SelectToDocumentEnd,
                ExecutedSelectToDocumentEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+End"));

            // Movement commands
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveRightByCharacter,
                ExecutedMoveRightByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.None, "Right"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveLeftByCharacter,
                ExecutedMoveLeftByCharacter, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.None, "Left"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveRightByWord,
                ExecutedMoveRightByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Right, ModifierKeys.Control, "Ctrl+Right"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveLeftByWord,
                ExecutedMoveLeftByWord, canExecuteIsCaretVisible, new KeyGesture(Key.Left, ModifierKeys.Control, "Ctrl+Left"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveDownByLine,
                ExecutedMoveDownByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Down, ModifierKeys.None, "Down"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveUpByLine,
                ExecutedMoveUpByLine, canExecuteIsCaretVisible, new KeyGesture(Key.Up, ModifierKeys.None, "Up"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveDownByPage,
                ExecutedMoveDownByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageDown, ModifierKeys.None, "PageDown"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveUpByPage,
                ExecutedMoveUpByPage, canExecuteIsCaretVisible, new KeyGesture(Key.PageUp, ModifierKeys.None, "PageUp"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveToLineStart,
                ExecutedMoveToLineStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.None, "Home"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveToLineEnd,
                ExecutedMoveToLineEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.None, "End"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveToDocumentStart,
                ExecutedMoveToDocumentStart, canExecuteIsCaretVisible, new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.MoveToDocumentEnd,
                ExecutedMoveToDocumentEnd, canExecuteIsCaretVisible, new KeyGesture(Key.End, ModifierKeys.Control, "Ctrl+End"));
            
            // Text editing commands
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.ToggleInsert,
                ExecutedToggleInsert, canExecuteIsCaretVisible, new KeyGesture(Key.Insert, ModifierKeys.None, "Insert"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.Backspace,
                ExecutedBackspace, canExecuteIsEditable, new KeyGesture(Key.Backspace, ModifierKeys.None, "Backspace"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.Delete,
                ExecutedDelete, canExecuteIsEditable, new KeyGesture(Key.Delete, ModifierKeys.None, "Delete"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.DeleteNextWord,
                ExecutedDeleteNextWord, canExecuteIsEditable, new KeyGesture(Key.Delete, ModifierKeys.Control, "Ctrl+Delete"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.DeletePreviousWord,
                ExecutedDeletePreviousWord, canExecuteIsEditable, new KeyGesture(Key.Backspace, ModifierKeys.Control, "Ctrl+Backspace"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.EnterParagraphBreak,
                ExecutedEnterParagraphBreak, canExecuteIsEditable, new KeyGesture(Key.Return, ModifierKeys.None, "Return"), new KeyGesture(Key.KeypadEnter, ModifierKeys.None, "KeypadEnter"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.EnterLineBreak,
                ExecutedEnterLineBreak, canExecuteIsEditable, new KeyGesture(Key.Return, ModifierKeys.Shift, "Shift+Return"), new KeyGesture(Key.KeypadEnter, ModifierKeys.Shift, "Shift+KeypadEnter"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.TabForward,
                ExecutedTabForward, CanExecuteTabForward, new KeyGesture(Key.Return, ModifierKeys.None, "Tab"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), EditingCommands.TabBackward,
                ExecutedTabBackward, CanExecuteTabBackward, new KeyGesture(Key.Return, ModifierKeys.None, "Shift+Tab"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), ApplicationCommands.Copy,
                ExecutedCopy, CanExecuteCopy, new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), ApplicationCommands.Cut,
                ExecutedCut, CanExecuteCut, new KeyGesture(Key.X, ModifierKeys.Control, "Ctrl+X"));
            CommandManager.RegisterClassBindings(typeof(TextBoxBase), ApplicationCommands.Paste,
                ExecutedPaste, CanExecutePaste, new KeyGesture(Key.V, ModifierKeys.Control, "Ctrl+V"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxBase"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBoxBase(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }
        
        /// <summary>
        /// Clears the text box's content.
        /// </summary>
        public void Clear()
        {
            if (TextEditor != null)
                TextEditor.Clear();
        }

        /// <summary>
        /// Selects the specified range of text.
        /// </summary>
        /// <param name="start">The index of the first character to select.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(Int32 start, Int32 length)
        {
            if (TextEditor != null)
                TextEditor.Select(start, length);
        }

        /// <summary>
        /// Selects the entirety of the editor's text.
        /// </summary>
        public void SelectAll()
        {
            if (TextEditor != null)
                TextEditor.SelectAll();
        }

        /// <summary>
        /// Selects the word, whitespace, or symbol at the current caret position.
        /// </summary>
        public void SelectCurrentToken()
        {
            if (TextEditor != null)
                TextEditor.SelectCurrentToken();
        }

        /// <summary>
        /// Appends the specified string to the end of the text box's content.
        /// </summary>
        /// <param name="textData">The text to append to the end of the text box's content.</param>
        public void AppendText(String textData)
        {
            if (TextEditor != null)
                TextEditor.AppendText(textData);
        }

        /// <summary>
        /// Copies the currently selected text onto the clipboard.
        /// </summary>
        public void Copy()
        {
            if (TextEditor != null)
                TextEditor.Copy();
        }

        /// <summary>
        /// Cuts the currently selected text onto the clipboard.
        /// </summary>
        public void Cut()
        {
            if (TextEditor != null)
                TextEditor.Cut();
        }

        /// <summary>
        /// Pastes the contents of the clipboard at the current caret position.
        /// </summary>
        public void Paste()
        {
            if (TextEditor != null)
                TextEditor.Paste();
        }

        /// <summary>
        /// Scrolls the text box one line up.
        /// </summary>
        public void LineUp()
        {
            LineUpInternal();
        }

        /// <summary>
        /// Scrolls the text box one line down.
        /// </summary>
        public void LineDown()
        {
            LineDownInternal();
        }

        /// <summary>
        /// Scrolls the text box one line to the left.
        /// </summary>
        public void LineLeft()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.LineLeft();
        }

        /// <summary>
        /// Scrolls the text box one line to the right.
        /// </summary>
        public void LineRight()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.LineRight();
        }

        /// <summary>
        /// Scrolls the text box one page towards the top of its content.
        /// </summary>
        public void PageUp()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.PageUp();
        }

        /// <summary>
        /// Scrolls the text box one page towards the bottom of its content.
        /// </summary>
        public void PageDown()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.PageDown();
        }

        /// <summary>
        /// Scrolls the text box one page towards the left of its content.
        /// </summary>
        public void PageLeft()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.PageLeft();
        }

        /// <summary>
        /// Scrolls the text box one page towards the right of its content.
        /// </summary>
        public void PageRight()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.PageRight();
        }

        /// <summary>
        /// Scrolls the text box to the beginning of its content.
        /// </summary>
        public void ScrollToHome()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToHome();
        }

        /// <summary>
        /// Scrolls the text box to the end of its content.
        /// </summary>
        public void ScrollToEnd()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Scrolls the text box to the specified horizontal offset.
        /// </summary>
        /// <param name="offset">The horizontal offset to which to move the text box's scrollable area.</param>
        public void ScrollToHorizontalOffset(Double offset)
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToHorizontalOffset(offset);
        }

        /// <summary>
        /// Scrolls the text box to the specified vertical offset.
        /// </summary>
        /// <param name="offset">The vertical offset to which to move the text box's scrollable area.</param>
        public void ScrollToVerticalOffset(Double offset)
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ScrollToVerticalOffset(offset);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text box will accept the return key as a normal character.
        /// </summary>
        /// <value><see langword="true"/> if the text box will accept the RETURN key as a character;
        /// otherwise, <see langword="false"/>. The default value is <see langword="false"/></value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="AcceptsReturnProperty"/></dpropField>
        ///		<dpropStylingName>accepts-return</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean AcceptsReturn
        {
            get { return GetValue<Boolean>(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text box will accept the tab key as a normal character. If false,
        /// tab will instead be used for tab navigation.
        /// </summary>
        /// <value><see langword="true"/> if the text box will accept the TAB key as a character;
        /// otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="AcceptsTabProperty"/></dpropField>
        ///		<dpropStylingName>accepts-tab</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean AcceptsTab
        {
            get { return GetValue<Boolean>(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a <see cref="ScrollBarVisibility"/> value which specifies whether the text box's horizontal scroll bar is visible.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value which specifies whether the text box's horizontal scroll bar is visible.
        /// The default value is <see cref="ScrollBarVisibility.Hidden"/>.</value>
        /// <remarks>
        ///	<dprop>
        ///		<dpropFields><see cref="HorizontalScrollBarVisibilityProperty"/></dpropFields>
        ///		<dpropStylingName>horizontal-scroll-bar-visibility</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="ScrollBarVisibility"/> value which specifies whether the text box's vertical scroll bar is visible.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value which specifies whether the text box's vertical scroll bar is visible.
        /// The default value is <see cref="ScrollBarVisibility.Hidden"/>.</value>
        /// <remarks>
        ///	<dprop>
        ///		<dpropFields><see cref="VerticalScrollBarVisibilityProperty"/></dpropFields>
        ///		<dpropStylingName>vertical-scroll-bar-visibility</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight is displayed when the text box does not have focus.
        /// </summary>
        /// <value><see langword="true"/> if the selection highlight is displayed when the text box does not have focus;
        /// otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsInactiveSelectionHighlightEnabledProperty"/></dpropField>
        ///		<dpropStylingName>inactive-selection-highlight-enabled</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsInactiveSelectionHighlightEnabled
        {
            get { return GetValue<Boolean>(IsInactiveSelectionHighlightEnabledProperty); }
            set { SetValue(IsInactiveSelectionHighlightEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a read-only text box. A read-only text box cannot be changed by the
        /// user, but may still be changed programmatically.
        /// </summary>
        /// <value><see langword="true"/> if the text box cannot be edited by the user; otherwise, <see langword="false"/>.
        /// The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropFields><see cref="IsReadOnlyProperty"/></dpropFields>
        ///		<dpropStylingName>read-only</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsReadOnly
        {
            get { return GetValue<Boolean>(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the caret is visible when this text box is read-only.
        /// </summary>
        /// <value><see langword="true"/> if the caret is visible when the text box is read-only; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsReadOnlyCaretVisibleProperty"/></dpropField>
        ///		<dpropStylingName>read-only-caret-visible</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsReadOnlyCaretVisible
        {
            get { return GetValue<Boolean>(IsReadOnlyCaretVisibleProperty); }
            set { SetValue(IsReadOnlyCaretVisibleProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the text box has focus and selected text.
        /// </summary>
        /// <value><see langword="true"/> if the text box is focused and has selected text; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsSelectionActiveProperty"/></dpropField>
        ///		<dpropStylingName>selection-active</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean IsSelectionActive
        {
            get { return GetValue<Boolean>(IsSelectionActiveProperty); }
        }
        
        /// <summary>
        /// Gets the horizontal offset of the text box's scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the current horizontal 
        /// offset in device-independent pixels of the text box's scroll viewer.</value>
        public Double HorizontalOffset
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.HorizontalOffset;
            }
        }

        /// <summary>
        /// Gets the vertical offset of the text box's scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the current vertical
        /// offset in device-independent pixels of the text box's scroll viewer.</value>
        public Double VerticalOffset
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.VerticalOffset;
            }
        }

        /// <summary>
        /// Gets the width of the content which is being displayed by the text box's scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the width in device-independent pixels of 
        /// the content which is being displayed by the text box's scroll viewer.</value>
        public Double ExtentWidth
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.ExtentWidth;
            }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the text box's scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the height in device-independent pixels of
        /// the content which is being displayed by the text box's scroll viewer.</value>
        public Double ExtentHeight
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.ExtentHeight;
            }
        }

        /// <summary>
        /// Gets the width of the text box's scrollable viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the width in device-independent pixels
        /// of the text box's scrollable viewport.</value>
        public Double ViewportWidth
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.ViewportWidth;
            }
        }

        /// <summary>
        /// Gets the height of the text box's scrollable viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> which represents the height in device-independent pixels
        /// of the text box's scrollable viewport.</value>
        public Double ViewportHeight
        {
            get
            {
                var scrollViewer = TextEditorScrollViewer;
                return (scrollViewer == null) ? 0 : scrollViewer.ViewportHeight;
            }
        }

        /// <summary>
        /// Occurs when the selected text is changed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="SelectionChangedEvent"/></revtField>
        ///		<revtStylingName>selection-changed</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        /// <summary>
        /// Occurs when the text box's text changes.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///		<revtField><see cref="TextChangedEvent"/></revtField>
        ///		<revtStylingName>text-changed</revtStylingName>
        ///		<revtStrategy>Bubbling</revtStrategy>
        ///		<revtDelegate><see cref="UpfRoutedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfRoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AcceptsReturn"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="AcceptsReturn"/> dependency property.</value>
        public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(HandleAcceptsReturnChanged));

        /// <summary>
        /// Identifies the <see cref="AcceptsTab"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="AcceptsTab"/> dependency property.</value>
        public static readonly DependencyProperty AcceptsTabProperty = DependencyProperty.Register("AcceptsTab", typeof(Boolean), typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleAcceptsTabChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HorizontalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(TextBoxBase),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Hidden, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(TextBoxBase),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Hidden, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsInactiveSelectionHighlightEnabled"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsInactiveSelectionHighlightEnabled"/> dependency property.</value>
        public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = DependencyProperty.Register("IsInactiveSelectionHighlightEnabled", typeof(Boolean), typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsReadOnly"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsReadOnly"/> dependency property.</value>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleIsReadOnlyChanged));

        /// <summary>
        /// Identifies the <see cref="IsReadOnlyCaretVisible"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsReadOnlyCaretVisible"/> dependency property.</value>
        public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty = DependencyProperty.Register("IsReadOnlyCaretVisibleProperty", typeof(Boolean), typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="IsSelectionActive"/> read-only dependency property.
        /// </summary>
        internal static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterReadOnly("IsSelectionActive", typeof(Boolean), typeof(TextBoxBase),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="IsSelectionActive"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsSelectionActive"/> dependency property.</value>
        public static readonly DependencyProperty IsSelectionActiveProperty = IsSelectionActivePropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="SelectionChanged"/> routed event.</value>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged",
            RoutingStrategy.Bubble, typeof(UpfRoutedEventHandler), typeof(TextBoxBase));

        /// <summary>
        /// Identifies the <see cref="TextChanged"/> routed event.
        /// </summary>
        /// <value>The identifier for the <see cref="TextChanged"/> routed event.</value>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged",
            RoutingStrategy.Bubble, typeof(UpfRoutedEventHandler), typeof(TextBoxBase));

        /// <summary>
        /// Moves the text box up one line.
        /// </summary>
        internal virtual void LineUpInternal()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.LineUp();
        }

        /// <summary>
        /// Moves the text box down one line.
        /// </summary>
        internal virtual void LineDownInternal()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
                scrollViewer.LineDown();
        }

        /// <summary>
        /// Gets the text box's text editor.
        /// </summary>
        internal TextEditor TextEditor
        {
            get { return PART_Editor; }
        }

        /// <summary>
        /// Gets the text box's scroll viewer.
        /// </summary>
        internal ScrollViewer TextEditorScrollViewer
        {
            get
            {
                if (PART_Editor == null)
                    return null;

                return PART_Editor.Parent as ScrollViewer;
            }
        }

        /// <inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            UpdateIsSelectionActive();

            base.OnGotKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardDevice device, IInputElement oldFocus, IInputElement newFocus, RoutedEventData data)
        {
            UpdateIsSelectionActive();

            base.OnLostKeyboardFocus(device, oldFocus, newFocus, data);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> routed event.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(SelectionChangedEvent);
            var evtData = RoutedEventData.Retrieve(this);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> routed event.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            var evtDelegate = EventManager.GetInvocationDelegate<UpfRoutedEventHandler>(TextChangedEvent);
            var evtData = RoutedEventData.Retrieve(this);
            evtDelegate(this, evtData);
        }

        /// <summary>
        /// Occurs when the control handles a <see cref="SelectionChangedEvent"/> routed event.
        /// </summary>
        private static void HandleSelectionChanged(DependencyObject dobj, RoutedEventData data)
        {
            var textBox = (TextBoxBase)dobj;
            textBox.UpdateIsSelectionActive();

            if (textBox.TextEditor != null && data.OriginalSource == textBox.TextEditor)
            {
                textBox.OnSelectionChanged();
                data.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AcceptsReturn"/> dependency property changes.
        /// </summary>
        private static void HandleAcceptsReturnChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AcceptsTab"/> dependency property changes.
        /// </summary>
        private static void HandleAcceptsTabChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="IsReadOnly"/> dependency property changes.
        /// </summary>
        private static void HandleIsReadOnlyChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.SelectAll"/> command.
        /// </summary>
        private static void ExecutedSelectAll(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.SelectAll();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectRightByCharacter"/> command.
        /// </summary>
        private static void ExecutedSelectRightByCharacter(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretRight(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectLeftByCharacter"/> command.
        /// </summary>
        private static void ExecutedSelectLeftByCharacter(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretLeft(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectRightByWord"/> command.
        /// </summary>
        private static void ExecutedSelectRightByWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToNextWord(true);
        }
        
        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectLeftByWord"/> command.
        /// </summary>
        private static void ExecutedSelectLeftByWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToPreviousWord(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectUpByLine"/> command.
        /// </summary>
        private static void ExecutedSelectDownByLine(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretUp(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectDownByLine"/> command.
        /// </summary>
        private static void ExecutedSelectUpByLine(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretDown(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectUpByPage"/> command.
        /// </summary>
        private static void ExecutedSelectDownByPage(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretPageUp(true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectDownByPage"/> command.
        /// </summary>
        private static void ExecutedSelectUpByPage(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretPageDown(true);
        }
        
        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToLineStart"/> command.
        /// </summary>
        private static void ExecutedSelectToLineStart(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToHome(true, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToLineEnd"/> command.
        /// </summary>
        private static void ExecutedSelectToLineEnd(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToEnd(true, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToDocumentStart"/> command.
        /// </summary>
        private static void ExecutedSelectToDocumentStart(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToHome(true, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.SelectToDocumentEnd"/> command.
        /// </summary>
        private static void ExecutedSelectToDocumentEnd(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToEnd(true, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveRightByCharacter"/> command.
        /// </summary>
        private static void ExecutedMoveRightByCharacter(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretRight(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveLeftByCharacter"/> command.
        /// </summary>
        private static void ExecutedMoveLeftByCharacter(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretLeft(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveRightByWord"/> command.
        /// </summary>
        private static void ExecutedMoveRightByWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToNextWord(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveLeftByWord"/> command.
        /// </summary>
        private static void ExecutedMoveLeftByWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToPreviousWord(false);
        }
        
        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveUpByLine"/> command.
        /// </summary>
        private static void ExecutedMoveDownByLine(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretUp(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveDownByLine"/> command.
        /// </summary>
        private static void ExecutedMoveUpByLine(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretDown(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveUpByPage"/> command.
        /// </summary>
        private static void ExecutedMoveDownByPage(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretPageUp(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveDownByPage"/> command.
        /// </summary>
        private static void ExecutedMoveUpByPage(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretPageDown(false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToLineStart"/> command.
        /// </summary>
        private static void ExecutedMoveToLineStart(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToHome(false, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToLineEnd"/> command.
        /// </summary>
        private static void ExecutedMoveToLineEnd(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToEnd(false, false);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToDocumentStart"/> command.
        /// </summary>
        private static void ExecutedMoveToDocumentStart(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToHome(false, true);
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.MoveToDocumentEnd"/> command.
        /// </summary>
        private static void ExecutedMoveToDocumentEnd(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible) || !textBox.IsEnabled)
                return;

            textBox.TextEditor.MoveCaretToEnd(false, true);
        }
        
        /// <summary>
        /// Executes the <see cref="EditingCommands.ToggleInsert"/> command.
        /// </summary>
        private static void ExecutedToggleInsert(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.ToggleInsertionMode();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.Backspace"/> command.
        /// </summary>
        private static void ExecutedBackspace(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.Backspace();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.Delete"/> command.
        /// </summary>
        private static void ExecutedDelete(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.Delete();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.DeleteNextWord"/> command.
        /// </summary>
        private static void ExecutedDeleteNextWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.DeleteNextWord();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.DeletePreviousWord"/> command.
        /// </summary>
        private static void ExecutedDeletePreviousWord(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.DeletePreviousWord();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.EnterParagraphBreak"/> command.
        /// </summary>
        private static void ExecutedEnterParagraphBreak(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.InsertNewLine();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.EnterLineBreak"/> command.
        /// </summary>
        private static void ExecutedEnterLineBreak(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.InsertNewLine();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.TabForward"/> command.
        /// </summary>
        private static void ExecutedTabForward(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.InsertTab();
        }

        /// <summary>
        /// Executes the <see cref="EditingCommands.TabBackward"/> command.
        /// </summary>
        private static void ExecutedTabBackward(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.InsertTab();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Copy"/> command.
        /// </summary>
        private static void ExecutedCopy(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.TextEditor.IsMasked || textBox.TextEditor.SelectionLength == 0 || !textBox.IsEnabled)
                return;

            textBox.TextEditor.Copy();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Cut"/> command.
        /// </summary>
        private static void ExecutedCut(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.TextEditor.IsMasked || textBox.TextEditor.SelectionLength == 0 || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.Cut();
        }

        /// <summary>
        /// Executes the <see cref="ApplicationCommands.Paste"/> command.
        /// </summary>
        private static void ExecutedPaste(DependencyObject element, ICommand command, Object parameter, RoutedEventData data)
        {
            var textBox = (TextBoxBase)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            textBox.TextEditor.Paste();
        }

        /// <summary>
        /// Specifies that a command can execute if the text box is enabled.
        /// </summary>
        private static void CanExecuteIsEnabled(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null || !textBox.IsEnabled)
                return;

            data.CanExecute = true;
            data.Handled = true;
        }

        /// <summary>
        /// Specifies that a command can execute if the text box is enabled and editable.
        /// </summary>
        private static void CanExecuteIsEditable(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null || textBox.IsReadOnly || !textBox.IsEnabled)
                return;

            data.CanExecute = true;
            data.Handled = true;
        }

        /// <summary>
        /// Specifies that a command can execute if the text box is enabled and the caret is visible.
        /// </summary>
        private static void CanExecuteIsCaretVisible(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null || !textBox.IsEnabled)
                return;

            if (textBox.IsReadOnly && !textBox.IsReadOnlyCaretVisible)
                return;

            data.CanExecute = true;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.TabForward"/> command can execute.
        /// </summary>
        private static void CanExecuteTabForward(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            data.CanExecute = textBox.TextEditor != null && textBox.IsEnabled && textBox.AcceptsTab && !textBox.IsReadOnly;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="EditingCommands.TabBackward"/> command can execute.
        /// </summary>
        private static void CanExecuteTabBackward(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            data.CanExecute = textBox.TextEditor != null && textBox.IsEnabled && textBox.AcceptsTab && !textBox.IsReadOnly;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Copy"/> command can execute.
        /// </summary>
        private static void CanExecuteCopy(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null)
                return;

            if (textBox.TextEditor.IsMasked)
            {
                data.CanExecute = false;
                data.Handled = true;
                return;
            }

            data.CanExecute = textBox.IsEnabled && textBox.TextEditor.SelectionLength > 0;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Cut"/> command can execute.
        /// </summary>
        private static void CanExecuteCut(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null)
                return;

            if (textBox.TextEditor.IsMasked)
            {
                data.CanExecute = false;
                data.Handled = true;
                return;
            }

            data.CanExecute = textBox.IsEnabled && !textBox.IsReadOnly && textBox.TextEditor.SelectionLength > 0;
            data.Handled = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ApplicationCommands.Paste"/> command can execute.
        /// </summary>
        private static void CanExecutePaste(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            var textBox = (TextBox)element;
            if (textBox.TextEditor == null)
                return;

            data.CanExecute = textBox.IsEnabled && !textBox.IsReadOnly;
            data.Handled = true;
        }
        
        /// <summary>
        /// Updates the value of the <see cref="IsSelectionActive"/> property.
        /// </summary>
        private void UpdateIsSelectionActive()
        {
            var isSelectionActive = IsKeyboardFocusWithin;

            if (TextEditor == null || TextEditor.SelectionLength == 0)
                isSelectionActive = false;

            var oldValue = GetValue<Boolean>(IsSelectionActiveProperty);
            if (oldValue != isSelectionActive)
            {
                SetValue(IsSelectionActivePropertyKey, isSelectionActive);
            }
        }

        // Component references.
        private readonly TextEditor PART_Editor = null;
    }
}
