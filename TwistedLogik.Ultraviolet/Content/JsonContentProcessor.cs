using Newtonsoft.Json.Linq;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads XML documents.
    /// </summary>
    [ContentProcessor]
    public sealed class JsonContentProcessor : ContentProcessor<JObject, JObject>
    {
        /// <inheritdoc/>
        public override JObject Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            return input;
        }
    }
}
