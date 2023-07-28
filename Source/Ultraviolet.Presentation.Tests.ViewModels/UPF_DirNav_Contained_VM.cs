using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_DirNav_Contained_VM
    {
        public void HandleViewOpening(DependencyObject dobj, RoutedEventData data)
        {
            btn1.Focus();
        }

        private readonly Button btn1 = null;
    }
}
