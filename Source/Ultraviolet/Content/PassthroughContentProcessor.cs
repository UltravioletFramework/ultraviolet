using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor which passes its input through unchanged.
    /// </summary>
    [ContentProcessor]
    internal sealed class PassthroughContentProcessor : ContentProcessor<Object, Object>
    {
        /// <inheritdoc/>
        public override Object Process(ContentManager manager, IContentProcessorMetadata metadata, Object input) => input;

        /// <summary>
        /// Gets the singleton instance of the <see cref="PassthroughContentProcessor"/> class.
        /// </summary>
        public static PassthroughContentProcessor Instance { get; } = new PassthroughContentProcessor();
    }
}
