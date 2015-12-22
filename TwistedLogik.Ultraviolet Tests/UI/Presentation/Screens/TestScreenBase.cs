using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
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
