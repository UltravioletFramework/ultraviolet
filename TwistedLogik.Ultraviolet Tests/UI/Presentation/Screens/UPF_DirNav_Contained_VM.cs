using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
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
