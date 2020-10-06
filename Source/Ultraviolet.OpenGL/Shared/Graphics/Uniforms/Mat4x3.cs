using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 4 columns and 3 rows.
    /// </summary>
    public struct Mat4x3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat4x3"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat4x3(Matrix value)
        {
            this.M11 = value.M11;
            this.M12 = value.M12;
            this.M13 = value.M13;
            this.M14 = value.M14;
            this.M21 = value.M21;
            this.M22 = value.M22;
            this.M23 = value.M23;
            this.M24 = value.M24;
            this.M31 = value.M31;
            this.M32 = value.M32;
            this.M33 = value.M33;
            this.M34 = value.M34;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat4x3"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat4x3 value)
        {
            return new Matrix(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat4x3"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat3x4"/>.</param>
        public static void Transpose(ref Mat4x3 matrix, out Mat3x4 result)
        {
            Mat3x4 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;
            temp.M13 = matrix.M31;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;
            temp.M23 = matrix.M32;

            temp.M31 = matrix.M13;
            temp.M32 = matrix.M23;
            temp.M33 = matrix.M33;

            temp.M41 = matrix.M14;
            temp.M42 = matrix.M24;
            temp.M43 = matrix.M34;

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

        /// <summary>
        /// The value at row 3, column 4 of the matrix.
        /// </summary>
        public Single M34;
    }
}
