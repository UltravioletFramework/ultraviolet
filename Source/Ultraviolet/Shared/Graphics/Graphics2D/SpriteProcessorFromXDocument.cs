using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content processor which loads sprites from XML files.
    /// </summary>
    [ContentProcessor]
    internal sealed class SpriteProcessorFromXDocument : ContentProcessor<XDocument, Sprite>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete) =>
            implProcessor.ExportPreprocessed(manager, metadata, writer, CreateSpriteDescription(manager, metadata, input), delete);

        /// <inheritdoc/>
        public override Sprite ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            implProcessor.ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override Sprite Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input) =>
            implProcessor.Process(manager, metadata, CreateSpriteDescription(manager, metadata, input));

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => implProcessor.SupportsPreprocessing;
        
        /// <summary>
        /// Retrieves the nearest instance of the specified attribute in the XML hierarchy, starting
        /// at the specified element and moving up towards the root.
        /// </summary>
        private static XAttribute GetFrameAttribute(XElement element, String name)
        {
            var current = element;

            while (current != null)
            {
                if (current == element.Document.Root)
                    return null;

                var attr = current.Attribute(name);
                if (attr != null)
                    return attr;

                current = current.Parent;
            }

            return null;
        }

        /// <summary>
        /// Creates a sprite description from the specified input file.
        /// </summary>
        private static SpriteDescription CreateSpriteDescription(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var spriteDescription = new SpriteDescription();

            // Get all of the sprite's animation elements.
            var animationElementsSingle = input.Root.Elements("Animation");
            var animationElementsGroup = input.Root.Elements("Animations").SelectMany(x => x.Elements("Animation"));
            var animationElements = Enumerable.Union(animationElementsSingle, animationElementsGroup).ToList();
            var animationList = new List<SpriteAnimationDescription>();

            // Process each animation.
            foreach (var animationElement in animationElements)
            {
                var animationDesc = new SpriteAnimationDescription();
                animationDesc.Name = (String)animationElement.Attribute("Name");
                animationDesc.Repeat = (String)animationElement.Attribute("Repeat");

                // Get all of the animation's frame elements.
                var frameElementsSingle = animationElement.Elements("Frame");
                var frameElementsGroup = animationElement.Elements("Frames").SelectMany(x => x.Elements("Frame"));
                var frameElements = Enumerable.Union(frameElementsSingle, frameElementsGroup).ToList();
                var frameList = new List<SpriteFrameDescription>();
                animationDesc.Frames = new[] { new SpriteFrameBatchDescription() { Items = frameList } };

                // Process each frame.
                foreach (var frameElement in frameElements)
                {
                    var frameDescription = new SpriteFrameDescription();
                    frameDescription.Atlas = ResolveDependencyAssetPath(metadata, (String)GetFrameAttribute(frameElement, "Atlas"));
                    frameDescription.AtlasCell = (String)GetFrameAttribute(frameElement, "AtlasCell");
                    frameDescription.Texture = ResolveDependencyAssetPath(metadata, (String)GetFrameAttribute(frameElement, "Texture"));
                    frameDescription.X = (Int32?)GetFrameAttribute(frameElement, "X") ?? 0;
                    frameDescription.Y = (Int32?)GetFrameAttribute(frameElement, "Y") ?? 0;
                    frameDescription.Width = (Int32?)GetFrameAttribute(frameElement, "Width") ?? 0;
                    frameDescription.Height = (Int32?)GetFrameAttribute(frameElement, "Height") ?? 0;
                    frameDescription.Origin = new Point2(
                        (Int32?)GetFrameAttribute(frameElement, "OriginX") ?? 0,
                        (Int32?)GetFrameAttribute(frameElement, "OriginY") ?? 0);                        
                    frameDescription.Duration = (Int32?)GetFrameAttribute(frameElement, "Duration") ?? 0;

                    // VALIDATION: Both atlas and texture specified
                    if (frameDescription.Atlas != null && frameDescription.Texture != null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsBothTextureAndAtlas);

                    // VALIDATION: Atlas cell, but no atlas
                    if (frameDescription.Atlas == null && frameDescription.AtlasCell != null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsCellButNoAtlas);

                    // VALIDATION: Atlas, but no atlas cell
                    if (frameDescription.Atlas != null && frameDescription.AtlasCell == null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsAtlasButNoCell);

                    frameList.Add(frameDescription);
                }

                // Get all of the animation's frame groups.
                var frameGroupElementsSingle = animationElement.Elements("FrameGroup");
                var frameGroupElementsGroup = animationElement.Elements("FrameGroups").SelectMany(x => x.Elements("FrameGroup"));
                var frameGroupElements = Enumerable.Union(frameGroupElementsSingle, frameGroupElementsGroup).ToList();
                var frameGroupList = new List<SpriteFrameGroupDescription>();
                animationDesc.FrameGroups = new[] { new SpriteFrameGroupBatchDescription() { Items = frameGroupList } };

                // Process eachh frame group.
                foreach (var frameGroupElement in frameGroupElements)
                {
                    var frameGroupDescription = new SpriteFrameGroupDescription();
                    frameGroupDescription.Texture = ResolveDependencyAssetPath(metadata, (String)GetFrameAttribute(frameGroupElement, "Texture"));
                    frameGroupDescription.X = (Int32?)GetFrameAttribute(frameGroupElement, "AreaX") ?? 0;
                    frameGroupDescription.Y = (Int32?)GetFrameAttribute(frameGroupElement, "AreaY") ?? 0;
                    frameGroupDescription.Width = (Int32?)GetFrameAttribute(frameGroupElement, "AreaWidth") ?? 0;
                    frameGroupDescription.Height = (Int32?)GetFrameAttribute(frameGroupElement, "AreaHeight") ?? 0;
                    frameGroupDescription.FrameWidth = (Int32?)GetFrameAttribute(frameGroupElement, "FrameWidth") ?? 0;
                    frameGroupDescription.FrameHeight = (Int32?)GetFrameAttribute(frameGroupElement, "FrameHeight") ?? 0;
                    frameGroupDescription.FrameCount = (Int32?)GetFrameAttribute(frameGroupElement, "FrameCount") ?? 0;
                    frameGroupDescription.Origin = new Point2(
                        (Int32?)GetFrameAttribute(frameGroupElement, "OriginX") ?? 0,
                        (Int32?)GetFrameAttribute(frameGroupElement, "OriginX") ?? 0);
                    frameGroupDescription.Duration = (Int32?)GetFrameAttribute(frameGroupElement, "Duration") ?? 0;

                    frameGroupList.Add(frameGroupDescription);
                }

                animationList.Add(animationDesc);
            }
            spriteDescription.Animations = new[] { new SpriteAnimationBatchDescription() { Items = animationList.ToArray() } };

            return spriteDescription;
        }

        // The processor which implements the conversion from SpriteDescription -> Sprite.
        private readonly SpriteProcessor implProcessor = new SpriteProcessor();
    }
}
