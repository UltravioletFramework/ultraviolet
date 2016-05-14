using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the EffectParameter class.
    /// </summary>
    public unsafe sealed class OpenGLEffectParameter : EffectParameter
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLEffectParameter class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The effect parameter's name.</param>
        /// <param name="type">The effect parameter's uniform type.</param>
        public OpenGLEffectParameter(UltravioletContext uv, String name, UInt32 type)
            : base(uv)
        {
            Contract.Require(name, "name");

            this.name = name ?? String.Empty;
            this.type = type;
        }

        /// <inheritdoc/>
        public override Boolean GetValueBoolean()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_BOOL);

            return data.GetBoolean();
        }

        /// <inheritdoc/>
        public override void SetValue(Boolean value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_BOOL);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Int32 GetValueInt32()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            return data.GetInt32();
        }

        /// <inheritdoc/>
        public override void SetValue(Int32 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);
            
            data.Set(value);
        }

        /// <inheritdoc/>
        public override Int32[] GetValueInt32Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            return data.GetInt32Array();
        }

        /// <inheritdoc/>
        public override void SetValue(Int32[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override UInt32 GetValueUInt32()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            return data.GetUInt32();
        }

        /// <inheritdoc/>
        public override void SetValue(UInt32 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override UInt32[] GetValueUInt32Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            return data.GetUInt32Array();
        }

        /// <inheritdoc/>
        public override void SetValue(UInt32[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_UNSIGNED_INT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Single GetValueSingle()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            return data.GetSingle();
        }

        /// <inheritdoc/>
        public override void SetValue(Single value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Single[] GetValueSingleArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            return data.GetSingleArray();
        }

        /// <inheritdoc/>
        public override void SetValue(Single[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Double GetValueDouble()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            return data.GetDouble();
        }

        /// <inheritdoc/>
        public override void SetValue(Double value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Double[] GetValueDoubleArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            return data.GetDoubleArray();
        }

        /// <inheritdoc/>
        public override void SetValue(Double[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_DOUBLE);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector2 GetValueVector2()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            return data.GetVector2();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector2 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector2[] GetValueVector2Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            return data.GetVector2Array();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector2[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC2);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector3 GetValueVector3()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            return data.GetVector3();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector3 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector3[] GetValueVector3Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            return data.GetVector3Array();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector3[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector4 GetValueVector4()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            return data.GetVector4();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector4 value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Vector4[] GetValueVector4Array()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            return data.GetVector4Array();
        }

        /// <inheritdoc/>
        public override void SetValue(Vector4[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Color GetValueColor()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            return data.GetColor();
        }

        /// <inheritdoc/>
        public override void SetValue(Color value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Color[] GetValueColorArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            return data.GetColorArray();
        }

        /// <inheritdoc/>
        public override void SetValue(Color[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_VEC3 || type == gl.GL_FLOAT_VEC4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Matrix GetValueMatrix()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            return data.GetMatrix();
        }

        /// <inheritdoc/>
        public override void SetValue(Matrix value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Matrix[] GetValueMatrixArray()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            return data.GetMatrixArray();
        }

        /// <inheritdoc/>
        public override void SetValue(Matrix[] value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_FLOAT_MAT4);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override Texture2D GetValueTexture2D()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_SAMPLER_2D);

            return data.GetTexture2D();
        }

        /// <inheritdoc/>
        public override void SetValue(Texture2D value)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<InvalidCastException>(type == gl.GL_SAMPLER_2D);

            data.Set(value);
        }

        /// <inheritdoc/>
        public override String Name
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return name; 
            }
        }

        /// <summary>
        /// Gets the parameter's data container.
        /// </summary>
        public OpenGLEffectParameterData Data
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return data;
            }
        }

        // Property values.
        private readonly String name;

        // State values.
        private readonly UInt32 type;
        private readonly OpenGLEffectParameterData data = new OpenGLEffectParameterData();
    }
}
