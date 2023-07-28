using System;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of GL_COLOR_WRITEMASK.
    /// </summary>
    internal unsafe struct CachedColorMask
    {
        private CachedColorMask(ColorWriteChannels value)
        {
            this.value = value;
        }

        public static implicit operator CachedColorMask(ColorWriteChannels value) => new CachedColorMask(value);

        public static explicit operator ColorWriteChannels(CachedColorMask mask) => mask.value;

        public static CachedColorMask FromDevice()
        {
            var colorMaskComponents = stackalloc bool[4];
            GL.GetBooleanv(GL.GL_COLOR_WRITEMASK, colorMaskComponents);
            GL.ThrowIfError();
            
            return
                (colorMaskComponents[0] ? ColorWriteChannels.Red : ColorWriteChannels.None) |
                (colorMaskComponents[1] ? ColorWriteChannels.Green : ColorWriteChannels.None) |
                (colorMaskComponents[2] ? ColorWriteChannels.Blue : ColorWriteChannels.None) |
                (colorMaskComponents[3] ? ColorWriteChannels.Alpha : ColorWriteChannels.None);
        }

        public static Boolean TryUpdate(ref CachedColorMask current, ColorWriteChannels desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.ColorMask(
                (desired & ColorWriteChannels.Red) == ColorWriteChannels.Red,
                (desired & ColorWriteChannels.Green) == ColorWriteChannels.Green,
                (desired & ColorWriteChannels.Blue) == ColorWriteChannels.Blue,
                (desired & ColorWriteChannels.Alpha) == ColorWriteChannels.Alpha);
            GL.ThrowIfError();

            return true;
        }

        private readonly ColorWriteChannels value;
    }
}
