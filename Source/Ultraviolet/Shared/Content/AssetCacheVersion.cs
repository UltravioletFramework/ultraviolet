using System;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a particular version of a content asset which is cached within a <see cref="ContentManager"/> instance.
    /// </summary>
    internal struct AssetCacheVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCacheVersion"/> structure.
        /// </summary>
        /// <param name="assetDensityBucket">The density bucket associated with this asset.</param>
        /// <param name="assetOrigin">The origin path of the asset.</param>
        /// <param name="assetInstance">The asset instance.</param>
        public AssetCacheVersion(ScreenDensityBucket assetDensityBucket, String assetOrigin, Object assetInstance)
        {
            this.AssetDensityBucket = assetDensityBucket;
            this.AssetOrigin = assetOrigin;
            this.AssetInstance = assetInstance;
        }

        /// <summary>
        /// Reloads the asset represented by this version structure.
        /// </summary>
        /// <param name="content">The content manager with which to reload the cache entry.</param>
        /// <param name="assetPath">The asset path that represents the asset.</param>
        /// <param name="assetWatchers">The asset watchers that are watching the asset.</param>
        /// <param name="assetType">The asset type.</param>
        public void Reload(ContentManager content, String assetPath, IAssetWatcherCollection assetWatchers, Type assetType)
        {
            var assetLKG = content.LoadImpl(assetPath, assetType, AssetDensityBucket, true, true, assetWatchers, null);
            content.AssetCache.PurgeAsset(assetPath, false);
            content.LoadImpl(assetPath, assetType, AssetDensityBucket, true, true, assetWatchers, assetLKG);
        }

        /// <summary>
        /// Gets the density bucket associated with this asset.
        /// </summary>
        public ScreenDensityBucket AssetDensityBucket { get; }

        /// <summary>
        /// Gets the origin path of the asset, if it was loaded from an override directory,
        /// or <see langword="null"/> if it was loaded from the main content directory.
        /// </summary>
        public String AssetOrigin { get; }

        /// <summary>
        /// Gets the asset instance.
        /// </summary>
        public Object AssetInstance { get; }
    }
}
