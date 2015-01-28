
namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Initializes factory methods for the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed class PresentationFoundationFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UIViewFactory>((uv, uiPanelDefinition) => PresentationFoundationView.Load(uv, uiPanelDefinition));
        }
    }
}
