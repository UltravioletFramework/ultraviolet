using System;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Documents;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents an element which is used to indicate the position of child content within a component template.
    /// </summary>
    [UvmlKnownType]
    public class ContentPresenter : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPresenter"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public ContentPresenter(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the content presenter's content.
        /// </summary>
        /// <value>The <see cref="Object"/> that is displayed by the control. The
        /// default value is <see langword="null"/></value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentProperty"/></dpropField>
        ///     <dpropStylingName>content</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.CoerceObjectToString"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Object Content
        {
            get { return GetValue<Object>(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the content presenter's content when that content
        /// is being displayed as string.
        /// </summary>
        /// <value>A format string that specifies how to format the control's content. The default
        /// value is <see langword="null"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentStringFormatProperty"/></dpropField>
        ///     <dpropStylingName>content-string-format</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String ContentStringFormat
        {
            get { return GetValue<String>(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name to use when aliasing the <see cref="Content"/> and <see cref="ContentStringFormat"/> properties.
        /// </summary>
        /// <value>The name to use when aliasing the <see cref="Content"/> and <see cref="ContentStringFormat"/> properties.</value>
        /// <remarks>
        /// <para>When a <see cref="ContentPresenter"/> is part of a component template, its <see cref="Content"/> and 
        /// <see cref="ContentStringFormat"/> properties will automatically be bound to corresponding properties on the
        /// template's data source. By default, each property will be bound to a property on the data source with the same name.
        /// When <see cref="ContentSource"/> has a non-<see langword="null"/> value, the content presenter will be automatically
        /// bound to properties which match the specified value instead. For example, if the value of <see cref="ContentSource"/>
        /// is set to "<code>Foo</code>", then the <see cref="Content"/> property will be bound to a property
        /// called <code>Foo</code>, and the <see cref="ContentStringFormat"/> property will be bound to a property
        /// called <code>FooStringFormat</code>.</para>
        /// <dprop>
        ///     <dpropField><see cref="ContentSourceProperty"/></dpropField>
        ///     <dpropStylingName>content-source</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public String ContentSource
        {
            get { return GetValue<String>(ContentSourceProperty); }
            set { SetValue(ContentSourceProperty, value); }
        }

        /// <summary>
        /// Gets the distance in device-independent pixels by which the presenter's content is offset.
        /// </summary>
        /// <value>A <see cref="Size2D"/> that represents the distance in device-independent pixels
        /// by which the presenter's content is offset along both axes. The default value is
        /// <see cref="Size2D.Zero"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="ContentOffsetProperty"/></dpropField>
        ///     <dpropStylingName>content-offset</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Size2D ContentOffset
        {
            get { return GetValue<Size2D>(ContentOffsetProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="Content"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Content"/> dependency property.</value>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(ContentPresenter),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString, HandleContentChanged));

        /// <summary>
        /// Identifies the <see cref="ContentStringFormat"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentStringFormat"/> dependency property.</value>
        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(String), typeof(ContentPresenter),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure, HandleContentStringFormatChanged));

        /// <summary>
        /// Identifies the <see cref="ContentSource"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentSource"/> dependency property.</value>
        public static readonly DependencyProperty ContentSourceProperty = DependencyProperty.Register("ContentSource", typeof(String), typeof(ContentPresenter),
            new PropertyMetadata<String>());

        /// <summary>
        /// Identifies the <see cref="ContentOffset"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="ContentOffset"/> dependency property.</value>
        public static readonly DependencyProperty ContentOffsetProperty = DependencyProperty.Register("ContentOffset", typeof(Size2D), typeof(ContentPresenter),
            new PropertyMetadata<Size2D>(HandleContentOffsetChanged));

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            UpdateTextParserCache();
            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            if (newView != null)
            {
                UpdateTextParserCache();
            }
            base.OnViewChanged(oldView, newView);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseDevice device, Double x, Double y, Double dx, Double dy, RoutedEventData data)
        {
            LinkUtil.UpdateLinkCursor(textLayoutCommands, this, Mouse.GetPosition(this));

            base.OnMouseMove(device, x, y, dx, dy, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseDevice device, RoutedEventData data)
        {
            LinkUtil.UpdateLinkCursor(textLayoutCommands, this, Mouse.GetPosition(this));

            base.OnMouseLeave(device, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                LinkUtil.ActivateTextLink(textLayoutCommands, this, data);
            }
            base.OnMouseDown(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseDevice device, MouseButton button, RoutedEventData data)
        {
            if (button == MouseButton.Left)
            {
                LinkUtil.ExecuteTextLink(textLayoutCommands, this, data);
            }
            base.OnMouseUp(device, button, data);
        }

        /// <inheritdoc/>
        protected override void OnIsMouseOverChanged()
        {
            LinkUtil.UpdateLinkCursor(textLayoutCommands, this, null);

            base.OnIsMouseOverChanged();
        }
        
        /// <inheritdoc/>
        protected override void CacheLayoutParametersOverride()
        {
            DigestDataBoundContentProperties();

            base.CacheLayoutParametersOverride();
        }

        /// <inheritdoc/>
        protected override void UpdateOverride(UltravioletTime time)
        {
            var font = GetValue<SourcedResource<UltravioletFont>>(TextElement.FontProperty);
            if (textLayoutCommands != null && textLayoutCommands.Settings.Font != font.Resource?.Value)
            {
                InvalidateMeasure();
            }
            base.UpdateOverride(time);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutCommands != null && textLayoutCommands.Count > 0)
            {
                var foreground = GetValue<Color>(TextElement.ForegroundProperty);
                var positionRaw = Display.DipsToPixels(UntransformedAbsolutePosition + ContentOffset);
                var positionX = dc.IsTransformed ? positionRaw.X : Math.Floor(positionRaw.X);
                var positionY = dc.IsTransformed ? positionRaw.Y : Math.Floor(positionRaw.Y);
                var position = new Vector2((Single)positionX, (Single)positionY);
                View.Resources.TextRenderer.Draw((SpriteBatch)dc, textLayoutCommands, position, foreground * dc.Opacity);
            }

            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            DigestDataBoundContentProperties();

            var content = Content;
            if (content == null)
                return Size2D.Zero;

            var contentElement = content as UIElement;
            if (contentElement != null)
            {
                contentElement.Measure(availableSize);

                if (textParserResult != null)
                    textParserResult.Clear();

                if (textLayoutCommands != null)
                    textLayoutCommands.Clear();

                return new Size2D(
                    Math.Min(availableSize.Width, contentElement.DesiredSize.Width),
                    Math.Min(availableSize.Height, contentElement.DesiredSize.Height));
            }
            else
            {
                if (textParserResult == null || textParserResult.Count == 0)
                {
                    var font = GetValue<SourcedResource<UltravioletFont>>(TextElement.FontProperty);
                    if (font.IsLoaded)
                    {
                        var lineSpacing = font.Resource.Value.Regular.LineSpacing;
                        return new Size2D(0, Math.Min(availableSize.Height, Display.PixelsToDips(lineSpacing)));
                    }
                    return Size2D.Zero;
                }

                UpdateTextLayoutCache(availableSize);

                var textWidth  = Display.PixelsToDips(textLayoutCommands.ActualWidth);
                var textHeight = Display.PixelsToDips(textLayoutCommands.ActualHeight);
                return new Size2D(textWidth, textHeight);
            }
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            DigestDataBoundContentProperties();

            var content = Content;

            var contentElement = content as UIElement;
            if (contentElement != null)
            {
                var contentElementRect = new RectangleD(0, 0, finalSize.Width, finalSize.Height);
                contentElement.Arrange(contentElementRect, options);

                return finalSize;
            }
            else
            {
                if (textParserResult != null && textParserResult.Count > 0)
                {
                    if (textLayoutCommands.Settings.Width != finalSize.Width || textLayoutCommands.Settings.Height != finalSize.Height)
                        UpdateTextLayoutCache(finalSize);

                    return finalSize;
                }
            }

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override void PositionChildrenOverride()
        {
            VisualTreeHelper.ForEachChild<UIElement>(this, this, (child, state) =>
            {
                var offset = ((ContentPresenter)state).ContentOffset;
                child.Position(offset);
                child.PositionChildren();
            });
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipOverride()
        {
            var contentElement = Content as UIElement;
            if (contentElement != null)
            {
                if (!Bounds.Contains(contentElement.UntransformedRelativeBounds))
                {
                    return UntransformedAbsoluteBounds;
                }
            }

            return base.ClipOverride();
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            return base.GetLogicalChild(childIndex);
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            var contentElement = Content as UIElement;
            if (contentElement == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return contentElement;
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get
            {
                return base.LogicalChildrenCount;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get 
            {
                return Content is UIElement ? 1 : 0;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Content"/> dependency property changes.
        /// </summary>
        private static void HandleContentChanged(DependencyObject dobj, Object oldValue, Object newValue)
        {
            var contentPresenter = (ContentPresenter)dobj;

            var oldElement = oldValue as UIElement;
            if (oldElement != null)
            {
                oldElement.ChangeVisualParent(null);
                oldElement.CacheLayoutParameters();
            }

            var newElement = newValue as UIElement;
            if (newElement != null)
            {
                newElement.CacheLayoutParameters();
                newElement.ChangeVisualParent(contentPresenter);
            }
            else
            {
                contentPresenter.UpdateTextParserCache();
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ContentStringFormat"/> dependency property changes.
        /// </summary>
        private static void HandleContentStringFormatChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var contentPresenter = (ContentPresenter)dobj;
            contentPresenter.UpdateTextParserCache();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ContentOffset"/> dependency property changes.
        /// </summary>
        private static void HandleContentOffsetChanged(DependencyObject dobj, Size2D oldValue, Size2D newValue)
        {
            var presenter = (ContentPresenter)dobj;
            presenter.Position(presenter.MostRecentPositionOffset);
            presenter.PositionChildren();
        }
        
        /// <summary>
        /// Updates the cache which contains the element's parsed text.
        /// </summary>
        private void UpdateTextParserCache()
        {
            if (textParserResult != null)
                textParserResult.Clear();

            if (View == null)
                return;

            var content = Content;

            var contentElement = content as UIElement;
            if (contentElement == null)
            {
                if (textParserResult == null)
                    textParserResult = new TextParserTokenStream();

                var contentAsString = default(String);
                var contentFormat = ContentStringFormat;
                if (contentFormat == null)
                {
                    contentAsString = (content == null) ? String.Empty : content.ToString();
                }
                else
                {
                    contentAsString = String.Format(contentFormat, content);
                }

                View.Resources.TextRenderer.Parser.Parse(contentAsString, textParserResult);
            }

            InvalidateArrange();
        }

        /// <summary>
        /// Updates the cache which contains the element's laid-out text.
        /// </summary>
        /// <param name="availableSize">The amount of space in which the element's text can be laid out.</param>
        private void UpdateTextLayoutCache(Size2D availableSize)
        {
            if (textLayoutCommands != null)
                textLayoutCommands.Clear();

            if (View == null)
                return;

            var content = Content;

            var contentElement = content as UIElement;
            if (contentElement == null)
            {
                if (textLayoutCommands == null)
                    textLayoutCommands = new TextLayoutCommandStream();

                var font = GetValue<SourcedResource<UltravioletFont>>(TextElement.FontProperty);
                var fontStyle = GetValue<UltravioletFontStyle>(TextElement.FontStyleProperty);
                if (font.IsLoaded)
                {
                    var availableSizeInPixels = Display.DipsToPixels(availableSize);

                    var cursorpos = textLayoutCommands.CursorPosition;

                    var textRenderingMode = TextOptions.GetTextRenderingMode(this);
                    var textScript = TextOptions.GetTextScript(this);
                    var textLanguage = TextOptions.GetTextLanguage(this);
                    var textDirection = FlowDirection == FlowDirection.RightToLeft ? TextDirection.RightToLeft : TextDirection.LeftToRight;

                    var options = (textRenderingMode == TextRenderingMode.Shaped) ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                    var flags = LayoutUtil.ConvertAlignmentsToTextFlags(HorizontalAlignment, VerticalAlignment);
                    var settings = new TextLayoutSettings(font,
                        (Int32)Math.Ceiling(availableSizeInPixels.Width),
                        (Int32)Math.Ceiling(availableSizeInPixels.Height), flags, options, textDirection, textScript, fontStyle, null, textLanguage);

                    View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutCommands, settings);
                    View.Resources.TextRenderer.UpdateCursor(textLayoutCommands, cursorpos);
                }
            }
        }     

        /// <summary>
        /// Digests any content-related dependency properties on this control which are currently data bound.
        /// </summary>
        private void DigestDataBoundContentProperties()
        {
            DigestImmediatelyIfDataBound(ContentSourceProperty);
            DigestImmediatelyIfDataBound(ContentStringFormatProperty);
            DigestImmediatelyIfDataBound(ContentProperty);
        }

        // Cached parser/layout results for content text.
        private TextParserTokenStream textParserResult;
        private TextLayoutCommandStream textLayoutCommands;        
    }
}
