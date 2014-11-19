using System;
using System.IO;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// <para>Represents a content processor.</para>
    /// <para>Content processors take the data structures created by content importers and transform them into game assets.</para>
    /// </summary>
    /// <typeparam name="Input">The type of the intermediate object which serves as the content processor's input.</typeparam>
    /// <typeparam name="Output">The type of content asset which is produced by the content processor.</typeparam>
    public abstract class ContentProcessor<Input, Output> : IContentProcessor
    {
        /// <inheritdoc/>
        void IContentProcessor.ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, Object input, Boolean delete)
        {
            ExportPreprocessed(manager, metadata, writer, (Input)input, delete);
        }

        /// <inheritdoc/>
        Object IContentProcessor.ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            return ImportPreprocessed(manager, metadata, reader);
        }

        /// <inheritdoc/>
        Object IContentProcessor.Process(ContentManager manager, IContentProcessorMetadata metadata, Object input)
        {
            return Process(manager, metadata, (Input)input);
        }

        /// <inheritdoc/>
        public virtual void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, Input obj, Boolean delete)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public virtual Output ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public abstract Output Process(ContentManager manager, IContentProcessorMetadata metadata, Input input);

        /// <inheritdoc/>
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
