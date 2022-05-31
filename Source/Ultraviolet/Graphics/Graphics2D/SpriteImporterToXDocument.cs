using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content importer which loads sprite definition files.
    /// </summary>
    [ContentImporter(".sprite")]
    internal sealed class SpriteImporterToXDocument : ContentImporter<XDocument>
    {
        /// <inheritdoc/>
        public override XDocument Import(IContentImporterMetadata metadata, Stream stream) =>
            XDocument.Load(stream);
    }
}
