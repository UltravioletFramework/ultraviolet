using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_DirNav_Cycle_VM
    {
        public void HandleViewOpening(DependencyObject dobj, RoutedEventData data)
        {
            btnL.Focus();
        }

        private readonly Button btnL = null;
    }
}
