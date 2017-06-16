using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a Ultraviolet Style Sheet (UVSS) document which is composed of multiple smaller documents.
    /// </summary>
    public abstract class CompositeUvssDocument : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUvssDocument"/> class.
        /// </summary>
        /// <param name="uv">The ultraviolet context.</param>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="validationComplete">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        protected CompositeUvssDocument(UltravioletContext uv, 
            AssetWatcherValidatingHandler<UvssDocument> validating = null, AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete = null)
            : base(uv)
        {
            this.validating = validating;
            this.validationComplete = validationComplete;
        }
        
        /// <summary>
        /// Converts the composite document to a <see cref="UvssDocument"/> instance appropriate
        /// for the primary display's screen density.
        /// </summary>
        /// <returns>The <see cref="UvssDocument"/> which was created.</returns>
        public UvssDocument ToUvssDocument()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var primaryDisplay = Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return ToUvssDocumentInternal(primaryDisplayDensity);
        }

        /// <summary>
        /// Converts the composite document to a <see cref="UvssDocument"/> instance appropriate
        /// for the specified creen density.
        /// </summary>
        /// <returns>The <see cref="UvssDocument"/> which was created.</returns>
        public UvssDocument ToUvssDocument(ScreenDensityBucket density)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return ToUvssDocumentInternal(density);
        }

        /// <summary>
        /// Removes all child documents from the composite document.
        /// </summary>
        public void Clear()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            foreach (var child in children)
                child.Dispose();

            children.Clear();
        }

        /// <summary>
        /// Appends a child document to the composite document.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="asset"></param>
        public void Append(ContentManager content, String asset)
        {
            Contract.Require(content, nameof(content));
            Contract.Require(asset, nameof(asset));
            Contract.EnsureNotDisposed(this, Disposed);

            var child = new CompositeUvssDocumentChild(content, asset, validating, validationComplete);
            children.Add(child);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var child in children)
                    child.Dispose();
            }

            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Converts the composite document to a <see cref="UvssDocument"/> instance appropriate
        /// for the specified creen density.
        /// </summary>
        private UvssDocument ToUvssDocumentInternal(ScreenDensityBucket density)
        {
            var document = new UvssDocument(Ultraviolet);

            foreach (var child in children)
                document.Append(child.Get(density));

            return document;
        }

        // State values.
        private readonly List<CompositeUvssDocumentChild> children = new List<CompositeUvssDocumentChild>();
        private readonly AssetWatcherValidatingHandler<UvssDocument> validating;
        private readonly AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete;
    }
}
