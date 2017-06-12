using System;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represent an entry in a <see cref="ContentManager"/> instance's internal content cache.
    /// </summary>
    internal struct AssetCacheEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCacheEntry"/> class.
        /// </summary>
        /// <param name="asset">The asset which is being stored by the cache.</param>
        /// <param name="assetType">The type as which the asset was loaded.</param>
        /// <param name="origin">The override directory which contains the asset.</param>
        public AssetCacheEntry(Object asset, Type assetType, String origin)
        {
            this.Asset = asset;
            this.AssetType = assetType;
            this.Origin = origin == null ? null : Path.GetFullPath(origin);
        }

        /// <summary>
        /// Gets the asset which is being stored by the cache.
        /// </summary>
        public Object Asset { get; private set; }

        /// <summary>
        /// Gets the type as which the asset was loaded.
        /// </summary>
        public Type AssetType { get; private set; }

        /// <summary>
        /// Gets the override directory which contains the asset.
        /// </summary>
        public String Origin { get; private set; }        
    }
}
