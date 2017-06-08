using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    public partial struct SourcedImage : IEquatable<SourcedImage>, IInterpolatable<SourcedImage>
    {
        /// <summary>
        /// Initializes the <see cref="SourcedImage"/> type.
        /// </summary>
        static SourcedImage()
        {
            Tweening.Interpolators.RegisterDefault<SourcedImage>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedImage"/> structure.
        /// </summary>
        /// <param name="resource">The texture image resouce which this object represents.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        [Preserve]
        public SourcedImage(TextureImage resource, AssetSource source)
        {
            this.resource  = resource;
            this.source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced image to its underlying image object.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedImage"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        [Preserve]
        public static implicit operator TextureImage(SourcedImage sourced)
        {
            return sourced.Resource;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{Resource} {Source.ToString().ToLowerInvariant()}";

        /// <inheritdoc/>
        [Preserve]
        public SourcedImage Interpolate(SourcedImage target, Single t)
        {
            return (t >= 1) ? target : this;
        }
                
        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public TextureImage Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid
        {
            get { return resource != null && resource.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return resource != null && resource.IsLoaded; }
        }

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source
        {
            get { return source; }
        }

        // Property values.
        private readonly TextureImage resource;
        private readonly AssetSource source;
    }
}
