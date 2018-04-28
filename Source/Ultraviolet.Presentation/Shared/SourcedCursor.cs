using System;
using Ultraviolet.Content;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    public partial struct SourcedCursor : IEquatable<SourcedCursor>, IInterpolatable<SourcedCursor>, IResourceWrapper
    {
        /// <summary>
        /// Initializes the <see cref="SourcedCursor"/> type.
        /// </summary>
        static SourcedCursor()
        {
            Tweening.Interpolators.RegisterDefault<SourcedCursor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedCursor"/> structure.
        /// </summary>
        /// <param name="resource">The cursor resource which this object represents.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        public SourcedCursor(SourcedCursorResource resource, AssetSource source)
        {
            this.resource = resource;
            this.source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced image to its underlying image object.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedImage"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        public static implicit operator SourcedCursorResource(SourcedCursor sourced)
        {
            return sourced.Resource;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{Resource} {Source.ToString().ToLowerInvariant()}";
        
        /// <summary>
        /// Loads the resource using the specified content manager.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="density">The screen density for which to load the resource.</param>
        public void Load(ContentManager contentManager, ScreenDensityBucket density)
        {
            resource.Load(contentManager, density);
        }

        /// <inheritdoc/>
        public SourcedCursor Interpolate(SourcedCursor target, Single t)
        {
            return (t >= 1) ? target : this;
        }

        /// <inheritdoc/>
        Object IResourceWrapper.Resource => Resource?.CursorCollection;

        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public SourcedCursorResource Resource
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
        private readonly SourcedCursorResource resource;
        private readonly AssetSource source;
    }
}
