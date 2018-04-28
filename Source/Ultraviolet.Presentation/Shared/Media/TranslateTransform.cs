using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which translates an object.
    /// </summary>
    [UvmlKnownType]
    public sealed class TranslateTransform : Transform
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
        /// Gets or sets the amount that the object is translated along the x-axis.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the distance along the x-axis, in 
        /// device-independent pixels, across which the transformed object is translated.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="XProperty"/></dpropField>
        ///		<dpropStylingName>x</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double X
        {
            get { return GetValue<Double>(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount that the object is translated along the y-axis.
        /// </summary>
        /// <value>A <see cref="Double"/> value that represents the distance along the y-axis, in 
        /// device-independent pixels, across which the transformed object is translated.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="YProperty"/></dpropField>
        ///		<dpropStylingName>y</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Double Y
        {
            get { return GetValue<Double>(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="X"/> dependency property.</value>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(Double), typeof(TranslateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleTranslationChanged));

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Y"/> dependency property.</value>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(Double), typeof(TranslateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleTranslationChanged));

        /// <summary>
        /// Called when the value of the <see cref="X"/> or <see cref="Y"/> dependency properties change.
        /// </summary>
        private static void HandleTranslationChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            var transform = (TranslateTransform)dobj;
            transform.UpdateValue();
            transform.InvalidateDependencyObject();
        }

        /// <summary>
        /// Updates the transform's cached value.
        /// </summary>
        private void UpdateValue()
        {
            this.value = Matrix.CreateTranslation((Single)X, (Single)Y, 0f);

            Matrix inverse;
            if (Matrix.TryInvert(value, out inverse))
            {
                this.inverse = inverse;
            }
            else
            {
                this.inverse = null;
            }

            this.isIdentity = Matrix.Identity.Equals(value);
        }

        // Property values.
        private Matrix value = Matrix.Identity;
        private Matrix? inverse;
        private Boolean isIdentity;
    }
}
