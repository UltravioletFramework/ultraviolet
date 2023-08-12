using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads text files.
    /// </summary>
    [ContentImporter(".txt")]
    public sealed class TextContentImporter : ContentImporter<String[]>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".txt" };

        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override String[] Import(IContentImporterMetadata metadata, Stream stream)
        {
            var lines = new List<String>();
            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }
    }
}
