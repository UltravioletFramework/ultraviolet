using System;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Represents a transformation based on an arbitrary matrix.
    /// </summary>
    [UvmlKnownType]
    public sealed class MatrixTransform : Transform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTransform"/> class.
        /// </summary>
        public MatrixTransform()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTransform"/> class.
        /// </summary>
        /// <param name="matrix">The transformation matrix that this transform represents.</param>
        public MatrixTransform(Matrix matrix)
        {
            this.Matrix = matrix;
        }

        /// <inheritdoc/>
        public override Matrix Value
        {
            get { return GetValue<Matrix>(MatrixProperty); }
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
        /// Gets or sets the transformation matrix that this transform represents.
        /// </summary>
        /// <value>A <see cref="Matrix"/> which represents the transformation applied by this instance.
        /// The default value is <see cref="Ultraviolet.Matrix.Identity"/>.</value>
        /// <remarks>
        /// <dprop>
        ///		<dpropField><see cref="MatrixProperty"/></dpropField>
        ///		<dpropStylingName>matrix</dpropStylingName>
        ///		<dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Matrix Matrix
        {
            get { return GetValue<Matrix>(MatrixProperty); }
            set { SetValue<Matrix>(MatrixProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Matrix"/> dependency property.
        /// </summary>
        /// <value>The identifier for the <see cref="Matrix"/> dependency property.</value>
        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register("Matrix", typeof(Matrix), typeof(MatrixTransform),
            new PropertyMetadata<Matrix>(Matrix.Identity, PropertyMetadataOptions.None, HandleMatrixChanged));

        /// <summary>
        /// Called when the value of the <see cref="Matrix"/> dependency property changes.
        /// </summary>
        private static void HandleMatrixChanged(DependencyObject dobj, Matrix oldValue, Matrix newValue)
        {
            var transform = (MatrixTransform)dobj;

            Matrix inverse;
            if (Matrix.TryInvert(newValue, out inverse))
            {
                transform.inverse = inverse;
            }
            else
            {
                transform.inverse = null;
            }

            transform.isIdentity = Matrix.Identity.Equals(newValue);
            transform.InvalidateDependencyObject();
        }

        // The matrix's cached inverse.
        private Matrix? inverse;
        private Boolean isIdentity = true;
    }
}
