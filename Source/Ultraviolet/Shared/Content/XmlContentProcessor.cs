using System.Xml.Linq;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads XML documents.
    /// </summary>
    [ContentProcessor]
    public sealed class XmlContentProcessor : ContentProcessor<XDocument, XDocument>
    {
        /// <inheritdoc/>
        public override XDocument Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            return input;
        }
    }
}
