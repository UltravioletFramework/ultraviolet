using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
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
        [Preserve]
        [JsonConstructor]
        public Circle(Int32 x, Int32 y, Int32 radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="position">The position of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        [Preserve]
        public Circle(Point2 position, Int32 radius)
            : this(position.X, position.Y, radius)
        {

        }

        /// <summary>
        /// Compares two circles for equality.
        /// </summary>
        /// <param name="c1">The first <see cref="Circle"/> to compare.</param>
        /// <param name="c2">The second <see cref="Circle"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified circles are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Circle c1, Circle c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Compares two circles for inequality.
        /// </summary>
        /// <param name="c1">The first <see cref="Circle"/> to compare.</param>
        /// <param name="c2">The second <see cref="Circle"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified circles are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Circle c1, Circle c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by adding the specified <see cref="Point2"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="Circle"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static Circle operator +(Circle circle, Point2 point)
        {
            Circle result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by subtracting the specified <see cref="Point2"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="Circle"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static Circle operator -(Circle circle, Point2 point)
        {
            Circle result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by adding the specified <see cref="Point2F"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleF operator +(Circle circle, Point2F point)
        {
            CircleF result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by subtracting the specified <see cref="Point2F"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleF"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleF operator -(Circle circle, Point2F point)
        {
            CircleF result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator +(Circle circle, Point2D point)
        {
            CircleD result;

            result.X = circle.X + point.X;
            result.Y = circle.Y + point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Offsets the specified <see cref="Circle"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="Circle"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator -(Circle circle, Point2D point)
        {
            CircleD result;

            result.X = circle.X - point.X;
            result.Y = circle.Y - point.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Circle"/> structure to a <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator CircleF(Circle circle)
        {
            CircleF result;

            result.X = circle.X;
            result.Y = circle.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Implicitly converts a <see cref="Circle"/> structure to a <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static implicit operator CircleD(Circle circle)
        {
            CircleD result;

            result.X = circle.X;
            result.Y = circle.Y;
            result.Radius = circle.Radius;

            return result;
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="circle">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Circle circle)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out circle);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <returns>A instance of the <see cref="Circle"/> structure equivalent to the circle contained in <paramref name="s"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Circle circle)
        {
            circle = default(Circle);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
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
        [Preserve]
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
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Radius.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2}", X, Y, Radius);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Circle other)
        {
            return X == other.X && Y == other.Y && Radius == other.Radius;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Circle Interpolate(Circle target, Single t)
        {
            Circle result;

            result.X = Tweening.Lerp(this.X, target.X, t);
            result.Y = Tweening.Lerp(this.Y, target.Y, t);
            result.Radius = Tweening.Lerp(this.Radius, target.Radius, t);

            return result;
        }

        /// <summary>
        /// Gets an instance representing the unit circle.
        /// </summary>
        public static Circle Unit
        {
            get { return new Circle(0, 0, 1); }
        }

        /// <summary>
        /// The circle's position.
        /// </summary>
        [JsonIgnore]
        public Point2 Position
        {
            get { return new Point2(X, Y); }
        }

        /// <summary>
        /// The x-coordinate of the circle's center.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Int32 X;

        /// <summary>
        /// The y-coordinate of the circle's center.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Int32 Y;

        /// <summary>
        /// Gets the circle's radius.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "radius", Required = Required.Always)]
        public Int32 Radius;
    }
}
