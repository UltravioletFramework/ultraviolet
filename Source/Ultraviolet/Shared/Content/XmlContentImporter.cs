using System.IO;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads XML documents.
    /// </summary>
    [ContentImporter(".xml")]
    public sealed class XmlContentImporter : ContentImporter<XDocument>
    {
        /// <inheritdoc/>
        public override XDocument Import(IContentImporterMetadata metadata, Stream stream)
        {
            return XmlUtil.Load(stream);
        }
    }
}
