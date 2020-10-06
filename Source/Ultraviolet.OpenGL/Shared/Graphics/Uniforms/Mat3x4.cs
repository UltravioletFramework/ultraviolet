using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 3 columns and 4 rows.
    /// </summary>
    public struct Mat3x4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat3x4"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat3x4(Matrix value)
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
            this.M41 = value.M41;
            this.M42 = value.M42;
            this.M43 = value.M43;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat3x4"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat3x4 value)
        {
            return new Matrix(
                value.M11, value.M12, value.M13, 0,
                value.M21, value.M22, value.M23, 0,
                value.M31, value.M32, value.M33, 0,
                value.M41, value.M42, value.M43, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat3x4"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat4x3"/>.</param>
        public static void Transpose(ref Mat3x4 matrix, out Mat4x3 result)
        {
            Mat4x3 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;
            temp.M13 = matrix.M31;
            temp.M14 = matrix.M41;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;
            temp.M23 = matrix.M32;
            temp.M24 = matrix.M42;

            temp.M31 = matrix.M13;
            temp.M32 = matrix.M23;
            temp.M33 = matrix.M33;
            temp.M34 = matrix.M43;

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

        /// <summary>
        /// The value at row 4, column 1 of the matrix.
        /// </summary>
        public Single M41;

        /// <summary>
        /// The value at row 4, column 2 of the matrix.
        /// </summary>
        public Single M42;

        /// <summary>
        /// The value at row 4, column 3 of the matrix.
        /// </summary>
        public Single M43;
    }
}
