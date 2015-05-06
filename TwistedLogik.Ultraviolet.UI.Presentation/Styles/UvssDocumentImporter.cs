using System;
using System.IO;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a content importer for the *.uvss file type.
    /// </summary>
    [ContentImporter(".uvss")]
    public class UvssDocumentImporter : ContentImporter<String>
    {
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
