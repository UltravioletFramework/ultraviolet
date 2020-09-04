using System;
using Ultraviolet.Audio;
using Ultraviolet.Content;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Loads song assets.
    /// </summary>
    [ContentProcessor]
    public sealed class BASSSongProcessor : ContentProcessor<BASSMediaDescription, Song>
    {
        /// <inheritdoc/>
        public override Song Process(ContentManager manager, IContentProcessorMetadata metadata, BASSMediaDescription input)
        {
            if (!input.IsFilename)
                throw new NotSupportedException();

            return new BASSSong(manager.Ultraviolet, (String)input.Data);
        }
    }
}
