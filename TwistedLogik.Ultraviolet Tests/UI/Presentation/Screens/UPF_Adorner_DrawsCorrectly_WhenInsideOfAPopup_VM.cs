using TwistedLogik.Ultraviolet.Tests.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Documents;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup_VM
    {
        public void HandleViewLoaded(DependencyObject dobj, ref RoutedEventData data)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(button);
            adornerLayer.Add(new ExampleBoxesAdorner(button));
        }

        private readonly Button button = null;
    }
}
