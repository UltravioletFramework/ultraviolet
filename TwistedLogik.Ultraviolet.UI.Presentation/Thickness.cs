using System;
using System.Diagnostics;
using System.Globalization;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the thickness of a frame around a rectangle.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{Left:{Left} Top:{Top} Right:{Right} Bottom:{Bottom}\}")]
    public struct Thickness : IEquatable<Thickness>, IInterpolatable<Thickness>
    {
        /// <summary>
        /// Initializes the <see cref="Thickness"/> type.
        /// </summary>
        static Thickness()
        {
            Tweening.Interpolators.RegisterDefault<Thickness>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="value">The size, in device independent pixels, of the bounding rectangles' four edges.</param>
        public Thickness(Double value)
            : this(value, value, value, value)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="left">The width, in device independent pixels (1/96 of an inch), of the left side of the bounding rectangle.</param>
        /// <param name="top">The width, in device independent pixels (1/96 of an inch), of the top side of the bounding rectangle.</param>
        /// <param name="right">The width, in device independent pixels (1/96 of an inch), of the right side of the bounding rectangle.</param>
        /// <param name="bottom">The width, in device independent pixels (1/96 of an inch), of the bottom side of the bounding rectangle.</param>
        public Thickness(Double left, Double top, Double right, Double bottom)
        {
            this.left   = left;
            this.top    = top;
            this.right  = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Adds two <see cref="Thickness"/> values together.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to add.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to add.</param>
        /// <returns>A <see cref="Thickness"/> that is the sum of the specified <see cref="Thickness"/> values.</returns>
        public static Thickness operator +(Thickness t1, Thickness t2)
        {
            return new Thickness(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> value from another <see cref="Thickness"/> value.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to subtract.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to subtract.</param>
        /// <returns>A <see cref="Thickness"/> that is the difference of the specified <see cref="Thickness"/> values.</returns>
        public static Thickness operator -(Thickness t1, Thickness t2)
        {
            return new Thickness(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size2D"/> to which to add the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="Size2D"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the sum of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator +(Size2D size, Thickness thickness)
        {
            var width  = Math.Max(0, size.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, size.Height + thickness.Top + thickness.Bottom);

            return new Size2D(width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="Size2D"/>.</param>
        /// <param name="size">The <see cref="Size2D"/> to which to add the <see cref="Thickness"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the sum of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator +(Thickness thickness, Size2D size)
        {
            var width  = Math.Max(0, size.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, size.Height + thickness.Top + thickness.Bottom);

            return new Size2D(width, height);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> from a <see cref="Size2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size2D"/> from which to subtract the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to subtract from the <see cref="Size2D"/>.</param>
        /// <returns>A <see cref="Size2D"/> that is the difference of the original <see cref="Size2D"/> and the specified <see cref="Thickness"/>.</returns>
        public static Size2D operator -(Size2D size, Thickness thickness)
        {
            var width  = Math.Max(0, size.Width - (thickness.Left + thickness.Right));
            var height = Math.Max(0, size.Height - (thickness.Top + thickness.Bottom));

            return new Size2D(width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> to which to add the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="RectangleD"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the sum of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator +(RectangleD rectangle, Thickness thickness)
        {
            var width = Math.Max(0, rectangle.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, rectangle.Height + thickness.Top + thickness.Bottom);

            return new RectangleD(rectangle.X - thickness.Left, rectangle.Y - thickness.Top, width, height);
        }

        /// <summary>
        /// Adds a <see cref="Thickness"/> to a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="thickness">The <see cref="Thickness"/> to add to the <see cref="RectangleD"/>.</param>
        /// <param name="rectangle">The <see cref="RectangleD"/> to which to add the <see cref="Thickness"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the sum of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator +(Thickness thickness, RectangleD rectangle)
        {
            var width = Math.Max(0, rectangle.Width + thickness.Left + thickness.Right);
            var height = Math.Max(0, rectangle.Height + thickness.Top + thickness.Bottom);

            return new RectangleD(rectangle.X - thickness.Left, rectangle.Y - thickness.Top, width, height);
        }

        /// <summary>
        /// Subtracts a <see cref="Thickness"/> from a <see cref="RectangleD"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleD"/> from which to subtract the <see cref="Thickness"/>.</param>
        /// <param name="thickness">The <see cref="Thickness"/> to subtract from the <see cref="RectangleD"/>.</param>
        /// <returns>A <see cref="RectangleD"/> that is the difference of the original <see cref="RectangleD"/> and the specified <see cref="Thickness"/>.</returns>
        public static RectangleD operator -(RectangleD rectangle, Thickness thickness)
        {
            var width  = Math.Max(0, rectangle.Width - (thickness.Left + thickness.Right));
            var height = Math.Max(0, rectangle.Height - (thickness.Top + thickness.Bottom));

            return new RectangleD(rectangle.X + thickness.Left, rectangle.Y + thickness.Top, width, height);
        }

        /// <summary>
        /// Compares two <see cref="Thickness"/> values for equality.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to compare.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to compare.</param>
        /// <returns><c>true</c> if the specified thicknesses are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Thickness t1, Thickness t2)
        {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Compares two <see cref="Thickness"/> values for inequality.
        /// </summary>
        /// <param name="t1">The first <see cref="Thickness"/> to compare.</param>
        /// <param name="t2">The second <see cref="Thickness"/> to compare.</param>
        /// <returns><c>true</c> if the specified thicknesses are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Thickness t1, Thickness t2)
        {
            return !t1.Equals(t2);
        }

        /// <summary>
        /// Converts the string representation of a bounding rectangle into an instance of the <see cref="Thickness"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a bounding rectangle to convert.</param>
        /// <param name="thickness">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Thickness thickness)
        {
            return TryParse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out thickness);
        }

        /// <summary>
        /// Converts the string representation of a bounding rectangle into an instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="s">A string containing a bounding rectangle to convert.</param>
        /// <returns>A instance of the <see cref="Thickness"/> structure equivalent to the bounding rectangle contained in <paramref name="s"/>.</returns>
        public static Thickness Parse(String s)
        {
            return Parse(s, NumberStyles.Float, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a bounding rectangle into an instance of the <see cref="Thickness"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a bounding rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="thickness">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Thickness thickness)
        {
            Contract.Require(s, "s");

            thickness = default(Thickness);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 1 && components.Length != 4)
                return false;

            if (components.Length == 1)
            {
                Double value;
                if (!Double.TryParse(components[0], style, provider, out value))
                    return false;

                thickness = new Thickness(value);
            }
            else
            {
                Double top, left, right, bottom;
                if (!Double.TryParse(components[0], style, provider, out top))
                    return false;
                if (!Double.TryParse(components[1], style, provider, out left))
                    return false;
                if (!Double.TryParse(components[2], style, provider, out right))
                    return false;
                if (!Double.TryParse(components[3], style, provider, out bottom))
                    return false;

                thickness = new Thickness(top, left, right, bottom);
            }

            return true;
        }

        /// <summary>
        /// Converts the string representation of a bounding rectangle into an instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="s">A string containing a bounding rectangle to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Thickness"/> structure equivalent to the bounding rectangle contained in <paramref name="s"/>.</returns>
        public static Thickness Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Thickness thickness;
            if (!TryParse(s, style, provider, out thickness))
            {
                throw new FormatException();
            }
            return thickness;
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
                hash = hash * 23 + top.GetHashCode();
                hash = hash * 23 + left.GetHashCode();
                hash = hash * 23 + right.GetHashCode();
                hash = hash * 23 + bottom.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2} {3}", top, left, right, bottom);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Thickness))
                return false;
            return Equals((Thickness)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Thickness other)
        {
            return
                left == other.left &&
                top == other.top &&
                right == other.right &&
                bottom == other.bottom;                
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Thickness Interpolate(Thickness target, Single t)
        {
            var left   = Tweening.Lerp(this.left, target.left, t);
            var top    = Tweening.Lerp(this.top, target.top, t);
            var right  = Tweening.Lerp(this.right, target.right, t);
            var bottom = Tweening.Lerp(this.bottom, target.bottom, t);
            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Gets an instance representing a thickness of zero on all edges.
        /// </summary>
        public static Thickness Zero
        {
            get { return new Thickness(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets an instance representing a thickness of one on all edges.
        /// </summary>
        public static Thickness One
        {
            get { return new Thickness(1, 1, 1, 1); }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the left side of the bounding rectangle.
        /// </summary>
        public Double Left
        {
            get { return left; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the top side of the bounding rectangle.
        /// </summary>
        public Double Top
        {
            get { return top; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the right side of the bounding rectangle.
        /// </summary>
        public Double Right
        {
            get { return right; }
        }

        /// <summary>
        /// Gets the width, in device independent pixels (1/96 of an inch), of the bottom side of the bounding rectangle.
        /// </summary>
        public Double Bottom
        {
            get { return bottom; }
        }

        // Property values.
        private readonly Double left;
        private readonly Double top;
        private readonly Double right;
        private readonly Double bottom;
    }
}
