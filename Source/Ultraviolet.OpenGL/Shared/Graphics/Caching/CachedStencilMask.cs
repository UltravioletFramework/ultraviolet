using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_STENCIL_WRITEMASK.
    /// </summary>
    internal unsafe struct CachedStencilMask
    {
        private CachedStencilMask(UInt32 value)
        {
            this.value = value;
        }

        public static implicit operator CachedStencilMask(UInt32 value) => new CachedStencilMask(value);

        public static explicit operator UInt32(CachedStencilMask mask) => mask.value;

        public static CachedStencilMask FromDevice()
        {
            var value = (uint)gl.GetInteger(gl.GL_STENCIL_WRITEMASK);
            gl.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedStencilMask current, UInt32 desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            gl.StencilMask(desired);
            gl.ThrowIfError();

            return true;
        }

        private readonly UInt32 value;
    }
}
