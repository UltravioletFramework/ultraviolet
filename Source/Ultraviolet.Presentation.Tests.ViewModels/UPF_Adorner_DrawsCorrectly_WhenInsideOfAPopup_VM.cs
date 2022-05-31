using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Documents;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup_VM
    {
        public void HandleViewLoaded(DependencyObject dobj, RoutedEventData data)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(button);
            adornerLayer.Add(new ExampleBoxesAdorner(button));
        }

        private readonly Button button = null;
    }
}
