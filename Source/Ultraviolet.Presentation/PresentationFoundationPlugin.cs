using Ultraviolet.Presentation.Styles;
using Ultraviolet.UI;

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
        public override void Configure(UltravioletContext uv, UltravioletFactory factory)
        {
            {
                factory.SetFactoryMethod<UIViewProviderInitializerFactory>(() => new PresentationFoundationInitializer());
                factory.SetFactoryMethod<UIViewFactory>((uv, uiPanel, uiPanelDefinition, vmfactory) => PresentationFoundationView.Load(uv, uiPanel, uiPanelDefinition, vmfactory));
                factory.SetFactoryMethod<MessageBoxScreenFactory>((mb, mbowner) => new MessageBoxScreen(mb, mbowner.GlobalContent));
            }
            base.Initialize(uv, factory);
        }

        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, UltravioletFactory factory)
        {
            var content = uv.GetContent();
            {
                content.Importers.RegisterImporter<UvssDocumentImporter>(".uvss");

                content.Processors.RegisterProcessor<UvssDocumentProcessor>();
            }
            base.Initialize(uv, factory);
        }

        /// <inheritdoc/>
        public override void Register(UltravioletConfiguration configuration) =>
            PresentationFoundation.Configure(configuration, presentationConfig);

        // UPF configuration settings.
        private readonly PresentationFoundationConfiguration presentationConfig;
    }
}
