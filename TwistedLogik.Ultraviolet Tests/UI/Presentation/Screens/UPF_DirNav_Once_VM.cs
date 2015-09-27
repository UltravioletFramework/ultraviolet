using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_DirNav_Once_VM
    {
        public void HandleLoaded(DependencyObject dobj, ref RoutedEventData data)
        {
            btnL.Focus();
        }

        private readonly Button btnL = null;
    }
}
