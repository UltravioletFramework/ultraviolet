using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents the base class for text box controls.
    /// </summary>
    [UvmlKnownType]
    public abstract class TextBoxBase : TextEditingControl
    {
        /// <summary>
        /// Initializes the <see cref="TextBoxBase"/> type.
        /// </summary>
        static TextBoxBase()
        {
            EventManager.RegisterClassHandler(typeof(TextBoxBase), SelectionChangedEvent, new UpfRoutedEventHandler(HandleSelectionChanged));
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
            {
                UpdateLayout();
                scrollViewer.LineLeft();
            }
        }

        /// <summary>
        /// Scrolls the text box one line to the right.
        /// </summary>
        public void LineRight()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.LineRight();
            }
        }

        /// <summary>
        /// Scrolls the text box one page towards the top of its content.
        /// </summary>
        public void PageUp()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.PageUp();
            }
        }

        /// <summary>
        /// Scrolls the text box one page towards the bottom of its content.
        /// </summary>
        public void PageDown()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.PageDown();
            }
        }

        /// <summary>
        /// Scrolls the text box one page towards the left of its content.
        /// </summary>
        public void PageLeft()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.PageLeft();
            }
        }

        /// <summary>
        /// Scrolls the text box one page towards the right of its content.
        /// </summary>
        public void PageRight()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.PageRight();
            }
        }

        /// <summary>
        /// Scrolls the text box to the beginning of its content.
        /// </summary>
        public void ScrollToHome()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToHome();
            }
        }

        /// <summary>
        /// Scrolls the text box to the end of its content.
        /// </summary>
        public void ScrollToEnd()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToEnd();
            }
        }

        /// <summary>
        /// Scrolls the text box to the specified horizontal offset.
        /// </summary>
        /// <param name="offset">The horizontal offset to which to move the text box's scrollable area.</param>
        public void ScrollToHorizontalOffset(Double offset)
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToHorizontalOffset(offset);
            }
        }

        /// <summary>
        /// Scrolls the text box to the specified vertical offset.
        /// </summary>
        /// <param name="offset">The vertical offset to which to move the text box's scrollable area.</param>
        public void ScrollToVerticalOffset(Double offset)
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.ScrollToVerticalOffset(offset);
            }
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
            {
                UpdateLayout();
                scrollViewer.LineUp();
            }
        }

        /// <summary>
        /// Moves the text box down one line.
        /// </summary>
        internal virtual void LineDownInternal()
        {
            var scrollViewer = TextEditorScrollViewer;
            if (scrollViewer != null)
            {
                UpdateLayout();
                scrollViewer.LineDown();
            }
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

        /// <inheritdoc/>
        protected override TextEditor TextEditor => PART_Editor;

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
