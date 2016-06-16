using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_KeyNav_SuppressTab_VM
    {
        public void SuppressTab(DependencyObject dobj, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            data.Handled = true;
        }
    }
}
