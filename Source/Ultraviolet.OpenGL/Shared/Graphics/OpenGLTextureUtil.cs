using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains utility methods for working with OpenGL textures.
    /// </summary>
    internal static class OpenGLTextureUtil
    {
        /// <summary>
        /// Gets the OpenGL internal texture format that corresponds to the specified number of bytes per pixel.
        /// </summary>
        public static UInt32 GetInternalFormatFromBytesPerPixel(Int32 bytesPerPixel, Boolean srgbEncoded)
        {
            if (bytesPerPixel == 4)
            {
                if (srgbEncoded)
                    return gl.IsGLES2 ? gl.GL_SRGB_ALPHA : gl.GL_SRGB8_ALPHA8;

                return gl.IsGLES2 ? gl.GL_RGBA : gl.GL_RGBA8;
            }

            if (bytesPerPixel == 3)
            {
                if (srgbEncoded)
                    return gl.IsGLES2 ? gl.GL_SRGB : gl.GL_SRGB8;

                return gl.GL_RGB8;
            }

            return gl.GL_NONE;
        }

        /// <summary>
        /// Gets the OpenGL texture format that corresponds to the specified number of bytes per pixel.
        /// </summary>
        public static UInt32 GetFormatFromBytesPerPixel(Int32 bytesPerPixel)
        {
            if (bytesPerPixel == 4)
                return gl.GL_RGBA;

            if (bytesPerPixel == 3)
                return gl.GL_RGB;

            return gl.GL_NONE;
        }
    }
}
