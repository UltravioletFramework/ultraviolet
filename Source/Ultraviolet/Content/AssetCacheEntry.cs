using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represent an entry in a <see cref="ContentManager"/> instance's internal content cache.
    /// </summary>
    internal class AssetCacheEntry : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCacheEntry"/> class.
        /// </summary>
        /// <param name="assetDensityBucket">The density bucket associated with this asset.</param>
        /// <param name="assetOrigin">The origin path of the asset.</param>
        /// <param name="assetInstance">The asset instance.</param>
        /// <param name="assetType">The asset type.</param>
        public AssetCacheEntry(ScreenDensityBucket assetDensityBucket, String assetOrigin, Object assetInstance, Type assetType)
        {
            this.obj = new AssetCacheVersion(assetDensityBucket, assetOrigin, assetInstance);
            this.AssetType = assetType;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (obj is AssetCacheVersion acv)
            {
                if (acv.AssetInstance is IDisposable disposable)
                    disposable.Dispose();
            }
            else
            {
                var dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;
                foreach (var kvp in dictionary)
                {
                    if (kvp.Value.AssetInstance is IDisposable disposable)
                        disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Reloads the asset represented by this cache entry.
        /// </summary>
        /// <param name="content">The content manager with which to reload the cache entry.</param>
        /// <param name="assetPath">The asset path that represents the asset.</param>
        /// <param name="assetWatchers">The asset watchers that are watching the asset.</param>
        public void Reload(ContentManager content, String assetPath, IAssetWatcherCollection assetWatchers)
        {
            if (obj is AssetCacheVersion acv)
            {
                acv.Reload(content, assetPath, assetWatchers, AssetType);
            }
            else
            {
                var dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;
                foreach (var kvp in dictionary)
                {
                    kvp.Value.Reload(content, assetPath, assetWatchers, AssetType);
                }
            }
        }

        /// <summary>
        /// Purges any versions of this asset which correspond to screen densities that are not in use.
        /// </summary>
        /// <param name="usedScreenDensities">A list of screen densities which are currently in use.</param>
        /// <returns><see langword="true"/> if the entire cache entry should be purged; otherwise, <see langword="false"/>.</returns>
        public Boolean PurgeUnusedVersions(ScreenDensityBucket[] usedScreenDensities)
        {
            Contract.Require(usedScreenDensities, nameof(usedScreenDensities));

            if (obj is AssetCacheVersion acv)
            {
                return !usedScreenDensities.Contains(acv.AssetDensityBucket);
            }
            else
            {
                var dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;
                var removedKeys = dictionary.Keys.Where(x => !usedScreenDensities.Contains((ScreenDensityBucket)x)).ToArray();
                if (removedKeys.Length == dictionary.Count)
                    return true;

                foreach (var removedKey in removedKeys)
                    dictionary.Remove(removedKey);

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this asset has a version for the specified <see cref="ScreenDensityBucket"/> value.
        /// </summary>
        /// <param name="assetDensityBucket">The <see cref="ScreenDensityBucket"/> value to evaluate.</param>
        /// <returns><see langword="true"/> if the asset has a version for the specified <see cref="ScreenDensityBucket"/> value; otherwise, <see langword="false"/>.</returns>
        public Boolean HasVersion(ScreenDensityBucket assetDensityBucket)
        {
            if (obj is AssetCacheVersion acv)
            {
                return acv.AssetDensityBucket == assetDensityBucket;
            }
            else
            {
                var dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;
                return dictionary.ContainsKey((Byte)assetDensityBucket);
            }
        }

        /// <summary>
        /// Sets the version of the asset associated with the specified <see cref="ScreenDensityBucket"/> value.
        /// </summary>
        /// <param name="assetDensityBucket">The density bucket associated with this asset.</param>
        /// <param name="assetOrigin">The origin path of the asset.</param>
        /// <param name="assetInstance">The asset instance.</param>
        /// <returns><see langword="true"/> if the asset was set; otherwise, <see langword="false"/>.</returns>
        public Boolean SetVersion(ScreenDensityBucket assetDensityBucket, String assetOrigin, Object assetInstance)
        {
            var dictionary = default(Dictionary<Byte, AssetCacheVersion>);

            if (obj is AssetCacheVersion acv)
            {
                if (acv.AssetDensityBucket == assetDensityBucket)
                    return false;

                dictionary = new Dictionary<Byte, AssetCacheVersion>();
                dictionary[(Byte)acv.AssetDensityBucket] = acv;
                obj = dictionary;
            }
            else
            {
                dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;
            }

            if (dictionary.ContainsKey((Byte)assetDensityBucket))
                return false;

            dictionary[(Byte)assetDensityBucket] = new AssetCacheVersion(assetDensityBucket, assetOrigin, assetInstance);
            return true;
        }

        /// <summary>
        /// Gets the version of the asset associated with the specified <see cref="ScreenDensityBucket"/> value,
        /// if there is an asset version for that value.
        /// </summary>
        /// <param name="assetDensityBucket">The <see cref="ScreenDensityBucket"/> value for which to retrieve an asset instance.</param>
        /// <param name="asset">The asset version associated with the specified <see cref="ScreenDensityBucket"/> value.</param>
        /// <returns><see langword="true"/> if an asset was retrieved; otherwise, <see langword="false"/>.</returns>
        public Boolean GetVersion(ScreenDensityBucket assetDensityBucket, out Object asset)
        {
            if (obj is AssetCacheVersion acv)
            {
                if (acv.AssetDensityBucket == assetDensityBucket)
                {
                    asset = acv.AssetInstance;
                    return true;
                }

                asset = null;
                return false;
            }
            else
            {
                var dictionary = (Dictionary<Byte, AssetCacheVersion>)obj;

                if (dictionary.TryGetValue((Byte)assetDensityBucket, out var version))
                {
                    asset = version.AssetInstance;
                    return true;
                }

                asset = null;
                return false;
            }
        }
        
        /// <summary>
        /// Gets the type as which the asset was loaded.
        /// </summary>
        public Type AssetType { get; private set; }
        
        // The underlying asset instance, or if there are multiple versions available,
        // the version library which tracks them.
        private Object obj;
    }
}
