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
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public override Song Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return new BASSSong(manager.Ultraviolet, input);
        }
    }
}
