using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Loads song assets.
    /// </summary>
    [ContentProcessor]
    public sealed class FMODSongProcessor : ContentProcessor<String, Song>
    {
        /// <inheritdoc/>
        public override Song Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new FMODSong(manager.Ultraviolet, input);
        }
    }
}
