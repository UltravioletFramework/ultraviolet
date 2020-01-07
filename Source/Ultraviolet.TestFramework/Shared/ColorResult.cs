using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a color value.
    /// </summary>
    public sealed class ColorResult
    {
        /// <summary>
        /// Initializes a new instance of the ColorResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal ColorResult(Color value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified alpha component.
        /// </summary>
        /// <param name="a">The color's expected alpha component.</param>
        /// <returns>The result object.</returns>
        public void ShouldHaveAlphaComponent(Int32 a)
        {
            Assert.AreEqual((Byte)a, value.A);
        }

        /// <summary>
        /// Asserts that this value should have the specified red component.
        /// </summary>
        /// <param name="r">The color's expected red component.</param>
        /// <returns>The result object.</returns>
        public void ShouldHaveRedComponent(Int32 r)
        {
            Assert.AreEqual((Byte)r, value.R);
        }

        /// <summary>
        /// Asserts that this value should have the specified green component.
        /// </summary>
        /// <param name="g">The color's expected green component.</param>
        /// <returns>The result object.</returns>
        public void ShouldHaveGreenComponent(Int32 g)
        {
            Assert.AreEqual((Byte)g, value.G);
        }

        /// <summary>
        /// Asserts that this value should have the specified blue component.
        /// </summary>
        /// <param name="b">The color's expected blue component.</param>
        /// <returns>The result object.</returns>
        public void ShouldHaveBlueComponent(Int32 b)
        {
            Assert.AreEqual((Byte)b, value.B);
        }

        /// <summary>
        /// Asserts that this value should have the specified ARGB components.
        /// </summary>
        /// <param name="a">The color's expected alpha component.</param>
        /// <param name="r">The color's expected red component.</param>
        /// <param name="g">The color's expected green component.</param>
        /// <param name="b">The color's expected blue component.</param>
        /// <returns>The result object.</returns>
        public void ShouldHaveArgbComponents(Int32 a, Int32 r, Int32 g, Int32 b)
        {
            Assert.AreEqual((Byte)a, value.A);
            Assert.AreEqual((Byte)r, value.R);
            Assert.AreEqual((Byte)g, value.G);
            Assert.AreEqual((Byte)b, value.B);
        }

        /// <summary>
        /// Asserts that this value should have the specified packed value.
        /// </summary>
        /// <param name="packedValue">The color's expected packed value.</param>
        /// <returns>The result object.</returns>
        public void ShouldHavePackedValue(UInt32 packedValue)
        {
            Assert.AreEqual(packedValue, value.PackedValue);
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Color Value
        {
            get { return value; }
        }

        // State values.
        private readonly Color value;
    }
}
