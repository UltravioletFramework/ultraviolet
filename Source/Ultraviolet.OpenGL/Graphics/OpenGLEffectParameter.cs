using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the EffectParameter class.
    /// </summary>
    public sealed class OpenGLEffectParameter : EffectParameter
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectParameter class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect parameter's name.</param>
        /// <param name="type">The effect parameter's uniform type.</param>
        /// <param name="sizeInBytes">The effect parameter's size in bytes.</param>
        public OpenGLEffectParameter(UltravioletContext uv, String name, UInt32 type, UInt32 sizeInBytes)
            : base(uv)
        {
            Contract.Require(name, nameof(name));

            this.name = name ?? String.Empty;
            this.type = type;
            this.data = new OpenGLEffectParameterData(sizeInBytes);
        }

        /// <inheritdoc/>
        public override Boolean GetValueBoolean()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_BOOL);

            return data.GetBoolean();
        }

        /// <inheritdoc/>
        public override void SetValue(Boolean value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_BOOL);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueBooleanArray(Boolean[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_BOOL);

            data.GetBooleanArray(value, count);
        }

        /// <inheritdoc/>
        public override Int32 GetValueInt32()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_INT);

            return data.GetInt32();
        }

        /// <inheritdoc/>
        public override void SetValue(Int32 value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_INT);
            
            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueInt32Array(Int32[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_INT);

            data.GetInt32Array(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Int32[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override UInt32 GetValueUInt32()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_UNSIGNED_INT);

            return data.GetUInt32();
        }

        /// <inheritdoc/>
        public override void SetValue(UInt32 value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueUInt32Array(UInt32[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_UNSIGNED_INT);

            data.GetUInt32Array(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(UInt32[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Single GetValueSingle()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT);

            return data.GetSingle();
        }

        /// <inheritdoc/>
        public override void SetValue(Single value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueSingleArray(Single[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT);

            data.GetSingleArray(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Single[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Double GetValueDouble()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_DOUBLE);

            return data.GetDouble();
        }

        /// <inheritdoc/>
        public override void SetValue(Double value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_DOUBLE);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueDoubleArray(Double[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_DOUBLE);

            data.GetDoubleArray(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Double[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_DOUBLE);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector2 GetValueVector2()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC2);

            return data.GetVector2();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector2 value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueVector2Array(Vector2[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC2);

            data.GetVector2Array(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Vector2[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector3 GetValueVector3()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3);

            return data.GetVector3();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector3 value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueVector3Array(Vector3[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3);

            data.GetVector3Array(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Vector3[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector4 GetValueVector4()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC4);

            return data.GetVector4();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector4 value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueVector4Array(Vector4[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC4);

            data.GetVector4Array(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Vector4[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Color GetValueColor()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3 || type == GL.GL_FLOAT_VEC4);

            return data.GetColor();
        }

        /// <inheritdoc/>
        public override void SetValue(Color value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3 || type == GL.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override void GetValueColorArray(Color[] value, Int32 count)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3 || type == GL.GL_FLOAT_VEC4);

            data.GetColorArray(value, count);
        }

        /// <inheritdoc/>
        public override void SetValue(Color[] value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_FLOAT_VEC3 || type == GL.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Matrix GetValueMatrix()
        {
            switch (type)
            {
                case GL.GL_FLOAT_MAT2:
                    return (Matrix)data.GetMat2();
                case GL.GL_FLOAT_MAT2x3:
                    return (Matrix)data.GetMat2x3();
                case GL.GL_FLOAT_MAT2x4:
                    return (Matrix)data.GetMat2x4();
                case GL.GL_FLOAT_MAT3:
                    return (Matrix)data.GetMat3();
                case GL.GL_FLOAT_MAT3x2:
                    return (Matrix)data.GetMat3x2();
                case GL.GL_FLOAT_MAT3x4:
                    return (Matrix)data.GetMat3x4();
                case GL.GL_FLOAT_MAT4:
                    return data.GetMat4();
                case GL.GL_FLOAT_MAT4x2:
                    return (Matrix)data.GetMat4x2();
                case GL.GL_FLOAT_MAT4x3:
                    return (Matrix)data.GetMat4x3();
            }

            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public override void SetValue(Matrix value)
        {
            switch (type)
            {
                case GL.GL_FLOAT_MAT2:
                    data.SetMat2(value);
                    return;
                case GL.GL_FLOAT_MAT2x3:
                    data.SetMat2x3(value);
                    return;
                case GL.GL_FLOAT_MAT2x4:
                    data.SetMat2x4(value);
                    return;
                case GL.GL_FLOAT_MAT3:
                    data.SetMat3(value);
                    return;
                case GL.GL_FLOAT_MAT3x2:
                    data.SetMat3x2(value);
                    return;
                case GL.GL_FLOAT_MAT3x4:
                    data.SetMat3x4(value);
                    return;
                case GL.GL_FLOAT_MAT4:
                    data.SetMat4(value);
                    return;
                case GL.GL_FLOAT_MAT4x2:
                    data.SetMat4x2(value);
                    return;
                case GL.GL_FLOAT_MAT4x3:
                    data.SetMat4x3(value);
                    return;
            }

            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public override void SetValueRef(ref Matrix value)
        {
            switch (type)
            {
                case GL.GL_FLOAT_MAT2:
                    data.SetMat2(value);
                    return;
                case GL.GL_FLOAT_MAT2x3:
                    data.SetMat2x3(value);
                    return;
                case GL.GL_FLOAT_MAT2x4:
                    data.SetMat2x4(value);
                    return;
                case GL.GL_FLOAT_MAT3:
                    data.SetMat3(value);
                    return;
                case GL.GL_FLOAT_MAT3x2:
                    data.SetMat3x2(value);
                    return;
                case GL.GL_FLOAT_MAT3x4:
                    data.SetMat3x4(value);
                    return;
                case GL.GL_FLOAT_MAT4:
                    data.SetMat4(value);
                    return;
                case GL.GL_FLOAT_MAT4x2:
                    data.SetMat4x2(value);
                    return;
                case GL.GL_FLOAT_MAT4x3:
                    data.SetMat4x3(value);
                    return;
            }

            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public override void GetValueMatrixArray(Matrix[] value, Int32 count)
        {
            switch (type)
            {
                case GL.GL_FLOAT_MAT2:
                    data.GetMat2Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT2x3:
                    data.GetMat2x3Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT2x4:
                    data.GetMat2x4Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT3:
                    data.GetMat3Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT3x2:
                    data.GetMat3x2Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT3x4:
                    data.GetMat3x4Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT4:
                    data.GetMat4Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT4x2:
                    data.GetMat4x2Array(value, count);
                    return;
                case GL.GL_FLOAT_MAT4x3:
                    data.GetMat4x3Array(value, count);
                    return;
            }

            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public override void SetValue(Matrix[] value)
        {
            switch (type)
            {
                case GL.GL_FLOAT_MAT2:
                    data.SetMat2(value);
                    return;
                case GL.GL_FLOAT_MAT2x3:
                    data.SetMat2x3(value);
                    return;
                case GL.GL_FLOAT_MAT2x4:
                    data.SetMat2x4(value);
                    return;
                case GL.GL_FLOAT_MAT3:
                    data.SetMat3(value);
                    return;
                case GL.GL_FLOAT_MAT3x2:
                    data.SetMat3x2(value);
                    return;
                case GL.GL_FLOAT_MAT3x4:
                    data.SetMat3x4(value);
                    return;
                case GL.GL_FLOAT_MAT4:
                    data.SetMat4(value);
                    return;
                case GL.GL_FLOAT_MAT4x2:
                    data.SetMat4x2(value);
                    return;
                case GL.GL_FLOAT_MAT4x3:
                    data.SetMat4x3(value);
                    return;
            }

            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public override Texture2D GetValueTexture2D()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_SAMPLER_2D);

            return data.GetTexture2D();
        }

        /// <inheritdoc/>
        public override void SetValue(Texture2D value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_SAMPLER_2D);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Texture3D GetValueTexture3D()
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_SAMPLER_3D);

            return data.GetTexture3D();
        }

        /// <inheritdoc/>
        public override void SetValue(Texture3D value)
        {
            Contract.Ensure<InvalidCastException>(type == GL.GL_SAMPLER_3D);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override String Name => name;

        /// <inheritdoc/>
        public override Int32 ElementCount => data.ElementCount;

        /// <inheritdoc/>
        public override Int32 SizeInBytes => data.SizeInBytes;

        /// <summary>
        /// Gets the parameter's data container.
        /// </summary>
        public OpenGLEffectParameterData Data => data;

        // Property values.
        private readonly String name;

        // State values.
        private readonly UInt32 type;
        private readonly OpenGLEffectParameterData data;
    }
}
