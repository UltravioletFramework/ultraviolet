using System.Xml.Linq;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads XML documents.
    /// </summary>
    [Preserve(AllMembers = true)]
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
