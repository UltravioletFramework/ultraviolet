using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

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
        /// Occurs when the value of the <see cref="FillColor"/> property changes.
        /// </summary>
        public event UpfEventHandler FillColorChanged;

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        [Styled("fill-color")]
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(Shape),
            new PropertyMetadata(UltravioletBoxedValues.Color.White, HandleFillColorChanged));
        
        /// <summary>
        /// Raises the <see cref="FillColorChanged"/> event.
        /// </summary>
        protected virtual void OnFillColorChanged()
        {
            var temp = FillColorChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="FillColor"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleFillColorChanged(DependencyObject dobj)
        {
            var shape = (Shape)dobj;
            shape.OnFillColorChanged();
        }
    }
}
