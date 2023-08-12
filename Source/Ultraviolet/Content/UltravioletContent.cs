using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the core implementation of the Ultraviolet content subsystem.
    /// </summary>
    public sealed class UltravioletContent : UltravioletResource, IUltravioletContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletContent"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UltravioletContent(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public ContentManifestRegistry Manifests => manifests;

        /// <inheritdoc/>
        public ContentImporterRegistry Importers => importers;

        /// <inheritdoc/>
        public ContentProcessorRegistry Processors => processors;

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);
        
        // Registered content importers and processors.
        private readonly ContentManifestRegistry manifests = new ContentManifestRegistry();
        private readonly ContentImporterRegistry importers = new ContentImporterRegistry();
        private readonly ContentProcessorRegistry processors = new ContentProcessorRegistry();
    }
}
