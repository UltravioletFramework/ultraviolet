using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace UltravioletSample.UI.Screens
{
    public class ExampleScreen : UIScreen
    {
        public ExampleScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/ExampleScreen", "ExampleScreen", globalContent)
        {

        }

        protected override Object CreateViewModel(UIView view)
        {
            return new ExampleViewModel(Ultraviolet);
        }
    }
}
