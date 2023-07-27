using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_DEPTH_FUNC.
    /// </summary>
    internal unsafe struct CachedDepthFunc
    {
        private CachedDepthFunc(UInt32 value)
        {
            this.value = value;
        }

        public static implicit operator CachedDepthFunc(UInt32 value) => new CachedDepthFunc(value);

        public static explicit operator UInt32(CachedDepthFunc mask) => mask.value;

        public static CachedDepthFunc FromDevice()
        {
            var value = (UInt32)GL.GetInteger(GL.GL_DEPTH_FUNC);
            GL.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedDepthFunc current, UInt32 desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.DepthFunc(desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 value;
    }
}
