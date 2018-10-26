using System;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// An internal content importer which can import any file as an array of bytes.
    /// </summary>
    internal sealed class InternalByteArrayImporter : ContentImporter<Byte[]>
    {
        /// <inheritdoc/>
        public override Byte[] Import(IContentImporterMetadata metadata, Stream stream)
        {
            var buffer = new Byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }

    /// <summary>
    /// An internal content importer which can import any file as a memory stream.
    /// </summary>
    internal sealed class InternalMemoryStreamImporter : ContentImporter<MemoryStream>
    {
        /// <inheritdoc/>
        public override MemoryStream Import(IContentImporterMetadata metadata, Stream stream)
        {
            var buffer = new Byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return new MemoryStream(buffer);
        }
    }
}
