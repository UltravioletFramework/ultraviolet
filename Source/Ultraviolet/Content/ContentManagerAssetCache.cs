using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a cache of content assets held by an instance of the <see cref="ContentManager"/> class.
    /// </summary>
    public class ContentManagerAssetCache : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManagerAssetCache"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="contentManager">The <see cref="ContentManager"/> instance that owns this asset cache.</param>
        internal ContentManagerAssetCache(UltravioletContext uv, ContentManager contentManager)
            : base(uv)
        {
            Contract.Require(contentManager, nameof(contentManager));

            this.ContentManager = contentManager;
        }

        /// <summary>
        /// Purges the content manager's internal cache, removing all references to previously loaded objects
        /// so that they can be collected.
        /// </summary>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void Purge(Boolean lowMemory)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
            {
                foreach (var kvp in assetFlags)
                {
                    var asset = kvp.Key;
                    var flags = kvp.Value;

                    var preserve = (flags & AssetFlags.PreserveThroughLowMemory) == AssetFlags.PreserveThroughLowMemory;
                    if (preserve && lowMemory)
                        continue;

                    assetCache.Remove(asset);

                    ContentManager.Dependencies.ClearAssetDependencies(asset);
                }
            }
        }

        /// <summary>
        /// Purges the specified asset from the content manager's internal cache.
        /// </summary>
        /// <param name="asset">The asset to purge from the cache.</param>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void PurgeAsset(String asset, Boolean lowMemory)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
            {
                if (!GetAssetFlags(asset, out var flags))
                    return;

                var preserve = (flags & AssetFlags.PreserveThroughLowMemory) == AssetFlags.PreserveThroughLowMemory;
                if (preserve && lowMemory)
                    return;

                assetCache.Remove(asset);
                
                ContentManager.Dependencies.ClearAssetDependencies(asset);
            }
        }

        /// <summary>
        /// Purges the specified asset from the content manager's internal cache.
        /// </summary>
        /// <param name="asset">The asset to purge from the cache.</param>
        /// <param name="lowMemory">A value indicating whether the cache is being purged due to the operating system
        /// being low on memory. If this value is <see langword="true"/>, then assets which have the 
        /// <see cref="AssetFlags.PreserveThroughLowMemory"/> flag will be ignored by this method. Otherwise,
        /// all of the cache's assets will be purged.</param>
        public void PurgeAsset(AssetID asset, Boolean lowMemory)
        {
            Contract.Ensure<ArgumentException>(asset.IsValid, nameof(asset));

            PurgeAsset(AssetID.GetAssetPath(asset), lowMemory);
        }

        /// <summary>
        /// Purges any asset versions for screen densities which are not in use.
        /// </summary>
        public void PurgeUnusedScreenDensities()
        {
            var usedScreenDensities = Ultraviolet.GetPlatform().Displays.Select(x => x.DensityBucket).Distinct().ToArray();
            var purgedCacheEntries = new List<String>();

            foreach (var kvp in assetCache)
            {
                if (kvp.Value.PurgeUnusedVersions(usedScreenDensities))
                    purgedCacheEntries.Add(kvp.Key);
            }

            foreach (var purgedCacheEntry in purgedCacheEntries)
                PurgeAsset(purgedCacheEntry, false);
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> instance that owns this asset cache.
        /// </summary>
        public ContentManager ContentManager { get; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var instance in assetCache)
                    instance.Value.Dispose();

                assetCache.Clear();
                assetFlags.Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Attempts to retrieve the cache entry for the specified asset.
        /// </summary>
        /// <param name="asset">The asset path of the asset for which to retrieve a cache entry.</param>
        /// <param name="entry">The cache entry for the specified asset, if it exists.</param>
        /// <returns><see langword="true"/> if a cache entry was found for the specified asset; otherwise, <see langword="false"/>.</returns>
        internal Boolean TryGetCacheEntry(String asset, out AssetCacheEntry entry) => assetCache.TryGetValue(asset, out entry);

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <remarks>Please note that, for performance reasons, the content manager's internal cache does not normalize asset paths.
        /// This means that if you reference the same asset by two different but equivalent paths (i.e. "foo/bar" and "foo\\bar"), 
        /// each of those paths will represent a <b>separate entry in the cache</b> with <b>separate asset flags</b>.</remarks>
        /// <param name="asset">The asset path of the asset for which to set flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> values to associate with the specified asset.</param>
        internal void SetAssetFlags(String asset, AssetFlags flags)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
                assetFlags[asset] = flags;
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to set flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> values to associate with the specified asset.</param>
        internal void SetAssetFlags(AssetID asset, AssetFlags flags)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
                assetFlags[AssetID.GetAssetPath(asset)] = flags;
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <remarks>Please note that, for performance reasons, the content manager's internal cache does not normalize asset paths.
        /// This means that if you reference the same asset by two different but equivalent paths (i.e. "foo/bar" and "foo\\bar"), 
        /// each of those paths will represent a <b>separate entry in the cache</b> with <b>separate asset flags</b>.</remarks>
        /// <param name="asset">The asset path of the asset for which to retrieve flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> value associated with the specified asset.</param>
        /// <returns><see langword="true"/> if the specified asset has flags defined within this 
        /// content manager; otherwise, <see langword="false"/>.</returns>
        internal Boolean GetAssetFlags(String asset, out AssetFlags flags)
        {
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
                return assetFlags.TryGetValue(asset, out flags);
        }

        /// <summary>
        /// Sets the flags associated with the specified asset.
        /// </summary>
        /// <param name="asset">The asset identifier of the asset for which to retrieve flags.</param>
        /// <param name="flags">A collection of <see cref="AssetFlags"/> value associated with the specified asset.</param>
        /// <returns><see langword="true"/> if the specified asset has flags defined within this 
        /// content manager; otherwise, <see langword="false"/>.</returns>
        internal Boolean GetAssetFlags(AssetID asset, out AssetFlags flags)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lock (SyncObject)
                return assetFlags.TryGetValue(AssetID.GetAssetPath(asset), out flags);
        }

        /// <summary>
        /// Gets the cache's synchronization object.
        /// </summary>
        internal Object SyncObject { get; } = new Object();

        /// <summary>
        /// Updates the content manager's internal cache with the specified object instance.
        /// </summary>
        internal void UpdateCache(String asset, AssetMetadata metadata, ref Object instance, Type type, ScreenDensityBucket densityBucket)
        {
            lock (SyncObject)
            {
                var assetDensityBucket = densityBucket;
                var assetOrigin = metadata.IsOverridden ? metadata.OverrideDirectory : null;

                if (!assetCache.TryGetValue(asset, out var assetCacheEntry))
                {
                    assetCacheEntry = new AssetCacheEntry(assetDensityBucket, assetOrigin, instance, type);
                    assetCache[asset] = assetCacheEntry;

                    if (!assetFlags.ContainsKey(asset))
                        assetFlags[asset] = AssetFlags.None;
                }
                else
                {
                    assetCacheEntry.SetVersion(assetDensityBucket, assetOrigin, instance);
                }
            }
        }

        // Asset data.
        private readonly Dictionary<String, AssetCacheEntry> assetCache = new Dictionary<String, AssetCacheEntry>();
        private readonly Dictionary<String, AssetFlags> assetFlags = new Dictionary<String, AssetFlags>();
    }
}
