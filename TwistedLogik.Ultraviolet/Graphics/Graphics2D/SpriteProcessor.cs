using System;
using System.Collections.Generic;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content processor which loads sprites.
    /// </summary>
    [ContentProcessor]
    public sealed class SpriteProcessor : ContentProcessor<SpriteDescription, Sprite>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SpriteDescription input, Boolean delete)
        {
            Contract.Require(writer, "writer");
            Contract.Require(input, "obj");

            writer.Write((input.Animations == null) ? 0 : input.Animations.Length);
            if (input.Animations != null)
            {
                foreach (var animation in input.Animations)
                {
                    writer.Write(animation.Name ?? String.Empty);
                    writer.Write(animation.Repeat ?? String.Empty);

                    writer.Write((animation.FrameGroups == null) ? 0 : animation.FrameGroups.Length);
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

                    writer.Write((animation.Frames == null) ? 0 : animation.Frames.Length);
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
            var desc = new SpriteDescription();

            var animationCount = reader.ReadInt32();
            if (animationCount > 0)
            {
                desc.Animations = new SpriteAnimationDescription[animationCount];
                for (int i = 0; i < animationCount; i++)
                {
                    desc.Animations[i] = new SpriteAnimationDescription();
                    desc.Animations[i].Name = reader.ReadString();
                    desc.Animations[i].Repeat = reader.ReadString();

                    var groupCount = reader.ReadInt32();
                    desc.Animations[i].FrameGroups = new SpriteFrameGroupDescription[groupCount];
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
                        desc.Animations[i].FrameGroups[j] = group;
                    }

                    var frameCount = reader.ReadInt32();
                    desc.Animations[i].Frames = new SpriteFrameDescription[frameCount];
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
                        desc.Animations[i].Frames[j] = frame;
                    }
                }
            }

            return Process(manager, metadata, desc);
        }

        /// <inheritdoc/>
        public override Sprite Process(ContentManager manager, IContentProcessorMetadata metadata, SpriteDescription input)
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
                    for (int i = 0; i < groupDesc.FrameCount; i++)
                    {
                        if (groupDesc.AreaHeight > 0 && groupY + groupDesc.FrameHeight > groupDesc.AreaY + groupDesc.AreaHeight)
                            break;

                        var frame = new SpriteFrameDescription();
                        frame.Texture = groupDesc.Texture;
                        frame.TextureResource = manager.Load<Texture2D>(frame.Texture);
                        frame.X = groupX;
                        frame.Y = groupY;
                        frame.Width = groupDesc.FrameWidth;
                        frame.Height = groupDesc.FrameHeight;
                        frame.OriginX = groupDesc.OriginX;
                        frame.OriginY = groupDesc.OriginY;
                        frame.Duration = groupDesc.Duration;
                        animation.Frames.Add(new SpriteFrame(frame));

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
                    frame.Atlas = frameDesc.Atlas;
                    frame.AtlasCell = frameDesc.AtlasCell;
                    if (frame.Atlas != null)
                    {
                        var atlas = manager.Load<TextureAtlas>(frame.Atlas);
                        if (!atlas.ContainsCell(frame.AtlasCell))
                            throw new InvalidDataException(UltravioletStrings.SpriteContainsInvalidAtlasCell.Format(frame.AtlasCell));

                        var cell = atlas[frame.AtlasCell];

                        frame.TextureResource = atlas;
                        frame.X = cell.X;
                        frame.Y = cell.Y;
                        frame.Width = cell.Width;
                        frame.Height = cell.Height;
                    }
                    else
                    {
                        frame.Texture = frameDesc.Texture;
                        frame.TextureResource = manager.Load<Texture2D>(frame.Texture);
                        frame.X = frameDesc.X;
                        frame.Y = frameDesc.Y;
                        frame.Width = frameDesc.Width;
                        frame.Height = frameDesc.Height;
                    }
                    frame.OriginX = frameDesc.OriginX;
                    frame.OriginY = frameDesc.OriginY;
                    frame.Duration = frameDesc.Duration;
                    animation.Frames.Add(new SpriteFrame(frame));
                }

                // Initialize the animation's default controller.
                animation.Controller.PlayAnimation(animation);
                animations.Add(animation);
            }
            return new Sprite(animations);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
