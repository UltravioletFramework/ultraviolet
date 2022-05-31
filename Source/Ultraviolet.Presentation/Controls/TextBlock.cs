using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents a lightweight control for displaying text.
    /// </summary>
    [UvmlKnownType]
    [UvmlDefaultProperty("Text")]
    public class TextBlock : TextBlockBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlock"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TextBlock(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets the text block's text.
        /// </summary>
        /// <returns>A <see cref="String"/> instance containing the text block's text.</returns>
        public String GetText()
        {
            return GetValue<VersionedStringSource>(TextProperty).ToString();
        }

        /// <summary>
        /// Gets the text block's text.
        /// </summary>
        /// <param name="stringBuilder">A <see cref="StringBuilder"/> instance to populate with the text block's text.</param>
        public void GetText(StringBuilder stringBuilder)
        {
            Contract.Require(stringBuilder, nameof(stringBuilder));

            var value = GetValue<VersionedStringSource>(TextProperty);

            stringBuilder.Length = 0;
            stringBuilder.AppendVersionedStringSource(value);
        }

        /// <summary>
        /// Sets the text block's text.
        /// </summary>
        /// <param name="value">A <see cref="String"/> instance to set as the text block's text.</param>
        public void SetText(String value)
        {
            SetValue(TextProperty, new VersionedStringSource(value));
        }

        /// <summary>
        /// Sets the text block's text.
        /// </summary>
        /// <param name="value">A <see cref="StringBuilder"/> instance whose contents will be set as the text block's text.</param>
        public void SetText(StringBuilder value)
        {
            SetValue(TextProperty, (value == null) ? VersionedStringSource.Invalid : new VersionedStringSource(value.ToString()));
        }

        /// <summary>
        /// Gets or sets the text block's text source.
        /// </summary>
        /// <value>A <see cref="VersionedStringSource"/> which represents the source of the text block's text.
        /// The default value is <see cref="VersionedStringSource.Invalid"/>.</value>
        /// <remarks>
        /// <para>In most cases, rather than using this property to access the element's text, you should use the <see cref="GetText(StringBuilder)"/>
        /// and <see cref="SetText(StringBuilder)"/> methods in order to avoid allocating onto the managed heap.</para>
        /// <dprop>
        ///     <dpropField><see cref="TextProperty"/></dpropField>
        ///     <dpropStylingName>text</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        public VersionedStringSource Text
        {
            get { return GetValue<VersionedStringSource>(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Text"/> dependency property.</value>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(VersionedStringSource), typeof(TextBlock),
            new PropertyMetadata<VersionedStringSource>(VersionedStringSource.Invalid, PropertyMetadataOptions.AffectsMeasure, HandleTextChanged));

        /// <inheritdoc/>
        protected override void OnViewChanged(PresentationFoundationView oldView, PresentationFoundationView newView)
        {
            UpdateTextParserResult();

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
        protected override void OnInitialized()
        {
            UpdateTextParserResult();
            base.OnInitialized();
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (textLayoutCommands.Count > 0)
            {
                var positionRaw = Display.DipsToPixels(UntransformedAbsolutePosition);
                var positionX = dc.IsTransformed ? positionRaw.X : Math.Floor(positionRaw.X);
                var positionY = dc.IsTransformed ? positionRaw.Y : Math.Floor(positionRaw.Y);
                var position = new Vector2((Single)positionX, (Single)positionY);
                View.Resources.TextRenderer.Draw((SpriteBatch)dc, textLayoutCommands, position, Foreground * dc.Opacity);
            }
            base.DrawOverride(time, dc);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            UpdateTextLayoutResult(availableSize);

            var sizePixels = new Size2D(textLayoutCommands.ActualWidth, textLayoutCommands.ActualHeight);
            var sizeDips = Display.PixelsToDips(sizePixels);

            return sizeDips;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (textLayoutCommands.Settings.Width != finalSize.Width || textLayoutCommands.Settings.Height != finalSize.Height)
                UpdateTextLayoutResult(finalSize);

            return finalSize;
        }

        /// <summary>
        /// Occurs when the value of the Text dependency property changes.
        /// </summary>
        private static void HandleTextChanged(DependencyObject dobj, VersionedStringSource oldValue, VersionedStringSource newValue)
        {
            var label = (TextBlock)dobj;
            label.UpdateTextParserResult();
        }

        /// <summary>
        /// Updates the cache that contains the result of parsing the label's text.
        /// </summary>
        private void UpdateTextParserResult()
        {
            textParserResult.Clear();

            if (View == null)
                return;

            var text = GetValue<VersionedStringSource>(TextProperty);
            if (text.IsValid)
            {
                var textString = text.ToString();
                View.Resources.TextRenderer.Parser.Parse(textString, textParserResult);
            }
        }

        /// <summary>
        /// Updates the cache that contains the result of laying out the label's text.
        /// </summary>
        /// <param name="availableSize">The size of the space that is available for laying out text.</param>
        private void UpdateTextLayoutResult(Size2D availableSize)
        {
            textLayoutCommands.Clear();

            if (textParserResult.Count > 0 && Font.IsLoaded)
            {
                var unconstrainedWidth = Double.IsPositiveInfinity(availableSize.Width) && HorizontalAlignment != HorizontalAlignment.Stretch;
                var unconstrainedHeight = Double.IsPositiveInfinity(availableSize.Height) && VerticalAlignment != VerticalAlignment.Stretch;

                var constraintX = unconstrainedWidth ? null : (Int32?)Math.Ceiling(Display.DipsToPixels(availableSize.Width));
                var constraintY = unconstrainedHeight ? null : (Int32?)Math.Ceiling(Display.DipsToPixels(availableSize.Height));

                var cursorpos = textLayoutCommands.CursorPosition;

                var textRenderingMode = TextOptions.GetTextRenderingMode(this);
                var textScript = TextOptions.GetTextScript(this);
                var textLanguage = TextOptions.GetTextLanguage(this);
                var textDirection = FlowDirection == FlowDirection.RightToLeft ? TextDirection.RightToLeft : TextDirection.LeftToRight;

                var options = (textRenderingMode == TextRenderingMode.Shaped) ? TextLayoutOptions.Shape : TextLayoutOptions.None;
                var flags = LayoutUtil.ConvertAlignmentsToTextFlags(HorizontalContentAlignment, VerticalContentAlignment);
                var settings = new TextLayoutSettings(Font, constraintX, constraintY, flags, options, textDirection, textScript, FontStyle, null, textLanguage);

                View.Resources.TextRenderer.CalculateLayout(textParserResult, textLayoutCommands, settings);
                View.Resources.TextRenderer.UpdateCursor(textLayoutCommands, cursorpos);
            }
        }        

        // State values.
        private readonly TextParserTokenStream textParserResult = new TextParserTokenStream();
        private readonly TextLayoutCommandStream textLayoutCommands = new TextLayoutCommandStream();
    }
}
