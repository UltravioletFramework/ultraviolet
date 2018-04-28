using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents a content processor that loads cursor collections.
    /// </summary>
    [ContentProcessor]
    internal sealed class CursorCollectionProcessorFromJObject : ContentProcessor<JObject, CursorCollection>
    {
        /// <inheritdoc/>
        public override CursorCollection Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
            var desc = input.ToObject<CursorCollectionDescription>(serializer);
            return innerProcessor.Process(manager, metadata, desc);
        }

        private static readonly CursorCollectionProcessor innerProcessor =
            new CursorCollectionProcessor();
    }
}
