using System;
using System.IO;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represent an entry in a <see cref="ContentManager"/> instance's internal content cache.
    /// </summary>
    internal sealed class ContentCacheData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentCacheData"/> class.
        /// </summary>
        /// <param name="asset">The asset which is being stored by the cache.</param>
        /// <param name="origin">The override directory which contains the asset.</param>
        public ContentCacheData(Object asset, String origin)
        {
            this.Asset = asset;
            this.Origin = Path.GetFullPath(origin);
        }

        /// <summary>
        /// Gets the asset which is being stored by the cache.
        /// </summary>
        public Object Asset { get; private set; }

        /// <summary>
        /// Gets the override directory which contains the asset.
        /// </summary>
        public String Origin { get; private set; }
    }
}
