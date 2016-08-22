using System;
using System.Security;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a standard implementation of <see cref="SpriteBatchBase{VertexType, SpriteData}"/> using vertices which
    /// specify position, color, and texture data. This version of <see cref="SpriteBatch"/> is used on platforms which
    /// don't support non-integral vertex attributes -- we have to normalize our texture coordinates on the CPU, resulting in
    /// a loss of precision.
    /// </summary>
    [SecuritySafeCritical]
    internal class SpriteBatchFallback : SpriteBatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchFallback"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="batchSize">The maximum number of sprites that can be drawn in a single batch.</param>
        internal SpriteBatchFallback(UltravioletContext uv, Int32 batchSize = 2048)
            : base(uv, batchSize)
        {

        }
        
        /// <summary>
        /// Generates vertices for a group of sprites.
        /// </summary>
        /// <param name="texture">The batch's texture.</param>
        /// <param name="sprites">The batch's sprite metadata array.</param>
        /// <param name="vertices">The batch's vertex data array.</param>
        /// <param name="data">The batch's custom data array.</param>
        /// <param name="offset">The offset of the first sprite being drawn.</param>
        /// <param name="count">The number of sprites being drawn.</param>
        /// <returns>The vertex stride.</returns>
        [SecuritySafeCritical]
        protected override unsafe void GenerateVertices(Texture2D texture, SpriteHeader[] sprites,
            SpriteVertex[] vertices, SpriteBatchData[] data, Int32 offset, Int32 count)
        {
            CalculateUV(texture);

            fixed (SpriteHeader* pSprites1 = &sprites[offset])
            fixed (SpriteBatchData* pData1 = &data[offset])
            fixed (SpriteVertex* pVertices1 = &vertices[0])
            {
                var pSprites = pSprites1;
                var pData = pData1;
                var pVertices = pVertices1;

                for (int i = 0; i < count; i++)
                {
                    CalculateSinAndCos(pSprites->Rotation);
                    CalculateRelativeOrigin(pSprites);

                    for (int v = 0; v < 4; v++)
                    {
                        CalculatePositionAndNormalizableTextureCoordinates(pSprites, v,
                            (MutableVector2*)&pVertices->Position, &pVertices->U, &pVertices->V);

                        pVertices->Color = pSprites->Color;
                        pVertices++;
                    }

                    pSprites++;
                    pData++;
                }
            }
        }
    }
}
