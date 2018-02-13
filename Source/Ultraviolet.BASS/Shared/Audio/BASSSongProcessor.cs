using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Loads song assets.
    /// </summary>
    [Preserve(AllMembers = true)]
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
