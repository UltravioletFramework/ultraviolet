using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content importer which loads sprite definition files.
    /// </summary>
    [ContentImporter(".jssprite")]
    internal sealed class SpriteImporterToJObject : ContentImporter<JObject>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".jssprite" };

        /// <inheritdoc/>
        public override JObject Import(IContentImporterMetadata metadata, Stream stream)
        {
            using (var sreader = new StreamReader(stream))
            using (var jreader = new JsonTextReader(sreader))
            {
                return JObject.Load(jreader);
            }
        }
    }
}
