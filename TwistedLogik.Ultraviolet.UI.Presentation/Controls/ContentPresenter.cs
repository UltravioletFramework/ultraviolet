using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
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
        public Object Content
        {
            get { return GetValue<Object>(ContentProperty); }
            set { SetValue<Object>(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the formatting string used to format the content presenter's content when that content
        /// is being displayed as string.
        /// </summary>
        public String ContentStringFormat
        {
            get { return GetValue<String>(ContentStringFormatProperty); }
            set { SetValue<String>(ContentStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name to use when aliasing the <see cref="Content"/> and <see cref="ContentStringFormat"/> properties.
        /// </summary>
        public String ContentSource
        {
            get { return GetValue<String>(ContentSourceProperty); }
            set { SetValue<String>(ContentSourceProperty, value); }
        }

        /// <inheritdoc/>
        public Size2D ContentOffset
        {
            get { return GetValue<Size2D>(ContentOffsetProperty); }
        }

        /// <summary>
        /// Identifies the <see cref="Content"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content'.</remarks>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(ContentPresenter),
            new PropertyMetadata<Object>(null, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.CoerceObjectToString, HandleContentChanged));

        /// <summary>
        /// Identifies the <see cref="ContentStringFormat"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-string-format'.</remarks>
        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(String), typeof(ContentPresenter),
            new PropertyMetadata<String>(null, PropertyMetadataOptions.AffectsMeasure, HandleContentStringFormatChanged));

        /// <summary>
        /// Identifies the <see cref="ContentSource"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-source'.</remarks>
        public static readonly DependencyProperty ContentSourceProperty = DependencyProperty.Register("ContentSource", typeof(String), typeof(ContentPresenter),
            new PropertyMetadata<String>());

        /// <summary>
        /// Identifies the <see cref="ContentOffset"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'content-offset'.</remarks>
        public static readonly DependencyProperty ContentOffsetProperty = DependencyProperty.Register("ContentOffset", typeof(Size2D), typeof(ContentPresenter),
            new PropertyMetadata<Size2D>(HandleContentOffsetChanged));

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            UpdateTextParserCache();
            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            containingControl = FindContainingControl();

            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutResult != null && textLayoutResult.Count > 0 && containingControl != null)
            {
                var position = Display.DipsToPixels(AbsolutePosition + ContentOffset);
                var color    = containingControl.Foreground;

                View.Resources.TextRenderer.Draw(dc.SpriteBatch, textLayoutResult, (Vector2)position, color);
            }

            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var content = Content;
            var contentText = content as String;
            if (contentText != null)
            {
                if (contentText == String.Empty)
                {
                    if (containingControl != null && containingControl.Font.IsLoaded)
                    {
                        var lineSpacing = containingControl.Font.Resource.Value.Regular.LineSpacing;
                        return new Size2D(0, Math.Min(availableSize.Height, Display.PixelsToDips(lineSpacing)));
                    }
                    return Size2D.Zero;
                }

                UpdateTextParserCache();
                UpdateTextLayoutCache(availableSize);

                var textWidth  = Display.PixelsToDips(textLayoutResult.ActualWidth);
                var textHeight = Display.PixelsToDips(textLayoutResult.ActualHeight);
                return new Size2D(textWidth, textHeight);
            }
            else
            {
                if (textParserResult != null)
                    textParserResult.Clear();

                if (textLayoutResult != null)
                    textLayoutResult.Clear();
            }

            var contentElement = content as UIElement;
            if (contentElement != null)
            {
                contentElement.Measure(availableSize);
                return new Size2D(
                    Math.Min(availableSize.Width, contentElement.DesiredSize.Width),
                    Math.Min(availableSize.Height, contentElement.DesiredSize.Height));
            }

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var content = Content;

            var contentText = content as String;
            if (contentText != null)
            {
                UpdateTextLayoutCache(finalSize);
                return finalSize;
            }

            var contentElement = content as UIElement;
            if (contentElement != null)
            {
                var contentElementRect = new RectangleD(0, 0, finalSize.Width, finalSize.Height);
                contentElement.Arrange(contentElementRect, options);

                return finalSize;
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
        protected override RectangleD? ClipCore()
        {
            var contentElement = Content as UIElement;
            if (contentElement != null)
            {
                if (!AbsoluteBounds.Contains(contentElement.AbsoluteBounds))
                {
                    return AbsoluteBounds;
                }
            }

            return base.ClipCore();
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
            }

            var newElement = newValue as UIElement;
            if (newElement != null)
            {
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
        /// Finds the <see cref="Control"/> which contains the content presenter - either the template parent, if
        /// it has one, or the nearest ancestor in the logical tree of type <see cref="Control"/>.
        /// </summary>
        private Control FindContainingControl()
        {
            var container = TemplatedParent as Control;
            if (containingControl == null)
            {
                var current = LogicalTreeHelper.GetParent(this);
                while (current != null)
                {
                    var control = current as Control;
                    if (control != null)
                    {
                        container = control;
                        break;
                    }
                    current = LogicalTreeHelper.GetParent(current);
                }
            }
            return container;
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
                    textParserResult = new TextParserResult();

                var contentAsString = String.Format(ContentStringFormat ?? "{0}", content);
                View.Resources.TextRenderer.Parse(contentAsString, textParserResult);
            }

            InvalidateArrange();
        }

        /// <summary>
        /// Updates the cache which contains the element's laid-out text.
        /// </summary>
        /// <param name="availableSize">The amount of space in which the element's text can be laid out.</param>
        private void UpdateTextLayoutCache(Size2D availableSize)
        {
            if (textLayoutResult != null)
                textLayoutResult.Clear();

            if (View == null || containingControl == null)
                return;

            var content = Content;

            var contentElement = content as UIElement;
            if (contentElement == null)
            {
                if (textLayoutResult == null)
                    textLayoutResult = new TextLayoutResult();

                var font = containingControl.Font;
                if (font.IsLoaded)
                {
                    var availableSizeInPixels = Display.DipsToPixels(availableSize);

                    var settings = new TextLayoutSettings(font,
                        (Int32)availableSizeInPixels.Width,
                        (Int32)availableSizeInPixels.Height, TextFlags.Standard, containingControl.FontStyle);

                    View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutResult, settings);
                }
            }
        }

        // Cached parser/layout results for content text.
        private TextParserResult textParserResult;
        private TextLayoutResult textLayoutResult;

        // Cached layout parameters.
        private Control containingControl;
    }
}
