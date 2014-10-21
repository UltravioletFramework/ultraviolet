using System;
using System.IO;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an individual asset within a content manifest.
    /// </summary>
    public sealed class ContentManifestAsset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifestAsset"/> class.
        /// </summary>
        /// <param name="group">The <see cref="ContentManifestGroup"/> that owns the asset.</param>
        /// <param name="element">The XML element that defines the asset.</param>
        internal ContentManifestAsset(ContentManifestGroup group, XElement element)
        {
            Contract.Require(element, "element");

            var name = element.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestAssetName);

            var path = element.Value;
            if (String.IsNullOrEmpty(path))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestAssetPath.Format(name));

            this.ManifestGroup = group;
            this.Name = name;
            this.RelativePath = path;
            this.AbsolutePath = Path.Combine(group.Directory, path);
            this.Type = group.Type;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AssetID"/> structure that represents this asset.
        /// </summary>
        /// <returns>The instance of the <see cref="AssetID"/> structure that was created.</returns>
        public AssetID CreateAssetID()
        {
            return new AssetID(ManifestGroup.Manifest.Name, ManifestGroup.Name, Name, AbsolutePath, ManifestGroup.IndexOf(this));
        }

        /// <summary>
        /// Gets the <see cref="ContentManifestGroup"/> that owns the asset.
        /// </summary>
        public ContentManifestGroup ManifestGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's name within the content manifest.
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's path relative to its group's root directory.
        /// </summary>
        /// <remarks>This is the same path that is passed to <see cref="ContentManager.Load{T}(String, Boolean)"/>.</remarks>
        public String RelativePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's path including its group's root directory.
        /// </summary>
        /// <remarks>This is the same path that is passed to <see cref="ContentManager.Load{T}(String, Boolean)"/>.</remarks>
        public String AbsolutePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset's type.
        /// </summary>
        public Type Type
        {
            get;
            private set;
        }
    }
}
