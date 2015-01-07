using System;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UIElement("RadioButton")]
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public RadioButton(UltravioletContext uv, String id)
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

            var checkBoxSize    = (Int32)display.DipsToPixels(RadioButtonSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(RadioButtonPadding);

            width  = (width ?? 0) + checkBoxSize + checkBoxPadding;
            height = Math.Max((height ?? 0), checkBoxSize);
        }

        /// <inheritdoc/>
        public override void ReloadContent()
        {
            ReloadRadioButtonImage();
            ReloadRadioButtonFillImage();

            base.ReloadContent();
        }

        /// <summary>
        /// Gets or sets the color with which the element's radio button image is drawn.
        /// </summary>
        public Color RadioButtonColor
        {
            get { return GetValue<Color>(RadioButtonImageProperty); }
            set { SetValue<Color>(RadioButtonImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's radio button image.
        /// </summary>
        public SourcedRef<Image> RadioButtonImage
        {
            get { return GetValue<SourcedRef<Image>>(RadioButtonImageProperty); }
            set { SetValue<SourcedRef<Image>>(RadioButtonImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the element's radio button fill image is drawn.
        /// </summary>
        public Color RadioButtonFillColor
        {
            get { return GetValue<Color>(RadioButtonFillColorProperty); }
            set { SetValue<Color>(RadioButtonFillColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element's radio button fill image.
        /// </summary>
        public SourcedRef<Image> RadioButtonFillImage
        {
            get { return GetValue<SourcedRef<Image>>(RadioButtonFillImageProperty); }
            set { SetValue<SourcedRef<Image>>(RadioButtonFillImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the element's radio button image in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double RadioButtonSize
        {
            get { return GetValue<Double>(RadioButtonSizeProperty); }
            set { SetValue<Double>(RadioButtonSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of padding between the element's radio button image and its text in device 
        /// independent pixels (1/96 of an inch).
        /// </summary>
        public Double RadioButtonPadding
        {
            get { return GetValue<Double>(RadioButtonPaddingProperty); }
            set { SetValue<Double>(RadioButtonPaddingProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonFillColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonFillColorChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonFillImage"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonFillImageChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonSize"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonSizeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonPadding"/> property changes.
        /// </summary>
        public event UIElementEventHandler RadioButtonPaddingChanged;

        /// <summary>
        /// Identifies the <see cref="RadioButtonColor"/> dependency property.
        /// </summary>
        [Styled("radiobutton-color")]
        public static readonly DependencyProperty RadioButtonColorProperty = DependencyProperty.Register("RadioButtonColor", typeof(Color), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonColorChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="RadioButtonImage"/> dependency property.
        /// </summary>
        [Styled("radiobutton-image")]
        public static readonly DependencyProperty RadioButtonImageProperty = DependencyProperty.Register("RadioButtonImage", typeof(SourcedRef<Image>), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="RadioButtonFillColor"/> dependency property.
        /// </summary>
        [Styled("radiobutton-fill-color")]
        public static readonly DependencyProperty RadioButtonFillColorProperty = DependencyProperty.Register("RadioButtonColor", typeof(Color), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonFillColorChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="RadioButtonImage"/> dependency property.
        /// </summary>
        [Styled("radiobutton-fill-image")]
        public static readonly DependencyProperty RadioButtonFillImageProperty = DependencyProperty.Register("RadioButtonFillImage", typeof(SourcedRef<Image>), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonFillImageChanged, null, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="RadioButtonSize"/> dependency property.
        /// </summary>
        [Styled("radiobutton-size")]
        public static readonly DependencyProperty RadioButtonSizeProperty = DependencyProperty.Register("RadioButtonSize", typeof(Double), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonSizeChanged, () => 32.0, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="RadioButtonPadding"/> dependency property.
        /// </summary>
        [Styled("radiobutton-padding")]
        public static readonly DependencyProperty RadioButtonPaddingProperty = DependencyProperty.Register("RadioButtonPadding", typeof(Double), typeof(RadioButton),
            new DependencyPropertyMetadata(HandleRadioButtonPaddingChanged, () => 8.0, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawRadioButtonImage(spriteBatch, opacity);
            DrawRadioButtonFillImage(spriteBatch, opacity);

            base.OnDrawing(time, spriteBatch, opacity);
        }

        /// <inheritdoc/>
        protected override void OnParentRelativeLayoutChanged()
        {
            UpdateTextArea();
            base.OnParentRelativeLayoutChanged();
        }

        /// <inheritdoc/>
        protected override void OnCheckedChanged()
        {
            if (Parent != null && Checked)
            {
                var count = Parent.ContentElementCount;
                for (int i = 0; i < count; i++)
                {
                    var sibling = Parent.GetContentElement(i);
                    if (sibling == this)
                        continue;

                    if (sibling is RadioButton)
                    {
                        ((RadioButton)sibling).Checked = false;
                    }
                }
            }
            base.OnCheckedChanged();
        }

        /// <inheritdoc/>
        protected override void ToggleChecked()
        {
            if (!Checked)
            {
                Checked = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonColorChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonColorChanged()
        {
            var temp = RadioButtonColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonImageChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonImageChanged()
        {
            var temp = RadioButtonImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonFillColorChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonFillColorChanged()
        {
            var temp = RadioButtonFillColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonFillImageChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonFillImageChanged()
        {
            var temp = RadioButtonFillImageChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonSizeChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonSizeChanged()
        {
            UpdateCachedTextLayout();

            var temp = RadioButtonSizeChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="RadioButtonPaddingChanged"/> event.
        /// </summary>
        protected virtual void OnRadioButtonPaddingChanged()
        {
            UpdateCachedTextLayout();

            var temp = RadioButtonPaddingChanged;
            if (temp != null)
            {
                temp(this);
            }
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
        /// Draws the element's radio button image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawRadioButtonImage(SpriteBatch spriteBatch, Single opacity)
        {
            var size = (Int32)ConvertMeasureToPixels(RadioButtonSize, 0);
            var area = new Rectangle(0, 0, size, size);
            DrawElementImage(spriteBatch, RadioButtonImage, area, RadioButtonColor * Opacity * opacity);
        }

        /// <summary>
        /// Draws the element's radio button fill image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawRadioButtonFillImage(SpriteBatch spriteBatch, Single opacity)
        {
            var size = (Int32)ConvertMeasureToPixels(RadioButtonSize, 0);
            var area = new Rectangle(0, 0, size, size);
            DrawElementImage(spriteBatch, RadioButtonFillImage, area, RadioButtonFillColor * Opacity * opacity, false);
        }

        /// <summary>
        /// Reloads the element's radio button image.
        /// </summary>
        protected void ReloadRadioButtonImage()
        {
            LoadContent(RadioButtonImage);
        }

        /// <summary>
        /// Reloads the element's radio button fill image.
        /// </summary>
        protected void ReloadRadioButtonFillImage()
        {
            LoadContent(RadioButtonFillImage);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonColorChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonImageChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.ReloadRadioButtonImage();
            element.OnRadioButtonImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonFillColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonFillColorChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonFillColorChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonFillImage"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonFillImageChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.ReloadRadioButtonFillImage();
            element.OnRadioButtonFillImageChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonSize"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonSizeChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonSizeChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="RadioButtonPadding"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleRadioButtonPaddingChanged(DependencyObject dobj)
        {
            var element = (RadioButton)dobj;
            element.OnRadioButtonPaddingChanged();
            element.UpdateTextArea();
        }

        /// <summary>
        /// Updates the size of the area in which the element draws its text.
        /// </summary>
        private void UpdateTextArea()
        {
            var display = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;

            var checkBoxWidth   = (Int32)display.DipsToPixels(RadioButtonSize);
            var checkBoxPadding = (Int32)display.DipsToPixels(RadioButtonPadding);

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
