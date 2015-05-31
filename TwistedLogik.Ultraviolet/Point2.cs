using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a point in two-dimensional space.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y}\}")]
    public struct Point2 : IEquatable<Point2>, IInterpolatable<Point2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="x">The point's x-coordinate.</param>
        /// <param name="y">The point's y-coordinate.</param>
        public Point2(Int32 x, Int32 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Adds two points.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2"/> to add.</param>
        /// <param name="p2">The second <see cref="Point2"/> to add.</param>
        /// <returns>A <see cref="Point2"/> that represents the sum of the specified points.</returns>
        public static Point2 operator +(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Subtracts one point from another point.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2"/> to subtract.</param>
        /// <param name="p2">The second <see cref="Point2"/> to subtract.</param>
        /// <returns>A <see cref="Point2"/> that represents the difference of the specified points.</returns>
        public static Point2 operator -(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Offsets a point by adding the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2"/> to offset.</param>
        /// <param name="offset">The <see cref="Size2"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2"/> that represents the original point plus the specified offset.</returns>
        public static Point2 operator +(Point2 point, Size2 offset)
        {
            return new Point2(point.X + offset.Width, point.Y + offset.Height);
        }

        /// <summary>
        /// Offsets a point by subtracting the specified size.
        /// </summary>
        /// <param name="point">The <see cref="Point2"/> to size.</param>
        /// <param name="offset">The <see cref="Size2"/> that specifies how much to offset <paramref name="point"/>.</param>
        /// <returns>A <see cref="Point2"/> that represents the original point plus the specified offset.</returns>
        public static Point2 operator -(Point2 point, Size2 offset)
        {
            return new Point2(point.X - offset.Width, point.Y - offset.Height);
        }

        /// <summary>
        /// Compares two points for equality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2"/> to compare.</param>
        /// <returns><c>true</c> if the specified points are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Point2 p1, Point2 p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Compares two points for inequality.
        /// </summary>
        /// <param name="p1">The first <see cref="Point2"/> to compare.</param>
        /// <param name="p2">The second <see cref="Point2"/> to compare.</param>
        /// <returns><c>true</c> if the specified points are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Point2 p1, Point2 p2)
        {
            return !p1.Equals(p2);
        }
        
        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Point2 point)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out point);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <returns>A instance of the <see cref="Point2"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        public static Point2 Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="point">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Point2 point)
        {
            point = default(Point2);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 2)
                return false;

            Int32 x, y;
            if (!Int32.TryParse(components[0], style, provider, out x))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out y))
                return false;

            point = new Point2(x, y);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a point into an instance of the <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a point to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Point2"/> structure equivalent to the point contained in <paramref name="s"/>.</returns>
        public static Point2 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Point2 point;
            if (!TryParse(s, style, provider, out point))
            {
                throw new FormatException();
            }
            return point;
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
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
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
            return String.Format(provider, "{0} {1}", x, y);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Point2))
                return false;
            return Equals((Point2)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Point2 other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Point2 Interpolate(Point2 target, Single t)
        {
            var width  = Tweening.Lerp(this.x, target.x, t);
            var height = Tweening.Lerp(this.y, target.y, t);
            return new Point2(width, height);
        }

        /// <summary>
        /// Gets the point at (0, 0).
        /// </summary>
        public static Point2 Zero
        {
            get { return new Point2(0, 0); }
        }

        /// <summary>
        /// Gets the point's x-coordinate.
        /// </summary>
        public Int32 X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the point's y-coordinate.
        /// </summary>
        public Int32 Y
        {
            get { return y; }
        }

        // Property values.
        private readonly Int32 x;
        private readonly Int32 y;
    }
}
