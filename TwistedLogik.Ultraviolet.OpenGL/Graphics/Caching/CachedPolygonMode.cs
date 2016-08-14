using System;
using TwistedLogik.Gluon;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached polygon rasterization mode values.
    /// </summary>
    internal unsafe struct CachedPolygonMode
    {
        public CachedPolygonMode(UInt32 mode)
        {
            this.mode = mode;
        }

        public static implicit operator CachedPolygonMode(UInt32 value) => new CachedPolygonMode(value);

        public static explicit operator UInt32(CachedPolygonMode depth) => depth.mode;

        public static CachedPolygonMode FromDevice()
        {
            if (gl.IsGLES)
            {
                return new CachedPolygonMode(gl.GL_FILL);
            }
            else
            {
                var mode = (UInt32)gl.GetInteger(gl.GL_POLYGON_MODE);
                gl.ThrowIfError();

                return new CachedPolygonMode(mode);
            }
        }

        public static Boolean TryUpdate(ref CachedPolygonMode current, UInt32 desired)
        {
            if (current.mode == desired)
                return false;

            current = desired;
            gl.PolygonMode(gl.GL_FRONT_AND_BACK, desired);
            gl.ThrowIfError();

            return true;
        }

        private readonly UInt32 mode;
    }
}
