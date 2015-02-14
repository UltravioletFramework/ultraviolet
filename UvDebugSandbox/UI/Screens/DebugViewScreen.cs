using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
using TwistedLogik.Ultraviolet.UI;

namespace UvDebugSandbox.UI.Screens
{
    public class SomeObject
    {
        public SomeObject()
        {
            Baz = 11111;
        }

        public Double Baz
        {
            get;
            set;
        }
    }

    public class DebugViewModel
    {
        public DebugViewModel()
        {
            FooBar = 34243;
            SObj = new SomeObject();
        }

        public SomeObject SObj
        {
            get;
            private set;
        }

        public Double FooBar
        {
            get;
            set;
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
