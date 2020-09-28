using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_VisualClone_DrawsCorrectly_WithTransformedParent : TestScreenBase<UPF_VisualClone_DrawsCorrectly_WithTransformedParent_VM>
    {
        public UPF_VisualClone_DrawsCorrectly_WithTransformedParent(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_VisualClone_DrawsCorrectly_WithTransformedParent", "View", globalContent)
        {

        }
    }
}
