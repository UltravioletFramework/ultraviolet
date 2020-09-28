using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_VisualClone_DrawsCorrectly : TestScreenBase<UPF_VisualClone_DrawsCorrectly_VM>
    {
        public UPF_VisualClone_DrawsCorrectly(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_VisualClone_DrawsCorrectly", "View", globalContent)
        {

        }
    }
}
