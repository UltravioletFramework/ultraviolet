using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which loads text assets.
    /// </summary>
    [ContentProcessor]
    public sealed class TextContentProcessor : ContentProcessor<String[], IEnumerable<String>>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The <see cref="ContentManager"/> with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public unsafe override IEnumerable<String> Process(ContentManager manager, IContentProcessorMetadata metadata, String[] input)
        {
            return input;
        }
    }
}
