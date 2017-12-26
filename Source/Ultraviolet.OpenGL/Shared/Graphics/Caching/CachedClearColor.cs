using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_COLOR_CLEAR_VALUE.
    /// </summary>
    internal unsafe struct CachedClearColor
    {
        private CachedClearColor(Color value)
        {
            this.value = value;
        }

        public static implicit operator CachedClearColor(Color value) => new CachedClearColor(value);

        public static explicit operator Color(CachedClearColor color) => color.value;

        public static CachedClearColor FromDevice()
        {
            var components = stackalloc float[4];
            gl.GetFloatv(gl.GL_COLOR_CLEAR_VALUE, components);
            gl.ThrowIfError();

            return new Color(
                (Byte)(components[0] * Byte.MaxValue),
                (Byte)(components[1] * Byte.MaxValue),
                (Byte)(components[2] * Byte.MaxValue),
                (Byte)(components[3] * Byte.MaxValue));
        }

        public static Boolean TryUpdate(ref CachedClearColor current, Color desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            gl.ClearColor(desired.R / 255f, desired.G / 255f, desired.B / 255f, desired.A / 255f);
            gl.ThrowIfError();

            return true;
        }

        private readonly Color value;
    }
}
