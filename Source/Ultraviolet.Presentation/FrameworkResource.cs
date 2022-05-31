using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a resource used by the Presentation Foundation.
    /// </summary>
    /// <typeparam name="TResource">The type of resource which is represented by this object.</typeparam>
    public sealed class FrameworkResource<TResource> where TResource : class
    {
        /// <summary>
        /// Loads the resource from the specified content manager.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="asset">The asset identifier that identifies the resource to load.</param>
        /// <param name="density">The screen density for which to load the resource.</param>
        public void Load(ContentManager content, AssetID asset, ScreenDensityBucket density)
        {
            Contract.Require(content, nameof(content));

            var watch = content.Ultraviolet.GetUI().WatchingViewFilesForChanges;
            value = watch ? content.Watchers.GetSharedWatchedAsset<TResource>(asset, density) :
                (WatchableAssetReference<TResource>)content.Load<TResource>(asset, density);
        }
        
        /// <summary>
        /// Implicitly converts an instance of <see cref="FrameworkResource{T}"/> to its underlying resource.
        /// </summary>
        /// <param name="resource">The object to convert.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator TResource(FrameworkResource<TResource> resource) => resource?.Value;

        /// <summary>
        /// Gets the resource value that this object represents.
        /// </summary>
        public TResource Value => value;

        // Property values.
        private WatchableAssetReference<TResource> value;
    }
}
