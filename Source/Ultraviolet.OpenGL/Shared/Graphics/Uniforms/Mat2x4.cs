using System;

namespace Ultraviolet.OpenGL.Graphics.Uniforms
{
    /// <summary>
    /// Represents a matrix with 2 columns and 4 rows.
    /// </summary>
    public struct Mat2x4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mat2x4"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/> from which to initialize this structure.</param>
        public Mat2x4(Matrix value)
        {
            this.M11 = value.M11;
            this.M12 = value.M12;
            this.M21 = value.M21;
            this.M22 = value.M22;
            this.M31 = value.M31;
            this.M32 = value.M32;
            this.M41 = value.M41;
            this.M42 = value.M42;
        }

        /// <summary>
        /// Explcitly converts a <see cref="Mat2x4"/> to a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static explicit operator Matrix(Mat2x4 value)
        {
            return new Matrix(
                value.M11, value.M12, 0, 0,
                value.M21, value.M22, 0, 0,
                value.M31, value.M32, 1, 0,
                value.M41, value.M42, 0, 1);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Mat2x4"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Mat4x2"/>.</param>
        public static void Transpose(ref Mat2x4 matrix, out Mat4x2 result)
        {
            Mat4x2 temp;

            temp.M11 = matrix.M11;
            temp.M12 = matrix.M21;
            temp.M13 = matrix.M31;
            temp.M14 = matrix.M41;

            temp.M21 = matrix.M12;
            temp.M22 = matrix.M22;
            temp.M23 = matrix.M32;
            temp.M24 = matrix.M42;

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

        /// <summary>
        /// The value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M31;

        /// <summary>
        /// The value at row 3, column 2 of the matrix.
        /// </summary>
        public Single M32;

        /// <summary>
        /// The value at row 4, column 1 of the matrix.
        /// </summary>
        public Single M41;

        /// <summary>
        /// The value at row 4, column 2 of the matrix.
        /// </summary>
        public Single M42;
    }
}
