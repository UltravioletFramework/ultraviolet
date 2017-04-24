using System;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor.
    /// </summary>
    public interface IContentProcessor
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        /// <param name="delete">A value indicating whether the original file will be deleted after preprocessing is complete.</param>
        void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, Object input, Boolean delete);

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        Object ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader);

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        Object Process(ContentManager manager, IContentProcessorMetadata metadata, Object input);

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        Boolean SupportsPreprocessing { get; }
    }
}
