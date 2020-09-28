using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_KeyNav_Contained : TestScreenBase<UPF_KeyNav_Contained_VM>
    {
        public UPF_KeyNav_Contained(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_KeyNav_Contained", "View", globalContent)
        {

        }
    }
}
