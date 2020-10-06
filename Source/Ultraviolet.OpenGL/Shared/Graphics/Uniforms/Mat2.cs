using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 2 columns and 2 rows.
    /// </summary>
    public struct Mat2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat2"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat2(Matrix value)
        {
            this.M11 = value.M11;
            this.M12 = value.M12;
            this.M21 = value.M21;
            this.M22 = value.M22;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat2"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat2 value)
        {
            return new Matrix(
                value.M11, value.M12, 0, 0,
                value.M21, value.M22, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat2"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat2"/>.</param>
        public static void Transpose(ref Mat2 matrix, out Mat2 result)
        {
            Mat2 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;

            result = temp;
        }

        /// <summary>
        /// The value at row 1, column 1 of the matrix.
        /// </summary>
        public Single M11;

        /// <summary>
        /// The value at row 1, column 2 of the matrix.
        /// </summary>
        public Single M12;

        /// <summary>
        /// The value at row 2, column 1 of the matrix.
        /// </summary>
        public Single M21;

        /// <summary>
        /// The value at row 2, column 2 of the matrix.
        /// </summary>
        public Single M22;
    }
}
