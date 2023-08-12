using System;
using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Imports .vert files.
    /// </summary>
    [ContentImporter(".vert"), ContentImporter(".verth")]
    public sealed class OpenGLVertexShaderImporter : ContentImporter<String>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".vert", ".verth" };

        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The stream that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override String Import(IContentImporterMetadata metadata, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
