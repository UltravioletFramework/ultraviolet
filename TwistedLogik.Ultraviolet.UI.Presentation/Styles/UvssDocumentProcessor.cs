using System;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    /// <summary>
    /// Represents a content processor for the *.uvss file type.
    /// </summary>
    [ContentProcessor]
    public class UvssDocumentProcessor : ContentProcessor<String, UvssDocument>
    {
        /// <inheritdoc/>
        public override UvssDocument Process(ContentManager manager, IContentProcessorMetadata metadata, String input)
        {
            return UvssDocument.Parse(input);
        }
    }
}
