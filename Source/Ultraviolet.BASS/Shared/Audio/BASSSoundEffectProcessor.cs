using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Loads sound effect assets.
    /// </summary>
    [ContentProcessor]
    public sealed class BASSSoundEffectProcessor : ContentProcessor<String, SoundEffect>
    {
        /// <inheritdoc/>
        public override SoundEffect Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new BASSSoundEffect(manager.Ultraviolet, input);
        }
    }
}
