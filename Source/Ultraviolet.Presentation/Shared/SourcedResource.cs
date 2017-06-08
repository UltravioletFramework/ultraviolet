using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    /// <typeparam name="T">The type of asset which this object represents.</typeparam>
    public partial struct SourcedResource<T> : IEquatable<SourcedResource<T>>, IInterpolatable<SourcedResource<T>> 
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedResource{T}"/> structure.
        /// </summary>
        /// <param name="asset">The asset identifier of the resource.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        [Preserve]
        public SourcedResource(AssetID asset, AssetSource source)
        {
            this.resource = new FrameworkResource<T>();
            this.asset = asset;
            this.source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced asset to its underlying value.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedResource{T}"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        [Preserve]
        public static implicit operator T(SourcedResource<T> sourced)
        {
            return sourced.Resource;
        }
        
        /// <summary>
        /// Loads the resource using the specified content manager.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the resource.</param>
        public void Load(ContentManager contentManager)
        {
            resource.Load(contentManager, asset);
        }

        /// <inheritdoc/>
        public override String ToString() => $"{Resource} {Source.ToString().ToLowerInvariant()}";

        /// <inheritdoc/>
        [Preserve]
        public SourcedResource<T> Interpolate(SourcedResource<T> target, Single t)
        {
            return (t >= 1) ? target : this;
        }
        
        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public FrameworkResource<T> Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid
        {
            get { return resource != null && asset.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return resource != null && resource.Value != null; }
        }

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source
        {
            get { return source; }
        }

        // Property values.
        private readonly FrameworkResource<T> resource;
        private readonly AssetID asset;
        private readonly AssetSource source;
    }
}
