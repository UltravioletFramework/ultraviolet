using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content processor which loads sprites.
    /// </summary>
    internal sealed class SpriteProcessor : ContentProcessor<SpriteDescription, Sprite>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, SpriteDescription input, Boolean delete)
        {
            var animations = input.Animations?.SelectMany(x => x.Items)?.ToList();
            if (animations != null)
            {
                writer.Write(animations.Count);

                foreach (var animation in animations)
                {
                    writer.Write(animation.Name ?? String.Empty);
                    writer.Write(animation.Repeat ?? String.Empty);

                    var groups = animation.FrameGroups?.SelectMany(x => x.Items)?.ToList();
                    if (groups != null)
                    {
                        writer.Write(groups.Count);

                        foreach (var group in groups)
                        {
                            writer.Write(group.Texture ?? String.Empty);
                            writer.Write(group.X ?? 0);
                            writer.Write(group.Y ?? 0);
                            writer.Write(group.Width ?? 0);
                            writer.Write(group.Height ?? 0);
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
                            writer.Write(frame.X ?? 0);
                            writer.Write(frame.Y ?? 0);
                            writer.Write(frame.Width ?? 0);
                            writer.Write(frame.Height ?? 0);
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
                        group.X = reader.ReadInt32();
                        group.Y = reader.ReadInt32();
                        group.Width = reader.ReadInt32();
                        group.Height = reader.ReadInt32();
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
                        frame.X = reader.ReadInt32();
                        frame.Y = reader.ReadInt32();
                        frame.Width = reader.ReadInt32();
                        frame.Height = reader.ReadInt32();
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
        public override Sprite Process(ContentManager manager, IContentProcessorMetadata metadata, SpriteDescription input)
        {
            return CreateSprite(manager, metadata, input);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

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
                    foreach (var groupBatch in animDesc.FrameGroups ?? Enumerable.Empty<SpriteFrameGroupBatchDescription>())
                    {
                        foreach (var groupDesc in groupBatch.Items ?? Enumerable.Empty<SpriteFrameGroupDescription>())
                        {
                            metadata.AddAssetDependency(groupDesc.Texture);

                            var groupTexture = manager.Load<Texture2D>(groupDesc.Texture, metadata.AssetDensity, true, metadata.IsLoadedFromSolution);

                            var groupFrameCount = groupDesc.FrameCount ?? 0;
                            var groupFrameWidth = groupDesc.FrameWidth ?? 0;
                            var groupFrameHeight = groupDesc.FrameHeight ?? 0;

                            var groupAreaX = groupDesc.X ?? 0;
                            var groupAreaY = groupDesc.Y ?? 0;
                            var groupAreaWidth = groupDesc.Width ?? 0;
                            var groupAreaHeight = groupDesc.Height ?? 0;

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
                                frame.X = groupX;
                                frame.Y = groupY;
                                frame.Width = groupFrameWidth;
                                frame.Height = groupFrameHeight;
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
                    foreach (var frameBatch in animDesc.Frames ?? Enumerable.Empty<SpriteFrameBatchDescription>())
                    {
                        foreach (var frameDesc in frameBatch.Items ?? Enumerable.Empty<SpriteFrameDescription>())
                        {
                            var frame = new SpriteFrameDescription();
                            frame.Origin = frameDesc.Origin ?? Point2.Zero;
                            frame.Duration = frameDesc.Duration ?? 0;

                            if (!String.IsNullOrWhiteSpace(frameDesc.Atlas))
                            {
                                metadata.AddAssetDependency(frameDesc.Atlas);

                                var atlas = manager.Load<TextureAtlas>(frameDesc.Atlas, metadata.AssetDensity, true, metadata.IsLoadedFromSolution);
                                if (!atlas.ContainsCell(frameDesc.AtlasCell))
                                    throw new InvalidDataException(UltravioletStrings.SpriteContainsInvalidAtlasCell.Format(frameDesc.AtlasCell));

                                var cell = atlas[frameDesc.AtlasCell];

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
                                metadata.AddAssetDependency(frameDesc.Texture);

                                var texture = manager.Load<Texture2D>(frameDesc.Texture, metadata.AssetDensity, true, metadata.IsLoadedFromSolution);

                                frame.Texture = frameDesc.Texture;
                                frame.X = frameDesc.X ?? 0;
                                frame.Y = frameDesc.Y ?? 0;
                                frame.Width = frameDesc.Width ?? 0;
                                frame.Height = frameDesc.Height ?? 0;
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
