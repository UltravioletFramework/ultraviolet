using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents an RGBA color.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{A:{A} R:{R} G:{G} B:{B}\}")]
    public struct Color : IEquatable<Color>, IInterpolatable<Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="packedValue">The color's packed value.</param>
        [CLSCompliant(false)]
        public Color(UInt32 packedValue)
        {
            this.packedValue = packedValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified component values.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Color(Single r, Single g, Single b)
        {
            if (r < 0) r = 0;
            if (r > 1) r = 1;
            if (g < 0) g = 0;
            if (g > 1) g = 1;
            if (b < 0) b = 0;
            if (b > 1) b = 1;

            var rbyte = (byte)(r * Byte.MaxValue);
            var gbyte = (byte)(g * Byte.MaxValue);
            var bbyte = (byte)(b * Byte.MaxValue);

            this.packedValue = (uint)((rbyte) | (gbyte << 8) | (bbyte << 16) | (255 << 24));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified component values.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Color(Single r, Single g, Single b, Single a)
        {
            if (r < 0) r = 0;
            if (r > 1) r = 1;
            if (g < 0) g = 0;
            if (g > 1) g = 1;
            if (b < 0) b = 0;
            if (b > 1) b = 1;
            if (a < 0) a = 0;
            if (a > 1) a = 1;

            var rbyte = (byte)(r * Byte.MaxValue);
            var gbyte = (byte)(g * Byte.MaxValue); 
            var bbyte = (byte)(b * Byte.MaxValue);
            var abyte = (byte)(a * Byte.MaxValue);

            this.packedValue = (uint)((rbyte) | (gbyte << 8) | (bbyte << 16) | (abyte << 24));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified component values.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Color(Int32 r, Int32 g, Int32 b)
        {
            if (r < 0) r = 0;
            if (r > Byte.MaxValue) r = Byte.MaxValue;
            if (g < 0) g = 0;
            if (g > Byte.MaxValue) g = Byte.MaxValue;
            if (b < 0) b = 0;
            if (b > Byte.MaxValue) b = Byte.MaxValue;

            this.packedValue = (uint)((r) | (g << 8) | (b << 16) | (255 << 24));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified component values.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Color(Int32 r, Int32 g, Int32 b, Int32 a)
        {
            if (r < 0) r = 0;
            if (r > Byte.MaxValue) r = Byte.MaxValue;
            if (g < 0) g = 0;
            if (g > Byte.MaxValue) g = Byte.MaxValue;
            if (b < 0) b = 0;
            if (b > Byte.MaxValue) b = Byte.MaxValue;
            if (a < 0) a = 0;
            if (a > Byte.MaxValue) a = Byte.MaxValue;

            this.packedValue = (uint)((r) | (g << 8) | (b << 16) | (a << 24));
        }

        /// <summary>
        /// Compares two colors for equality.
        /// </summary>
        /// <param name="c1">The first <see cref="Color"/> to compare.</param>
        /// <param name="c2">The second <see cref="Color"/> to compare.</param>
        /// <returns><c>true</c> if the specified colors are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Color c1, Color c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Compares two colors for inequality.
        /// </summary>
        /// <param name="c1">The first <see cref="Color"/> to compare.</param>
        /// <param name="c2">The second <see cref="Color"/> to compare.</param>
        /// <returns><c>true</c> if the specified colors are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Color c1, Color c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Multiplies each of the color's components by the specified scaling factor.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to multiply.</param>
        /// <param name="alpha">The scaling factor by which to multiply the color.</param>
        /// <returns>The scaled <see cref="Color"/>.</returns>
        public static Color operator *(Color color, Single alpha)
        {
            if (alpha < 0)
                alpha = 0;

            if (alpha > 1)
                alpha = 1;

            var r = (Byte)(color.R * alpha);
            var g = (Byte)(color.G * alpha);
            var b = (Byte)(color.B * alpha);
            var a = (Byte)(color.A * alpha);

            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Color color)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out color);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <returns>A instance of the <see cref="Color"/> structure equivalent to the color contained in <paramref name="s"/>.</returns>
        public static Color Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Color color)
        {
            color = default(Color);

            if (String.IsNullOrEmpty(s))
                return false;

            var trimmed = s.Trim();
            if (trimmed.StartsWith("#"))
            {
                return TryParseHex(trimmed, style, provider, ref color);
            }
            else
            {
                return TryParseDelimited(trimmed, style, provider, ref color);
            }
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Color"/> structure equivalent to the color contained in <paramref name="s"/>.</returns>
        public static Color Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Color color;
            if (!TryParse(s, style, provider, out color))
                throw new FormatException();
            return color;
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from a 32-bit integer in ARGB format.
        /// </summary>
        /// <param name="value">The integer from which to create a color.</param>
        /// <returns>The <see cref="Color"/> that corresponds to the specified integer value.</returns>
        [CLSCompliant(false)]
        public static Color FromArgb(UInt32 value)
        {
            var a = (byte)(value >> 24);
            var r = (byte)(value >> 16);
            var g = (byte)(value >> 8);
            var b = (byte)(value);

            return new Color((uint)((r) | (g << 8) | (b << 16) | (a << 24)));
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from a 32-bit integer in RGBA format.
        /// </summary>
        /// <param name="value">The integer from which to create a color.</param>
        /// <returns>The <see cref="Color"/> that corresponds to the specified integer value.</returns>
        [CLSCompliant(false)]
        public static Color FromRgba(UInt32 value)
        {
            return new Color(value);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from a 32-bit integer in BGRA format.
        /// </summary>
        /// <param name="value">The integer from which to create a color.</param>
        /// <returns>The <see cref="Color"/> that corresponds to the specified integer value.</returns>
        [CLSCompliant(false)]
        public static Color FromBgra(UInt32 value)
        {
            var a = (byte)(value >> 24);
            var b = (byte)(value >> 16);
            var g = (byte)(value >> 8);
            var r = (byte)(value);

            return new Color((uint)((r) | (g << 8) | (b << 16) | (a << 24)));
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            return packedValue.GetHashCode();
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
            return String.Format(provider, "#{0:x2}{1:x2}{2:x2}{3:x2}", A, R, G, B);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Color))
                return false;
            return Equals((Color)obj);
        }
        
        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Color other)
        {
            return packedValue == other.packedValue;
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a 32-bit integer in ARGB format.
        /// </summary>
        /// <returns>The ARGB value that corresponds to this <see cref="Color"/>.</returns>
        [CLSCompliant(false)]
        public UInt32 ToArgb()
        {
            return (uint)((A << 24) | (R << 16) | (G << 8) | (B));
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a 32-bit integer in RGBA format.
        /// </summary>
        /// <returns>The RGBA value that corresponds to this <see cref="Color"/>.</returns>
        [CLSCompliant(false)]
        public UInt32 ToRgba()
        {
            return packedValue;
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a 32-bit integer in BGRA format.
        /// </summary>
        /// <returns>The BGRA value that corresponds to this <see cref="Color"/>.</returns>
        [CLSCompliant(false)]
        public UInt32 ToBgra()
        {
            return (uint)((A << 24) | (B << 16) | (G << 8) | (R));
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Color Interpolate(Color target, Single t)
        {
            var a = Tweening.Lerp(this.A, target.A, t);
            var r = Tweening.Lerp(this.R, target.R, t);
            var g = Tweening.Lerp(this.G, target.G, t);
            var b = Tweening.Lerp(this.B, target.B, t);
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Gets the color's packed value.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32 PackedValue
        {
            get { return packedValue; }
        }

        /// <summary>
        /// Gets the color's alpha component.
        /// </summary>
        public Byte A
        {
            get { return (byte)(packedValue >> 24); }
        }

        /// <summary>
        /// Gets the color's red component.
        /// </summary>
        public Byte R
        {
            get { return (byte)(packedValue); }
        }

        /// <summary>
        /// Gets the color's green component.
        /// </summary>
        public Byte G
        {
            get { return (byte)(packedValue >> 8); }
        }

        /// <summary>
        /// Gets the color's blue component.
        /// </summary>
        public Byte B
        {
            get { return (byte)(packedValue >> 16); }
        }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:0.
        /// </summary>
        public static Color Transparent { get { return Color.FromArgb(0x00000000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:255.
        /// </summary>
        public static Color Black { get { return Color.FromArgb(0xFF000000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:255 A:255.
        /// </summary>
        public static Color White { get { return Color.FromArgb(0xFFFFFFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:0 A:255.
        /// </summary>
        public static Color Red { get { return Color.FromArgb(0xFFFF0000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:0 A:255.
        /// </summary>
        public static Color Lime { get { return Color.FromArgb(0xFF00FF00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:128 B:0 A:255.
        /// </summary>
        public static Color Green { get { return Color.FromArgb(0xFF008000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:255 A:255.
        /// </summary>
        public static Color Blue { get { return Color.FromArgb(0xFF0000FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:255 A:255.
        /// </summary>
        public static Color Magenta { get { return Color.FromArgb(0xFFFF00FF); } }

        /// <summary>
        /// Gets a system-defined <see cref="Color"/> with the value R:100 G:149 B:237 A:255.
        /// </summary>
        public static Color CornflowerBlue { get { return Color.FromArgb(0xFF6495ED); } }

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>Strings parsed by this method must be 8-character hexadecimal numbers prefixed by the hash (#) symbol,
        /// in the format #AARRGGBB.</remarks>
        private static Boolean TryParseHex(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            if (s.Length != "#ffffffff".Length)
                return false;

            var astr = s.Substring(1, 2);
            var rstr = s.Substring(3, 2);
            var gstr = s.Substring(5, 2);
            var bstr = s.Substring(7, 2);

            Int32 a, r, g, b;
            if (!Int32.TryParse(astr, NumberStyles.AllowHexSpecifier, provider, out a))
                return false;
            if (!Int32.TryParse(rstr, NumberStyles.AllowHexSpecifier, provider, out r))
                return false;
            if (!Int32.TryParse(gstr, NumberStyles.AllowHexSpecifier, provider, out g))
                return false;
            if (!Int32.TryParse(bstr, NumberStyles.AllowHexSpecifier, provider, out b))
                return false;

            color = new Color(r, g, b, a);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>Strings parsed by this methods must be comma-delimited lists of three or four color components,
        /// in either R, G, B or A, R, G, B format.</remarks>
        private static Boolean TryParseDelimited(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            var components = s.Split(',');
            if (components.Length == 3)
            {
                Int32 r, g, b;
                if (!Int32.TryParse(components[0], style, provider, out r))
                    return false;
                if (!Int32.TryParse(components[1], style, provider, out g))
                    return false;
                if (!Int32.TryParse(components[2], style, provider, out b))
                    return false;

                color = new Color(r, g, b);
                return true;
            }
            if (components.Length == 4)
            {
                Int32 a, r, g, b;
                if (!Int32.TryParse(components[0], style, provider, out a))
                    return false;
                if (!Int32.TryParse(components[1], style, provider, out r))
                    return false;
                if (!Int32.TryParse(components[2], style, provider, out g))
                    return false;
                if (!Int32.TryParse(components[3], style, provider, out b))
                    return false;

                color = new Color(r, g, b, a);
                return true;
            }
            return false;
        }

        // The packed value of the color in RGBA format.
        private readonly UInt32 packedValue;
    }
}
