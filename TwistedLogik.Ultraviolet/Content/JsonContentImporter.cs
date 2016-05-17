using Newtonsoft.Json.Linq;
using System.IO;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads JSON documents.
    /// </summary>
    [ContentImporter(".json")]
    public sealed class JsonContentImporter : ContentImporter<JObject>
    {
        /// <inheritdoc/>
        public override JObject Import(IContentImporterMetadata metadata, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JObject.Parse(json);
            }
        }
    }
}
