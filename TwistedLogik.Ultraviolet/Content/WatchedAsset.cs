using System;
using System.IO;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents the method that is called when a watched asset is being reloaded.
    /// </summary>
    /// <returns><see langword="true"/> if the asset was reloaded successfully; otherwise, <see langword="false"/>.</returns>
    public delegate Boolean WatchedAssetReloadingHandler();

    /// <summary>
    /// Represents a loaded content asset which is being watched for changes.
    /// </summary>
    /// <typeparam name="TContent">The type of content asset which is being watched.</typeparam>
    public struct WatchedAsset<TContent> : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchedAsset{TContent}"/> structure.
        /// </summary>
        /// <param name="owner">The <see cref="ContentManager"/> that owns the watched asset.</param>
        /// <param name="assetPath">The asset path that represents the asset.</param>
        /// <param name="assetFilePath">The resolved file path to the file that represents the asset.</param>
        /// <param name="watcher">The <see cref="FileSystemWatcher"/> which is responsible for watching the asset.</param>
        /// <param name="onReloading">The action to perform when the asset is changed.</param>
        public WatchedAsset(ContentManager owner, String assetPath, String assetFilePath, FileSystemWatcher watcher, WatchedAssetReloadingHandler onReloading)
        {
            this.Owner = owner;
            this.AssetPath = assetPath;
            this.AssetFilePath = Path.GetFullPath(assetFilePath);
            this.OnReloading = onReloading;

            this.watcher = watcher;
            this.watcher.Changed += Watcher_Changed;
        }

        /// <summary>
        /// Implicitly converts an instance of <see cref="WatchedAsset{TContent}"/> to its watched content type.
        /// </summary>
        /// <param name="asset">The asset to convert.</param>
        public static implicit operator TContent(WatchedAsset<TContent> asset)
        {
            return asset.Owner.Load<TContent>(asset.AssetPath);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.watcher.Changed -= Watcher_Changed;
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> which owns this asset.
        /// </summary>
        public ContentManager Owner { get; }

        /// <summary>
        /// Gets the asset path that represents the asset.
        /// </summary>
        public String AssetPath { get; }

        /// <summary>
        /// Gets the resolved file path to the file that represents the asset.
        /// </summary>
        public String AssetFilePath { get; }

        /// <summary>
        /// Gets the delegate which is invoked when the watched asset is reloaded.
        /// </summary>
        public WatchedAssetReloadingHandler OnReloading { get; }
                
        /// <summary>
        /// Gets the asset which is being watched by this instance.
        /// </summary>
        public TContent Value => Owner.Load<TContent>(AssetPath);

        /// <summary>
        /// Occurs when the watched asset changes.
        /// </summary>
        private void Watcher_Changed(Object sender, FileSystemEventArgs e)
        {
            if ((e.FullPath == AssetFilePath || Owner.IsWatchedDependencyPath(AssetPath, e.FullPath)) && OnReloading != null)
            {
                var asset = this;
                var lastKnownGoodVersion = Owner.LoadWatched(asset);

                Owner.Ultraviolet.QueueWorkItem(state =>
                {
                    if (state != null)
                    {
                        var dependency = (String)state;
                        asset.Owner.PurgeCache(dependency, false);
                    }
                    asset.Owner.PurgeCache(asset.AssetPath, false);
                    asset.Owner.LoadWatched(asset, lastKnownGoodVersion);
                }, e.FullPath);
            }
        }

        // State values.
        private readonly FileSystemWatcher watcher;
    }
}
