using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a Ultraviolet Style Sheet (UVSS) document which is composed of multiple smaller documents.
    /// </summary>
    public class CompositeUvssDocument : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUvssDocument"/> class.
        /// </summary>
        /// <param name="uv">The ultraviolet context.</param>
        private CompositeUvssDocument(UltravioletContext uv)
            : base(uv)
        {
            this.validating = Global_Validating;
            this.validationComplete = Global_ValidationComplete;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUvssDocument"/> class.
        /// </summary>
        /// <param name="uv">The ultraviolet context.</param>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="reloading">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        public CompositeUvssDocument(UltravioletContext uv, AssetWatcherValidatingHandler<UvssDocument> validating = null, AssetWatcherValidationCompleteHandler<UvssDocument> reloading = null)
            : base(uv)
        {
            this.validating = validating;
            this.validationComplete = reloading;
        }
        
        /// <summary>
        /// Creates a new <see cref="CompositeUvssDocument"/> which will serve as the application's global style sheet.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The <see cref="CompositeUvssDocument"/> which was created.</returns>
        public static CompositeUvssDocument CreateForGlobalStyleSheet(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            return new CompositeUvssDocument(uv);
        }

        /// <summary>
        /// Converts the composite document to a <see cref="UvssDocument"/> instance.
        /// </summary>
        /// <returns></returns>
        public UvssDocument ToUvssDocument()
        {
            var document = new UvssDocument(Ultraviolet);

            foreach (var child in children)
                document.Append(child.ValidatingValue ?? child.Value);

            return document;
        }

        /// <summary>
        /// Removes all child documents from the composite document.
        /// </summary>
        public void Clear()
        {
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

            var watched = default(WatchedAsset<UvssDocument>);
            var watching = Ultraviolet.GetUI().WatchingViewFilesForChanges;
            if (watching)
            {
                watched = new WatchedAsset<UvssDocument>(content, asset,
                    (p, a) => validating?.Invoke(p, a) ?? true,
                    (p, a, v) => validationComplete?.Invoke(p, a, v));
            }
            else
            {
                watched = new WatchedAsset<UvssDocument>(content, asset);
            }

            children.Add(watched);
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
        /// Handles the validation phase when reloading the global style sheet.
        /// </summary>
        private bool Global_Validating(String path, UvssDocument asset)
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            return upf.TrySetGlobalStyleSheet(ToUvssDocument());
        }

        /// <summary>
        /// Handles the reloading phase when reloading the global style sheet.
        /// </summary>
        private void Global_ValidationComplete(String path, UvssDocument asset, Boolean validated)
        {
            if (validated)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.TrySetGlobalStyleSheet(ToUvssDocument());
        }

        // State values.
        private readonly List<WatchedAsset<UvssDocument>> children = new List<WatchedAsset<UvssDocument>>();
        private readonly AssetWatcherValidatingHandler<UvssDocument> validating;
        private readonly AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete;
    }
}
