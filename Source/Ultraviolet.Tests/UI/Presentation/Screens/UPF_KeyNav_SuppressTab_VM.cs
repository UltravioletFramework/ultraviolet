using Ultraviolet.Input;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_KeyNav_SuppressTab_VM
    {
        public void SuppressTab(DependencyObject dobj, KeyboardDevice device, Key key, ModifierKeys modifiers, RoutedEventData data)
        {
            data.Handled = true;
        }
    }
}
