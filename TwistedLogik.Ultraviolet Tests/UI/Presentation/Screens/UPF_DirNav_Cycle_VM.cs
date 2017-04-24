using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Tests.UI.Presentation.Screens
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
