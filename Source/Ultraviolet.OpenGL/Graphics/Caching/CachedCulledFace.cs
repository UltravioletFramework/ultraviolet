using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_CULL_FACE_MODE.
    /// </summary>
    internal unsafe struct CachedCulledFace
    {
        private CachedCulledFace(UInt32 mode)
        {
            this.mode = mode;
        }

        public static implicit operator CachedCulledFace(UInt32 mode) => new CachedCulledFace(mode);

        public static explicit operator UInt32(CachedCulledFace face) => face.mode;

        public static CachedCulledFace FromDevice()
        {
            var value = (UInt32)GL.GetInteger(GL.GL_CULL_FACE_MODE);
            GL.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedCulledFace current, UInt32 desired)
        {
            if (current.mode == desired)
                return false;

            current = desired;
            GL.CullFace(desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 mode;
    }
}
