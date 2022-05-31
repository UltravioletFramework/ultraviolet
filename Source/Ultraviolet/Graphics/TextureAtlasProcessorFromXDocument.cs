using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a content processor which loads XML definition files as texture atlases.
    /// </summary>
    [ContentProcessor]
    public sealed partial class TextureAtlasProcessorFromXDocument : ContentProcessor<XDocument, TextureAtlas>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete) =>
            internalProcessor.ExportPreprocessed(manager, metadata, writer, CreateTextureAtlasDescription(input), delete);

        /// <inheritdoc/>
        public override TextureAtlas ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            internalProcessor.ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override TextureAtlas Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input) =>
            internalProcessor.Process(manager, metadata, CreateTextureAtlasDescription(input));

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing =>
            internalProcessor.SupportsPreprocessing;
        
        /// <summary>
        /// Creates a texture atlas description from the specified input XML.
        /// </summary>
        private TextureAtlasDescription CreateTextureAtlasDescription(XDocument input)
        {
            var atlasDesc = new TextureAtlasDescription();
            var atlasMetadata = new TextureAtlasMetadataDescription();
            atlasDesc.Metadata = atlasMetadata;

            var metadataElement = input.Root.Element("Metadata");
            if (metadataElement != null)
            {
                atlasMetadata.RootDirectory = (String)metadataElement.Element("RootDirectory") ?? atlasMetadata.RootDirectory;
                atlasMetadata.RequirePowerOfTwo = (Boolean?)metadataElement.Element("RequirePowerOfTwo") ?? atlasMetadata.RequirePowerOfTwo;
                atlasMetadata.RequireSquare = (Boolean?)metadataElement.Element("RequireSquare") ?? atlasMetadata.RequireSquare;
                atlasMetadata.MaximumWidth = (Int32?)metadataElement.Element("MaximumWidth") ?? atlasMetadata.MaximumWidth;
                atlasMetadata.MaximumHeight = (Int32?)metadataElement.Element("MaximumHeight") ?? atlasMetadata.MaximumHeight;
                atlasMetadata.Padding = (Int32?)metadataElement.Element("Padding") ?? atlasMetadata.Padding;
                atlasMetadata.FlattenCellName = (Boolean?)metadataElement.Element("FlattenCellName") ?? atlasMetadata.FlattenCellName;
            }

            var imageRootElement = input.Root.Element("Images");
            if (imageRootElement != null)
            {
                var imageDescCollection = new List<TextureAtlasImageDescription>();
                atlasDesc.Images = imageDescCollection;

                var imageElements = imageRootElement.Elements("Include");
                foreach (var imageElement in imageElements)
                {
                    var imageDesc = new TextureAtlasImageDescription();
                    imageDescCollection.Add(imageDesc);
                    
                    imageDesc.Name = (String)imageElement.Attribute("Name");
                    imageDesc.Path = imageElement.Value;
                }
            }

            return atlasDesc;
        }

        // Implements the conversion from TextureAtlasDescription to TextureAtlas.
        private readonly TextureAtlasProcessor internalProcessor = new TextureAtlasProcessor();
    }
}
