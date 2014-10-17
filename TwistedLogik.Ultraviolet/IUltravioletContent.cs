using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's content management subsystem.
    /// </summary>
    public interface IUltravioletContent : IUltravioletSubsystem
    {
        /// <summary>
        /// Registers any content importers or processors defined in the Ultraviolet Core assembly or
        /// any assembly containing the implementation of one of the Ultraviolet context's subsystems.
        /// </summary>
        void RegisterImportersAndProcessors();

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
