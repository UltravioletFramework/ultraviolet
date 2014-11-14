using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Content
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
            Contract.Require(name, "name");
            Contract.Require(groups, "groups");

            this.Name = name;

            foreach (var group in groups)
            {
                AddInternal(new ContentManifestGroup(this, group));
            }
        }

        /// <summary>
        /// Loads a content manifest from the file at the specified path.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified file.</returns>
        public static ContentManifest Load(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            var fss = FileSystemService.Create();
            using (var stream = fss.OpenRead(path))
            {
                return Load(stream);
            }
        }

        /// <summary>
        /// Loads a content manifst from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the content manifest to load.</param>
        /// <returns>The <see cref="ContentManifest"/> that was loaded from the specified stream.</returns>
        public static ContentManifest Load(Stream stream)
        {
            Contract.Require(stream, "stream");

            var xml = XDocument.Load(stream);

            var name = xml.Root.AttributeValueString("Name");
            if (String.IsNullOrEmpty(name))
                throw new InvalidDataException(UltravioletStrings.InvalidContentManifestName);

            var groups = xml.Root.Elements("ContentGroup") ?? Enumerable.Empty<XElement>();
            return new ContentManifest(name, groups);
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
