using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents one of the child documents that makes up a <see cref="CompositeUvssDocument"/> instance.
    /// </summary>
    internal sealed class CompositeUvssDocumentChild : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUvssDocumentChild"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager with which to load the asset.</param>
        /// <param name="assetPath">The asset path of the document to load.</param>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="validationComplete">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        public CompositeUvssDocumentChild(ContentManager contentManager, String assetPath, 
            AssetWatcherValidatingHandler<UvssDocument> validating = null, AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete = null)
        {
            Contract.Require(contentManager, nameof(contentManager));
            Contract.Require(assetPath, nameof(assetPath));

            this.contentManager = contentManager;
            this.assetPath = assetPath;
            this.assetVersions = new Dictionary<Byte, WatchedAsset<UvssDocument>>();
            this.validating = validating;
            this.validationComplete = validationComplete;
        }
        
        /// <summary>
        /// Gets the version of this document which has been loaded for the primary display's density bucket.
        /// </summary>
        /// <returns>The version of this document which has been loaded for the primary display's density bucket.</returns>
        public UvssDocument Get()
        {
            Contract.EnsureNotDisposed(this, disposed);

            var primaryDisplay = contentManager.Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return Get(primaryDisplayDensity);
        }

        /// <summary>
        /// Gets the version of this document which has been loaded for the specified density bucket.
        /// </summary>
        /// <param name="density">The density bucket for which to retrieve a document.</param>
        /// <returns>The version of this document which has been loaded for the specified density bucket.</returns>
        public UvssDocument Get(ScreenDensityBucket density)
        {
            Contract.EnsureNotDisposed(this, disposed);

            if (!assetVersions.TryGetValue((Byte)density, out var document))
            {
                var watching = contentManager.Ultraviolet.GetUI().WatchingViewFilesForChanges;
                if (watching)
                {
                    document = new WatchedAsset<UvssDocument>(contentManager, assetPath, density,
                        (p, a) => validating?.Invoke(p, a) ?? true,
                        (p, a, v) => validationComplete?.Invoke(p, a, v));
                }
                else
                {
                    document = new WatchedAsset<UvssDocument>(contentManager, assetPath, density);
                }
                assetVersions[(Byte)density] = document;
            }

            return document.ValidatingValue ?? document.Value;
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;

            foreach (var kvp in assetVersions)
                kvp.Value.Dispose();

            assetVersions.Clear();
            disposed = true;
        }

        // State values.
        private readonly ContentManager contentManager;
        private readonly String assetPath;
        private readonly Dictionary<Byte, WatchedAsset<UvssDocument>> assetVersions;
        private readonly AssetWatcherValidatingHandler<UvssDocument> validating;
        private readonly AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete;
        private Boolean disposed;
    }
}
