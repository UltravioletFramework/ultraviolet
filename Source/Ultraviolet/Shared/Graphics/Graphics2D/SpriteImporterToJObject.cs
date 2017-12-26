using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content importer which loads sprite definition files.
    /// </summary>
    [Preserve(AllMembers = true)]
    [ContentImporter(".jssprite")]
    internal sealed class SpriteImporterToJObject : ContentImporter<JObject>
    {
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
