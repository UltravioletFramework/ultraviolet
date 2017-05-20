using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a point in two-dimensional space with double-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y}\}")]
    public struct Point2D : IEquatable<Point2D>, IInterpolatable<Point2D>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="x">The point's x-coordinate.</param>
        /// <param name="y">The point's y-coordinate.</param>
        [Preserve]
        [JsonConstructor]
        public Point2D(Double x, Double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Compares two points for equality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2D"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified points are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Point2D p1, Point2D p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Compares two points for inequality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2D"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2D"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified points are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Point2D p1, Point2D p2)
        {
            return !p1.Equals(p2);
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2D"/> to add.</param>
        /// <param name="p2">The second <see cref="Point2D"/> to add.</param>
        /// <returns>A <see cref="Point2D"/> that represents the sum of the specified points.</returns>
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
        public static explicit operator Point2F(Point2D point)
        {
            Point2F result;

            result.X = (Single)point.X;
            result.Y = (Single)point.Y;

            return result;
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Point2D point)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out point);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <returns>A instance of the <see cref="Point2D"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Point2D Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2D"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Point2D point)
        {
            point = default(Point2D);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 2)
                return false;

            Double x, y;
            if (!Double.TryParse(components[0], style, provider, out x))
                return false;
            if (!Double.TryParse(components[1], style, provider, out y))
                return false;

            point = new Point2D(x, y);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Point2D"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Point2D Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Point2D point;
            if (!TryParse(s, style, provider, out point))
            {
                throw new FormatException();
            }
            return point;
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

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Converts the object to a human-readable string using the specified culture information.
        /// </summary>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A human-readable string that represents the object.</returns>
        public String ToString(IFormatProvider provider)
        {
            return String.Format(provider, "{0} {1}", X, Y);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Point2D))
                return false;
            return Equals((Point2D)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Point2D other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
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
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Double X;

        /// <summary>
        /// The point's y-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Double Y;
    }
}
