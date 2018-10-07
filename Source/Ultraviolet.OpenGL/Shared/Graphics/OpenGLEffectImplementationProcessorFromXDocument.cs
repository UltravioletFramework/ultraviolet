using System;
using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Loads shader effect assets from XML definition files.
    /// </summary>
    [ContentProcessor]
    public sealed class OpenGLEffectImplementationProcessorFromXDocument : EffectImplementationProcessor<XDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEffectImplementationProcessorFromXDocument"/> class.
        /// </summary>
        public OpenGLEffectImplementationProcessorFromXDocument()
        {
            this.v1 = new OpenGLEffectImplementationProcessorFromXDocumentV1(this);
            this.v2 = new OpenGLEffectImplementationProcessorFromXDocumentV2(this);
        }

        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            var version = GetVersionFromXml(input);
            switch (version)
            {
                case 1:
                    v1.ExportPreprocessed(manager, metadata, writer, input, delete);
                    break;

                case 2:
                    v2.ExportPreprocessed(manager, metadata, writer, input, delete);
                    break;

                default:
                    throw new InvalidDataException();
            }
        }

        /// <inheritdoc/>
        public override EffectImplementation ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var version = GetVersionFromStream(reader);
            switch (version)
            {
                case 1:
                    return v1.ImportPreprocessed(manager, metadata, reader);

                case 2:
                    return v2.ImportPreprocessed(manager, metadata, reader);

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        public override EffectImplementation Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var version = GetVersionFromXml(input);
            switch (version)
            {
                case 1:
                    return v1.Process(manager, metadata, input);

                case 2:
                    return v2.Process(manager, metadata, input);

                default:
                    throw new InvalidDataException();
            }
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

        /// <summary>
        /// Gets the version of the specified XML effect definition file.
        /// </summary>
        private Int32 GetVersionFromXml(XDocument xml)
        {
            var versionAttr = xml.Root.Attribute("Version");
            if (versionAttr != null)
                return (Int32)versionAttr;
            
            return 1;
        }

        /// <summary>
        /// Gets the version of the specified preprocessed data stream.
        /// </summary>
        private Int32 GetVersionFromStream(BinaryReader reader)
        {
            const Int32 MagicHeaderLength = 4;

            if (reader.BaseStream.Length - reader.BaseStream.Position < MagicHeaderLength)
                return 1;

            var header = reader.ReadBytes(MagicHeaderLength);
            if (header[0] == 255 && header[1] == 255 && header[2] == 255)
            {
                return header[3];
            }
            else
            {
                reader.BaseStream.Seek(-MagicHeaderLength, SeekOrigin.Current);
                return 1;
            }
        }

        // Implementations for handling different file versions
        private readonly OpenGLEffectImplementationProcessorFromXDocumentV1 v1;
        private readonly OpenGLEffectImplementationProcessorFromXDocumentV2 v2;
    }
}
