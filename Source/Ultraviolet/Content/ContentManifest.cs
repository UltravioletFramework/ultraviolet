using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Ultraviolet.Core;
using Ultraviolet.Core.Xml;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a manifest containing a list of related content assets.
    /// </summary>
    public sealed class ContentManifest : UltravioletNamedCollection<ContentManifestGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifest"/> class.
        /// </summary>
        /// <param name="name">The content manifest's name.</param>
        /// <param name="groups">The content manifest's group definitions.</param>
        private ContentManifest(String name, IEnumerable<XElement> groups)
        {
            Contract.Require(name, nameof(name));
            Contract.Require(groups, nameof(groups));

            this.Name = name;

            foreach (var group in groups)
            {
                AddInternal(new ContentManifestGroup(this, group));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManifest"/> class.
        /// </summary>
        /// <param name="desc">The content manifest description.</param>
        private ContentManifest(ContentManifestDescription desc)
        {
            Contract.Require(desc, nameof(desc));

            this.Name = desc.Name;

            if (desc.Groups != null)
            {
                foreach (var group in desc.Groups)
                {
                    AddInternal(new ContentManifestGroup(this, group));
                }
            }
        }

        /// <summary>
        /// Loads a content manifest from the XML file at the specified path.
        /// </summary>
        /// <param name="path">The path to the XML file to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified XML file.</returns>
        public static ContentManifest Load(String path)
        {
            var extension = Path.GetExtension(path);
            switch (extension?.ToLower() ?? String.Empty)
            {
                case ".js":
                case ".json":
                case ".jsmanifest":
                    return LoadJson(path);

                default:
                    return LoadXml(path);
            }
        }

        /// <summary>
        /// Loads a content manifest from the XML file at the specified path.
        /// </summary>
        /// <param name="path">The path to the XML file to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified XML file.</returns>
        public static ContentManifest LoadXml(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            var fss = FileSystemService.Create();
            using (var stream = fss.OpenRead(path))
            {
                return LoadXml(stream);
            }
        }

        /// <summary>
        /// Loads a content manifest from the JSON file at the specified path.
        /// </summary>
        /// <param name="path">The path to the JSON file to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified JSON file.</returns>
        public static ContentManifest LoadJson(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            var fss = FileSystemService.Create();
            using (var stream = fss.OpenRead(path))
            {
                return LoadJson(stream);
            }
        }

        /// <summary>
        /// Loads a content manifst from the specified XML stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the content manifest to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified XML stream.</returns>
        public static ContentManifest Load(Stream stream) =>
            LoadXml(stream);

        /// <summary>
        /// Loads a content manifst from the specified XML stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the content manifest to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified XML stream.</returns>
        public static ContentManifest LoadXml(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var xml = XDocument.Load(stream);

            var name = xml.Root.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestName);

            var groups = xml.Root.Elements("ContentGroup") ?? Enumerable.Empty<XElement>();
            return new ContentManifest(name, groups);
        }

        /// <summary>
        /// Loads a content manifst from the specified JSON stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the content manifest to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified JSON stream.</returns>
        public static ContentManifest LoadJson(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            using (var sreader = new StreamReader(stream))
            {
                using (var jreader = new JsonTextReader(sreader))
                {
                    var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
                    var desc = serializer.Deserialize<ContentManifestDescription>(jreader);
                    return new ContentManifest(desc);
                }
            }
        }

        /// <summary>
        /// Gets the content manifest's name.
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the specified item's name.
        /// </summary>
        /// <param name="item">The item for which to retrieve a name.</param>
        /// <returns>The specified item's name.</returns>
        protected override String GetName(ContentManifestGroup item)
        {
            return item.Name;
        }
    }
}
