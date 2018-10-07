using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a shader uniform.
    /// </summary>
    public unsafe sealed class OpenGLShaderUniform
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLShaderUniform class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect parameter's name.</param>
        /// <param name="type">The effect parameter's uniform type.</param>
        /// <param name="program">The effect parameter's associated program identifier.</param>
        /// <param name="location">The effect parameter's uniform location.</param>
        /// <param name="sampler">The effect's corresponding texture sampler, if any.</param>
        public OpenGLShaderUniform(UltravioletContext uv, String name, UInt32 type, UInt32 program, Int32 location, Int32 sampler)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(name, nameof(name));

            this.uv = uv;
            this.Name = name ?? String.Empty;
            this.Type = type;
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
                    SetValue(source.GetInt32Array());
                    break;

                case OpenGLEffectParameterDataType.UInt32:
                    SetValue(source.GetUInt32());
                    break;

                case OpenGLEffectParameterDataType.UInt32Array:
                    SetValue(source.GetUInt32Array());
                    break;

                case OpenGLEffectParameterDataType.Single:
                    SetValue(source.GetSingle());
                    break;

                case OpenGLEffectParameterDataType.SingleArray:
                    SetValue(source.GetSingleArray());
                    break;

                case OpenGLEffectParameterDataType.Double:
                    SetValue(source.GetDouble());
                    break;

                case OpenGLEffectParameterDataType.DoubleArray:
                    SetValue(source.GetDoubleArray());
                    break;

                case OpenGLEffectParameterDataType.Vector2:
                    SetValue(source.GetVector2());
                    break;

                case OpenGLEffectParameterDataType.Vector2Array:
                    SetValue(source.GetVector2Array());
                    break;

                case OpenGLEffectParameterDataType.Vector3:
                    SetValue(source.GetVector3());
                    break;

                case OpenGLEffectParameterDataType.Vector3Array:
                    SetValue(source.GetVector3Array());
                    break;

                case OpenGLEffectParameterDataType.Vector4:
                    SetValue(source.GetVector4());
                    break;

                case OpenGLEffectParameterDataType.Vector4Array:
                    SetValue(source.GetVector4Array());
                    break;

                case OpenGLEffectParameterDataType.Color:
                    SetValue(source.GetColor());
                    break;

                case OpenGLEffectParameterDataType.ColorArray:
                    SetValue(source.GetColorArray());
                    break;

                case OpenGLEffectParameterDataType.Matrix:
                    SetValue(source.GetMatrix());
                    break;
					
                case OpenGLEffectParameterDataType.MatrixArray:
                    SetValue(source.GetMatrixArray());
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
            gl.Uniform1i(location, value ? 1 : 0);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Int32 value)
        {
            gl.Uniform1i(location, value);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Int32[] value)
        {
            if (value == null)
            {
                gl.Uniform1iv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (int* pValue = value)
                {
                    gl.Uniform1iv(location, value.Length, pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(UInt32 value)
        {
            gl.Uniform1ui(location, value);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(UInt32[] value)
        {
            if (value == null)
            {
                gl.Uniform1uiv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (uint* pValue = value)
                {
                    gl.Uniform1uiv(location, value.Length, pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Single value)
        {
            gl.Uniform1f(location, value);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Single[] value)
        {
            if (value == null)
            {
                gl.Uniform1fv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (float* pValue = value)
                {
                    gl.Uniform1fv(location, value.Length, pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Double value)
        {
            gl.Uniform1d(location, value);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Double[] value)
        {
            if (value == null)
            {
                gl.Uniform1dv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (double* pValue = value)
                {
                    gl.Uniform1dv(location, value.Length, pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector2 value)
        {
            gl.Uniform2f(location, value.X, value.Y);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector2[] value)
        {
            if (value == null)
            {
                gl.Uniform2fv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (Vector2* pValue = value)
                {
                    gl.Uniform2fv(location, value.Length, (float*)pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector3 value)
        {
            gl.Uniform3f(location, value.X, value.Y, value.Z);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector3[] value)
        {
            if (value == null)
            {
                gl.Uniform3fv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (Vector3* pValue = value)
                {
                    gl.Uniform3fv(location, value.Length, (float*)pValue);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector4 value)
        {
            gl.Uniform4f(location, value.X, value.Y, value.Z, value.W);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Vector4[] value)
        {
            if (value == null)
            {
                gl.Uniform4fv(location, 0, null);
                gl.ThrowIfError();
            }
            else
            {
                fixed (Vector4* pValue = value)
                {
                    gl.Uniform4fv(location, value.Length, (float*)pValue);
                    gl.ThrowIfError();
                }
            }
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

            if (Type == gl.GL_FLOAT_VEC3)
            {
                gl.Uniform3f(location, nr, ng, nb);
            }
            else
            {
                gl.Uniform4f(location, nr, ng, nb, na);
            }
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Color[] value)
        {
            if (value == null)
            {
                if (Type == gl.GL_FLOAT_VEC3)
                {
                    gl.Uniform3fv(location, 0, null);
                    gl.ThrowIfError();
                }
                else
                {
                    gl.Uniform4fv(location, 0, null);
                    gl.ThrowIfError();
                }
            }
            else
            {
                if (Type == gl.GL_FLOAT_VEC3)
                {
                    var normalized = stackalloc float[3 * value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        normalized[(i * 3) + 0] = value[i].R / (float)Byte.MaxValue;
                        normalized[(i * 3) + 1] = value[i].G / (float)Byte.MaxValue;
                        normalized[(i * 3) + 2] = value[i].B / (float)Byte.MaxValue;
                    }

                    gl.Uniform3fv(location, value.Length, normalized);
                    gl.ThrowIfError();
                }
                else
                {
                    var normalized = stackalloc float[4 * value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        normalized[(i * 4) + 0] = value[i].R / (float)Byte.MaxValue;
                        normalized[(i * 4) + 1] = value[i].G / (float)Byte.MaxValue;
                        normalized[(i * 4) + 2] = value[i].B / (float)Byte.MaxValue;
                        normalized[(i * 4) + 3] = value[i].A / (float)Byte.MaxValue;
                    }

                    gl.Uniform4fv(location, value.Length, normalized);
                    gl.ThrowIfError();
                }
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Matrix value)
        {
            var transpose = gl.IsMatrixTranspositionAvailable;
            if (!transpose)
                Matrix.Transpose(ref value, out value);

            gl.UniformMatrix4fv(location, 1, transpose, (float*)&value);
            gl.ThrowIfError();
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Matrix[] value)
        {
            var transpose = gl.IsMatrixTranspositionAvailable;

            if (!transpose)
            {
                for (int i = 0; i < value.Length; i++)
                    Matrix.Transpose(ref value[i], out value[i]);
            }

            fixed (Matrix* pValue = value)
            {
                gl.UniformMatrix4fv(location, value.Length, transpose, (float*)pValue);
                gl.ThrowIfError();
            }

            if (!transpose)
            {
                for (int i = 0; i < value.Length; i++)
                    Matrix.Transpose(ref value[i], out value[i]);
            }
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Texture2D value)
        {
            gl.Uniform1i(location, sampler);
            gl.ThrowIfError();

            uv.GetGraphics().SetTexture(sampler, value);
        }

        /// <summary>
        /// Sets the parameter's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(Texture3D value)
        {
            gl.Uniform1i(location, sampler);
            gl.ThrowIfError();

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
        /// Gets the shader program identifier.
        /// </summary>
        public UInt32 Program { get; }

        // State values.
        private readonly UltravioletContext uv;
        private readonly Int32 location;
        private readonly Int32 sampler;
        private OpenGLEffectParameterData source;
        private Int64 version;
    }
}
