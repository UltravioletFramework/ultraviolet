using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a content processor which loads sprites from JSON files.
    /// </summary>
    [ContentProcessor]
    internal sealed class SpriteProcessorFromJObject : ContentProcessor<JObject, Sprite>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, JObject input, Boolean delete) =>
            implProcessor.ExportPreprocessed(manager, metadata, writer, CreateSpriteDescription(manager, metadata, input), delete);

        /// <inheritdoc/>
        public override Sprite ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader) =>
            implProcessor.ImportPreprocessed(manager, metadata, reader);

        /// <inheritdoc/>
        public override Sprite Process(ContentManager manager, IContentProcessorMetadata metadata, JObject input) =>
            implProcessor.Process(manager, metadata, CreateSpriteDescription(manager, metadata, input));
        
        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => implProcessor.SupportsPreprocessing;

        /// <summary>
        /// Creates a sprite description from the specified input file.
        /// </summary>
        private static SpriteDescription CreateSpriteDescription(ContentManager manager, IContentProcessorMetadata metadata, JObject input)
        {
            var serializer = JsonSerializer.CreateDefault(UltravioletJsonSerializerSettings.Instance);
            var spriteDesc = input.ToObject<SpriteDescription>();
            var spriteFrameDefaults = spriteDesc.FrameDefaults;
            var spriteFrameGroupDefaults = spriteDesc.FrameGroupDefaults;

            foreach (var animationBatch in spriteDesc.Animations ?? Enumerable.Empty<SpriteAnimationBatchDescription>())
            {
                var animFrameDefaults = animationBatch.FrameDefaults;
                var animFrameGroupDefaults = animationBatch.FrameGroupDefaults;

                foreach (var animation in animationBatch.Items ?? Enumerable.Empty<SpriteAnimationDescription>())
                {
                    foreach (var frameGroupBatch in animation.FrameGroups ?? Enumerable.Empty<SpriteFrameGroupBatchDescription>())
                    {
                        var frameGroupDefaults = frameGroupBatch.FrameGroupDefaults;

                        foreach (var frameGroup in frameGroupBatch.Items ?? Enumerable.Empty<SpriteFrameGroupDescription>())
                        {
                            frameGroup.Texture = ResolveDependencyAssetPath(metadata, frameGroup.Texture ??
                                frameGroupDefaults?.Texture ?? animFrameGroupDefaults?.Texture ?? spriteFrameGroupDefaults?.Texture);

                            frameGroup.X = frameGroup.X ??
                                frameGroupDefaults?.X ?? animFrameGroupDefaults?.X ?? spriteFrameGroupDefaults?.X ?? 0;

                            frameGroup.Y = frameGroup.Y ??
                                frameGroupDefaults?.Y ?? animFrameGroupDefaults?.Y ?? spriteFrameGroupDefaults?.Y ?? 0;

                            frameGroup.Width = frameGroup.Width ??
                                frameGroupDefaults?.Width ?? animFrameGroupDefaults?.Width ?? spriteFrameGroupDefaults?.Width ?? 0;

                            frameGroup.Height = frameGroup.Height ??
                                frameGroupDefaults?.Height ?? animFrameGroupDefaults?.Height ?? spriteFrameGroupDefaults?.Height ?? 0;

                            frameGroup.Origin = frameGroup.Origin ??
                                frameGroupDefaults?.Origin ?? animFrameGroupDefaults?.Origin ?? spriteFrameGroupDefaults?.Origin ?? Point2.Zero;

                            frameGroup.FrameCount = frameGroup.FrameCount ??
                                frameGroupDefaults?.FrameCount ?? animFrameGroupDefaults?.FrameCount ?? spriteFrameGroupDefaults.FrameCount ?? 0;

                            frameGroup.FrameWidth = frameGroup.FrameWidth ??
                                frameGroupDefaults?.FrameWidth ?? animFrameGroupDefaults?.FrameWidth ?? spriteFrameGroupDefaults.FrameWidth ?? 0;

                            frameGroup.FrameHeight = frameGroup.FrameHeight ??
                                frameGroupDefaults?.FrameHeight ?? animFrameGroupDefaults?.FrameHeight ?? spriteFrameGroupDefaults.FrameHeight ?? 0;

                            frameGroup.Duration = frameGroup.Duration ??
                                frameGroupDefaults?.Duration ?? animFrameGroupDefaults?.Duration ?? spriteFrameGroupDefaults.Duration ?? 0;
                        }
                    }

                    foreach (var frameBatch in animation.Frames ?? Enumerable.Empty<SpriteFrameBatchDescription>())
                    {
                        var frameDefaults = frameBatch.FrameDefaults;

                        foreach (var frame in frameBatch.Items ?? Enumerable.Empty<SpriteFrameDescription>())
                        {
                            frame.Atlas = ResolveDependencyAssetPath(metadata, frame.Atlas ??
                                frameDefaults?.Atlas ?? animFrameDefaults?.Atlas ?? spriteFrameDefaults?.Atlas);

                            frame.AtlasCell = frame.AtlasCell ??
                                frameDefaults?.AtlasCell ?? animFrameDefaults?.AtlasCell ?? spriteFrameDefaults?.AtlasCell;

                            frame.Texture = ResolveDependencyAssetPath(metadata, frame.Texture ??
                                frameDefaults?.Texture ?? animFrameDefaults?.Texture ?? spriteFrameDefaults?.Texture);

                            frame.X = frame.X ??
                                frameDefaults?.X ?? animFrameDefaults?.X ?? spriteFrameDefaults?.X ?? 0;

                            frame.Y = frame.Y ??
                                frameDefaults?.Y ?? animFrameDefaults?.Y ?? spriteFrameDefaults?.Y ?? 0;

                            frame.Width = frame.Width ??
                                frameDefaults?.Width ?? animFrameDefaults?.Width ?? spriteFrameDefaults?.Width ?? 0;

                            frame.Height = frame.Height ??
                                frameDefaults?.Height ?? animFrameDefaults?.Height ?? spriteFrameDefaults?.Height ?? 0;

                            frame.Origin = frame.Origin ??
                                frameDefaults?.Origin ?? animFrameDefaults?.Origin ?? spriteFrameDefaults?.Origin ?? Point2.Zero;

                            frame.Duration = frame.Duration ??
                                frameDefaults?.Duration ?? animFrameDefaults?.Duration ?? spriteFrameDefaults?.Duration ?? 0;
                        }
                    }
                }
            }

            return spriteDesc;
        }

        // The processor which implements the conversion from SpriteDescription -> Sprite.
        private readonly SpriteProcessor implProcessor = new SpriteProcessor();
    }
}
