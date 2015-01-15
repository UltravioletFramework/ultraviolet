using System.IO;
using TwistedLogik.Ultraviolet.UI.Presentation.Elements;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;
using System;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Animations;

namespace SafeProjectName.UI.Screens
{
    public class DebugViewModel
    {

    }

    public class DebugViewScreen : UIScreen
    {
        public DebugViewScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/DebugViewScreen", "DebugViewScreen", globalContent)
        {
            Contract.Require(uiScreenService, "uiScreenService");

            this.uiScreenService = uiScreenService;
        }

        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new DebugViewModel());
            }
            base.OnViewLoaded();
        }

        protected override void OnDrawingBackground(TwistedLogik.Ultraviolet.UltravioletTime time, TwistedLogik.Ultraviolet.Graphics.Graphics2D.SpriteBatch spriteBatch)
        {
            base.OnDrawingBackground(time, spriteBatch);
        }

        private readonly UIScreenService uiScreenService;
    }
}
