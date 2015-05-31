using System;
using System.Diagnostics;
using System.Globalization;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a two-dimensional vector.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{X:{X} Y:{Y}\}")]
    public struct Vector2 : IEquatable<Vector2>, IInterpolatable<Vector2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> structure with all of its components set to the specified value.
        /// </summary>
        /// <param name="value">The value to which to set the vector's components.</param>
        public Vector2(Single value)
        {
            this.x = value;
            this.y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> structure with the specified x and y components.
        /// </summary>
        /// <param name="x">The vector's x component.</param>
        /// <param name="y">The vector's y component.</param>
        public Vector2(Single x, Single y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector2"/> to compare.</param>
        /// <returns><c>true</c> if the specified vectors are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Vector2 vector1, Vector2 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/> to compare.</param>
        /// <param name="vector2">The second <see cref="Vector2"/> to compare.</param>
        /// <returns><c>true</c> if the specified vectors are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Vector2 vector1, Vector2 vector2)
        {
            return !vector1.Equals(vector2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x * vector2.x, 
                vector1.y * vector2.Y
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Single factor, Vector2 vector)
        {
            return new Vector2(
                vector.x * factor,
                vector.y * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(Vector2 vector, Single factor)
        {
            return new Vector2(
                vector.x * factor, 
                vector.y * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x / vector2.x,
                vector1.y / vector2.y
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 operator /(Vector2 vector, Single factor)
        {
            return new Vector2(
                vector.x / factor,
                vector.y / factor
            );
        }

        /// <summary>
        /// Reverses the signs of a vector's components.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reverse.</param>
        /// <returns>The reversed <see cref="Vector2"/>.</returns>
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2(-vector.x, -vector.y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2(Vector2 vector)
        {
            return new Point2((Int32)vector.X, (Int32)vector.Y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2F"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2F(Vector2 vector)
        {
            return new Point2F(vector.X, vector.Y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Vector2"/> structure to a <see cref="Point2D"/> structure.
        /// </summary>
        /// <param name="vector">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Point2D(Vector2 vector)
        {
            return new Point2D(vector.X, vector.Y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector2(Point2 point)
        {
            return new Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2F"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector2(Point2F point)
        {
            return new Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Explicitly converts a <see cref="Point2D"/> structure to a <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="point">The structure to convert.</param>
        /// <returns>The converted structure.</returns>
        public static explicit operator Vector2(Point2D point)
        {
            return new Vector2((Single)point.X, (Single)point.Y);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Vector2 vector)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out vector);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <returns>A instance of the <see cref="Vector2"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        public static Vector2 Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector2"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="vector">A variable to populate with the converted value.</param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Vector2 vector)
        {
            vector = default(Vector2);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
            if (components.Length != 2)
                return false;

            Single x, y;
            if (!Single.TryParse(components[0], style, provider, out x))
                return false;
            if (!Single.TryParse(components[1], style, provider, out y))
                return false;

            vector = new Vector2(x, y);
            return true;
        }

        /// <summary>
        /// Converts the string representation of a vector into an instance of the <see cref="Vector2"/> structure.
        /// </summary>
        /// <param name="s">A string containing a vector to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the <see cref="Vector2"/> structure equivalent to the vector contained in <paramref name="s"/>.</returns>
        public static Vector2 Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Vector2 vector;
            if (!TryParse(s, style, provider, out vector))
                throw new FormatException();
            return vector;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>The dot product of the specified vectors.</returns>
        public static Single Dot(Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The dot product of the specified vectors.</param>
        public static void Dot(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            result = vector1.x * vector2.x + vector1.y * vector2.y;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Add(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the addition operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Add(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Subtract(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        /// <summary>
        /// Subtracts one vector from another vector.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the subtraction operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Subtract(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Multiply(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x * vector2.x,
                vector1.y * vector2.y
            );
        }

        /// <summary>
        /// Multiplies two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the multiplication operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the multiplication operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Multiply(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(
                vector1.x * vector2.x, 
                vector1.y * vector2.y
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Multiply(Vector2 vector, Single factor)
        {
            return new Vector2(
                vector.x * factor,
                vector.y * factor
            );
        }

        /// <summary>
        /// Multiplies a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to multiply.</param>
        /// <param name="factor">The scaling factor by which to multiply the vector.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Multiply(ref Vector2 vector, Single factor, out Vector2 result)
        {
            result = new Vector2(
                vector.x * factor,
                vector.y * factor
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Divide(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x / vector2.x,
                vector1.y / vector2.y
            );
        }

        /// <summary>
        /// Divides two vectors.
        /// </summary>
        /// <param name="vector1">The <see cref="Vector2"/> to the left of the division operator.</param>
        /// <param name="vector2">The <see cref="Vector2"/> to the right of the division operator.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Divide(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(
                vector1.x / vector2.x,
                vector1.y / vector2.y
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public static Vector2 Divide(Vector2 vector, Single factor)
        {
            return new Vector2(
                vector.x / factor,
                vector.y / factor
            );
        }

        /// <summary>
        /// Divides a vector by a scaling factor.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to divide.</param>
        /// <param name="factor">The scaling factor by which to divide the vector.</param>
        /// <param name="result">The resulting <see cref="Vector2"/>.</param>
        public static void Divide(ref Vector2 vector, Single factor, out Vector2 result)
        {
            result = new Vector2(
                vector.x / factor,
                vector.y / factor
            );
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform(Vector2 vector, Matrix matrix)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y) + matrix.M14;
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y) + matrix.M24;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector2"/>.</param>
        public static void Transform(ref Vector2 vector, ref Matrix matrix, out Vector2 result)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y) + matrix.M14;
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y) + matrix.M24;
            result = new Vector2(x, y);
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        public static Vector2 TransformNormal(Vector2 vector, Matrix matrix)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Transforms a vector normal by a matrix.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The matrix by which to transform the vector.</param>
        /// <param name="result">The transformed <see cref="Vector2"/>.</param>
        public static void TransformNormal(ref Vector2 vector, ref Matrix matrix, out Vector2 result)
        {
            var x = (matrix.M11 * vector.X + matrix.M12 * vector.Y);
            var y = (matrix.M21 * vector.X + matrix.M22 * vector.Y);
            result = new Vector2(x, y);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to normalize.</param>
        /// <returns>The normalized <see cref="Vector2"/>.</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            var inverseMagnitude = 1f / vector.Length();
            return new Vector2(vector.x * inverseMagnitude, vector.y * inverseMagnitude);
        }

        /// <summary>
        /// Normalizes a vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to normalize.</param>
        /// <param name="result">The normalized <see cref="Vector2"/>.</param>
        public static void Normalize(ref Vector2 vector, out Vector2 result)
        {
            var inverseMagnitude = 1f / vector.Length();
            result = new Vector2(vector.x * inverseMagnitude, vector.y * inverseMagnitude);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to negate.</param>
        /// <returns>The negated <see cref="Vector2"/>.</returns>
        public static Vector2 Negate(Vector2 vector)
        {
            return new Vector2(-vector.x, -vector.y);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to negate.</param>
        /// <param name="result">The negated <see cref="Vector2"/>.</param>
        public static void Negate(ref Vector2 vector, out Vector2 result)
        {
            result = new Vector2(-vector.x, -vector.y);
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped <see cref="Vector2"/>.</returns>
        public static Vector2 Clamp(Vector2 vector, Vector2 min, Vector2 max)
        {
            return new Vector2(
                vector.x < min.x ? min.x : vector.x > max.x ? max.x : vector.x,
                vector.y < min.y ? min.y : vector.y > max.y ? max.y : vector.y
            );
        }

        /// <summary>
        /// Clamps a vector to the specified range.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">The clamped <see cref="Vector2"/>.</param>
        public static void Clamp(ref Vector2 vector, ref Vector2 min, ref Vector2 max, out Vector2 result)
        {
            result = new Vector2(
                vector.x < min.x ? min.x : vector.x > max.x ? max.x : vector.x,
                vector.y < min.y ? min.y : vector.y > max.y ? max.y : vector.y
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>A <see cref="Vector2"/> which contains the lowest value of each component of the specified vectors.</returns>
        public static Vector2 Min(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x < vector2.x ? vector1.x : vector2.x,
                vector1.y < vector2.y ? vector1.y : vector2.y
            );
        }

        /// <summary>
        /// Returns a vector which contains the lowest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">A <see cref="Vector2"/> which contains the lowest value of each component of the specified vectors.</param>
        public static void Min(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(
                vector1.x < vector2.x ? vector1.x : vector2.x,
                vector1.y < vector2.y ? vector1.y : vector2.y
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <returns>A <see cref="Vector2"/> which contains the highest value of each component of the specified vectors.</returns>
        public static Vector2 Max(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.x < vector2.x ? vector2.x : vector1.x,
                vector1.y < vector2.y ? vector2.y : vector1.y
            );
        }

        /// <summary>
        /// Returns a vector which contains the highest value of each component of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">A <see cref="Vector2"/> which contains the highest value of each component of the specified vectors.</param>
        public static void Max(ref Vector2 vector1, ref Vector2 vector2, out Vector2 result)
        {
            result = new Vector2(
                vector1.x < vector2.x ? vector2.x : vector1.x,
                vector1.y < vector2.y ? vector2.y : vector1.y
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <returns>The reflected <see cref="Vector2"/>.</returns>
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            var dot = vector.x * normal.x + vector.y * normal.y;

            return new Vector2(
                vector.x - 2f * dot * normal.x,
                vector.y - 2f * dot * normal.y
            );
        }

        /// <summary>
        /// Reflects the specified vector off of a surface with the specified normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reflect.</param>
        /// <param name="normal">The normal vector of the surface over which to reflect the vector.</param>
        /// <param name="result">The reflected <see cref="Vector2"/>.</param>
        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            var dot = vector.x * normal.x + vector.y * normal.y;

            result = new Vector2(
                vector.x - 2f * dot * normal.x,
                vector.y - 2f * dot * normal.y
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 Lerp(Vector2 vector1, Vector2 vector2, Single amount)
        {
            return new Vector2(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount
            );
        }

        /// <summary>
        /// Performs a linear interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void Lerp(ref Vector2 vector1, ref Vector2 vector2, Single amount, out Vector2 result)
        {
            result = new Vector2(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount
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
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 Hermite(Vector2 vector1, Vector2 tangent1, Vector2 vector2, Vector2 tangent2, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            return new Vector2(
                (float)(vector1.x * polynomial1 + tangent1.x * polynomial2 + vector2.x * polynomial3 + tangent2.x * polynomial4),
                (float)(vector1.y * polynomial1 + tangent1.y * polynomial2 + vector2.y * polynomial3 + tangent2.y * polynomial4)
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
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void Hermite(ref Vector2 vector1, ref Vector2 tangent1, ref Vector2 vector2, ref Vector2 tangent2, Single amount, out Vector2 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            var polynomial1 = (2.0 * t3 - 3.0 * t2 + 1);    // (2t^3 - 3t^2 + 1)
            var polynomial2 = (t3 - 2.0 * t2 + amount);     // (t3 - 2t^2 + t)  
            var polynomial3 = (-2.0 * t3 + 3.0 * t2);       // (-2t^2 + 3t^2)
            var polynomial4 = (t3 - t2);                    // (t^3 - t^2)

            result = new Vector2(
                (float)(vector1.x * polynomial1 + tangent1.x * polynomial2 + vector2.x * polynomial3 + tangent2.x * polynomial4),
                (float)(vector1.y * polynomial1 + tangent1.y * polynomial2 + vector2.y * polynomial3 + tangent2.y * polynomial4)
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first <see cref="Vector2"/>.</param>
        /// <param name="vector2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated vector.</returns>
        public static Vector2 SmoothStep(Vector2 vector1, Vector2 vector2, Single amount)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            return new Vector2(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount
            );
        }

        /// <summary>
        /// Performs a smooth step interpolation between the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="amount">A value from 0.0 to 1.0 representing the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void SmoothStep(ref Vector2 vector1, ref Vector2 vector2, Single amount, out Vector2 result)
        {
            amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
            amount = (float)(amount * amount * (3.0 - 2.0 * amount));

            result = new Vector2(
                vector1.x + (vector2.x - vector1.x) * amount,
                vector1.y + (vector2.y - vector1.y) * amount
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
        /// <returns>The interpolated <see cref="Vector2"/>.</returns>
        public static Vector2 CatmullRom(Vector2 vector1, Vector2 vector2, Vector2 vector3, Vector2 vector4, Single amount)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            return new Vector2(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3))
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
        /// <param name="result">The interpolated <see cref="Vector2"/>.</param>
        public static void CatmullRom(ref Vector2 vector1, ref Vector2 vector2, ref Vector2 vector3, ref Vector2 vector4, Single amount, out Vector2 result)
        {
            var t2 = amount * amount;
            var t3 = t2 * amount;

            result = new Vector2(
                (float)(0.5 * (2.0 * vector2.X + (-vector1.X + vector3.X) * amount + (2.0 * vector1.X - 5.0 * vector2.X + 4.0 * vector3.X - vector4.X) * t2 + (-vector1.X + 3.0 * vector2.X - 3.0 * vector3.X + vector4.X) * t3)),
                (float)(0.5 * (2.0 * vector2.Y + (-vector1.Y + vector3.Y) * amount + (2.0 * vector1.Y - 5.0 * vector2.Y + 4.0 * vector3.Y - vector4.Y) * t2 + (-vector1.Y + 3.0 * vector2.Y - 3.0 * vector3.Y + vector4.Y) * t3))
            );
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        public static Single Distance(Vector2 vector1, Vector2 vector2)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The distance between the specified coordinates.</param>
        public static void Distance(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;

            result = (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <returns>The square of the distance between the specified coordinates.</returns>
        public static Single DistanceSquared(Vector2 vector1, Vector2 vector2)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;

            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Calculates the square of the distance between two coordinates.
        /// </summary>
        /// <param name="vector1">The first coordinate.</param>
        /// <param name="vector2">The second coordinate.</param>
        /// <param name="result">The square of the distance between the specified coordinates.</param>
        public static void DistanceSquared(ref Vector2 vector1, ref Vector2 vector2, out Single result)
        {
            var dx = vector2.x - vector1.x;
            var dy = vector2.y - vector1.y;

            result = dx * dx + dy * dy;
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <returns>A <see cref="Vector2"/> containing the Cartesian coordinates of the specified point.</returns>
        public static Vector2 Barycentric(Vector2 v1, Vector2 v2, Vector2 v3, Single b2, Single b3)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.x) + (b2 * v2.x) + (b3 * v3.x);
            var py = (b1 * v1.y) + (b2 * v2.y) + (b3 * v3.y);
            return new Vector2(px, py);
        }

        /// <summary>
        /// Computes the Cartesian coordinates of a point specified in areal Barycentric coordinates relative to a triangle.
        /// </summary>
        /// <param name="v1">The first vertex of the triangle.</param>
        /// <param name="v2">The second vertex of the triangle.</param>
        /// <param name="v3">The third vertex of the triangle.</param>
        /// <param name="b2">Barycentric coordinate b2, which expresses the weighting factor towards the second triangle vertex.</param>
        /// <param name="b3">Barycentric coordinate b3, which expresses the weighting factor towards the third triangle vertex.</param>
        /// <param name="result">A <see cref="Vector2"/> containing the Cartesian coordinates of the specified point.</param>
        public static void Barycentric(ref Vector2 v1, ref Vector2 v2, ref Vector2 v3, Single b2, Single b3, out Vector2 result)
        {
            var b1 = (1 - b2 - b3);
            var px = (b1 * v1.x) + (b2 * v2.x) + (b3 * v3.x);
            var py = (b1 * v1.y) + (b2 * v2.y) + (b3 * v3.y);
            result = new Vector2(px, py);
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
            return String.Format(provider, "{0} {1}", x, y);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Vector2))
                return false;
            return Equals((Vector2)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Vector2 other)
        {
            return x == other.x && y == other.y;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public Single Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public Single LengthSquared()
        {
            return x * x + y * y;
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        public Vector2 Interpolate(Vector2 target, Single t)
        {
            var x = Tweening.Lerp(this.x, target.x, t);
            var y = Tweening.Lerp(this.y, target.y, t);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Gets a vector with both components set to zero.
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }

        /// <summary>
        /// Gets a vector with both components set to one.
        /// </summary>
        public static Vector2 One
        {
            get { return new Vector2(1, 1); }
        }

        /// <summary>
        /// Returns the unit vector for the x-axis.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return new Vector2(1, 0); }
        }

        /// <summary>
        /// Returns the unit vector for the y-axis.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return new Vector2(0, 1); }
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

        // Property values.
        private readonly Single x;
        private readonly Single y;
    }
}
