using Newtonsoft.Json.Linq;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads JSON documents.
    /// </summary>
    [ContentProcessor]
    public sealed class JsonContentProcessor : ContentProcessor<JObject, JObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentProcessor"/> class.
        /// </summary>
        public JsonContentProcessor() { }

        /// <inheritdoc/>
        public override JObject Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            return input;
        }
    }
}
