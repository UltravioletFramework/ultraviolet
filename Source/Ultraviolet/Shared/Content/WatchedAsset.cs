using System;
using Ultraviolet.Platform;

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
        /// <param name="validationComplete">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        public WatchedAsset(ContentManager owner, String assetPath, AssetWatcherValidatingHandler<T> validating = null, AssetWatcherValidationCompleteHandler<T> validationComplete = null)
            : this(owner, assetPath, null, validating, validationComplete)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchedAsset{T}"/> class.
        /// </summary>
        /// <param name="owner">The content manager that owns the resource.</param>
        /// <param name="assetPath">The asset path of the asset which is represented by this wrapper.</param>
        /// <param name="assetDensity">The density bucket for which to load the asset, or <see langword="null"/> to use the density of the primary display.</param>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="validationComplete">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        public WatchedAsset(ContentManager owner, String assetPath, ScreenDensityBucket? assetDensity, 
            AssetWatcherValidatingHandler<T> validating = null, AssetWatcherValidationCompleteHandler<T> validationComplete = null)
        {
            var instance = default(T);
            if (assetDensity == null)
                instance = owner.Load<T>(assetPath);
            else
                instance = owner.Load<T>(assetPath, assetDensity.Value);

            this.Owner = owner;
            this.AssetPath = assetPath;
            this.AssetDensity = assetDensity;
            this.ValidatingValue = instance;
            this.Value = instance;

            this.watcher = new DelegateAssetWatcher<T>(
                (p, a) =>
                {
                    this.ValidatingValue = a;
                    return validating?.Invoke(p, a) ?? true;
                },
                (p, a, v) =>
                {
                    this.ValidatingValue = default(T);
                    this.Value = a;
                    validationComplete?.Invoke(p, a, v);
                });

            if (assetDensity == null)
                this.Owner.Watchers.AddWatcher(assetPath, watcher);
            else
                this.Owner.Watchers.AddWatcher(assetPath, assetDensity.Value, watcher);
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
            this.AssetDensity = null;
            this.ValidatingValue = asset;
            this.Value = asset;
        }

        /// <summary>
        /// Implicitly converts a <see cref="WatchedAsset{T}"/> to its underlying value.
        /// </summary>
        /// <param name="wrapper">The asset which is represented by the specified wrapper.</param>
        public static implicit operator T(WatchedAsset<T> wrapper) => wrapper.Value;

        /// <summary>
        /// Releases resources associated with the wrapper.
        /// </summary>
        public void Dispose()
        {
            if (this.watcher != null)
            {
                if (this.AssetDensity == null)
                    this.Owner.Watchers.RemoveWatcher(AssetPath, watcher);
                else
                    this.Owner.Watchers.RemoveWatcher(AssetPath, AssetDensity.Value, watcher);

                this.watcher = null;
            }

            this.ValidatingValue = default(T);
            this.Value = default(T);
        }
        
        /// <summary>
        /// Gets the <see cref="ContentManager"/> that owns the resource.
        /// </summary>
        public ContentManager Owner { get; }

        /// <summary>
        /// Retrieves the version of the wrapped asset which is in the process of being validated.
        /// </summary>
        public T ValidatingValue { get; private set; }

        /// <summary>
        /// Retrieves the last known-good version of the wrapped asset.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the asset path of the asset which is represented by this wrapper.
        /// </summary>
        public String AssetPath { get; }

        /// <summary>
        /// Gets the screen density of the asset which is represented by this wrapper,
        /// or <see langword="null"/> to use the primary display's density.
        /// </summary>
        public ScreenDensityBucket? AssetDensity { get; }

        /// <summary>
        /// Gets a value indicating whether the asset which is contained by this wrapper
        /// is actually being watched for changes.
        /// </summary>
        public Boolean IsWatched
        {
            get { return watcher != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper is in the process of validating its underlying asset.
        /// </summary>
        public Boolean IsValidating
        {
            get { return ValidatingValue != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper contains a valid asset.
        /// </summary>
        public Boolean HasValue
        {
            get { return Value != null; }
        }
        
        // State values.
        private DelegateAssetWatcher<T> watcher;
    }
}
