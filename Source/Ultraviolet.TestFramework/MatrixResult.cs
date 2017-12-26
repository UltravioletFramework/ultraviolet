using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a 4x4 matrix value.
    /// </summary>
    public sealed class MatrixResult
    {
        /// <summary>
        /// Initializes a new instance of the MatrixResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal MatrixResult(Matrix value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="m11">The value at row 1, column 1 of the matrix.</param>
        /// <param name="m12">The value at row 1, column 2 of the matrix.</param>
        /// <param name="m13">The value at row 1, column 3 of the matrix.</param>
        /// <param name="m14">The value at row 1, column 4 of the matrix.</param>
        /// <param name="m21">The value at row 2, column 1 of the matrix.</param>
        /// <param name="m22">The value at row 2, column 2 of the matrix.</param>
        /// <param name="m23">The value at row 2, column 3 of the matrix.</param>
        /// <param name="m24">The value at row 2, column 4 of the matrix.</param>
        /// <param name="m31">The value at row 3, column 1 of the matrix.</param>
        /// <param name="m32">The value at row 3, column 2 of the matrix.</param>
        /// <param name="m33">The value at row 3, column 3 of the matrix.</param>
        /// <param name="m34">The value at row 3, column 4 of the matrix.</param>
        /// <param name="m41">The value at row 4, column 1 of the matrix.</param>
        /// <param name="m42">The value at row 4, column 2 of the matrix.</param>
        /// <param name="m43">The value at row 4, column 3 of the matrix.</param>
        /// <param name="m44">The value at row 4, column 4 of the matrix.</param>
        /// <returns>The result object.</returns>
        public MatrixResult ShouldBe(
            Single m11, Single m12, Single m13, Single m14,
            Single m21, Single m22, Single m23, Single m24,
            Single m31, Single m32, Single m33, Single m34,
            Single m41, Single m42, Single m43, Single m44)
        {
            Assert.AreEqual(m11, value.M11, delta);
            Assert.AreEqual(m12, value.M12, delta);
            Assert.AreEqual(m13, value.M13, delta);
            Assert.AreEqual(m14, value.M14, delta);

            Assert.AreEqual(m21, value.M21, delta);
            Assert.AreEqual(m22, value.M22, delta);
            Assert.AreEqual(m23, value.M23, delta);
            Assert.AreEqual(m24, value.M24, delta);

            Assert.AreEqual(m31, value.M31, delta);
            Assert.AreEqual(m32, value.M32, delta);
            Assert.AreEqual(m33, value.M33, delta);
            Assert.AreEqual(m34, value.M34, delta);

            Assert.AreEqual(m41, value.M41, delta);
            Assert.AreEqual(m42, value.M42, delta);
            Assert.AreEqual(m43, value.M43, delta);
            Assert.AreEqual(m44, value.M44, delta);

            return this;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected matrix.
        /// </summary>
        /// <param name="expected">The expected matrix value.</param>
        /// <returns>The result object.</returns>
        public MatrixResult ShouldBe(Matrix expected)
        {
            Assert.AreEqual(expected.M11, value.M11, delta);
            Assert.AreEqual(expected.M12, value.M12, delta);
            Assert.AreEqual(expected.M13, value.M13, delta);
            Assert.AreEqual(expected.M14, value.M14, delta);

            Assert.AreEqual(expected.M21, value.M21, delta);
            Assert.AreEqual(expected.M22, value.M22, delta);
            Assert.AreEqual(expected.M23, value.M23, delta);
            Assert.AreEqual(expected.M24, value.M24, delta);

            Assert.AreEqual(expected.M31, value.M31, delta);
            Assert.AreEqual(expected.M32, value.M32, delta);
            Assert.AreEqual(expected.M33, value.M33, delta);
            Assert.AreEqual(expected.M34, value.M34, delta);

            Assert.AreEqual(expected.M41, value.M41, delta);
            Assert.AreEqual(expected.M42, value.M42, delta);
            Assert.AreEqual(expected.M43, value.M43, delta);
            Assert.AreEqual(expected.M44, value.M44, delta);

            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public MatrixResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Matrix Value
        {
            get { return value; }
        }

        // State values.
        private readonly Matrix value;
        private Single delta = 0.1f;
    }
}
