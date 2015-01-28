using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Screens
{
    public class DebugViewModel
    {
        public void TestClick(UIElement element)
        {
            Console.WriteLine("click");
        }
    }

    public class DebugViewScreen : UvDebugScreen
    {
        public DebugViewScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/DebugViewScreen", "DebugViewScreen", globalContent, uiScreenService)
        {

        }

        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new DebugViewModel());
            }
            base.OnViewLoaded();
        }
    }
}
