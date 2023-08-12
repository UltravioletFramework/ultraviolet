using System;
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
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".sprite" };

        /// <inheritdoc/>
        public override XDocument Import(IContentImporterMetadata metadata, Stream stream) =>
            XDocument.Load(stream);
    }
}
