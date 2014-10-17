using System;
using System.IO;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a content processor.
    /// Content processors take the data structures created by content importers and transform them into game assets.
    /// </summary>
    public abstract class ContentProcessor<Input, Output> : IContentProcessor
    {
        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="input">The asset to export to the stream.</param>
        void IContentProcessor.ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, Object input)
        {
            ExportPreprocessed(manager, metadata, writer, (Input)input);
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        Object IContentProcessor.ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            return ImportPreprocessed(manager, metadata, reader);
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        Object IContentProcessor.Process(ContentManager manager, IContentProcessorMetadata metadata, Object input)
        {
            return Process(manager, metadata, (Input)input);
        }

        /// <summary>
        /// Exports an asset to a preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="writer">A writer on the stream to which to export the asset.</param>
        /// <param name="obj">The asset to export to the stream.</param>
        public virtual void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, Input obj)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Imports an asset from the specified preprocessed binary stream.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="reader">A reader on the stream that contains the asset to import.</param>
        /// <returns>The asset that was imported from the stream.</returns>
        public virtual Output ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Processes the specified data structure into a game asset.
        /// </summary>
        /// <param name="manager">The content manager with which the asset is being processed.</param>
        /// <param name="metadata">The asset's metadata.</param>
        /// <param name="input">The input data structure to process.</param>
        /// <returns>The game asset that was created.</returns>
        public abstract Output Process(ContentManager manager, IContentProcessorMetadata metadata, Input input);

        /// <summary>
        /// Gets a value indicating whether the processor supports preprocessing assets.
        /// </summary>
        public virtual Boolean SupportsPreprocessing 
        {
            get { return false; } 
        }

        /// <summary>
        /// Resolves the asset path of the specified dependency.
        /// </summary>
        /// <param name="metadata">The content processor metadata.</param>
        /// <param name="dependency">The relative path of the dependency to resolve.</param>
        /// <returns>The asset path of the specified dependency.</returns>
        protected static String ResolveDependencyAssetPath(IContentProcessorMetadata metadata, String dependency)
        {
            Contract.Require(metadata, "metadata");

            if (dependency == null)
                return null;

            if (metadata.AssetPath == null)
                return dependency;

            return ContentManager.NormalizeAssetPath(Path.Combine(Path.GetDirectoryName(metadata.AssetPath), dependency));
        }
    }
}
