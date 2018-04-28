using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Loads sound effect assets.
    /// </summary>
    [ContentProcessor]
    public sealed class FMODSoundEffectProcessor : ContentProcessor<String, SoundEffect>
    {
        /// <inheritdoc/>
        public override SoundEffect Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new FMODSoundEffect(manager.Ultraviolet, input);
        }
    }
}
