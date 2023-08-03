using System;
using Ultraviolet.Graphics;
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
        public static UInt32 GetInternalGLFormatFromTextureFormat(TextureFormat format, Boolean srgbEncoded)
        {
            if (format == TextureFormat.RGBA || format == TextureFormat.BGRA)
            {
                if (srgbEncoded)
                    return GL.IsSizedTextureInternalFormatAvailable ? GL.GL_SRGB8_ALPHA8 : GL.GL_SRGB_ALPHA;

                return GL.IsSizedTextureInternalFormatAvailable ? GL.GL_RGBA8 : GL.GL_RGBA;
            }

            if (format == TextureFormat.RGB || format == TextureFormat.BGR)
            {
                if (srgbEncoded)
                    return GL.IsSizedTextureInternalFormatAvailable ? GL.GL_SRGB8 : GL.GL_SRGB;
                
                return GL.IsSizedTextureInternalFormatAvailable ? GL.GL_RGB8 : GL.GL_RGB;
            }

            return GL.GL_NONE;
        }

        /// <summary>
        /// Gets the OpenGL texture format that corresponds to the specified number of bytes per pixel.
        /// </summary>
        public static UInt32 GetGLFormatFromTextureFormat(TextureFormat format)
        {
            if (format == TextureFormat.RGBA || format == TextureFormat.BGRA)
                return GL.GL_RGBA;

            if (format == TextureFormat.RGB || format == TextureFormat.BGR)
                return GL.GL_RGB;

            return GL.GL_NONE;
        }

        /// <summary>
        /// Gets the component count of the given texture format.
        /// </summary>
        public static Int32 GetComponentCountFromTextureFormat(TextureFormat format)
        {
            if (format == TextureFormat.RGBA || format == TextureFormat.BGRA)
                return 4;

            if (format == TextureFormat.RGB || format == TextureFormat.BGR)
                return 3;

            return 0;
        }
    }
}
