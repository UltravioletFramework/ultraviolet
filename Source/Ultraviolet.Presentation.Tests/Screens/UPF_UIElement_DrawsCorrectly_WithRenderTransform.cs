using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_UIElement_DrawsCorrectly_WithRenderTransform : TestScreenBase<UPF_UIElement_DrawsCorrectly_WithRenderTransform_VM>
    {
        public UPF_UIElement_DrawsCorrectly_WithRenderTransform(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_UIElement_DrawsCorrectly_WithRenderTransform", "View", globalContent)
        {

        }
    }
}
