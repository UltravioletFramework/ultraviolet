using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_KeyNav_Simple : TestScreenBase<UPF_KeyNav_Simple_VM>
    {
        public UPF_KeyNav_Simple(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_KeyNav_Simple", "View", globalContent)
        {

        }
    }
}
