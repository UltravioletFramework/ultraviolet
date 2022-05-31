using System;
using Ultraviolet.Content;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    /// <typeparam name="T">The type of asset which this object represents.</typeparam>
    public partial struct SourcedResource<T> : IEquatable<SourcedResource<T>>, IInterpolatable<SourcedResource<T>>, IResourceWrapper
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedResource{T}"/> structure.
        /// </summary>
        /// <param name="asset">The asset identifier of the resource.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        public SourcedResource(AssetID asset, AssetSource source)
        {
            this.Resource = new FrameworkResource<T>();
            this.Asset = asset;
            this.Source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced asset to its underlying value.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedResource{T}"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        public static implicit operator T(SourcedResource<T> sourced) => sourced.Resource;

        /// <inheritdoc/>
        public override String ToString() => $"{Resource} {Source.ToString().ToLowerInvariant()}";

        /// <summary>
        /// Loads the resource using the specified content manager.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="density">The screen density for which to load the resource.</param>
        public void Load(ContentManager contentManager, ScreenDensityBucket density)
        {
            Resource.Load(contentManager, Asset, density);
        }

        /// <inheritdoc/>
        public SourcedResource<T> Interpolate(SourcedResource<T> target, Single t)
        {
            return (t >= 1) ? target : this;
        }

        /// <inheritdoc/>
        Object IResourceWrapper.Resource => Resource?.Value;

        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public FrameworkResource<T> Resource { get; }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid => Resource != null && Asset.IsValid;

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded => Resource != null && Resource.Value != null;
        
        /// <summary>
        /// Gets the <see cref="AssetID"/> that identifies this resource.
        /// </summary>
        public AssetID Asset { get; }

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source { get; }
    }
}
