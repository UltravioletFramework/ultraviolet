using System;
using Ultraviolet.Content;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    public partial struct SourcedImage : IEquatable<SourcedImage>, IInterpolatable<SourcedImage>, IResourceWrapper
    {
        /// <summary>
        /// Initializes the <see cref="SourcedImage"/> type.
        /// </summary>
        static SourcedImage() => Tweening.Interpolators.RegisterDefault<SourcedImage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedImage"/> structure.
        /// </summary>
        /// <param name="resource">The texture image resouce which this object represents.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        public SourcedImage(TextureImage resource, AssetSource source)
        {
            this.Resource = resource;
            this.Source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced image to its underlying image object.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedImage"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        public static implicit operator TextureImage(SourcedImage sourced) => sourced.Resource;

        /// <inheritdoc/>
        public override String ToString() => $"{Resource} {Source.ToString().ToLowerInvariant()}";

        /// <summary>
        /// Loads the resource using the specified content manager.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="density">The screen density for which to load the resource.</param>
        public void Load(ContentManager contentManager, ScreenDensityBucket density)
        {
            var watch = contentManager.Ultraviolet.GetUI().WatchingViewFilesForChanges;
            Resource.Load(contentManager, density, watch);
        }

        /// <inheritdoc/>
        public SourcedImage Interpolate(SourcedImage target, Single t) => (t >= 1) ? target : this;

        /// <inheritdoc/>
        Object IResourceWrapper.Resource => Resource?.Texture;

        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public TextureImage Resource { get; }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid => Resource != null && Resource.IsValid;

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded => Resource != null && Resource.IsLoaded;

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source { get; }
    }
}
