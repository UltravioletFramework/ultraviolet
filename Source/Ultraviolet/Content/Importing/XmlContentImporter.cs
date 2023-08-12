using System;
using System.IO;
using System.Xml.Linq;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads XML documents.
    /// </summary>
    [ContentImporter(".xml")]
    [ContentImporter(".prog")]
    public sealed class XmlContentImporter : ContentImporter<XDocument>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".xml", ".prog" };

        /// <inheritdoc/>
        public override XDocument Import(IContentImporterMetadata metadata, Stream stream)
        {
            return XmlUtil.Load(stream);
        }
    }
}
