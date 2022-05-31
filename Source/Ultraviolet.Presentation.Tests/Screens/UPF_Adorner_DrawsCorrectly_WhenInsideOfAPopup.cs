using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup : TestScreenBase<UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup_VM>
    {
        public UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_Adorner_DrawsCorrectly_WhenInsideOfAPopup", "View", globalContent)
        {

        }
    }
}
