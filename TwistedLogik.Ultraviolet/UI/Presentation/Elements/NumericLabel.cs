using System;
using System.ComponentModel;
using System.Text;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a label on a user interface which is optimized for displaying numbers.
    /// </summary>
    [UIElement("NumericLabel")]
    [DefaultProperty("Value")]
    public class NumericLabel : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericLabel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public NumericLabel(UltravioletContext uv, String id)
            : base(uv, id)
        {
            var dpBackgroundColor = DependencyProperty.FindByName("BackgroundColor", typeof(UIElement));
            SetDefaultValue<Color>(dpBackgroundColor, Color.Transparent);
        }

        /// <summary>
        /// Calculates the element's recommended size based on its content
        /// and the specified constraints.
        /// </summary>
        /// <param name="width">The element's recommended width.</param>
        /// <param name="height">The element's recommended height.</param>
        public override void CalculateContentSize(ref Int32? width, ref Int32? height)
        {
            if (Font == null || buffer.Length == 0)
                return;

            var fontFace = Font.GetFace(Bold, Italic);
            var size = fontFace.MeasureString(buffer);

            if (width == null)
                width = size.Width;

            if (height == null)
                height = size.Height;
        }

        /// <summary>
        /// Gets or sets the label's format string.
        /// </summary>
        public String Format
        {
            get { return GetValue<String>(FormatProperty); }
            set { SetValue<String>(FormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value displayed by the label.
        /// </summary>
        public Double Value
        {
            get { return GetValue<Double>(ValueProperty); }
            set { SetValue<Double>(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label is displayed with a bold font face.
        /// </summary>
        public Boolean Bold
        {
            get { return GetValue<Boolean>(BoldProperty); }
            set { SetValue<Boolean>(BoldProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label is displayed with an italic font face.
        /// </summary>
        public Boolean Italic
        {
            get { return GetValue<Boolean>(ItalicProperty); }
            set { SetValue<Boolean>(ItalicProperty, value); }
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
        /// Occurs when the value of the <see cref="Format"/> property changes.
        /// </summary>
        public event UIElementEventHandler FormatChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property changes.
        /// </summary>
        public event UIElementEventHandler ValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Bold"/> property changes.
        /// </summary>
        public event UIElementEventHandler BoldChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Italic"/> property changes.
        /// </summary>
        public event UIElementEventHandler ItalicChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment"/> property changes.
        /// </summary>
        public event UIElementEventHandler TextAlignmentChanged;

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(String), typeof(NumericLabel),
            new DependencyPropertyMetadata(HandleFormatChanged, () => "{0}", DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(NumericLabel),
            new DependencyPropertyMetadata(HandleValueChanged, () => 0.0, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Bold"/> dependency property.
        /// </summary>
        [Styled("bold")]
        public static readonly DependencyProperty BoldProperty = DependencyProperty.Register("Bold", typeof(Boolean), typeof(NumericLabel),
            new DependencyPropertyMetadata(HandleBoldChanged, () => false, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="Italic"/> dependency property.
        /// </summary>
        [Styled("italic")]
        public static readonly DependencyProperty ItalicProperty = DependencyProperty.Register("Italic", typeof(Boolean), typeof(NumericLabel),
            new DependencyPropertyMetadata(HandleItalicChanged, () => false, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        [Styled("text-align")]
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextFlags), typeof(NumericLabel),
            new DependencyPropertyMetadata(HandleTextAlignmentChanged, () => TextFlags.AlignCenter | TextFlags.AlignMiddle, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, SpriteBatch spriteBatch, Single opacity)
        {
            DrawBackgroundImage(spriteBatch, opacity);
            DrawText(spriteBatch, opacity);

            base.OnDrawing(time, spriteBatch, opacity);
        }

        /// <summary>
        /// Draws the element's text.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw.</param>
        /// <param name="opacity">The cumulative opacity of all of the element's parent elements.</param>
        protected virtual void DrawText(SpriteBatch spriteBatch, Single opacity)
        {
            if (Font == null || buffer.Length == 0)
                return;
            
            var fontFace  = Font.GetFace(Bold, Italic);
            var padding   = ConvertThicknessToPixels(Padding, 0);
            var offsetX   = 0;
            var offsetY   = 0;
            var size      = fontFace.MeasureString(buffer);
            var alignment = TextAlignment;

            var textAreaWidth  = ActualWidth - ((Int32)padding.Left + (Int32)padding.Right);
            var textAreaHeight = ActualHeight - ((Int32)padding.Top + (Int32)padding.Bottom);

            if ((alignment & TextFlags.AlignLeft) == TextFlags.AlignLeft)
            {
                offsetX = 0;
            }
            else if ((alignment & TextFlags.AlignCenter) == TextFlags.AlignCenter)
            { 
                offsetX = (textAreaWidth - size.Width) / 2;
            }
            else if ((alignment & TextFlags.AlignRight) == TextFlags.AlignRight)
            {
                offsetX = textAreaWidth - size.Width;
            }

            if ((alignment & TextFlags.AlignTop) == TextFlags.AlignTop)
            {
                offsetY = 0;
            }
            else if ((alignment & TextFlags.AlignMiddle) == TextFlags.AlignMiddle)
            {
                offsetY = (textAreaHeight - size.Height) / 2;
            }
            else if ((alignment & TextFlags.AlignBottom) == TextFlags.AlignBottom)
            {
                offsetY = textAreaHeight - size.Height;
            }

            var position = new Vector2(
                AbsoluteScreenX + (Int32)padding.Left + offsetX,
                AbsoluteScreenY + (Int32)padding.Top + offsetY);

            spriteBatch.DrawString(fontFace, buffer, position, FontColor * Opacity * opacity);
        }

        /// <summary>
        /// Raises the <see cref="FormatChanged"/> event.
        /// </summary>
        protected virtual void OnFormatChanged()
        {
            var temp = FormatChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var temp = ValueChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BoldChanged"/> event.
        /// </summary>
        protected virtual void OnBoldChanged()
        {
            var temp = BoldChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItalicChanged"/> event.
        /// </summary>
        protected virtual void OnItalicChanged()
        {
            var temp = ItalicChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="TextAlignmentChanged"/> event.
        /// </summary>
        protected virtual void OnTextAlignmentChanged()
        {
            var temp = TextAlignmentChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Format"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleFormatChanged(DependencyObject dobj)
        {
            var label = (NumericLabel)dobj;
            label.OnFormatChanged();
            label.UpdateTextBuffer();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleValueChanged(DependencyObject dobj)
        {
            var label = (NumericLabel)dobj;
            label.OnValueChanged();
            label.UpdateTextBuffer();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Bold"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleBoldChanged(DependencyObject dobj)
        {
            var label = (NumericLabel)dobj;
            label.OnBoldChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Italic"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleItalicChanged(DependencyObject dobj)
        {
            var label = (NumericLabel)dobj;
            label.OnItalicChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextAlignment"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleTextAlignmentChanged(DependencyObject dobj)
        {
            var label = (NumericLabel)dobj;
            label.OnTextAlignmentChanged();
        }

        /// <summary>
        /// Updates the label's text buffer.
        /// </summary>
        private void UpdateTextBuffer()
        {
            buffer.Length = 0;

            formatter.Reset();
            formatter.AddArgument(Value);
            formatter.Format(Format ?? "{0}", buffer);
        }

        // State values.
        private readonly StringFormatter formatter = new StringFormatter();
        private readonly StringBuilder buffer = new StringBuilder();
    }
}
