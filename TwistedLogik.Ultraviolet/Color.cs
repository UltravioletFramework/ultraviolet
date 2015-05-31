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
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:248 B:255 A:255.
        /// </summary>
        public static Color AliceBlue { get { return Color.FromArgb(0xFFF0F8FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:235 B:215 A:255.
        /// </summary>
        public static Color AntiqueWhite { get { return Color.FromArgb(0xFFFAEBD7); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:255 A:255.
        /// </summary>
        public static Color Aqua { get { return Color.FromArgb(0xFF00FFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:127 G:255 B:212 A:255.
        /// </summary>
        public static Color Aquamarine { get { return Color.FromArgb(0xFF7FFFD4); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:255 B:255 A:255.
        /// </summary>
        public static Color Azure { get { return Color.FromArgb(0xFFF0FFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:245 B:220 A:255.
        /// </summary>
        public static Color Beige { get { return Color.FromArgb(0xFFF5F5DC); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:196 A:255.
        /// </summary>
        public static Color Bisque { get { return Color.FromArgb(0xFFFFE4C4); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:255.
        /// </summary>
        public static Color Black { get { return Color.FromArgb(0xFF000000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:235 B:205 A:255.
        /// </summary>
        public static Color BlanchedAlmond { get { return Color.FromArgb(0xFFFFEBCD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:255 A:255.
        /// </summary>
        public static Color Blue { get { return Color.FromArgb(0xFF0000FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:138 G:43 B:226 A:255.
        /// </summary>
        public static Color BlueViolet { get { return Color.FromArgb(0xFF8A2BE2); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:165 G:42 B:42 A:255.
        /// </summary>
        public static Color Brown { get { return Color.FromArgb(0xFFA52A2A); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:222 G:184 B:135 A:255.
        /// </summary>
        public static Color BurlyWood { get { return Color.FromArgb(0xFFDEB887); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:95 G:158 B:160 A:255.
        /// </summary>
        public static Color CadetBlue { get { return Color.FromArgb(0xFF5F9EA0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:127 G:255 B:0 A:255.
        /// </summary>
        public static Color Chartreuse { get { return Color.FromArgb(0xFF7FFF00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:210 G:105 B:30 A:255.
        /// </summary>
        public static Color Chocolate { get { return Color.FromArgb(0xFFD2691E); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:127 B:80 A:255.
        /// </summary>
        public static Color Coral { get { return Color.FromArgb(0xFFFF7F50); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:100 G:149 B:237 A:255.
        /// </summary>
        public static Color CornflowerBlue { get { return Color.FromArgb(0xFF6495ED); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:248 B:220 A:255.
        /// </summary>
        public static Color Cornsilk { get { return Color.FromArgb(0xFFFFF8DC); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:220 G:20 B:60 A:255.
        /// </summary>
        public static Color Crimson { get { return Color.FromArgb(0xFFDC143C); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:255 A:255.
        /// </summary>
        public static Color Cyan { get { return Color.FromArgb(0xFF00FFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:139 A:255.
        /// </summary>
        public static Color DarkBlue { get { return Color.FromArgb(0xFF00008B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:139 B:139 A:255.
        /// </summary>
        public static Color DarkCyan { get { return Color.FromArgb(0xFF008B8B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:184 G:134 B:11 A:255.
        /// </summary>
        public static Color DarkGoldenrod { get { return Color.FromArgb(0xFFB8860B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:169 G:169 B:169 A:255.
        /// </summary>
        public static Color DarkGray { get { return Color.FromArgb(0xFFA9A9A9); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:100 B:0 A:255.
        /// </summary>
        public static Color DarkGreen { get { return Color.FromArgb(0xFF006400); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:189 G:183 B:107 A:255.
        /// </summary>
        public static Color DarkKhaki { get { return Color.FromArgb(0xFFBDB76B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:0 B:139 A:255.
        /// </summary>
        public static Color DarkMagenta { get { return Color.FromArgb(0xFF8B008B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:85 G:107 B:47 A:255.
        /// </summary>
        public static Color DarkOliveGreen { get { return Color.FromArgb(0xFF556B2F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:140 B:0 A:255.
        /// </summary>
        public static Color DarkOrange { get { return Color.FromArgb(0xFFFF8C00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:153 G:50 B:204 A:255.
        /// </summary>
        public static Color DarkOrchid { get { return Color.FromArgb(0xFF9932CC); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:0 B:0 A:255.
        /// </summary>
        public static Color DarkRed { get { return Color.FromArgb(0xFF8B0000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:233 G:150 B:122 A:255.
        /// </summary>
        public static Color DarkSalmon { get { return Color.FromArgb(0xFFE9967A); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:143 G:188 B:139 A:255.
        /// </summary>
        public static Color DarkSeaGreen { get { return Color.FromArgb(0xFF8FBC8B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:72 G:61 B:139 A:255.
        /// </summary>
        public static Color DarkSlateBlue { get { return Color.FromArgb(0xFF483D8B); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:47 G:79 B:79 A:255.
        /// </summary>
        public static Color DarkSlateGray { get { return Color.FromArgb(0xFF2F4F4F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:206 B:209 A:255.
        /// </summary>
        public static Color DarkTurquoise { get { return Color.FromArgb(0xFF00CED1); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:148 G:0 B:211 A:255.
        /// </summary>
        public static Color DarkViolet { get { return Color.FromArgb(0xFF9400D3); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:20 B:147 A:255.
        /// </summary>
        public static Color DeepPink { get { return Color.FromArgb(0xFFFF1493); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:191 B:255 A:255.
        /// </summary>
        public static Color DeepSkyBlue { get { return Color.FromArgb(0xFF00BFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:105 G:105 B:105 A:255.
        /// </summary>
        public static Color DimGray { get { return Color.FromArgb(0xFF696969); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:30 G:144 B:255 A:255.
        /// </summary>
        public static Color DodgerBlue { get { return Color.FromArgb(0xFF1E90FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:178 G:34 B:34 A:255.
        /// </summary>
        public static Color Firebrick { get { return Color.FromArgb(0xFFB22222); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:240 A:255.
        /// </summary>
        public static Color FloralWhite { get { return Color.FromArgb(0xFFFFFAF0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:34 G:139 B:34 A:255.
        /// </summary>
        public static Color ForestGreen { get { return Color.FromArgb(0xFF228B22); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:255 A:255.
        /// </summary>
        public static Color Fuchsia { get { return Color.FromArgb(0xFFFF00FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:220 G:220 B:220 A:255.
        /// </summary>
        public static Color Gainsboro { get { return Color.FromArgb(0xFFDCDCDC); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:248 G:248 B:255 A:255.
        /// </summary>
        public static Color GhostWhite { get { return Color.FromArgb(0xFFF8F8FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:215 B:0 A:255.
        /// </summary>
        public static Color Gold { get { return Color.FromArgb(0xFFFFD700); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:218 G:165 B:32 A:255.
        /// </summary>
        public static Color Goldenrod { get { return Color.FromArgb(0xFFDAA520); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:128 B:128 A:255.
        /// </summary>
        public static Color Gray { get { return Color.FromArgb(0xFF808080); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:128 B:0 A:255.
        /// </summary>
        public static Color Green { get { return Color.FromArgb(0xFF008000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:173 G:255 B:47 A:255.
        /// </summary>
        public static Color GreenYellow { get { return Color.FromArgb(0xFFADFF2F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:255 B:240 A:255.
        /// </summary>
        public static Color Honeydew { get { return Color.FromArgb(0xFFF0FFF0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:105 B:180 A:255.
        /// </summary>
        public static Color HotPink { get { return Color.FromArgb(0xFFFF69B4); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:205 G:92 B:92 A:255.
        /// </summary>
        public static Color IndianRed { get { return Color.FromArgb(0xFFCD5C5C); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:75 G:0 B:130 A:255.
        /// </summary>
        public static Color Indigo { get { return Color.FromArgb(0xFF4B0082); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:240 A:255.
        /// </summary>
        public static Color Ivory { get { return Color.FromArgb(0xFFFFFFF0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:230 B:140 A:255.
        /// </summary>
        public static Color Khaki { get { return Color.FromArgb(0xFFF0E68C); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:230 G:230 B:250 A:255.
        /// </summary>
        public static Color Lavender { get { return Color.FromArgb(0xFFE6E6FA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:240 B:245 A:255.
        /// </summary>
        public static Color LavenderBlush { get { return Color.FromArgb(0xFFFFF0F5); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:124 G:252 B:0 A:255.
        /// </summary>
        public static Color LawnGreen { get { return Color.FromArgb(0xFF7CFC00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:205 A:255.
        /// </summary>
        public static Color LemonChiffon { get { return Color.FromArgb(0xFFFFFACD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:173 G:216 B:230 A:255.
        /// </summary>
        public static Color LightBlue { get { return Color.FromArgb(0xFFADD8E6); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:128 B:128 A:255.
        /// </summary>
        public static Color LightCoral { get { return Color.FromArgb(0xFFF08080); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:224 G:255 B:255 A:255.
        /// </summary>
        public static Color LightCyan { get { return Color.FromArgb(0xFFE0FFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:250 B:210 A:255.
        /// </summary>
        public static Color LightGoldenrodYellow { get { return Color.FromArgb(0xFFFAFAD2); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:211 G:211 B:211 A:255.
        /// </summary>
        public static Color LightGray { get { return Color.FromArgb(0xFFD3D3D3); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:144 G:238 B:144 A:255.
        /// </summary>
        public static Color LightGreen { get { return Color.FromArgb(0xFF90EE90); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:182 B:193 A:255.
        /// </summary>
        public static Color LightPink { get { return Color.FromArgb(0xFFFFB6C1); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:160 B:122 A:255.
        /// </summary>
        public static Color LightSalmon { get { return Color.FromArgb(0xFFFFA07A); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:32 G:178 B:170 A:255.
        /// </summary>
        public static Color LightSeaGreen { get { return Color.FromArgb(0xFF20B2AA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:135 G:206 B:250 A:255.
        /// </summary>
        public static Color LightSkyBlue { get { return Color.FromArgb(0xFF87CEFA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:119 G:136 B:153 A:255.
        /// </summary>
        public static Color LightSlateGray { get { return Color.FromArgb(0xFF778899); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:176 G:196 B:222 A:255.
        /// </summary>
        public static Color LightSteelBlue { get { return Color.FromArgb(0xFFB0C4DE); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:224 A:255.
        /// </summary>
        public static Color LightYellow { get { return Color.FromArgb(0xFFFFFFE0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:0 A:255.
        /// </summary>
        public static Color Lime { get { return Color.FromArgb(0xFF00FF00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:50 G:205 B:50 A:255.
        /// </summary>
        public static Color LimeGreen { get { return Color.FromArgb(0xFF32CD32); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:240 B:230 A:255.
        /// </summary>
        public static Color Linen { get { return Color.FromArgb(0xFFFAF0E6); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:255 A:255.
        /// </summary>
        public static Color Magenta { get { return Color.FromArgb(0xFFFF00FF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:0 B:0 A:255.
        /// </summary>
        public static Color Maroon { get { return Color.FromArgb(0xFF800000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:102 G:205 B:170 A:255.
        /// </summary>
        public static Color MediumAquamarine { get { return Color.FromArgb(0xFF66CDAA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:205 A:255.
        /// </summary>
        public static Color MediumBlue { get { return Color.FromArgb(0xFF0000CD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:186 G:85 B:211 A:255.
        /// </summary>
        public static Color MediumOrchid { get { return Color.FromArgb(0xFFBA55D3); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:147 G:112 B:219 A:255.
        /// </summary>
        public static Color MediumPurple { get { return Color.FromArgb(0xFF9370DB); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:60 G:179 B:113 A:255.
        /// </summary>
        public static Color MediumSeaGreen { get { return Color.FromArgb(0xFF3CB371); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:123 G:104 B:238 A:255.
        /// </summary>
        public static Color MediumSlateBlue { get { return Color.FromArgb(0xFF7B68EE); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:250 B:154 A:255.
        /// </summary>
        public static Color MediumSpringGreen { get { return Color.FromArgb(0xFF00FA9A); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:72 G:209 B:204 A:255.
        /// </summary>
        public static Color MediumTurquoise { get { return Color.FromArgb(0xFF48D1CC); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:199 G:21 B:133 A:255.
        /// </summary>
        public static Color MediumVioletRed { get { return Color.FromArgb(0xFFC71585); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:25 G:25 B:112 A:255.
        /// </summary>
        public static Color MidnightBlue { get { return Color.FromArgb(0xFF191970); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:255 B:250 A:255.
        /// </summary>
        public static Color MintCream { get { return Color.FromArgb(0xFFF5FFFA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:225 A:255.
        /// </summary>
        public static Color MistyRose { get { return Color.FromArgb(0xFFFFE4E1); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:181 A:255.
        /// </summary>
        public static Color Moccasin { get { return Color.FromArgb(0xFFFFE4B5); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:222 B:173 A:255.
        /// </summary>
        public static Color NavajoWhite { get { return Color.FromArgb(0xFFFFDEAD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:128 A:255.
        /// </summary>
        public static Color Navy { get { return Color.FromArgb(0xFF000080); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:253 G:245 B:230 A:255.
        /// </summary>
        public static Color OldLace { get { return Color.FromArgb(0xFFFDF5E6); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:128 B:0 A:255.
        /// </summary>
        public static Color Olive { get { return Color.FromArgb(0xFF808000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:107 G:142 B:35 A:255.
        /// </summary>
        public static Color OliveDrab { get { return Color.FromArgb(0xFF6B8E23); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:165 B:0 A:255.
        /// </summary>
        public static Color Orange { get { return Color.FromArgb(0xFFFFA500); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:69 B:0 A:255.
        /// </summary>
        public static Color OrangeRed { get { return Color.FromArgb(0xFFFF4500); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:218 G:112 B:214 A:255.
        /// </summary>
        public static Color Orchid { get { return Color.FromArgb(0xFFDA70D6); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:238 G:232 B:170 A:255.
        /// </summary>
        public static Color PaleGoldenrod { get { return Color.FromArgb(0xFFEEE8AA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:152 G:251 B:152 A:255.
        /// </summary>
        public static Color PaleGreen { get { return Color.FromArgb(0xFF98FB98); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:175 G:238 B:238 A:255.
        /// </summary>
        public static Color PaleTurquoise { get { return Color.FromArgb(0xFFAFEEEE); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:219 G:112 B:147 A:255.
        /// </summary>
        public static Color PaleVioletRed { get { return Color.FromArgb(0xFFDB7093); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:239 B:213 A:255.
        /// </summary>
        public static Color PapayaWhip { get { return Color.FromArgb(0xFFFFEFD5); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:218 B:185 A:255.
        /// </summary>
        public static Color PeachPuff { get { return Color.FromArgb(0xFFFFDAB9); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:205 G:133 B:63 A:255.
        /// </summary>
        public static Color Peru { get { return Color.FromArgb(0xFFCD853F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:192 B:203 A:255.
        /// </summary>
        public static Color Pink { get { return Color.FromArgb(0xFFFFC0CB); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:221 G:160 B:221 A:255.
        /// </summary>
        public static Color Plum { get { return Color.FromArgb(0xFFDDA0DD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:176 G:224 B:230 A:255.
        /// </summary>
        public static Color PowderBlue { get { return Color.FromArgb(0xFFB0E0E6); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:0 B:128 A:255.
        /// </summary>
        public static Color Purple { get { return Color.FromArgb(0xFF800080); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:0 A:255.
        /// </summary>
        public static Color Red { get { return Color.FromArgb(0xFFFF0000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:188 G:143 B:143 A:255.
        /// </summary>
        public static Color RosyBrown { get { return Color.FromArgb(0xFFBC8F8F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:65 G:105 B:225 A:255.
        /// </summary>
        public static Color RoyalBlue { get { return Color.FromArgb(0xFF4169E1); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:69 B:19 A:255.
        /// </summary>
        public static Color SaddleBrown { get { return Color.FromArgb(0xFF8B4513); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:128 B:114 A:255.
        /// </summary>
        public static Color Salmon { get { return Color.FromArgb(0xFFFA8072); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:244 G:164 B:96 A:255.
        /// </summary>
        public static Color SandyBrown { get { return Color.FromArgb(0xFFF4A460); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:46 G:139 B:87 A:255.
        /// </summary>
        public static Color SeaGreen { get { return Color.FromArgb(0xFF2E8B57); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:245 B:238 A:255.
        /// </summary>
        public static Color SeaShell { get { return Color.FromArgb(0xFFFFF5EE); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:160 G:82 B:45 A:255.
        /// </summary>
        public static Color Sienna { get { return Color.FromArgb(0xFFA0522D); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:192 G:192 B:192 A:255.
        /// </summary>
        public static Color Silver { get { return Color.FromArgb(0xFFC0C0C0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:135 G:206 B:235 A:255.
        /// </summary>
        public static Color SkyBlue { get { return Color.FromArgb(0xFF87CEEB); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:106 G:90 B:205 A:255.
        /// </summary>
        public static Color SlateBlue { get { return Color.FromArgb(0xFF6A5ACD); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:112 G:128 B:144 A:255.
        /// </summary>
        public static Color SlateGray { get { return Color.FromArgb(0xFF708090); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:250 A:255.
        /// </summary>
        public static Color Snow { get { return Color.FromArgb(0xFFFFFAFA); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:127 A:255.
        /// </summary>
        public static Color SpringGreen { get { return Color.FromArgb(0xFF00FF7F); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:70 G:130 B:180 A:255.
        /// </summary>
        public static Color SteelBlue { get { return Color.FromArgb(0xFF4682B4); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:210 G:180 B:140 A:255.
        /// </summary>
        public static Color Tan { get { return Color.FromArgb(0xFFD2B48C); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:128 B:128 A:255.
        /// </summary>
        public static Color Teal { get { return Color.FromArgb(0xFF008080); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:216 G:191 B:216 A:255.
        /// </summary>
        public static Color Thistle { get { return Color.FromArgb(0xFFD8BFD8); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:99 B:71 A:255.
        /// </summary>
        public static Color Tomato { get { return Color.FromArgb(0xFFFF6347); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:0.
        /// </summary>
        public static Color Transparent { get { return Color.FromArgb(0x00000000); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:64 G:224 B:208 A:255.
        /// </summary>
        public static Color Turquoise { get { return Color.FromArgb(0xFF40E0D0); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:238 G:130 B:238 A:255.
        /// </summary>
        public static Color Violet { get { return Color.FromArgb(0xFFEE82EE); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:222 B:179 A:255.
        /// </summary>
        public static Color Wheat { get { return Color.FromArgb(0xFFF5DEB3); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:255 A:255.
        /// </summary>
        public static Color White { get { return Color.FromArgb(0xFFFFFFFF); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:245 B:245 A:255.
        /// </summary>
        public static Color WhiteSmoke { get { return Color.FromArgb(0xFFF5F5F5); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:0 A:255.
        /// </summary>
        public static Color Yellow { get { return Color.FromArgb(0xFFFFFF00); } }

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:154 G:205 B:50 A:255.
        /// </summary>
        public static Color YellowGreen { get { return Color.FromArgb(0xFF9ACD32); } }

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
