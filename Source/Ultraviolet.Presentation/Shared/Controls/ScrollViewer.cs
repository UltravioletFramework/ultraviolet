using System;
using Ultraviolet.Core;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the method that is called in response to a <see cref="ScrollViewer.ScrollChanged"/> routed event.
    /// </summary>
    /// <param name="element">The element that raised the event.</param>
    /// <param name="data">The routed event metadata for this event invocation.</param>
    public delegate void UpfScrollChangedEventHandler(DependencyObject element, ScrollChangedRoutedEventData data);

    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UvmlKnownType(null, "Ultraviolet.Presentation.Controls.Templates.ScrollViewer.xml")]
    public class ScrollViewer : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="ScrollViewer"/> type.
        /// </summary>
        static ScrollViewer()
        {
            // Dependency property overrides
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ScrollViewer), new PropertyMetadata<KeyboardNavigationMode>(KeyboardNavigationMode.Local));
            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(ScrollViewer), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False));

            // Event handlers
            EventManager.RegisterClassHandler(typeof(ScrollViewer), FrameworkElement.RequestBringIntoViewEvent, new UpfRequestBringIntoViewEventHandler(HandleRequestBringIntoView));
            EventManager.RegisterClassHandler(typeof(ScrollViewer), Thumb.DragCompletedEvent, new UpfDragCompletedEventHandler(HandleThumbDragCompleted));
            EventManager.RegisterClassHandler(typeof(ScrollViewer), RangeBase.ValueChangedEvent, new UpfRoutedEventHandler(HandleScrollBarValueChanged));

            // Commands - vertical scroll
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.LineDownCommand, ExecutedLineDownCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.LineUpCommand, ExecutedLineUpCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.PageDownCommand, ExecutedPageDownCommand, CanExecutePageScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.PageUpCommand, ExecutedPageUpCommand, CanExecutePageScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToBottomCommand, ExecutedScrollToBottomCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToTopCommand, ExecutedScrollToTopCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToVerticalOffsetCommand, ExecutedScrollToVerticalOffsetCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.DeferScrollToVerticalOffsetCommand, ExecutedDeferScrollToVerticalOffsetCommand, CanExecuteDeferredScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ComponentCommands.ScrollPageDown, ExecutedPageDownCommand, CanExecutePageScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ComponentCommands.ScrollPageUp, ExecutedPageUpCommand, CanExecutePageScrollCommand);

            // Commands - horizontal scroll            
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.LineRightCommand, ExecutedLineRightCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.LineLeftCommand, ExecutedLineLeftCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.PageRightCommand, ExecutedPageRightCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.PageLeftCommand, ExecutedPageLeftCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToRightEndCommand, ExecutedScrollToRightEndCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToLeftEndCommand, ExecutedScrollToLeftEndCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToHorizontalOffsetCommand, ExecutedScrollToHorizontalOffsetCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.DeferScrollToHorizontalOffsetCommand, ExecutedDeferScrollToHorizontalOffsetCommand, CanExecuteDeferredScrollCommand);

            // Commands - misc
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToEndCommand, ExecutedScrollToEndCommand, CanExecuteScrollCommand);
            CommandManager.RegisterClassBindings(typeof(ScrollViewer), ScrollBar.ScrollToHomeCommand, ExecutedScrollToHomeCommand, CanExecuteScrollCommand);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ScrollViewer(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Sets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.HorizontalScrollBarVisibility"/> attached property.
        /// </summary>
        /// <param name="element">The element on which to set the property.</param>
        /// <param name="value">The property value to set on the element.</param>
        public static void SetHorizontalScrollBarVisibility(DependencyObject element, ScrollBarVisibility value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(HorizontalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.HorizontalScrollBarVisibility"/> attached property
        /// on the specified object.
        /// </summary>
        /// <param name="element">The element for which to retrieve the property value.</param>
        public static ScrollBarVisibility GetHorizontalScrollBarVisibility(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.VerticalScrollBarVisibility"/> attached property.
        /// </summary>
        /// <param name="element">The element on which to set the property.</param>
        /// <param name="value">The property value to set on the element.</param>
        public static void SetVerticalScrollBarVisibility(DependencyObject element, ScrollBarVisibility value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(VerticalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.VerticalScrollBarVisibility"/> attached property
        /// on the specified object.
        /// </summary>
        /// <param name="element">The element for which to retrieve the property value.</param>
        /// <returns>The value of the property on the specified element.</returns>
        public static ScrollBarVisibility GetVerticalScrollBarVisibility(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.IsDeferredScrollingEnabled"/> attached property.
        /// </summary>
        /// <param name="element">The element on which to set the property.</param>
        /// <param name="value">The property value to set on the element.</param>
        public static void SetIsDeferredScrollingEnabled(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(IsDeferredScrollingEnabledProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.IsDeferredScrollingEnabled"/> attached property
        /// on the specified object.
        /// </summary>
        /// <param name="element">The element for which to retrieve the property value.</param>
        /// <returns>The value of the property on the specified element.</returns>
        public static Boolean GetIsDeferredScrollingEnabled(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(IsDeferredScrollingEnabledProperty);
        }

        /// <summary>
        /// Scrolls the viewer's content up by one line.
        /// </summary>
        public void LineUp()
        {
            ScrollToVerticalOffset(ContentVerticalOffset - ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content down by one line.
        /// </summary>
        public void LineDown()
        {
            ScrollToVerticalOffset(ContentVerticalOffset + ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content left by one line.
        /// </summary>
        public void LineLeft()
        {
            ScrollToHorizontalOffset(ContentHorizontalOffset - ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content right by one line.
        /// </summary>
        public void LineRight()
        {
            ScrollToHorizontalOffset(ContentHorizontalOffset + ScrollDeltaKey);
        }

        /// <summary>
        /// Scrolls the viewer's content up by one page.
        /// </summary>
        public void PageUp()
        {
            ScrollToVerticalOffset(ContentVerticalOffset - ViewportHeight);
        }

        /// <summary>
        /// Scrolls the viewer's content down by one page.
        /// </summary>
        public void PageDown()
        {
            ScrollToVerticalOffset(ContentVerticalOffset + ViewportHeight);
        }

        /// <summary>
        /// Scrolls the viewer's content left by one page.
        /// </summary>
        public void PageLeft()
        {
            ScrollToHorizontalOffset(ContentHorizontalOffset - ViewportWidth);
        }

        /// <summary>
        /// Scrolls the viewer's content right by one page.
        /// </summary>
        public void PageRight()
        {
            ScrollToHorizontalOffset(ContentHorizontalOffset + ViewportWidth);
        }

        /// <summary>
        /// Scrolls the viewer to the top of its content.
        /// </summary>
        public void ScrollToTop()
        {
            ScrollToVerticalOffset(PART_VScroll?.Minimum ?? 0);
        }

        /// <summary>
        /// Scrolls the viewer to the bottom of its content.
        /// </summary>
        public void ScrollToBottom()
        {
            ScrollToVerticalOffset(PART_VScroll?.Maximum ?? 0);
        }

        /// <summary>
        /// Scrolls the viewer to the left end of its content.
        /// </summary>
        public void ScrollToLeftEnd()
        {
            ScrollToHorizontalOffset(PART_HScroll?.Minimum ?? 0);
        }

        /// <summary>
        /// Scrolls the viewer to the right end of its content.
        /// </summary>
        public void ScrollToRightEnd()
        {
            ScrollToHorizontalOffset(PART_HScroll?.Maximum ?? 0);
        }

        /// <summary>
        /// Scrolls the viewer to the beginning of its content.
        /// </summary>
        public void ScrollToHome()
        {
            ScrollToHorizontalOffset(Double.NegativeInfinity);
            ScrollToVerticalOffset(Double.NegativeInfinity);
        }

        /// <summary>
        /// Scrolls the viewer to the end of its content.
        /// </summary>
        public void ScrollToEnd()
        {
            ScrollToHorizontalOffset(Double.PositiveInfinity);
            ScrollToVerticalOffset(Double.PositiveInfinity);
        }

        /// <summary>
        /// Moves the scroll viewer to the specified horizontal offset.
        /// </summary>
        /// <param name="offset">The horizontal offset to which to move the scroll viewer.</param>
        public void ScrollToHorizontalOffset(Double offset)
        {
            ChangeHorizontalOffset(offset, false);
        }

        /// <summary>
        /// Moves the scroll viewer to the specified vertical offset.
        /// </summary>
        /// <param name="offset">The vertical offset to which to move the scroll viewer.</param>
        public void ScrollToVerticalOffset(Double offset)
        {
            ChangeVerticalOffset(offset, false);
        }

        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's horizontal scroll bar.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies the visibility of the scroll
        /// viewer's horizontal scroll bar. The default value is <see cref="ScrollBarVisibility.Disabled"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="HorizontalScrollBarVisibilityProperty"/></dpropField>
        ///     <dpropStylingName>hscrollbar-visibility</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value specifying the visibility of the scroll viewer's vertical scroll bar.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies the visibility of the scroll
        /// viewer's vertical scroll bar. The default value is <see cref="ScrollBarVisibility.Disabled"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="VerticalScrollBarVisibilityProperty"/></dpropField>
        ///     <dpropStylingName>vscrollbar-visibility</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsArrange"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates whether the scroll viewer's horizontal scroll bar is visible.
        /// </summary>
        public Visibility ComputedHorizontalScrollBarVisibility
        {
            get { return GetValue<Visibility>(ComputedHorizontalScrollBarVisibilityProperty); }
            private set { SetValue(ComputedHorizontalScrollBarVisibilityPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value that indicates whether the scroll viewer's vertical scroll bar is visible.
        /// </summary>
        public Visibility ComputedVerticalScrollBarVisibility
        {
            get { return GetValue<Visibility>(ComputedVerticalScrollBarVisibilityProperty); }
            private set { SetValue(ComputedVerticalScrollBarVisibilityPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the margin which is applied to the scroll viewer's content pane.
        /// </summary>
        /// <value>A <see cref="Thickness"/> value which represents the margin that is applied
        /// to the scroll viewer's content pane.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentMarginProperty"/></dpropField>
        ///     <dpropStylingName>content-margin</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Thickness ContentMargin
        {
            get { return GetValue<Thickness>(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this scroll viewer applies a clipping rectangle to its content pane.
        /// </summary>
        /// <value><see langword="true"/> if the scroll viewer's content is clipped; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentClippedProperty"/></dpropField>
        ///     <dpropStylingName>content-clipped</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean ContentClipped
        {
            get { return GetValue<Boolean>(ContentClippedProperty); }
            set { SetValue(ContentClippedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scroll viewer should defer
        /// scrolling until after the user is finished dragging the thumb.
        /// </summary>
        public Boolean IsDeferredScrollingEnabled
        {
            get { return GetValue<Boolean>(IsDeferredScrollingEnabledProperty); }
            set { SetValue(IsDeferredScrollingEnabledProperty, value); }
        }

        /// <summary>
        /// Gets the width of the content which is being displayed by the scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the content which is being displayed by the scroll viewer.</value>
        public Double ExtentWidth
        {
            get { return GetValue<Double>(ExtentWidthProperty); }
        }

        /// <summary>
        /// Gets the height of the content which is being displayed by the scroll viewer.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the content which is being displayed by the scroll viewer.</value>
        public Double ExtentHeight
        {
            get { return GetValue<Double>(ExtentHeightProperty); }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's scrollable area.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the scroll viewer's scrollable area.</value>
        public Double ScrollableWidth
        {
            get { return GetValue<Double>(ScrollableWidthProperty); }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's scrollable area.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the scroll viewer's scrollable area.</value>
        public Double ScrollableHeight
        {
            get { return GetValue<Double>(ScrollableHeightProperty); }
        }

        /// <summary>
        /// Gets the width of the scroll viewer's viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the width in device-independent
        /// pixels of the scroll viewer's viewport.</value>
        public Double ViewportWidth
        {
            get { return GetValue<Double>(ViewportWidthProperty); }
        }

        /// <summary>
        /// Gets the height of the scroll viewer's viewport.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the height in device-independent
        /// pixels of the scroll viewer's viewport.</value>
        public Double ViewportHeight
        {
            get { return GetValue<Double>(ViewportHeightProperty); }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the horizontal offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double HorizontalOffset
        {
            get { return GetValue<Double>(HorizontalOffsetProperty); }
            private set { SetValue(HorizontalOffsetPropertyKey, value); }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the vertical offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double VerticalOffset
        {
            get { return GetValue<Double>(VerticalOffsetProperty); }
            private set { SetValue(VerticalOffsetPropertyKey, value); }
        }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the horizontal offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double ContentHorizontalOffset
        {
            get { return GetValue<Double>(ContentHorizontalOffsetProperty); }
            private set { SetValue(ContentHorizontalOffsetPropertyKey, value); }
        }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        /// <value>A <see cref="Double"/> that represents the vertical offset
        /// of the scrolled content in device-independent pixels.</value>
        public Double ContentVerticalOffset
        {
            get { return GetValue<Double>(ContentVerticalOffsetProperty); }
            private set { SetValue(ContentVerticalOffsetPropertyKey, value); }
        }

        /// <summary>
        /// Occurs when the scroll viewer's scrolling properties are changed.
        /// </summary>
        /// <remarks>
        /// <revt>
        ///     <revtField><see cref="ScrollChangedEvent"/></revtField>
        ///     <revtStylingName>scroll-changed</revtStylingName>
        ///     <revtStrategy>Bubbling</revtStrategy>
        ///     <revtDelegate><see cref="UpfScrollChangedEventHandler"/></revtDelegate>
        /// </revt>
        /// </remarks>
        public event UpfScrollChangedEventHandler ScrollChanged
        {
            add { AddHandler(ScrollChangedEvent, value); }
            remove { RemoveHandler(ScrollChangedEvent, value); }
        }

        /// <summary>
        /// The private access key for the <see cref="ComputedHorizontalScrollBarVisibility"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ComputedHorizontalScrollBarVisibility"/> dependency property.</value>
        private static readonly DependencyPropertyKey ComputedHorizontalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedHorizontalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer),
            new PropertyMetadata<Visibility>(Visibility.Visible));

        /// <summary>
        /// Identifies the <see cref="ComputedHorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ComputedHorizontalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty = ComputedHorizontalScrollBarVisibilityPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ComputedVerticalScrollBarVisibilityProperty"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ComputedVerticalScrollBarVisibilityProperty"/> dependency property.</value>
        private static readonly DependencyPropertyKey ComputedVerticalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedVerticalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer),
            new PropertyMetadata<Visibility>(Visibility.Visible));

        /// <summary>
        /// Identifies the <see cref="ComputedVerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ComputedVerticalScrollBarVisibility"/> dependency property.</value>
        public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty = ComputedVerticalScrollBarVisibilityPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.HorizontalScrollBarVisibility"/> attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.HorizontalScrollBarVisibility"/> attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the scroll viewer's horizontal scroll bar should be visible.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies whether the scroll viewer's horizontal <see cref="ScrollViewer"/> should
        /// be visible. The default value is <see cref="ScrollBarVisibility.Disabled"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="HorizontalScrollBarVisibilityProperty"/></dpropField>
        ///		<dpropStylingName>hscrollbar-visibility</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("HorizontalScrollBarVisibility", "hscrollbar-visibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Disabled, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.VerticalScrollBarVisibility"/> attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.VerticalScrollBarVisibility"/> attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the scroll viewer's vertical scroll bar should be visible.
        /// </summary>
        /// <value>A <see cref="ScrollBarVisibility"/> value that specifies whether the scroll viewer's vertical <see cref="ScrollViewer"/> should
        /// be visible. The default value is <see cref="ScrollBarVisibility.Visible"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="VerticalScrollBarVisibilityProperty"/></dpropField>
        ///		<dpropStylingName>vscrollbar-visibility</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("VerticalScrollBarVisibility", "vscrollbar-visibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new PropertyMetadata<ScrollBarVisibility>(ScrollBarVisibility.Visible, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="ContentMargin"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentMargin"/> dependency property.</value>
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ScrollViewer),
            new PropertyMetadata<Thickness>(PresentationBoxedValues.Thickness.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ContentClipped"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentClipped"/> dependency property.</value>
        public static readonly DependencyProperty ContentClippedProperty = DependencyProperty.Register("ContentClipped", typeof(Boolean), typeof(ScrollViewer),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None, HandleContentClippedChanged));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.IsDeferredScrollingEnabled"/> attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.Controls.ScrollViewer.IsDeferredScrollingEnabled"/> attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ScrollViewer"/> should defer
        /// scrolling until after the user is finished dragging the thumb.
        /// </summary>
        /// <value>A <see cref="Boolean"/> that represents whether the <see cref="ScrollViewer"/> should defer
        /// scrolling until after the user is finished dragging the thumb. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="IsDeferredScrollingEnabledProperty"/></dpropField>
        ///		<dpropStylingName>deferred-scrolling-enabled</dpropStylingName>
        ///		<dpropMetadata><see cref="PropertyMetadataOptions.None"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty IsDeferredScrollingEnabledProperty = DependencyProperty.RegisterAttached("IsDeferredScrollingEnabled", typeof(Boolean), typeof(ScrollViewer),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// The private access key for the <see cref="ExtentWidth"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ExtentWidth"/> dependency property.</value>
        private static readonly DependencyPropertyKey ExtentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExtentWidth", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ExtentWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ExtentWidth"/> dependency property.</value>
        public static readonly DependencyProperty ExtentWidthProperty = ExtentWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ExtentHeight"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ExtentHeight"/> dependency property.</value>
        private static readonly DependencyPropertyKey ExtentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ExtentHeight", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ExtentHeight"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ExtentHeight"/> dependency property.</value>
        public static readonly DependencyProperty ExtentHeightProperty = ExtentHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ScrollableWidth"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ScrollableHeight"/> dependency property.</value>
        private static readonly DependencyPropertyKey ScrollableWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ScrollableWidth", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ScrollableWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ScrollableWidth"/> dependency property.</value>
        public static readonly DependencyProperty ScrollableWidthProperty = ScrollableWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ScrollableHeight"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ScrollableHeight"/> dependency property.</value>
        private static readonly DependencyPropertyKey ScrollableHeightPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ScrollableHeight", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ScrollableHeight"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ScrollableHeight"/> dependency property.</value>
        public static readonly DependencyProperty ScrollableHeightProperty = ScrollableHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ViewportWidth"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ViewportWidth"/> dependency property.</value>
        private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ViewportWidth", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ViewportWidth"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ViewportWidth"/> dependency property.</value>
        public static readonly DependencyProperty ViewportWidthProperty = ViewportWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ViewportHeight"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ViewportHeight"/> dependency property.</value>
        private static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ViewportHeight", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ViewportHeight"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ViewportHeight"/> dependency property.</value>
        public static readonly DependencyProperty ViewportHeightProperty = ViewportHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="HorizontalOffset"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="HorizontalOffset"/> dependency property.</value>
        private static readonly DependencyPropertyKey HorizontalOffsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HorizontalOffset", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="HorizontalOffset"/> dependency property.</value>
        public static readonly DependencyProperty HorizontalOffsetProperty = HorizontalOffsetPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="VerticalOffset"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="VerticalOffset"/> dependency property.</value>
        private static readonly DependencyPropertyKey VerticalOffsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("VerticalOffset", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="VerticalOffset"/> dependency property.</value>
        public static readonly DependencyProperty VerticalOffsetProperty = VerticalOffsetPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ContentHorizontalOffset"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ContentHorizontalOffset"/> dependency property.</value>
        private static readonly DependencyPropertyKey ContentHorizontalOffsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ContentHorizontalOffset", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="ContentHorizontalOffset"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentHorizontalOffset"/> dependency property.</value>
        public static readonly DependencyProperty ContentHorizontalOffsetProperty = ContentHorizontalOffsetPropertyKey.DependencyProperty;

        /// <summary>
        /// The private access key for the <see cref="ContentVerticalOffset"/> read-only dependency property.
        /// </summary>
        /// <value>The private access key for the <see cref="ContentVerticalOffset"/> dependency property.</value>
        private static readonly DependencyPropertyKey ContentVerticalOffsetPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ContentVerticalOffset", typeof(Double), typeof(ScrollViewer),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the <see cref="ContentVerticalOffset"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentVerticalOffset"/> dependency property.</value>
        public static readonly DependencyProperty ContentVerticalOffsetProperty = ContentVerticalOffsetPropertyKey.DependencyProperty;
        
        /// <summary>
        /// Identifies the <see cref="ScrollChanged"/> event.
        /// </summary>
        /// <value>The identifier for the <see cref="ScrollChanged"/> routed event.</value>
        public static readonly RoutedEvent ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Bubble,
            typeof(UpfScrollChangedEventHandler), typeof(ScrollViewer));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (PART_ContentPresenter == null || PART_HScroll == null || PART_VScroll == null)
                return Size2D.Zero;

            var child = GetVisualChild(0);

            var hVisibility = HorizontalScrollBarVisibility;
            var vVisibility = VerticalScrollBarVisibility;

            var hComputedVisibility = (hVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
            var vComputedVisibility = (vVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;

            ComputedHorizontalScrollBarVisibility = hComputedVisibility;
            ComputedVerticalScrollBarVisibility = vComputedVisibility;

            var hAuto = (hVisibility == ScrollBarVisibility.Auto);
            var vAuto = (vVisibility == ScrollBarVisibility.Auto);

            PART_ContentPresenter.CanScrollHorizontally = (hVisibility != ScrollBarVisibility.Disabled);
            PART_ContentPresenter.CanScrollVertically = (vVisibility != ScrollBarVisibility.Disabled);

            var availableSizeSansMargins = availableSize - Margin;
            child.DigestImmediately();
            child.Measure(availableSizeSansMargins);

            if (hAuto || vAuto)
            {
                var hAutoVisible = hAuto && PART_ContentPresenter.ExtentWidth > PART_ContentPresenter.ViewportWidth;
                if (hAutoVisible)
                {
                    ComputedHorizontalScrollBarVisibility = Visibility.Visible;
                }

                var vAutoVisible = vAuto && PART_ContentPresenter.ExtentHeight > PART_ContentPresenter.ViewportHeight;
                if (vAutoVisible)
                {
                    ComputedVerticalScrollBarVisibility = Visibility.Visible;
                }

                if (hAutoVisible || vAutoVisible)
                {
                    child.InvalidateMeasure();
                    child.DigestImmediately();
                    child.Measure(availableSizeSansMargins);
                }

                if (hAuto && vAuto && (hAutoVisible != vAutoVisible))
                {
                    hAutoVisible = !hAutoVisible && PART_ContentPresenter.ExtentWidth > PART_ContentPresenter.ViewportWidth;
                    if (hAutoVisible)
                    {
                        ComputedHorizontalScrollBarVisibility = Visibility.Visible;
                    }

                    vAutoVisible = !vAutoVisible && PART_ContentPresenter.ExtentHeight > PART_ContentPresenter.ViewportHeight;
                    if (vAutoVisible)
                    {
                        ComputedVerticalScrollBarVisibility = Visibility.Visible;
                    }

                    if (hAutoVisible || vAutoVisible)
                    {
                        child.InvalidateMeasure();
                        child.DigestImmediately();
                        child.Measure(availableSizeSansMargins);
                    }
                }
            }

            PART_HScroll.Minimum = 0;
            PART_VScroll.Minimum = 0;

            PART_HScroll.Maximum = ScrollableWidth;
            PART_VScroll.Maximum = ScrollableHeight;

            PART_HScroll.ViewportSize = ViewportWidth;
            PART_VScroll.ViewportSize = ViewportHeight;

            PART_HScroll.IsEnabled = PART_ContentPresenter.CanScrollHorizontally && ScrollableWidth > 0;
            PART_VScroll.IsEnabled = PART_ContentPresenter.CanScrollVertically && ScrollableHeight > 0;

            if (hComputedVisibility != Visibility.Visible)
            {
                HorizontalOffset = 0;
                PART_HScroll.Value = 0;
            }

            if (vComputedVisibility != Visibility.Visible)
            {
                VerticalOffset = 0;
                PART_VScroll.Value = 0;
            }

            child.InvalidateMeasure();
            child.DigestImmediately();
            child.Measure(availableSizeSansMargins);

            return child.DesiredSize;
        }

        /// <inheritdoc/>
        protected override void PositionOverride()
        {
            if (PART_ContentPresenter != null)
                PART_ContentPresenter.PositionChildren();

            var newHorizontalOffset = HorizontalOffset;
            var newVerticalOffset = VerticalOffset;
            var newExtentWidth = ExtentWidth;
            var newExtentHeight = ExtentHeight;
            var newViewportWidth = ViewportWidth;
            var newViewportHeight = ViewportHeight;

            var scrollChanged =
                !MathUtil.AreApproximatelyEqual(oldHorizontalOffset, newHorizontalOffset) ||
                !MathUtil.AreApproximatelyEqual(oldVerticalOffset, newVerticalOffset) ||
                !MathUtil.AreApproximatelyEqual(oldExtentWidth, newExtentWidth) ||
                !MathUtil.AreApproximatelyEqual(oldExtentHeight, newExtentHeight) ||
                !MathUtil.AreApproximatelyEqual(oldViewportWidth, newViewportWidth) ||
                !MathUtil.AreApproximatelyEqual(oldViewportHeight, newViewportHeight);

            if (scrollChanged)
            {
                var scrollInfo = new ScrollChangedInfo(
                    newHorizontalOffset, newHorizontalOffset - oldHorizontalOffset,
                    newVerticalOffset, newVerticalOffset - oldVerticalOffset,
                    newExtentWidth, newExtentWidth - oldExtentWidth,
                    newExtentHeight, newExtentHeight - oldExtentHeight,
                    newViewportWidth, newViewportWidth - oldViewportWidth,
                    newViewportHeight, newViewportHeight - oldViewportHeight);

                oldHorizontalOffset = newHorizontalOffset;
                oldVerticalOffset = newVerticalOffset;
                oldExtentWidth = newExtentWidth;
                oldExtentHeight = newExtentHeight;
                oldViewportWidth = newViewportWidth;
                oldViewportHeight = newViewportHeight;

                var evtDelegate = EventManager.GetInvocationDelegate<UpfScrollChangedEventHandler>(ScrollChangedEvent);
                var evtData = ScrollChangedRoutedEventData.Retrieve(this, scrollInfo);
                evtDelegate(this, evtData);
            }

            base.PositionOverride();
        }

        /// <inheritdoc/>
        protected override void PrepareOverride()
        {
            LayoutUpdated -= OnLayoutUpdated;
            LayoutUpdated += OnLayoutUpdated;

            base.PrepareOverride();
        }

        /// <inheritdoc/>
        protected override void CleanupOverride()
        {
            LayoutUpdated -= OnLayoutUpdated;

            base.CleanupOverride();
        }
        
        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseDevice device, Double x, Double y, RoutedEventData data)
        {
            if (!data.Handled)
            {
                if (x != 0 && PART_HScroll != null)
                {
                    ChangeHorizontalOffset(HorizontalOffset + (ScrollDeltaMouseWheel * x), false);
                }
                if (y != 0 && PART_VScroll != null)
                {
                    ChangeVerticalOffset(VerticalOffset + (ScrollDeltaMouseWheel * -y), false);
                }
                data.Handled = true;
            }
            base.OnMouseWheel(device, x, y, data);
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent == null || !templatedParent.HandlesScrolling)
            {
                HandleKeyInput(key, modifiers, data);
            }

            base.OnKeyDown(device, key, modifiers, data);
        }

        /// <inheritdoc/>
        protected override void OnGamePadButtonDown(GamePadDevice device, GamePadButton button, Boolean repeat, RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent == null || !templatedParent.HandlesScrolling)
            {
                switch (button)
                {
                    case GamePadButton.LeftStickUp:
                        HandleKeyInput(Key.Up, ModifierKeys.None, data);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickDown:
                        HandleKeyInput(Key.Down, ModifierKeys.None, data);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickLeft:
                        HandleKeyInput(Key.Left, ModifierKeys.None, data);
                        data.Handled = true;
                        break;

                    case GamePadButton.LeftStickRight:
                        HandleKeyInput(Key.Right, ModifierKeys.None, data);
                        data.Handled = true;
                        break;
                }
            }

            base.OnGamePadButtonDown(device, button, repeat, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchDown(TouchDevice device, Int64 id, Double x, Double y, Single pressure, RoutedEventData data)
        {
            if (!data.Handled && device.IsFirstTouchInGesture(id))
            {
                Touch.Capture(View, this, id, CaptureMode.SubTree);
                data.Handled = true;
            }
            base.OnTouchDown(device, id, x, y, pressure, data);
        }

        /// <inheritdoc/>
        protected override void OnTouchMove(TouchDevice device, Int64 id, Double x, Double y, Double dx, Double dy, Single pressure, RoutedEventData data)
        {
            if (!data.Handled && device.IsFirstTouchInGesture(id))
            {
                if (dx != 0 && PART_HScroll != null)
                {
                    PART_HScroll.Value -= dx;
                }
                if (dy != 0 && PART_VScroll != null)
                {
                    PART_VScroll.Value -= dy;
                }
                data.Handled = true;
            }
            base.OnTouchMove(device, id, x, y, dx, dy, pressure, data);
        }

        /// <inheritdoc/>
        protected internal override Boolean HandlesScrolling
        {
            get { return true; }
        }

        /// <summary>
        /// Occurs when an element requests that it be brought into view.
        /// </summary>
        private static void HandleRequestBringIntoView(DependencyObject element, RectangleD targetRectangle, RoutedEventData data)
        {
            var scrollViewer = element as ScrollViewer;
            if (scrollViewer == null)
                return;

            var requester = data.OriginalSource as UIElement;
            if (requester == null || requester == scrollViewer)
                return;

            if (targetRectangle.IsEmpty)
                targetRectangle = new RectangleD(0, 0, requester.RenderSize.Width, requester.RenderSize.Height);

            var presenter = scrollViewer.PART_ContentPresenter;
            if (presenter == null)
                return;

            var boundsViewport = new RectangleD(scrollViewer.ContentHorizontalOffset, scrollViewer.ContentVerticalOffset,
                scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);
            var boundsRequester = RectangleD.TransformAxisAligned(requester.Bounds, requester.GetTransformToAncestorMatrix(scrollViewer));
            boundsRequester = RectangleD.Offset(boundsRequester, scrollViewer.ContentHorizontalOffset, scrollViewer.ContentVerticalOffset);

            var minX = 0.0;
            var minY = 0.0;

            var requesterIsToLeft =
                MathUtil.IsApproximatelyLessThan(boundsRequester.Left, boundsViewport.Left) &&
                MathUtil.IsApproximatelyLessThan(boundsRequester.Right, boundsViewport.Right);

            var requesterIsToRight =
                MathUtil.IsApproximatelyGreaterThan(boundsRequester.Left, boundsViewport.Left) &&
                MathUtil.IsApproximatelyGreaterThan(boundsRequester.Right, boundsViewport.Right);

            var requesterIsWider =
                boundsRequester.Width > boundsViewport.Width;

            if ((requesterIsToLeft && !requesterIsWider) || (requesterIsToRight && requesterIsWider))
            {
                minX = boundsRequester.Left;
            }
            else
            {
                if (requesterIsToLeft || requesterIsToRight)
                {
                    minX = boundsRequester.Right;
                }
                else
                {
                    minX = boundsViewport.Left;
                }
            }

            var requesterIsAbove =
                MathUtil.IsApproximatelyLessThan(boundsRequester.Top, boundsViewport.Top) &&
                MathUtil.IsApproximatelyLessThan(boundsRequester.Bottom, boundsViewport.Bottom);

            var requesterIsBelow =
                MathUtil.IsApproximatelyGreaterThan(boundsRequester.Top, boundsViewport.Top) &&
                MathUtil.IsApproximatelyGreaterThan(boundsRequester.Bottom, boundsViewport.Bottom);

            var requesterIsTaller =
                boundsRequester.Height > boundsViewport.Height;

            if ((requesterIsAbove && !requesterIsTaller) || (requesterIsBelow && requesterIsTaller))
            {
                minY = boundsRequester.Top;
            }
            else
            {
                if (requesterIsAbove || requesterIsBelow)
                {
                    minY = boundsRequester.Bottom;
                }
                else
                {
                    minY = boundsViewport.Top;
                }
            }

            scrollViewer.ChangeHorizontalOffset(minX, false);
            scrollViewer.ChangeVerticalOffset(minY, false);

            data.Handled = true;
        }

        /// <summary>
        /// Occurs when the user stops dragging one of the viewer's scroll thumbs.
        /// </summary>
        private static void HandleThumbDragCompleted(DependencyObject element, Double hchange, Double vchange, RoutedEventData data)
        {
            var scrollViewer = element as ScrollViewer;
            if (scrollViewer == null)
                return;

            var scrollBar = ((data.OriginalSource as Thumb)?.Parent as Track)?.TemplatedParent as OrientedScrollBar;
            if (scrollBar == null)
                return;

            if (scrollViewer.PART_HScroll == scrollBar)
                scrollViewer.ContentHorizontalOffset = scrollViewer.HorizontalOffset;

            if (scrollViewer.PART_VScroll == scrollBar)
                scrollViewer.ContentVerticalOffset = scrollViewer.VerticalOffset;
        }

        /// <summary>
        /// Handles the <see cref="RangeBase.ValueChanged"/> event for the scroll viewer's scroll bars.
        /// </summary>
        private static void HandleScrollBarValueChanged(DependencyObject element, RoutedEventData data)
        {
            var scrollViewer = (ScrollViewer)element;
            if (scrollViewer.PART_HScroll == data.OriginalSource || scrollViewer.PART_VScroll == data.OriginalSource)
            {
                scrollViewer.Position(scrollViewer.MostRecentPositionOffset);
                data.Handled = true;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ContentClipped"/> dependency property changes.
        /// </summary>
        private static void HandleContentClippedChanged(DependencyObject element, Boolean oldValue, Boolean newValue)
        {
            var scrollViewer = (ScrollViewer)element;
            if (scrollViewer.PART_ContentPresenter != null)
                scrollViewer.PART_ContentPresenter.Clip();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineDownCommand"/> command.
        /// </summary>
        private static void ExecutedLineDownCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.LineDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineUpCommand"/> command.
        /// </summary>
        private static void ExecutedLineUpCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.LineUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageDownCommand"/> command.
        /// </summary>
        private static void ExecutedPageDownCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.PageDown();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageUpCommand"/> command.
        /// </summary>
        private static void ExecutedPageUpCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.PageUp();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToBottomCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToBottomCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToBottom();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToTopCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToTopCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToTop();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToVerticalOffsetCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToVerticalOffsetCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (!(parameter is Double) && data.CommandValueParameter == null)
                return;

            var pvalue = data.CommandValueParameter.HasValue ? data.CommandValueParameter.Value.AsDouble() : (Double)parameter;

            var scrollViewer = element as ScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ChangeVerticalOffset(pvalue, false);
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.DeferScrollToVerticalOffsetCommand"/> command.
        /// </summary>
        private static void ExecutedDeferScrollToVerticalOffsetCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (!(parameter is Double) && data.CommandValueParameter == null)
                return;

            var pvalue = data.CommandValueParameter.HasValue ? data.CommandValueParameter.Value.AsDouble() : (Double)parameter;

            var scrollViewer = element as ScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ChangeVerticalOffset(pvalue, true);
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineRightCommand"/> command.
        /// </summary>
        private static void ExecutedLineRightCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.LineRight();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.LineLeftCommand"/> command.
        /// </summary>
        private static void ExecutedLineLeftCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.LineLeft();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageRightCommand"/> command.
        /// </summary>
        private static void ExecutedPageRightCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.PageRight();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.PageLeftCommand"/> command.
        /// </summary>
        private static void ExecutedPageLeftCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.PageLeft();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToRightEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToRightEndCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToRightEnd();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToLeftEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToLeftEndCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToLeftEnd();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToHorizontalOffsetCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToHorizontalOffsetCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (!(parameter is Double) && data.CommandValueParameter == null)
                return;

            var pvalue = data.CommandValueParameter.HasValue ? data.CommandValueParameter.Value.AsDouble() : (Double)parameter;

            var scrollViewer = element as ScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ChangeHorizontalOffset(pvalue, false);
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.DeferScrollToHorizontalOffsetCommand"/> command.
        /// </summary>
        private static void ExecutedDeferScrollToHorizontalOffsetCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            if (!(parameter is Double) && data.CommandValueParameter == null)
                return;

            var pvalue = data.CommandValueParameter.HasValue ? data.CommandValueParameter.Value.AsDouble() : (Double)parameter;

            var scrollViewer = element as ScrollViewer;
            if (scrollViewer != null)
                scrollViewer.ChangeHorizontalOffset(pvalue, true);
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToEndCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToEndCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToEnd();
        }

        /// <summary>
        /// Exeuctes the <see cref="ScrollBar.ScrollToHomeCommand"/> command.
        /// </summary>
        private static void ExecutedScrollToHomeCommand(DependencyObject element, ICommand command, Object parameter, ExecutedRoutedEventData data)
        {
            (element as ScrollViewer)?.ScrollToHome();
        }

        /// <summary>
        /// Determines whether a scroll command can execute.
        /// </summary>
        private static void CanExecuteScrollCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;
        }

        /// <summary>
        /// Determines whether the <see cref="ScrollBar.PageUpCommand"/> 
        /// and <see cref="ScrollBar.PageDownCommand"/> commands can execute.
        /// </summary>
        private static void CanExecutePageScrollCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;

            var scrollViewer = element as ScrollViewer;
            var scrollViewerParent = scrollViewer?.TemplatedParent as Control;
            if (scrollViewerParent != null && scrollViewerParent.HandlesScrolling)
            {
                data.CanExecute = false;
                data.ContinueRouting = true;
                data.Handled = true;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ScrollBar.DeferScrollToHorizontalOffsetCommand"/> 
        /// and <see cref="ScrollBar.DeferScrollToVerticalOffsetCommand"/> commands can execute.
        /// </summary>
        private static void CanExecuteDeferredScrollCommand(DependencyObject element, ICommand command, Object parameter, CanExecuteRoutedEventData data)
        {
            data.CanExecute = true;

            var scrollViewer = element as ScrollViewer;
            if (scrollViewer != null && !scrollViewer.IsDeferredScrollingEnabled)
            {
                data.CanExecute = false;
                data.Handled = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is at least partially visible within the scrollable area.
        /// </summary>
        private Boolean IsElementVisibleWithinScrollViewer(UIElement element)
        {
            if (PART_ContentPresenter == null || element == null)
                return false;

            if (!element.IsDescendantOf(PART_ContentPresenter))
                return false;

            return element.TransformedVisualBounds.Intersects(PART_ContentPresenter.TransformedVisualBounds);
        }

        /// <summary>
        /// Clamps the specified horizontal offset so that it falls within the scrollable area.
        /// </summary>
        private Double ClampHorizontalOffset(Double value) => Math.Max(0, Math.Min(value, ScrollableWidth));

        /// <summary>
        /// Clamps the specified vertical offset so that it falls within the scrollable area.
        /// </summary>
        private Double ClampVerticalOffset(Double value) => Math.Max(0, Math.Min(value, ScrollableHeight));

        /// <summary>
        /// Scrolls in response to keyboard input.
        /// </summary>
        private void HandleKeyInput(Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            var templatedParent = TemplatedParent as Control;
            if (templatedParent?.HandlesScrolling ?? false)
                return;

            if (data.OriginalSource == this)
            {
                HandleKeyScrolling(key, modifiers, data);
            }
            else
            {

                var isArrowKey = (key == Key.Left || key == Key.Right || key == Key.Up || key == Key.Down);
                if (isArrowKey)
                {
                    if (PART_ContentPresenter == null)
                    {
                        HandleKeyScrolling(key, modifiers, data);
                    }
                    else
                    {
                        var direction = FocusNavigator.ArrowKeyToFocusNavigationDirection(key);
                        var focusedCurrent = Keyboard.GetFocusedElement(View) as UIElement;
                        var focusedCurrentIsVisible = IsElementVisibleWithinScrollViewer(focusedCurrent);
                        var focusedNext = (focusedCurrentIsVisible ? focusedCurrent.PredictFocus(direction) : PART_ContentPresenter.PredictFocus(direction)) as UIElement;
                        if (focusedNext == null)
                        {
                            HandleKeyScrolling(key, modifiers, data);
                        }
                        else
                        {
                            var focusedNextIsVisible = IsElementVisibleWithinScrollViewer(focusedNext);
                            if (focusedNextIsVisible)
                            {
                                data.Handled = true;
                            }
                            else
                            {
                                HandleKeyScrolling(key, modifiers, data);
                                UpdateLayout();

                                focusedNextIsVisible = IsElementVisibleWithinScrollViewer(focusedNext);
                            }

                            if (focusedNextIsVisible)
                                focusedNext.Focus();
                        }
                    }
                }
                else
                {
                    HandleKeyScrolling(key, modifiers, data);
                }
            }
        }

        /// <summary>
        /// Scrolls in the direction corresponding to the given key.
        /// </summary>
        private void HandleKeyScrolling(Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            if ((modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                return;

            switch (key)
            {
                case Key.Left:
                    LineLeft();
                    data.Handled = true;
                    break;

                case Key.Right:
                    LineRight();
                    data.Handled = true;
                    break;

                case Key.Down:
                    LineDown();
                    data.Handled = true;
                    break;

                case Key.Up:
                    LineUp();
                    data.Handled = true;
                    break;

                case Key.PageUp:
                    PageUp();
                    data.Handled = true;
                    break;

                case Key.PageDown:
                    PageDown();
                    data.Handled = true;
                    break;

                case Key.Home:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ScrollToTop();
                    }
                    else
                    {
                        ScrollToLeftEnd();
                    }
                    data.Handled = true;
                    break;

                case Key.End:
                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ScrollToBottom();
                    }
                    else
                    {
                        ScrollToRightEnd();
                    }
                    data.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles the <see cref="UIElement.LayoutUpdated"/> event.
        /// </summary>
        private void OnLayoutUpdated(Object sender, EventArgs e)
        {
            var extentWidth = PART_ContentPresenter?.ExtentWidth ?? 0;
            SetValue(ExtentWidthPropertyKey, extentWidth);

            var extentHeight = PART_ContentPresenter?.ExtentHeight ?? 0;
            SetValue(ExtentHeightPropertyKey, extentHeight);

            var viewportWidth = PART_ContentPresenter?.ViewportWidth ?? 0;
            SetValue(ViewportWidthPropertyKey, viewportWidth);

            var viewportHeight = PART_ContentPresenter?.ViewportHeight ?? 0;
            SetValue(ViewportHeightPropertyKey, viewportHeight);

            var scrollableWidth = Math.Max(0, extentWidth - viewportWidth);
            SetValue(ScrollableWidthPropertyKey, scrollableWidth);

            var scrollableHeight = Math.Max(0, extentHeight - viewportHeight);
            SetValue(ScrollableHeightPropertyKey, scrollableHeight);

            var hOffset = ClampHorizontalOffset(HorizontalOffset);
            if (hOffset != HorizontalOffset)
                ChangeHorizontalOffset(hOffset, false);

            var vOffset = ClampVerticalOffset(VerticalOffset);
            if (vOffset != VerticalOffset)
                ChangeVerticalOffset(vOffset, false);
        }

        /// <summary>
        /// Changes the scroll viewer's horizontal offset to the specified value.
        /// </summary>
        private void ChangeHorizontalOffset(Double value, Boolean defer)
        {
            value = ClampHorizontalOffset(value);
            HorizontalOffset = value;

            if (!IsDeferredScrollingEnabled || !defer)
            {
                ContentHorizontalOffset = value;
                if (PART_HScroll != null)
                    PART_HScroll.Value = value;
            }
        }

        /// <summary>
        /// Changes the scroll viewer's vertical offset to the specified value.
        /// </summary>
        private void ChangeVerticalOffset(Double value, Boolean defer)
        {
            value = ClampVerticalOffset(value);
            VerticalOffset = value;

            if (!IsDeferredScrollingEnabled || !defer)
            {
                ContentVerticalOffset = value;
                if (PART_VScroll != null)
                    PART_VScroll.Value = value;
            }
        }

        // Scroll deltas for various input events.
        private const Double ScrollDeltaMouseWheel = 48.0;
        private const Double ScrollDeltaKey = 16.0;

        // Tracks the most recent scroll info values for ScrollChanged events.
        private Double oldHorizontalOffset;
        private Double oldVerticalOffset;
        private Double oldExtentWidth;
        private Double oldExtentHeight;
        private Double oldViewportWidth;
        private Double oldViewportHeight;
        
        // Control component references.
        private readonly ScrollContentPresenter PART_ContentPresenter = null;
        private readonly HScrollBar PART_HScroll = null;
        private readonly VScrollBar PART_VScroll = null;
    }
}
