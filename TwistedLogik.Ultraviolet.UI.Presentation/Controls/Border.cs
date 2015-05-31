using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a control which renders a border around its content.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.Border.xml")]
    public class Border : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Border(UltravioletContext uv, String name)
            : base(uv, name)
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
        /// Identifies the <see cref="BorderThickness"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'border-thickness'.</remarks>
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border),
            new PropertyMetadata<Thickness>(PresentationBoxedValues.Thickness.One, PropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the <see cref="BorderColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'border-color'.</remarks>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Color), typeof(Border),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.Black));
        
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
    }
}
