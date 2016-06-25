using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads XML documents.
    /// </summary>
    [ContentProcessor]
    public sealed class JsonContentProcessor : ContentProcessor<JObject, JObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentProcessor"/> class.
        /// </summary>
        [Preserve]
        public JsonContentProcessor() { }

        /// <inheritdoc/>
        public override JObject Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            return input;
        }
    }
}
