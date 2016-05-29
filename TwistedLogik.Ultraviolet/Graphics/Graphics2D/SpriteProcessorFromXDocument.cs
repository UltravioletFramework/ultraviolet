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

            writer.Write(description.Animations?.Count ?? 0);
            if (description.Animations != null)
            {
                foreach (var animation in description.Animations)
                {
                    writer.Write(animation.Name ?? String.Empty);
                    writer.Write(animation.Repeat ?? String.Empty);

                    writer.Write(animation.FrameGroups?.Count ?? 0);
                    if (animation.FrameGroups != null)
                    {
                        foreach (var group in animation.FrameGroups)
                        {
                            writer.Write(group.Texture ?? String.Empty);
                            writer.Write(group.AreaX);
                            writer.Write(group.AreaY);
                            writer.Write(group.AreaWidth);
                            writer.Write(group.AreaHeight);
                            writer.Write(group.FrameWidth);
                            writer.Write(group.FrameHeight);
                            writer.Write(group.FrameCount);
                            writer.Write(group.OriginX);
                            writer.Write(group.OriginY);
                            writer.Write(group.Duration);
                        }
                    }

                    writer.Write(animation.Frames?.Count ?? 0);
                    if (animation.Frames != null)
                    {
                        foreach (var frame in animation.Frames)
                        {
                            writer.Write(frame.Atlas ?? String.Empty);
                            writer.Write(frame.AtlasCell ?? String.Empty);
                            writer.Write(frame.Texture ?? String.Empty);
                            writer.Write(frame.X);
                            writer.Write(frame.Y);
                            writer.Write(frame.Width);
                            writer.Write(frame.Height);
                            writer.Write(frame.OriginX);
                            writer.Write(frame.OriginY);
                            writer.Write(frame.Duration);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override Sprite ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var description = new SpriteDescription();

            var animationCount = reader.ReadInt32();
            if (animationCount > 0)
            {
                description.Animations = new SpriteAnimationDescription[animationCount];
                for (int i = 0; i < animationCount; i++)
                {
                    description.Animations[i] = new SpriteAnimationDescription();
                    description.Animations[i].Name = reader.ReadString();
                    description.Animations[i].Repeat = reader.ReadString();

                    var groupCount = reader.ReadInt32();
                    description.Animations[i].FrameGroups = new SpriteFrameGroupDescription[groupCount];
                    for (int j = 0; j < groupCount; j++)
                    {
                        var group = new SpriteFrameGroupDescription();
                        group.Texture = reader.ReadString();
                        group.AreaX = reader.ReadInt32();
                        group.AreaY = reader.ReadInt32();
                        group.AreaWidth = reader.ReadInt32();
                        group.AreaHeight = reader.ReadInt32();
                        group.FrameWidth = reader.ReadInt32();
                        group.FrameHeight = reader.ReadInt32();
                        group.FrameCount = reader.ReadInt32();
                        group.OriginX = reader.ReadInt32();
                        group.OriginY = reader.ReadInt32();
                        group.Duration = reader.ReadInt32();
                        description.Animations[i].FrameGroups[j] = group;
                    }

                    var frameCount = reader.ReadInt32();
                    description.Animations[i].Frames = new SpriteFrameDescription[frameCount];
                    for (int j = 0; j < frameCount; j++)
                    {
                        var frame = new SpriteFrameDescription();
                        frame.Atlas = reader.ReadString();
                        frame.AtlasCell = reader.ReadString();
                        frame.Texture = reader.ReadString();
                        frame.X = reader.ReadInt32();
                        frame.Y = reader.ReadInt32();
                        frame.Width = reader.ReadInt32();
                        frame.Height = reader.ReadInt32();
                        frame.OriginX = reader.ReadInt32();
                        frame.OriginY = reader.ReadInt32();
                        frame.Duration = reader.ReadInt32();
                        description.Animations[i].Frames[j] = frame;
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
                animationDesc.Frames = frameList;

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
                    frameDescription.OriginX = (Int32?)GetFrameAttribute(frameElement, "OriginX") ?? 0;
                    frameDescription.OriginY = (Int32?)GetFrameAttribute(frameElement, "OriginY") ?? 0;
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
                animationDesc.FrameGroups = frameGroupList;

                // Process eachh frame group.
                foreach (var frameGroupElement in frameGroupElements)
                {
                    var frameGroupDescription = new SpriteFrameGroupDescription();
                    frameGroupDescription.Texture = ResolveDependencyAssetPath(metadata, (String)GetFrameAttribute(frameGroupElement, "Texture"));
                    frameGroupDescription.AreaX = (Int32?)GetFrameAttribute(frameGroupElement, "AreaX") ?? 0;
                    frameGroupDescription.AreaY = (Int32?)GetFrameAttribute(frameGroupElement, "AreaY") ?? 0;
                    frameGroupDescription.AreaWidth = (Int32?)GetFrameAttribute(frameGroupElement, "AreaWidth") ?? 0;
                    frameGroupDescription.AreaHeight = (Int32?)GetFrameAttribute(frameGroupElement, "AreaHeight") ?? 0;
                    frameGroupDescription.FrameWidth = (Int32?)GetFrameAttribute(frameGroupElement, "FrameWidth") ?? 0;
                    frameGroupDescription.FrameHeight = (Int32?)GetFrameAttribute(frameGroupElement, "FrameHeight") ?? 0;
                    frameGroupDescription.FrameCount = (Int32?)GetFrameAttribute(frameGroupElement, "FrameCount") ?? 0;
                    frameGroupDescription.OriginX = (Int32?)GetFrameAttribute(frameGroupElement, "OriginX") ?? 0;
                    frameGroupDescription.OriginY = (Int32?)GetFrameAttribute(frameGroupElement, "OriginY") ?? 0;
                    frameGroupDescription.Duration = (Int32?)GetFrameAttribute(frameGroupElement, "Duration") ?? 0;

                    frameGroupList.Add(frameGroupDescription);
                }

                animationList.Add(animationDesc);
            }
            spriteDescription.Animations = animationList.ToArray();

            return spriteDescription;
        }

        /// <summary>
        /// Creates a sprite from the specified description.
        /// </summary>
        private static Sprite CreateSprite(ContentManager manager, IContentProcessorMetadata metadata, SpriteDescription input)
        {
            // Process sprite's animation data.
            var animations = new List<SpriteAnimation>();
            foreach (var animdesc in input.Animations)
            {
                var animName = animdesc.Name;
                var animRepeat = animdesc.Repeat == "none" ? SpriteAnimationRepeat.None : SpriteAnimationRepeat.Loop;
                var animation = new SpriteAnimation(animName, animRepeat);

                // Process the animation's frame groups.
                foreach (var groupDesc in animdesc.FrameGroups)
                {
                    var groupX = groupDesc.AreaX;
                    var groupY = groupDesc.AreaY;
                    var groupTexture = manager.Load<Texture2D>(groupDesc.Texture);

                    for (int i = 0; i < groupDesc.FrameCount; i++)
                    {
                        if (groupDesc.AreaHeight > 0 && groupY + groupDesc.FrameHeight > groupDesc.AreaY + groupDesc.AreaHeight)
                            break;

                        var frame = new SpriteFrameDescription();
                        frame.Texture = groupDesc.Texture;
                        frame.X = groupX;
                        frame.Y = groupY;
                        frame.Width = groupDesc.FrameWidth;
                        frame.Height = groupDesc.FrameHeight;
                        frame.OriginX = groupDesc.OriginX;
                        frame.OriginY = groupDesc.OriginY;
                        frame.Duration = groupDesc.Duration;
                        animation.Frames.Add(new SpriteFrame(frame, groupTexture));

                        groupX += groupDesc.FrameWidth;
                        if (groupX + groupDesc.FrameWidth > groupDesc.AreaX + groupDesc.AreaWidth)
                        {
                            groupX = groupDesc.AreaX;
                            groupY += groupDesc.FrameHeight;
                        }
                    }
                }

                // Process the animation's frame data.
                foreach (var frameDesc in animdesc.Frames)
                {
                    var frame = new SpriteFrameDescription();
                    frame.OriginX = frameDesc.OriginX;
                    frame.OriginY = frameDesc.OriginY;
                    frame.Duration = frameDesc.Duration;

                    if (!String.IsNullOrWhiteSpace(frameDesc.Atlas))
                    {
                        var atlas = manager.Load<TextureAtlas>(frameDesc.Atlas);
                        if (!atlas.ContainsCell(frame.AtlasCell))
                            throw new InvalidDataException(UltravioletStrings.SpriteContainsInvalidAtlasCell.Format(frame.AtlasCell));

                        var cell = atlas[frame.AtlasCell];

                        frame.Atlas = frameDesc.Atlas;
                        frame.AtlasCell = frameDesc.AtlasCell;
                        frame.X = cell.X;
                        frame.Y = cell.Y;
                        frame.Width = cell.Width;
                        frame.Height = cell.Height;
                        animation.Frames.Add(new SpriteFrame(frame, atlas));
                    }
                    else
                    {
                        var texture = manager.Load<Texture2D>(frameDesc.Texture);

                        frame.Texture = frameDesc.Texture;
                        frame.X = frameDesc.X;
                        frame.Y = frameDesc.Y;
                        frame.Width = frameDesc.Width;
                        frame.Height = frameDesc.Height;
                        animation.Frames.Add(new SpriteFrame(frame, texture));
                    }
                }

                // Initialize the animation's default controller.
                animation.Controller.PlayAnimation(animation);
                animations.Add(animation);
            }
            return new Sprite(animations);
        }
    }
}
