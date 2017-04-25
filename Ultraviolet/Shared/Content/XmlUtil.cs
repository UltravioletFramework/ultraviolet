using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Contains utility methods for handling XML files.
    /// </summary>
    public static class XmlUtil
    {
        /// <summary>
        /// Loads the XML document from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from which to load the XML document.</param>
        /// <returns>An <see cref="XDocument"/> representing the XML document that was loaded.</returns>
        public static XDocument Load(Stream stream)
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
