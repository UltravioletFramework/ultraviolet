using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a transformation which scales an object.
    /// </summary>
    [UvmlKnownType]
    public sealed class ScaleTransform : Transform
    {
        /// <inheritdoc/>
        public override Matrix GetValue()
        {
            var centerX = CenterX;
            var centerY = CenterY;

            var hasCenter = (centerX != 0 || centerY != 0);
            if (hasCenter)
            {
                var mtxCenter = Matrix.CreateTranslation(-(Single)centerX, -(Single)centerY, 0f);
                var mtxScale  = Matrix.CreateScale(ScaleX, ScaleY, 1f);

                Matrix mtxResult;
                Matrix.Multiply(ref mtxCenter, ref mtxScale, out mtxResult);

                return mtxResult;
            }
            return Matrix.CreateScale(ScaleX, ScaleY, 1f);
        }

        /// <inheritdoc/>
        public override Matrix GetValueForDisplay(IUltravioletDisplay display)
        {
            var centerX = CenterX;
            var centerY = CenterY;

            var hasCenter = (centerX != 0 || centerY != 0);
            if (hasCenter)
            {
                var displayCenterX = (Single)display.DipsToPixels(centerX);
                var displayCenterY = (Single)display.DipsToPixels(centerY);

                var mtxCenter = Matrix.CreateTranslation(-displayCenterX, -displayCenterY, 0f);
                var mtxScale  = Matrix.CreateScale(ScaleX, ScaleY, 1f);

                Matrix mtxResult;
                Matrix.Multiply(ref mtxCenter, ref mtxScale, out mtxResult);

                return mtxResult;
            }
            return Matrix.CreateScale(ScaleX, ScaleY, 1f);
        }

        /// <summary>
        /// Gets or sets the transform's scaling factor along the x-axis.
        /// </summary>
        public Single ScaleX
        {
            get { return GetValue<Single>(ScaleXProperty); }
            set { SetValue<Single>(ScaleXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transform's scaling factor along the y-axis.
        /// </summary>
        public Single ScaleY
        {
            get { return GetValue<Single>(ScaleYProperty); }
            set { SetValue<Single>(ScaleYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the x-coordinate around which the object is scaled.
        /// </summary>
        public Double CenterX
        {
            get { return GetValue<Double>(CenterXProperty); }
            set { SetValue<Double>(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the y-coordinate around which the object is scaled.
        /// </summary>
        public Double CenterY
        {
            get { return GetValue<Double>(CenterYProperty); }
            set { SetValue<Double>(CenterYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ScaleX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(Single), typeof(ScaleTransform),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.One, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="ScaleY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(Single), typeof(ScaleTransform),
            new PropertyMetadata<Single>(CommonBoxedValues.Single.One, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CenterX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(Double), typeof(ScaleTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the <see cref="CenterY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(Double), typeof(ScaleTransform),
            new PropertyMetadata<Double>(CommonBoxedValues.Double.Zero, PropertyMetadataOptions.None));
    }
}
