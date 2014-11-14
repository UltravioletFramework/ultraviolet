using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content importer which loads XML documents.
    /// </summary>
    [ContentImporter(".xml")]
    public sealed class XmlContentImporter : ContentImporter<XDocument>
    {
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override XDocument Import(IContentImporterMetadata metadata, Stream stream)
        {
            /* FIX:
             * Either XML loading on Xamarin is a bit wonky or I'm doing something wrong;
             * a straight XDocument.Load() doesn't seem to work here. Manually removing
             * the UTF-8 BOM and parsing the document as a string does the trick.
             */
            if (StripUtf8Preamble(stream))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var xml = reader.ReadToEnd();
                    return XDocument.Parse(xml);
                }
            }
            return XDocument.Load(stream);
        }

        /// <summary>
        /// Strips the UTF-8 preamble from the specified stream, if it exists.
        /// </summary>
        /// <param name="stream">The stream from which to strip the UTF-8 preamble.</param>
        /// <returns>true if the preamble was stripped; otherwise, false.</returns>
        private static Boolean StripUtf8Preamble(Stream stream)
        {
            var position = stream.Position;
            var utf8Preamble = Encoding.UTF8.GetPreamble();
            if (stream.Length >= utf8Preamble.Length)
            {
                var utf8PreambleBuffer = new Byte[utf8Preamble.Length];
                stream.Read(utf8PreambleBuffer, 0, utf8PreambleBuffer.Length);
                for (var i = 0; i < utf8Preamble.Length; i++)
                {
                    if (utf8Preamble[i] != utf8PreambleBuffer[i])
                    {
                        stream.Seek(position, SeekOrigin.Begin);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
