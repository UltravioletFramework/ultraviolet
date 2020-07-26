using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an RGBA color.
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(UltravioletJsonConverter))]
    public partial struct Color : IEquatable<Color>, IInterpolatable<Color>
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
        /// Initializes a new instance of the <see cref="Color"/> structure using the x, y, and z components
        /// of the specified vector as normalized red, green, and blue values.
        /// </summary>
        /// <param name="vector">The vector from which to create the color.</param>
        public Color(Vector3 vector)
            : this(vector.X, vector.Y, vector.Z)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure using the x, y, z, and w components
        /// of the specified vector as normalized red, green, blue, and alpha values.
        /// </summary>
        /// <param name="vector">The vector from which to create the color.</param>
        public Color(Vector4 vector)
            : this(vector.X, vector.Y, vector.Z, vector.W)
        {

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
        /// Converts a normalized color channel from sRGB to linear.
        /// </summary>
        /// <param name="x">The normalized color channel value to convert.</param>
        /// <returns>The converted color channel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ConvertSrgbColorChannelToLinear(Byte x)
        {
            return (Byte)(255f * ConvertSrgbColorChannelToLinear(x / 255f));
        }

        /// <summary>
        /// Converts a color channel from linear to sRGB.
        /// </summary>
        /// <param name="x">The color channel value to convert.</param>
        /// <returns>The converted color channel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ConvertLinearColorChannelToSrgb(Byte x)
        {
            return (Byte)(255f * ConvertLinearColorChannelToSrgb(x / 255f));
        }

        /// <summary>
        /// Converts a normalized color channel from sRGB to linear.
        /// </summary>
        /// <param name="c">The normalized color channel value to convert.</param>
        /// <returns>The converted color channel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ConvertSrgbColorChannelToLinear(Single c)
        {
            return (c < 0.04045f) ? c / 12.92f : (Single)Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        /// <summary>
        /// Converts a normalized color channel from linear to sRGB.
        /// </summary>
        /// <param name="c">The normalized color channel value to convert.</param>
        /// <returns>The converted color channel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ConvertLinearColorChannelToSrgb(Single c)
        {
            return (c < 0.0031308f) ? c * 12.92f : 1.055f * (Single)Math.Pow(c, 1.0 / 2.4) - 0.055f;
        }

        /// <summary>
        /// Converts an sRGB color value to a linear color value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ConvertSrgbColorToLinear(Color c)
        {
            var r = ConvertSrgbColorChannelToLinear(c.R / 255.0f);
            var g = ConvertSrgbColorChannelToLinear(c.G / 255.0f);
            var b = ConvertSrgbColorChannelToLinear(c.B / 255.0f);
            var a = c.A / 255.0f;
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Converts a linear color value to an sRGB color value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ConvertLinearColorToSrgb(Color c)
        {
            var r = ConvertLinearColorChannelToSrgb(c.R / 255.0f);
            var g = ConvertLinearColorChannelToSrgb(c.G / 255.0f);
            var b = ConvertLinearColorChannelToSrgb(c.B / 255.0f);
            var a = c.A / 255.0f;
            return new Color(r, g, b, a);
        }

        /// <inheritdoc/>
        public override String ToString() => $"#{A:x2}{R:x2}{G:x2}{B:x2}";

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

        // Associates the names of colors with their values.
        private static readonly Dictionary<String, Color> NamedColorRegistry =
            new Dictionary<String, Color>(StringComparer.Ordinal);

        // The packed value of the color in RGBA format.
        private readonly UInt32 packedValue;
    }
}
