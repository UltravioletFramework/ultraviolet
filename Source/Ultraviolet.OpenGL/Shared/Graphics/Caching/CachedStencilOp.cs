using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of a stencil operation.
    /// </summary>
    internal unsafe struct CachedStencilOp
    {
        public CachedStencilOp(UInt32 sfail, UInt32 dpfail, UInt32 dppass)
        {
            this.sfail = sfail;
            this.dpfail = dpfail;
            this.dppass = dppass;
        }

        public static CachedStencilOp FromDevice(UInt32 face)
        {
            switch (face)
            {
                case GL.GL_NONE:
                case GL.GL_FRONT:
                    {
                        var sfail = (UInt32)GL.GetInteger(GL.GL_STENCIL_FAIL);
                        GL.ThrowIfError();

                        var dpfail = (UInt32)GL.GetInteger(GL.GL_STENCIL_PASS_DEPTH_FAIL);
                        GL.ThrowIfError();

                        var dppass = (UInt32)GL.GetInteger(GL.GL_STENCIL_PASS_DEPTH_PASS);
                        GL.ThrowIfError();

                        return new CachedStencilOp(sfail, dpfail, dppass);
                    }

                case GL.GL_BACK:
                    {
                        var sfail = (UInt32)GL.GetInteger(GL.GL_STENCIL_BACK_FAIL);
                        GL.ThrowIfError();

                        var dpfail = (UInt32)GL.GetInteger(GL.GL_STENCIL_BACK_PASS_DEPTH_FAIL);
                        GL.ThrowIfError();

                        var dppass = (UInt32)GL.GetInteger(GL.GL_STENCIL_BACK_PASS_DEPTH_PASS);
                        GL.ThrowIfError();

                        return new CachedStencilOp(sfail, dpfail, dppass);
                    }

                default:
                    throw new ArgumentException(nameof(face));
            }
        }

        public static Boolean TryUpdate(UInt32 face, ref CachedStencilOp current, CachedStencilOp desired)
        {
            if (current.sfail == desired.sfail && current.dpfail == desired.dpfail && current.dppass == desired.dppass)
                return false;

            current = desired;
            GL.StencilOpSeparate(face, desired.sfail, desired.dpfail, desired.dppass);
            GL.ThrowIfError();

            return true;
        }

        public static Boolean TryUpdateCombined(ref CachedStencilOp front, ref CachedStencilOp back, CachedStencilOp desired)
        {
            if (front.sfail == back.sfail && back.sfail == desired.sfail &&
                front.dpfail == back.dpfail && back.dpfail == desired.dpfail &&
                front.dppass == back.dppass && back.dppass == desired.dppass)
            {
                return false;
            }

            front = desired;
            back = desired;
            GL.StencilOp(desired.sfail, desired.dpfail, desired.dppass);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 sfail;
        private readonly UInt32 dpfail;
        private readonly UInt32 dppass;
    }
}
