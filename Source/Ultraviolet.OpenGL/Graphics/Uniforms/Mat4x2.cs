using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 4 columns and 2 rows.
    /// </summary>
    public struct Mat4x2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat4x2"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat4x2(Matrix value)
        {
            this.M11 = value.M11;
            this.M12 = value.M12;
            this.M13 = value.M13;
            this.M14 = value.M14;
            this.M21 = value.M21;
            this.M22 = value.M22;
            this.M23 = value.M23;
            this.M24 = value.M24;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat4x2"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat4x2 value)
        {
            return new Matrix(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat4x2"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat2x4"/>.</param>
        public static void Transpose(ref Mat4x2 matrix, out Mat2x4 result)
        {
            Mat2x4 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;

            temp.M31 = matrix.M13;
            temp.M32 = matrix.M23;

            temp.M41 = matrix.M14;
            temp.M42 = matrix.M24;

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
        /// The value at row 1, column 3 of the matrix.
        /// </summary>
        public Single M13;

        /// <summary>
        /// The value at row 1, column 4 of the matrix.
        /// </summary>
        public Single M14;

        /// <summary>
        /// The value at row 2, column 1 of the matrix.
        /// </summary>
        public Single M21;

        /// <summary>
        /// The value at row 2, column 2 of the matrix.
        /// </summary>
        public Single M22;

        /// <summary>
        /// The value at row 2, column 3 of the matrix.
        /// </summary>
        public Single M23;

        /// <summary>
        /// The value at row 2, column 4 of the matrix.
        /// </summary>
        public Single M24;
    }
}
