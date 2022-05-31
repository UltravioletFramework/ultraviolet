using System;

namespace Ultraviolet.Presentation.Controls
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
        /// <value>A <see cref="Color"/> value which is used to fill the shape.
        /// The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="FillColorProperty"/></dpropField>
        ///     <dpropStylingName>fill-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color FillColor
        {
            get { return GetValue<Color>(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="FillColor"/> dependency property.</value>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(Shape),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));        
    }
}
