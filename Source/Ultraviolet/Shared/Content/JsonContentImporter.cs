using System.IO;
using Newtonsoft.Json.Linq;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads JSON documents.
    /// </summary>
    [ContentImporter(".json")]
    public sealed class JsonContentImporter : ContentImporter<JObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentImporter"/> class.
        /// </summary>
        public JsonContentImporter() { }

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
