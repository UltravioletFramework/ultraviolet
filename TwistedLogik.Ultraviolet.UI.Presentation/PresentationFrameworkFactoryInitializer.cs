
namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Initializes factory methods for the Ultraviolet Presentation Framework.
    /// </summary>
    public sealed class PresentationFrameworkFactoryInitializer : IUltravioletFactoryInitializer
    {
        /// <inheritdoc/>
        public void Initialize(UltravioletContext owner, UltravioletFactory factory)
        {
            factory.SetFactoryMethod<UIViewFactory>((uv, uiPanelDefinition) => PresentationFrameworkView.Load(uv, uiPanelDefinition));
        }
    }
}
