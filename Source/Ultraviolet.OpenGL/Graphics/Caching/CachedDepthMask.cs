using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_DEPTH_WRITEMASK.
    /// </summary>
    internal unsafe struct CachedDepthMask
    {
        private CachedDepthMask(Boolean value)
        {
            this.value = value;
        }

        public static implicit operator CachedDepthMask(Boolean value) => new CachedDepthMask(value);

        public static explicit operator Boolean(CachedDepthMask mask) => mask.value;

        public static CachedDepthMask FromDevice()
        {
            var value = GL.GetBoolean(GL.GL_DEPTH_WRITEMASK);
            GL.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedDepthMask current, Boolean desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.DepthMask(desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly Boolean value;
    }
}
