using System;
using System.Diagnostics;
using System.Globalization;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a 4x4 transformation matrix.
    /// </summary>
    [Serializable]
    [DebuggerDisplay(@"\{ \{M11:{M11} M12:{M12} M13:{M13} M14:{M14}\} \{M21:{M21} M22:{M22} M23:{M23} M24:{M24}\} \{M31:{M31} M32:{M32} M33:{M33} M34:{M34}\} \{M41:{M41} M42:{M42} M43:{M43} M44:{M44}\} \}")]
    public struct Matrix : IEquatable<Matrix>, IInterpolatable<Matrix>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="m11">The value at row 1, column 1 of the matrix.</param>
        /// <param name="m12">The value at row 1, column 2 of the matrix.</param>
        /// <param name="m13">The value at row 1, column 3 of the matrix.</param>
        /// <param name="m14">The value at row 1, column 4 of the matrix.</param>
        /// <param name="m21">The value at row 2, column 1 of the matrix.</param>
        /// <param name="m22">The value at row 2, column 2 of the matrix.</param>
        /// <param name="m23">The value at row 2, column 3 of the matrix.</param>
        /// <param name="m24">The value at row 2, column 4 of the matrix.</param>
        /// <param name="m31">The value at row 3, column 1 of the matrix.</param>
        /// <param name="m32">The value at row 3, column 2 of the matrix.</param>
        /// <param name="m33">The value at row 3, column 3 of the matrix.</param>
        /// <param name="m34">The value at row 3, column 4 of the matrix.</param>
        /// <param name="m41">The value at row 4, column 1 of the matrix.</param>
        /// <param name="m42">The value at row 4, column 2 of the matrix.</param>
        /// <param name="m43">The value at row 4, column 3 of the matrix.</param>
        /// <param name="m44">The value at row 4, column 4 of the matrix.</param>
        public Matrix(
            Single m11, Single m12, Single m13, Single m14,
            Single m21, Single m22, Single m23, Single m24,
            Single m31, Single m32, Single m33, Single m34,
            Single m41, Single m42, Single m43, Single m44)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m14 = m14;

            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m24 = m24;

            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            this.m34 = m34;

            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
            this.m44 = m44;
        }

        /// <summary>
        /// Compares two matrices for equality.
        /// </summary>
        /// <param name="m1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="m2">The second <see cref="Matrix"/> to compare.</param>
        /// <returns><c>true</c> if the specified matrices are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Matrix m1, Matrix m2)
        {
            return m1.Equals(m2);
        }

        /// <summary>
        /// Compares two matrices for inequality.
        /// </summary>
        /// <param name="m1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="m2">The second <see cref="Matrix"/> to compare.</param>
        /// <returns><c>true</c> if the specified matrices are unequal; otherwise, <c>false</c>.</returns>
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
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, out Matrix matrix)
        {
            return TryParse(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out matrix);
        }

        /// <summary>
        /// Converts the string representation of a matrix into an instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="s">A string containing a matrix to convert.</param>
        /// <returns>A instance of the <see cref="Matrix"/> structure equivalent to the matrix contained in <paramref name="s"/>.</returns>
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
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Matrix matrix)
        {
            matrix = default(Matrix);

            if (String.IsNullOrEmpty(s))
                return false;

            var components = s.Split(' ');
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

            var m11 = normalizedRight.X;
            var m21 = normalizedRight.Y;
            var m31 = normalizedRight.Z;
            var m41 = 0f;

            var m12 = normalizedUp.X;
            var m22 = normalizedUp.Y;
            var m32 = normalizedUp.Z;
            var m42 = 0f;

            var m13 = normalizedBackward.X;
            var m23 = normalizedBackward.Y;
            var m33 = normalizedBackward.Z;
            var m43 = 0f;

            var m14 = position.X;
            var m24 = position.Y;
            var m34 = position.Z;
            var m44 = 1f;

            return new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44
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

            var m11 = normalizedRight.X;
            var m21 = normalizedRight.Y;
            var m31 = normalizedRight.Z;
            var m41 = 0f;

            var m12 = normalizedUp.X;
            var m22 = normalizedUp.Y;
            var m32 = normalizedUp.Z;
            var m42 = 0f;

            var m13 = normalizedBackward.X;
            var m23 = normalizedBackward.Y;
            var m33 = normalizedBackward.Z;
            var m43 = 0f;

            var m14 = position.X;
            var m24 = position.Y;
            var m34 = position.Z;
            var m44 = 1f;

            result = new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44
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
            var m11 = (float)(2.0 / width);
            var m22 = (float)(2.0 / height);
            var m33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var m34 = (float)(zNearPlane / (zNearPlane - zFarPlane));

            return new Matrix(
                m11, 0, 0, 0,
                  0, m22, 0, 0,
                  0, 0, m33, m34,
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
            var m11 = (float)(2.0 / width);
            var m22 = (float)(2.0 / height);
            var m33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var m34 = (float)(zNearPlane / (zNearPlane - zFarPlane));

            result = new Matrix(
                m11, 0, 0, 0,
                  0, m22, 0, 0,
                  0, 0, m33, m34,
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
            var m11 = (float)(2.0 / (right - left));
            var m14 = (float)((left + right) / (left - right));
            var m22 = (float)(2.0 / (top - bottom));
            var m24 = (float)((top + bottom) / (bottom - top));
            var m33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var m34 = (float)(zNearPlane / (zNearPlane - zFarPlane));

            return new Matrix(
                m11, 0, 0, m14,
                  0, m22, 0, m24,
                  0, 0, m33, m34,
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
            var m11 = (float)(2.0 / (right - left));
            var m14 = (float)((left + right) / (left - right));
            var m22 = (float)(2.0 / (top - bottom));
            var m24 = (float)((top + bottom) / (bottom - top));
            var m33 = (float)(1.0 / (zNearPlane - zFarPlane));
            var m34 = (float)(zNearPlane / (zNearPlane - zFarPlane));

            result = new Matrix(
                m11, 0, 0, m14,
                  0, m22, 0, m24,
                  0, 0, m33, m34,
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
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var m11 = 2f * nearPlaneDistance / width;
            var m22 = 2f * nearPlaneDistance / height;
            var m33 = farPlaneDistance / nearmfar;
            var m34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var m43 = -1f;

            return new Matrix(
                m11, 0, 0, 0,
                0, m22, 0, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var m11 = 2f * nearPlaneDistance / width;
            var m22 = 2f * nearPlaneDistance / height;
            var m33 = farPlaneDistance / nearmfar;
            var m34 = nearPlaneDistance * farPlaneDistance / nearmfar;
            var m43 = -1f;

            result = new Matrix(
                m11, 0, 0, 0,
                0, m22, 0, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(fieldOfView > 0 && fieldOfView < Math.PI, "fieldOfView");
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var yScale = 1f / Math.Tan(fieldOfView * 0.5f);
            var xScale = yScale / aspectRatio;
            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var m11 = (float)xScale;
            var m22 = (float)yScale;
            var m33 = (float)(farPlaneDistance / nearmfar);
            var m34 = (float)(nearPlaneDistance * farPlaneDistance / nearmfar);
            var m43 = -1f;

            return new Matrix(
                m11, 0, 0, 0,
                0, m22, 0, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(fieldOfView > 0 && fieldOfView < Math.PI, "fieldOfView");
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var yScale = 1f / Math.Tan(fieldOfView * 0.5f);
            var xScale = yScale / aspectRatio;
            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var m11 = (float)xScale;
            var m22 = (float)yScale;
            var m33 = (float)(farPlaneDistance / nearmfar);
            var m34 = (float)(nearPlaneDistance * farPlaneDistance / nearmfar);
            var m43 = -1f;

            result = new Matrix(
                m11, 0, 0, 0,
                0, m22, 0, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var rpl = right + left;
            var rml = right - left;
            var tpb = top + bottom;
            var tmb = top - bottom;

            var m11 = (float)(2.0 * nearPlaneDistance / rml);
            var m13 = (float)(rpl / rml);
            var m22 = (float)(2.0 * nearPlaneDistance / tmb);
            var m23 = (float)(tpb / tmb);            
            var m33 = (float)(farPlaneDistance / nearmfar);
            var m34 = (float)(nearPlaneDistance * farPlaneDistance / nearmfar);
            var m43 = -1f;

            return new Matrix(
                m11, 0, m13, 0,
                0, m22, m23, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(nearPlaneDistance > 0, "nearPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > 0, "farPlaneDistance");
            Contract.EnsureRange(farPlaneDistance > nearPlaneDistance, "nearPlaneDistance");

            var nearmfar = nearPlaneDistance - farPlaneDistance;

            var rpl = right + left;
            var rml = right - left;
            var tpb = top + bottom;
            var tmb = top - bottom;

            var m11 = (float)(2.0 * nearPlaneDistance / rml);
            var m13 = (float)(rpl / rml);
            var m22 = (float)(2.0 * nearPlaneDistance / tmb);
            var m23 = (float)(tpb / tmb);
            var m33 = (float)(farPlaneDistance / nearmfar);
            var m34 = (float)(nearPlaneDistance * farPlaneDistance / nearmfar);
            var m43 = -1f;

            result = new Matrix(
                m11, 0, m13, 0,
                0, m22, m23, 0,
                0, 0, m33, m34,
                0, 0, m43, 0
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
            Contract.EnsureRange(viewportWidth > 0, "viewportWidth");
            Contract.EnsureRange(viewportHeight > 0, "viewportHeight");

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
            Contract.EnsureRange(viewportWidth > 0, "viewportWidth");
            Contract.EnsureRange(viewportHeight > 0, "viewportHeight");

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

            var m11 = (float)(xx * C + c);
            var m12 = (float)(xy * C - (axis.Z * s));
            var m13 = (float)(xz * C + (axis.Y * s));
            var m21 = (float)(xy * C + (axis.Z * s));
            var m22 = (float)(yy * C + c);
            var m23 = (float)(yz * C + (axis.X * s));
            var m31 = (float)(xz * C - (axis.Y * s));
            var m32 = (float)(yz * C + (axis.X * s));
            var m33 = (float)(zz * C + c);

            return new Matrix(
                m11, m12, m13, 0,
                m21, m22, m23, 0,
                m31, m32, m33, 0,
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

            var m11 = (float)(xx * C + c);
            var m12 = (float)(xy * C - (axis.Z * s));
            var m13 = (float)(xz * C + (axis.Y * s));
            var m21 = (float)(xy * C + (axis.Z * s));
            var m22 = (float)(yy * C + c);
            var m23 = (float)(yz * C + (axis.X * s));
            var m31 = (float)(xz * C - (axis.Y * s));
            var m32 = (float)(yz * C + (axis.X * s));
            var m33 = (float)(zz * C + c);

            result = new Matrix(
                m11, m12, m13, 0,
                m21, m22, m23, 0,
                m31, m32, m33, 0,
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
        /// Adds a matrix to another matrix.
        /// </summary>
        /// <param name="m1">The <see cref="Matrix"/> on the left side of the addition operator.</param>
        /// <param name="m2">The <see cref="Matrix"/> on the right side of the addition operator.</param>
        /// <param name="result">The resulting <see cref="Matrix"/>.</param>
        public static void Add(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result = new Matrix(
                m1.m11 + m2.m11,
                m1.m12 + m2.m12,
                m1.m13 + m2.m13,
                m1.m14 + m2.m14,
                m1.m21 + m2.m21,
                m1.m22 + m2.m22,
                m1.m23 + m2.m23,
                m1.m24 + m2.m24,
                m1.m31 + m2.m31,
                m1.m32 + m2.m32,
                m1.m33 + m2.m33,
                m1.m34 + m2.m34,
                m1.m41 + m2.m41,
                m1.m42 + m2.m42,
                m1.m43 + m2.m43,
                m1.m44 + m2.m44
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
                m1.m11 + m2.m11,
                m1.m12 + m2.m12,
                m1.m13 + m2.m13,
                m1.m14 + m2.m14,
                m1.m21 + m2.m21,
                m1.m22 + m2.m22,
                m1.m23 + m2.m23,
                m1.m24 + m2.m24,
                m1.m31 + m2.m31,
                m1.m32 + m2.m32,
                m1.m33 + m2.m33,
                m1.m34 + m2.m34,
                m1.m41 + m2.m41,
                m1.m42 + m2.m42,
                m1.m43 + m2.m43,
                m1.m44 + m2.m44
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
                m1.m11 - m2.m11,
                m1.m12 - m2.m12,
                m1.m13 - m2.m13,
                m1.m14 - m2.m14,
                m1.m21 - m2.m21,
                m1.m22 - m2.m22,
                m1.m23 - m2.m23,
                m1.m24 - m2.m24,
                m1.m31 - m2.m31,
                m1.m32 - m2.m32,
                m1.m33 - m2.m33,
                m1.m34 - m2.m34,
                m1.m41 - m2.m41,
                m1.m42 - m2.m42,
                m1.m43 - m2.m43,
                m1.m44 - m2.m44
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
                m1.m11 - m2.m11,
                m1.m12 - m2.m12,
                m1.m13 - m2.m13,
                m1.m14 - m2.m14,
                m1.m21 - m2.m21,
                m1.m22 - m2.m22,
                m1.m23 - m2.m23,
                m1.m24 - m2.m24,
                m1.m31 - m2.m31,
                m1.m32 - m2.m32,
                m1.m33 - m2.m33,
                m1.m34 - m2.m34,
                m1.m41 - m2.m41,
                m1.m42 - m2.m42,
                m1.m43 - m2.m43,
                m1.m44 - m2.m44
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
                multiplicand.m11 * multiplier,
                multiplicand.m12 * multiplier,
                multiplicand.m13 * multiplier,
                multiplicand.m14 * multiplier,
                multiplicand.m21 * multiplier,
                multiplicand.m22 * multiplier,
                multiplicand.m23 * multiplier,
                multiplicand.m24 * multiplier,
                multiplicand.m31 * multiplier,
                multiplicand.m32 * multiplier,
                multiplicand.m33 * multiplier,
                multiplicand.m34 * multiplier,
                multiplicand.m41 * multiplier,
                multiplicand.m42 * multiplier,
                multiplicand.m43 * multiplier,
                multiplicand.m44 * multiplier
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
                multiplicand.m11 * multiplier,
                multiplicand.m12 * multiplier,
                multiplicand.m13 * multiplier,
                multiplicand.m14 * multiplier,
                multiplicand.m21 * multiplier,
                multiplicand.m22 * multiplier,
                multiplicand.m23 * multiplier,
                multiplicand.m24 * multiplier,
                multiplicand.m31 * multiplier,
                multiplicand.m32 * multiplier,
                multiplicand.m33 * multiplier,
                multiplicand.m34 * multiplier,
                multiplicand.m41 * multiplier,
                multiplicand.m42 * multiplier,
                multiplicand.m43 * multiplier,
                multiplicand.m44 * multiplier
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
            float m11 = (multiplicand.M11 * multiplier.M11 + multiplicand.M12 * multiplier.M21 + multiplicand.M13 * multiplier.M31 + multiplicand.M14 * multiplier.M41);
            float m12 = (multiplicand.M11 * multiplier.M12 + multiplicand.M12 * multiplier.M22 + multiplicand.M13 * multiplier.M32 + multiplicand.M14 * multiplier.M42);
            float m13 = (multiplicand.M11 * multiplier.M13 + multiplicand.M12 * multiplier.M23 + multiplicand.M13 * multiplier.M33 + multiplicand.M14 * multiplier.M43);
            float m14 = (multiplicand.M11 * multiplier.M14 + multiplicand.M12 * multiplier.M24 + multiplicand.M13 * multiplier.M34 + multiplicand.M14 * multiplier.M44);
            float m21 = (multiplicand.M21 * multiplier.M11 + multiplicand.M22 * multiplier.M21 + multiplicand.M23 * multiplier.M31 + multiplicand.M24 * multiplier.M41);
            float m22 = (multiplicand.M21 * multiplier.M12 + multiplicand.M22 * multiplier.M22 + multiplicand.M23 * multiplier.M32 + multiplicand.M24 * multiplier.M42);
            float m23 = (multiplicand.M21 * multiplier.M13 + multiplicand.M22 * multiplier.M23 + multiplicand.M23 * multiplier.M33 + multiplicand.M24 * multiplier.M43);
            float m24 = (multiplicand.M21 * multiplier.M14 + multiplicand.M22 * multiplier.M24 + multiplicand.M23 * multiplier.M34 + multiplicand.M24 * multiplier.M44);
            float m31 = (multiplicand.M31 * multiplier.M11 + multiplicand.M32 * multiplier.M21 + multiplicand.M33 * multiplier.M31 + multiplicand.M34 * multiplier.M41);
            float m32 = (multiplicand.M31 * multiplier.M12 + multiplicand.M32 * multiplier.M22 + multiplicand.M33 * multiplier.M32 + multiplicand.M34 * multiplier.M42);
            float m33 = (multiplicand.M31 * multiplier.M13 + multiplicand.M32 * multiplier.M23 + multiplicand.M33 * multiplier.M33 + multiplicand.M34 * multiplier.M43);
            float m34 = (multiplicand.M31 * multiplier.M14 + multiplicand.M32 * multiplier.M24 + multiplicand.M33 * multiplier.M34 + multiplicand.M34 * multiplier.M44);
            float m41 = (multiplicand.M41 * multiplier.M11 + multiplicand.M42 * multiplier.M21 + multiplicand.M43 * multiplier.M31 + multiplicand.M44 * multiplier.M41);
            float m42 = (multiplicand.M41 * multiplier.M12 + multiplicand.M42 * multiplier.M22 + multiplicand.M43 * multiplier.M32 + multiplicand.M44 * multiplier.M42);
            float m43 = (multiplicand.M41 * multiplier.M13 + multiplicand.M42 * multiplier.M23 + multiplicand.M43 * multiplier.M33 + multiplicand.M44 * multiplier.M43);
            float m44 = (multiplicand.M41 * multiplier.M14 + multiplicand.M42 * multiplier.M24 + multiplicand.M43 * multiplier.M34 + multiplicand.M44 * multiplier.M44);

            result = new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44
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
            float m11 = (multiplicand.M11 * multiplier.M11 + multiplicand.M12 * multiplier.M21 + multiplicand.M13 * multiplier.M31 + multiplicand.M14 * multiplier.M41);
            float m12 = (multiplicand.M11 * multiplier.M12 + multiplicand.M12 * multiplier.M22 + multiplicand.M13 * multiplier.M32 + multiplicand.M14 * multiplier.M42);
            float m13 = (multiplicand.M11 * multiplier.M13 + multiplicand.M12 * multiplier.M23 + multiplicand.M13 * multiplier.M33 + multiplicand.M14 * multiplier.M43);
            float m14 = (multiplicand.M11 * multiplier.M14 + multiplicand.M12 * multiplier.M24 + multiplicand.M13 * multiplier.M34 + multiplicand.M14 * multiplier.M44);
            float m21 = (multiplicand.M21 * multiplier.M11 + multiplicand.M22 * multiplier.M21 + multiplicand.M23 * multiplier.M31 + multiplicand.M24 * multiplier.M41);
            float m22 = (multiplicand.M21 * multiplier.M12 + multiplicand.M22 * multiplier.M22 + multiplicand.M23 * multiplier.M32 + multiplicand.M24 * multiplier.M42);
            float m23 = (multiplicand.M21 * multiplier.M13 + multiplicand.M22 * multiplier.M23 + multiplicand.M23 * multiplier.M33 + multiplicand.M24 * multiplier.M43);
            float m24 = (multiplicand.M21 * multiplier.M14 + multiplicand.M22 * multiplier.M24 + multiplicand.M23 * multiplier.M34 + multiplicand.M24 * multiplier.M44);
            float m31 = (multiplicand.M31 * multiplier.M11 + multiplicand.M32 * multiplier.M21 + multiplicand.M33 * multiplier.M31 + multiplicand.M34 * multiplier.M41);
            float m32 = (multiplicand.M31 * multiplier.M12 + multiplicand.M32 * multiplier.M22 + multiplicand.M33 * multiplier.M32 + multiplicand.M34 * multiplier.M42);
            float m33 = (multiplicand.M31 * multiplier.M13 + multiplicand.M32 * multiplier.M23 + multiplicand.M33 * multiplier.M33 + multiplicand.M34 * multiplier.M43);
            float m34 = (multiplicand.M31 * multiplier.M14 + multiplicand.M32 * multiplier.M24 + multiplicand.M33 * multiplier.M34 + multiplicand.M34 * multiplier.M44);
            float m41 = (multiplicand.M41 * multiplier.M11 + multiplicand.M42 * multiplier.M21 + multiplicand.M43 * multiplier.M31 + multiplicand.M44 * multiplier.M41);
            float m42 = (multiplicand.M41 * multiplier.M12 + multiplicand.M42 * multiplier.M22 + multiplicand.M43 * multiplier.M32 + multiplicand.M44 * multiplier.M42);
            float m43 = (multiplicand.M41 * multiplier.M13 + multiplicand.M42 * multiplier.M23 + multiplicand.M43 * multiplier.M33 + multiplicand.M44 * multiplier.M43);
            float m44 = (multiplicand.M41 * multiplier.M14 + multiplicand.M42 * multiplier.M24 + multiplicand.M43 * multiplier.M34 + multiplicand.M44 * multiplier.M44);

            return new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44
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
                dividend.m11 / divisor,
                dividend.m12 / divisor,
                dividend.m13 / divisor,
                dividend.m14 / divisor,
                dividend.m21 / divisor,
                dividend.m22 / divisor,
                dividend.m23 / divisor,
                dividend.m24 / divisor,
                dividend.m31 / divisor,
                dividend.m32 / divisor,
                dividend.m33 / divisor,
                dividend.m34 / divisor,
                dividend.m41 / divisor,
                dividend.m42 / divisor,
                dividend.m43 / divisor,
                dividend.m44 / divisor
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
                dividend.m11 / divisor,
                dividend.m12 / divisor,
                dividend.m13 / divisor,
                dividend.m14 / divisor,
                dividend.m21 / divisor,
                dividend.m22 / divisor,
                dividend.m23 / divisor,
                dividend.m24 / divisor,
                dividend.m31 / divisor,
                dividend.m32 / divisor,
                dividend.m33 / divisor,
                dividend.m34 / divisor,
                dividend.m41 / divisor,
                dividend.m42 / divisor,
                dividend.m43 / divisor,
                dividend.m44 / divisor
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
                dividend.m11 / divisor.m11,
                dividend.m12 / divisor.m12,
                dividend.m13 / divisor.m13,
                dividend.m14 / divisor.m14,
                dividend.m21 / divisor.m21,
                dividend.m22 / divisor.m22,
                dividend.m23 / divisor.m23,
                dividend.m24 / divisor.m24,
                dividend.m31 / divisor.m31,
                dividend.m32 / divisor.m32,
                dividend.m33 / divisor.m33,
                dividend.m34 / divisor.m34,
                dividend.m41 / divisor.m41,
                dividend.m42 / divisor.m42,
                dividend.m43 / divisor.m43,
                dividend.m44 / divisor.m44
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
                dividend.m11 / divisor.m11,
                dividend.m12 / divisor.m12,
                dividend.m13 / divisor.m13,
                dividend.m14 / divisor.m14,
                dividend.m21 / divisor.m21,
                dividend.m22 / divisor.m22,
                dividend.m23 / divisor.m23,
                dividend.m24 / divisor.m24,
                dividend.m31 / divisor.m31,
                dividend.m32 / divisor.m32,
                dividend.m33 / divisor.m33,
                dividend.m34 / divisor.m34,
                dividend.m41 / divisor.m41,
                dividend.m42 / divisor.m42,
                dividend.m43 / divisor.m43,
                dividend.m44 / divisor.m44
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
                matrix.m11, matrix.m21, matrix.m31, matrix.m41,
                matrix.m12, matrix.m22, matrix.m32, matrix.m42,
                matrix.m13, matrix.m23, matrix.m33, matrix.m43,
                matrix.m14, matrix.m24, matrix.m34, matrix.m44
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
                matrix.m11, matrix.m21, matrix.m31, matrix.m41,
                matrix.m12, matrix.m22, matrix.m32, matrix.m42,
                matrix.m13, matrix.m23, matrix.m33, matrix.m43,
                matrix.m14, matrix.m24, matrix.m34, matrix.m44
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
            var m11 = matrix1.m11 + (matrix2.m11 - matrix1.m11) * amount;
            var m12 = matrix1.m12 + (matrix2.m12 - matrix1.m12) * amount;
            var m13 = matrix1.m13 + (matrix2.m13 - matrix1.m13) * amount;
            var m14 = matrix1.m14 + (matrix2.m14 - matrix1.m14) * amount;

            var m21 = matrix1.m21 + (matrix2.m21 - matrix1.m21) * amount;
            var m22 = matrix1.m22 + (matrix2.m22 - matrix1.m22) * amount;
            var m23 = matrix1.m23 + (matrix2.m23 - matrix1.m23) * amount;
            var m24 = matrix1.m24 + (matrix2.m24 - matrix1.m24) * amount;

            var m31 = matrix1.m31 + (matrix2.m31 - matrix1.m31) * amount;
            var m32 = matrix1.m32 + (matrix2.m32 - matrix1.m32) * amount;
            var m33 = matrix1.m33 + (matrix2.m33 - matrix1.m33) * amount;
            var m34 = matrix1.m34 + (matrix2.m34 - matrix1.m34) * amount;

            var m41 = matrix1.m41 + (matrix2.m41 - matrix1.m41) * amount;
            var m42 = matrix1.m42 + (matrix2.m42 - matrix1.m42) * amount;
            var m43 = matrix1.m43 + (matrix2.m43 - matrix1.m43) * amount;
            var m44 = matrix1.m44 + (matrix2.m44 - matrix1.m44) * amount;

            result = new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44);
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
            var m11 = matrix1.m11 + (matrix2.m11 - matrix1.m11) * amount;
            var m12 = matrix1.m12 + (matrix2.m12 - matrix1.m12) * amount;
            var m13 = matrix1.m13 + (matrix2.m13 - matrix1.m13) * amount;
            var m14 = matrix1.m14 + (matrix2.m14 - matrix1.m14) * amount;

            var m21 = matrix1.m21 + (matrix2.m21 - matrix1.m21) * amount;
            var m22 = matrix1.m22 + (matrix2.m22 - matrix1.m22) * amount;
            var m23 = matrix1.m23 + (matrix2.m23 - matrix1.m23) * amount;
            var m24 = matrix1.m24 + (matrix2.m24 - matrix1.m24) * amount;

            var m31 = matrix1.m31 + (matrix2.m31 - matrix1.m31) * amount;
            var m32 = matrix1.m32 + (matrix2.m32 - matrix1.m32) * amount;
            var m33 = matrix1.m33 + (matrix2.m33 - matrix1.m33) * amount;
            var m34 = matrix1.m34 + (matrix2.m34 - matrix1.m34) * amount;

            var m41 = matrix1.m41 + (matrix2.m41 - matrix1.m41) * amount;
            var m42 = matrix1.m42 + (matrix2.m42 - matrix1.m42) * amount;
            var m43 = matrix1.m43 + (matrix2.m43 - matrix1.m43) * amount;
            var m44 = matrix1.m44 + (matrix2.m44 - matrix1.m44) * amount;

            return new Matrix(
                m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <param name="result">The invertex <see cref="Matrix"/>.</param>
        public static void Invert(ref Matrix matrix, out Matrix result)
        {
            var s0 = matrix.m11 * matrix.m22 - matrix.m21 * matrix.m12;
            var s1 = matrix.m11 * matrix.m23 - matrix.m21 * matrix.m13;
            var s2 = matrix.m11 * matrix.m24 - matrix.m21 * matrix.m14;
            var s3 = matrix.m12 * matrix.m23 - matrix.m22 * matrix.m13;
            var s4 = matrix.m12 * matrix.m24 - matrix.m22 * matrix.m14;
            var s5 = matrix.m13 * matrix.m24 - matrix.m23 * matrix.m14;

            var c5 = matrix.m33 * matrix.m44 - matrix.m43 * matrix.m34;
            var c4 = matrix.m32 * matrix.m44 - matrix.m42 * matrix.m34;
            var c3 = matrix.m32 * matrix.m43 - matrix.m42 * matrix.m33;
            var c2 = matrix.m31 * matrix.m44 - matrix.m41 * matrix.m34;
            var c1 = matrix.m31 * matrix.m43 - matrix.m41 * matrix.m33;
            var c0 = matrix.m31 * matrix.m42 - matrix.m41 * matrix.m32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
                throw new DivideByZeroException();

            var invdet = 1.0 / det;

            var m11 = (matrix.m22 * c5 - matrix.m23 * c4 + matrix.m24 * c3) * invdet;
            var m12 = (-matrix.m12 * c5 + matrix.m13 * c4 - matrix.m14 * c3) * invdet;
            var m13 = (matrix.m42 * s5 - matrix.m43 * s4 + matrix.m44 * s3) * invdet;
            var m14 = (-matrix.m32 * s5 + matrix.m33 * s4 - matrix.m34 * s3) * invdet;

            var m21 = (-matrix.m21 * c5 + matrix.m23 * c2 - matrix.m24 * c1) * invdet;
            var m22 = (matrix.m11 * c5 - matrix.m13 * c2 + matrix.m14 * c1) * invdet;
            var m23 = (-matrix.m41 * s5 + matrix.m43 * s2 - matrix.m44 * s1) * invdet;
            var m24 = (matrix.m31 * s5 - matrix.m33 * s2 + matrix.m34 * s1) * invdet;

            var m31 = (matrix.m21 * c4 - matrix.m22 * c2 + matrix.m24 * c0) * invdet;
            var m32 = (-matrix.m11 * c4 + matrix.m12 * c2 - matrix.m14 * c0) * invdet;
            var m33 = (matrix.m41 * s4 - matrix.m42 * s2 + matrix.m44 * s0) * invdet;
            var m34 = (-matrix.m31 * s4 + matrix.m32 * s2 - matrix.m34 * s0) * invdet;

            var m41  = (-matrix.m21 * c3 + matrix.m22 * c1 - matrix.m23 * c0) * invdet;
            var m42 = (matrix.m11 * c3 - matrix.m12 * c1 + matrix.m13 * c0) * invdet;
            var m43 = (-matrix.m41 * s3 + matrix.m42 * s1 - matrix.m43 * s0) * invdet;
            var m44 = (matrix.m31 * s3 - matrix.m32 * s1 + matrix.m33 * s0) * invdet;

            result = new Matrix(
                (float)m11, (float)m12, (float)m13, (float)m14,
                (float)m21, (float)m22, (float)m23, (float)m24,
                (float)m31, (float)m32, (float)m33, (float)m34,
                (float)m41, (float)m42, (float)m43, (float)m44);
        }

        /// <summary>
        /// Calculates the inverse of the specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to invert.</param>
        /// <returns>The inverted <see cref="Matrix"/>.</returns>
        public static Matrix Invert(Matrix matrix)
        {
            var s0 = matrix.m11 * matrix.m22 - matrix.m21 * matrix.m12;
            var s1 = matrix.m11 * matrix.m23 - matrix.m21 * matrix.m13;
            var s2 = matrix.m11 * matrix.m24 - matrix.m21 * matrix.m14;
            var s3 = matrix.m12 * matrix.m23 - matrix.m22 * matrix.m13;
            var s4 = matrix.m12 * matrix.m24 - matrix.m22 * matrix.m14;
            var s5 = matrix.m13 * matrix.m24 - matrix.m23 * matrix.m14;

            var c5 = matrix.m33 * matrix.m44 - matrix.m43 * matrix.m34;
            var c4 = matrix.m32 * matrix.m44 - matrix.m42 * matrix.m34;
            var c3 = matrix.m32 * matrix.m43 - matrix.m42 * matrix.m33;
            var c2 = matrix.m31 * matrix.m44 - matrix.m41 * matrix.m34;
            var c1 = matrix.m31 * matrix.m43 - matrix.m41 * matrix.m33;
            var c0 = matrix.m31 * matrix.m42 - matrix.m41 * matrix.m32;

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0)
                throw new DivideByZeroException();

            var invdet = 1.0 / det;

            var m11 = (matrix.m22 * c5 - matrix.m23 * c4 + matrix.m24 * c3) * invdet;
            var m12 = (-matrix.m12 * c5 + matrix.m13 * c4 - matrix.m14 * c3) * invdet;
            var m13 = (matrix.m42 * s5 - matrix.m43 * s4 + matrix.m44 * s3) * invdet;
            var m14 = (-matrix.m32 * s5 + matrix.m33 * s4 - matrix.m34 * s3) * invdet;

            var m21 = (-matrix.m21 * c5 + matrix.m23 * c2 - matrix.m24 * c1) * invdet;
            var m22 = (matrix.m11 * c5 - matrix.m13 * c2 + matrix.m14 * c1) * invdet;
            var m23 = (-matrix.m41 * s5 + matrix.m43 * s2 - matrix.m44 * s1) * invdet;
            var m24 = (matrix.m31 * s5 - matrix.m33 * s2 + matrix.m34 * s1) * invdet;

            var m31 = (matrix.m21 * c4 - matrix.m22 * c2 + matrix.m24 * c0) * invdet;
            var m32 = (-matrix.m11 * c4 + matrix.m12 * c2 - matrix.m14 * c0) * invdet;
            var m33 = (matrix.m41 * s4 - matrix.m42 * s2 + matrix.m44 * s0) * invdet;
            var m34 = (-matrix.m31 * s4 + matrix.m32 * s2 - matrix.m34 * s0) * invdet;

            var m41  = (-matrix.m21 * c3 + matrix.m22 * c1 - matrix.m23 * c0) * invdet;
            var m42 = (matrix.m11 * c3 - matrix.m12 * c1 + matrix.m13 * c0) * invdet;
            var m43 = (-matrix.m41 * s3 + matrix.m42 * s1 - matrix.m43 * s0) * invdet;
            var m44 = (matrix.m31 * s3 - matrix.m32 * s1 + matrix.m33 * s0) * invdet;

            return new Matrix(
                (float)m11, (float)m12, (float)m13, (float)m14,
                (float)m21, (float)m22, (float)m23, (float)m24,
                (float)m31, (float)m32, (float)m33, (float)m34,
                (float)m41, (float)m42, (float)m43, (float)m44);
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
                hash = hash * 23 + m11.GetHashCode();
                hash = hash * 23 + m12.GetHashCode();
                hash = hash * 23 + m13.GetHashCode();
                hash = hash * 23 + m14.GetHashCode();
                hash = hash * 23 + m21.GetHashCode();
                hash = hash * 23 + m22.GetHashCode();
                hash = hash * 23 + m23.GetHashCode();
                hash = hash * 23 + m24.GetHashCode();
                hash = hash * 23 + m31.GetHashCode();
                hash = hash * 23 + m32.GetHashCode();
                hash = hash * 23 + m33.GetHashCode();
                hash = hash * 23 + m34.GetHashCode();
                hash = hash * 23 + m41.GetHashCode();
                hash = hash * 23 + m42.GetHashCode();
                hash = hash * 23 + m43.GetHashCode();
                hash = hash * 23 + m44.GetHashCode();
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
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
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
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Matrix other)
        {
            return
                m11 == other.m11 &&
                m12 == other.m12 &&
                m13 == other.m13 &&
                m14 == other.m14 &&
                m21 == other.m21 &&
                m22 == other.m22 &&
                m23 == other.m23 &&
                m24 == other.m24 &&
                m31 == other.m31 &&
                m32 == other.m32 &&
                m33 == other.m33 &&
                m34 == other.m34 &&
                m41 == other.m41 &&
                m42 == other.m42 &&
                m43 == other.m43 &&
                m44 == other.m44;
        }

        /// <summary>
        /// Calculates the matrix's determinant.
        /// </summary>
        /// <returns>The matrix's determinant.</returns>
        public Single Determinant()
        {
            var s0 = m11 * m22 - m21 * m12;
            var s1 = m11 * m23 - m21 * m13;
            var s2 = m11 * m24 - m21 * m14;
            var s3 = m12 * m23 - m22 * m13;
            var s4 = m12 * m24 - m22 * m14;
            var s5 = m13 * m24 - m23 * m14;

            var c5 = m33 * m44 - m43 * m34;
            var c4 = m32 * m44 - m42 * m34;
            var c3 = m32 * m43 - m42 * m33;
            var c2 = m31 * m44 - m41 * m34;
            var c1 = m31 * m43 - m41 * m33;
            var c0 = m31 * m42 - m41 * m32;

            return (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
        }

        /// <summary>
        /// Interpolates between this value and the specified value.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
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
        /// Gets the value at row 1, column 1 of the matrix.
        /// </summary>
        public Single M11 { get { return m11; } }

        /// <summary>
        /// Gets the value at row 1, column 2 of the matrix.
        /// </summary>
        public Single M12 { get { return m12; } }

        /// <summary>
        /// Gets the value at row 1, column 3 of the matrix.
        /// </summary>
        public Single M13 { get { return m13; } }

        /// <summary>
        /// Gets the value at row 1, column 4 of the matrix.
        /// </summary>
        public Single M14 { get { return m14; } }

        /// <summary>
        /// Gets the value at row 2, column 1 of the matrix.
        /// </summary>
        public Single M21 { get { return m21; } }

        /// <summary>
        /// Gets the value at row 2, column 2 of the matrix.
        /// </summary>
        public Single M22 { get { return m22; } }

        /// <summary>
        /// Gets the value at row 2, column 3 of the matrix.
        /// </summary>
        public Single M23 { get { return m23; } }

        /// <summary>
        /// Gets the value at row 2, column 4 of the matrix.
        /// </summary>
        public Single M24 { get { return m24; } }

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M31 { get { return m31; } }

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M32 { get { return m32; } }

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M33 { get { return m33; } }

        /// <summary>
        /// Gets the value at row 3, column 1 of the matrix.
        /// </summary>
        public Single M34 { get { return m34; } }

        /// <summary>
        /// Gets the value at row 4, column 1 of the matrix.
        /// </summary>
        public Single M41 { get { return m41; } }

        /// <summary>
        /// Gets the value at row 4, column 2 of the matrix.
        /// </summary>
        public Single M42 { get { return m42; } }

        /// <summary>
        /// Gets the value at row 4, column 3 of the matrix.
        /// </summary>
        public Single M43 { get { return m43; } }

        /// <summary>
        /// Gets the value at row 4, column 4 of the matrix.
        /// </summary>
        public Single M44 { get { return m44; } }

        /// <summary>
        /// Gets the matrix's right vector.
        /// </summary>
        public Vector3 Right { get { return new Vector3(m11, m21, m31); } }

        /// <summary>
        /// Gets the matrix's left vector.
        /// </summary>
        public Vector3 Left { get { return new Vector3(-m11, -m21, -m31); } }

        /// <summary>
        /// Gets the matrix's up vector.
        /// </summary>
        public Vector3 Up { get { return new Vector3(m12, m22, m32); } }

        /// <summary>
        /// Gets the matrix's down vector.
        /// </summary>
        public Vector3 Down { get { return new Vector3(-m12, -m22, -m32); } }

        /// <summary>
        /// Gets the matrix's backwards vector.
        /// </summary>
        public Vector3 Backward { get { return new Vector3(m13, m23, m33); } }

        /// <summary>
        /// Gets the matrix's forwards vector.
        /// </summary>
        public Vector3 Forward { get { return new Vector3(-m13, -m23, -m33); } }

        /// <summary>
        /// Gets the matrix's translation vector.
        /// </summary>
        public Vector3 Translation { get { return new Vector3(m14, m24, m34); } }

        // Property values.
        private readonly Single
            m11, m21, m31, m41,
            m12, m22, m32, m42,
            m13, m23, m33, m43,
            m14, m24, m34, m44;
    }
}
