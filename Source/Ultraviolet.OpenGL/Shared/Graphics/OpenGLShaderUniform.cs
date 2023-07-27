using System;
using System.Buffers;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;
using Ultraviolet.OpenGL.Graphics.Uniforms;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a shader uniform.
    /// </summary>
    public unsafe sealed class OpenGLShaderUniform
    {
        /// <summary>
        /// Represents a method which is used to transpose matrices.
        /// </summary>
        private delegate void MatrixTransposeDelegate(IntPtr ptr, Int32 index);

        /// <summary>
        /// Represents a method which is used to upload matrix data to the graphics device.
        /// </summary>
        private delegate void MatrixUploadDelegate(Int32 location, Int32 count, Boolean transpose, IntPtr value);

        /// <summary>
        /// Initializes a new instance of the OpenGLShaderUniform class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect parameter's name.</param>
        /// <param name="type">The effect parameter's uniform type.</param>
        /// <param name="count">The effect parameter's count of array elements.</param>
        /// <param name="program">The effect parameter's associated program identifier.</param>
        /// <param name="location">The effect parameter's uniform location.</param>
        /// <param name="sampler">The effect's corresponding texture sampler, if any.</param>
        public OpenGLShaderUniform(UltravioletContext uv, String name, UInt32 type, UInt32 count, UInt32 program, Int32 location, Int32 sampler)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(name, nameof(name));

            this.uv = uv;
            this.Name = name ?? String.Empty;
            this.Type = type;
            this.Count = count;
            this.SizeInBytes = CalculateSizeInBytes(type, count);
            this.Program = program;
            this.location = location;
            this.sampler = sampler;
        }

        /// <summary>
        /// Applies the shader uniform.
        /// </summary>
        public void Apply()
        {
            if (source == null)
                throw new InvalidOperationException(OpenGLStrings.ShaderUniformHasNoSource);

            if (source.Version == version)
            {
                switch (source.DataType)
                {
                    case OpenGLEffectParameterDataType.Texture2D:
                        if (uv.GetGraphics().GetTexture(sampler) == source.GetTexture2D())
                            return;
                        break;

                    case OpenGLEffectParameterDataType.Texture3D:
                        if (uv.GetGraphics().GetTexture(sampler) == source.GetTexture3D())
                            return;
                        break;

                    default:
                        return;
                }
            }

            version = source.Version;
            switch (source.DataType)
            {
                case OpenGLEffectParameterDataType.None:
                    break;

                case OpenGLEffectParameterDataType.Boolean:
                    SetValue(source.GetBoolean());
                    break;

                case OpenGLEffectParameterDataType.Int32:
                    SetValue(source.GetInt32());
                    break;

                case OpenGLEffectParameterDataType.Int32Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Int32*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.UInt32:
                    SetValue(source.GetUInt32());
                    break;

                case OpenGLEffectParameterDataType.UInt32Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((UInt32*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Single:
                    SetValue(source.GetSingle());
                    break;

                case OpenGLEffectParameterDataType.SingleArray:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Single*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Double:
                    SetValue(source.GetDouble());
                    break;

                case OpenGLEffectParameterDataType.DoubleArray:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Double*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Vector2:
                    SetValue(source.GetVector2());
                    break;

                case OpenGLEffectParameterDataType.Vector2Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Vector2*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Vector3:
                    SetValue(source.GetVector3());
                    break;

                case OpenGLEffectParameterDataType.Vector3Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Vector3*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Vector4:
                    SetValue(source.GetVector4());
                    break;

                case OpenGLEffectParameterDataType.Vector4Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Vector4*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Color:
                    SetValue(source.GetColor());
                    break;

                case OpenGLEffectParameterDataType.ColorArray:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Color*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat2:
                    SetValue(source.GetMat2());
                    break;

                case OpenGLEffectParameterDataType.Mat2Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat2*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat2x3:
                    SetValue(source.GetMat2x3());
                    break;

                case OpenGLEffectParameterDataType.Mat2x3Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat2x3*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat2x4:
                    SetValue(source.GetMat2x4());
                    break;

                case OpenGLEffectParameterDataType.Mat2x4Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat2x4*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat3:
                    SetValue(source.GetMat3());
                    break;

                case OpenGLEffectParameterDataType.Mat3Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat3*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat3x2:
                    SetValue(source.GetMat3x2());
                    break;

                case OpenGLEffectParameterDataType.Mat3x2Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat3x2*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat3x4:
                    SetValue(source.GetMat3x4());
                    break;

                case OpenGLEffectParameterDataType.Mat3x4Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat3x4*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat4:
                    SetValue(source.GetMat4());
                    break;
					
                case OpenGLEffectParameterDataType.Mat4Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Matrix*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat4x2:
                    SetValue(source.GetMat4x2());
                    break;

                case OpenGLEffectParameterDataType.Mat4x2Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat4x2*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Mat4x3:
                    SetValue(source.GetMat4x3());
                    break;

                case OpenGLEffectParameterDataType.Mat4x3Array:
                    fixed (Byte* pBuffer = source.RawDataBuffer)
                        SetValue((Mat4x3*)pBuffer, source.ElementCount);
                    break;

                case OpenGLEffectParameterDataType.Texture2D:
                    SetValue(source.GetTexture2D());
                    break;

                case OpenGLEffectParameterDataType.Texture3D:
                    SetValue(source.GetTexture3D());
                    break;

                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedDataType);
            }
        }

        /// <summary>
        /// Sets the effect parameter which the uniform uses as its data source.
        /// </summary>
        /// <param name="source">The effect parameter which the uniform will use as its data source.</param>
        public void SetDataSource(OpenGLEffectParameterData source)
        {
            Contract.Require(source, nameof(source));

            this.source = source;
            this.version = 0;
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Boolean value)
        {
            GL.Uniform1i(location, value ? 1 : 0);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Int32 value)
        {
            GL.Uniform1i(location, value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Int32* pValue, Int32 count)
        {
            GL.Uniform1iv(location, count, pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(UInt32 value)
        {
            GL.Uniform1ui(location, value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(UInt32* pValue, Int32 count)
        {
            GL.Uniform1uiv(location, count, pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Single value)
        {
            GL.Uniform1f(location, value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Single* pValue, Int32 count)
        {
            GL.Uniform1fv(location, count, pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Double value)
        {
            GL.Uniform1d(location, value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Double* pValue, Int32 count)
        {
            GL.Uniform1dv(location, count, pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector2 value)
        {
            GL.Uniform2f(location, value.X, value.Y);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Vector2* pValue, Int32 count)
        {
            GL.Uniform2fv(location, count, (Single*)pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector3 value)
        {
            GL.Uniform3f(location, value.X, value.Y, value.Z);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Vector3* pValue, Int32 count)
        {
            GL.Uniform3fv(location, count, (Single*)pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector4 value)
        {
            GL.Uniform4f(location, value.X, value.Y, value.Z, value.W);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Vector4* pValue, Int32 count)
        {
            GL.Uniform4fv(location, count, (Single*)pValue);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Color value)
        {
            var nr = value.R / (float)Byte.MaxValue;
            var ng = value.G / (float)Byte.MaxValue;
            var nb = value.B / (float)Byte.MaxValue;
            var na = value.A / (float)Byte.MaxValue;

            if (Type == GL.GL_FLOAT_VEC3)
            {
                GL.Uniform3f(location, nr, ng, nb);
            }
            else
            {
                GL.Uniform4f(location, nr, ng, nb, na);
            }
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Color* pValue, Int32 count)
        {
            if (pValue == null)
            {
                if (Type == GL.GL_FLOAT_VEC3)
                {
                    GL.Uniform3fv(location, 0, null);
                    GL.ThrowIfError();
                }
                else
                {
                    GL.Uniform4fv(location, 0, null);
                    GL.ThrowIfError();
                }
            }
            else
            {
                if (Type == GL.GL_FLOAT_VEC3)
                {
                    var normalized = stackalloc float[3 * count];
                    for (var i = 0; i < count; i++)
                    {
                        var c = pValue[i];
                        normalized[(i * 3) + 0] = c.R / (float)Byte.MaxValue;
                        normalized[(i * 3) + 1] = c.G / (float)Byte.MaxValue;
                        normalized[(i * 3) + 2] = c.B / (float)Byte.MaxValue;
                    }

                    GL.Uniform3fv(location, count, normalized);
                    GL.ThrowIfError();
                }
                else
                {
                    var normalized = stackalloc float[4 * count];
                    for (var i = 0; i < count; i++)
                    {
                        var c = pValue[i];
                        normalized[(i * 4) + 0] = c.R / (float)Byte.MaxValue;
                        normalized[(i * 4) + 1] = c.G / (float)Byte.MaxValue;
                        normalized[(i * 4) + 2] = c.B / (float)Byte.MaxValue;
                        normalized[(i * 4) + 3] = c.A / (float)Byte.MaxValue;
                    }

                    GL.Uniform4fv(location, count, normalized);
                    GL.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat2 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (!transpose)
                Mat2.Transpose(ref value, out value);

            GL.UniformMatrix2fv(location, 1, transpose, (float*)&value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat2* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat2>((IntPtr)pValue, count,
                (ptr, index) => Mat2.Transpose(ref ((Mat2*)ptr)[index], out ((Mat2*)ptr)[index]),
                (ptr, index) => Mat2.Transpose(ref ((Mat2*)ptr)[index], out ((Mat2*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix2fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat2x3 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix2x3fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat2x3.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix2x3fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat2x3* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat2x3>((IntPtr)pValue, count,
                (ptr, index) => Mat2x3.Transpose(ref ((Mat2x3*)ptr)[index], out ((Mat3x2*)ptr)[index]),
                (ptr, index) => Mat3x2.Transpose(ref ((Mat3x2*)ptr)[index], out ((Mat2x3*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix2x3fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat2x4 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix2x4fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat2x4.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix2x4fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat2x4* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat2x4>((IntPtr)pValue, count,
                (ptr, index) => Mat2x4.Transpose(ref ((Mat2x4*)ptr)[index], out ((Mat4x2*)ptr)[index]),
                (ptr, index) => Mat4x2.Transpose(ref ((Mat4x2*)ptr)[index], out ((Mat2x4*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix2x4fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat3 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (!transpose)
                Mat3.Transpose(ref value, out value);

            GL.UniformMatrix3fv(location, 1, transpose, (Single*)&value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat3* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat3>((IntPtr)pValue, count,
                (ptr, index) => Mat3.Transpose(ref ((Mat3*)ptr)[index], out ((Mat3*)ptr)[index]),
                (ptr, index) => Mat3.Transpose(ref ((Mat3*)ptr)[index], out ((Mat3*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix3fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat3x2 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix3x2fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat3x2.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix3x2fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }
       
        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat3x2* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat3x2>((IntPtr)pValue, count,
                (ptr, index) => Mat3x2.Transpose(ref ((Mat3x2*)ptr)[index], out ((Mat2x3*)ptr)[index]),
                (ptr, index) => Mat2x3.Transpose(ref ((Mat2x3*)ptr)[index], out ((Mat3x2*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix3x2fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat3x4 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix3x4fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat3x4.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix3x4fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat3x4* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat3x4>((IntPtr)pValue, count,
                (ptr, index) => Mat3x4.Transpose(ref ((Mat3x4*)ptr)[index], out ((Mat4x3*)ptr)[index]),
                (ptr, index) => Mat4x3.Transpose(ref ((Mat4x3*)ptr)[index], out ((Mat3x4*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix3x4fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Matrix value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (!transpose)
                Matrix.Transpose(ref value, out value);

            GL.UniformMatrix4fv(location, 1, transpose, (Single*)&value);
            GL.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Matrix* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Matrix>((IntPtr)pValue, count,
                (ptr, index) => Matrix.Transpose(ref ((Matrix*)ptr)[index], out ((Matrix*)ptr)[index]),
                (ptr, index) => Matrix.Transpose(ref ((Matrix*)ptr)[index], out ((Matrix*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix4fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat4x2 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix4x2fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat4x2.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix4x2fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat4x2* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat4x2>((IntPtr)pValue, count,
                (ptr, index) => Mat4x2.Transpose(ref ((Mat4x2*)ptr)[index], out ((Mat2x4*)ptr)[index]),
                (ptr, index) => Mat2x4.Transpose(ref ((Mat2x4*)ptr)[index], out ((Mat4x2*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix4x2fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Mat4x3 value)
        {
            var transpose = GL.IsMatrixTranspositionAvailable;
            if (transpose)
            {
                GL.UniformMatrix4x3fv(location, 1, true, (Single*)&value);
                GL.ThrowIfError();
            }
            else
            {
                Mat4x3.Transpose(ref value, out var valueTransposed);

                GL.UniformMatrix4x3fv(location, 1, false, (Single*)&valueTransposed);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="pValue">A pointer to the buffer that contains the value to set.</param>
        /// <param name="count">The number of elements in the array to set.</param>
        public void SetValue(Mat4x3* pValue, Int32 count)
        {
            SetMatrixArrayValueInternal<Mat4x3>((IntPtr)pValue, count,
                (ptr, index) => Mat4x3.Transpose(ref ((Mat4x3*)ptr)[index], out ((Mat3x4*)ptr)[index]),
                (ptr, index) => Mat3x4.Transpose(ref ((Mat3x4*)ptr)[index], out ((Mat4x3*)ptr)[index]),
                (l, c, t, v) => GL.UniformMatrix4x3fv(l, c, t, (Single*)v));
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Texture2D value)
        {
            GL.Uniform1i(location, sampler);
            GL.ThrowIfError();

            uv.GetGraphics().SetTexture(sampler, value);
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Texture3D value)
        {
            GL.Uniform1i(location, sampler);
            GL.ThrowIfError();

            uv.GetGraphics().SetTexture(sampler, value);
        }

        /// <summary>
        /// Gets the shader uniform's name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the shader uniform's type.
        /// </summary>
        public UInt32 Type { get; }

        /// <summary>
        /// Gets the shader uniform's count of array elements.
        /// </summary>
        public UInt32 Count { get; }

        /// <summary>
        /// Gets the shader uniform's total size in bytes.
        /// </summary>
        public UInt32 SizeInBytes { get; }

        /// <summary>
        /// Gets the shader program identifier.
        /// </summary>
        public UInt32 Program { get; }

        /// <summary>
        /// Calculates the size of a shader uniform in bytes.
        /// </summary>
        private static UInt32 CalculateSizeInBytes(UInt32 type, UInt32 count)
        {
            switch (type)
            {
                case GL.GL_FLOAT:
                    return count * sizeof(Single);
                case GL.GL_FLOAT_VEC2:
                    return count * sizeof(Single) * 2;
                case GL.GL_FLOAT_VEC3:
                    return count * sizeof(Single) * 3;
                case GL.GL_FLOAT_VEC4:
                    return count * sizeof(Single) * 4;
                case GL.GL_DOUBLE:
                    return count * sizeof(Double);
                case GL.GL_INT:
                    return count * sizeof(Int32);
                case GL.GL_INT_VEC2:
                    return count * sizeof(Int32) * 2;
                case GL.GL_INT_VEC3:
                    return count * sizeof(Int32) * 3;
                case GL.GL_INT_VEC4:
                    return count * sizeof(Int32) * 4;
                case GL.GL_UNSIGNED_INT:
                    return count * sizeof(UInt32);
                case GL.GL_UNSIGNED_INT_VEC2:
                    return count * sizeof(UInt32) * 2;
                case GL.GL_UNSIGNED_INT_VEC3:
                    return count * sizeof(UInt32) * 3;
                case GL.GL_UNSIGNED_INT_VEC4:
                    return count * sizeof(UInt32) * 4;
                case GL.GL_BOOL:
                    return count * sizeof(Boolean);
                case GL.GL_BOOL_VEC2:
                    return count * sizeof(Boolean) * 2;
                case GL.GL_BOOL_VEC3:
                    return count * sizeof(Boolean) * 3;
                case GL.GL_BOOL_VEC4:
                    return count * sizeof(Boolean) * 4;
                case GL.GL_FLOAT_MAT2:
                    return count * sizeof(Single) * 4;
                case GL.GL_FLOAT_MAT3:
                    return count * sizeof(Single) * 9;
                case GL.GL_FLOAT_MAT4:
                    return count * sizeof(Single) * 16;
                case GL.GL_FLOAT_MAT2x3:
                case GL.GL_FLOAT_MAT3x2:
                    return count * sizeof(Single) * 6;
                case GL.GL_FLOAT_MAT2x4:
                case GL.GL_FLOAT_MAT4x2:
                    return count * sizeof(Single) * 8;
                case GL.GL_FLOAT_MAT3x4:
                case GL.GL_FLOAT_MAT4x3:
                    return count * sizeof(Single) * 12;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        private void SetMatrixArrayValueInternal<TMatrix>(IntPtr pValue, Int32 count,
            MatrixTransposeDelegate transpose, MatrixTransposeDelegate untranspose, MatrixUploadDelegate upload) where TMatrix : unmanaged
        {
            var glslTranspositionAvailable = GL.IsMatrixTranspositionAvailable;
            if (glslTranspositionAvailable)
            {
                upload(location, count, true, pValue);
                GL.ThrowIfError();
            }
            else
            {
                var transpositionBufferSize = sizeof(TMatrix) * count;
                if (transpositionBufferSize > MemoryPool<Byte>.Shared.MaxBufferSize)
                {
                    for (var i = 0; i < count; i++)
                        transpose(pValue, i);

                    upload(location, count, false, pValue);
                    GL.ThrowIfError();

                    for (var i = 0; i < count; i++)
                        untranspose(pValue, i);

                }
                else
                {
                    using (var transpositionBuffer = MemoryPool<Byte>.Shared.Rent(transpositionBufferSize))
                    using (var transpositionBufferHandle = transpositionBuffer.Memory.Pin())
                    {
                        for (var i = 0; i < count; i++)
                            transpose(pValue, i);

                        upload(location, count, false, pValue);
                        GL.ThrowIfError();
                    }
                }
            }
        }

        // State values.
        private readonly UltravioletContext uv;
        private readonly Int32 location;
        private readonly Int32 sampler;
        private OpenGLEffectParameterData source;
        private Int64 version;
    }
}
