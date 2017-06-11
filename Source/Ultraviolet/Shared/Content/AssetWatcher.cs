using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an object which monitors changes to a particular asset in a <see cref="ContentManager"/>.
    /// </summary>
    public abstract class AssetWatcher<T> : IAssetWatcher
    {
        /// <inheritdoc/>
        bool IAssetWatcher.OnValidating(String path, Object asset) => OnValidating(path, (T)asset);

        /// <inheritdoc/>
        void IAssetWatcher.OnReloaded(String path, Object asset, Boolean validated) => OnReloaded(path, (T)asset, validated);

        /// <inheritdoc/>
        Type IAssetWatcher.Type => typeof(T);

        /// <summary>
        /// Gets a value indicating whether the specified asset is valid and should be loaded.
        /// </summary>
        /// <param name="path">The asset path of the asset which is being reloaded.</param>
        /// <param name="asset">The asset which is being reloaded.</param>
        /// <returns><see langword="true"/> if the specified asset is valid; otherwise, <see langword="false"/>.</returns>
        public abstract bool OnValidating(String path, T asset);

        /// <summary>
        /// Called when the watched asset is reloaded.
        /// </summary>
        /// <param name="path">The asset path of the asset which was reloaded.</param>
        /// <param name="asset">The asset which was reloaded.</param>
        /// <param name="validated">A value indicating whether the asset that was loading validated successfully.</param>
        public abstract void OnReloaded(String path, T asset, Boolean validated);
    }
}
