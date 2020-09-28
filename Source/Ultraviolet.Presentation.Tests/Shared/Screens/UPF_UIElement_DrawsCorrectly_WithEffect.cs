using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_UIElement_DrawsCorrectly_WithEffect : TestScreenBase<UPF_UIElement_DrawsCorrectly_WithEffect_VM>
    {
        public UPF_UIElement_DrawsCorrectly_WithEffect(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_UIElement_DrawsCorrectly_WithEffect", "View", globalContent)
        {

        }
    }
}
