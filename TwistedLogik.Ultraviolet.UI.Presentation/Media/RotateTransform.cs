using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which rotates an object around the specified origin.
    /// </summary>
    public sealed class RotateTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix GetValue()
        {
            return Matrix.CreateTranslation(-(Single)CenterX, -(Single)CenterY, 0f) * Matrix.CreateRotationZ(Angle); 
        }

        /// <inheritdoc/>
        public override Matrix GetValueForDisplay(IUltravioletDisplay display)
        {
            var displayCenterX = (Single)display.DipsToPixels(CenterX);
            var displayCenterY = (Single)display.DipsToPixels(CenterY);

            return Matrix.CreateTranslation(-displayCenterX, -displayCenterY, 0f) * Matrix.CreateRotationZ(Angle); 
        }

        /// <summary>
        /// Gets tor sets the angle of rotation in degrees.
        /// </summary>
        public Single Angle
        {
            get { return GetValue<Single>(AngleProperty); }
            set { SetValue<Single>(AngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the x-coordinate around which the object is rotated.
        /// </summary>
        public Double CenterX
        {
            get { return GetValue<Double>(CenterXProperty); }
            set { SetValue<Double>(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the y-coordinate around which the object is rotated.
        /// </summary>
        public Double CenterY
        {
            get { return GetValue<Double>(CenterYProperty); }
            set { SetValue<Double>(CenterYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Angle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(Single), typeof(RotateTransform),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CenterX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(Double), typeof(RotateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CenterY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(Double), typeof(RotateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));
    }
}
