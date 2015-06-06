using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which translates an object.
    /// </summary>
    public sealed class TranslateTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix GetValue()
        {
            return Matrix.CreateTranslation((Single)X, (Single)Y, 0f);
        }

        /// <inheritdoc/>
        public override Matrix GetValueForDisplay(IUltravioletDisplay display)
        {
            var displayX = (Single)display.DipsToPixels(X);
            var displayY = (Single)display.DipsToPixels(Y);

            return Matrix.CreateTranslation(displayX, displayY, 0f);
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
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(Double), typeof(TranslateTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));
    }
}
