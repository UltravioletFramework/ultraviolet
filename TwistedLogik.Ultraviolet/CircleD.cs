using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a circle with single-precision floating point radius and position.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Radius:{Radius}\}")]
    public struct CircleD : IEquatable<CircleD>, IInterpolatable<CircleD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        [Preserve]
        [JsonConstructor]
        public CircleD(Double x, Double y, Double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="position">The position of the circle's center.</param>
        /// <param name="radius">The circle's radius.</param>
        [Preserve]
        public CircleD(Point2D position, Double radius)
            : this(position.X, position.Y, radius)
        {

        }

        /// <summary>
        /// Compares two circles for equality.
        /// </summary>
        /// <param name="c1">The first <see cref="CircleD"/> to compare.</param>
        /// <param name="c2">The second <see cref="CircleD"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified circles are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(CircleD c1, CircleD c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Compares two circles for inequality.
        /// </summary>
        /// <param name="c1">The first <see cref="CircleD"/> to compare.</param>
        /// <param name="c2">The second <see cref="CircleD"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified circles are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(CircleD c1, CircleD c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator +(CircleD circle, Point2 point)
        {
            return new CircleD(circle.Position + point, circle.Radius);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator -(CircleD circle, Point2 point)
        {
            return new CircleD(circle.Position - point, circle.Radius);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator +(CircleD circle, Point2F point)
        {
            return new CircleD(circle.Position + point, circle.Radius);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2F"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator -(CircleD circle, Point2F point)
        {
            return new CircleD(circle.Position - point, circle.Radius);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by adding the specified <see cref="Point2D"/> to its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator +(CircleD circle, Point2D point)
        {
            return new CircleD(circle.Position + point, circle.Radius);
        }

        /// <summary>
        /// Offsets the specified <see cref="CircleD"/> by subtracting the specified <see cref="Point2D"/> from its location.
        /// </summary>
        /// <param name="circle">The <see cref="CircleD"/> to offset.</param>
        /// <param name="point">The <see cref="Point2D"/> by which to offset the Circle.</param>
        /// <returns>A <see cref="CircleD"/> that has been offset by the specified amount.</returns>
        [Preserve]
        public static CircleD operator -(CircleD circle, Point2D point)
        {
            return new CircleD(circle.Position - point, circle.Radius);
        }

        /// <summary>
        /// Explicitly converts a <see cref="CircleD"/> structure to a <see cref="Circle"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator Circle(CircleD circle)
        {
            return new Circle((Int32)circle.x, (Int32)circle.y, (Int32)circle.radius);
        }

        /// <summary>
        /// Explicitly converts a <see cref="CircleD"/> structure to a <see cref="CircleF"/> structure.
        /// </summary>
        /// <param name="circle">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        [Preserve]
        public static explicit operator CircleF(CircleD circle)
        {
            return new CircleF((Single)circle.x, (Single)circle.y, (Single)circle.radius);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="CircleD"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="circle">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out CircleD circle)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out circle);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <returns>A instance of the <see cref="CircleD"/> structure equivalent to the circle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static CircleD Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="CircleD"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="circle">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out CircleD circle)
        {
            circle = default(CircleD);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries); 
            if (components.Length != 3)
                return false;

            Single x, y, radius;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out radius))
                return false;

            circle = new CircleD(x, y, radius);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a circle into an instance of the <see cref="CircleD"/> structure.
        /// </summary>
        /// <param name="s">A string containing a circle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="CircleD"/> structure equivalent to the circle contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static CircleD Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            CircleD circle;
            if (!TryParse(s, style, provider, out circle))
                throw new FormatException();
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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is CircleD))
                return false;
            return Equals((CircleD)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        public Boolean Equals(CircleD other)
        {
            return x == other.x && y == other.y && radius == other.radius;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public CircleD Interpolate(CircleD target, Single t)
        {
            var x      = Tweening.Lerp(this.x, target.x, t);
            var y      = Tweening.Lerp(this.y, target.y, t);
            var radius = Tweening.Lerp(this.radius, target.radius, t);
            return new CircleD(x, y, radius);
        }

        /// <summary>
        /// Gets an instance which represents the unit circle.
        /// </summary>
        public static CircleD Unit
        {
            get { return new CircleD(0, 0, 1f); }
        }

        /// <summary>
        /// Gets the circle's position.
        /// </summary>
        [JsonIgnore]
        public Point2D Position
        {
            get { return new Point2D(x, y); }
        }

        /// <summary>
        /// Gets the x-coordinate of the circle's center.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Double X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate of the circle's center.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Double Y
        {
            get { return y; } 
        }

        /// <summary>
        /// Gets the circle's radius.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "radius", Required = Required.Always)]
        public Double Radius
        {
            get { return radius; }
        }

        // Property values.
        private readonly Double x;
        private readonly Double y;
        private readonly Double radius;
    }
}
