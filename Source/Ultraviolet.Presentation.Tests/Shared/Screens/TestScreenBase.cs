using System;
using Ultraviolet.Content;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation.Tests.Screens
{
    public abstract class TestScreenBase<TViewModel> : UIScreen
        where TViewModel : class, new()
    {
        public TestScreenBase(String rootDirectory, String definitionAsset, ContentManager globalContent)
            : base(rootDirectory, definitionAsset, globalContent)
        {

        }

        protected override Object CreateViewModel(UIView view)
        {
            return new TViewModel();
        }
    }
}
