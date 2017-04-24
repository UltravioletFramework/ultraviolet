using Ultraviolet.Content;

namespace Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_TextBox_BindsCorrectlyToViewModel : TestScreenBase<UPF_TextBox_BindsCorrectlyToViewModel_VM>
    {
        public UPF_TextBox_BindsCorrectlyToViewModel(ContentManager globalContent)
            : base("Content/UI/Screens/UPF_TextBox_BindsCorrectlyToViewModel", "View", globalContent)
        {

        }
    }
}
