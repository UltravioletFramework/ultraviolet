using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a content processor that loads text table layouts from XML definition files.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentProcessor]
    public sealed class TextTableLayoutProcessorFromJObject : ContentProcessor<JObject, TextTableLayout>
    {
        /// <inheritdoc/>
        public override TextTableLayout Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new UltravioletJsonConverter());

            var layoutDesc = input.ToObject<TextTableLayoutDescription>(serializer);
            return new TextTableLayout(layoutDesc);
        }
    }
}
