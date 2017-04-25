using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Documents;
using Ultraviolet.Tests.UI.Presentation.Controls;

namespace Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_Adorner_DrawsCorrectly_VM
    {
        public void HandleViewLoaded(DependencyObject dobj, RoutedEventData data)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(button);
            adornerLayer.Add(new ExampleBoxesAdorner(button));
        }

        private readonly Button button = null;
    }
}
