using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the metadata for a batch of sprites.
    /// </summary>
    /// <typeparam name="SpriteData">The type of data object associated with each of the batch's sprite instances.</typeparam>
    internal sealed partial class SpriteBatchInfo<SpriteData> where SpriteData : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchInfo{SpriteData}"/> class.
        /// </summary>
        /// <param name="batchSize">The sprite batch size.</param>
        public SpriteBatchInfo(Int32 batchSize)
        {
            Contract.EnsureRange(batchSize > 0, nameof(batchSize));

            this.headers = new SpriteHeader[batchSize];
            this.data = new SpriteData[batchSize];
            this.textures = new Texture2D[batchSize];

            this.sortTexture = new FunctorComparer<Int32>((x, y) => { return textures[x].CompareTo(textures[y]); });
            this.sortBackToFront = new FunctorComparer<Int32>((x, y) => { return headers[y].Depth.CompareTo(headers[x].Depth); });
            this.sortFrontToBack = new FunctorComparer<Int32>((x, y) => { return headers[x].Depth.CompareTo(headers[y].Depth); });
        }

        /// <summary>
        /// Reserves space in the batch's buffers for a sprite.
        /// </summary>
        /// <param name="texture">The sprite's texture.</param>
        /// <param name="customData">The sprite's custom data.</param>
        /// <returns>The index of the sprite that was reserved.</returns>
        public Int32 Reserve(Texture2D texture, ref SpriteData customData)
        {
            if (count >= headers.Length)
                ExpandBuffers();

            this.textures[count] = texture;
            this.data[count] = customData;

            return count++;
        }

        /// <summary>
        /// Clears the batch's data.
        /// </summary>
        public void Clear()
        {
            Array.Clear(textures, 0, count);
            count = 0;
            sorted = false;
        }

        /// <summary>
        /// Sorts the batch's sprites.
        /// </summary>
        /// <param name="spriteSortMode">The sprite sort mode.</param>
        public void Sort(SpriteSortMode spriteSortMode)
        {
            EnsureSortedBuffers();

            switch (spriteSortMode)
            {
                case SpriteSortMode.Texture:
                    Array.Sort(order, 0, count, sortTexture);
                    break;

                case SpriteSortMode.BackToFront:
                    Array.Sort(order, 0, count, sortBackToFront);
                    break;

                case SpriteSortMode.FrontToBack:
                    Array.Sort(order, 0, count, sortFrontToBack);
                    break;

                default:
                    throw new NotSupportedException();
            }

            PopulateSortedBuffers();
            sorted = true;
        }

        /// <summary>
        /// Gets the batch's sprite header array.
        /// </summary>
        /// <returns>The batch's sprite header array.</returns>
        public SpriteHeader[] GetHeaders()
        {
            return sorted ? headersSorted : headers;
        }

        /// <summary>
        /// Gets the batch's sprite data array.
        /// </summary>
        /// <returns>The batch's sprite data array.</returns>
        public SpriteData[] GetData()
        {
            return sorted ? dataSorted : data;
        }

        /// <summary>
        /// Gets the texture for the specified sprite.
        /// </summary>
        /// <param name="ix">The index of the sprite for which to retrieve a texture.</param>
        /// <returns>The texture for the specified sprite.</returns>
        public Texture2D GetTexture(Int32 ix)
        {
            return sorted ? textures[order[ix]] : textures[ix];
        }

        /// <summary>
        /// Gets the number of sprites in the batch.
        /// </summary>
        public Int32 Count
        {
            get { return count; }
        }

        /// <summary>
        /// Expands the batch's buffers.
        /// </summary>
        private void ExpandBuffers()
        {
            var size = headers.Length * 2;
            Array.Resize(ref headers, size);
            Array.Resize(ref data, size);
            Array.Resize(ref textures, size);
        }

        /// <summary>
        /// Ensures that the sorted buffers exist and are of the appropriate size.
        /// </summary>
        private void EnsureSortedBuffers()
        {
            if (dataSorted == null || dataSorted.Length < data.Length)
                dataSorted = new SpriteData[data.Length];

            if (headersSorted == null || headersSorted.Length < headers.Length)
                headersSorted = new SpriteHeader[headers.Length];

            if (order == null || order.Length < data.Length)
                order = new Int32[data.Length];

            for (int i = 0; i < count; i++)
                order[i] = i;
        }

        /// <summary>
        /// Populates the batch's sorted buffers based on the current render order.
        /// </summary>
        private void PopulateSortedBuffers()
        {
            for (int i = 0; i < count; i++)
            {
                var sortedIx = order[i];
                dataSorted[i] = data[sortedIx];
                headersSorted[i] = headers[sortedIx];
            }
        }

        // Batch data.
        private Int32 count;
        private Boolean sorted;
        private SpriteHeader[] headers;
        private SpriteHeader[] headersSorted = null;
        private SpriteData[] data;
        private SpriteData[] dataSorted = null;
        private Texture2D[] textures;
        private Int32[] order;

        // Sorting comparisons.
        private readonly Comparer<Int32> sortTexture;
        private readonly Comparer<Int32> sortBackToFront;
        private readonly Comparer<Int32> sortFrontToBack;
    }
}
