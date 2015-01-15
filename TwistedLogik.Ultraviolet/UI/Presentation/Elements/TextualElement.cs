using System;
using System.ComponentModel;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a UI element which is primarily concerned with rendering a single string of text.
    /// </summary>
    [DefaultProperty("Text")]
    public abstract class TextualElement : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's identifier.</param>
        public TextualElement(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        protected override Size2 MeasureCore(Size2 availableSize)
        {
            if (Font == null || String.IsNullOrEmpty(Text))
                return Size2.Zero;

            var settings = new TextLayoutSettings(Font, availableSize.Width, availableSize.Height, TextAlignment);
            UIElementResources.TextRenderer.CalculateLayout(cachedParserResult, cachedLayoutResultTemp, settings);

            return new Size2(cachedLayoutResultTemp.ActualWidth, cachedLayoutResultTemp.ActualHeight);
        }

        protected override void ArrangeCore(Rectangle finalRect)
        {
            UpdateCachedTextLayout(finalRect);

            base.ArrangeCore(finalRect);
        }


        /// <summary>
        /// Gets or sets the label's text.
        /// </summary>
        public String Text
        {
            get { return GetValue<String>(TextProperty); }
            set { SetValue<String>(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label's text alignment flags.
        /// </summary>
        public TextFlags TextAlignment
        {
            get { return GetValue<TextFlags>(TextAlignmentProperty); }
            set { SetValue<TextFlags>(TextAlignmentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(TextualElement),
            new DependencyPropertyMetadata(HandleTextChanged, null, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        [Styled("text-align")]
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextFlags), typeof(TextualElement),
            new DependencyPropertyMetadata(HandleTextAlignmentChanged, () => TextFlags.AlignCenter | TextFlags.AlignMiddle, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnFontAssetIDChanged()
        {
            if (Parent != null)
            {
                Parent.InvalidateMeasure();
                Parent.InvalidateArrange();
            }
            base.OnFontAssetIDChanged();
        }

        /// <summary>
        /// Updates the parsed text cache.
        /// </summary>
        protected void UpdateCachedTextParse()
        {
            if (String.IsNullOrEmpty(Text))
            {
                cachedParserResult.Clear();
            }
            else
            {
                UIElementResources.TextRenderer.Parse(Text, cachedParserResult);
            }

            if (Parent != null)
            {
                Parent.InvalidateMeasure();
                Parent.InvalidateArrange();
            }
        }

        /// <summary>
        /// Updates the text layout cache.
        /// </summary>
        protected void UpdateCachedTextLayout(Rectangle finalRect)
        {
            cachedLayoutResult.Clear();

            if (String.IsNullOrEmpty(Text))
                return;

            if (cachedParserResult.Count > 0 && Font != null)
            {
                var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

                var padding  = display.DipsToPixels(Padding);
                var width    = finalRect.Width  - ((Int32)padding.Left + (Int32)padding.Right);
                var height   = finalRect.Height - ((Int32)padding.Top + (Int32)padding.Bottom);
                var settings = new TextLayoutSettings(Font, width, height, TextAlignment);
                UIElementResources.TextRenderer.CalculateLayout(cachedParserResult, cachedLayoutResult, settings);
            }
        }

        /// <summary>
        /// Draws the element's text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected void DrawText(SpriteBatch spriteBatch, Single opacity)
        {
            if (cachedLayoutResult.Count > 0)
            {
                var padding  = MeasureUtil.ConvertThicknessToPixels(Ultraviolet, Padding, 0);                
                var position = new Vector2(
                    AbsoluteScreenX + (Int32)padding.Left + TextAreaX,
                    AbsoluteScreenY + (Int32)padding.Top  + TextAreaY);

                UIElementResources.TextRenderer.Draw(spriteBatch, CachedLayoutResult, position, FontColor * Opacity * opacity);
            }
        }

        /// <summary>
        /// Gets the distance between the left edge of the element's content area and
        /// the left edge of the area in which the element draws its text.
        /// </summary>
        protected virtual Int32 TextAreaX
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the distance between the top edge of the element's content area and
        /// the top edge of the area in which the element draws its text.
        /// </summary>
        protected virtual Int32 TextAreaY
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the width of the area in which the element draws its text.
        /// </summary>
        protected virtual Int32 TextAreaWidth
        {
            get { return ActualWidth; }
        }

        /// <summary>
        /// Gets the height of the area in which the element draws its text.
        /// </summary>
        protected virtual Int32 TextAreaHeight
        {
            get { return ActualHeight; }
        }

        /// <summary>
        /// Gets the cached result of parsing the element's text.
        /// </summary>
        protected TextParserResult CachedParserResult
        {
            get { return cachedParserResult; }
        }

        /// <summary>
        /// Gets the cached result of laying out the element's text.
        /// </summary>
        protected TextLayoutResult CachedLayoutResult
        {
            get { return cachedLayoutResult; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextChanged(DependencyObject dobj)
        {
            var element = (TextualElement)dobj;
            element.UpdateCachedTextParse();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextAlignmentChanged(DependencyObject dobj)
        {
            var label = (TextualElement)dobj;

        }

        // State values.
        private readonly TextParserResult cachedParserResult = new TextParserResult();
        private readonly TextLayoutResult cachedLayoutResult = new TextLayoutResult();

        [ThreadStatic]
        private static readonly TextLayoutResult cachedLayoutResultTemp = new TextLayoutResult();
    }
}
