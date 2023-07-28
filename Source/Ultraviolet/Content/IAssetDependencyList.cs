using System;
using System.Collections.Generic;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the type-agnostic interface for an <see cref="AssetDependencyCollection"/> instance.
    /// </summary>
    internal interface IAssetDependencyCollection
    {
        /// <summary>
        /// Gets the content manager that owns the collection.
        /// </summary>
        ContentManager Owner { get; }

        /// <summary>
        /// Gets the normalized asset path of the asset which is being watched.
        /// </summary>
        String AssetPath { get; }

        /// <summary>
        /// Gets the asset file path of the asset which is being watched.
        /// </summary>
        String AssetFilePath { get; }

        /// <summary>
        /// Gets the set of assets which are dependent upon this asset.
        /// </summary>
        ISet<String> DependentAssets { get; }
    }
}
