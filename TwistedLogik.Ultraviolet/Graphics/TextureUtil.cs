using System;
using System.Security;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Contains utility methods for manipulating texture data.
    /// </summary>
    [CLSCompliant(false)]
    [SecurityCritical]
    public static class TextureUtil
    {
        /// <summary>
        /// Reorients the data in the specified buffer, flipping it horizontally, vertically, or both.
        /// </summary>
        /// <param name="buffer">The buffer to reorient.</param>
        /// <param name="width">The width in pixels of the texture represented by the buffer.</param>
        /// <param name="height">The height in pixels of the texture represented by the buffer.</param>
        /// <param name="size">The size in bytes of one element of the buffer.</param>
        /// <param name="flipHorizontally">A value indicating whether to flip the data horizontally.</param>
        /// <param name="flipVertically">A value indicating whether to flip the data vertically.</param>
        public static unsafe void ReorientTextureData(void* buffer, Int32 width, Int32 height, Int32 size, Boolean flipHorizontally, Boolean flipVertically)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (!flipHorizontally && !flipVertically)
                return;

            var bytes  = (byte*)buffer;
            var index  = 0;

            var srcX = 0;
            var srcY = 0;

            var rowsToProcess = (height % 2 == 0) ? height / 2 : 1 + height / 2;
            for (int row = 0; row < rowsToProcess; row++)
            {
                srcX = 0;

                for (int col = 0; col < width; col++)
                {
                    var dstX = flipHorizontally ? (width - (1 + srcX)) : srcX;
                    var dstY = flipVertically ? (height - (1 + srcY)) : srcY;

                    var srcIx = size * index;
                    var dstIx = size * ((dstY * width) + dstX);

                    var pSrc = bytes + srcIx;
                    var pDst = bytes + dstIx;

                    for (int offset = 0; offset < size; offset++)
                    {
                        var srcVal = *pSrc;
                        var dstVal = *pDst;

                        *pSrc++ = dstVal;
                        *pDst++ = srcVal;
                    }

                    srcX++;
                    index++;
                }

                srcY++;
            }
        }
    }
}
