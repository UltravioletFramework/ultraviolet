using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of a blend function.
    /// </summary>
    internal unsafe struct CachedBlendFunction
    {
        public CachedBlendFunction(UInt32 srcRGB, UInt32 srcAlpha, UInt32 dstRGB, UInt32 dstAlpha)
        {
            this.srcRGB = srcRGB;
            this.srcAlpha = srcAlpha;
            this.dstRGB = dstRGB;
            this.dstAlpha = dstAlpha;
        }

        public static CachedBlendFunction FromDevice()
        {
            var srcRGB = (UInt32)gl.GetInteger(gl.GL_BLEND_SRC_RGB);
            gl.ThrowIfError();

            var srcAlpha = (UInt32)gl.GetInteger(gl.GL_BLEND_SRC_ALPHA);
            gl.ThrowIfError();

            var dstRGB = (UInt32)gl.GetInteger(gl.GL_BLEND_DST_RGB);
            gl.ThrowIfError();

            var dstAlpha = (UInt32)gl.GetInteger(gl.GL_BLEND_DST_ALPHA);
            gl.ThrowIfError();

            return new CachedBlendFunction(srcRGB, srcAlpha, dstRGB, dstAlpha);
        }

        public static Boolean TryUpdate(ref CachedBlendFunction current, CachedBlendFunction desired)
        {
            if (current.srcRGB == desired.srcRGB && current.srcAlpha == desired.srcAlpha &&
                current.dstRGB == desired.dstRGB && current.dstAlpha == desired.dstAlpha)
            {
                return false;
            }

            current = desired;
            if (desired.srcRGB == desired.srcAlpha && desired.dstRGB == desired.dstAlpha)
            {
                gl.BlendFunc(desired.srcRGB, desired.dstRGB);
                gl.ThrowIfError();
            }
            else
            {
                gl.BlendFuncSeparate(desired.srcRGB, desired.dstRGB, desired.srcAlpha, desired.dstAlpha);
                gl.ThrowIfError();
            }

            return true;
        }

        private readonly UInt32 srcRGB;
        private readonly UInt32 srcAlpha;
        private readonly UInt32 dstRGB;
        private readonly UInt32 dstAlpha;
    }
}
