using Ultraviolet.Content;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's content management subsystem.
    /// </summary>
    public interface IUltravioletContent : IUltravioletSubsystem
    {
        /// <summary>
        /// Gets the content manifest registry.
        /// </summary>
        ContentManifestRegistry Manifests
        {
            get;
        }

        /// <summary>
        /// Gets the content importer registry.
        /// </summary>
        ContentImporterRegistry Importers
        {
            get;
        }

        /// <summary>
        /// Gets the content processor registry.
        /// </summary>
        ContentProcessorRegistry Processors
        {
            get;
        }
    }
}
