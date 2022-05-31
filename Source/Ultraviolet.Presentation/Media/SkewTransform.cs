using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which skews an object in two dimensions.
    /// </summary>
    [UvmlKnownType]
    public sealed class SkewTransform : Transform
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

        /// <inheritdoc/>
        public override Boolean IsIdentity
        {
            get { return isIdentity; }
        }

        /// <summary>
        /// Gets or sets the angle of skew in degrees along the x-axis.
        /// </summary>
        /// <value>A <see cref="Single"/> value that represents the angle of skew, in degrees, applied
        /// along the transformed object's x-axis. The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="AngleXProperty"/></dpropField>
        ///		<dpropStylingName>angle-x</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Single AngleX
        {
            get { return GetValue<Single>(AngleXProperty); }
            set { SetValue(AngleXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle of skew in degrees along the y-axis.
        /// </summary>
        /// <value>A <see cref="Single"/> value that represents the angle of skew, in degrees, applied
        /// along the transformed object's y-axis. The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="AngleYProperty"/></dpropField>
        ///		<dpropStylingName>angle-y</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Single AngleY
        {
            get { return GetValue<Single>(AngleYProperty); }
            set { SetValue(AngleYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the x-coordinate around which the object is rotated.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the x-coordinate, in device-independent pixels,
        /// around which the object is skewed. The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CenterXProperty"/></dpropField>
        ///		<dpropStylingName>center-x</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double CenterX
        {
            get { return GetValue<Double>(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the y-coordinate around which the object is rotated.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the y-coordinate, in device-independent pixels,
        /// around which the object is skewed. The default value is 0.0.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="CenterYProperty"/></dpropField>
        ///		<dpropStylingName>center-y</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double CenterY
        {
            get { return GetValue<Double>(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AngleX"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="AngleX"/> dependency property.</value>
        public static readonly DependencyProperty AngleXProperty = DependencyProperty.Register("AngleX", typeof(Single), typeof(SkewTransform),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.Zero, PropertyMetadataOptions.None, HandleAngleChanged));

        /// <summary>
        /// Identifies the <see cref="AngleY"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="AngleY"/> dependency property.</value>
        public static readonly DependencyProperty AngleYProperty = DependencyProperty.Register("AngleY", typeof(Single), typeof(SkewTransform),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.Zero, PropertyMetadataOptions.None, HandleAngleChanged));

        /// <summary>
        /// Identifies the <see cref="CenterX"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CenterX"/> dependency property.</value>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(Double), typeof(SkewTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleCenterChanged));

        /// <summary>
        /// Identifies the <see cref="CenterY"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="CenterY"/> dependency property.</value>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(Double), typeof(SkewTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleCenterChanged));

        /// <summary>
        /// Called when the value of the <see cref="AngleX"/> or <see cref="AngleY"/> dependency properties changes.
        /// </summary>
        private static void HandleAngleChanged(DependencyObject dobj, Single oldValue, Single newValue)
        {
            var transform = (SkewTransform)dobj;
            transform.UpdateValue();
            transform.InvalidateDependencyObject();
        }

        /// <summary>
        /// Called when the value of the <see cref="CenterX"/> or <see cref="CenterY"/> dependency properties change.
        /// </summary>
        private static void HandleCenterChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var transform = (SkewTransform)dobj;
            transform.UpdateValue();
            transform.InvalidateDependencyObject();
        }

        /// <summary>
        /// Updates the transform's cached value.
        /// </summary>
        private void UpdateValue()
        {
            var centerX = (Single)CenterX;
            var centerY = (Single)CenterY;

            var tanX = (Single)Math.Tan(Radians.FromDegrees(AngleX));
            var tanY = (Single)Math.Tan(Radians.FromDegrees(AngleY));

            var mtxSkew = new Matrix(
                   1, tanY, 0, 0,
                tanX,    1, 0, 0,
                   0,    0, 1, 0,
                   0,    0, 0, 1);

            var hasCenter = (centerX != 0 || centerY != 0);
            if (hasCenter)
            {
                var mtxTransformCenter = Matrix.CreateTranslation(-centerX, -centerY, 0f);
                var mtxTransformCenterInverse = Matrix.CreateTranslation(centerX, centerY, 0f);

                Matrix mtxResult;
                Matrix.Multiply(ref mtxTransformCenter, ref mtxSkew, out mtxResult);
                Matrix.Multiply(ref mtxResult, ref mtxTransformCenterInverse, out mtxResult);

                this.value = mtxResult;
            }
            else
            {
                this.value = mtxSkew;
            }

            Matrix invertedValue;
            this.inverse = Matrix.TryInvert(value, out invertedValue) ? invertedValue : (Matrix?)null; 
            this.isIdentity = Matrix.Identity.Equals(value);
        }

        // Property values.
        private Matrix value = Matrix.Identity;
        private Matrix? inverse;
        private Boolean isIdentity;
    }
}
