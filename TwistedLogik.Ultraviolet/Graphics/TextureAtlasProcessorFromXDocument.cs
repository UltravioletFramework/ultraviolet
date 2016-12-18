using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a content processor which loads XML definition files as texture atlases.
    /// </summary>
    [Preserve(AllMembers = true)]
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

            var metadataElement = input.Root.Elements().Where(x => x.Name.LocalName == "Metadata").SingleOrDefault();
            if (metadataElement != null)
            {
                atlasMetadata.RootDirectory = 
                    (String)metadataElement.Elements().Where(x => x.Name.LocalName == "RootDirectory").SingleOrDefault() ?? atlasMetadata.RootDirectory;
                atlasMetadata.RequirePowerOfTwo = 
                    (Boolean?)metadataElement.Elements().Where(x => x.Name.LocalName == "RequirePowerOfTwo").SingleOrDefault() ?? atlasMetadata.RequirePowerOfTwo;
                atlasMetadata.RequireSquare = 
                    (Boolean?)metadataElement.Elements().Where(x => x.Name.LocalName == "RequireSquare").SingleOrDefault() ?? atlasMetadata.RequireSquare;
                atlasMetadata.MaximumWidth = 
                    (Int32?)metadataElement.Elements().Where(x => x.Name.LocalName == "MaximumWidth").SingleOrDefault() ?? atlasMetadata.MaximumWidth;
                atlasMetadata.MaximumHeight = 
                    (Int32?)metadataElement.Elements().Where(x => x.Name.LocalName == "MaximumHeight").SingleOrDefault() ?? atlasMetadata.MaximumHeight;
                atlasMetadata.Padding = 
                    (Int32?)metadataElement.Elements().Where(x => x.Name.LocalName == "Padding").SingleOrDefault() ?? atlasMetadata.Padding;
            }

            var imageRootElement = input.Root.Elements().Where(x => x.Name.LocalName == "Images").SingleOrDefault();
            if (imageRootElement != null)
            {
                var imageDescCollection = new List<TextureAtlasImageDescription>();
                atlasDesc.Images = imageDescCollection;

                var imageElements = imageRootElement.Elements().Where(x => x.Name.LocalName == "Include");
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
