using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an Ultraviolet context's registry of loaded content manifests.
    /// </summary>
    public sealed class ContentManifestRegistry : UltravioletNamedCollection<ContentManifest>
    {
        /// <summary>
        /// Loads a content manifest from the file at the specified path and adds it to the registry.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        public void Load(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            var manifest = ContentManifest.Load(path);
            Add(manifest);
        }
        
        /// <summary>
        /// Loads the content manifests from the files at the specified collection of paths and adds them to the registry.
        /// </summary>
        /// <param name="paths">A collection of paths representing the files to load.</param>
        public void Load(IEnumerable<String> paths)
        {
            Contract.Require(paths, nameof(paths));

            foreach (var path in paths)
            {
                Load(path);
            }
        }

        /// <summary>
        /// Loads a content manifest from the specified stream and adds it to the registry.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the content manifest to load.</param>
        public void Load(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var manifest = ContentManifest.LoadXml(stream);
            Add(manifest);
        }

        /// <summary>
        /// Clears the content manifest registry.
        /// </summary>
        public void Clear()
        {
            ClearInternal();
        }

        /// <summary>
        /// Adds a <see cref="ContentManifest"/> to the registry.
        /// </summary>
        /// <param name="manifest">The <see cref="ContentManifest"/> to add to the registry.</param>
        public void Add(ContentManifest manifest)
        {
            Contract.Require(manifest, nameof(manifest));

            if (Contains(manifest.Name))
                throw new InvalidOperationException(UltravioletStrings.ContentManifestAlreadyContainsAsset.Format(manifest.Name));

            AddInternal(manifest);
        }

        /// <summary>
        /// Removes the specified content manifest from the registry.
        /// </summary>
        /// <param name="name">The name of the content manifest to remove from the registry.</param>
        /// <returns><see langword="true"/> if the content manifest was removed from the registry; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var item = this[name];
            if (item != null)
            {
                return RemoveInternal(item);
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the content manifest registry contains a manifest with the specified name.
        /// </summary>
        /// <param name="name">The manifest name to evaluate.</param>
        /// <returns><see langword="true"/> if the content manifest registry contains a manifest with the specified name; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            return StorageByName.ContainsKey(name);
        }

        /// <summary>
        /// Gets a value indicating whether the content manifest registry contains the specified manifest.
        /// </summary>
        /// <param name="manifest">The manifest to evaluate.</param>
        /// <returns><see langword="true"/> if the content manifest registry contains the specified manifest; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(ContentManifest manifest)
        {
            Contract.Require(manifest, nameof(manifest));

            return ContainsInternal(manifest);
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected override String GetName(ContentManifest item)
        {
            return item.Name;
        }
    }
}
