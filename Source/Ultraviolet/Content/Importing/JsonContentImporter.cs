using System;
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
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".json" };

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
