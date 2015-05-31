using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which renders a geometric shape.
    /// </summary>
    [UvmlKnownType]
    public abstract class Shape : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Shape(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the color used to fill the shape.
        /// </summary>
        public Color FillColor
        {
            get { return GetValue<Color>(FillColorProperty); }
            set { SetValue<Color>(FillColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        /// <remarks>The styling name of this dependency property is 'fill-color'.</remarks>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(Shape),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));        
    }
}
