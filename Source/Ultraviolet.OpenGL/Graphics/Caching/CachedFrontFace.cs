using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_FRONT_FACE.
    /// </summary>
    internal unsafe struct CachedFrontFace
    {
        private CachedFrontFace(UInt32 mode)
        {
            this.mode = mode;
        }

        public static implicit operator CachedFrontFace(UInt32 mode) => new CachedFrontFace(mode);

        public static explicit operator UInt32(CachedFrontFace face) => face.mode;

        public static CachedFrontFace FromDevice()
        {
            var value = (UInt32)GL.GetInteger(GL.GL_FRONT_FACE);
            GL.ThrowIfError();

            return value;
        }

        public static Boolean TryUpdate(ref CachedFrontFace current, UInt32 desired)
        {
            if (current.mode == desired)
                return false;

            current = desired;
            GL.FrontFace(desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 mode;
    }
}
