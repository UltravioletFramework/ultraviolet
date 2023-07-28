using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_BLEND_COLOR.
    /// </summary>
    internal unsafe struct CachedBlendColor
    {
        private CachedBlendColor(Color value)
        {
            this.value = value;
        }

        public static implicit operator CachedBlendColor(Color value) => new CachedBlendColor(value);

        public static explicit operator Color(CachedBlendColor color) => color.value;

        public static CachedBlendColor FromDevice()
        {
            var components = stackalloc float[4];
            GL.GetFloatv(GL.GL_BLEND_COLOR, components);
            GL.ThrowIfError();

            return new Color(
                (Byte)(components[0] * Byte.MaxValue),
                (Byte)(components[1] * Byte.MaxValue),
                (Byte)(components[2] * Byte.MaxValue),
                (Byte)(components[3] * Byte.MaxValue));
        }

        public static Boolean TryUpdate(ref CachedBlendColor current, Color desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.BlendColor(desired.R / 255f, desired.G / 255f, desired.B / 255f, desired.A / 255f);
            GL.ThrowIfError();

            return true;
        }

        private readonly Color value;
    }
}
