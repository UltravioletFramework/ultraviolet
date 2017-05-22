using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a point in two-dimensional space with single-precision floating point components.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y}\}")]
    public struct Point2F : IEquatable<Point2F>, IInterpolatable<Point2F>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="x">The point's x-coordinate.</param>
        /// <param name="y">The point's y-coordinate.</param>
        [Preserve]
        [JsonConstructor]
        public Point2F(Single x, Single y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Compares two points for equality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2F"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2F"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified points are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Point2F p1, Point2F p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Compares two points for inequality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2F"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2F"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified points are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Point2F p1, Point2F p2)
        {
            return !p1.Equals(p2);
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2F"/> to add.</param>
        /// <param name="p2">The second <see cref="Point2F"/> to add.</param>
        /// <returns>A <see cref="Point2F"/> that represents the sum of the specified points.</returns>
        [Preserve]
        public static Point2F operator +(Point2F p1, Point2F p2)
        {
            Point2F result;

            result.X = p1.X + p2.X;
            result.Y = p1.Y + p2.Y;

            return result;
        }

        /// <summary>
        /// Subtracts one point from another point.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2F"/> to subtract.</param>
        /// <param name="p2">The second <see cref="Point2F"/> to subtract.</param>
        /// <returns>A <see cref="Point2F"/> that represents the difference of the specified points.</returns>
        [Preserve]
        public static Point2F operator -(Point2F p1, Point2F p2)
        {
            Point2F result;

            result.X = p1.X - p2.X;
            result.Y = p1.Y - p2.Y;

            return result;
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2F"/> that represents the original point plus the specified offset.</returns>
        [Preserve]
        public static Point2F operator -(Point2F point, Size2 offset)
        {
            Point2F result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2D"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2F"/> that represents the original point plus the specified offset.</returns>
        [Preserve]
        public static Point2F operator +(Point2F point, Size2 offset)
        {
            Point2F result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2F"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2F"/> that represents the original point plus the specified offset.</returns>
        [Preserve]
        public static Point2F operator +(Point2F point, Size2F offset)
        {
            Point2F result;

            result.X = point.X + offset.Width;
            result.Y = point.Y + offset.Height;

            return result;
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2F"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2F"/> that represents the original point plus the specified offset.</returns>
        [Preserve]
        public static Point2F operator -(Point2F point, Size2F offset)
        {
            Point2F result;

            result.X = point.X - offset.Width;
            result.Y = point.Y - offset.Height;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2F operator *(Point2F point, Int32 multiplier)
        {
            Point2F result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2F operator *(Point2F point, Single multiplier)
        {
            Point2F result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Point2F"/> by a scalar multiplier.
        /// </summary>
        /// <param name="point">The size to multiply.</param>
        /// <param name="multiplier">The multiplier to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2D operator *(Point2F point, Double multiplier)
        {
            Point2D result;

            result.X = point.X * multiplier;
            result.Y = point.Y * multiplier;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2F operator /(Point2F point, Int32 divisor)
        {
            Point2F result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2F"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2F operator /(Point2F point, Single divisor)
        {
            Point2F result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }

        /// <summary>
        /// Divides a <see cref="Point2F"/> by a scalar divisor.
        /// </summary>
        /// <param name="point">The size to divide.</param>
        /// <param name="divisor">The divisor to apply to the size.</param>
        /// <returns>A <see cref="Point2D"/> which is the result of the muliplication.</returns>
        [Preserve]
        public static Point2D operator /(Point2F point, Double divisor)
        {
            Point2D result;

            result.X = point.X / divisor;
            result.Y = point.Y / divisor;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Vector2(Point2F point)
        {
            Vector2 result;

            result.X = point.X;
            result.Y = point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size2(Point2F point)
        {
            Size2 result;

            result.Width = (Int32)point.X;
            result.Height = (Int32)point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Size2F"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size2F(Point2F point)
        {
            Size2F result;

            result.Width = point.X;
            result.Height = point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Size2D"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Size2D(Point2F point)
        {
            Size2D result;

            result.Width = point.X;
            result.Height = point.Y;

            return result;
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Point2(Point2F point)
        {
            Point2 result;

            result.X = (Int32)point.X;
            result.Y = (Int32)point.Y;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Point2F"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator Point2D(Point2F point)
        {
            Point2D result;

            result.X = point.X;
            result.Y = point.Y;

            return result;
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2F"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Point2F point)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out point);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <returns>A instance of the <see cref="Point2F"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Point2F Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2F"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Point2F point)
        {
            point = default(Point2F);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 2)
                return false;

            Single x, y;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;

            point = new Point2F(x, y);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Point2F"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Point2F Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Point2F point;
            if (!TryParse(s, style, provider, out point))
            {
                throw new FormatException();
            }
            return point;
        }
        
        /// <summary>
        /// Transforms a point by a quaternion.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the point.</param>
        /// <returns>The transformed <see cref="Point2F"/>.</returns>
        public static Point2F Transform(Point2F point, Quaternion quaternion)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Point2F result;

            result.X = point.X * (1.0f - yy2 - zz2) + point.Y * (xy2 - wz2);
            result.Y = point.X * (xy2 + wz2) + point.Y * (1.0f - xx2 - zz2);

            return result;
        }

        /// <summary>
        /// Transforms a point by a quaternion.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to transform.</param>
        /// <param name="quaternion">The quaternion by which to transform the point.</param>
        /// <param name="result">The transformed <see cref="Point2D"/>.</param>
        public static void Transform(ref Point2F point, ref Quaternion quaternion, out Point2F result)
        {
            var x2 = quaternion.X + quaternion.X;
            var y2 = quaternion.Y + quaternion.Y;
            var z2 = quaternion.Z + quaternion.Z;

            var wz2 = quaternion.W * z2;
            var xx2 = quaternion.X * x2;
            var xy2 = quaternion.X * y2;
            var yy2 = quaternion.Y * y2;
            var zz2 = quaternion.Z * z2;

            Point2F temp;

            temp.X = point.X * (1.0f - yy2 - zz2) + point.Y * (xy2 - wz2);
            temp.Y = point.X * (xy2 + wz2) + point.Y * (1.0f - xx2 - zz2);

            result = temp;
        }

        /// <summary>
        /// Transforms a point by a matrix.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the point.</param>
        /// <returns>The transformed <see cref="Point2F"/>.</returns>
        public static Point2F Transform(Point2F point, Matrix matrix)
        {
            Point2F result;

            result.X = (matrix.M11 * point.X + matrix.M21 * point.Y) + matrix.M41;
            result.Y = (matrix.M12 * point.X + matrix.M22 * point.Y) + matrix.M42;

            return result;
        }

        /// <summary>
        /// Transforms a point by a matrix.
        /// </summary>
        /// <param name="point">The <see cref="Point2F"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the point.</param>
        /// <param name="result">The transformed <see cref="Point2F"/>.</param>
        public static void Transform(ref Point2F point, ref Matrix matrix, out Point2F result)
        {
            Point2F temp;

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
            if (!(obj is Point2F))
                return false;
            return Equals((Point2F)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Point2F other)
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
        public Point2F Interpolate(Point2F target, Single t)
        {
            Point2F result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);

            return result;
        }

        /// <summary>
        /// Gets the point at (0, 0).
        /// </summary>
        public static Point2F Zero
        {
            get { return new Point2F(0, 0); }
        }

        /// <summary>
        /// The point's x-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Single X;

        /// <summary>
        /// The point's y-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Single Y;
    }
}
