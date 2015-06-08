using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which rotates an object around the specified origin.
    /// </summary>
    [UvmlKnownType]
    public sealed class RotateTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix Value
        {
            get { return value; }
        }

        /// <inheritdoc/>
        public override Matrix? Inverse
        {
            get { return inverse; }
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
            new PropertyMetadata<Single>(CommonBoxedValues.Single.Zero, PropertyMetadataOptions.None, HandleAngleChanged));

        /// <summary>
        /// Identifies the <see cref="CenterX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(Double), typeof(RotateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleCenterChanged));

        /// <summary>
        /// Identifies the <see cref="CenterY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(Double), typeof(RotateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleCenterChanged));

        /// <summary>
        /// Called when the value of the <see cref="Angle"/> dependency property changes.
        /// </summary>
        private static void HandleAngleChanged(DependencyObject dobj, Single oldValue, Single newValue)
        {
            ((RotateTransform)dobj).UpdateValue();
        }

        /// <summary>
        /// Called when the value of the <see cref="CenterX"/> or <see cref="CenterY"/> dependency properties change.
        /// </summary>
        private static void HandleCenterChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            ((RotateTransform)dobj).UpdateValue();
        }

        /// <summary>
        /// Updates the transform's cached value.
        /// </summary>
        private void UpdateValue()
        {
            var centerX = CenterX;
            var centerY = CenterY;
            var radians = Radians.FromDegrees((Single)Angle);

            var hasCenter = (centerX != 0 || centerY != 0);
            if (hasCenter)
            {
                var mtxCenter = Matrix.CreateTranslation(-(Single)centerX, -(Single)centerY, 0f);
                var mtxRotate = Matrix.CreateRotationZ(radians);

                Matrix mtxResult;
                Matrix.Multiply(ref mtxCenter, ref mtxRotate, out mtxResult);

                this.value = mtxResult;
            }
            else
            {
                this.value = Matrix.CreateRotationZ(radians);
            }

            Matrix inverse;
            if (Matrix.TryInvert(value, out inverse))
            {
                this.inverse = inverse;
            }
            else
            {
                this.inverse = null;
            }
        }

        // Property values.
        private Matrix value = Matrix.Identity;
        private Matrix? inverse;
    }
}
