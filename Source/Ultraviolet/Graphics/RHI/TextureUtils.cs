using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Texture helper methids.
    /// </summary>
    public static class TextureUtils
    {
        /// <summary>
        /// Gets the texture format given a surface format and bytes per pixel
        /// </summary>
        /// <param name="surfaceFormat">The surface format</param>
        /// <param name="bytesPerPixel">The bytes per pixel used</param>
        /// <returns></returns>
        public static TextureFormat GetTextureFormatFromSurfaceFormat(SurfaceSourceDataFormat surfaceFormat, int bytesPerPixel)
        {
            if (bytesPerPixel == 4) {
                switch (surfaceFormat)
                {
                    case SurfaceSourceDataFormat.RGBA:
                        return TextureFormat.RGBA;

                    case SurfaceSourceDataFormat.BGRA:
                        return TextureFormat.BGRA;
                }
            }
            if (bytesPerPixel == 3)
            {
                switch (surfaceFormat)
                {
                    case SurfaceSourceDataFormat.RGBA:
                        return TextureFormat.RGB;

                    case SurfaceSourceDataFormat.BGRA:
                        return TextureFormat.BGR;
                }
            }
            return TextureFormat.RGBA;
        }
    }
}
