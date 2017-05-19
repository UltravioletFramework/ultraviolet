using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a 4x4 transformation matrix.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{ \{M11:{M11} M12:{M12} M13:{M13} M14:{M14}\} \{M21:{M21} M22:{M22} M23:{M23} M24:{M24}\} \{M31:{M31} M32:{M32} M33:{M33} M34:{M34}\} \{M41:{M41} M42:{M42} M43:{M43} M44:{M44}\} \}")]
    [JsonConverter(typeof(UltravioletJsonConverter))]
    public struct Matrix : IEquatable<Matrix>, IInterpolatable<Matrix>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="M11">The value at row 1, column 1 of the matrix.</param>
        /// <param name="M12">The value at row 1, column 2 of the matrix.</param>
        /// <param name="M13">The value at row 1, column 3 of the matrix.</param>
        /// <param name="M14">The value at row 1, column 4 of the matrix.</param>
        /// <param name="M21">The value at row 2, column 1 of the matrix.</param>
        /// <param name="M22">The value at row 2, column 2 of the matrix.</param>
        /// <param name="M23">The value at row 2, column 3 of the matrix.</param>
        /// <param name="M24">The value at row 2, column 4 of the matrix.</param>
        /// <param name="M31">The value at row 3, column 1 of the matrix.</param>
        /// <param name="M32">The value at row 3, column 2 of the matrix.</param>
        /// <param name="M33">The value at row 3, column 3 of the matrix.</param>
        /// <param name="M34">The value at row 3, column 4 of the matrix.</param>
        /// <param name="M41">The value at row 4, column 1 of the matrix.</param>
        /// <param name="M42">The value at row 4, column 2 of the matrix.</param>
        /// <param name="M43">The value at row 4, column 3 of the matrix.</param>
        /// <param name="M44">The value at row 4, column 4 of the matrix.</param>
        [Preserve]
        public Matrix(
            Single M11, Single M12, Single M13, Single M14,
            Single M21, Single M22, Single M23, Single M24,
            Single M31, Single M32, Single M33, Single M34,
            Single M41, Single M42, Single M43, Single M44)
        {
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
            this.M14 = M14;

            this.M21 = M21;
            this.M22 = M22;
            this.M23 = M23;
            this.M24 = M24;

            this.M31 = M31;
            this.M32 = M32;
            this.M33 = M33;
            this.M34 = M34;

            this.M41 = M41;
            this.M42 = M42;
            this.M43 = M43;
            this.M44 = M44;
        }

        /// <summary>
        /// Compares two matrices for equality.
        /// </summary>
        /// <param name="m1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="m2">The second <see cref="Matrix"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified matrices are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }

        /// <summary>
        /// Compares two matrices for inequality.
        /// </summary>
        /// <param name="m1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="m2">The second <see cref="Matrix"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified matrices are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
        }

        /// <summary>
        /// Multiplies a <see cref="Matrix"/> by a scaling factor.
        /// </summary>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator *(Single multiplier, Matrix multiplicand)
        {
            Matrix result;
            Multiply(ref multiplicand, multiplier, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Matrix"/> by a scaling factor.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator *(Matrix multiplicand, Single multiplier)
        {
            Matrix result;
            Multiply(ref multiplicand, multiplier, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a <see cref="Matrix"/> by another matrix.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator *(Matrix multiplicand, Matrix multiplier)
        {
            Matrix result;
            Multiply(ref multiplicand, ref multiplier, out result);
            return result;
        }

        /// <summary>
        /// Divides a <see cref="Matrix"/> by a scaling factor.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator /(Matrix dividend, Single divisor)
        {
            Matrix result;
            Divide(ref dividend, divisor, out result);
            return result;
        }

        /// <summary>
        /// Divides a <see cref="Matrix"/> by a another matrix.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator /(Matrix dividend, Matrix divisor)
        {
            Matrix result;
            Divide(ref dividend, ref divisor, out result);
            return result;
        }

        /// <summary>
        /// Adds a <see cref="Matrix"/> to another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the addition operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the addition operator.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix result;
            Add(ref m1, ref m2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts a <see cref="Matrix"/> from another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the subtraction operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix result;
            Subtract(ref m1, ref m2, out result);
            return result;
        }

        /// <summary>
        /// Negates the specified elements of the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to negate.</param>
        /// <returns>The negated <see cref="Matrix"/>.</returns>
        [Preserve]
        public static Matrix operator -(Matrix matrix)
        {
            Matrix result;
            Negate(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Converts the string representation of a matrix into an instance of the <see cref="Matrix"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a <see cref="Matrix"/> to convert.</param>
        /// <param name="matrix">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out Matrix matrix)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out matrix);
        }

        /// <summary>
        /// Converts the string representation of a matrix into an instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="s">A string containing a matrix to convert.</param>
        /// <returns>A instance of the <see cref="Matrix"/> structure equivalent to the matrix contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Matrix Parse(String s)
        {
            return Parse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a matrix into an instance of the <see cref="Matrix"/> structure.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a <see cref="Matrix"/> to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <param name="matrix">A variable to populate with the converted value.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Matrix matrix)
        {
            matrix = default(Matrix);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 16)
                return false;

            var values = new Single[16];
            for (int i = 0; i < values.Length; i++)
            {
                if (!Single.TryParse(components[i], style, provider, out values[i]))
                    return false;
            }

            matrix = new Matrix(
                values[0], values[1], values[2], values[3],
                values[4], values[5], values[6], values[7],
                values[8], values[9], values[10], values[11],
                values[12], values[13], values[14], values[15]
            );
            return true;
        }

        /// <summary>
        /// Converts the string representation of a <see cref="Matrix"/> into an instance of the Matrix structure.
        /// </summary>
        /// <param name="s">A string containing a <see cref="Matrix"/> to convert.</param>
        /// <param name="style">A set of <see cref="NumberStyles"/> values indicating which elements are present in <paramref name="s"/>.</param>
        /// <param name="provider">A format provider that provides culture-specific formatting information.</param>
        /// <returns>A instance of the Matrix structure equivalent to the matrix contained in <paramref name="s"/>.</returns>
        [Preserve]
        public static Matrix Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            Matrix matrix;
            if (!TryParse(s, style, provider, out matrix))
                throw new FormatException();
            return matrix;
        }

        /// <summary>
        /// Creates a world matrix with the specified parameters.
        /// </summary>
        /// <param name="position">The object's position.</param>
        /// <param name="forward">The object's forward vector.</param>
        /// <param name="up">The object's up vector.</param>
        /// <returns>The <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
        {
            var normalizedBackward = Vector3.Normalize(-forward);
            var normalizedRight = Vector3.Normalize(Vector3.Cross(up, normalizedBackward));
            var normalizedUp = Vector3.Cross(normalizedBackward, normalizedRight);

            var M11 = normalizedRight.X;
            var M21 = normalizedRight.Y;
            var M31 = normalizedRight.Z;
            var M41 = 0f;

            var M12 = normalizedUp.X;
            var M22 = normalizedUp.Y;
            var M32 = normalizedUp.Z;
            var M42 = 0f;

            var M13 = normalizedBackward.X;
            var M23 = normalizedBackward.Y;
            var M33 = normalizedBackward.Z;
            var M43 = 0f;

            var M14 = position.X;
            var M24 = position.Y;
            var M34 = position.Z;
            var M44 = 1f;

            return new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            );
        }

        /// <summary>
        /// Creates a world matrix with the specified parameters.
        /// </summary>
        /// <param name="position">The object's position.</param>
        /// <param name="forward">The object's forward vector.</param>
        /// <param name="up">The object's up vector.</param>
        /// <param name="result">The <see cref="Matrix"/> that was created.</param>
        public static void CreateWorld(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
        {
            var normalizedBackward = Vector3.Normalize(-forward);
            var normalizedRight = Vector3.Normalize(Vector3.Cross(up, normalizedBackward));
            var normalizedUp = Vector3.Cross(normalizedBackward, normalizedRight);

            var M11 = normalizedRight.X;
            var M21 = normalizedRight.Y;
            var M31 = normalizedRight.Z;
            var M41 = 0f;

            var M12 = normalizedUp.X;
            var M22 = normalizedUp.Y;
            var M32 = normalizedUp.Z;
            var M42 = 0f;

            var M13 = normalizedBackward.X;
            var M23 = normalizedBackward.Y;
            var M33 = normalizedBackward.Z;
            var M43 = 0f;

            var M14 = position.X;
            var M24 = position.Y;
            var M34 = position.Z;
            var M44 = 1f;

            result = new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            );
        }

        /// <summary>
        /// Creates a view matrix.
        /// </summary>
        /// <param name="cameraPosition">The camera's position.</param>
        /// <param name="cameraTarget">The camera's target.</param>
        /// <param name="cameraUp">The camera's up vector.</param>
        /// <returns>The view <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUp)
        {
            var zAxis = Vector3.Normalize(cameraPosition - cameraTarget);
            var xAxis = Vector3.Normalize(Vector3.Cross(cameraUp, zAxis));
            var yAxis = Vector3.Cross(zAxis, xAxis);

            var zDot = -Vector3.Dot(zAxis, cameraPosition);
            var xDot = -Vector3.Dot(xAxis, cameraPosition);
            var yDot = -Vector3.Dot(yAxis, cameraPosition);

            return new Matrix(
                xAxis.X, xAxis.Y, xAxis.Z, xDot,
                yAxis.X, yAxis.Y, yAxis.Z, yDot,
                zAxis.X, zAxis.Y, zAxis.Z, zDot,
                      0,       0,       0,    1);
        }

        /// <summary>
        /// Creates a view matrix.
        /// </summary>
        /// <param name="cameraPosition">The camera's position.</param>
        /// <param name="cameraTarget">The camera's target.</param>
        /// <param name="cameraUp">The camera's up vector.</param>
        /// <param name="result">The view <see cref="Matrix"/> that was created.</param>
        public static void CreateLookAt(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUp, out Matrix result)
        {
            var zAxis = Vector3.Normalize(cameraPosition - cameraTarget);
            var xAxis = Vector3.Normalize(Vector3.Cross(cameraUp, zAxis));
            var yAxis = Vector3.Cross(zAxis, xAxis);
            
            var zDot = -Vector3.Dot(zAxis, cameraPosition);
            var xDot = -Vector3.Dot(xAxis, cameraPosition);
            var yDot = -Vector3.Dot(yAxis, cameraPosition);

            result = new Matrix(
                xAxis.X, xAxis.Y, xAxis.Z, xDot,
                yAxis.X, yAxis.Y, yAxis.Z, yDot,
                zAxis.X, zAxis.Y, zAxis.Z, zDot,
                      0,       0,       0,    1);
        }

        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="width">The width of the view volume.</param>
        /// <param name="height">The height of the view volume.</param>
        /// <param name="zNearPlane">The distance from the camera to the near z-plane.</param>
        /// <param name="zFarPlane">The distance from the camera to the far z-plane.</param>
        /// <returns>The projection <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateOrthographic(Single width, Single height, Single zNearPlane, Single zFarPlane)
        {
            var M11 = (float)(2.0 / width);
            var M22 = (float)(2.0 / height);
            var M33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var M34 = zNearPlane / (zNearPlane - zFarPlane);

            return new Matrix(
                M11, 0, 0, 0,
                  0, M22, 0, 0,
                  0, 0, M33, M34,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="width">The width of the view volume.</param>
        /// <param name="height">The height of the view volume.</param>
        /// <param name="zNearPlane">The distance from the camera to the near z-plane.</param>
        /// <param name="zFarPlane">The distance from the camera to the far z-plane.</param>
        /// <param name="result">The projection <see cref="Matrix"/> that was created.</param>
        public static void CreateOrthographic(Single width, Single height, Single zNearPlane, Single zFarPlane, out Matrix result)
        {
            var M11 = (float)(2.0 / width);
            var M22 = (float)(2.0 / height);
            var M33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var M34 = zNearPlane / (zNearPlane - zFarPlane);

            result = new Matrix(
                M11, 0, 0, 0,
                  0, M22, 0, 0,
                  0, 0, M33, M34,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates an off-center orthographic projection matrix.
        /// </summary>
        /// <param name="left">The minimum x-value of the view volume.</param>
        /// <param name="right">The maximum x-value of the view volume.</param>
        /// <param name="bottom">The minimum y-value of the view volume.</param>
        /// <param name="top">The maximum y-value of the view volume.</param>
        /// <param name="zNearPlane">The distance from the camera to the near z-plane.</param>
        /// <param name="zFarPlane">The distance from the camera to the far z-plane.</param>
        /// <returns>The projection <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateOrthographicOffCenter(Single left, Single right, Single bottom, Single top, Single zNearPlane, Single zFarPlane)
        {
            var M11 = (float)(2.0 / (right - left));
            var M14 = (left + right) / (left - right);
            var M22 = (float)(2.0 / (top - bottom));
            var M24 = (top + bottom) / (bottom - top);
            var M33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var M34 = zNearPlane / (zNearPlane - zFarPlane);

            return new Matrix(
                M11, 0, 0, M14,
                  0, M22, 0, M24,
                  0, 0, M33, M34,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates an off-center orthographic projection matrix.
        /// </summary>
        /// <param name="left">The minimum x-value of the view volume.</param>
        /// <param name="right">The maximum x-value of the view volume.</param>
        /// <param name="bottom">The minimum y-value of the view volume.</param>
        /// <param name="top">The maximum y-value of the view volume.</param>
        /// <param name="zNearPlane">The distance from the camera to the near z-plane.</param>
        /// <param name="zFarPlane">The distance from the camera to the far z-plane.</param>
        /// <param name="result">The projection <see cref="Matrix"/> that was created.</param>
        public static void CreateOrthographicOffCenter(Single left, Single right, Single bottom, Single top, Single zNearPlane, Single zFarPlane, out Matrix result)
        {
            var M11 = (float)(2.0 / (right - left));
            var M14 = (left + right) / (left - right);
            var M22 = (float)(2.0 / (top - bottom));
            var M24 = (top + bottom) / (bottom - top);
            var M33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var M34 = zNearPlane / (zNearPlane - zFarPlane);

            result = new Matrix(
                M11, 0, 0, M14,
                  0, M22, 0, M24,
                  0, 0, M33, M34,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a perspective matrix.
        /// </summary>
        /// <param name="width">The width of the view volume at the near plane.</param>
        /// <param name="height">The height of the view volume at the near plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreatePerspective(Single width, Single height, Single nearPlaneDistance, Single farPlaneDistance)
        {
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var M11 = 2f * nearPlaneDistance / width;
            var M22 = 2f * nearPlaneDistance / height;
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            return new Matrix(
                M11, 0, 0, 0,
                0, M22, 0, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates a perspective matrix.
        /// </summary>
        /// <param name="width">The width of the view volume at the near plane.</param>
        /// <param name="height">The height of the view volume at the near plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <param name="result">The <see cref="Matrix"/> that was created.</param>
        public static void CreatePerspective(Single width, Single height, Single nearPlaneDistance, Single farPlaneDistance, out Matrix result)
        {
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var M11 = 2f * nearPlaneDistance / width;
            var M22 = 2f * nearPlaneDistance / height;
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            result = new Matrix(
                M11, 0, 0, 0,
                0, M22, 0, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates a perspective matrix based on a field of view.
        /// </summary>
        /// <param name="fieldOfView">The field of view in radians.</param>
        /// <param name="aspectRatio">The aspect ratio of the view.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreatePerspectiveFieldOfView(Single fieldOfView, Single aspectRatio, Single nearPlaneDistance, Single farPlaneDistance)
        {
            Contract.EnsureRange(fieldOfView > 0 && fieldOfView < Math.PI, nameof(fieldOfView));
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var yScale = 1f / Math.Tan(fieldOfView * 0.5f);
            var xScale = yScale / aspectRatio;
            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var M11 = (float)xScale;
            var M22 = (float)yScale;
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            return new Matrix(
                M11, 0, 0, 0,
                0, M22, 0, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates a perspective matrix based on a field of view.
        /// </summary>
        /// <param name="fieldOfView">The field of view in radians.</param>
        /// <param name="aspectRatio">The aspect ratio of the view.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <param name="result">The <see cref="Matrix"/> that was created.</param>
        public static void CreatePerspectiveFieldOfView(Single fieldOfView, Single aspectRatio, Single nearPlaneDistance, Single farPlaneDistance, out Matrix result)
        {
            Contract.EnsureRange(fieldOfView > 0 && fieldOfView < Math.PI, nameof(fieldOfView));
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var yScale = 1f / Math.Tan(fieldOfView * 0.5f);
            var xScale = yScale / aspectRatio;
            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var M11 = (float)xScale;
            var M22 = (float)yScale;
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            result = new Matrix(
                M11, 0, 0, 0,
                0, M22, 0, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates a customized perspective matrix.
        /// </summary>
        /// <param name="left">The minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">The maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">The minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">The maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreatePerspectiveOffCenter(Single left, Single right, Single bottom, Single top, Single nearPlaneDistance, Single farPlaneDistance)
        {
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var rpl = right + left;
            var rml = right - left;
            var tpb = top + bottom;
            var tmb = top - bottom;

            var M11 = (float)(2.0 * nearPlaneDistance / rml);
            var M13 = rpl / rml;
            var M22 = (float)(2.0 * nearPlaneDistance / tmb);
            var M23 = tpb / tmb;            
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            return new Matrix(
                M11, 0, M13, 0,
                0, M22, M23, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates a customized perspective matrix.
        /// </summary>
        /// <param name="left">The minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">The maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">The minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">The maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <param name="result">The <see cref="Matrix"/> that was created.</param>
        public static void CreatePerspectiveOffCenter(Single left, Single right, Single bottom, Single top, Single nearPlaneDistance, Single farPlaneDistance, out Matrix result)
        {
            Contract.EnsureRange(nearPlaneDistance > 0, nameof(nearPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > 0, nameof(farPlaneDistance));
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, nameof(nearPlaneDistance));

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var rpl = right + left;
            var rml = right - left;
            var tpb = top + bottom;
            var tmb = top - bottom;

            var M11 = (float)(2.0 * nearPlaneDistance / rml);
            var M13 = rpl / rml;
            var M22 = (float)(2.0 * nearPlaneDistance / tmb);
            var M23 = tpb / tmb;
            var M33 = farPlaneDistance / nearmfar;
            var M34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var M43 = -1f;

            result = new Matrix(
                M11, 0, M13, 0,
                0, M22, M23, 0,
                0, 0, M33, M34,
                0, 0, M43, 0
            );
        }

        /// <summary>
        /// Creates the projection matrix used by a sprite batch.
        /// </summary>
        /// <param name="viewportWidth">The current viewport's width.</param>
        /// <param name="viewportHeight">The current viewport's height.</param>
        /// <returns>The projection <see cref="Matrix"/> used by a sprite batch.</returns>
        public static Matrix CreateSpriteBatchProjection(Single viewportWidth, Single viewportHeight)
        {
            Contract.EnsureRange(viewportWidth > 0, nameof(viewportWidth));
            Contract.EnsureRange(viewportHeight > 0, nameof(viewportHeight));

            var sx = +2f * (1f / viewportWidth);
            var sy = -2f * (1f / viewportHeight);

            return new Matrix(
                sx, 0f, 0f, -1f,
                0f, sy, 0f, 1f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates the projection matrix used by a sprite batch.
        /// </summary>
        /// <param name="viewportWidth">The current viewport's width.</param>
        /// <param name="viewportHeight">The current viewport's height.</param>
        /// <param name="result">The projection <see cref="Matrix"/> used by a sprite batch.</param>
        public static void CreateSpriteBatchProjection(Single viewportWidth, Single viewportHeight, out Matrix result)
        {
            Contract.EnsureRange(viewportWidth > 0, nameof(viewportWidth));
            Contract.EnsureRange(viewportHeight > 0, nameof(viewportHeight));

            var sx = +2f * (1f / viewportWidth);
            var sy = -2f * (1f / viewportHeight);

            result = new Matrix(
                sx, 0f, 0f, -1f,
                0f, sy, 0f, 1f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="x">The amount to translate along the x-axis.</param>
        /// <param name="y">The amount to translate along the y-axis.</param>
        /// <param name="z">The amount to translate along the z-axis.</param>
        /// <returns>The translation <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateTranslation(Single x, Single y, Single z)
        {
            return new Matrix(
                1f, 0f, 0f, x,
                0f, 1f, 0f, y,
                0f, 0f, 1f, z,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="x">The amount to translate along the x-axis.</param>
        /// <param name="y">The amount to translate along the y-axis.</param>
        /// <param name="z">The amount to translate along the z-axis.</param>
        /// <param name="result">The translation <see cref="Matrix"/> that was created.</param>
        public static void CreateTranslation(Single x, Single y, Single z, out Matrix result)
        {
            result = new Matrix(
                1f, 0f, 0f, x,
                0f, 1f, 0f, y,
                0f, 0f, 1f, z,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="position">A vector describing the amount to translate along each axis.</param>
        /// <returns>The translation <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateTranslation(Vector3 position)
        {
            return new Matrix(
                1f, 0f, 0f, position.X,
                0f, 1f, 0f, position.Y,
                0f, 0f, 1f, position.Z,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="position">A vector describing the amount to translate along each axis.</param>
        /// <param name="result">The translation <see cref="Matrix"/> that was created.</param>
        public static void CreateTranslation(ref Vector3 position, out Matrix result)
        {
            result = new Matrix(
                1f, 0f, 0f, position.X,
                0f, 1f, 0f, position.Y,
                0f, 0f, 1f, position.Z,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the x-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <returns>The rotation <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateRotationX(Single radians)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);

            return new Matrix(
                1f,  0f,   0f, 0f,
                0f, cos, -sin, 0f,
                0f, sin,  cos, 0f,
                0f,  0f,   0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the x-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <param name="result">The rotation <see cref="Matrix"/> that was created.</param>
        public static void CreateRotationX(Single radians, out Matrix result)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);

            result = new Matrix(
                1f, 0f, 0f, 0f,
                0f, cos, -sin, 0f,
                0f, sin, cos, 0f,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the y-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <returns>The rotation <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateRotationY(Single radians)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);

            return new Matrix(
                 cos, 0f, sin, 0f,
                  0f, 1f, 0f, 0f,
                -sin, 0f, cos, 0f,
                  0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the y-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <param name="result">The rotation <see cref="Matrix"/> that was created.</param>
        public static void CreateRotationY(Single radians, out Matrix result)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);

            result = new Matrix(
                 cos, 0f, sin, 0f,
                  0f, 1f, 0f, 0f,
                -sin, 0f, cos, 0f,
                  0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the z-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <returns>The rotation <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateRotationZ(Single radians)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);
            return new Matrix(
                cos, -sin, 0f, 0f,
                sin, cos, 0f, 0f,
                 0f, 0f, 1f, 0f,
                 0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation over the z-axis.
        /// </summary>
        /// <param name="radians">The number of radians to rotate.</param>
        /// <param name="result">The rotation <see cref="Matrix"/> that was created.</param>
        public static void CreateRotationZ(Single radians, out Matrix result)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);
            result = new Matrix(
                cos, -sin, 0f, 0f,
                sin, cos, 0f, 0f,
                 0f, 0f, 1f, 0f,
                 0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation around the specified axis.
        /// </summary>
        /// <param name="axis">The axis around which to rotate.</param>
        /// <param name="angle">The angle to rotate, specified in radians.</param>
        /// <returns>The <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateFromAxisAngle(Vector3 axis, Single angle)
        {
            var c = Math.Cos(angle);
            var s = Math.Sin(angle);
            var C = 1 - c;

            var xx = axis.X * axis.X;
            var xy = axis.X * axis.Y;
            var xz = axis.X * axis.Z;
            var yy = axis.Y * axis.Y;
            var yz = axis.Y * axis.Z;
            var zz = axis.Z * axis.Z;

            var M11 = (float)(xx * C + c);
            var M12 = (float)(xy * C - (axis.Z * s));
            var M13 = (float)(xz * C + (axis.Y * s));
            var M21 = (float)(xy * C + (axis.Z * s));
            var M22 = (float)(yy * C + c);
            var M23 = (float)(yz * C + (axis.X * s));
            var M31 = (float)(xz * C - (axis.Y * s));
            var M32 = (float)(yz * C + (axis.X * s));
            var M33 = (float)(zz * C + c);

            return new Matrix(
                M11, M12, M13, 0,
                M21, M22, M23, 0,
                M31, M32, M33, 0,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a matrix that represents a rotation around the specified axis.
        /// </summary>
        /// <param name="axis">The axis around which to rotate.</param>
        /// <param name="angle">The angle to rotate, specified in radians.</param>
        /// <param name="result">The <see cref="Matrix"/> that was created.</param>
        public static void CreateFromAxisAngle(ref Vector3 axis, Single angle, out Matrix result)
        {
            var c = Math.Cos(angle);
            var s = Math.Sin(angle);
            var C = 1 - c;

            var xx = axis.X * axis.X;
            var xy = axis.X * axis.Y;
            var xz = axis.X * axis.Z;
            var yy = axis.Y * axis.Y;
            var yz = axis.Y * axis.Z;
            var zz = axis.Z * axis.Z;

            var M11 = (float)(xx * C + c);
            var M12 = (float)(xy * C - (axis.Z * s));
            var M13 = (float)(xz * C + (axis.Y * s));
            var M21 = (float)(xy * C + (axis.Z * s));
            var M22 = (float)(yy * C + c);
            var M23 = (float)(yz * C + (axis.X * s));
            var M31 = (float)(xz * C - (axis.Y * s));
            var M32 = (float)(yz * C + (axis.X * s));
            var M33 = (float)(zz * C + c);

            result = new Matrix(
                M11, M12, M13, 0,
                M21, M22, M23, 0,
                M31, M32, M33, 0,
                  0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scale">The scaling factor.</param>
        /// <returns>The scaling <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateScale(Single scale)
        {
            return new Matrix(
                scale, 0, 0, 0,
                0, scale, 0, 0,
                0, 0, scale, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scale">The scaling factor.</param>
        /// <param name="result">The scaling <see cref="Matrix"/> that was created.</param>
        public static void CreateScale(Single scale, out Matrix result)
        {
            result = new Matrix(
                scale, 0, 0, 0,
                0, scale, 0, 0,
                0, 0, scale, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scaleX">The scaling factor along the x-axis.</param>
        /// <param name="scaleY">The scaling factor along the y-axis.</param>
        /// <param name="scaleZ">The scaling factor along the z-axis.</param>
        /// <returns>The scaling <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateScale(Single scaleX, Single scaleY, Single scaleZ)
        {
            return new Matrix(
                scaleX, 0, 0, 0,
                0, scaleY, 0, 0,
                0, 0, scaleZ, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scaleX">The scaling factor along the x-axis.</param>
        /// <param name="scaleY">The scaling factor along the y-axis.</param>
        /// <param name="scaleZ">The scaling factor along the z-axis.</param>
        /// <param name="result">The scaling <see cref="Matrix"/> that was created.</param>
        public static void CreateScale(Single scaleX, Single scaleY, Single scaleZ, out Matrix result)
        {
            result = new Matrix(
                scaleX, 0, 0, 0,
                0, scaleY, 0, 0,
                0, 0, scaleZ, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="scale">A vector describing the scaling factor along each axis.</param>
        /// <returns>The scaling <see cref="Matrix"/> that was created.</returns>
        public static Matrix CreateScale(Vector3 scale)
        {
            return new Matrix(
                scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// <param name="v">A vector describing the scaling factor along each axis.</param>
        /// <param name="result">The scaling <see cref="Matrix"/> that was created.</param>
        public static void CreateScale(ref Vector3 v, out Matrix result)
        {
            result = new Matrix(
                v.X, 0f, 0f, 0f,
                 0f, v.Y, 0f, 0f,
                 0f, 0f, v.Z, 0f,
                 0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// Concatenates two matrices. The operation specified by the matrix on the left is applied first; the operation specified by the matrix 
        /// on the right is applied second. This is in contrast to the <see cref="Multiply(ref Matrix, ref Matrix, out Matrix)"/> method, which applies the 
        /// operand on the right first, and the operand on the left second.
        /// </summary>
        /// <param name="m1">The first matrix to concatenate.</param>
        /// <param name="m2">The second matrix to concatenate.</param>
        /// <param name="result">A matrix which is the result of concatenating the specified matrices.</param>
        public static void Concat(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            Multiply(ref m2, ref m1, out result);
        }

        /// <summary>
        /// Concatenates two matrices. The operation specified by the matrix on the left is applied first; the operation specified by the matrix 
        /// on the right is applied second. This is in contrast to the <see cref="Multiply(ref Matrix, ref Matrix, out Matrix)"/> method, which applies the 
        /// operand on the right first, and the operand on the left second.
        /// </summary>
        /// <param name="m1">The first matrix to concatenate.</param>
        /// <param name="m2">The second matrix to concatenate.</param>
        /// <returns>A matrix which is the result of concatenating the specified matrices.</returns>
        public static Matrix Concat(Matrix m1, Matrix m2)
        {
            return m2 * m1;
        }

        /// <summary>
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the addition operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Add(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result = new Matrix(
                m1.M11 + m2.M11,
                m1.M12 + m2.M12,
                m1.M13 + m2.M13,
                m1.M14 + m2.M14,
                m1.M21 + m2.M21,
                m1.M22 + m2.M22,
                m1.M23 + m2.M23,
                m1.M24 + m2.M24,
                m1.M31 + m2.M31,
                m1.M32 + m2.M32,
                m1.M33 + m2.M33,
                m1.M34 + m2.M34,
                m1.M41 + m2.M41,
                m1.M42 + m2.M42,
                m1.M43 + m2.M43,
                m1.M44 + m2.M44
            );
        }

        /// <summary>
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the addition operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the addition operator.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Add(Matrix m1, Matrix m2)
        {
            return new Matrix(
                m1.M11 + m2.M11,
                m1.M12 + m2.M12,
                m1.M13 + m2.M13,
                m1.M14 + m2.M14,
                m1.M21 + m2.M21,
                m1.M22 + m2.M22,
                m1.M23 + m2.M23,
                m1.M24 + m2.M24,
                m1.M31 + m2.M31,
                m1.M32 + m2.M32,
                m1.M33 + m2.M33,
                m1.M34 + m2.M34,
                m1.M41 + m2.M41,
                m1.M42 + m2.M42,
                m1.M43 + m2.M43,
                m1.M44 + m2.M44
            );
        }

        /// <summary>
        /// Subtracts a matrix from another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the subtraction operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the subtraction operator.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Subtract(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result = new Matrix(
                m1.M11 - m2.M11,
                m1.M12 - m2.M12,
                m1.M13 - m2.M13,
                m1.M14 - m2.M14,
                m1.M21 - m2.M21,
                m1.M22 - m2.M22,
                m1.M23 - m2.M23,
                m1.M24 - m2.M24,
                m1.M31 - m2.M31,
                m1.M32 - m2.M32,
                m1.M33 - m2.M33,
                m1.M34 - m2.M34,
                m1.M41 - m2.M41,
                m1.M42 - m2.M42,
                m1.M43 - m2.M43,
                m1.M44 - m2.M44
            );
        }

        /// <summary>
        /// Subtracts a matrix from another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the subtraction operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the subtraction operator.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Subtract(Matrix m1, Matrix m2)
        {
            return new Matrix(
                m1.M11 - m2.M11,
                m1.M12 - m2.M12,
                m1.M13 - m2.M13,
                m1.M14 - m2.M14,
                m1.M21 - m2.M21,
                m1.M22 - m2.M22,
                m1.M23 - m2.M23,
                m1.M24 - m2.M24,
                m1.M31 - m2.M31,
                m1.M32 - m2.M32,
                m1.M33 - m2.M33,
                m1.M34 - m2.M34,
                m1.M41 - m2.M41,
                m1.M42 - m2.M42,
                m1.M43 - m2.M43,
                m1.M44 - m2.M44
            );
        }

        /// <summary>
        /// Multiplies a matrix by a scaling factor.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Multiply(ref Matrix multiplicand, Single multiplier, out Matrix result)
        {
            result = new Matrix(
                multiplicand.M11 * multiplier,
                multiplicand.M12 * multiplier,
                multiplicand.M13 * multiplier,
                multiplicand.M14 * multiplier,
                multiplicand.M21 * multiplier,
                multiplicand.M22 * multiplier,
                multiplicand.M23 * multiplier,
                multiplicand.M24 * multiplier,
                multiplicand.M31 * multiplier,
                multiplicand.M32 * multiplier,
                multiplicand.M33 * multiplier,
                multiplicand.M34 * multiplier,
                multiplicand.M41 * multiplier,
                multiplicand.M42 * multiplier,
                multiplicand.M43 * multiplier,
                multiplicand.M44 * multiplier
            );
        }

        /// <summary>
        /// Multiplies a matrix by a scaling factor.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Multiply(Matrix multiplicand, Single multiplier)
        {
            return new Matrix(
                multiplicand.M11 * multiplier,
                multiplicand.M12 * multiplier,
                multiplicand.M13 * multiplier,
                multiplicand.M14 * multiplier,
                multiplicand.M21 * multiplier,
                multiplicand.M22 * multiplier,
                multiplicand.M23 * multiplier,
                multiplicand.M24 * multiplier,
                multiplicand.M31 * multiplier,
                multiplicand.M32 * multiplier,
                multiplicand.M33 * multiplier,
                multiplicand.M34 * multiplier,
                multiplicand.M41 * multiplier,
                multiplicand.M42 * multiplier,
                multiplicand.M43 * multiplier,
                multiplicand.M44 * multiplier
            );
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Multiply(ref Matrix multiplicand, ref Matrix multiplier, out Matrix result)
        {
            float M11 = (multiplicand.M11 * multiplier.M11 + multiplicand.M12 * multiplier.M21 + multiplicand.M13 * multiplier.M31 + multiplicand.M14 * multiplier.M41);
            float M12 = (multiplicand.M11 * multiplier.M12 + multiplicand.M12 * multiplier.M22 + multiplicand.M13 * multiplier.M32 + multiplicand.M14 * multiplier.M42);
            float M13 = (multiplicand.M11 * multiplier.M13 + multiplicand.M12 * multiplier.M23 + multiplicand.M13 * multiplier.M33 + multiplicand.M14 * multiplier.M43);
            float M14 = (multiplicand.M11 * multiplier.M14 + multiplicand.M12 * multiplier.M24 + multiplicand.M13 * multiplier.M34 + multiplicand.M14 * multiplier.M44);
            float M21 = (multiplicand.M21 * multiplier.M11 + multiplicand.M22 * multiplier.M21 + multiplicand.M23 * multiplier.M31 + multiplicand.M24 * multiplier.M41);
            float M22 = (multiplicand.M21 * multiplier.M12 + multiplicand.M22 * multiplier.M22 + multiplicand.M23 * multiplier.M32 + multiplicand.M24 * multiplier.M42);
            float M23 = (multiplicand.M21 * multiplier.M13 + multiplicand.M22 * multiplier.M23 + multiplicand.M23 * multiplier.M33 + multiplicand.M24 * multiplier.M43);
            float M24 = (multiplicand.M21 * multiplier.M14 + multiplicand.M22 * multiplier.M24 + multiplicand.M23 * multiplier.M34 + multiplicand.M24 * multiplier.M44);
            float M31 = (multiplicand.M31 * multiplier.M11 + multiplicand.M32 * multiplier.M21 + multiplicand.M33 * multiplier.M31 + multiplicand.M34 * multiplier.M41);
            float M32 = (multiplicand.M31 * multiplier.M12 + multiplicand.M32 * multiplier.M22 + multiplicand.M33 * multiplier.M32 + multiplicand.M34 * multiplier.M42);
            float M33 = (multiplicand.M31 * multiplier.M13 + multiplicand.M32 * multiplier.M23 + multiplicand.M33 * multiplier.M33 + multiplicand.M34 * multiplier.M43);
            float M34 = (multiplicand.M31 * multiplier.M14 + multiplicand.M32 * multiplier.M24 + multiplicand.M33 * multiplier.M34 + multiplicand.M34 * multiplier.M44);
            float M41 = (multiplicand.M41 * multiplier.M11 + multiplicand.M42 * multiplier.M21 + multiplicand.M43 * multiplier.M31 + multiplicand.M44 * multiplier.M41);
            float M42 = (multiplicand.M41 * multiplier.M12 + multiplicand.M42 * multiplier.M22 + multiplicand.M43 * multiplier.M32 + multiplicand.M44 * multiplier.M42);
            float M43 = (multiplicand.M41 * multiplier.M13 + multiplicand.M42 * multiplier.M23 + multiplicand.M43 * multiplier.M33 + multiplicand.M44 * multiplier.M43);
            float M44 = (multiplicand.M41 * multiplier.M14 + multiplicand.M42 * multiplier.M24 + multiplicand.M43 * multiplier.M34 + multiplicand.M44 * multiplier.M44);

            result = new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            );
        }

        /// <summary>
        /// Multiplies a matrix by another matrix.
        /// </summary>
        /// <param name="multiplicand">The multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Multiply(Matrix multiplicand, Matrix multiplier)
        {
            float M11 = (multiplicand.M11 * multiplier.M11 + multiplicand.M12 * multiplier.M21 + multiplicand.M13 * multiplier.M31 + multiplicand.M14 * multiplier.M41);
            float M12 = (multiplicand.M11 * multiplier.M12 + multiplicand.M12 * multiplier.M22 + multiplicand.M13 * multiplier.M32 + multiplicand.M14 * multiplier.M42);
            float M13 = (multiplicand.M11 * multiplier.M13 + multiplicand.M12 * multiplier.M23 + multiplicand.M13 * multiplier.M33 + multiplicand.M14 * multiplier.M43);
            float M14 = (multiplicand.M11 * multiplier.M14 + multiplicand.M12 * multiplier.M24 + multiplicand.M13 * multiplier.M34 + multiplicand.M14 * multiplier.M44);
            float M21 = (multiplicand.M21 * multiplier.M11 + multiplicand.M22 * multiplier.M21 + multiplicand.M23 * multiplier.M31 + multiplicand.M24 * multiplier.M41);
            float M22 = (multiplicand.M21 * multiplier.M12 + multiplicand.M22 * multiplier.M22 + multiplicand.M23 * multiplier.M32 + multiplicand.M24 * multiplier.M42);
            float M23 = (multiplicand.M21 * multiplier.M13 + multiplicand.M22 * multiplier.M23 + multiplicand.M23 * multiplier.M33 + multiplicand.M24 * multiplier.M43);
            float M24 = (multiplicand.M21 * multiplier.M14 + multiplicand.M22 * multiplier.M24 + multiplicand.M23 * multiplier.M34 + multiplicand.M24 * multiplier.M44);
            float M31 = (multiplicand.M31 * multiplier.M11 + multiplicand.M32 * multiplier.M21 + multiplicand.M33 * multiplier.M31 + multiplicand.M34 * multiplier.M41);
            float M32 = (multiplicand.M31 * multiplier.M12 + multiplicand.M32 * multiplier.M22 + multiplicand.M33 * multiplier.M32 + multiplicand.M34 * multiplier.M42);
            float M33 = (multiplicand.M31 * multiplier.M13 + multiplicand.M32 * multiplier.M23 + multiplicand.M33 * multiplier.M33 + multiplicand.M34 * multiplier.M43);
            float M34 = (multiplicand.M31 * multiplier.M14 + multiplicand.M32 * multiplier.M24 + multiplicand.M33 * multiplier.M34 + multiplicand.M34 * multiplier.M44);
            float M41 = (multiplicand.M41 * multiplier.M11 + multiplicand.M42 * multiplier.M21 + multiplicand.M43 * multiplier.M31 + multiplicand.M44 * multiplier.M41);
            float M42 = (multiplicand.M41 * multiplier.M12 + multiplicand.M42 * multiplier.M22 + multiplicand.M43 * multiplier.M32 + multiplicand.M44 * multiplier.M42);
            float M43 = (multiplicand.M41 * multiplier.M13 + multiplicand.M42 * multiplier.M23 + multiplicand.M43 * multiplier.M33 + multiplicand.M44 * multiplier.M43);
            float M44 = (multiplicand.M41 * multiplier.M14 + multiplicand.M42 * multiplier.M24 + multiplicand.M43 * multiplier.M34 + multiplicand.M44 * multiplier.M44);

            return new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            );
        }

        /// <summary>
        /// Divides a matrix by a scaling factor.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Divide(ref Matrix dividend, Single divisor, out Matrix result)
        {
            result = new Matrix(
                dividend.M11 / divisor,
                dividend.M12 / divisor,
                dividend.M13 / divisor,
                dividend.M14 / divisor,
                dividend.M21 / divisor,
                dividend.M22 / divisor,
                dividend.M23 / divisor,
                dividend.M24 / divisor,
                dividend.M31 / divisor,
                dividend.M32 / divisor,
                dividend.M33 / divisor,
                dividend.M34 / divisor,
                dividend.M41 / divisor,
                dividend.M42 / divisor,
                dividend.M43 / divisor,
                dividend.M44 / divisor
            );
        }

        /// <summary>
        /// Divides a matrix by a scaling factor.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Divide(Matrix dividend, Single divisor)
        {
            return new Matrix(
                dividend.M11 / divisor,
                dividend.M12 / divisor,
                dividend.M13 / divisor,
                dividend.M14 / divisor,
                dividend.M21 / divisor,
                dividend.M22 / divisor,
                dividend.M23 / divisor,
                dividend.M24 / divisor,
                dividend.M31 / divisor,
                dividend.M32 / divisor,
                dividend.M33 / divisor,
                dividend.M34 / divisor,
                dividend.M41 / divisor,
                dividend.M42 / divisor,
                dividend.M43 / divisor,
                dividend.M44 / divisor
            );
        }

        /// <summary>
        /// Divides a matrix by another matrix.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Divide(ref Matrix dividend, ref Matrix divisor, out Matrix result)
        {
            result = new Matrix(
                dividend.M11 / divisor.M11,
                dividend.M12 / divisor.M12,
                dividend.M13 / divisor.M13,
                dividend.M14 / divisor.M14,
                dividend.M21 / divisor.M21,
                dividend.M22 / divisor.M22,
                dividend.M23 / divisor.M23,
                dividend.M24 / divisor.M24,
                dividend.M31 / divisor.M31,
                dividend.M32 / divisor.M32,
                dividend.M33 / divisor.M33,
                dividend.M34 / divisor.M34,
                dividend.M41 / divisor.M41,
                dividend.M42 / divisor.M42,
                dividend.M43 / divisor.M43,
                dividend.M44 / divisor.M44
            );
        }

        /// <summary>
        /// Divides a matrix by another matrix.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix Divide(Matrix dividend, Matrix divisor)
        {
            return new Matrix(
                dividend.M11 / divisor.M11,
                dividend.M12 / divisor.M12,
                dividend.M13 / divisor.M13,
                dividend.M14 / divisor.M14,
                dividend.M21 / divisor.M21,
                dividend.M22 / divisor.M22,
                dividend.M23 / divisor.M23,
                dividend.M24 / divisor.M24,
                dividend.M31 / divisor.M31,
                dividend.M32 / divisor.M32,
                dividend.M33 / divisor.M33,
                dividend.M34 / divisor.M34,
                dividend.M41 / divisor.M41,
                dividend.M42 / divisor.M42,
                dividend.M43 / divisor.M43,
                dividend.M44 / divisor.M44
            );
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transpose.</param>
        /// <param name="result">The transposed <see cref="Matrix"/>.</param>
        public static void Transpose(ref Matrix matrix, out Matrix result)
        {
            result = new Matrix(
                matrix.M11, matrix.M21, matrix.M31, matrix.M41,
                matrix.M12, matrix.M22, matrix.M32, matrix.M42,
                matrix.M13, matrix.M23, matrix.M33, matrix.M43,
                matrix.M14, matrix.M24, matrix.M34, matrix.M44
            );
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transpose.</param>
        /// <returns>The transposed <see cref="Matrix"/>.</returns>
        public static Matrix Transpose(Matrix matrix)
        {
            return new Matrix(
                matrix.M11, matrix.M21, matrix.M31, matrix.M41,
                matrix.M12, matrix.M22, matrix.M32, matrix.M42,
                matrix.M13, matrix.M23, matrix.M33, matrix.M43,
                matrix.M14, matrix.M24, matrix.M34, matrix.M44
            );
        }

        /// <summary>
        /// Linearly interpolates between the specified matrices.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/> to interpolate.</param>
        /// <param name="matrix2">The second <see cref="Matrix"/> to interpolate.</param>
        /// <param name="amount">A value from 0.0 to 1.0 indicating the interpolation factor.</param>
        /// <param name="result">The interpolated <see cref="Matrix"/>.</param>
        public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, Single amount, out Matrix result)
        {
            var M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
            var M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
            var M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
            var M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;

            var M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
            var M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
            var M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
            var M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;

            var M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
            var M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
            var M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
            var M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;

            var M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
            var M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
            var M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
            var M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;

            result = new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44);
        }

        /// <summary>
        /// Linearly interpolates between the specified matrices.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/> to interpolate.</param>
        /// <param name="matrix2">The second <see cref="Matrix"/> to interpolate.</param>
        /// <param name="amount">A value from 0.0 to 1.0 indicating the interpolation factor.</param>
        /// <returns>The interpolated <see cref="Matrix"/>.</returns>
        public static Matrix Lerp(Matrix matrix1, Matrix matrix2, Single amount)
        {
            var M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
            var M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
            var M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
            var M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;

            var M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
            var M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
            var M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
            var M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;

            var M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
            var M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
            var M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
            var M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;

            var M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
            var M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
            var M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
            var M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;

            return new Matrix(
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <param name="result">The invertex <see cref="Matrix"/>.</param>
        public static void Invert(ref Matrix matrix, out Matrix result)
        {
            var s0 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
            var s1 = matrix.M11 * matrix.M23 - matrix.M21 * matrix.M13;
            var s2 = matrix.M11 * matrix.M24 - matrix.M21 * matrix.M14;
            var s3 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;
            var s4 = matrix.M12 * matrix.M24 - matrix.M22 * matrix.M14;
            var s5 = matrix.M13 * matrix.M24 - matrix.M23 * matrix.M14;

            var c5 = matrix.M33 * matrix.M44 - matrix.M43 * matrix.M34;
            var c4 = matrix.M32 * matrix.M44 - matrix.M42 * matrix.M34;
            var c3 = matrix.M32 * matrix.M43 - matrix.M42 * matrix.M33;
            var c2 = matrix.M31 * matrix.M44 - matrix.M41 * matrix.M34;
            var c1 = matrix.M31 * matrix.M43 - matrix.M41 * matrix.M33;
            var c0 = matrix.M31 * matrix.M42 - matrix.M41 * matrix.M32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
                throw new DivideByZeroException();

            var invdet = 1.0 / det;

            var M11 = (matrix.M22 * c5 - matrix.M23 * c4 + matrix.M24 * c3) * invdet;
            var M12 = (-matrix.M12 * c5 + matrix.M13 * c4 - matrix.M14 * c3) * invdet;
            var M13 = (matrix.M42 * s5 - matrix.M43 * s4 + matrix.M44 * s3) * invdet;
            var M14 = (-matrix.M32 * s5 + matrix.M33 * s4 - matrix.M34 * s3) * invdet;

            var M21 = (-matrix.M21 * c5 + matrix.M23 * c2 - matrix.M24 * c1) * invdet;
            var M22 = (matrix.M11 * c5 - matrix.M13 * c2 + matrix.M14 * c1) * invdet;
            var M23 = (-matrix.M41 * s5 + matrix.M43 * s2 - matrix.M44 * s1) * invdet;
            var M24 = (matrix.M31 * s5 - matrix.M33 * s2 + matrix.M34 * s1) * invdet;

            var M31 = (matrix.M21 * c4 - matrix.M22 * c2 + matrix.M24 * c0) * invdet;
            var M32 = (-matrix.M11 * c4 + matrix.M12 * c2 - matrix.M14 * c0) * invdet;
            var M33 = (matrix.M41 * s4 - matrix.M42 * s2 + matrix.M44 * s0) * invdet;
            var M34 = (-matrix.M31 * s4 + matrix.M32 * s2 - matrix.M34 * s0) * invdet;

            var M41  = (-matrix.M21 * c3 + matrix.M22 * c1 - matrix.M23 * c0) * invdet;
            var M42 = (matrix.M11 * c3 - matrix.M12 * c1 + matrix.M13 * c0) * invdet;
            var M43 = (-matrix.M41 * s3 + matrix.M42 * s1 - matrix.M43 * s0) * invdet;
            var M44 = (matrix.M31 * s3 - matrix.M32 * s1 + matrix.M33 * s0) * invdet;

            result = new Matrix(
                (float)M11, (float)M12, (float)M13, (float)M14,
                (float)M21, (float)M22, (float)M23, (float)M24,
                (float)M31, (float)M32, (float)M33, (float)M34,
                (float)M41, (float)M42, (float)M43, (float)M44);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <returns>The inverted <see cref="Matrix"/>.</returns>
        public static Matrix Invert(Matrix matrix)
        {
            var s0 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
            var s1 = matrix.M11 * matrix.M23 - matrix.M21 * matrix.M13;
            var s2 = matrix.M11 * matrix.M24 - matrix.M21 * matrix.M14;
            var s3 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;
            var s4 = matrix.M12 * matrix.M24 - matrix.M22 * matrix.M14;
            var s5 = matrix.M13 * matrix.M24 - matrix.M23 * matrix.M14;

            var c5 = matrix.M33 * matrix.M44 - matrix.M43 * matrix.M34;
            var c4 = matrix.M32 * matrix.M44 - matrix.M42 * matrix.M34;
            var c3 = matrix.M32 * matrix.M43 - matrix.M42 * matrix.M33;
            var c2 = matrix.M31 * matrix.M44 - matrix.M41 * matrix.M34;
            var c1 = matrix.M31 * matrix.M43 - matrix.M41 * matrix.M33;
            var c0 = matrix.M31 * matrix.M42 - matrix.M41 * matrix.M32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
                throw new DivideByZeroException();

            var invdet = 1.0 / det;

            var M11 = (matrix.M22 * c5 - matrix.M23 * c4 + matrix.M24 * c3) * invdet;
            var M12 = (-matrix.M12 * c5 + matrix.M13 * c4 - matrix.M14 * c3) * invdet;
            var M13 = (matrix.M42 * s5 - matrix.M43 * s4 + matrix.M44 * s3) * invdet;
            var M14 = (-matrix.M32 * s5 + matrix.M33 * s4 - matrix.M34 * s3) * invdet;

            var M21 = (-matrix.M21 * c5 + matrix.M23 * c2 - matrix.M24 * c1) * invdet;
            var M22 = (matrix.M11 * c5 - matrix.M13 * c2 + matrix.M14 * c1) * invdet;
            var M23 = (-matrix.M41 * s5 + matrix.M43 * s2 - matrix.M44 * s1) * invdet;
            var M24 = (matrix.M31 * s5 - matrix.M33 * s2 + matrix.M34 * s1) * invdet;

            var M31 = (matrix.M21 * c4 - matrix.M22 * c2 + matrix.M24 * c0) * invdet;
            var M32 = (-matrix.M11 * c4 + matrix.M12 * c2 - matrix.M14 * c0) * invdet;
            var M33 = (matrix.M41 * s4 - matrix.M42 * s2 + matrix.M44 * s0) * invdet;
            var M34 = (-matrix.M31 * s4 + matrix.M32 * s2 - matrix.M34 * s0) * invdet;

            var M41  = (-matrix.M21 * c3 + matrix.M22 * c1 - matrix.M23 * c0) * invdet;
            var M42 = (matrix.M11 * c3 - matrix.M12 * c1 + matrix.M13 * c0) * invdet;
            var M43 = (-matrix.M41 * s3 + matrix.M42 * s1 - matrix.M43 * s0) * invdet;
            var M44 = (matrix.M31 * s3 - matrix.M32 * s1 + matrix.M33 * s0) * invdet;

            return new Matrix(
                (float)M11, (float)M12, (float)M13, (float)M14,
                (float)M21, (float)M22, (float)M23, (float)M24,
                (float)M31, (float)M32, (float)M33, (float)M34,
                (float)M41, (float)M42, (float)M43, (float)M44);
        }

        /// <summary>
        /// Attempts to calculate the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <param name="result">The inverted <see cref="Matrix"/>.</param>
        /// <returns><see langword="true"/> if the matrix was inverted; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryInvert(Matrix matrix, out Matrix result)
        {
            var s0 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
            var s1 = matrix.M11 * matrix.M23 - matrix.M21 * matrix.M13;
            var s2 = matrix.M11 * matrix.M24 - matrix.M21 * matrix.M14;
            var s3 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;
            var s4 = matrix.M12 * matrix.M24 - matrix.M22 * matrix.M14;
            var s5 = matrix.M13 * matrix.M24 - matrix.M23 * matrix.M14;

            var c5 = matrix.M33 * matrix.M44 - matrix.M43 * matrix.M34;
            var c4 = matrix.M32 * matrix.M44 - matrix.M42 * matrix.M34;
            var c3 = matrix.M32 * matrix.M43 - matrix.M42 * matrix.M33;
            var c2 = matrix.M31 * matrix.M44 - matrix.M41 * matrix.M34;
            var c1 = matrix.M31 * matrix.M43 - matrix.M41 * matrix.M33;
            var c0 = matrix.M31 * matrix.M42 - matrix.M41 * matrix.M32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
            {
                result = Matrix.Identity;
                return false;
            }

            var invdet = 1.0 / det;

            var M11 = (matrix.M22 * c5 - matrix.M23 * c4 + matrix.M24 * c3) * invdet;
            var M12 = (-matrix.M12 * c5 + matrix.M13 * c4 - matrix.M14 * c3) * invdet;
            var M13 = (matrix.M42 * s5 - matrix.M43 * s4 + matrix.M44 * s3) * invdet;
            var M14 = (-matrix.M32 * s5 + matrix.M33 * s4 - matrix.M34 * s3) * invdet;

            var M21 = (-matrix.M21 * c5 + matrix.M23 * c2 - matrix.M24 * c1) * invdet;
            var M22 = (matrix.M11 * c5 - matrix.M13 * c2 + matrix.M14 * c1) * invdet;
            var M23 = (-matrix.M41 * s5 + matrix.M43 * s2 - matrix.M44 * s1) * invdet;
            var M24 = (matrix.M31 * s5 - matrix.M33 * s2 + matrix.M34 * s1) * invdet;

            var M31 = (matrix.M21 * c4 - matrix.M22 * c2 + matrix.M24 * c0) * invdet;
            var M32 = (-matrix.M11 * c4 + matrix.M12 * c2 - matrix.M14 * c0) * invdet;
            var M33 = (matrix.M41 * s4 - matrix.M42 * s2 + matrix.M44 * s0) * invdet;
            var M34 = (-matrix.M31 * s4 + matrix.M32 * s2 - matrix.M34 * s0) * invdet;

            var M41  = (-matrix.M21 * c3 + matrix.M22 * c1 - matrix.M23 * c0) * invdet;
            var M42 = (matrix.M11 * c3 - matrix.M12 * c1 + matrix.M13 * c0) * invdet;
            var M43 = (-matrix.M41 * s3 + matrix.M42 * s1 - matrix.M43 * s0) * invdet;
            var M44 = (matrix.M31 * s3 - matrix.M32 * s1 + matrix.M33 * s0) * invdet;

            result = new Matrix(
                (float)M11, (float)M12, (float)M13, (float)M14,
                (float)M21, (float)M22, (float)M23, (float)M24,
                (float)M31, (float)M32, (float)M33, (float)M34,
                (float)M41, (float)M42, (float)M43, (float)M44);

            return true;
        }

        /// <summary>
        /// Attempts to calculate the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <param name="result">The inverted <see cref="Matrix"/>.</param>
        /// <returns><see langword="true"/> if the matrix was inverted; otherwise, <see langword="false"/>.</returns>
        public static Boolean TryInvertRef(ref Matrix matrix, out Matrix result)
        {
            var s0 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
            var s1 = matrix.M11 * matrix.M23 - matrix.M21 * matrix.M13;
            var s2 = matrix.M11 * matrix.M24 - matrix.M21 * matrix.M14;
            var s3 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;
            var s4 = matrix.M12 * matrix.M24 - matrix.M22 * matrix.M14;
            var s5 = matrix.M13 * matrix.M24 - matrix.M23 * matrix.M14;

            var c5 = matrix.M33 * matrix.M44 - matrix.M43 * matrix.M34;
            var c4 = matrix.M32 * matrix.M44 - matrix.M42 * matrix.M34;
            var c3 = matrix.M32 * matrix.M43 - matrix.M42 * matrix.M33;
            var c2 = matrix.M31 * matrix.M44 - matrix.M41 * matrix.M34;
            var c1 = matrix.M31 * matrix.M43 - matrix.M41 * matrix.M33;
            var c0 = matrix.M31 * matrix.M42 - matrix.M41 * matrix.M32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
            {
                result = Matrix.Identity;
                return false;
            }

            var invdet = 1.0 / det;

            var M11 = (matrix.M22 * c5 - matrix.M23 * c4 + matrix.M24 * c3) * invdet;
            var M12 = (-matrix.M12 * c5 + matrix.M13 * c4 - matrix.M14 * c3) * invdet;
            var M13 = (matrix.M42 * s5 - matrix.M43 * s4 + matrix.M44 * s3) * invdet;
            var M14 = (-matrix.M32 * s5 + matrix.M33 * s4 - matrix.M34 * s3) * invdet;

            var M21 = (-matrix.M21 * c5 + matrix.M23 * c2 - matrix.M24 * c1) * invdet;
            var M22 = (matrix.M11 * c5 - matrix.M13 * c2 + matrix.M14 * c1) * invdet;
            var M23 = (-matrix.M41 * s5 + matrix.M43 * s2 - matrix.M44 * s1) * invdet;
            var M24 = (matrix.M31 * s5 - matrix.M33 * s2 + matrix.M34 * s1) * invdet;

            var M31 = (matrix.M21 * c4 - matrix.M22 * c2 + matrix.M24 * c0) * invdet;
            var M32 = (-matrix.M11 * c4 + matrix.M12 * c2 - matrix.M14 * c0) * invdet;
            var M33 = (matrix.M41 * s4 - matrix.M42 * s2 + matrix.M44 * s0) * invdet;
            var M34 = (-matrix.M31 * s4 + matrix.M32 * s2 - matrix.M34 * s0) * invdet;

            var M41  = (-matrix.M21 * c3 + matrix.M22 * c1 - matrix.M23 * c0) * invdet;
            var M42 = (matrix.M11 * c3 - matrix.M12 * c1 + matrix.M13 * c0) * invdet;
            var M43 = (-matrix.M41 * s3 + matrix.M42 * s1 - matrix.M43 * s0) * invdet;
            var M44 = (matrix.M31 * s3 - matrix.M32 * s1 + matrix.M33 * s0) * invdet;

            result = new Matrix(
                (float)M11, (float)M12, (float)M13, (float)M14,
                (float)M21, (float)M22, (float)M23, (float)M24,
                (float)M31, (float)M32, (float)M33, (float)M34,
                (float)M41, (float)M42, (float)M43, (float)M44);

            return true;
        }

        /// <summary>
        /// Negates the specified matrix's elements.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to negate.</param>
        /// <param name="result">The negated <see cref="Matrix"/>.</param>
        public static void Negate(ref Matrix matrix, out Matrix result)
        {
            result = new Matrix(
                -matrix.M11, -matrix.M12, -matrix.M13, -matrix.M14,
                -matrix.M21, -matrix.M22, -matrix.M23, -matrix.M24,
                -matrix.M31, -matrix.M32, -matrix.M33, -matrix.M34,
                -matrix.M41, -matrix.M42, -matrix.M43, -matrix.M44
            );
        }

        /// <summary>
        /// Negates the specified matrix's elements.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to negate.</param>
        /// <returns>The negated <see cref="Matrix"/>.</returns>
        public static Matrix Negate(Matrix matrix)
        {
            return new Matrix(
                -matrix.M11, -matrix.M12, -matrix.M13, -matrix.M14,
                -matrix.M21, -matrix.M22, -matrix.M23, -matrix.M24,
                -matrix.M31, -matrix.M32, -matrix.M33, -matrix.M34,
                -matrix.M41, -matrix.M42, -matrix.M43, -matrix.M44
            );
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
                hash = hash * 23 + M11.GetHashCode();
                hash = hash * 23 + M12.GetHashCode();
                hash = hash * 23 + M13.GetHashCode();
                hash = hash * 23 + M14.GetHashCode();
                hash = hash * 23 + M21.GetHashCode();
                hash = hash * 23 + M22.GetHashCode();
                hash = hash * 23 + M23.GetHashCode();
                hash = hash * 23 + M24.GetHashCode();
                hash = hash * 23 + M31.GetHashCode();
                hash = hash * 23 + M32.GetHashCode();
                hash = hash * 23 + M33.GetHashCode();
                hash = hash * 23 + M34.GetHashCode();
                hash = hash * 23 + M41.GetHashCode();
                hash = hash * 23 + M42.GetHashCode();
                hash = hash * 23 + M43.GetHashCode();
                hash = hash * 23 + M44.GetHashCode();
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
            return String.Format(provider, "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}",
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            if (!(obj is Matrix))
                return false;
            return Equals((Matrix)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(Matrix other)
        {
            return
                M11 == other.M11 &&
                M12 == other.M12 &&
                M13 == other.M13 &&
                M14 == other.M14 &&
                M21 == other.M21 &&
                M22 == other.M22 &&
                M23 == other.M23 &&
                M24 == other.M24 &&
                M31 == other.M31 &&
                M32 == other.M32 &&
                M33 == other.M33 &&
                M34 == other.M34 &&
                M41 == other.M41 &&
                M42 == other.M42 &&
                M43 == other.M43 &&
                M44 == other.M44;
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean EqualsRef(ref Matrix other)
        {
            return
                M11 == other.M11 &&
                M12 == other.M12 &&
                M13 == other.M13 &&
                M14 == other.M14 &&
                M21 == other.M21 &&
                M22 == other.M22 &&
                M23 == other.M23 &&
                M24 == other.M24 &&
                M31 == other.M31 &&
                M32 == other.M32 &&
                M33 == other.M33 &&
                M34 == other.M34 &&
                M41 == other.M41 &&
                M42 == other.M42 &&
                M43 == other.M43 &&
                M44 == other.M44;
        }

        /// <summary>
        /// Calculates the matrix's determinant.
        /// </summary>
        /// <returns>The matrix's determinant.</returns>
        public Single Determinant()
        {
            var s0 = M11 * M22 - M21 * M12;
            var s1 = M11 * M23 - M21 * M13;
            var s2 = M11 * M24 - M21 * M14;
            var s3 = M12 * M23 - M22 * M13;
            var s4 = M12 * M24 - M22 * M14;
            var s5 = M13 * M24 - M23 * M14;

            var c5 = M33 * M44 - M43 * M34;
            var c4 = M32 * M44 - M42 * M34;
            var c3 = M32 * M43 - M42 * M33;
            var c2 = M31 * M44 - M41 * M34;
            var c1 = M31 * M43 - M41 * M33;
            var c0 = M31 * M42 - M41 * M32;

            return (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        [Preserve]
        public Matrix Interpolate(Matrix target, Single t)
        {
            Matrix result;
            Matrix.Lerp(ref this, ref target, t, out result);
            return result;
        }

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        public static Matrix Identity
        {
            get
            {
                return new Matrix(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1);
            }
        }

        /// <summary>
        /// Gets the matrix's right vector.
        /// </summary>
        public Vector3 Right { get { return new Vector3(M11, M21, M31); } }

        /// <summary>
        /// Gets the matrix's left vector.
        /// </summary>
        public Vector3 Left { get { return new Vector3(-M11, -M21, -M31); } }

        /// <summary>
        /// Gets the matrix's up vector.
        /// </summary>
        public Vector3 Up { get { return new Vector3(M12, M22, M32); } }

        /// <summary>
        /// Gets the matrix's down vector.
        /// </summary>
        public Vector3 Down { get { return new Vector3(-M12, -M22, -M32); } }

        /// <summary>
        /// Gets the matrix's backwards vector.
        /// </summary>
        public Vector3 Backward { get { return new Vector3(M13, M23, M33); } }

        /// <summary>
        /// Gets the matrix's forwards vector.
        /// </summary>
        public Vector3 Forward { get { return new Vector3(-M13, -M23, -M33); } }

        /// <summary>
        /// Gets the matrix's translation vector.
        /// </summary>
        public Vector3 Translation { get { return new Vector3(M14, M24, M34); } }
        
        /// <summary>
        /// Gets the value at row 1, column 1 of the matrix.
        /// </summary>
        public Single M11;

        /// <summary>
        /// Gets the value at row 1, column 2 of the matrix.
        /// </summary>
        public Single M12;

        /// <summary>
        /// Gets the value at row 1, column 3 of the matrix.
        /// </summary>
        public Single M13;

        /// <summary>
        /// Gets the value at row 1, column 4 of the matrix.
        /// </summary>
        public Single M14;

        /// <summary>
        /// Gets the value at row 2, column 1 of the matrix.
        /// </summary>
        public Single M21;

        /// <summary>
        /// Gets the value at row 2, column 2 of the matrix.
        /// </summary>
        public Single M22;

        /// <summary>
        /// Gets the value at row 2, column 3 of the matrix.
        /// </summary>
        public Single M23;

        /// <summary>
        /// Gets the value at row 2, column 4 of the matrix.
        /// </summary>
        public Single M24;

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M31;

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M32;

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M33;

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M34;

        /// <summary>
        /// Gets the value at row 4, column 1 of the matrix.
        /// </summary>
        public Single M41;

        /// <summary>
        /// Gets the value at row 4, column 2 of the matrix.
        /// </summary>
        public Single M42;

        /// <summary>
        /// Gets the value at row 4, column 3 of the matrix.
        /// </summary>
        public Single M43;

        /// <summary>
        /// Gets the value at row 4, column 4 of the matrix.
        /// </summary>
        public Single M44;
    }
}
