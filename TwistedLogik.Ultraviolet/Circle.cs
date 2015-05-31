using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a circle with integer radius and position.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Radius:{Radius}\}")]
    public struct Circle : IEquatable<Circle>, IInterpolatable<Circle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        public Circle(Int32 x, Int32 y, Int32 radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="position">The position of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        public Circle(Point2 position, Int32 radius)
            : this(position.X, position.Y, radius)
        {

        }

        /// <summary>
        /// Compares two circles for equality.
        /// </summary>
        /// <param name="c1">The first <see cref="Circle"/> to compare.</param>
        /// <param name="c2">The second <see cref="Circle"/> to compare.</param>
        /// <returns><c>true</c> if the specified circles are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Circle c1, Circle c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Compares two circles for inequality.
        /// </summary>
        /// <param name="c1">The first <see cref="Circle"/> to compare.</param>
        /// <param name="c2">The second <see cref="Circle"/> to compare.</param>
        /// <returns><c>true</c> if the specified circles are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Circle c1, Circle c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Circle"/> structure to a <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static implicit operator CircleF(Circle circle)
        {
            return new CircleF(circle.x, circle.y, circle.radius);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="circle">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Circle circle)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out circle);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <returns>A instance of the <see cref="Circle"/> structure equivalent to the circle contained in <paramref name="s"/>.</returns>
        public static Circle Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="circle">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Circle circle)
        {
            circle = default(Circle);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 3)
                return false;

            Int32 x, y, radius;
            if (!Int32.TryParse(components[0], style, provider, out x))
                return false;
            if (!Int32.TryParse(components[1], style, provider, out y))
                return false;
            if (!Int32.TryParse(components[2], style, provider, out radius))
                return false;

            circle = new Circle(x, y, radius);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Circle"/> structure equivalent to the circle contained in <paramref name="s"/>.</returns>
        public static Circle Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Circle circle;
            if (!TryParse(s, style, provider, out circle))
            {
                throw new FormatException();
            }
            return circle;
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
                hash = hash * 23 + radius.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2}", x, y, radius);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Circle))
                return false;
            return Equals((Circle)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Circle other)
        {
            return x == other.x && y == other.y && radius == other.radius;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Circle Interpolate(Circle target, Single t)
        {
            var x      = Tweening.Lerp(this.x, target.x, t);
            var y      = Tweening.Lerp(this.y, target.y, t);
            var radius = Tweening.Lerp(this.radius, target.radius, t);
            return new Circle(x, y, radius);
        }

        /// <summary>
        /// Gets an instance representing the unit circle.
        /// </summary>
        public static Circle Unit
        {
            get { return new Circle(0, 0, 1); }
        }

        /// <summary>
        /// Gets the circle's position.
        /// </summary>
        public Point2 Position
        {
            get { return new Point2(x, y); }
        }

        /// <summary>
        /// Gets the x-coordinate of the circle's center.
        /// </summary>
        public Int32 X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the circle's center.
        /// </summary>
        public Int32 Y
        {
            get { return y; } 
        }

        /// <summary>
        /// Gets the circle's radius.
        /// </summary>
        public Int32 Radius
        {
            get { return radius; }
        }

        // Property values.
        private readonly Int32 x;
        private readonly Int32 y;
        private readonly Int32 radius;
    }
}
