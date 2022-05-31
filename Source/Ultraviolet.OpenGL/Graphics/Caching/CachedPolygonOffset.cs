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
            var factor = gl.GetFloat(gl.GL_POLYGON_OFFSET_FACTOR);
            gl.ThrowIfError();

            var units = gl.GetFloat(gl.GL_POLYGON_OFFSET_UNITS);
            gl.ThrowIfError();

            return new CachedPolygonOffset(factor, units);
        }

        public static Boolean TryUpdate(ref CachedPolygonOffset current, CachedPolygonOffset desired)
        {
            if (current.factor == desired.factor && current.units == desired.units)
                return false;

            current = desired;
            gl.PolygonOffset(desired.factor, desired.units);
            gl.ThrowIfError();

            return true;
        }

        private readonly Single factor;
        private readonly Single units;
    }
}
