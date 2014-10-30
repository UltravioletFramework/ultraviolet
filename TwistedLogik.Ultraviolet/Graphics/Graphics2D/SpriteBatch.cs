using System;
using System.Security;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SpriteBatch"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of <see cref="SpriteBatch"/> that was created.</returns>
    public delegate SpriteBatch SpriteBatchFactory(UltravioletContext uv);

    /// <summary>
    /// Represents a standard implementation of <see cref="SpriteBatchBase{VertexType, SpriteData}"/> using vertices which
    /// specify position, color, and texture data.
    /// </summary>
    [SecuritySafeCritical]
    public sealed class SpriteBatch : SpriteBatchBase<VertexPositionColorTexture, SpriteBatchData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="batchSize">The maximum number of sprites that can be drawn in a single batch.</param>
        internal SpriteBatch(UltravioletContext uv, Int32 batchSize = 2048)
            : base(uv, batchSize)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="SpriteBatch"/> that was created.</returns>
        public static SpriteBatch Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SpriteBatchFactory>()(uv);
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
            VertexPositionColorTexture[] vertices, SpriteBatchData[] data, Int32 offset, Int32 count)
        {
            CalculateUV(texture);

            fixed (SpriteHeader* pSprites1 = &sprites[offset])
            fixed (SpriteBatchData* pData1 = &data[offset])
            fixed (VertexPositionColorTexture* pVertices1 = &vertices[0])
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
                        CalculatePositionAndTextureCoordinates(pSprites, v,
                            (MutableVector3*)&pVertices->Position,
                            (MutableVector2*)&pVertices->TextureCoordinate);

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
