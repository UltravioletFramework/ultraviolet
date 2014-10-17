using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Loads XML assets.
    /// </summary>
    [ContentProcessor]
    public sealed class XmlContentProcessor : ContentProcessor<XDocument, XDocument>
    {
        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public unsafe override XDocument Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            return input;
        }
    }
}
