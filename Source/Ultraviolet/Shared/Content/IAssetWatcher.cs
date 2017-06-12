using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the type-agnostic interface for an <see cref="AssetWatcher{T}"/> instance.
    /// </summary>
    internal interface IAssetWatcher
    {
        /// <summary>
        /// Gets a value indicating whether the specified asset is valid and should be loaded.
        /// </summary>
        /// <param name="path">The asset path of the asset which is being reloaded.</param>
        /// <param name="asset">The asset which is being reloaded.</param>
        /// <returns><see langword="true"/> if the specified asset is valid; otherwise, <see langword="false"/>.</returns>
        bool OnValidating(String path, Object asset);
        
        /// <summary>
        /// Called when the watched asset is finished validating an asset.
        /// </summary>
        /// <param name="path">The asset path of the asset which is being reloaded.</param>
        /// <param name="asset">The asset which was reloaded.</param>
        /// <param name="validated">A value indicating whether the asset that was loading validated successfully.</param>
        void OnValidationComplete(String path, Object asset, Boolean validated);

        /// <summary>
        /// Gets the type of resource that the watcher is watching.
        /// </summary>
        Type Type { get; }
    }
}
