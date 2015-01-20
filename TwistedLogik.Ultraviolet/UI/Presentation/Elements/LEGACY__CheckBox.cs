using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a check box with binary states.
    /// </summary>
    [UIElement("CheckBox")]
    public class CheckBox : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public CheckBox(UltravioletContext uv, String id)
            : base(uv, id)
        {
            SetDefaultValue<Color>(BackgroundColorProperty, Color.Transparent);
            SetDefaultValue<TextFlags>(TextAlignmentProperty, TextFlags.AlignLeft | TextFlags.AlignMiddle);
        }

        /// <inheritdoc/>
        public override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            base.CalculateContentSize(ref width, ref height);

            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxSize    = (Int32)display.DipsToPixels(CheckBoxSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(CheckBoxPadding);

            width  = (width ?? 0) + checkBoxSize + checkBoxPadding;
            height = Math.Max((height ?? 0), checkBoxSize);
        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadCheckBoxImage();
            ReloadCheckBoxFillImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the color with which the check box image is rendered.
        /// </summary>
        public Color CheckBoxColor
        {
            get { return GetValue<Color>(CheckBoxColorProperty); }
            set { SetValue<Color>(CheckBoxColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's check box image.
        /// </summary>
        public SourcedRef<Image> CheckBoxImage
        {
            get { return GetValue<SourcedRef<Image>>(CheckBoxImageProperty); }
            set { SetValue<SourcedRef<Image>>(CheckBoxImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the check box fill image is rendered.
        /// </summary>
        public Color CheckBoxFillColor
        {
            get { return GetValue<Color>(CheckBoxFillColorProperty); }
            set { SetValue<Color>(CheckBoxFillColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's check box fill image.
        /// </summary>
        public SourcedRef<Image> CheckBoxFillImage
        {
            get { return GetValue<SourcedRef<Image>>(CheckBoxFillImageProperty); }
            set { SetValue<SourcedRef<Image>>(CheckBoxFillImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the element's check box image in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double CheckBoxSize
        {
            get { return GetValue<Double>(CheckBoxSizeProperty); }
            set { SetValue<Double>(CheckBoxSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the element's check box image and its text in device 
        /// independent pixels (1/96 of an inch).
        /// </summary>
        public Double CheckBoxPadding
        {
            get { return GetValue<Double>(CheckBoxPaddingProperty); }
            set { SetValue<Double>(CheckBoxPaddingProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxFillColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxFillColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxFillImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxFillImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxSize"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxPadding"/> property changes.
        /// </summary>
        public event UIElementEventHandler CheckBoxPaddingChanged;

        /// <summary>
        /// Identifies the <see cref="CheckBoxColor"/> dependency property.
        /// </summary>
        [Styled("checkbox-color")]
        public static readonly DependencyProperty CheckBoxColorProperty = DependencyProperty.Register("CheckBoxColor", typeof(Color), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CheckBoxImage"/> dependency property.
        /// </summary>
        [Styled("checkbox-image")]
        public static readonly DependencyProperty CheckBoxImageProperty = DependencyProperty.Register("CheckBoxImage", typeof(SourcedRef<Image>), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CheckBoxFillImage"/> dependency property.
        /// </summary>
        [Styled("checkbox-fill-color")]
        public static readonly DependencyProperty CheckBoxFillColorProperty = DependencyProperty.Register("CheckBoxFillColor", typeof(Color), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxFillColorChanged, () => Color.White, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CheckBoxFillImage"/> dependency property.
        /// </summary>
        [Styled("checkbox-fill-image")]
        public static readonly DependencyProperty CheckBoxFillImageProperty = DependencyProperty.Register("CheckBoxFillImage", typeof(SourcedRef<Image>), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxFillImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="CheckBoxSize"/> dependency property.
        /// </summary>
        [Styled("checkbox-size")]
        public static readonly DependencyProperty CheckBoxSizeProperty = DependencyProperty.Register("CheckBoxSize", typeof(Double), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxSizeChanged, () => 32.0, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="CheckBoxPadding"/> dependency property.
        /// </summary>
        [Styled("checkbox-padding")]
        public static readonly DependencyProperty CheckBoxPaddingProperty = DependencyProperty.Register("CheckBoxPadding", typeof(Double), typeof(CheckBox),
            new DependencyPropertyMetadata(HandleCheckBoxPaddingChanged, () => 8.0, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawCheckBoxImage(spriteBatch, opacity);
            DrawCheckBoxFillImage(spriteBatch, opacity);

            base.OnDrawing(time, spriteBatch, opacity);
        }

        /// <inheritdoc/>
        protected override void OnParentRelativeLayoutChanged()
        {
            UpdateTextArea();
            base.OnParentRelativeLayoutChanged();
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxColorChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxColorChanged()
        {
            var temp = CheckBoxColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxImageChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxImageChanged()
        {
            var temp = CheckBoxImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxFillColorChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxFillColorChanged()
        {
            var temp = CheckBoxFillColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxFillImageChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxFillImageChanged()
        {
            var temp = CheckBoxFillImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxSizeChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxSizeChanged()
        {
            UpdateCachedTextLayout();

            var temp = CheckBoxSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckBoxPaddingChanged"/> event.
        /// </summary>
        protected virtual void OnCheckBoxPaddingChanged()
        {
            UpdateCachedTextLayout();

            var temp = CheckBoxPaddingChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Draws the element's check box image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawCheckBoxImage(SpriteBatch spriteBatch, Single opacity)
        {
            var size = (Int32)ConvertMeasureToPixels(CheckBoxSize, 0);
            var area = new Rectangle(0, 0, size, size);
            DrawElementImage(spriteBatch, CheckBoxImage, area, CheckBoxColor * Opacity * opacity, false);
        }

        /// <summary>
        /// Draws the element's check box fill image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawCheckBoxFillImage(SpriteBatch spriteBatch, Single opacity)
        {
            var size = (Int32)ConvertMeasureToPixels(CheckBoxSize, 0);
            var area = new Rectangle(0, 0, size, size);
            DrawElementImage(spriteBatch, CheckBoxFillImage, area, CheckBoxFillColor * Opacity * opacity, false);
        }

        /// <summary>
        /// Reloads the element's check box image.
        /// </summary>
        protected void ReloadCheckBoxImage()
        {
            LoadContent(CheckBoxImage);
        }

        /// <summary>
        /// Reloads the element's check box fill image.
        /// </summary>
        protected void ReloadCheckBoxFillImage()
        {
            LoadContent(CheckBoxFillImage);
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaX
        {
            get { return textAreaX; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaY
        {
            get { return textAreaY; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaWidth
        {
            get { return textAreaWidth; }
        }

        /// <inheritdoc/>
        protected override Int32 TextAreaHeight
        {
            get { return textAreaHeight; }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxColorChanged(DependencyObject dobj)
        {
            var checkbox = (CheckBox)dobj;
            checkbox.OnCheckBoxColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxImageChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.ReloadCheckBoxImage();
            element.OnCheckBoxImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxFillColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxFillColorChanged(DependencyObject dobj)
        {
            var checkbox = (CheckBox)dobj;
            checkbox.OnCheckBoxFillColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxFillImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxFillImageChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.ReloadCheckBoxFillImage();
            element.OnCheckBoxFillImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxSizeChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.OnCheckBoxSizeChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CheckBoxPadding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleCheckBoxPaddingChanged(DependencyObject dobj)
        {
            var element = (CheckBox)dobj;
            element.OnCheckBoxPaddingChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Updates the size of the area in which the element draws its text.
        /// </summary>
        private void UpdateTextArea()
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxWidth   = (Int32)display.DipsToPixels(CheckBoxSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(CheckBoxPadding);

            textAreaX      = checkBoxWidth + checkBoxPadding;
            textAreaY      = 0;
            textAreaWidth  = ActualWidth - (checkBoxWidth + checkBoxPadding);
            textAreaHeight = ActualHeight;
        }

        // Property values.
        private Int32 textAreaX;
        private Int32 textAreaY;
        private Int32 textAreaWidth;
        private Int32 textAreaHeight;
    }
}
