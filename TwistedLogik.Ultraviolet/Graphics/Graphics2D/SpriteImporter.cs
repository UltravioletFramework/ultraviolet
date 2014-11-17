using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content importer which loads sprite definition files.
    /// </summary>
    [ContentImporter(".sprite")]
    [ContentImporter(".jssprite")]
    public sealed class SpriteImporter : ContentImporter<SpriteDescription>
    {
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The <see cref="Stream"/> that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override SpriteDescription Import(IContentImporterMetadata metadata, Stream stream)
        {
            var ext = (metadata.AssetFilePath == null) ? null : Path.GetExtension(metadata.AssetFilePath);
            if (ext == ".jssprite")
            {
                using (var sreader = new StreamReader(stream))
                using (var jreader = new JsonTextReader(sreader))
                {
                    var json = JObject.Load(jreader);
                    return Import(metadata, DataElement.CreateFromJson(json));
                }
            }
            return Import(metadata, DataElement.CreateFromXml(XmlUtil.Load(stream)));
        }

        /// <summary>
        /// Retrieves a frame attribute from the specified element and its ancestors.
        /// </summary>
        /// <param name="element">The element that defines the frame.</param>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The attribute value that was retrieved.</returns>
        private static T GetFrameAttribute<T>(DataElement element, String name)
        {
            var current = element;
            while (current != null)
            {
                if (current.Name == "Sprite")
                    break;

                var attr = current.Attribute(name);
                if (attr != null)
                {
                    return (T)Convert.ChangeType(attr.Value, typeof(T));
                }

                current = current.Parent;
            }

            return default(T);
        }

        /// <summary>
        /// Imports a sprite.
        /// </summary>
        /// <param name="metadata">The content importer metadata.</param>
        /// <param name="data">The data element that defines the sprite.</param>
        /// <returns>The sprite that was imported.</returns>
        private SpriteDescription Import(IContentImporterMetadata metadata, DataElement data)
        {
            var spriteDescription = new SpriteDescription();

            // Get all of the sprite's animation elements.
            var animationsRoots = data.Elements("Animations");
            var animations = Enumerable.Union(animationsRoots.SelectMany(x => x.Elements("Animation")), data.Elements("Animation"));
            var animationsList = new List<SpriteAnimationDescription>();

            // Process each animation.
            foreach (var animation in animations)
            {
                var animationDescription = new SpriteAnimationDescription();
                animationDescription.Name = animation.AttributeValue<String>("Name");
                animationDescription.Repeat = animation.AttributeValue<String>("Repeat");

                // Process frames.
                var framesRoots = animation.Elements("Frames");
                var frames = Enumerable.Union(framesRoots.SelectMany(x => x.Elements("Frame")), animation.Elements("Frame"));
                var framesList = new List<SpriteFrameDescription>();
                foreach (var frame in frames)
                {
                    var frameDescription = new SpriteFrameDescription();
                    frameDescription.Atlas = ResolveDependencyAssetPath(metadata, GetFrameAttribute<String>(frame, "Atlas"));
                    frameDescription.AtlasCell = GetFrameAttribute<String>(frame, "AtlasCell");
                    frameDescription.Texture = ResolveDependencyAssetPath(metadata, GetFrameAttribute<String>(frame, "Texture"));
                    frameDescription.X = GetFrameAttribute<Int32>(frame, "X");
                    frameDescription.Y = GetFrameAttribute<Int32>(frame, "Y");
                    frameDescription.Width = GetFrameAttribute<Int32>(frame, "Width");
                    frameDescription.Height = GetFrameAttribute<Int32>(frame, "Height");
                    frameDescription.OriginX = GetFrameAttribute<Int32>(frame, "OriginX");
                    frameDescription.OriginY = GetFrameAttribute<Int32>(frame, "OriginY");
                    frameDescription.Duration = GetFrameAttribute<Int32>(frame, "Duration");

                    // VALIDATION: Both atlas and texture specified
                    if (frameDescription.Atlas != null && frameDescription.Texture != null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsBothTextureAndAtlas);

                    // VALIDATION: Atlas cell, but no atlas
                    if (frameDescription.Atlas == null && frameDescription.AtlasCell != null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsCellButNoAtlas);

                    // VALIDATION: Atlas, but no atlas cell
                    if (frameDescription.Atlas != null && frameDescription.AtlasCell == null)
                        throw new InvalidDataException(UltravioletStrings.SpriteContainsAtlasButNoCell);

                    framesList.Add(frameDescription);
                }
                animationDescription.Frames = framesList.ToArray();

                // Process frame groups.
                var frameGroupsRoots = animation.Elements("FrameGroups");
                var frameGroups = Enumerable.Union(frameGroupsRoots.SelectMany(x => x.Elements("FrameGroup")), animation.Elements("FrameGroup"));
                var frameGroupsList = new List<SpriteFrameGroupDescription>();
                foreach (var frameGroup in frameGroups)
                {
                    var frameGroupDescription = new SpriteFrameGroupDescription();
                    frameGroupDescription.Texture = ResolveDependencyAssetPath(metadata, GetFrameAttribute<String>(frameGroup, "Texture"));
                    frameGroupDescription.AreaX = GetFrameAttribute<Int32>(frameGroup, "AreaX");
                    frameGroupDescription.AreaY = GetFrameAttribute<Int32>(frameGroup, "AreaY");
                    frameGroupDescription.AreaWidth = GetFrameAttribute<Int32>(frameGroup, "AreaWidth");
                    frameGroupDescription.AreaHeight = GetFrameAttribute<Int32>(frameGroup, "AreaHeight");
                    frameGroupDescription.FrameWidth = GetFrameAttribute<Int32>(frameGroup, "FrameWidth");
                    frameGroupDescription.FrameHeight = GetFrameAttribute<Int32>(frameGroup, "FrameHeight");
                    frameGroupDescription.FrameCount = GetFrameAttribute<Int32>(frameGroup, "FrameCount");
                    frameGroupDescription.OriginX = GetFrameAttribute<Int32>(frameGroup, "OriginX");
                    frameGroupDescription.OriginY = GetFrameAttribute<Int32>(frameGroup, "OriginY");
                    frameGroupDescription.Duration = GetFrameAttribute<Int32>(frameGroup, "Duration");

                    frameGroupsList.Add(frameGroupDescription);
                }
                animationDescription.FrameGroups = frameGroupsList.ToArray();

                animationsList.Add(animationDescription);
            }
            spriteDescription.Animations = animationsList.ToArray();

            return spriteDescription;
        }
    }
}
