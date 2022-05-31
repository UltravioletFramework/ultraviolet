using Ultraviolet.Content;
using Ultraviolet.Presentation.Tests.ViewModels;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public class UPF_DirNav_Contained : TestScreenBase<UPF_DirNav_Contained_VM>
    {
        public UPF_DirNav_Contained(ContentManager globalContent)
            : base("Resources/Content/UI/Screens/UPF_DirNav_Contained", "View", globalContent)
        {

        }
    }
}
