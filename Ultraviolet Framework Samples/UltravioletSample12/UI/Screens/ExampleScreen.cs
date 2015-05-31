using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace UltravioletSample.UI.Screens
{
    public class ExampleScreen : UIScreen
    {
        public ExampleScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/ExampleScreen", "ExampleScreen", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");
        }

        protected override void OnViewLoaded()
        {
            View.SetViewModel(new ExampleViewModel(Ultraviolet));

            base.OnViewLoaded();
        }
    }
}
