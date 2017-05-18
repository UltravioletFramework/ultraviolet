using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a four-dimensional vector.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Z:{Z} W:{W}\}")]
    public struct Vector4 : IEquatable<Vector4>, IInterpolatable<Vector4>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        [Preserve]
        public Vector4(Single value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
            this.W = value;
        }

        /// <summary>
        /// Initializes a new instance of the Vector4 structure with the specified x, y, z, and w components.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        /// <param name="z">The vector's z component.</param>
        /// <param name="w">The vector's w component.</param>
        [Preserve]
        [JsonConstructor]
        public Vector4(Single x, Single y, Single z, Single w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with its x and y components set to the 
        /// x and y components of the specified vector, and its z and w components set to the specified values.
        /// </summary>
        /// <param name="vector">The vector from which to set the vector's x and y components.</param>
        /// <param name="z">The vector's z component.</param>
        /// <param name="w">The vector's w component.</param>
        [Preserve]
        public Vector4(Vector2 vector, Single z, Single w)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure with its x, y, and z components set to the 
        /// x, y, and z components of the specified vector, and its w component set to the specified values.
        /// </summary>
        /// <param name="vector">The vector from which to set the vector's x, y, and z components.</param>
        /// <param name="w">The vector's w component.</param>
        [Preserve]
        public Vector4(Vector3 vector, Single w)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = w;
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector4"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified vectors are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Vector4 vector1, Vector4 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector4"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified vectors are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Vector4 vector1, Vector4 vector2)
        {
            return !vector1.Equals(vector2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator +(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z, vector1.W + vector2.W);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator -(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z, vector1.W - vector2.W);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator *(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X * vector2.X, 
                vector1.Y * vector2.Y, 
                vector1.Z * vector2.Z, 
                vector1.W * vector2.W
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator *(Single factor, Vector4 vector)
        {
            return new Vector4(
                vector.X * factor,
                vector.Y * factor,
                vector.Z * factor,
                vector.W * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator *(Vector4 vector, Single factor)
        {
            return new Vector4(
                vector.X * factor, 
                vector.Y * factor, 
                vector.Z * factor, 
                vector.W * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator /(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X / vector2.X,
                vector1.Y / vector2.Y,
                vector1.Z / vector2.Z,
                vector1.W / vector2.W
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator /(Vector4 vector, Single factor)
        {
            return new Vector4(
                vector.X / factor,
                vector.Y / factor,
                vector.Z / factor,
                vector.W / factor
            );
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector4"/>.</returns>
        [Preserve]
        public static Vector4 operator -(Vector4 vector)
        {
            return new Vector4(-vector.X, -vector.Y, -vector.Z, -vector.W);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector4"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Vector4 vector)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out vector);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector4"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <returns>A instance of the <see cref="Vector4"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Vector4 Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector4"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Vector4 vector)
        {
            vector = default(Vector4);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 4)
                return false;

            Single x, y, z, w;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out z))
                return false;
            if (!Single.TryParse(components[3], style, provider, out w))
                return false;

            vector = new Vector4(x, y, z, w);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector4"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Vector4"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Vector4 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Vector4 vector;
            if (!TryParse(s, style, provider, out vector))
                throw new FormatException();
            return vector;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector4 vector1, Vector4 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Add(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Add(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result = new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Subtract(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector4"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector4"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Subtract(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result = new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Multiply(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X * vector2.X,
                vector1.Y * vector2.Y,
                vector1.Z * vector2.Z,
                vector1.W * vector2.W
            );
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Multiply(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result = new Vector4(
                vector1.X * vector2.X,
                vector1.Y * vector2.Y,
                vector1.Z * vector2.Z,
                vector1.W * vector2.W
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Multiply(Vector4 vector, Single factor)
        {
            return new Vector4(
                vector.X * factor,
                vector.Y * factor,
                vector.Z * factor,
                vector.W * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Multiply(ref Vector4 vector, Single factor, out Vector4 result)
        {
            result = new Vector4(
                vector.X * factor,
                vector.Y * factor,
                vector.Z * factor,
                vector.W * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Divide(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X / vector2.X,
                vector1.Y / vector2.Y,
                vector1.Z / vector2.Z,
                vector1.W / vector2.W
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector4"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector4"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Divide(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result = new Vector4(
                vector1.X / vector2.X,
                vector1.Y / vector2.Y,
                vector1.Z / vector2.Z,
                vector1.W / vector2.W
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        public static Vector4 Divide(Vector4 vector, Single factor)
        {
            return new Vector4(
                vector.X / factor,
                vector.Y / factor,
                vector.Z / factor,
                vector.W / factor
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector4"/>.</param>
        public static void Divide(ref Vector4 vector, Single factor, out Vector4 result)
        {
            result = new Vector4(
                vector.X / factor,
                vector.Y / factor,
                vector.Z / factor,
                vector.W / factor
            );
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        public static Vector4 Transform(Vector4 vector, Matrix matrix)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z + matrix.M14 * vector.W);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z + matrix.M24 * vector.W);
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z + matrix.M34 * vector.W);
            var w = (matrix.M41 * vector.X + matrix.M42 * vector.Y + matrix.M43 * vector.Z + matrix.M44 * vector.W);
            return new Vector4(x, y, z, w);
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector4"/>.</param>
        public static void Transform(ref Vector4 vector, ref Matrix matrix, out Vector4 result)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z + matrix.M14 * vector.W);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z + matrix.M24 * vector.W);
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z + matrix.M34 * vector.W);
            var w = (matrix.M41 * vector.X + matrix.M42 * vector.Y + matrix.M43 * vector.Z + matrix.M44 * vector.W);
            result = new Vector4(x, y, z, w);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector4"/>.</returns>
        public static Vector4 Normalize(Vector4 vector)
        {
            var inverseMagnitude = 1f / vector.Length();
            return new Vector4(vector.X * inverseMagnitude, vector.Y * inverseMagnitude, vector.Z * inverseMagnitude, vector.W * inverseMagnitude);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector4"/>.</param>
        public static void Normalize(ref Vector4 vector, out Vector4 result)
        {
            var inverseMagnitude = 1f / vector.Length();
            result = new Vector4(vector.X * inverseMagnitude, vector.Y * inverseMagnitude, vector.Z * inverseMagnitude, vector.W * inverseMagnitude);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to negate.</param>
        /// <returns>The negated <see cref="Vector4"/>.</returns>
        public static Vector4 Negate(Vector4 vector)
        {
            return new Vector4(-vector.X, -vector.Y, -vector.Z, -vector.W);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector4"/>.</param>
        public static void Negate(ref Vector4 vector, out Vector4 result)
        {
            result = new Vector4(-vector.X, -vector.Y, -vector.Z, -vector.W);
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector4"/>.</returns>
        public static Vector4 Clamp(Vector4 vector, Vector4 min, Vector4 max)
        {
            return new Vector4(
                vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X,
                vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y,
                vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z,
                vector.W < min.W ? min.W : vector.W > max.W ? max.W : vector.W
            );
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector4"/>.</param>
        public static void Clamp(ref Vector4 vector, ref Vector4 min, ref Vector4 max, out Vector4 result)
        {
            result = new Vector4(
                vector.X < min.X ? min.X : vector.X > max.X ? max.X : vector.X,
                vector.Y < min.Y ? min.Y : vector.Y > max.Y ? max.Y : vector.Y,
                vector.Z < min.Z ? min.Z : vector.Z > max.Z ? max.Z : vector.Z,
                vector.W < min.W ? min.W : vector.W > max.W ? max.W : vector.W
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>A <see cref="Vector4"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector4 Min(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X < vector2.X ? vector1.X : vector2.X,
                vector1.Y < vector2.Y ? vector1.Y : vector2.Y,
                vector1.Z < vector2.Z ? vector1.Z : vector2.Z,
                vector1.W < vector2.W ? vector1.W : vector2.W
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">A <see cref="Vector4"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result = new Vector4(
                vector1.X < vector2.X ? vector1.X : vector2.X,
                vector1.Y < vector2.Y ? vector1.Y : vector2.Y,
                vector1.Z < vector2.Z ? vector1.Z : vector2.Z,
                vector1.W < vector2.W ? vector1.W : vector2.W
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <returns>A <see cref="Vector4"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector4 Max(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(
                vector1.X < vector2.X ? vector2.X : vector1.X,
                vector1.Y < vector2.Y ? vector2.Y : vector1.Y,
                vector1.Z < vector2.Z ? vector2.Z : vector1.Z,
                vector1.W < vector2.W ? vector2.W : vector1.W
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="result">A <see cref="Vector4"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector4 vector1, ref Vector4 vector2, out Vector4 result)
        {
            result = new Vector4(
                vector1.X < vector2.X ? vector2.X : vector1.X,
                vector1.Y < vector2.Y ? vector2.Y : vector1.Y,
                vector1.Z < vector2.Z ? vector2.Z : vector1.Z,
                vector1.W < vector2.W ? vector2.W : vector1.W
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector4"/>.</returns>
        public static Vector4 Reflect(Vector4 vector, Vector4 normal)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z + vector.W * normal.W;

            return new Vector4(
                vector.X - 2f * dot * normal.X,
                vector.Y - 2f * dot * normal.Y,
                vector.Z - 2f * dot * normal.Z,
                vector.W - 2f * dot * normal.W
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector4"/>.</param>
        public static void Reflect(ref Vector4 vector, ref Vector4 normal, out Vector4 result)
        {
            var dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z + vector.W * normal.W;

            result = new Vector4(
                vector.X - 2f * dot * normal.X,
                vector.Y - 2f * dot * normal.Y,
                vector.Z - 2f * dot * normal.Z,
                vector.W - 2f * dot * normal.W
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 Lerp(Vector4 vector1, Vector4 vector2, Single amount)
        {
            return new Vector4(
                vector1.X + (vector2.X - vector1.X) * amount,
                vector1.Y + (vector2.Y - vector1.Y) * amount,
                vector1.Z + (vector2.Z - vector1.Z) * amount,
                vector1.W + (vector2.W - vector1.W) * amount
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector4"/>.</param>
        /// <param name="vector2">The second <see cref="Vector4"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void Lerp(ref Vector4 vector1, ref Vector4 vector2, Single amount, out Vector4 result)
        {
            result = new Vector4(
                vector1.X + (vector2.X - vector1.X) * amount,
                vector1.Y + (vector2.Y - vector1.Y) * amount,
                vector1.Z + (vector2.Z - vector1.Z) * amount,
                vector1.W + (vector2.W - vector1.W) * amount
            );
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 Hermite(Vector4 vector1, Vector4 tangent1, Vector4 vector2, Vector4 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            return new Vector4(
                (float)(vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4),
                (float)(vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4),
                (float)(vector1.Z * polynomial1 + tangent1.Z * polynomial2 + vector2.Z * polynomial3 + tangent2.Z * polynomial4),
                (float)(vector1.W * polynomial1 + tangent1.W * polynomial2 + vector2.W * polynomial3 + tangent2.W * polynomial4)
            );
        }

        /// <summary>
        /// Performs a cubic Hermite spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="vector2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void Hermite(ref Vector4 vector1, ref Vector4 tangent1, ref Vector4 vector2, ref Vector4 tangent2, Single amount, out Vector4 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            result = new Vector4(
                (float)(vector1.X * polynomial1 + tangent1.X * polynomial2 + vector2.X * polynomial3 + tangent2.X * polynomial4),
                (float)(vector1.Y * polynomial1 + tangent1.Y * polynomial2 + vector2.Y * polynomial3 + tangent2.Y * polynomial4),
                (float)(vector1.Z * polynomial1 + tangent1.Z * polynomial2 + vector2.Z * polynomial3 + tangent2.Z * polynomial4),
                (float)(vector1.W * polynomial1 + tangent1.W * polynomial2 + vector2.W * polynomial3 + tangent2.W * polynomial4)
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 SmoothStep(Vector4 vector1, Vector4 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            return new Vector4(
                vector1.X + (vector2.X - vector1.X) * amount,
                vector1.Y + (vector2.Y - vector1.Y) * amount,
                vector1.Z + (vector2.Z - vector1.Z) * amount,
                vector1.W + (vector2.W - vector1.W) * amount
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void SmoothStep(ref Vector4 vector1, ref Vector4 vector2, Single amount, out Vector4 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            result = new Vector4(
                vector1.X + (vector2.X - vector1.X) * amount,
                vector1.Y + (vector2.Y - vector1.Y) * amount,
                vector1.Z + (vector2.Z - vector1.Z) * amount,
                vector1.W + (vector2.W - vector1.W) * amount
            );
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector4"/>.</returns>
        public static Vector4 CatmullRom(Vector4 vector1, Vector4 vector2, Vector4 vector3, Vector4 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            return new Vector4(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3)),
                (float)(0.5 * (2.0 * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0 * vector1.Z - 5.0 * vector2.Z + 4.0 * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0 * vector2.Z - 3.0 * vector3.Z + vector4.Z) * t3)),
                (float)(0.5 * (2.0 * vector2.W + (-vector1.W + vector3.W) * amount + (2.0 * vector1.W - 5.0 * vector2.W + 4.0 * vector3.W - vector4.W) * t2 + (-vector1.W + 3.0 * vector2.W - 3.0 * vector3.W + vector4.W) * t3))
            );
        }

        /// <summary>
        /// Performs a Catmull-Rom spline interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="vector3">The third vector.</param>
        /// <param name="vector4">The fourth vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector4"/>.</param>
        public static void CatmullRom(ref Vector4 vector1, ref Vector4 vector2, ref Vector4 vector3, ref Vector4 vector4, Single amount, out Vector4 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result = new Vector4(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3)),
                (float)(0.5 * (2.0 * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0 * vector1.Z - 5.0 * vector2.Z + 4.0 * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0 * vector2.Z - 3.0 * vector3.Z + vector4.Z) * t3)),
                (float)(0.5 * (2.0 * vector2.W + (-vector1.W + vector3.W) * amount + (2.0 * vector1.W - 5.0 * vector2.W + 4.0 * vector3.W - vector4.W) * t2 + (-vector1.W + 3.0 * vector2.W - 3.0 * vector3.W + vector4.W) * t3))
            );
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector4 vector1, Vector4 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            result = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector4 vector1, Vector4 vector2)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        /// <summary>
        /// Calculates square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector4 vector1, ref Vector4 vector2, out Single result)
        {
            var dx = vector2.X - vector1.X;
            var dy = vector2.Y - vector1.Y;
            var dz = vector2.Z - vector1.Z;
            var dw = vector2.W - vector1.W;

            result = dx * dx + dy * dy + dz * dz + dw * dw;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector4"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector4 Barycentric(Vector4 v1, Vector4 v2, Vector4 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            var py = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            var pz = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);
            var pw = (b1 * v1.W) + (b2 * v2.W) + (b3 * v3.W);
            return new Vector4(px, py, pz, pw);
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <param name="result">A <see cref="Vector4"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector4 v1, ref Vector4 v2, ref Vector4 v3, Single b2, Single b3, out Vector4 result)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.X) + (b2 * v2.X) + (b3 * v3.X);
            var py = (b1 * v1.Y) + (b2 * v2.Y) + (b3 * v3.Y);
            var pz = (b1 * v1.Z) + (b2 * v2.Z) + (b3 * v3.Z);
            var pw = (b1 * v1.W) + (b2 * v2.W) + (b3 * v3.W);
            result = new Vector4(px, py, pz, pw);
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
                hash = hash * 23 + Z.GetHashCode();
                hash = hash * 23 + W.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2} {3}", X, Y, Z, W);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Vector4))
                return false;
            return Equals((Vector4)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Vector4 Interpolate(Vector4 target, Single t)
        {
            var x = Tweening.Lerp(this.X, target.X, t);
            var y = Tweening.Lerp(this.Y, target.Y, t);
            var z = Tweening.Lerp(this.Z, target.Z, t);
            var w = Tweening.Lerp(this.W, target.W, t);
            return new Vector4(x, y, z, w);
        }

        /// <summary>
        /// Gets a vector with all four components set to zero.
        /// </summary>
        public static Vector4 Zero
        {
            get { return new Vector4(0, 0, 0, 0); }
        }

        /// <summary>
        /// Gets a vector with all four components set to one.
        /// </summary>
        public static Vector4 One
        {
            get { return new Vector4(1, 1, 1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector4 UnitX
        {
            get { return new Vector4(1, 0, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector4 UnitY
        {
            get { return new Vector4(0, 1, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the z-axis.
        /// </summary>
        public static Vector4 UnitZ
        {
            get { return new Vector4(0, 0, 1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the w-axis.
        /// </summary>
        public static Vector4 UnitW
        {
            get { return new Vector4(0, 0, 0, 1); }
        }

        /// <summary>
        /// The vector's x-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Single X;

        /// <summary>
        /// The vector's y-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public Single Y;

        /// <summary>
        /// The vector's z-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "z", Required = Required.Always)]
        public Single Z;

        /// <summary>
        /// The vector's w-coordinate.
        /// </summary>
        [Preserve]
        [JsonProperty(PropertyName = "w", Required = Required.Always)]
        public Single W;
    }
}
