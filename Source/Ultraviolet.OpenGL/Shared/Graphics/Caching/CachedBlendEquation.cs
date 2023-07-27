using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of a blend equation.
    /// </summary>
    internal unsafe struct CachedBlendEquation
    {
        public CachedBlendEquation(UInt32 modeRGB, UInt32 modeAlpha)
        {
            this.modeRGB = modeRGB;
            this.modeAlpha = modeAlpha;
        }

        public static CachedBlendEquation FromDevice()
        {
            var modeRGB = (UInt32)GL.GetInteger(GL.GL_BLEND_EQUATION_RGB);
            GL.ThrowIfError();

            var modeAlpha = (UInt32)GL.GetInteger(GL.GL_BLEND_EQUATION_ALPHA);
            GL.ThrowIfError();

            return new CachedBlendEquation(modeRGB, modeAlpha);
        }

        public static Boolean TryUpdate(ref CachedBlendEquation current, CachedBlendEquation desired)
        {
            if (current.modeRGB == desired.modeRGB && current.modeAlpha == desired.modeAlpha)
                return false;

            current = desired;
            if (desired.modeRGB == desired.modeAlpha)
            {
                GL.BlendEquation(desired.modeRGB);
                GL.ThrowIfError();
            }
            else
            {
                GL.BlendEquationSeparate(desired.modeRGB, desired.modeAlpha);
                GL.ThrowIfError();
            }

            return true;
        }

        private readonly UInt32 modeRGB;
        private readonly UInt32 modeAlpha;
    }
}
