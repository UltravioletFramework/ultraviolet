using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of a stencil function.
    /// </summary>
    internal unsafe struct CachedStencilFunc
    {
        public CachedStencilFunc(UInt32 func, Int32 reference, Int32 mask)
        {
            this.func = func;
            this.reference = reference;
            this.mask = mask;
        }

        public static CachedStencilFunc FromDevice(UInt32 face)
        {
            switch (face)
            {
                case GL.GL_NONE:
                case GL.GL_FRONT:
                    {
                        var func = (UInt32)GL.GetInteger(GL.GL_STENCIL_FUNC);
                        GL.ThrowIfError();

                        var reference = GL.GetInteger(GL.GL_STENCIL_REF);
                        GL.ThrowIfError();

                        var mask = GL.GetInteger(GL.GL_STENCIL_VALUE_MASK);
                        GL.ThrowIfError();

                        return new CachedStencilFunc(func, reference, mask);
                    }

                case GL.GL_BACK:
                    {
                        var func = (UInt32)GL.GetInteger(GL.GL_STENCIL_BACK_FUNC);
                        GL.ThrowIfError();

                        var reference = GL.GetInteger(GL.GL_STENCIL_BACK_REF);
                        GL.ThrowIfError();

                        var mask = GL.GetInteger(GL.GL_STENCIL_BACK_VALUE_MASK);
                        GL.ThrowIfError();

                        return new CachedStencilFunc(func, reference, mask);
                    }

                default:
                    throw new ArgumentException(nameof(face));
            }
        }

        public static Boolean TryUpdate(UInt32 face, ref CachedStencilFunc current, CachedStencilFunc desired)
        {
            if (current.func == desired.func && current.reference == desired.reference && current.mask == desired.mask)
                return false;

            current = desired;
            GL.StencilFuncSeparate(face, desired.func, desired.reference, (UInt32)desired.mask);
            GL.ThrowIfError();

            return true;
        }

        public static Boolean TryUpdateCombined(ref CachedStencilFunc front, ref CachedStencilFunc back, CachedStencilFunc desired)
        {
            if (front.func == back.func && back.func == desired.func &&
                front.reference == back.reference && back.reference == desired.reference &&
                front.mask == back.mask && back.mask == desired.mask)
            {
                return false;
            }

            front = desired;
            back = desired;
            GL.StencilFunc(desired.func, desired.reference, (UInt32)desired.mask);
            GL.ThrowIfError();

            return true;
        }

        private readonly UInt32 func;
        private readonly Int32 reference;
        private readonly Int32 mask;
    }
}
