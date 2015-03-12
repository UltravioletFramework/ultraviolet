using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
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
        /// <param name="id">The element's unique identifier within its view.</param>
        public ContentPresenter(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override void CacheLayoutParametersCore()
        {
            containingContentControl = Control as ContentControl;

            base.CacheLayoutParametersCore();
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (ContainingContentControl != null)
            {
                if (textLayoutResult != null && textLayoutResult.Count > 0)
                {
                    var position = Display.DipsToPixels(AbsolutePosition);
                    var color    = ContainingContentControl.FontColor;

                    View.Resources.TextRenderer.Draw(dc.SpriteBatch, textLayoutResult, (Vector2)position, color);
                }
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (ContainingContentControl == null)
                return availableSize;

            var content = ContainingContentControl.Content;
            var contentText = content as String;
            if (contentText != null)
            {
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
                return contentElement.DesiredSize;
            }

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            var container = ContainingContentControl;
            if (container == null)
                return finalSize;

            var content = container.Content;
            var contentText = content as String;
            if (contentText != null)
            {
                UpdateTextLayoutCache(finalSize);
                return finalSize;
            }

            var contentElement = content as UIElement;
            if (contentElement != null)
            {
                var hAlign = container.HorizontalContentAlignment;
                var vAlign = container.VerticalContentAlignment;

                var offsetX = LayoutUtil.PerformHorizontalAlignment(finalSize, contentElement.RenderSize, hAlign);
                var offsetY = LayoutUtil.PerformVerticalAlignment(finalSize, contentElement.RenderSize, vAlign);

                contentElement.Arrange(new RectangleD(offsetX, offsetY, finalSize.Width, finalSize.Height), options);
                return finalSize;
            }

            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipCore()
        {
            if (ContainingContentControl != null)
            {
                var contentElement = ContainingContentControl.Content as UIElement;
                if (contentElement != null)
                {
                    if (contentElement.RenderSize.Width > RenderSize.Width || 
                        contentElement.RenderSize.Height > RenderSize.Height)
                    {
                        return AbsoluteBounds;
                    }
                }
            }
            return base.ClipCore();
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            if (ContainingContentControl == null || ContainingContentControl.TreatContentAsLogicalChild)
                throw new ArgumentOutOfRangeException("childIndex");

            var contentElement = ContainingContentControl.Content as UIElement;
            if (contentElement == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return contentElement;
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            if (ContainingContentControl == null)
                throw new ArgumentOutOfRangeException("childIndex");

            var contentElement = ContainingContentControl.Content as UIElement;
            if (contentElement == null || childIndex != 0)
                throw new ArgumentOutOfRangeException("childIndex");

            return contentElement;
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get
            {
                if (ContainingContentControl == null || ContainingContentControl.TreatContentAsLogicalChild)
                    return 0;

                return ContainingContentControl.Content is UIElement ? 1 : 0;
            }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get 
            {
                return (ContainingContentControl != null && ContainingContentControl.Content is UIElement) ? 1 : 0;
            }
        }

        /// <summary>
        /// Updates the cache which contains the element's parsed text.
        /// </summary>
        private void UpdateTextParserCache()
        {
            if (textParserResult != null)
                textParserResult.Clear();

            if (View == null || ContainingContentControl == null)
                return;

            var content = ContainingContentControl.Content;

            var contentElement = content as UIElement;
            if (contentElement == null)
            {
                if (textParserResult == null)
                    textParserResult = new TextParserResult();

                var contentAsString = content.ToString();
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

            if (View == null)
                return;

            var content = ContainingContentControl.Content;

            var contentElement = content as UIElement;
            if (contentElement == null)
            {
                if (textLayoutResult == null)
                    textLayoutResult = new TextLayoutResult();

                var availableSizeInPixels = Display.DipsToPixels(availableSize);

                var hAlign = ContainingContentControl.HorizontalContentAlignment;
                var vAlign = ContainingContentControl.VerticalContentAlignment;

                var flags    = LayoutUtil.ConvertAlignmentsToTextFlags(hAlign, vAlign);
                var settings = new TextLayoutSettings(Font, 
                    (Int32)availableSizeInPixels.Width, 
                    (Int32)availableSizeInPixels.Height, flags, FontStyle);
                View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutResult, settings);
            }
        }

        /// <summary>
        /// Gets the <see cref="ContentControl"/> that contains this element.
        /// </summary>
        private ContentControl ContainingContentControl
        {
            get { return containingContentControl; }
        }

        // Cached parser/layout results for content text.
        private TextParserResult textParserResult;
        private TextLayoutResult textLayoutResult;

        // Cached layout parameters
        private ContentControl containingContentControl;
    }
}
