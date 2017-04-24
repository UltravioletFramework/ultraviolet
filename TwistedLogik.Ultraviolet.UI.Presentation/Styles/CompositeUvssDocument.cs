using System;
using System.Collections.Generic;
using Ultraviolet.Content;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a Ultraviolet Style Sheet (UVSS) document which is composed of multiple smaller documents.
    /// </summary>
    public sealed class CompositeUvssDocument : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeUvssDocument"/> class.
        /// </summary>
        /// <param name="uv">The ultraviolet context.</param>
        /// <param name="onReloading">The method to invoke when one of the composite document's child documents is reloaded.</param>
        public CompositeUvssDocument(UltravioletContext uv, WatchedAssetReloadingHandler onReloading = null)
            : base(uv)
        {
            this.onReloading = onReloading;
        }

        /// <summary>
        /// Converts the composite document to a <see cref="UvssDocument"/> instance.
        /// </summary>
        /// <returns></returns>
        public UvssDocument ToUvssDocument()
        {
            var document = new UvssDocument(Ultraviolet);
            foreach (var child in children)
            {
                if (child is UvssDocument)
                {
                    document.Append((UvssDocument)child);
                }
                else
                {
                    var watched = (WatchedAsset<UvssDocument>)child;
                    document.Append(watched);
                }
            }
            return document;
        }

        /// <summary>
        /// Removes all child documents from the composite document.
        /// </summary>
        public void Clear()
        {
            children.Clear();
        }

        /// <summary>
        /// Appends a child document to the composite document.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="asset"></param>
        public void Append(ContentManager content, String asset)
        {
            if (Ultraviolet.GetUI().WatchingViewFilesForChanges)
            {
                var child = content.LoadWatched<UvssDocument>(asset, () =>
                {
                    if (onReloading != null)
                        return onReloading();

                    return true;
                });
                children.Add(child);
            }
            else
            {
                var child = content.Load<UvssDocument>(asset);
                children.Add(child);
            }
        }

        // State values.
        private readonly List<Object> children = new List<Object>();
        private readonly WatchedAssetReloadingHandler onReloading;
    }
}
