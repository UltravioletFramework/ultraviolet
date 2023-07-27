using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
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
            if (!GL.IsPolygonModeAvailable)
                return new CachedPolygonMode(GL.GL_FILL);

            var modes = stackalloc int[2];
            GL.GetIntegerv(GL.GL_POLYGON_MODE, modes);
            GL.ThrowIfError();

            return new CachedPolygonMode((UInt32)modes[0]);
        }

        public static Boolean TryUpdate(ref CachedPolygonMode current, UInt32 desired)
        {
            if (current.mode == desired)
                return false;

            current = desired;
            GL.PolygonMode(GL.GL_FRONT_AND_BACK, desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 mode;
    }
}
