using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_DEPTH_CLEAR_VALUE.
    /// </summary>
    internal unsafe struct CachedClearDepth
    {
        private CachedClearDepth(Double value)
        {
            this.value = value;
        }

        public static implicit operator CachedClearDepth(Double value) => new CachedClearDepth(value);

        public static explicit operator Double(CachedClearDepth depth) => depth.value;

        public static CachedClearDepth FromDevice()
        {
            var value = gl.IsDoublePrecisionClearDepthAvailable ? gl.GetDouble(gl.GL_DEPTH_CLEAR_VALUE) : gl.GetFloat(gl.GL_DEPTH_CLEAR_VALUE);
            gl.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedClearDepth current, Double desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            gl.ClearDepth(desired);
            gl.ThrowIfError();

            return true;
        }

        private readonly Double value;
    }
}
