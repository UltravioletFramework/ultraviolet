using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached polygon offset values.
    /// </summary>
    internal unsafe struct CachedPolygonOffset
    {
        public CachedPolygonOffset(Single factor, Single units)
        {
            this.factor = factor;
            this.units = units;
        }

        public static CachedPolygonOffset FromDevice()
        {
            var factor = GL.GetFloat(GL.GL_POLYGON_OFFSET_FACTOR);
            GL.ThrowIfError();

            var units = GL.GetFloat(GL.GL_POLYGON_OFFSET_UNITS);
            GL.ThrowIfError();

            return new CachedPolygonOffset(factor, units);
        }

        public static Boolean TryUpdate(ref CachedPolygonOffset current, CachedPolygonOffset desired)
        {
            if (current.factor == desired.factor && current.units == desired.units)
                return false;

            current = desired;
            GL.PolygonOffset(desired.factor, desired.units);
            GL.ThrowIfError();

            return true;
        }

        private readonly Single factor;
        private readonly Single units;
    }
}
