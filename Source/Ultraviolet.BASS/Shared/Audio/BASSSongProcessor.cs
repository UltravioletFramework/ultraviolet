using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Loads song assets.
    /// </summary>
    [ContentProcessor]
    public sealed class BASSSongProcessor : ContentProcessor<String, Song>
    {
        /// <inheritdoc/>
        public override Song Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new BASSSong(manager.Ultraviolet, input);
        }
    }
}
