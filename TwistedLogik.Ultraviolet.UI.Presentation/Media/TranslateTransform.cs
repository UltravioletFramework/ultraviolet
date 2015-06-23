using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
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
        public Double X
        {
            get { return GetValue<Double>(XProperty); }
            set { SetValue<Double>(XProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount that the object is translated along the y-axis.
        /// </summary>
        public Double Y
        {
            get { return GetValue<Double>(YProperty); }
            set { SetValue<Double>(YProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(Double), typeof(TranslateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleTranslationChanged));

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(Double), typeof(TranslateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None, HandleTranslationChanged));

        /// <summary>
        /// Called when the value of the <see cref="X"/> or <see cref="Y"/> dependency properties change.
        /// </summary>
        private static void HandleTranslationChanged(DependencyObject dobj, Double oldValue, Double newValue)
        {
            ((TranslateTransform)dobj).UpdateValue();
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
