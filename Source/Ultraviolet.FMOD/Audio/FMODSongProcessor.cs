using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Loads song assets.
    /// </summary>
    [ContentProcessor]
    public sealed class FMODSongProcessor : ContentProcessor<FMODMediaDescription, Song>
    {
        /// <inheritdoc/>
        public override Song Process(ContentManager manager, IContentProcessorMetadata metadata, FMODMediaDescription input)
        {
            if (!input.IsFilename)
                throw new NotSupportedException();

            return new FMODSong(manager.Ultraviolet, (String)input.Data);
        }
    }
}
