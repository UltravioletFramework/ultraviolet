using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a wrapper around a content asset which watches for and responds to changes.
    /// </summary>
    public sealed class WatchedAsset<T> : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchedAsset{T}"/> class.
        /// </summary>
        /// <param name="owner">The content manager that owns the resource.</param>
        /// <param name="assetPath">The asset path of the asset which is represented by this wrapper.</param>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="reloading">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnReloaded(String, T, Boolean)"/> method.</param>
        public WatchedAsset(ContentManager owner, String assetPath, AssetWatcherValidatingHandler<T> validating = null, AssetWatcherReloadingHandler<T> reloading = null)
        {
            var instance = owner.Load<T>(assetPath);

            this.Owner = owner;
            this.AssetPath = assetPath;
            this.CurrentValue = instance;
            this.LastKnownGoodValue = instance;

            if (validating != null || reloading != null)
            {
                this.watcher = new DelegateAssetWatcher<T>(
                    (p, a) =>
                    {
                        this.CurrentValue = a;
                        return validating?.Invoke(p, a) ?? true;
                    },
                    (p, a, v) =>
                    {
                        this.LastKnownGoodValue = a;
                        reloading?.Invoke(p, a, v);
                    });
                this.Owner.AddWatcher(assetPath, watcher);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchedAsset{T}"/> class.
        /// </summary>
        /// <param name="owner">The content manager that owns the resource.</param>
        /// <param name="asset">The asset which is represented by this wrapper.</param>
        public WatchedAsset(ContentManager owner, T asset)
        {
            this.Owner = owner;
            this.AssetPath = null;
            this.CurrentValue = asset;
            this.LastKnownGoodValue = asset;
        }

        /// <summary>
        /// Implicitly converts a <see cref="WatchedAsset{T}"/> to its underlying value.
        /// </summary>
        /// <param name="wrapper">The asset which is represented by the specified wrapper.</param>
        public static implicit operator T(WatchedAsset<T> wrapper) => wrapper.LastKnownGoodValue;

        /// <summary>
        /// Releases resources associated with the wrapper.
        /// </summary>
        public void Dispose()
        {
            if (this.watcher != null)
            {
                this.Owner.RemoveWatcher(AssetPath, watcher);
                this.watcher = null;
            }

            this.CurrentValue = default(T);
            this.LastKnownGoodValue = default(T);
        }
        
        /// <summary>
        /// Gets the <see cref="ContentManager"/> that owns the resource.
        /// </summary>
        public ContentManager Owner { get; }

        /// <summary>
        /// Retrieves the current version of the asset which is represented by this wrapper.
        /// </summary>
        public T CurrentValue { get; private set; }

        /// <summary>
        /// Retrieves the last known-good version of the asset which is represented by this wrapper.
        /// </summary>
        public T LastKnownGoodValue { get; private set; }

        /// <summary>
        /// Gets the asset path of the asset which is represented by this wrapper.
        /// </summary>
        public String AssetPath { get; }

        /// <summary>
        /// Gets a value indicating whether the asset which is contained by this wrapper
        /// is actually being watched for changes.
        /// </summary>
        public Boolean IsWatched
        {
            get { return watcher != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper contains a valid asset.
        /// </summary>
        public Boolean IsValid
        {
            get { return CurrentValue != null; }
        }
        
        // State values.
        private DelegateAssetWatcher<T> watcher;
    }
}
