using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_STENCIL_CLEAR_VALUE.
    /// </summary>
    internal unsafe struct CachedClearStencil
    {
        private CachedClearStencil(Int32 value)
        {
            this.value = value;
        }

        public static implicit operator CachedClearStencil(Int32 value) => new CachedClearStencil(value);

        public static explicit operator Int32(CachedClearStencil stencil) => stencil.value;

        public static CachedClearStencil FromDevice()
        {
            var value = gl.GetInteger(gl.GL_STENCIL_CLEAR_VALUE);
            gl.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedClearStencil current, Int32 desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            gl.ClearStencil(desired);
            gl.ThrowIfError();

            return true;
        }

        private readonly Int32 value;
    }
}
