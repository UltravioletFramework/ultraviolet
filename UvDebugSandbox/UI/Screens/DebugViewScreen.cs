using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
using TwistedLogik.Ultraviolet.UI;

namespace UvDebugSandbox.UI.Screens
{
    public class DebugViewModel
    {
        public DebugViewModel(PresentationFoundationView view)
        {
            this.view = view;
        }

        private PresentationFoundationView view;

        public Double Foo
        {
            get { return foo; }
            set { foo = value; }
        }
        private Double foo;

        public void TestClick(UIElement element)
        {
            var a = view.GetElementByID("A");
            a.Classes.Toggle("red");
            a.Classes.Toggle("blue");
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
                View.SetViewModel(new DebugViewModel((PresentationFoundationView)View));
            }
            base.OnViewLoaded();
        }
    }
}
