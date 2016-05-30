using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content processor which loads sprites.
    /// </summary>
    [ContentProcessor]
    internal sealed class SpriteProcessorFromXDocument : ContentProcessor<XDocument, Sprite>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            var description = CreateSpriteDescription(manager, metadata, input);

            var animations = description.Animations?.SelectMany(x => x.Items)?.ToList();
            if (animations != null)
            {
                writer.Write(animations.Count);

                foreach (var animation in animations)
                {
                    writer.Write(animation.Name ?? String.Empty);
                    writer.Write(animation.Repeat ?? String.Empty);
                    
                    var frameGroups = animation.FrameGroups?.SelectMany(x => x.Items)?.ToList();
                    if (frameGroups != null)
                    {
                        writer.Write(frameGroups.Count);

                        foreach (var group in frameGroups)
                        {
                            writer.Write(group.Texture ?? String.Empty);
                            writer.Write(group.Area?.X ?? 0);
                            writer.Write(group.Area?.Y ?? 0);
                            writer.Write(group.Area?.Width ?? 0);
                            writer.Write(group.Area?.Height ?? 0);
                            writer.Write(group.FrameWidth ?? 0);
                            writer.Write(group.FrameHeight ?? 0);
                            writer.Write(group.FrameCount ?? 0);
                            writer.Write(group.Origin?.X ?? 0);
                            writer.Write(group.Origin?.Y ?? 0);
                            writer.Write(group.Duration ?? 0);
                        }
                    }
                    else
                    {
                        writer.Write(0);
                    }
                    
                    var frames = animation.Frames?.SelectMany(x => x.Items).ToList();
                    if (frames != null)
                    {
                        writer.Write(frames.Count);

                        foreach (var frame in frames)
                        {
                            writer.Write(frame.Atlas ?? String.Empty);
                            writer.Write(frame.AtlasCell ?? String.Empty);
                            writer.Write(frame.Texture ?? String.Empty);
                            writer.Write(frame.Area?.X ?? 0);
                            writer.Write(frame.Area?.Y ?? 0);
                            writer.Write(frame.Area?.Width ?? 0);
                            writer.Write(frame.Area?.Height ?? 0);
                            writer.Write(frame.Origin?.X ?? 0);
                            writer.Write(frame.Origin?.Y ?? 0);
                            writer.Write(frame.Duration ?? 0);
                        }
                    }
                    else
                    {
                        writer.Write(0);
                    }
                }
            }
            else
            {
                writer.Write(0);
            }
        }

        /// <inheritdoc/>
        public override Sprite ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var description = new SpriteDescription();

            var animationCount = reader.ReadInt32();
            if (animationCount > 0)
            {
                var animations = new SpriteAnimationDescription[animationCount];

                description.Animations = new[] { new SpriteAnimationBatchDescription() };
                description.Animations[0].Items = animations;

                for (int i = 0; i < animationCount; i++)
                {
                    animations[i] = new SpriteAnimationDescription();
                    animations[i].Name = reader.ReadString();
                    animations[i].Repeat = reader.ReadString();

                    var groupCount = reader.ReadInt32();
                    var groups = new SpriteFrameGroupDescription[groupCount];

                    animations[i].FrameGroups = new[] { new SpriteFrameGroupBatchDescription() };
                    animations[i].FrameGroups[0].Items = groups;

                    for (int j = 0; j < groupCount; j++)
                    {
                        var group = new SpriteFrameGroupDescription();
                        group.Texture = reader.ReadString();
                        group.Area = new Rectangle(
                            reader.ReadInt32(),
                            reader.ReadInt32(),
                            reader.ReadInt32(),
                            reader.ReadInt32());
                        group.FrameWidth = reader.ReadInt32();
                        group.FrameHeight = reader.ReadInt32();
                        group.FrameCount = reader.ReadInt32();
                        group.Origin = new Point2(
                            reader.ReadInt32(),
                            reader.ReadInt32());
                        group.Duration = reader.ReadInt32();

                        groups[j] = group;
                    }

                    var frameCount = reader.ReadInt32();
                    var frames = new SpriteFrameDescription[frameCount];

                    animations[i].Frames = new[] { new SpriteFrameBatchDescription() };
                    animations[i].Frames[0].Items = frames;

                    for (int j = 0; j < frameCount; j++)
                    {
                        var frame = new SpriteFrameDescription();
                        frame.Atlas = reader.ReadString();
                        frame.AtlasCell = reader.ReadString();
                        frame.Texture = reader.ReadString();
                        frame.Area = new Rectangle(
                            reader.ReadInt32(),
                            reader.ReadInt32(),
                            reader.ReadInt32(),
                            reader.ReadInt32());
                        frame.Origin = new Point2(
                            reader.ReadInt32(),
                            reader.ReadInt32());
                        frame.Duration = reader.ReadInt32();

                        frames[j] = frame;
                    }
                }
            }

            return CreateSprite(manager, metadata, description);
        }

        /// <inheritdoc/>
        public override Sprite Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var description = CreateSpriteDescription(manager, metadata, input);
            return CreateSprite(manager, metadata, description);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;
        
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
                    frameDescription.Area = new Rectangle(
                        (Int32?)GetFrameAttribute(frameElement, "X") ?? 0,
                        (Int32?)GetFrameAttribute(frameElement, "Y") ?? 0,
                        (Int32?)GetFrameAttribute(frameElement, "Width") ?? 0,
                        (Int32?)GetFrameAttribute(frameElement, "Height") ?? 0);
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
                    frameGroupDescription.Area = new Rectangle(
                        (Int32?)GetFrameAttribute(frameGroupElement, "AreaX") ?? 0,
                        (Int32?)GetFrameAttribute(frameGroupElement, "AreaY") ?? 0,
                        (Int32?)GetFrameAttribute(frameGroupElement, "AreaWidth") ?? 0,
                        (Int32?)GetFrameAttribute(frameGroupElement, "AreaHeight") ?? 0);
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

        /// <summary>
        /// Creates a sprite from the specified description.
        /// </summary>
        private static Sprite CreateSprite(ContentManager manager, IContentProcessorMetadata metadata, SpriteDescription input)
        {
            // Process sprite's animation data.
            var animations = new List<SpriteAnimation>();
            foreach (var animBatch in input.Animations)
            {
                foreach (var animDesc in animBatch.Items)
                {
                    var animName = animDesc.Name;
                    var animRepeat = animDesc.Repeat == "none" ? SpriteAnimationRepeat.None : SpriteAnimationRepeat.Loop;
                    var animation = new SpriteAnimation(animName, animRepeat);

                    // Process the animation's frame groups.
                    foreach (var groupBatch in animDesc.FrameGroups)
                    {
                        foreach (var groupDesc in groupBatch.Items)
                        {
                            var groupTexture = manager.Load<Texture2D>(groupDesc.Texture);

                            var groupFrameCount = groupDesc.FrameCount ?? 0;
                            var groupFrameWidth = groupDesc.FrameWidth ?? 0;
                            var groupFrameHeight = groupDesc.FrameHeight ?? 0;

                            var groupAreaX = groupDesc.Area?.X ?? 0;
                            var groupAreaY = groupDesc.Area?.Y ?? 0;
                            var groupAreaWidth = groupDesc.Area?.Width ?? 0;
                            var groupAreaHeight = groupDesc.Area?.Height ?? 0;

                            var groupOrigin = groupDesc?.Origin ?? Point2.Zero;
                            var groupDuration = groupDesc?.Duration ?? 0;

                            var groupX = groupAreaX;
                            var groupY = groupAreaY;

                            for (int i = 0; i < groupFrameCount; i++)
                            {
                                if (groupAreaHeight > 0 && groupY + groupFrameHeight > groupY + groupAreaHeight)
                                    break;

                                var frame = new SpriteFrameDescription();
                                frame.Texture = groupDesc.Texture;
                                frame.Area = new Rectangle(
                                    groupX,
                                    groupY,
                                    groupFrameWidth,
                                    groupFrameHeight);
                                frame.Origin = groupOrigin;
                                frame.Duration = groupDuration;
                                animation.Frames.Add(new SpriteFrame(frame, groupTexture));

                                groupX += groupFrameWidth;
                                if (groupX + groupFrameWidth > groupAreaX + groupAreaWidth)
                                {
                                    groupX = groupAreaX;
                                    groupY = groupY + groupFrameHeight;
                                }
                            }
                        }
                    }

                    // Process the animation's frame data.
                    foreach (var frameBatch in animDesc.Frames)
                    {
                        foreach (var frameDesc in frameBatch.Items)
                        {
                            var frame = new SpriteFrameDescription();
                            frame.Origin = frameDesc.Origin ?? Point2.Zero;
                            frame.Duration = frameDesc.Duration ?? 0;

                            if (!String.IsNullOrWhiteSpace(frameDesc.Atlas))
                            {
                                var atlas = manager.Load<TextureAtlas>(frameDesc.Atlas);
                                if (!atlas.ContainsCell(frame.AtlasCell))
                                    throw new InvalidDataException(UltravioletStrings.SpriteContainsInvalidAtlasCell.Format(frame.AtlasCell));

                                var cell = atlas[frame.AtlasCell];

                                frame.Atlas = frameDesc.Atlas;
                                frame.AtlasCell = frameDesc.AtlasCell;
                                frame.Area = new Rectangle(cell.X, cell.Y, cell.Width, cell.Height);
                                animation.Frames.Add(new SpriteFrame(frame, atlas));
                            }
                            else
                            {
                                var texture = manager.Load<Texture2D>(frameDesc.Texture);

                                frame.Texture = frameDesc.Texture;
                                frame.Area = new Rectangle(
                                    frameDesc.Area?.X ?? 0,
                                    frameDesc.Area?.Y ?? 0,
                                    frameDesc.Area?.Width ?? 0,
                                    frameDesc.Area?.Height ?? 0);
                                animation.Frames.Add(new SpriteFrame(frame, texture));
                            }
                        }
                    }

                    // Initialize the animation's default controller.
                    animation.Controller.PlayAnimation(animation);
                    animations.Add(animation);
                }
            }
            return new Sprite(animations);
        }
    }
}
