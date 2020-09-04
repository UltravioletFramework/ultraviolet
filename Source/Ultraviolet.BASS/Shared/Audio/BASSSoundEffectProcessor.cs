using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Loads sound effect assets.
    /// </summary>
    [ContentProcessor]
    public sealed class BASSSoundEffectProcessor : ContentProcessor<BASSMediaDescription, SoundEffect>
    {
        /// <inheritdoc/>
        public override SoundEffect Process(ContentManager manager, IContentProcessorMetadata metadata, BASSMediaDescription input)
        {
            return input.IsFilename ?
                new BASSSoundEffect(manager.Ultraviolet, (String)input.Data) :
                new BASSSoundEffect(manager.Ultraviolet, (Byte[])input.Data);
        }
    }
}
