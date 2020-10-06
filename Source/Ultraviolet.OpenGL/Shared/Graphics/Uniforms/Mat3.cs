using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 3 columns and 3 rows.
    /// </summary>
    public struct Mat3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat3"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat3(Matrix value)
        {
            this.M11 = value.M11;
            this.M12 = value.M12;
            this.M13 = value.M13;
            this.M21 = value.M21;
            this.M22 = value.M22;
            this.M23 = value.M23;
            this.M31 = value.M31;
            this.M32 = value.M32;
            this.M33 = value.M33;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat3"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat3 value)
        {
            return new Matrix(
                value.M11, value.M12, value.M13, 0,
                value.M21, value.M22, value.M23, 0,
                value.M31, value.M32, value.M33, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat3"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat3"/>.</param>
        public static void Transpose(ref Mat3 matrix, out Mat3 result)
        {
            Mat3 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;
            temp.M13 = matrix.M31;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;
            temp.M23 = matrix.M32;

            temp.M31 = matrix.M13;
            temp.M32 = matrix.M23;
            temp.M33 = matrix.M33;

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
        /// The value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M31;

        /// <summary>
        /// The value at row 3, column 2 of the matrix.
        /// </summary>
        public Single M32;

        /// <summary>
        /// The value at row 3, column 3 of the matrix.
        /// </summary>
        public Single M33;
    }
}
