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
            var value = GL.GetInteger(GL.GL_STENCIL_CLEAR_VALUE);
            GL.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedClearStencil current, Int32 desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.ClearStencil(desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly Int32 value;
    }
}
