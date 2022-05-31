using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup : TestScreenBase<UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup_VM>
    {
        public UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_Popup_LaidOutCorrectly_WithTransformedPlacementTarget_WhenNestedInsidePopup", "View", globalContent)
        {

        }
    }
}
