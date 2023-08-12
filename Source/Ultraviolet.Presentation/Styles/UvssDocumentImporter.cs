using System;
using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a content importer for the *.uvss file type.
    /// </summary>
    [ContentImporter(".uvss")]
    public class UvssDocumentImporter : ContentImporter<String>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".uvss" };

        /// <inheritdoc/>
        public override String Import(IContentImporterMetadata metadata, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
