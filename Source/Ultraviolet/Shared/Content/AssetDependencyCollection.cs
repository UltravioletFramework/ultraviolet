using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an asset's list of dependent assets.
    /// </summary>
    internal sealed class AssetDependencyCollection : IAssetDependencyCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDependencyCollection"/> class.
        /// </summary>
        /// <param name="owner">The content manager that owns the collection.</param>
        /// <param name="assetPath">The normalized asset path of the asset which is being watched.</param>
        /// <param name="assetFilePath">The asset file path of the asset which is being watched.</param>
        public AssetDependencyCollection(ContentManager owner, String assetPath, String assetFilePath)
        {
            Contract.Require(owner, nameof(owner));
            Contract.Require(assetPath, nameof(assetPath));
            Contract.Require(assetFilePath, nameof(assetFilePath));

            this.Owner = owner;
            this.AssetPath = assetPath;
            this.AssetFilePath = assetFilePath;
            this.DependentAssets = new HashSet<String>();
        }
        
        /// <inheritdoc/>
        public ContentManager Owner { get; }

        /// <inheritdoc/>
        public String AssetPath { get; }

        /// <inheritdoc/>
        public String AssetFilePath { get; }

        /// <inheritdoc/>
        public ISet<String> DependentAssets { get; }
    }
}
