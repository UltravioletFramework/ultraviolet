namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a plugin for the Ultraviolet Framework which provides user interface views using the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationPlugin : UltravioletPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationPlugin"/> class.
        /// </summary>
        /// <param name="presentationConfig">Configuration settings for the Ultraviolet Presentation Foundation.</param>
        public PresentationFoundationPlugin(PresentationFoundationConfiguration presentationConfig = null)
        {
            this.presentationConfig = presentationConfig;
        }

        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration) =>
            PresentationFoundation.Configure(configuration, presentationConfig);

        // UPF configuration settings.
        private readonly PresentationFoundationConfiguration presentationConfig;
    }
}
