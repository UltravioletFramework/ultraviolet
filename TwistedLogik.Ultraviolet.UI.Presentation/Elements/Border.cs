using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which renders a border around its content.
    /// </summary>
    [UIElement("Border", "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.Border.xml")]
    public class Border : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The unique identifier of this element within its layout.</param>
        public Border(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <summary>
        /// Gets or sets the thickness of the control's border.
        /// </summary>
        public Thickness BorderThickness
        {
            get { return GetValue<Thickness>(BorderThicknessProperty); }
            set { SetValue<Thickness>(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the control's border.
        /// </summary>
        public Color BorderColor
        {
            get { return GetValue<Color>(BorderColorProperty); }
            set { SetValue<Color>(BorderColorProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BorderThickness"/> property changes.
        /// </summary>
        public event UIElementEventHandler BorderThicknessChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor"/> property changes.
        /// </summary>
        public event UIElementEventHandler BorderColorChanged;

        /// <summary>
        /// Identifies the <see cref="BorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border),
            new DependencyPropertyMetadata(HandleBorderThicknessChanged, () => Thickness.One, DependencyPropertyOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="BorderColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Color), typeof(Border),
            new DependencyPropertyMetadata(HandleBorderColorChanged, () => Color.Black, DependencyPropertyOptions.None));

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            var borderColor     = BorderColor;
            var borderThickness = BorderThickness;
            var borderArea      = new RectangleD(0, 0, RelativeBounds.Width, RelativeBounds.Height);

            var leftSize = Math.Min(borderThickness.Left, borderArea.Width);
            var leftArea = new RectangleD(borderArea.Left, borderArea.Top, leftSize, borderArea.Height);

            var topSize = Math.Min(borderThickness.Top, borderArea.Height);
            var topArea = new RectangleD(borderArea.Left, borderArea.Top, borderArea.Width, topSize);

            var rightSize = Math.Min(borderThickness.Right, borderArea.Width);
            var rightPos  = Math.Max(borderArea.Left, borderArea.Right - rightSize);
            var rightArea = new RectangleD(rightPos, borderArea.Top, rightSize, borderArea.Height);

            var bottomSize = Math.Min(borderThickness.Bottom, borderArea.Height);
            var bottomPos  = Math.Max(borderArea.Top, borderArea.Bottom - bottomSize);
            var bottomArea = new RectangleD(borderArea.Left, bottomPos, borderArea.Width, bottomSize);

            DrawBlank(dc, leftArea, borderColor);
            DrawBlank(dc, topArea, borderColor);
            DrawBlank(dc, rightArea, borderColor);
            DrawBlank(dc, bottomArea, borderColor);

            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Raises the <see cref="BorderThicknessChanged"/> event.
        /// </summary>
        protected virtual void OnBorderThicknessChanged()
        {
            var temp = BorderThicknessChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="BorderColorChanged"/> event.
        /// </summary>
        protected virtual void OnBorderColorChanged()
        {
            var temp = BorderColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BorderThickness"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleBorderThicknessChanged(DependencyObject dobj)
        {
            var border = (Border)dobj;
            border.OnBorderThicknessChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BorderColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleBorderColorChanged(DependencyObject dobj)
        {
            var border = (Border)dobj;
            border.OnBorderColorChanged();
        }
    }
}
