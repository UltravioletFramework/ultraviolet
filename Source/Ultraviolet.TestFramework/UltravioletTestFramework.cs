using System;
using System.IO;
using System.Linq;
using System.Text;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents the test framework for Ultraviolet unit tests.
    /// </summary>
    public abstract class UltravioletTestFramework : CoreTestFramework
    {
        /// <summary>
        /// Gets the current machine's name, with any path-invalid characters removed.
        /// </summary>
        public static String GetSanitizedMachineName()
        {
            var invalid = Path.GetInvalidPathChars();
            var name = new StringBuilder(Environment.MachineName);
            for (int i = 0; i < name.Length; i++)
            {
                if (invalid.Contains(name[i]))
                {
                    name[i] = '_';
                }
            }
            return name.ToString();
        }

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
        protected static CircleDResult TheResultingValue(CircleD value)
        {
            return new CircleDResult(value);
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
        protected static RectangleDResult TheResultingValue(RectangleD value)
        {
            return new RectangleDResult(value);
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
        protected static Size2DResult TheResultingValue(Size2D value)
        {
            return new Size2DResult(value);
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
        protected static Size3DResult TheResultingValue(Size3D value)
        {
            return new Size3DResult(value);
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

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Point2Result TheResultingValue(Point2 value)
        {
            return new Point2Result(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Point2FResult TheResultingValue(Point2F value)
        {
            return new Point2FResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static Point2DResult TheResultingValue(Point2D value)
        {
            return new Point2DResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static PlaneResult TheResultingValue(Plane value)
        {
            return new PlaneResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static QuaternionResult TheResultingValue(Quaternion value)
        {
            return new QuaternionResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static RayResult TheResultingValue(Ray value)
        {
            return new RayResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static BoundingSphereResult TheResultingValue(BoundingSphere value)
        {
            return new BoundingSphereResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static BoundingBoxResult TheResultingValue(BoundingBox value)
        {
            return new BoundingBoxResult(value);
        }
    }
}
