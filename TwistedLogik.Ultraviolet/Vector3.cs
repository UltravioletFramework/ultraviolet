using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a three-dimensional vector.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y} Z:{Z}\}")]
    public struct Vector3 : IEquatable<Vector3>, IInterpolatable<Vector3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        public Vector3(Single value)
        {
            this.x = value;
            this.y = value;
            this.z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with the specified x, y, and z component.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        /// <param name="z">The vector's z component.</param>
        public Vector3(Single x, Single y, Single z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3"/> structure with its x and y components set to the 
        /// x and y components of the specified vector, and its z component set to the specified value.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> from which to set the vector's x and y components.</param>
        /// <param name="z">The vector's z component.</param>
        public Vector3(Vector2 vector, Single z)
        {
            this.x = vector.X;
            this.y = vector.Y;
            this.z = z;
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector3"/> to compare.</param>
        /// <returns><c>true</c> if the specified vectors are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector3"/> to compare.</param>
        /// <returns><c>true</c> if the specified vectors are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Vector3 vector1, Vector3 vector2)
        {
            return !vector1.Equals(vector2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x * vector2.x, 
                vector1.y * vector2.y, 
                vector1.z * vector2.z
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Single factor, Vector3 vector)
        {
            return new Vector3(
                vector.x * factor, 
                vector.y * factor, 
                vector.z * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator *(Vector3 vector, Single factor)
        {
            return new Vector3(
                vector.x * factor, 
                vector.y * factor, 
                vector.z * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator /(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x / vector2.x,
                vector1.y / vector2.y,
                vector1.z / vector2.z
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 operator /(Vector3 vector, Single factor)
        {
            return new Vector3(
                vector.x / factor,
                vector.y / factor,
                vector.z / factor
            );
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector3"/>.</returns>
        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector3"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Vector3 vector)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out vector);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <returns>A instance of the <see cref="Vector3"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        public static Vector3 Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector3"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Vector3 vector)
        {
            vector = default(Vector3);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 3)
                return false;

            Single x, y, z;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;
            if (!Single.TryParse(components[2], style, provider, out z))
                return false;

            vector = new Vector3(x, y, z);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector3"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Vector3"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        public static Vector3 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Vector3 vector;
            if (!TryParse(s, style, provider, out vector))
                throw new FormatException();
            return vector;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>The cross product of the specified vectors.</returns>
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            var cx = vector1.y * vector2.z - vector1.z * vector2.y;
            var cy = vector1.z * vector2.x - vector1.x * vector2.z;
            var cz = vector1.x * vector2.y - vector1.y * vector2.x;
            return new Vector3(cx, cy, cz);
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">The cross product of the specified vectors.</param>
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            var cx = vector1.y * vector2.z - vector1.z * vector2.y;
            var cy = vector1.z * vector2.x - vector1.x * vector2.z;
            var cz = vector1.x * vector2.y - vector1.y * vector2.x;
            result = new Vector3(cx, cy, cz);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            result = vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the addition operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Add(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="left">The <see cref="Vector3"/> to the left of the subtraction operator.</param>
        /// <param name="right">The <see cref="Vector3"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result = new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Multiply(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x * vector2.x,
                vector1.y * vector2.y,
                vector1.z * vector2.z
            );
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Multiply(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result = new Vector3(
                vector1.x * vector2.x,
                vector1.y * vector2.y,
                vector1.z * vector2.z
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Multiply(Vector3 vector, Single factor)
        {
            return new Vector3(
                vector.x * factor,
                vector.y * factor,
                vector.z * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Multiply(ref Vector3 vector, Single factor, out Vector3 result)
        {
            result = new Vector3(
                vector.x * factor,
                vector.y * factor,
                vector.z * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Divide(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x / vector2.x,
                vector1.y / vector2.y,
                vector1.z / vector2.z
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector3"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector3"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Divide(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result = new Vector3(
                vector1.x / vector2.x,
                vector1.y / vector2.y,
                vector1.z / vector2.z
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        public static Vector3 Divide(Vector3 vector, Single factor)
        {
            return new Vector3(
                vector.x / factor,
                vector.y / factor,
                vector.z / factor
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector3"/>.</param>
        public static void Divide(ref Vector3 vector, Single factor, out Vector3 result)
        {
            result = new Vector3(
                vector.x / factor,
                vector.y / factor,
                vector.z / factor
            );
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        public static Vector3 Transform(Vector3 vector, Matrix matrix)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z) + matrix.M14;
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z) + matrix.M24;
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z) + matrix.M34;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector3"/>.</param>
        public static void Transform(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z) + matrix.M14;
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z) + matrix.M24;
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z) + matrix.M34;
            result = new Vector3(x, y, z);
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        public static Vector3 TransformNormal(Vector3 vector, Matrix matrix)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z);
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector3"/>.</param>
        public static void TransformNormal(ref Vector3 vector, ref Matrix matrix, out Vector3 result)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y + matrix.M13 * vector.Z);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y + matrix.M23 * vector.Z);
            var z = (matrix.M31 * vector.X + matrix.M32 * vector.Y + matrix.M33 * vector.Z);
            result = new Vector3(x, y, z);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector3"/>.</returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            var inverseMagnitude = 1f / vector.Length();
            return new Vector3(vector.x * inverseMagnitude, vector.y * inverseMagnitude, vector.z * inverseMagnitude);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector3"/>.</param>
        public static void Normalize(ref Vector3 vector, out Vector3 result)
        {
            var inverseMagnitude = 1f / vector.Length();
            result = new Vector3(vector.x * inverseMagnitude, vector.y * inverseMagnitude, vector.z * inverseMagnitude);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to negate.</param>
        /// <returns>The negated <see cref="Vector3"/>.</returns>
        public static Vector3 Negate(Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector3"/>.</param>
        public static void Negate(ref Vector3 vector, out Vector3 result)
        {
            result = new Vector3(-vector.x, -vector.y, -vector.z);
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector3"/>.</returns>
        public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
        {
            return new Vector3(
                vector.x < min.x ? min.x : vector.x > max.x ? max.x : vector.x,
                vector.y < min.y ? min.y : vector.y > max.y ? max.y : vector.y,
                vector.z < min.z ? min.z : vector.z > max.z ? max.z : vector.z
            );
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector3"/>.</param>
        public static void Clamp(ref Vector3 vector, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result = new Vector3(
                vector.x < min.x ? min.x : vector.x > max.x ? max.x : vector.x,
                vector.y < min.y ? min.y : vector.y > max.y ? max.y : vector.y,
                vector.z < min.z ? min.z : vector.z > max.z ? max.z : vector.z
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>A <see cref="Vector3"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector3 Min(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x < vector2.x ? vector1.x : vector2.x,
                vector1.y < vector2.y ? vector1.y : vector2.y,
                vector1.z < vector2.z ? vector1.z : vector2.z
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">A <see cref="Vector3"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result = new Vector3(
                vector1.x < vector2.x ? vector1.x : vector2.x,
                vector1.y < vector2.y ? vector1.y : vector2.y,
                vector1.z < vector2.z ? vector1.z : vector2.z
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <returns>A <see cref="Vector3"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector3 Max(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.x < vector2.x ? vector2.x : vector1.x,
                vector1.y < vector2.y ? vector2.y : vector1.y,
                vector1.z < vector2.z ? vector2.z : vector1.z
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="result">A <see cref="Vector3"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            result = new Vector3(
                vector1.x < vector2.x ? vector2.x : vector1.x,
                vector1.y < vector2.y ? vector2.y : vector1.y,
                vector1.z < vector2.z ? vector2.z : vector1.z
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector3"/>.</returns>
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            var dot = vector.x * normal.x + vector.y * normal.y + vector.z * normal.z;

            return new Vector3(
                vector.x - 2f * dot * normal.x,
                vector.y - 2f * dot * normal.y,
                vector.z - 2f * dot * normal.z
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector3"/>.</param>
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            var dot = vector.x * normal.x + vector.y * normal.y + vector.z * normal.z;

            result = new Vector3(
                vector.x - 2f * dot * normal.x,
                vector.y - 2f * dot * normal.y,
                vector.z - 2f * dot * normal.z
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 Lerp(Vector3 vector1, Vector3 vector2, Single amount)
        {
            return new Vector3(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount,
                vector1.z + (vector2.z - vector1.z) * amount
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void Lerp(ref Vector3 vector1, ref Vector3 vector2, Single amount, out Vector3 result)
        {
            result = new Vector3(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount,
                vector1.z + (vector2.z - vector1.z) * amount
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
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 Hermite(Vector3 vector1, Vector3 tangent1, Vector3 vector2, Vector3 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            return new Vector3(
                (float)(vector1.x * polynomial1 + tangent1.x * polynomial2 + vector2.x * polynomial3 + tangent2.x * polynomial4),
                (float)(vector1.y * polynomial1 + tangent1.y * polynomial2 + vector2.y * polynomial3 + tangent2.y * polynomial4),
                (float)(vector1.z * polynomial1 + tangent1.z * polynomial2 + vector2.z * polynomial3 + tangent2.z * polynomial4)
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
        /// <param name="result">The interpolated vector.</param>
        public static void Hermite(ref Vector3 vector1, ref Vector3 tangent1, ref Vector3 vector2, ref Vector3 tangent2, Single amount, out Vector3 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            result = new Vector3(
                (float)(vector1.x * polynomial1 + tangent1.x * polynomial2 + vector2.x * polynomial3 + tangent2.x * polynomial4),
                (float)(vector1.y * polynomial1 + tangent1.y * polynomial2 + vector2.y * polynomial3 + tangent2.y * polynomial4),
                (float)(vector1.z * polynomial1 + tangent1.z * polynomial2 + vector2.z * polynomial3 + tangent2.z * polynomial4)
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 SmoothStep(Vector3 vector1, Vector3 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            return new Vector3(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount,
                vector1.z + (vector2.z - vector1.z) * amount
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector3"/>.</param>
        /// <param name="vector2">The second <see cref="Vector3"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void SmoothStep(ref Vector3 vector1, ref Vector3 vector2, Single amount, out Vector3 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            result = new Vector3(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount,
                vector1.z + (vector2.z - vector1.z) * amount
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
        /// <returns>The interpolated <see cref="Vector3"/>.</returns>
        public static Vector3 CatmullRom(Vector3 vector1, Vector3 vector2, Vector3 vector3, Vector3 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            return new Vector3(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3)),
                (float)(0.5 * (2.0 * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0 * vector1.Z - 5.0 * vector2.Z + 4.0 * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0 * vector2.Z - 3.0 * vector3.Z + vector4.Z) * t3))
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
        /// <param name="result">The interpolated <see cref="Vector3"/>.</param>
        public static void CatmullRom(ref Vector3 vector1, ref Vector3 vector2, ref Vector3 vector3, ref Vector3 vector4, Single amount, out Vector3 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result = new Vector3(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3)),
                (float)(0.5 * (2.0 * vector2.Z + (-vector1.Z + vector3.Z) * amount + (2.0 * vector1.Z - 5.0 * vector2.Z + 4.0 * vector3.Z - vector4.Z) * t2 + (-vector1.Z + 3.0 * vector2.Z - 3.0 * vector3.Z + vector4.Z) * t3))
            );
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector3 vector1, Vector3 vector2)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;
            var dz = vector2.z - vector1.z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;
            var dz = vector2.z - vector1.z;

            result = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector3 vector1, Vector3 vector2)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;
            var dz = vector2.z - vector1.z;

            return dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector3 vector1, ref Vector3 vector2, out Single result)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;
            var dz = vector2.z - vector1.z;

            result = dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector3"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector3 Barycentric(Vector3 v1, Vector3 v2, Vector3 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.x) + (b2 * v2.x) + (b3 * v3.x);
            var py = (b1 * v1.y) + (b2 * v2.y) + (b3 * v3.y);
            var pz = (b1 * v1.z) + (b2 * v2.z) + (b3 * v3.z);
            return new Vector3(px, py, pz);
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <param name="result">A <see cref="Vector3"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector3 v1, ref Vector3 v2, ref Vector3 v3, Single b2, Single b3, out Vector3 result)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.x) + (b2 * v2.x) + (b3 * v3.x);
            var py = (b1 * v1.y) + (b2 * v2.y) + (b3 * v3.y);
            var pz = (b1 * v1.z) + (b2 * v2.z) + (b3 * v3.z);
            result = new Vector3(px, py, pz);
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
                hash = hash * 23 + z.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2}", x, y, z);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Vector3))
                return false;
            return Equals((Vector3)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Vector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Vector3 Interpolate(Vector3 target, Single t)
        {
            var x = Tweening.Lerp(this.x, target.x, t);
            var y = Tweening.Lerp(this.y, target.y, t);
            var z = Tweening.Lerp(this.z, target.z, t);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Gets a vector with all three components set to zero.
        /// </summary>
        public static Vector3 Zero
        {
            get { return new Vector3(0, 0, 0); }
        }

        /// <summary>
        /// Gets a vector with all three components set to one.
        /// </summary>
        public static Vector3 One
        {
            get { return new Vector3(1, 1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector3 UnitX
        {
            get { return new Vector3(1, 0, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector3 UnitY
        {
            get { return new Vector3(0, 1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the z-axis.
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return new Vector3(0, 0, 1); }
        }

        /// <summary>
        /// Returns a unit vector representing right in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Right
        {
            get { return new Vector3(1, 0, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing left in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Left
        {
            get { return new Vector3(-1, 0, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing up in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Up
        {
            get { return new Vector3(0, 1, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing down in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Down
        {
            get { return new Vector3(0, -1, 0); }
        }

        /// <summary>
        /// Returns a unit vector representing backward in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Backward
        {
            get { return new Vector3(0, 0, 1); }
        }

        /// <summary>
        /// Returns a unit vector representing forward in a right-handed coordinate system.
        /// </summary>
        public static Vector3 Forward
        {
            get { return new Vector3(0, 0, -1); }
        }

        /// <summary>
        /// Gets the vector's x-coordinate.
        /// </summary>
        public Single X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the vector's y-coordinate.
        /// </summary>
        public Single Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the vector's z-coordinate.
        /// </summary>
        public Single Z
        {
            get { return z; }
        }

        // Property values.
        private readonly Single x;
        private readonly Single y;
        private readonly Single z;
    }
}
