using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents the test framework for Ultraviolet unit tests.
    /// </summary>
    public abstract class UltravioletTestFramework : NucleusTestFramework
    {
        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static MatrixResult TheResultingValue(Matrix value)
        {
            return new MatrixResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static CircleFResult TheResultingValue(CircleF value)
        {
            return new CircleFResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static CircleResult TheResultingValue(Circle value)
        {
            return new CircleResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static ColorResult TheResultingValue(Color value)
        {
            return new ColorResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static RadiansResult TheResultingValue(Radians value)
        {
            return new RadiansResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static RectangleFResult TheResultingValue(RectangleF value)
        {
            return new RectangleFResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static RectangleResult TheResultingValue(Rectangle value)
        {
            return new RectangleResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Size2FResult TheResultingValue(Size2F value)
        {
            return new Size2FResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Size2Result TheResultingValue(Size2 value)
        {
            return new Size2Result(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Size3FResult TheResultingValue(Size3F value)
        {
            return new Size3FResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Size3Result TheResultingValue(Size3 value)
        {
            return new Size3Result(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Vector2Result TheResultingValue(Vector2 value)
        {
            return new Vector2Result(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Vector3Result TheResultingValue(Vector3 value)
        {
            return new Vector3Result(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Vector4Result TheResultingValue(Vector4 value)
        {
            return new Vector4Result(value);
        }
    }
}
