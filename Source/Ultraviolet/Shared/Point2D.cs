using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a point in two-dimensional space with double-precision floating point components.
    /// </summary>
    [Serializable]
    public partial struct Point2D : IEquatable<Point2D>, IInterpolatable<Point2D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="x">The point's x-coordinate.</param>
        /// <param name="y">The point's y-coordinate.</param>
        [JsonConstructor]
        public Point2D(Double x, Double y)
        {
            this.X = x;
            this.Y = y;
        }
        
        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2D"/> to add.</param>
        /// <param name="p2">The second <see cref="Point2D"/> to add.</param>
        /// <returns>A <see cref="Point2D"/> that represents the sum of the specified points.</returns>
        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            Point2D result;

            result.X = p1.X + p2.X;
            result.Y = p1.Y + p2.Y;

            return result;
        }

        /// <summary>
        /// Subtracts one point from another point.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2D"/> to subtract.</param>
        /// <param name="p2">The second <see cref="Point2D"/> to subtract.</param>
        /// <returns>A <see cref="Point2D"/> that represents the difference of the specified points.</returns>
        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            Point2D result;

            result.X = p1.X - p2.X;
            result.Y = p1.Y - p2.Y;

            return result;
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2D"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator +(Point2D point, Size2 offset)
        {
            Point2D result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator -(Point2D point, Size2 offset)
        {
            Point2D result;

            result.X = point.X - offset.Width;
            result.Y = point.Y - offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2F"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator +(Point2D point, Size2F offset)
        {
            Point2D result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2F"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator -(Point2D point, Size2F offset)
        {
            Point2D result;

            result.X = point.X - offset.Width;
            result.Y = point.Y - offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator +(Point2D point, Size2D offset)
        {
            Point2D result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2D"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2D"/> that represents the original point plus the specified offset.</returns>
        public static Point2D operator -(Point2D point, Size2D offset)
        {
            Point2D result;

            result.X = point.X - offset.Width;
            result.Y = point.Y - offset.Height;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator *(Point2D point, Int32 multiplier)
        {
            Point2D result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator *(Point2D point, Single multiplier)
        {
            Point2D result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2D"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator *(Point2D point, Double multiplier)
        {
            Point2D result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator /(Point2D point, Int32 divisor)
        {
            Point2D result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator /(Point2D point, Single divisor)
        {
            Point2D result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2D"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        public static Point2D operator /(Point2D point, Double divisor)
        {
            Point2D result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }
        
        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector2(Point2D point)
        {
            Vector2 result;

            result.X = (Single)point.X;
            result.Y = (Single)point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Size2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2(Point2D point)
        {
            Size2 result;

            result.Width = (Int32)point.X;
            result.Height = (Int32)point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2F(Point2D point)
        {
            Size2F result;

            result.Width = (Single)point.X;
            result.Height = (Single)point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Size2D(Point2D point)
        {
            Size2D result;

            result.Width = point.X;
            result.Height = point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2(Point2D point)
        {
            Point2 result;

            result.X = (Int32)point.X;
            result.Y = (Int32)point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2F(Point2D point)
        {
            Point2F result;

            result.X = (Single)point.X;
            result.Y = (Single)point.Y;

            return result;
        }
        
        /// <summary>
        /// Transforms a point by a quaternion.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the point.</param>
        /// <returns>The transformed <see cref="Point2D"/>.</returns>
        public static Point2D Transform(Point2D point, Quaternion quaternion)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Point2D result;

            result.X = point.X * (1.0f - yy2 - zz2) + point.Y * (xy2 - wz2);
            result.Y = point.X * (xy2 + wz2) + point.Y * (1.0f - xx2 - zz2);

            return result;
        }

        /// <summary>
        /// Transforms a point by a quaternion.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the point.</param>
        /// <param name="result">The transformed <see cref="Point2D"/>.</param>
        public static void Transform(ref Point2D point, ref Quaternion quaternion, out Point2D result)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Point2D temp;

            temp.X = point.X * (1.0f - yy2 - zz2) + point.Y * (xy2 - wz2);
            temp.Y = point.X * (xy2 + wz2) + point.Y * (1.0f - xx2 - zz2);

            result = temp;
        }

        /// <summary>
        /// Transforms a point by a matrix.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the point.</param>
        /// <returns>The transformed <see cref="Point2D"/>.</returns>
        public static Point2D Transform(Point2D point, Matrix matrix)
        {
            Point2D result;

            result.X = (matrix.M11 * point.X + matrix.M21 * point.Y) + matrix.M41;
            result.Y = (matrix.M12 * point.X + matrix.M22 * point.Y) + matrix.M42;

            return result;
        }

        /// <summary>
        /// Transforms a point by a matrix.
        /// </summary>
        /// <param name="point">The <see cref="Point2D"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the point.</param>
        /// <param name="result">The transformed <see cref="Point2D"/>.</param>
        public static void Transform(ref Point2D point, ref Matrix matrix, out Point2D result)
        {
            Point2D temp;

            temp.X = (matrix.M11 * point.X + matrix.M21 * point.Y) + matrix.M41;
            temp.Y = (matrix.M12 * point.X + matrix.M22 * point.Y) + matrix.M42;

            result = temp;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y}}}";

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Point2D Interpolate(Point2D target, Single t)
        {
            Point2D result;

            result.X  = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);

            return result;
        }

        /// <summary>
        /// Gets the point at (0, 0).
        /// </summary>
        public static Point2D Zero
        {
            get { return new Point2D(0, 0); }
        }

        /// <summary>
        /// The point's x-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Double X;

        /// <summary>
        /// The point's y-coordinate.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Double Y;
    }
}
