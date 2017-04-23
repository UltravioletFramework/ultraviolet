using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents an RGBA color.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{A:{A} R:{R} G:{G} B:{B}\}")]
    [JsonConverter(typeof(UltravioletJsonConverter))]
    public struct Color : IEquatable<Color>, IInterpolatable<Color>
    {
        /// <summary>
        /// Initializes the <see cref="Color"/> type.
        /// </summary>
        static Color()
        {
            var namedColorProperties = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.PropertyType == typeof(Color));

            foreach (var namedColorProperty in namedColorProperties)
            {
                var name = namedColorProperty.Name;
                var value = (Color)namedColorProperty.GetValue(null, null);

                NamedColorRegistry[name] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="packedValue">The color's packed value in ABGR format.</param>
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        [Preserve]
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
        /// Initializes a new instance of the <see cref="Color"/> structure using the x, y, and z components
        /// of the specified vector as normalized red, green, and blue values.
        /// </summary>
        /// <param name="vector">The vector from which to create the color.</param>
        [Preserve]
        public Color(Vector3 vector)
            : this(vector.X, vector.Y, vector.Z)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure using the x, y, z, and w components
        /// of the specified vector as normalized red, green, blue, and alpha values.
        /// </summary>
        /// <param name="vector">The vector from which to create the color.</param>
        [Preserve]
        public Color(Vector4 vector)
            : this(vector.X, vector.Y, vector.Z, vector.W)
        {

        }

        /// <summary>
        /// Compares two colors for equality.
        /// </summary>
        /// <param name="c1">The first <see cref="Color"/> to compare.</param>
        /// <param name="c2">The second <see cref="Color"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified colors are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Color c1, Color c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Compares two colors for inequality.
        /// </summary>
        /// <param name="c1">The first <see cref="Color"/> to compare.</param>
        /// <param name="c2">The second <see cref="Color"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified colors are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
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
        [Preserve]
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
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Color color)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out color);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <returns>A instance of the <see cref="Color"/> structure equivalent to the color contained in <paramref name="s"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Color color)
        {
            color = default(Color);

            if (String.IsNullOrEmpty(s))
                return false;

            var trimmed = s.Trim();
            if (trimmed.StartsWith("#"))
            {
                switch (trimmed.Length)
                {
                    case 4:
                    case 5: 
                    case 7: 
                    case 9:
                        return TryParseHex(trimmed, style, provider, ref color);

                    default:
                        return false;
                }
            }
            else
            {
                return TryParseDelimitedOrNamed(trimmed, style, provider, ref color);
            }
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Color"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Color"/> structure equivalent to the color contained in <paramref name="s"/>.</returns>
        [Preserve]
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
            var r = (byte)(value >> 24);
            var g = (byte)(value >> 16);
            var b = (byte)(value >> 8);
            var a = (byte)(value);

            return new Color((uint)((r) | (g << 8) | (b << 16) | (a << 24)));
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from a 32-bit integer in BGRA format.
        /// </summary>
        /// <param name="value">The integer from which to create a color.</param>
        /// <returns>The <see cref="Color"/> that corresponds to the specified integer value.</returns>
        [CLSCompliant(false)]
        public static Color FromBgra(UInt32 value)
        {
            var b = (byte)(value >> 24);
            var g = (byte)(value >> 16);
            var r = (byte)(value >> 8);
            var a = (byte)(value);

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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
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
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
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
            return (uint)((R << 24) | (G << 16) | (B << 8) | (A));
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a 32-bit integer in BGRA format.
        /// </summary>
        /// <returns>The BGRA value that corresponds to this <see cref="Color"/>.</returns>
        [CLSCompliant(false)]
        public UInt32 ToBgra()
        {
            return (uint)((B << 24) | (G << 16) | (R << 8) | (A));
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a normalized 3-vector containing red, green, and blue components.
        /// </summary>
        /// <returns>The <see cref="Vector3"/> that corresponds to this <see cref="Color"/>.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(
                R / (float)Byte.MaxValue, 
                G / (float)Byte.MaxValue, 
                B / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Converts the <see cref="Color"/> to a normalized 4-vector containing red, green, blue, and alpha components.
        /// </summary>
        /// <returns>The <see cref="Vector4"/> that corresponds to this <see cref="Color"/>.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(
                R / (float)Byte.MaxValue, 
                G / (float)Byte.MaxValue, 
                B / (float)Byte.MaxValue, 
                A / (float)Byte.MaxValue);
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
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
        [Preserve]
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
        [Preserve]
        public static Color AliceBlue => FromArgb(0xFFF0F8FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:235 B:215 A:255.
        /// </summary>
        [Preserve]
        public static Color AntiqueWhite => FromArgb(0xFFFAEBD7);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Aqua => FromArgb(0xFF00FFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:127 G:255 B:212 A:255.
        /// </summary>
        [Preserve]
        public static Color Aquamarine => FromArgb(0xFF7FFFD4);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:255 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Azure => FromArgb(0xFFF0FFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:245 B:220 A:255.
        /// </summary>
        [Preserve]
        public static Color Beige => FromArgb(0xFFF5F5DC);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:196 A:255.
        /// </summary>
        [Preserve]
        public static Color Bisque => FromArgb(0xFFFFE4C4);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Black => FromArgb(0xFF000000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:235 B:205 A:255.
        /// </summary>
        [Preserve]
        public static Color BlanchedAlmond => FromArgb(0xFFFFEBCD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Blue => FromArgb(0xFF0000FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:138 G:43 B:226 A:255.
        /// </summary>
        [Preserve]
        public static Color BlueViolet => FromArgb(0xFF8A2BE2);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:165 G:42 B:42 A:255.
        /// </summary>
        [Preserve]
        public static Color Brown => FromArgb(0xFFA52A2A);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:222 G:184 B:135 A:255.
        /// </summary>
        [Preserve]
        public static Color BurlyWood => FromArgb(0xFFDEB887);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:95 G:158 B:160 A:255.
        /// </summary>
        [Preserve]
        public static Color CadetBlue => FromArgb(0xFF5F9EA0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:127 G:255 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Chartreuse => FromArgb(0xFF7FFF00);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:210 G:105 B:30 A:255.
        /// </summary>
        [Preserve]
        public static Color Chocolate => FromArgb(0xFFD2691E);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:127 B:80 A:255.
        /// </summary>
        [Preserve]
        public static Color Coral => FromArgb(0xFFFF7F50);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:100 G:149 B:237 A:255.
        /// </summary>
        [Preserve]
        public static Color CornflowerBlue => FromArgb(0xFF6495ED);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:248 B:220 A:255.
        /// </summary>
        [Preserve]
        public static Color Cornsilk => FromArgb(0xFFFFF8DC);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:220 G:20 B:60 A:255.
        /// </summary>
        [Preserve]
        public static Color Crimson => FromArgb(0xFFDC143C);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Cyan => FromArgb(0xFF00FFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:139 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkBlue => FromArgb(0xFF00008B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:139 B:139 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkCyan => FromArgb(0xFF008B8B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:184 G:134 B:11 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkGoldenrod => FromArgb(0xFFB8860B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:169 G:169 B:169 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkGray => FromArgb(0xFFA9A9A9);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:100 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkGreen => FromArgb(0xFF006400);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:189 G:183 B:107 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkKhaki => FromArgb(0xFFBDB76B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:0 B:139 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkMagenta => FromArgb(0xFF8B008B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:85 G:107 B:47 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkOliveGreen => FromArgb(0xFF556B2F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:140 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkOrange => FromArgb(0xFFFF8C00);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:153 G:50 B:204 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkOrchid => FromArgb(0xFF9932CC);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:0 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkRed => FromArgb(0xFF8B0000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:233 G:150 B:122 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkSalmon => FromArgb(0xFFE9967A);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:143 G:188 B:139 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkSeaGreen => FromArgb(0xFF8FBC8B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:72 G:61 B:139 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkSlateBlue => FromArgb(0xFF483D8B);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:47 G:79 B:79 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkSlateGray => FromArgb(0xFF2F4F4F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:206 B:209 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkTurquoise => FromArgb(0xFF00CED1);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:148 G:0 B:211 A:255.
        /// </summary>
        [Preserve]
        public static Color DarkViolet => FromArgb(0xFF9400D3);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:20 B:147 A:255.
        /// </summary>
        [Preserve]
        public static Color DeepPink => FromArgb(0xFFFF1493);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:191 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color DeepSkyBlue => FromArgb(0xFF00BFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:105 G:105 B:105 A:255.
        /// </summary>
        [Preserve]
        public static Color DimGray => FromArgb(0xFF696969);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:30 G:144 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color DodgerBlue => FromArgb(0xFF1E90FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:178 G:34 B:34 A:255.
        /// </summary>
        [Preserve]
        public static Color Firebrick => FromArgb(0xFFB22222);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:240 A:255.
        /// </summary>
        [Preserve]
        public static Color FloralWhite => FromArgb(0xFFFFFAF0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:34 G:139 B:34 A:255.
        /// </summary>
        [Preserve]
        public static Color ForestGreen => FromArgb(0xFF228B22);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Fuchsia => FromArgb(0xFFFF00FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:220 G:220 B:220 A:255.
        /// </summary>
        [Preserve]
        public static Color Gainsboro => FromArgb(0xFFDCDCDC);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:248 G:248 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color GhostWhite => FromArgb(0xFFF8F8FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:215 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Gold => FromArgb(0xFFFFD700);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:218 G:165 B:32 A:255.
        /// </summary>
        [Preserve]
        public static Color Goldenrod => FromArgb(0xFFDAA520);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:128 B:128 A:255.
        /// </summary>
        [Preserve]
        public static Color Gray => FromArgb(0xFF808080);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:128 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Green => FromArgb(0xFF008000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:173 G:255 B:47 A:255.
        /// </summary>
        [Preserve]
        public static Color GreenYellow => FromArgb(0xFFADFF2F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:255 B:240 A:255.
        /// </summary>
        [Preserve]
        public static Color Honeydew => FromArgb(0xFFF0FFF0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:105 B:180 A:255.
        /// </summary>
        [Preserve]
        public static Color HotPink => FromArgb(0xFFFF69B4);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:205 G:92 B:92 A:255.
        /// </summary>
        [Preserve]
        public static Color IndianRed => FromArgb(0xFFCD5C5C);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:75 G:0 B:130 A:255.
        /// </summary>
        [Preserve]
        public static Color Indigo => FromArgb(0xFF4B0082);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:240 A:255.
        /// </summary>
        [Preserve]
        public static Color Ivory => FromArgb(0xFFFFFFF0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:230 B:140 A:255.
        /// </summary>
        [Preserve]
        public static Color Khaki => FromArgb(0xFFF0E68C);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:230 G:230 B:250 A:255.
        /// </summary>
        [Preserve]
        public static Color Lavender => FromArgb(0xFFE6E6FA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:240 B:245 A:255.
        /// </summary>
        [Preserve]
        public static Color LavenderBlush => FromArgb(0xFFFFF0F5);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:124 G:252 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color LawnGreen => FromArgb(0xFF7CFC00);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:205 A:255.
        /// </summary>
        [Preserve]
        public static Color LemonChiffon => FromArgb(0xFFFFFACD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:173 G:216 B:230 A:255.
        /// </summary>
        [Preserve]
        public static Color LightBlue => FromArgb(0xFFADD8E6);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:240 G:128 B:128 A:255.
        /// </summary>
        [Preserve]
        public static Color LightCoral => FromArgb(0xFFF08080);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:224 G:255 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color LightCyan => FromArgb(0xFFE0FFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:250 B:210 A:255.
        /// </summary>
        [Preserve]
        public static Color LightGoldenrodYellow => FromArgb(0xFFFAFAD2);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:211 G:211 B:211 A:255.
        /// </summary>
        [Preserve]
        public static Color LightGray => FromArgb(0xFFD3D3D3);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:144 G:238 B:144 A:255.
        /// </summary>
        [Preserve]
        public static Color LightGreen => FromArgb(0xFF90EE90);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:182 B:193 A:255.
        /// </summary>
        [Preserve]
        public static Color LightPink => FromArgb(0xFFFFB6C1);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:160 B:122 A:255.
        /// </summary>
        [Preserve]
        public static Color LightSalmon => FromArgb(0xFFFFA07A);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:32 G:178 B:170 A:255.
        /// </summary>
        [Preserve]
        public static Color LightSeaGreen => FromArgb(0xFF20B2AA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:135 G:206 B:250 A:255.
        /// </summary>
        [Preserve]
        public static Color LightSkyBlue => FromArgb(0xFF87CEFA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:119 G:136 B:153 A:255.
        /// </summary>
        [Preserve]
        public static Color LightSlateGray => FromArgb(0xFF778899);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:176 G:196 B:222 A:255.
        /// </summary>
        [Preserve]
        public static Color LightSteelBlue => FromArgb(0xFFB0C4DE);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:224 A:255.
        /// </summary>
        [Preserve]
        public static Color LightYellow => FromArgb(0xFFFFFFE0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Lime => FromArgb(0xFF00FF00);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:50 G:205 B:50 A:255.
        /// </summary>
        [Preserve]
        public static Color LimeGreen => FromArgb(0xFF32CD32);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:240 B:230 A:255.
        /// </summary>
        [Preserve]
        public static Color Linen => FromArgb(0xFFFAF0E6);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color Magenta => FromArgb(0xFFFF00FF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:0 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Maroon => FromArgb(0xFF800000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:102 G:205 B:170 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumAquamarine => FromArgb(0xFF66CDAA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:205 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumBlue => FromArgb(0xFF0000CD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:186 G:85 B:211 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumOrchid => FromArgb(0xFFBA55D3);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:147 G:112 B:219 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumPurple => FromArgb(0xFF9370DB);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:60 G:179 B:113 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumSeaGreen => FromArgb(0xFF3CB371);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:123 G:104 B:238 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumSlateBlue => FromArgb(0xFF7B68EE);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:250 B:154 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumSpringGreen => FromArgb(0xFF00FA9A);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:72 G:209 B:204 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumTurquoise => FromArgb(0xFF48D1CC);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:199 G:21 B:133 A:255.
        /// </summary>
        [Preserve]
        public static Color MediumVioletRed => FromArgb(0xFFC71585);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:25 G:25 B:112 A:255.
        /// </summary>
        [Preserve]
        public static Color MidnightBlue => FromArgb(0xFF191970);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:255 B:250 A:255.
        /// </summary>
        [Preserve]
        public static Color MintCream => FromArgb(0xFFF5FFFA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:225 A:255.
        /// </summary>
        [Preserve]
        public static Color MistyRose => FromArgb(0xFFFFE4E1);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:228 B:181 A:255.
        /// </summary>
        [Preserve]
        public static Color Moccasin => FromArgb(0xFFFFE4B5);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:222 B:173 A:255.
        /// </summary>
        [Preserve]
        public static Color NavajoWhite => FromArgb(0xFFFFDEAD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:128 A:255.
        /// </summary>
        [Preserve]
        public static Color Navy => FromArgb(0xFF000080);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:253 G:245 B:230 A:255.
        /// </summary>
        [Preserve]
        public static Color OldLace => FromArgb(0xFFFDF5E6);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:128 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Olive => FromArgb(0xFF808000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:107 G:142 B:35 A:255.
        /// </summary>
        [Preserve]
        public static Color OliveDrab => FromArgb(0xFF6B8E23);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:165 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Orange => FromArgb(0xFFFFA500);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:69 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color OrangeRed => FromArgb(0xFFFF4500);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:218 G:112 B:214 A:255.
        /// </summary>
        [Preserve]
        public static Color Orchid => FromArgb(0xFFDA70D6);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:238 G:232 B:170 A:255.
        /// </summary>
        [Preserve]
        public static Color PaleGoldenrod => FromArgb(0xFFEEE8AA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:152 G:251 B:152 A:255.
        /// </summary>
        [Preserve]
        public static Color PaleGreen => FromArgb(0xFF98FB98);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:175 G:238 B:238 A:255.
        /// </summary>
        [Preserve]
        public static Color PaleTurquoise => FromArgb(0xFFAFEEEE);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:219 G:112 B:147 A:255.
        /// </summary>
        [Preserve]
        public static Color PaleVioletRed => FromArgb(0xFFDB7093);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:239 B:213 A:255.
        /// </summary>
        [Preserve]
        public static Color PapayaWhip => FromArgb(0xFFFFEFD5);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:218 B:185 A:255.
        /// </summary>
        [Preserve]
        public static Color PeachPuff => FromArgb(0xFFFFDAB9);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:205 G:133 B:63 A:255.
        /// </summary>
        [Preserve]
        public static Color Peru => FromArgb(0xFFCD853F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:192 B:203 A:255.
        /// </summary>
        [Preserve]
        public static Color Pink => FromArgb(0xFFFFC0CB);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:221 G:160 B:221 A:255.
        /// </summary>
        [Preserve]
        public static Color Plum => FromArgb(0xFFDDA0DD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:176 G:224 B:230 A:255.
        /// </summary>
        [Preserve]
        public static Color PowderBlue => FromArgb(0xFFB0E0E6);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:128 G:0 B:128 A:255.
        /// </summary>
        [Preserve]
        public static Color Purple => FromArgb(0xFF800080);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:0 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Red => FromArgb(0xFFFF0000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:188 G:143 B:143 A:255.
        /// </summary>
        [Preserve]
        public static Color RosyBrown => FromArgb(0xFFBC8F8F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:65 G:105 B:225 A:255.
        /// </summary>
        [Preserve]
        public static Color RoyalBlue => FromArgb(0xFF4169E1);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:139 G:69 B:19 A:255.
        /// </summary>
        [Preserve]
        public static Color SaddleBrown => FromArgb(0xFF8B4513);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:250 G:128 B:114 A:255.
        /// </summary>
        [Preserve]
        public static Color Salmon => FromArgb(0xFFFA8072);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:244 G:164 B:96 A:255.
        /// </summary>
        [Preserve]
        public static Color SandyBrown => FromArgb(0xFFF4A460);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:46 G:139 B:87 A:255.
        /// </summary>
        [Preserve]
        public static Color SeaGreen => FromArgb(0xFF2E8B57);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:245 B:238 A:255.
        /// </summary>
        [Preserve]
        public static Color SeaShell => FromArgb(0xFFFFF5EE);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:160 G:82 B:45 A:255.
        /// </summary>
        [Preserve]
        public static Color Sienna => FromArgb(0xFFA0522D);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:192 G:192 B:192 A:255.
        /// </summary>
        [Preserve]
        public static Color Silver => FromArgb(0xFFC0C0C0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:135 G:206 B:235 A:255.
        /// </summary>
        [Preserve]
        public static Color SkyBlue => FromArgb(0xFF87CEEB);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:106 G:90 B:205 A:255.
        /// </summary>
        [Preserve]
        public static Color SlateBlue => FromArgb(0xFF6A5ACD);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:112 G:128 B:144 A:255.
        /// </summary>
        [Preserve]
        public static Color SlateGray => FromArgb(0xFF708090);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:250 B:250 A:255.
        /// </summary>
        [Preserve]
        public static Color Snow => FromArgb(0xFFFFFAFA);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:255 B:127 A:255.
        /// </summary>
        [Preserve]
        public static Color SpringGreen => FromArgb(0xFF00FF7F);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:70 G:130 B:180 A:255.
        /// </summary>
        [Preserve]
        public static Color SteelBlue => FromArgb(0xFF4682B4);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:210 G:180 B:140 A:255.
        /// </summary>
        [Preserve]
        public static Color Tan => FromArgb(0xFFD2B48C);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:128 B:128 A:255.
        /// </summary>
        [Preserve]
        public static Color Teal => FromArgb(0xFF008080);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:216 G:191 B:216 A:255.
        /// </summary>
        [Preserve]
        public static Color Thistle => FromArgb(0xFFD8BFD8);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:99 B:71 A:255.
        /// </summary>
        [Preserve]
        public static Color Tomato => FromArgb(0xFFFF6347);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:0 G:0 B:0 A:0.
        /// </summary>
        [Preserve]
        public static Color Transparent => FromArgb(0x00000000);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:64 G:224 B:208 A:255.
        /// </summary>
        [Preserve]
        public static Color Turquoise => FromArgb(0xFF40E0D0);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:238 G:130 B:238 A:255.
        /// </summary>
        [Preserve]
        public static Color Violet => FromArgb(0xFFEE82EE);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:222 B:179 A:255.
        /// </summary>
        [Preserve]
        public static Color Wheat => FromArgb(0xFFF5DEB3);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:255 A:255.
        /// </summary>
        [Preserve]
        public static Color White => FromArgb(0xFFFFFFFF);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:245 G:245 B:245 A:255.
        /// </summary>
        [Preserve]
        public static Color WhiteSmoke => FromArgb(0xFFF5F5F5);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:255 G:255 B:0 A:255.
        /// </summary>
        [Preserve]
        public static Color Yellow => FromArgb(0xFFFFFF00);

        /// <summary>
        /// Gets the system-defined <see cref="Color"/> with the value R:154 G:205 B:50 A:255.
        /// </summary>
        [Preserve]
        public static Color YellowGreen => FromArgb(0xFF9ACD32);

        /// <summary>
        /// Converts the string representation of a color into an instance of the <see cref="Color"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a color to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="color">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Strings parsed by this method must be in one of the following formats:
        /// <list type="bullet">
        /// <item><description>#rgb</description></item>
        /// <item><description>#argb</description></item>
        /// <item><description>#rrggbb</description></item>
        /// <item><description>#aarrggbb</description></item>
        /// </list>
        /// </remarks>
        [Preserve]
        private static Boolean TryParseHex(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            var length = s.Length;
            var isShortForm = (length == 4 || length == 5);
            var isAlphaAvailable = (length == 5 || length == 9);

            var substrPos = 1;
            var substrLen = isShortForm ? 1 : 2;

            var astr = isShortForm ? "F" : "FF";
            if (isAlphaAvailable)
            {
                astr = s.Substring(substrPos, substrLen);
                substrPos += substrLen;
            }

            var rstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;

            var gstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;

            var bstr = s.Substring(substrPos, substrLen);
            substrPos += substrLen;
            
            Int32 a, r, g, b;
            if (!Int32.TryParse(astr, NumberStyles.AllowHexSpecifier, provider, out a))
                return false;
            if (!Int32.TryParse(rstr, NumberStyles.AllowHexSpecifier, provider, out r))
                return false;
            if (!Int32.TryParse(gstr, NumberStyles.AllowHexSpecifier, provider, out g))
                return false;
            if (!Int32.TryParse(bstr, NumberStyles.AllowHexSpecifier, provider, out b))
                return false;

            if (isShortForm)
            {
                a = (16 * a) + a;
                r = (16 * r) + r;
                g = (16 * g) + g;
                b = (16 * b) + b;
            }

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
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Strings parsed by this methods must be comma-delimited lists of three or four color components,
        /// in either R, G, B or A, R, G, B format, or a named color.</remarks>
        [Preserve]
        private static Boolean TryParseDelimitedOrNamed(String s, NumberStyles style, IFormatProvider provider, ref Color color)
        {
            var components = s.Split(',');
            if (components.Length == 1)
            {
                return NamedColorRegistry.TryGetValue(components[0], out color);
            }
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

        // Associates the names of colors with their values.
        private static readonly Dictionary<String, Color> NamedColorRegistry =
            new Dictionary<String, Color>(StringComparer.Ordinal);

        // The packed value of the color in RGBA format.
        private readonly UInt32 packedValue;
    }
}
