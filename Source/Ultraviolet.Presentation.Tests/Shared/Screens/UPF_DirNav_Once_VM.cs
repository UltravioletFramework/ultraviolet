using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_DirNav_Once_VM
    {
        public void HandleViewOpening(DependencyObject dobj, RoutedEventData data)
        {
            btnL.Focus();
        }

        private readonly Button btnL = null;
    }
}
